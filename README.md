<img src="https://i.ibb.co/PF3B9qn/logo.png">

# cnc_backend
Goal is to fully emulate Command and Conquer™ backend and have functioning client.

Codebase originate from <a href="https://github.com/Tratos/BF4Backend/tree/main">this repo</a>.
> Credit: Eisbaer, Warranty Voider, the1Domo

# Build
Open solution with VS2022 and compile build. Run `CNCEmu.exe`.
> .NET Framework 4.6.1 is required

# Learn
Want to contribute? There is a [Docs](https://github.com/Xevrac/cnc_backend/wiki) page where you can learn.

# Media
Latest updates can be followed on my <a href="https://www.youtube.com/playlist?list=PLfYG_Q01lhem8qrQB7T5HWXg18_CR5noX">YouTube channel</a>.

Latest video: <a href="https://youtu.be/E0tTsIs9xps">PoC #6</a>

<img src="https://i.ibb.co/55c5B1M/Screenshot-2024-01-17-132344.png">

# Tracker

✅ - Done

🏗️ - Work in progress

❌ - Not Implemented

⛔ - No plans to implement / _Fork_ yourself to implement and merge

<hr>

* Blaze - 🏗️
  * Authentication Layer - ✅
     * Authenticate state is success, minor tweaking to packet handling may be required further testing needed.
  * Game Manager Layer - 🏗️
  * User Profile Unlocks - ❌
  * User Profile Stats - ❌
    
* Web - 🏗️
  * **Temporary**: External webserver - ✅
  * Internal webserver - ❌
  * ShellUI - 🏗️

* Client State - 🏗️
  > * No playable functions yet
  > * Basic menu for development purposes
  * Landing Page - ❌
  * Development Page - 🏗️
  * Game Creation - 🏗️
  * Level Generation - ❌
  * Player presence - ✅
  * Can load in game - ❌
