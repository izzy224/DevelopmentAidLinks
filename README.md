# DevelopmentAidLinks

This is a project made in WPF Core + Entity Framework, and packages like:
Autofac, CefSharp, MahApps, HtmlAgilityPack, FreeSpire and Prism.

The use for the program is to extract tender URLs from the DevelopmentAid site by date and pick the english ones.

Also, the language detector(based on trigrams) wasn't made by me (check the author in the code commentary UI - LanguageDetector), because I didn't want to use 
Google Translate API.

Looking back to it, AngleSharp would have been a better solution than HtmlAgilityPack, because of the ability to create asynchronous awaits until the scripts are executed, rather 
than awaiting a manual delay.
