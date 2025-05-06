# IdleTest：放置類數字邏輯遊戲

## ASP.NET WEB 應用程式 (.NET Framework) MVC

> 本專案為個人練習 ASP.NET MVC 架構與後端邏輯整合而開發。

## 專案連結
[前往 GitHub Repo](https://github.com/shenjiec/IdleTest)

## 專案簡介
這是一款結合「放置」與「數字邏輯」的網頁遊戲，玩家透過自動擊敗怪物獲得資源，升級屬性、進行轉生以提升資源獲取效率。遊戲具備資源累積、成長曲線、重生系統等常見 Idle 類型玩法，並透過 ASP.NET MVC 架構實作遊戲邏輯與前後端互動。

---

## 遊戲玩法特色

- 怪物自動擊殺
- 精華系統強化資源獲取
- 轉生機制可重置並強化成長效率
- 遊戲數據即時呈現在畫面上

---

## 技術架構與實作方式

### 使用技術

- 後端：ASP.NET MVC (.NET Framework 4.x) 
- 資料存取：Entity Framework + EDMX (`Gaming.edmx`)
- 前端：Razor View + HTML + CSS
- 狀態暫存：TempData（用於 Controller 間傳遞資料）

### 架構說明

- Model：定義遊戲中的核心資料結構，例如怪物、精華、轉生等。
- View：使用 Razor View 呈現即時資源狀態、怪物資訊與操作介面。
- Controller：負責處理遊戲邏輯（如攻擊、升級、轉生），並透過 TempData 傳遞暫存狀態。
- Service：封裝進階邏輯，如傷害計算、升級條件判斷、資源獲取等，提升程式可維護性與可讀性。

---

## 如何執行專案

1. 使用 Visual Studio 開啟 MyWebApp1.sln
2. 建置並執行專案（F5）
3. 開啟瀏覽器並進入首頁，創建帳號後，即可開始遊玩

> 本專案已串接資料庫，使用 Entity Framework（Gaming.edmx）作為資料存取層，部分狀態（如資源、升級、精華等）會儲存在資料庫中。部分暫時性的狀態（如操作過程中傳遞的變數）則透過 TempData 管理。

---

## 開發者

- GitHub: [shenjiec](https://github.com/shenjiec)
- 技術領域：C# / ASP.NET MVC / Web 前後端整合

---

## 授權
本專案為學習用途，歡迎參考與使用。
