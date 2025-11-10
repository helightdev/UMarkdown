<h1 align="left">
    UMarkdown
    <img alt="GitHub Release" src="https://img.shields.io/github/v/release/helightdev/UMarkdown">
    <a href="https://umarkdown.helight.dev/">
        <img src="https://img.shields.io/badge/docs-umarkdown.helight.dev-007ec6.svg" alt="Docs">
    </a>
</h1>

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
    "dev.helight.umarkdown": "https://github.com/helightdev/UMarkdown.git?path=src/UMarkdown/Assets/Plugins/UMarkdown"
  }
}
```

## Usage

The MarkdownVisualElement(visible as MarkdownText in UXML) can be used to render Markdown content
directly in Unity UI Toolkit. Alternatively, you can programmatically convert Markdown text
into Unity rich text strings and VisualElements using the methods provided by `UnityMarkdown`.

For more details on both, please refer to the [usage documentation](https://umarkdown.helight.dev/usage/).

## License

UMarkdown is licensed under the MIT License. See the LICENSE file for more information.