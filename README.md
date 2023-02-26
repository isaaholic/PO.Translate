<!--
Hey, thanks for using the awesome-readme-template template.  
If you have any enhancements, then fork this project and create a pull request 
or just open an issue with the label "enhancement".
Don't forget to give this project a star for additional support ;)
Maybe you can mention me or this repo in the acknowledgements too
-->
<div align="center">

  <h1>PO File Translator v1.1.1</h1>
  <img src="https://i.ibb.co/zXcd9Vp/Screenshot-124.png" alt="Screenshot-124" border="0">
  
  <p>
     best way to translate file with .po extension
  </p>
  
   
<h4>
    <a href="https://www.nuget.org/packages/isaaholic.POTranslate.Core/">Nuget</a>
  </h4>
</div>

<br />

<!-- Table of Contents -->
# :notebook_with_decorative_cover: Table of Contents

- [About the Project](#star2-about-the-project)
  * [Screenshots](#camera-screenshots)
  * [Tech Stack](#space_invader-tech-stack)
  * [Features](#dart-features)
  * [Environment Variables](#key-environment-variables)
- [Getting Started](#toolbox-getting-started)
  * [Prerequisites](#bangbang-prerequisites)
  * [Installation](#gear-installation)
- [Usage](#eyes-usage)
- [Contributing](#wave-contributing)

  

<!-- About the Project -->
## :star2: About the Project


<!-- Screenshots -->
### :camera: Screenshots

<div align="center"> 
  <img src="https://i.ibb.co/zXcd9Vp/Screenshot-124.png" alt="screenshot1" />
  <img src="https://i.ibb.co/yVVt2zt/Screenshot-125.png" alt="screenshot2" />
</div>


<!-- TechStack -->
### :space_invader: Tech Stack

<details>
  <summary>dll</summary>
  <ul>
    <li><a href="https://dotnet.microsoft.com/en-us/">C# (.net 6 version)</a></li>
  </ul>
</details>

<!-- Features -->
### :dart: Features

- Translate .po files from English to Azerbaijani


<!-- Env Variables -->
### :key: Environment Variables

To run this project, you will need to add the following environment variables
<a href="https://support.google.com/googleapi/answer/6158862?hl=en">Google Cloud API</a>

`APIKey` 

```csharp
Operation.CreateClientFromApiKey(APIKey);
```

<!-- Getting Started -->
## 	:toolbox: Getting Started

<!-- Prerequisites -->
### :bangbang: Prerequisites

This project uses .net 6 version

```bash
 https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-6.0.406-windows-x64-installer
```

<!-- Installation -->
### :gear: Installation

Install package with nuget

```bash
  dotnet add package isaaholic.POTranslate.Core --version 1.1.1
```


<!-- Usage -->
## :eyes: Usage

If you want to use it correctly, you must assign file name and path of file folder.


```csharp
using isaaholic.POTranslate.Core;
Operation.CreateClientFromApiKey(APIKey);
Operation.OpenFile(fileName, path);
Operation.Translate();
```


<!-- Contributing -->
## :wave: Contributing
just me yet.

<a href="https://github.com/isaaholic">
  <img width="50px" src="https://avatars.githubusercontent.com/u/55139635?v=4" />
</a>

Contributions are always welcome!
