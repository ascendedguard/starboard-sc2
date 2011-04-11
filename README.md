**Starboard - Starcraft 2 Scoreboard Display**

Starboard is a scoreboard, designed by Ascend, meant for professional Starcraft 2 broadcasting. Features player name, color, race, and score displays with a subbar showing optional stream/match information.

**Coding Style**

Main development is done with Visual Studio 2010 (Express should be supported due to no cross-language necessity). Documentation style is done compliant to StyleCop and overall style uses the Model-View-ViewModel (MVVM) constructs.

As-is, the application runs as a single executable, no installation required, and has a requirement of .NET 4.0 Client Profile, available as a free download from Microsoft.

** Hotkeys **

As of version 0.4, you can click on the scoreboard and press a hotkey to make an instant change. The following are the supported hotkeys:

* Ctrl+Hotkey = Player 1
* Alt+Hotkey = Player 2

* PTZR - Change race to Protoss, Terran, Zerg, Random
* 1-8 - Change player color to choices 1-8, as shown on the main setup.
* +/- - Increment/Decrement player's score by 1.

** Changes **

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