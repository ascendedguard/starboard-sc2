**Starboard - Starcraft 2 Scoreboard Display**

Starboard is a scoreboard, designed by Ascend, meant for professional Starcraft 2 broadcasting. Features player name, color, race, and score displays with a subbar showing optional stream/match information.

**Coding Style**

Main development is done with Visual Studio 2010 (Express should be supported due to no cross-language necessity). Documentation style is done compliant to StyleCop and overall style uses the Model-View-ViewModel (MVVM) constructs.

As-is, the application runs as a single executable, no installation required, and has a requirement of .NET 4.0 Client Profile, available as a free download from Microsoft.

** Changes **

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