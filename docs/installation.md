## Installation

You can install UMarkdown via Unity Package Manager by adding the following Git URL:
```csharp
https://github.com/helightdev/UMarkdown.git?path=src/UMarkdown/Assets/Plugins/UMarkdown
```

You can also add the package manually to the `manifest.json` file located in the `Packages` folder of your Unity
project:

``` { .json .focus hl_lines="3"}
{
  "dependencies": {
    "dev.helight.umarkdown": "https://github.com/helightdev/UMarkdown.git?path=src/UMarkdown/Assets/Plugins/UMarkdown"
  }
}
```