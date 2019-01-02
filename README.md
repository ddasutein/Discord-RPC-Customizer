# C# Discord RPC Customizer

Add a custom Rich Presence status on Discord using this C# app.

## How do I use it?

### 1 - Download solution and build.

Download this solution by cloning this repo then open "DiscordRPC.csproj" and then build the project. Run the DiscordRPC.exe which can be found in bin/debug/DiscordRPC.exe. Also before launching the app, copy the 'discord-rpc-w32.dll' from the /lib/ folder and paste it in /bin/debug directory where the .exe file is located.

![Main window](https://raw.githubusercontent.com/ddasutein/csharp-discord-rpc-demo/master/screenshots/window.PNG)

### 2 - Add it as a game in Discord

Open the settings panel in the Discord Desktop app, go to **Games**, click "**Add it!**" and select the app ("Discord RPC").

### 3 - Obtain a Client ID

Now you will need a Client ID. To obtain it just go to [the Discord developper applications panel](https://discordapp.com/developers/applications/me) and click "**New App**".
Give it a nice App name, click the "**Create app**" button, then the "**Enable Rich Presence**" button.

### 4 - Mess with it

Copy the **Client ID** at the top of the page and paste it in the **Client ID** field in the app.
Click "**Initialize**" then "**Update**" and you should see this below your username and in your Discord profile:

![Discord Profile](https://raw.githubusercontent.com/nostrenz/cshap-discord-rpc-demo/master/screenshots/profile.png)

Of couse you can change what's in each fields, press "**Update**" and you'll see it changed on Discord after a few seconds.
Also, hitting **RunCallbacks** should tells you about errors or disconnections if any.

## Known issues

* This version (1.12.1) does not run on any OS older than the Windows 10 (1803) April 2018 Update. Currently working on a solution to enable backwards compatibility for older versions, including Windows 8.1 and 7. 

## Credits

Icon made by [freepik](https://www.flaticon.com/authors/freepik) from www.flaticon.com 

* Official documentation: https://discordapp.com/developers/docs/rich-presence/how-to
* Official SDK repository: https://github.com/discordapp/discord-rpc
