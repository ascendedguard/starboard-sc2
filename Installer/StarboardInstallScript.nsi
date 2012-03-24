;Starboard Install Script
;Uses Modern UI in NSIS
;Written by Will Eddins
;Based off Modern UI Example code

;--------------------------------
;Include Modern UI

  !include "MUI2.nsh"

;--------------------------------
;General

  ;Name and file
  Name "Starboard"
  OutFile "StarboardSetup.exe"
  SetCompressor lzma
  RequestExecutionLevel user
  
  CRCCheck on

  ;Default installation folder
  !ifdef D
	InstallDir "${D}"
  !else
	InstallDir "$LOCALAPPDATA\Starboard\Application\"
  !endif
  
  ;Get installation folder from registry if available
  InstallDirRegKey HKCU "Software\Ascend\Starboard" "Install_Dir"
  
  VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName" "Starboard"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "CompanyName" "Will 'Ascend' Eddins"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalCopyright" "Copyright © 2012"
  VIAddVersionKey /LANG=${LANG_ENGLISH} "Comments" "Installation package for Starboard"
  VIProductVersion 1.5.1.0
  
  BrandingText "Starboard v1.5.1 Installation"
  
;--------------------------------
;Interface Configuration

  !define MUI_HEADERIMAGE
  !define MUI_HEADERIMAGE_BITMAP "logo.bmp"
  !define MUI_ABORTWARNING

;--------------------------------
;Pages

  !insertmacro MUI_PAGE_INSTFILES
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  
;--------------------------------
;Languages
 
  !insertmacro MUI_LANGUAGE "English"

;--------------------------------
;Installer Sections

Section "Prerequisites" Prerequisites

  DetailPrint "Installing Prerequisite Files..."

  SetOutPath "$TEMP"
  
  ; Check if .NET 4.0 Client Profile is already installed
  ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Client" Install
  IntOp $8 $0 & 1
  
  IntCmp $8 1 InstallDone
  
  File .\dotNetFx40_Client_setup.exe
  ExecWait '"$TEMP\dotNetFx40_Client_setup.exe" /showfinalerror /norestart' $0
  Delete "$TEMP\dotNetFx40_Client_setup.exe"
  
  IntCmp $0 1614 RequireReboot
  IntCmp $0 1641 RequireReboot
  IntCmp $0 3010 RequireReboot
  
  Goto InstallDone
  
  RequireReboot:
  ; .NET needs to reboot the system
  SetRebootFlag true
  
  InstallDone:
  ; Nothing left to do here.
  
SectionEnd

Section "Remove Previous Versions" RemovePreviousVersion

  DetailPrint "Uninstall previous version..."

  ; We don't want any left-over files. There's nothing important from previous versions.
  RMDir /r "$LOCALAPPDATA\Starboard\Application\"

SectionEnd

Section "Required Files" RequiredFiles
SectionIn 1 RO

  DetailPrint "Installation Application Files..."

  SetOutPath "$INSTDIR"
  
  File "..\starboard-sc2\bin\Release\noconn.png"
  File "..\starboard-sc2\bin\Release\nosignal.png"
  File "..\starboard-sc2\bin\Release\notloaded.png"
  File "..\starboard-sc2\bin\Release\starboard.xbs"
  File "..\starboard-sc2\bin\Release\starboard-sc2.exe"
  
  SetOutPath "$INSTDIR"
  
  ;Store installation folder
  WriteRegStr HKCU "Software\Ascend\Starboard" "Install_Dir" $INSTDIR
  
  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"
  
  ;Add to Add/Remove Programs
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Starboard" "DisplayName" "Starboard"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Starboard" "InstallLocation" '"$INSTDIR"'
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Starboard" "DisplayIcon" '"$INSTDIR\starboard-sc2.exe"'
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Starboard" "UninstallString" '"$INSTDIR\Uninstall.exe"'
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Starboard" "Publisher" "Ascend"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Starboard" "URLInfoAbout" "http://ascendtv.com/starboard"
  WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Starboard" "NoModify" 1
  WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Starboard" "NoRepair" 1
  
SectionEnd

Section "Desktop Shortcut" DesktopShort

  CreateShortCut "$DESKTOP\Starboard.lnk" "$INSTDIR\starboard-sc2.exe" "" "" "" "" "" "Starcraft 2 Scoreboard"

SectionEnd

Section "Start Menu Shortcut" StartShort

  CreateShortCut "$SMPROGRAMS\Starboard.lnk" "$INSTDIR\starboard-sc2.exe" "" "" "" "" "" "Starcraft 2 Scoreboard"

SectionEnd

;--------------------------------
;Descriptions

  ;Language strings
  LangString DESC_RequiredFiles ${LANG_ENGLISH} "Files required to install Starboard. This step is required."
  LangString DESC_DesktopShort ${LANG_ENGLISH} "Creates a shortcut on the desktop."
  LangString DESC_StartShort ${LANG_ENGLISH} "Creates a shortcut in the start menu."
  ;Assign language strings to sections
  !insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${RequiredFiles} $(DESC_RequiredFiles)
	!insertmacro MUI_DESCRIPTION_TEXT ${DesktopShort} $(DESC_DesktopShort)
	!insertmacro MUI_DESCRIPTION_TEXT ${StartShort} $(DESC_StartShort)
  !insertmacro MUI_FUNCTION_DESCRIPTION_END
 
;--------------------------------
;Uninstaller Section

Section "Uninstall"

  RMDir /r "$LOCALAPPDATA\Starboard\Application\"
  
  RMDir "$LOCALAPPDATA\Starboard\"
  
  DeleteRegKey HKCU "Software\Ascend\Starboard"
  DeleteRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\Starboard"
  
  Delete "$DESKTOP\Starboard.lnk"
  Delete "$SMPROGRAMS\Starboard.lnk"
  
SectionEnd