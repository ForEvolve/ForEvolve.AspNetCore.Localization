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
 - `English (en)`
 - `French (fr)`
 - `Hebrew (he)` thanks to [aboyaniv](https://github.com/aboyaniv)
 - `Portuguese (pt)` thanks to [Matheus Avi](https://github.com/spyker0) (Same as `pt-BR`, needs to be checked)
 - `Brazilian portuguese (pt-BR)` thanks to [Matheus Avi](https://github.com/spyker0)
 - `Spanish (es)` thanks to [Oswaldo Diaz](https://github.com/OswaldoDG)
 - `Norwegian (bokmål) (nb)` thanks to [Petter Hoel](https://github.com/petterhoel) (If you are using `nb-NO` it should default to `nb`)
 - `Norwegian (bokmål) (no)` thanks to [Petter Hoel](https://github.com/petterhoel) (Same as `nb`)
 - `Chinese (zh)` thanks to [Jay Skyworker](https://github.com/jayskyworker) (Same as `zh-TW`, needs to be checked)
 - `Chinese Traditional (zh-Hant)` thanks to [Jay Skyworker](https://github.com/jayskyworker) (Same as `zh-TW`, needs to be checked)
 - `Chinese Traditional, Taiwan (zh-TW)` thanks to [Jay Skyworker](https://github.com/jayskyworker)
 - `Polish (pl)` thanks to [Denis Pujdak](https://github.com/fairking)
 
## Supported attributes

- CompareAttribute
- EmailAddressAttribute
- RequiredAttribute
- CreditCardAttribute
- FileExtensionsAttribute
- MaxLengthAttribute
- MinLengthAttribute
- PhoneAttribute
- RangeAttribute
- RegularExpressionAttribute
- UrlAttribute
- StringLengthAttribute (see [StringLengthLocalizationValidationAttributeAdapter.cs](https://github.com/ForEvolve/ForEvolve.AspNetCore.Localization/blob/master/src/ForEvolve.AspNetCore.Localization/Adapters/StringLengthLocalizationValidationAttributeAdapter.cs))

See [ForEvolveMvcDefaultLocalizationAdapterOptions.cs](https://github.com/ForEvolve/ForEvolve.AspNetCore.Localization/blob/master/src/ForEvolve.AspNetCore.Localization/ForEvolveMvcDefaultLocalizationAdapterOptions.cs) for the list of supported attributes used by the `DefaultLocalizationValidationAttributeAdapter`.

You can also create and register your own adapters and attributes.

## How to use
To enable localization for everything, including data annotation, you need to:

1. Make sure your application is targetting `Asp.Net Core 2.0`
1. Add `ForEvolve.AspNetCore.Localization` NuGet package to your project (or the `ForEvolve` meta-package).
1. In `Startup.cs` add and configure dependencies (see below).

``` csharp
public void ConfigureServices(IServiceCollection services)
{
    // Localization & options
    services.AddForEvolveLocalization();

    // ...

    // MVC
    services
        .AddMvc()
        .AddForEvolveMvcLocalization();
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    // Localization
    app.UseForEvolveRequestLocalization();

    //...
}
```

As you can see, it took only 3 lines of code to enable localization.

### Code break down

`services.AddForEvolveLocalization();` add all necessary services to the DI container, including supported resources, resource path, etc. This also calls `services.AddLocalization(...)` for you, defining a default `ResourcesPath` to `"Resources"`. You can change the default (all defaults actually).

To configure the options, you can pass a second argument of type `Action<ForEvolveLocalizationOptions>` to the `services.AddForEvolveLocalization();` extension method. 

> To make it easy to use, I made sure that everything is configurable at a single place instead of spreading settings around. 

**Example 1:**

``` csharp
services
    .AddForEvolveLocalization(options => {
        options.ResourcesPath = "new/place/where/to/store/resources";
    });
```

**Example 2:**

``` csharp
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

---

The `IMvcBuilder.AddForEvolveMvcLocalization();` extension method register the `ILocalizationValidationMetadataProvider` (this does the validation attribute localization magic) as well as `AddViewLocalization()` and `AddDataAnnotationsLocalization()`.

You can opt-out by setting `options.MvcOptions.EnableViewLocalization` or `options.MvcOptions.EnableDataAnnotationsLocalization` to `false` (in the call to `services.AddForEvolveLocalization();`).

---

The `IApplicationBuilder.UseForEvolveRequestLocalization()` extension method calls `app.UseRequestLocalization()` with some options. Once again all parameters are updatable in the initial call to `services.AddForEvolveLocalization();`.

## How to contribute a translation
Since I only know French and English, I can't translate messages into more languages, so contributions are very welcome.

I built a small tool to help find the culture-neutral and culture-specifics `CultureInfo` about a language; **please make sure that your translation covers the culture-neutral `CultureInfo` before creating a culture-specific one**.

- [CultureInfo Browser](https://cultureinfobrowser.azurewebsites.net).

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

- First we need a `DataAnnotationSharedResource.fr.resx` file (already there).
- Then we could add `DataAnnotationSharedResource.fr-CA.resx`, `DataAnnotationSharedResource.fr-FR.resx`, etc.

## Error messages
I modified default error messages a little to make them more linear. Sometimes it was written `The field {0} ...` and sometimes it was `The {0} field ...`. I decided to normalize messages to `The {0} field ...`. 

*I am open to suggestion if you think this makes no sense. English is only my secondary language.*

Error messages source (if you want the original error messages): [corefx/src/System.ComponentModel.Annotations/src/Resources/Strings.resx](https://github.com/dotnet/corefx/blob/1a76f612ffa3e459aa11add147e71206e4005555/src/System.ComponentModel.Annotations/src/Resources/Strings.resx)

## The plan
Before looking to the future let's look at the history of the project.

### The history of the project
I created this project because I did not want to code something similar to this every single time I start a new Asp.Net Core application. I did not want to write an error message on every ValidationAttribute either (which seems to be the official solution).

To be honest, I was a little disappointed to see how hard it is to localize Asp.Net Core validation attributes. This should be trivial. 

*I don't want to criticize the design made by the team that built that without knowing, so I will assume there are some good reasons behind these design choices (technical or not).*

That said, the other parts of the localization pipeline of Asp.Net Core are pretty neat with `IStringLocalizer`, `IHtmlLocalizer` and `IViewLocalizer`.

### The future
I plan on using this library for multiple projects so it should evolve in the future.
If you have ideas, requests or find bugs, feel free to open issues or submit PRs.

If you want to contributes some code, other than translating error messages, feel free to contact me.

To conclude, I hope this is only the beginning of the project. 

*For example, I'd like, at some point, to extract the resources somewhere else, maybe use some other resource provider like a database or JSON files...*

# Change log

## 2.0.0

- Update `MetadataProvider` so `DataTypeAttribute` gets the translation; Fix #21 
- Add functional tests that are covering most scenarios, related to error messages; closing #1
- Add functional tests that are covering French translation; related to #5. This should ensure that further breaking changes in the Asp.Net Core repo would be detected automatically by the CI pipeline.

### Possible/Known issues

- User-specified `ErrorMessage` on `DataTypeAttribute` might (will most likely) get overridden by `ForEvolve.AspNetCore.Localization`.

## 1.3.0

- Add `Norwegian (bokmål) (nb)` and `Norwegian (bokmål) (no)`

## 1.2.0

- Add `Spanish (es)`

## 1.1.0

- Add `Portuguese (pt)` and `Brazilian portuguese (pt-BR)`

## 1.0.0

- Initial `French (fr)` and `English (en)`
- Contributed `Hebrew (he)`
