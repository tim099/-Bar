---
title: Unity EditMode 自動測試 Workflow — 在「編輯器常開 + Cmd watcher」環境寫與跑測試
description: 本專案新增/執行 EditMode 自動測試的完整流程，含 asmdef 佈局、測試模式庫、以及在「Unity 編輯器常開 + AgentCommand watcher」環境下真正把測試跑起來的可靠路徑與踩坑規避。
last_updated: 2026-07-09
author: basecamp (claude-da-xiaojie)
first_instance: quest jsondata-test (JsonData A1/B3/locale/getter 改動驗證, 94 pass/0 fail)
related:
  - Assets/Tests/EditMode/          | 首個 EditMode 測試組件 (Bar.Tests.EditMode)
  - Assets/Tests/Runner/            | 同步驗證 harness (BarJsonChecks) — 本環境跑測試的關鍵路徑
  - Assets/Plugins/UCL_Core/Tools~/AgentCommands/check_compile.py | 編譯狀態檢查
  - Cmd_Invoke / Cmd_Recompile      | AssetDatabase.Refresh / 觸發重編 / 反射呼叫靜態方法
---

# 🧪 Unity EditMode 自動測試 Workflow

> 一句話：**測試放主專案 `Assets/Tests/`、用 EditMode NUnit 寫；但在「編輯器常開」環境不能靠 `-batchmode -runTests`(撞鎖) 也別靠 TestRunnerApi async(不穩)——改用「同步驗證 harness + Cmd_Invoke」同步跑、寫結果檔、poll。**

---

## 0. 適用情境與前提

- **被測 code 多在 UCL_Core submodule**（純 C#，如 `UCL.Core.JsonLib.JsonData`）。
- **測試放主專案**（`Assets/Tests/`，**不進 UCL_Core**）。代價：共用 UCL_Core 的其他專案(EOV)吃不到這套網——記為技術債，日後要共享再搬進 UCL_Core。
- **執行環境特殊**：Unity 編輯器**常駐開著**，agent 透過 `AgentCommands` 的 Cmd watcher 驅動編輯器。這使「開第二個 batch 實例跑測試」會撞專案鎖 → 必須走 in-editor 路徑（見 §4）。

---

## 1. 前置（一次性）

- **test-framework**：`Packages/manifest.json` 需有 `com.unity.test-framework`（本專案已 resolve 到 `1.6.0`, depth:0）。
  - ⚠ 若要新增/改版本：見 **§5 踩坑 1**——改 manifest 版本號會觸發 package 重解析 + domain reload，把 Cmd watcher 卡住。**別在靠 watcher 幹活的當下改**。
- `com.unity.ext.nunit` 通常已隨 test-framework 間接解析進來，不必手動加。

---

## 2. 建測試組件（asmdef）

`Assets/Tests/EditMode/<Name>.Tests.EditMode.asmdef`：

```json
{
    "name": "Bar.Tests.EditMode",
    "references": ["UCL_Core", "UnityEngine.TestRunner", "UnityEditor.TestRunner"],
    "includePlatforms": ["Editor"],
    "overrideReferences": true,
    "precompiledReferences": ["nunit.framework.dll"],
    "autoReferenced": false,
    "defineConstraints": ["UNITY_INCLUDE_TESTS"]
}
```

- `references` 掛**被測組件**(如 `UCL_Core`)才能存取被測型別；跨 git submodule reference 沒問題（assembly 層級，與 submodule 邊界無關）。
- `includePlatforms:[Editor]`：純 C# 邏輯用 EditMode 即可，**不需 PlayMode**（快、免進 play）。
- `defineConstraints:[UNITY_INCLUDE_TESTS]`：非測試 build 不編這組件。

---

## 3. 寫測試（NUnit）

- **namespace 先查清**：用被測型別的**實際 namespace**（例：`UCL.Core.JsonLib`，不是 asmdef 的 rootNamespace）。`grep -n "^namespace" <file>` 確認，猜錯直接編不過。
- 用 `[Test]` / `[TestCase]` / `[TestCaseSource]`。

### 常用測試模式庫

| 目的 | 模式 |
|---|---|
| **驗 refactor 未改行為**(golden/差分) | 若舊實作方法仍在（即使 private），用**反射**在**同一個 build** 跑「舊路徑 vs 新路徑」逐筆斷言 byte-identical → 免 git worktree、測的是實際出貨碼。舊私有方法只要未被該次 refactor 改動，其輸出 == 改動前基準。|
| **驗讀取無副作用**(Heisenbug) | 讀某屬性後**斷言物件狀態不變**（e.g. 讀 `Count` 後型別仍是原型別；讀後序列化不得出現幻影輸出）。|
| **文化不變**(locale) | `[SetUp]/[TearDown]` 存還原 `Thread.CurrentThread.CurrentCulture`；在 `de-DE`/`fr-FR`(逗號小數) 下驗數值序列化仍用 `.`。|
| **跨型矩陣** | 對每個來源型別建實例，交叉呼叫各 getter，斷言值正確 + 型別不符回 default。|

