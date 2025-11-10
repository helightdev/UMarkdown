# UMarkdown

UMarkdown is a utility library for parsing and converting Markdown text
into Unity compatible rich text and UI elements.

UMarkdown uses the widely used
[Markdig](https://github.com/xoofx/markdig) library for parsing Markdown syntax as
well as [ColorCode](https://github.com/CommunityToolkit/ColorCode-Universal) for syntax
highlighting of code blocks (planned to be replaced in the future)

## Installation

You can install UMarkdown via Unity Package Manager by adding the following Git URL:
`https://github.com/helightdev/UMarkdown.git?path=src/UMarkdown/Assets/Plugins/UMarkdown`

You can also add the package manually to the `manifest.json` file located in the `Packages` folder of your Unity
project:

```json
{
  "dependencies": {
    "com.helight.umarkdown": "https://github.com/helightdev/UMarkdown.git?path=src/UMarkdown/Assets/Plugins/UMarkdown"
  }
}
```

## License

UMarkdown is licensed under the MIT License. See the LICENSE file for more information.