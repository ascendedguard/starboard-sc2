![Starboard](http://ascendtv.com/starboard/starboardBlackText.png)

Starboard is a scoreboard, designed by Ascend, meant for professional Starcraft 2 broadcasting. Features player name, color, race, and score displays with a subbar showing optional stream/match information.

**Screenshots**

![Screenshot](http://ascendtv.com/starboard/Scoreboard.png)

**Coding Style**

Main development is done with Visual Studio 2010 (Express should be supported due to no cross-language necessity). Documentation style is done compliant to StyleCop and overall style uses the Model-View-ViewModel (MVVM) constructs.

As-is, the application runs as a single executable, no installation required, and has a requirement of .NET 4.0 Client Profile, available as a free download from Microsoft.

**Hotkeys**

All hotkeys require you to click on the visible scoreboard before using:

* F1 - Change Player 1 Name
* F2 - Change Player 2 Name
* (Press enter/esc to end changing player name)

* Ctrl+Hotkey = Player 1
* Alt+Hotkey = Player 2

* PTZR - Change race to Protoss, Terran, Zerg, Random
* 1-8 - Change player color to choices 1-8, as shown on the main setup.
* +/- - Increment/Decrement player's score by 1.

**Changes**

v1.3.1:

* Fixed bug where subbar and announcements would disappear after entering/exiting settings
* Added additional networking commands for increment player scores and retriving player information.

v1.3:

* Added remote control support.
* Slightly cleaner interface, featuring a fancy header graphic.

v1.2:

* Settings are saved to the registry upon closing, including: Size, Position, and Transparency settings.

v1.1:

* Added button to swap player positions on the scoreboard.

v1.0:

* Moved toggle buttons outside of the expanders
* The scoreboard itself no longer displays in the task manager.
* Added F1/F2 hotkeys for changing player names.
* Shorter transition timers on subbar text
* Fixed bug with setting transparency causing incorrect window positioning.

v0.5.0:

* Fixed crash when using invalid timer values.
* Added the ability to enable transparency for the scoreboard.
* Added the ability to customize the opacity of the scoreboard.
* Scores cannot be less than 0.
* Added fade-in and fade-out effects for when transparency is enabled.

v0.4.2:

* Fixed the 1-click buttons not working properly.

v0.4.1:

* Fixed not being able to minimize.
* Added icon

v0.4:

* Added 1-click selections for race and player color
* You can now shrink the subbar and announcement sections
* Setup window will auto-size to contents.
* Added hotkeys for player race, color, and score.

v0.3.1:

* Fixed crash when deleting single subbar or announcement messages.

v0.3:

* User can change (or remove) announcement and subbar text.
* User can customize the number of messages.
* Announcement messages can now rotate similar to subbar text.

v0.2:

* Scoreboard colors now animate between states
* Added build number to options screen.
* Added the ability to add announcement text over the scoreboard
* Added the ability to resize the scoreboard using a slider.
* Added the ability to hide the subbar to center the score.

v0.1: 

* Initial Release