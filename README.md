# RMStore 是練習 Logging 的 Demo 方案
從 [dahlsailrunner/aspnetcore-effective-logging](https://github.com/dahlsailrunner/aspnetcore-effective-logging) 調整而來。
使用者直接從 config 設定來驗證(tony/hello)，讓 Demo 方案著重在 Logging。
並透過學習 [Effective Logging in ASP.NET Core 課程](https://www.pluralsight.com/courses/asp-dotnet-core-effective-logging)將它整理到 Blog [有效地使用 ASP.NET Core Logging - 1](https://rainmakerho.github.io/2020/08/05/2020014/)之中，內容會分別對應到不同的 Branch 。

[Effective Logging in ASP.NET Core 課程](https://www.pluralsight.com/courses/asp-dotnet-core-effective-logging) 非常棒。

### 各專案說明
|專案|說明|
|---|---|
|RMStore.WebUI|ASP.NET Core Razor Page 專案，呈現畫面|
|RMStore.API|ASP.NET Core API 專案，負責取得資料|
|RMStore.Domain|RMStore參考使用的Domain專案|

### 環境
* Windows 10
* Visual Studio 2019
* ASP.NET Core
* EF In-Memory Database




