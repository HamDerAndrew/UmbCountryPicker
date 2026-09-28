# UmbCountryPicker

A friendly **country picker property editor** for Umbraco CMS.
The perfect dropdown for selecting countries in your Content Editor interface!

---

## Umbraco compatibility

| Umbraco CMS | Package version | Notes                                        |
| ----------- | --------------- | -------------------------------------------- |
| 17 (LTS)    | 17.x            | .NET 10, new backoffice (Web Components/Lit) |
| 13 (LTS)    | 13.x            | .NET 8, AngularJS backoffice                 |

Existing content survives the upgrade: when a site moves from Umbraco 13 to 17, the Data Type keeps
its `UmbCountryPicker` editor UI alias and its stored ISO codes are unchanged.

---

## Why Use UmbCountryPicker?

* **Intuitive UI**: Provides a simple dropdown that lists countries for content editors.
* **Smooth Integration**: Plug it right into your .NET-powered Umbraco site with minimal fuss.
* **Backoffice Localization**: Country names will be displayed based on the user’s Backoffice language setting.

---

## Features at a Glance

| Feature                       | Description                                            |
| ----------------------------- | ------------------------------------------------------ |
| Dropdown UI                   | Clean, user-friendly country selection in the editor.  |
| .NET Integration              | Register it, use it, done!                              |
| Auto-Translated Country Names | Languages adjust based on Backoffice culture settings. |

---

## Getting Started

### Installation

1. Install the package: `dotnet add package UmbCountryPicker`
2. In your Umbraco backoffice, create a new Data Type.
3. Assign **UmbCountryPicker** as the editor.
4. Use this Data Type wherever you want your country dropdown functionality.

---

## Screenshots & Localization Examples
As an example let's say this is your template code(sprinkled with a bit of CSS):
![code-example](https://raw.githubusercontent.com/HamDerAndrew/UmbCountryPicker/main/screenshots/code-example.png)


#### Let's look closer at how the content editor will interact with this and see the output

* **Content Editor View**
![content](https://raw.githubusercontent.com/HamDerAndrew/UmbCountryPicker/main/screenshots/content.png)
* **Frontend Display**
![frontend](https://raw.githubusercontent.com/HamDerAndrew/UmbCountryPicker/main/screenshots/frontend.png)

* **Backoffice Localization**

  * Default (en-US)
  ![code-example](https://raw.githubusercontent.com/HamDerAndrew/UmbCountryPicker/main/screenshots/backoffice-language-default-open.png)
  * Danish (da): Shows dropdown labeled in Danish.
  ![code-example](https://raw.githubusercontent.com/HamDerAndrew/UmbCountryPicker/main/screenshots/backoffice-language-danish-open.png)


---

## Under the Hood

UmbCountryPicker works by pulling in countries via ISO standards from its internally stored list and displays them in the appropriate culture. It includes:

* An `umbraco-package.json` manifest and a Lit-based web component (`client/src/country-picker.element.ts`, bundled to `wwwroot/country-picker.js`).
* A backoffice Management API endpoint at `/umbraco/management/api/v1/country-picker/countries` that serves the localized list.
* A value converter (e.g., `CountryPickerValueConverter.cs`) for Umbraco's ModelsBuilder for smooth retrieval of strongly typed country data in templates or views.

### Building from source

The C# project and the client bundle build separately:

```bash
cd client
npm ci --legacy-peer-deps
npm run build          # outputs to ../wwwroot
cd ..
dotnet build -c Release
```

`npm run watch` rebuilds the bundle on change while developing.

---

## License

Licensed under **MIT**, so feel free to use it in personal or commercial projects with peace of mind.

---

## Final Thoughts

UmbCountryPicker keeps things simple, localized, and developer-friendly. It’s almost plug‑and‑play: add to your Data Types, use it in your Document Types, and you're good to go + plus you're freed from managing country lists or translations yourself.
