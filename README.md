# ForEvolve.AspNetCore.Localization

[![VSTS Build status](https://forevolve.visualstudio.com/ForEvolve-Framework/_apis/build/status/ForEvolve.AspNetCore.Localization)](https://forevolve.visualstudio.com/ForEvolve-Framework/_build/latest?definitionId=37)

The [ForEvolve.AspNetCore.Localization](https://github.com/ForEvolve/ForEvolve.AspNetCore.Localization) package allows you to enable localization of Asp.Net Core 2.0 applications in a few line of code.

This is very useful for `ValidationAttributes` like `[Required]`. No need to specify any string or error message, `ForEvolve.AspNetCore.Localization` do it for you.

## NuGet

You can:

```cmd
Install-Package ForEvolve.AspNetCore.Localization
```

or

```cmd
dotnet add package ForEvolve.AspNetCore.Localization
```

or take a look at [https://www.nuget.org/packages/ForEvolve.AspNetCore.Localization/](https://www.nuget.org/packages/ForEvolve.AspNetCore.Localization/).

## Prerelease MyGet

For the pre-release packages, use the ForEvolve [NuGet V3 feed URL](https://www.myget.org/F/forevolve/api/v3/index.json) packages source. See the [Table of content](https://github.com/ForEvolve/Toc) project for more info.

## Supported languages:

-   `English (en)`
-   `French (fr)`
-   `Hebrew (he)` thanks to [aboyaniv](https://github.com/aboyaniv)
-   `Portuguese (pt)` thanks to [Matheus Avi](https://github.com/spyker0) (Same as `pt-BR`, needs to be checked)
-   `Brazilian portuguese (pt-BR)` thanks to [Matheus Avi](https://github.com/spyker0)
-   `Spanish (es)` thanks to [Oswaldo Diaz](https://github.com/OswaldoDG)
-   `Norwegian (bokmål) (nb)` thanks to [Petter Hoel](https://github.com/petterhoel) (If you are using `nb-NO` it should default to `nb`)
-   `Norwegian (bokmål) (no)` thanks to [Petter Hoel](https://github.com/petterhoel) (Same as `nb`)
-   `Chinese (zh)` thanks to [Jay Skyworker](https://github.com/jayskyworker) (Same as `zh-TW`, needs to be checked)
-   `Chinese Traditional (zh-Hant)` thanks to [Jay Skyworker](https://github.com/jayskyworker) (Same as `zh-TW`, needs to be checked)
-   `Chinese Traditional, Taiwan (zh-TW)` thanks to [Jay Skyworker](https://github.com/jayskyworker)
-   `Polish (pl)` thanks to [Denis Pujdak](https://github.com/fairking)

## Supported attributes

-   CompareAttribute
-   EmailAddressAttribute
-   RequiredAttribute
-   CreditCardAttribute
-   FileExtensionsAttribute
-   MaxLengthAttribute
-   MinLengthAttribute
-   PhoneAttribute
-   RangeAttribute
-   RegularExpressionAttribute
-   UrlAttribute
-   StringLengthAttribute (see [StringLengthLocalizationValidationAttributeAdapter.cs](https://github.com/ForEvolve/ForEvolve.AspNetCore.Localization/blob/master/src/ForEvolve.AspNetCore.Localization/Adapters/StringLengthLocalizationValidationAttributeAdapter.cs))

See [ForEvolveMvcDefaultLocalizationAdapterOptions.cs](https://github.com/ForEvolve/ForEvolve.AspNetCore.Localization/blob/master/src/ForEvolve.AspNetCore.Localization/ForEvolveMvcDefaultLocalizationAdapterOptions.cs) for the list of supported attributes used by the `DefaultLocalizationValidationAttributeAdapter`.

You can also create and register your own adapters and attributes.

## How to use

To enable localization for everything, including data annotation, you need to:

1. Make sure your application is targeting `Asp.Net Core 2.1+`
1. Add `ForEvolve.AspNetCore.Localization` NuGet package to your project (`dotnet add package ForEvolve.AspNetCore.Localization`).
1. In `Startup.cs` add and configure dependencies (see below).

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // MVC (2.1)
    services
        .AddMvc()
        .AddForEvolveLocalization();

    // MVC (3+)
    services
        .AddRazorPages() // or other part of MVC that returns an IMvcBuilder
        .AddForEvolveLocalization();
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    // Localization
    app.UseForEvolveRequestLocalization();

    //...
}
```

As you can see, it took only 2 lines of code to enable localization.

`IMvcBuilder.AddForEvolveLocalization();` adds all necessary services to the DI container, including supported resources, resource path, etc. This also calls `services.AddLocalization(...)` for you, defining a default `ResourcesPath` to `"Resources"`. It also registers the `ILocalizationValidationMetadataProvider` (this does the validation attribute localization magic) as well as `AddViewLocalization()` and `AddDataAnnotationsLocalization()`.

The `IApplicationBuilder.UseForEvolveRequestLocalization()` extension method calls `app.UseRequestLocalization()` with some options.

### Options

You can configure the default values through `ForEvolveLocalizationOptions`. To make it easy to use, everything is configurable at that single place.

---

# TODO UPDATE OPTIONS EXAMPLES

**Example 1:**

```csharp
services
    .AddForEvolveLocalization(options => {
        options.ResourcesPath = "new/place/where/to/store/resources";
    });
```

**Example 2:**

```csharp
services
    .AddForEvolveLocalization(options => {
        options.ResourcesPath = "new/place/where/to/store/resources";
        options.MvcOptions.EnableViewLocalization = false;
        options.MvcOptions.ConfigureValidationMetadataProvider = (provider) =>
        {
            provider.Adapters.Add(new SomeCoolAdapterThatICreatedOnlyForMyProject());
        };
    });
```

You can opt-out by setting `options.MvcOptions.EnableViewLocalization` or `options.MvcOptions.EnableDataAnnotationsLocalization` to `false` (in the call to `services.AddForEvolveLocalization();`).

# END TODO UPDATE OPTIONS EXAMPLES

---

## How to contribute a translation

Since I only know French and English, I can't translate messages into more languages, so contributions are very welcome.

I built a small tool to help find the culture-neutral and culture-specifics `CultureInfo` about a language; **please make sure that your translation covers the culture-neutral `CultureInfo` before creating a culture-specific one**.

-   [CultureInfo Browser](https://cultureinfobrowser.azurewebsites.net).

**How to submit a new translation:**

1. Fork the repo
1. Create a resource file for the language you want to translate error messages into.
1. Translate it (obviously)
1. Add the new language to the `supportedCultures` list in `StartupExtensions.cs`.
1. Add the new language to the `Supported languages` section of the `README.md` file with a "thanks to you" attribution and a link.
1. Open a pull request

Since I don't speak all languages, I cannot validate those that I don't know (except maybe by using Google Translate), so it's up to you to makes things right! (or PR corrections)

I will do my best to integrates PR as fast as possible.

### Where are the error messages located?

If you look under `src/ForEvolve.AspNetCore.Localization/Resources/`, you will find `DataAnnotationSharedResource.resx` and `DataAnnotationSharedResource.{lang}.resx` files.
You can copy any one of those and translate the values.

If you want to create a culture-specific translation, example: `fr-CA`, please make sure that there is an `fr` translation (neutral culture) first which will be the default for that language.

**Example:**

-   First we need a `DataAnnotationSharedResource.fr.resx` file (already there).
-   Then we could add `DataAnnotationSharedResource.fr-CA.resx`, `DataAnnotationSharedResource.fr-FR.resx`, etc.

## Error messages

I modified default error messages a little to make them more linear. Sometimes it was written `The field {0} ...` and sometimes it was `The {0} field ...`. I decided to normalize messages to `The {0} field ...`.

_I am open to suggestion if you think this makes no sense. English is only my secondary language._

Error messages source (if you want the original error messages): [corefx/src/System.ComponentModel.Annotations/src/Resources/Strings.resx](https://github.com/dotnet/corefx/blob/1a76f612ffa3e459aa11add147e71206e4005555/src/System.ComponentModel.Annotations/src/Resources/Strings.resx)

## The history of the project

I created this project because I did not want to code something similar to this every single time I start a new Asp.Net Core application. I did not want to write an error message on every ValidationAttribute either (which seems to be the official solution).

To be honest, I was a little disappointed to see how hard it is to localize Asp.Net Core validation attributes. This should be trivial.

_I don't want to criticize the design made by the team that built that without knowing, so I will assume there are some good reasons behind these design choices (technical or not)._

That said, the other parts of the localization pipeline of Asp.Net Core are pretty neat with `IStringLocalizer`, `IHtmlLocalizer` and `IViewLocalizer`.

## How to contribute?

If you have ideas, requests or find bugs, please open an issue.
If you want to contributes some code, other than translating error messages, please open an issue first so you don't waste your time.

For more information, please read [Contributing to ForEvolve open source projects](https://github.com/ForEvolve/ForEvolve.DependencyInjection/tree/master/CONTRIBUTING.md).

## Contributor Covenant Code of Conduct

Also, please read the [Contributor Covenant Code of Conduct](https://github.com/ForEvolve/ForEvolve.DependencyInjection/tree/master/CODE_OF_CONDUCT.md) that applies to all ForEvolve repositories.

# Change log

## 3.0.0

-   Remove the need to call `IServiceCollection.AddForEvolveLocalization()` (see #27)
-   Rename `IMvcBuilder.AddForEvolveMvcLocalization()` to `IMvcBuilder.AddForEvolveLocalization()`
-   Refactor `ForEvolveLocalizationOptions` to leverage the Asp.Net Core options patterns
-   Use `Nerdbank.GitVersioning` to manage versions automagically.
-   Move builds from Azure DevOps to GitHub Actions

## 2.2.0

-   Add `Polish (pl)`

## 2.1.0

-   Add `Chinese (zh)`
-   Add `Chinese (Traditional) (zh-Hant)`
-   Add `Chinese (Traditional, Taiwan) (zh-TW)`

## 2.0.0

-   Update `MetadataProvider` so `DataTypeAttribute` gets the translation; Fix #21
-   Add functional tests that are covering most scenarios, related to error messages; closing #1
-   Add functional tests that are covering French translation; related to #5. This should ensure that further breaking changes in the Asp.Net Core repo would be detected automatically by the CI pipeline.

### Possible/Known issues

-   User-specified `ErrorMessage` on `DataTypeAttribute` might (will most likely) get overridden by `ForEvolve.AspNetCore.Localization`.

## 1.3.0

-   Add `Norwegian (bokmål) (nb)` and `Norwegian (bokmål) (no)`

## 1.2.0

-   Add `Spanish (es)`

## 1.1.0

-   Add `Portuguese (pt)` and `Brazilian portuguese (pt-BR)`

## 1.0.0

-   Initial `French (fr)` and `English (en)`
-   Contributed `Hebrew (he)`
