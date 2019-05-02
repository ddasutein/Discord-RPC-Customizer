# C# Discord Rich Presence Customizer 

Want to have a custom Rich Presence on your Discord profile? This simple app can do that.

## Installation (for users)

### 1 - Go to the 'Releases tab'

### 2 - Look for the latest version and download the ZIP file

### 3 - Create a folder on your preferred save location and extract the ZIP content to the folder

### 4 - Run DiscordRPC.exe.

### 5 - Follow the tutorial on the first run screen OR scroll down on this README and skip to Step 2 under Installation (for developers) to configure the RPC client.

### 6 - Enjoy! :) Please do submit an issue on this repository if you encounter issues

## Installation (for developers)

**Dependencies:**

* Visual Studio 2017 or Visual Studio 2019

* .NET Framework 4.7.2 (*will be upgraded to .NET Core 3.0 in the future*)

* Newtonsoft

Within the Visual Studio solution, you will be presented with 3 project files

* `src/DiscordRPC.Core/Discord RPC.sln` contains the Discord RPC C# library by [Lachee/discord-rpc-csharp](https://github.com/Lachee/discord-rpc-csharp).

* `src/DiscordRPC.Main/DiscordRPC.Main.csproj` is the main starting point of the application, as well as the MainWindow.XAML file for the user interface. 

* `src/DiscordRPC.Legacy/DiscordRPC.Legacy.csproj` contains the old codebase from the original fork before moving the codebase to the new directory `DiscordRPC.Main`. This may be removed in the future.

### 1 - Download solution and build.

Download this solution by cloning this repo then open "DiscordRPC.csproj" and then build the project. Run the DiscordRPC.exe which can be found in bin/debug/DiscordRPC.exe. Also before launching the app, copy the 'discord-rpc-w32.dll' from the /lib/ folder and paste it in /bin/debug directory where the .exe file is located.

***Note:** As of Version 2.0.0, the application will no longer require the 'discord-rpc-w32' DLL file. If you are downloading an older release such as 1.13 or 1.12.1, you must have the DLL file in the same directory as the .exe file*

![Main window](https://raw.githubusercontent.com/ddasutein/Discord-RPC-csharp/master/screenshots/discord-rpc.png)

### 2 - Add it as a game in Discord

Open the settings panel in the Discord Desktop app, go to **Games**, click "**Add it!**" and select the app ("Discord RPC").

### 3 - Obtain a Client ID

Now you will need a Client ID. To obtain it just go to [the Discord developer applications panel](https://discordapp.com/developers/applications/me) and click "**New App**".
Give it a nice App name, click the "**Create app**" button, then the "**Enable Rich Presence**" button.

### 4 - Update your presence

Copy the **Client ID** at the top of the page and paste it in the **Client ID** field in the app.
Click "**Initialize**" then "**Update**" and you should see this below your username and in your Discord profile:

![Discord Profile](https://raw.githubusercontent.com/ddasutein/Discord-RPC-csharp/master/screenshots/discord-profile.png)

Of couse you can change what's in each fields, press "**Update**" and you'll see it changed on Discord after a few seconds.

Version 1.13 and older: Hitting **RunCallbacks** should tells you about errors or disconnections if any.

## Credits

Icon made by [freepik](https://www.flaticon.com/authors/freepik) from www.flaticon.com 

* Official documentation: https://discordapp.com/developers/docs/rich-presence/how-to
* Official SDK repository: https://github.com/discordapp/discord-rpc

All UI icons by [icon8.com]

