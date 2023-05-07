# ModSupport

Adds a 'Mods' menu to the main screen and pause menu with various tools to help troubleshoot mod-related problems.  
Allows the user to send error reports to the ModSupport server to help fix mod errors. The prompts for this report can appear on exit, or if the game "hangs", for example on endless loading screen.  
Compares the mods of the connected co-op partner and the host. For this functionality to work, both the host and the co-op partner needs this mod.

## Error reports
The error reporting functionality relies on the ModSupport server, which is hosted by me. This means it's not guaranteed that it'll always be up.  
The reports contain only the list of mods you are using, and the log (the contents of output_log.txt). Nothing else. The server does not keep your IP address or anything that can be used to personally identify you. 
I'm doing my best to remove potentially problematic data in the logs such as SteamID. Despite my best efforts though, there may be some data about your PC that Outward puts into the logs (Direct3D version, GPU, game's local path, etc.) so no guarantees.  
You are of course free to disable the error report feature, or use the 'Send only errors' option (which only sends program code related stuff, nothing else).  
Only modders are able to see these reports on the server.  
Reports that are older than a month will be automatically deleted.  

## Configuration
It is strongly recommended to use [Outward Config Manager](https://outward.thunderstore.io/package/Mefino/Outward_Config_Manager/).

### Basic configuration
- **Show alert when exiting**: Shows an alert on exit if there are errors, with the option to send report. Default on.
- **Show alert on endless loading screen**: Shows an alert if too many errors happen during loading. Default on.
- **Online features**: Enables online features, such as error reporting. Default on.
- **Send only errors**: Sends only errors instead of the entire log. It's a good idea to use this if your bandwidth is limited. Default off.

### Advanced configuration
- **Send reports without asking**: Sends reports to the ModReport server without asking, and does not display any popups on response (success or failure). Default off.
- **Show alert on errors**: Shows an alert every time an error happens, with the option to send report. This option is intended for debugging purposes, and you should not turn it on unless you want to be interrupted every time an error happens. Default off.
- **Errors advanced mode**: Shows the number of (actual) errors (instead of just exceptions). This feature is intended for developers and advanced users. Default off.

## Planned features
- Alert if the game fails to start
- External watchdog process to catch game crashes
- Capture more logs (possibly needs preload patcher)
- More information about mods in the 'Mods' menu (source, author, etc.)
- Match not just mods and versions but also configuration between the host and client in co-op
- Ability to sync mods with the host (download, install, etc.) while keeping the client's original mod setup intact
- Detect non-DE mods
- Warning if a mod is known to be not multiplayer compatible
- Show a list of mods that threw errors during the last play session
- Show the mod that is possibly causing endless loading screen

## Special thanks
- **Nielsjuh** for helping me test the multiplayer bits
- **Ravial** for suggesting and helping me with AWS
- **Emo** for Outward UI related support

## Changelog

### 1.1.1
- Supported languages: English, French, German, Italian, Spanish

### 1.1.0
 - Option to send only errors instead of the entire log
 - Aggregate similar log entries in the last 5 minutes (this is to prevent spam caused by endless loading screen)

### 1.0.0
- Initial release