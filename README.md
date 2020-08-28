# RMStore 是練習 Logging 的 Demo 方案
從 [dahlsailrunner/aspnetcore-effective-logging](https://github.com/dahlsailrunner/aspnetcore-effective-logging) 調整而來。
使用者直接從 config 驗證，讓 Demo 方案著重在 Logging。

### 各專案說明
|專案|說明|
|---|---|
|RMStore.WebUI|ASP.NET Core Razor Page 專案，呈現畫面|
|RMStore.API|ASP.NET Core API 專案，負責取得資料|
|RMStore.Domain|RMStore參考使用的Domain專案|
|RMStore.Infrastructure|RMStore參考使用的Middleware, Filter, Atrribute, BasePageMode 專案|

### 環境
* Windows 10
* Visual Studio 2019
* ASP.NET Core
* EF In-Memory Database

### performancelog
這個 Branch 是

1. 將共用的 Middleware 、 Filter 等 Class 抽到 RMStore.Infrastructure 專案，供 API & WebUI 使用。
2. 記錄 Razor Page & Action 的執行時間，並使用 LoggerMessage.Define 來減少參數的 boxing ，以增加效能。

切換請使用
```

git checkout performancelog

```
使用 Serilog 將 Log 寫到檔案之中，WebUI 與 API 均寫到同一個檔案之中。
檔案路徑在 appsettings.json 的 Serilog : WriteTo 區段之中




