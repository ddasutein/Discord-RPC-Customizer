<h1 align="center">
<sub>
<img  src="https://raw.githubusercontent.com/ddasutein/Discord-RPC-csharp/master/src/DiscordRPC.Main/Resources/RikoRPC_logo_128px.png"
      height="35"
      width="35">
</sub>
RikoRPC for Discord
</h1>
<p align="center">Create a custom Discord Rich Presence
<br>
Download now
<br>
<a href="https://github.com/ddasutein/Discord-RPC-csharp/releases">GitHub (Portable Version)</a>
| Microsoft Store (coming soon)
<h1></h1>
</p>

## Installation (Users)

1. Go to the 'Releases tab'

2. Get the latest version and download the ZIP file

3. Create a folder on your preferred save location and extract the ZIP content to the folder

4. Run RikoRPC.exe (Add to your Anti-Virus Exclusion List in case it flags this as malware)

5. Follow Tutorial Screen

6. Please do submit an issue on this repository if you encounter issues or want to suggest new features! Enjoy! 😊

## Installation (Developers)

**Dependencies:**

* Visual Studio 2017 or Visual Studio 2019

* .NET Framework 4.7.2 (*will be upgraded to .NET Core 3.0 in the future*)

* Newtonsoft

* DiscordRichPresence by [Lachee/discord-rpc-csharp](https://github.com/Lachee/discord-rpc-csharp)

* RestSharp

* (Not in this build yet) DesktopHelpers

* (Not in this build yet) Microsoft.Windows.SDK.Contracts

**Installation:**

1. Download solution and build.

    In the Visual Studio solution, you will be presented with 2 projects:
    
    - `src/DiscordRPC.Core/Discord RPC.sln` contains the Discord RPC C# library by [Lachee/discord-rpc-csharp](https://github.com/Lachee/discord-rpc-csharp). *Deprecated as of version 2.3.0. Replaced with Nuget Package.*

    - `src/DiscordRPC.Main/DiscordRPC.Main.csproj` is the main starting point of the application, as well as the MainWindow.XAML file for the user interface. 

2. Create a Discord Application

    Go to [the Discord developer applications panel](https://discordapp.com/developers/applications/me) and click create a new application. After creating your application, copy the Client ID.

3. Start and Update Discord Rich Presence

    Build application then copy the **Client ID** from the Discord Application Portal and Paste in Settings > **Client ID** field in the RikoRPC app. Click "**Go online**" to initialize Discord Presence.

    ![Main window](https://raw.githubusercontent.com/ddasutein/Discord-RPC-csharp/master/screenshots/discord-rpc.png)

    ![Discord Profile](https://raw.githubusercontent.com/ddasutein/Discord-RPC-csharp/master/screenshots/discord-profile.png)

## Privacy Policy

*Last updated: July 22, 2019*

All right, I know most people don't bother reading Terms of Service or Privacy Policy, so I'll keep this short and simple. Because the entire source code is open source, you can see everything for yourself.

**What data does this application collect/process?**

Please note that no information from this application is sent to any third-party services except Discord and GitHub.

1. Discord Profile and Application Data

    RikoRPC will use Discord's servers to send or pull the following data:

    - Username
    
    - User avatar

    - Discord Application API data (Client ID and Rich Presence Art Assets)

    - RikoRPC application data including; Client ID, Detail, State, and Image Keys

    - [Other data transmission subject to Discord's Privacy Policy](https://discordapp.com/privacy)    

2. Processing of running applications on user's machine

    RikoRPC will scan for processes, specifically looking for the Discord Client (Stable, PTB, or Canary) and Spotify Desktop client as part of the application's functionality. This processing is done locally on the user's machine and does not send them to any third-party.

3. GitHub

    RikoRPC will access GitHub to query for a new version of this application by comparing the current application version to the latest version on GitHub.

## Credits

User Interface

- Discord Wumpus Logo by [Discord](https://discordapp.com/branding)

- UI Icons by [icon8.com](https://icons8.com)

Source Code

- Original fork by [nostrenz/cshap-discord-rpc-demo](https://github.com/nostrenz/cshap-discord-rpc-demo)

- Official documentation: https://discordapp.com/developers/docs/rich-presence/how-to

- Official SDK repository: https://github.com/discordapp/discord-rpc


## MIT License

Copyright 2019 Dasutein

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