反射差分範例（重建「舊 ToJson」對拍新 `ToJson()`）：
```csharp
var mi = typeof(JsonData).GetMethod("SerializeValue", BindingFlags.NonPublic|BindingFlags.Instance);
string OldToJson(JsonData d){ var sb=new StringBuilder(); mi.Invoke(d, new object[]{ d.GetObj(), sb }); return sb.ToString(); }
// 斷言: OldToJson(d) == d.ToJson()
```

---

## 4. ★ 在本環境「跑」測試（最關鍵、坑最多）

### 4a. 標準路（乾淨環境 / CI 用，本專案編輯器常開時**不適用**）
- Test Runner 視窗：`Window > General > Test Runner > EditMode > Run All`。
- CI/headless：`Unity -batchmode -runTests -testPlatform EditMode -testResults results.xml -projectPath . -logFile -`。
- ⚠ 兩者都要「編輯器沒開」或「獨立 batch 實例」；本專案編輯器常駐 → **會撞專案鎖**。

### 4b. 本環境路（編輯器常開 + Cmd watcher）——實測可行
1. **讓編輯器 import 新檔**：新增 .cs/.asmdef 後，`Cmd_Invoke` 呼叫 `AssetDatabase.Refresh`：
   ```
   run_cmd.py run Invoke --arg type=UnityEditor.AssetDatabase --arg member=Refresh
   ```
   ⚠ **`Recompile` Cmd 只重編既有組件，不 import 新檔**（`.meta` 不會生成）→ 新檔一定要先 Refresh。
2. **驗編譯**：`check_compile.py --watch --watch-timeout 90`，確認 0 error + 時間戳是新的 + `Library/ScriptAssemblies/<你的組件>.dll` 存在。
3. **跑測試 = 同步驗證 harness**（**不要**用 TestRunnerApi async，見 §5 踩坑 2）：
   - 寫一個**靜態方法** `RunChecks()`，inline 把所有斷言跑一遍、計數 pass/fail、**同步寫結果檔**（e.g. `Temp/bar_checks.json`）。
   - 該 harness 的 asmdef 要 reference 被測組件(UCL_Core)。
   - `Cmd_Invoke` 同步觸發它：
     ```
     run_cmd.py run Invoke --arg type=<NS>.BarJsonChecks --arg member=RunChecks
     ```
   - poll 結果檔 → `{"pass":N,"fail":M,"fails":"..."}`。
   - 範本見 `Assets/Tests/Runner/BarJsonChecks.cs`。
- **兩套並存**：NUnit 那套(§2/§3)留給 CI/未來乾淨環境；同步 harness 是「編輯器常開時當下拿結果」的路。

---

## 5. 踩坑清單（hard-won，2026-07-09 jsondata-test）

1. **改 `manifest.json` 版本號 → package 重解析 + domain reload → Cmd watcher 佇列卡死（~10 分鐘）。**
   Editor.log 會出現 `Package Manager server shutdown` / `MemoryLeaks`(domain unload)。
   → 靠編輯器 watcher 幹活時**別動 manifest 版本**；要動就 pin 成**已 resolve 的版本**、並容忍一次 reload、reload 後再繼續。
2. **`TestRunnerApi.Execute` 的 async callback 在本環境不穩**，兩個獨立原因：
   (a) `api = ScriptableObject.CreateInstance<TestRunnerApi>()` 若是 **local 變數**，方法回傳後被 GC → `RunFinished` 永不觸發 → **必須用 static 欄位持有** api 與 callbacks；
   (b) `Execute(EditMode)` 會觸發測試組件重編/domain 擾動 → 驅動它的 Cmd 可能 timeout。
   → **純驗證場景**不值得跟它纏鬥，改用 §4b 的**同步 harness**。
3. **`AssetDatabase.Refresh` ≠ `Recompile`**：前者 import 新資產(生 .meta)，後者只重編既有組件。新增檔要先 Refresh。
4. **namespace 猜錯直接編不過**：先 `grep "^namespace"` 確認被測型別的實際 namespace。

---

## 6. 驗收判定

- 同步 harness 結果檔 `fail==0` → 改動實測正常。
- **byte-identical 差分綠燈** = 「refactor 未改變輸出行為」的 runtime 驗收證據（可用來收對應的 refactor task review）。

---

## 7. 首個實例（可照抄）

- Quest：`jsondata-test`（5 tasks 全 done）。
- 驗證對象：`UCL_JsonData` 的 A1(序列化去深拷) / B3(讀取副作用) / locale / getter(UInt/ULong) 改動。
- 結果：**94 pass / 0 fail**，A1 byte-identical 成為 quest `jsondata-opt` T04 的驗收證據。
- 產物：`Assets/Tests/EditMode/*`（NUnit）、`Assets/Tests/Runner/*`（同步 harness + TestRunnerApi runner）。
