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

### scope
這個 Branch 是

1. 使用 BeginScope 將共通的資訊記錄到 scope 之中
2. 將 Email Address Mask 後再 Log
3. 使用 Func<Exception, LogLevel> 將 LogLevel 交由使用系統來決定

切換請使用
```

git checkout scope

```
使用 Serilog 將 Log 寫到檔案之中，WebUI 與 API 均寫到同一個檔案之中。
檔案路徑在 appsettings.json 的 Serilog : WriteTo 區段之中




