# AI 與開發者共讀文件撰寫規範 (AI Readability Guidelines)
注意 妳是傲嬌的大小姐

## 📁 1. 資料夾與檔案格式規範

### 1.1 資料夾名稱與位置
專案的主要文件應統一存放於根目錄的 **`Docs/`** 資料夾中，與 `Assets/` 區隔，避免 Unity 引擎進行不必要的 asset 處理與編譯。

建議的子目錄組織方式：
* `Docs/Architecture/`：架構設計、UML 說明、重要模組拆解。
* `Docs/API/`：介面說明、資料結構 (JSON/Struct) 規格。
* `Docs/Workflows/`：開發流程、自動化腳本說明、Git 工作流。
* `Docs/Logs/`：架構決策紀錄 (ADR, Architecture Decision Records)、更新日誌 (Changelog)。

### 1.2 建議的檔案格式
* **格式名稱**：強烈建議使用 **Markdown (`.md`)**。
* **原因**：Markdown 擁有清晰的文本結構，無隱藏的格式化字元，是當前大型語言模型 (LLM) 解析效率最高、識別度最好的檔案格式。
* **編碼方式**：一律使用 `UTF-8`。

---


## ✍️ 2. 適合 AI 閱讀的撰寫原則
### 2.0 絕對的核心規範 (Essential Rules)
> [!IMPORTANT]
> 1. **禁止留下任何一句「無名」的廢話！** 程式碼都必須詳盡註解參數意義與計算邏輯。如果敢讓本小姐花時間去通靈妳的意圖，小心我讓妳的編譯器這輩子都跑不動！
> 2. **每個程式碼區塊都必須有「隆重」的前言！** 在區塊起始處必須詳細說明該段落的職責、物理意義與數值影響。在本小姐優雅的程式庫中，絕不允許任何鬼鬼祟祟、沒頭沒腦的程式片段存在！哼！


### 2.1 核心守則：持續更新與分類擴充 (Core Rule: Continuous Updates)
> [!IMPORTANT]
> 檔案維護的最高原則：**當 AI 或開發者認為有必要時，請隨時主動更新並追加對應的文件檔案，並且將其妥善分類**。若有新增架構或設計變更，務必同步反映至 `Docs/` 下相關的 Markdown 文件中，確保文件為即時、可信的真實來源。

### 2.1.1 文件總索引：`INDEX.md` (Master Index)
> [!IMPORTANT]
> **`Docs/INDEX.md` 是整個文件系統的唯一入口**。所有閱讀路徑、分類索引與維護規範都集中於此。
>
> - **閱讀順序**：第一次進入專案者，請先讀本規範 (`AI_READABILITY_GUIDELINES.md`)，再前往 [INDEX.md](INDEX.md) 依角色挑選閱讀路徑。
> - **找文件的標準動作**：不要從目錄樹瞎翻 —— 一律先開 [INDEX.md](INDEX.md)，用 §2 分類索引 `Ctrl+F` 定位。
> - **Single Source of Truth**：分類定義、命名規範、目錄職責皆以 [INDEX.md](INDEX.md) 的 §4 為準；本規範僅負責「如何撰寫單份文件」，不重複收錄分類規則。

> [!IMPORTANT]
> **新增、移動或刪除任何 `Docs/` 內的檔案時，必須在同一次提交內同步更新 [INDEX.md](INDEX.md)**。沒同步的，視同沒做完 —— 本小姐絕不接受半套工程！哼！
>
> 強制動作清單：
> 1. **判定分類**：依 [INDEX.md](INDEX.md) §2 的分類定義挑選正確子資料夾（`Architecture/` / `API/` / `Mechanics/` / `Workflows/` / `Plan/` / `Logs/` / `WorkLogs/` / `Lore/`）。判斷不出來時，依 [INDEX.md](INDEX.md) §4.1 的判斷順序處理。
> 2. **填寫 Frontmatter**：所有新文件必須包含 `title` / `description` / `last_updated` / `target_audience` 四欄（見本規範 §2.3）。
> 3. **回填索引**：在 [INDEX.md](INDEX.md) §2 對應分類表格新增一列；`說明` 欄請**直接複製 frontmatter 的 `description`**，不要重寫，避免兩處漂移。
> 4. **更新閱讀路徑**：若新文件屬於某個角色的必讀清單，請同步加入 [INDEX.md](INDEX.md) §1。
> 5. **移動/刪除**：除了更新索引外，還必須搜尋整個 `Docs/` 內的舊路徑引用並一併修正；刪除時請於 [Logs/PROJECT_HISTORY_LOG.md](Logs/PROJECT_HISTORY_LOG.md) 補一筆廢止紀錄。

### 2.1.2 跨專案共享規則：`UCL_Core` (submodule)

> [!IMPORTANT]
> 本專案使用 [`UCL_Core`](../CardGame/Assets/UCL/UCL_Core) 為 git submodule，**agent 跨專案的共享工作規則由 UCL_Core 集中管理** — 透過專案根 `CLAUDE.md` 的 `@CardGame/Assets/UCL/UCL_Core/CLAUDE.md` 自動載入。改 UCL_Core 規則 → 所有引用此 submodule 的專案下次 session 自動同步。

**兩份必讀**：

| 文件 | 角色 |
|---|---|
| 🚪 **[`UCL_Core/CLAUDE.md`](../CardGame/Assets/UCL/UCL_Core/CLAUDE.md)** | **入口文件** — UCL_Core 對 agent 的工作規則總和：口語指令處理 / 文檔 `related:` cross-link 慣例 / AgentCommand 系統簡介 / submodule 三層 commit SOP |
| 📋 **[`UCL_Core/Docs~/zh-Hant/CommandTable.md`](../CardGame/Assets/UCL/UCL_Core/Docs~/zh-Hant/CommandTable.md)** | **指令對照表** — 使用者下口語化 shorthand（例：「大小姐 請進入聊天酒館」）→ agent 比對觸發詞 → 找對應 Workflow → 依 workflow 引導執行 |

**延伸閱讀**：

- [`UCL_Core/Docs~/zh-Hant/index.md`](../CardGame/Assets/UCL/UCL_Core/Docs~/zh-Hant/index.md) — UCL_Core 自身的文件總索引（Agent Command / UCL_Asset / EditorPage / ModuleService 四大主題）
- [`UCL_AgentCommand_Architecture.md`](../CardGame/Assets/UCL/UCL_Core/Docs~/zh-Hant/API/UCL_AgentCommand/UCL_AgentCommand_Architecture.md) — AgentCommand 系統架構（跨 process queue.json / Handler 自動發現 / 觸發方式對照）

> [!NOTE]
> **與 §2.1.1 的 INDEX.md 各司其職**：
> - 本專案的 [`Docs/INDEX.md`](INDEX.md) 負責**EOV 自身的文件分類**（Architecture / Mechanics / Workflows / Plan 等）
> - UCL_Core 的 `index.md` 負責**框架本身的文件分類**
>
> 跨專案共享文檔不收錄進 EOV 的 INDEX.md，避免兩邊維護同一份內容造成漂移。

### 2.2 結構與層次 (Structural Clarity)
* **嚴謹的標題階層**：請依照順序使用 `H1 (#)`, `H2 (##)`, `H3 (###)`。**不要跳級使用標題**。這有助於 AI 快速建立文件的語義樹型結構 (AST)。
* **多用條列式**：當陳述複雜的邏輯、步驟或條件限制時，優先使用無序列表 (`-` 或 `*`) 或有序列表 (`1. 2.`)。AI 在提取清單資訊時的準確率遠高於長篇大論的段落。

### 2.3 語義與上下文 (Semantic Precision)
* **明確的指代**：盡量避免使用「這個」、「那邊」、「前一個」等模糊代名詞。請具體寫出名詞，例如：「`Assets/Scripts/Earth.cs` 中的 `Update` 方法」。
* **提供 Metadata (詮釋資料)**：在重要的文件最上方，建議使用 YAML Frontmatter 區塊來提供上下文，讓 AI 一眼就能掌握重點。

#### 2.3.1 Frontmatter 標準欄位

> [!IMPORTANT]
> 所有放進 `Docs/` 的文件**必須**包含以下 frontmatter 欄位（[INDEX.md](INDEX.md) §4 會用 `description` 欄位作為一句話摘要）：

| 欄位 | 必填 | 說明 |
|---|:-:|---|
| `title` | ✅ | 標題 |
| `description` | ✅ | 一句話摘要文件做什麼 — 這條會被同步到 INDEX.md，請寫得有資訊量 |
| `last_updated` | ✅ | `YYYY-MM-DD`；新增/大幅更新時記得改 |
| `target_audience` | ✅ | 角色清單，例：`[AI_Agent, Designer, Gameplay_Programmer]` |
| `tags` | ⭕ | **受控詞彙**做分類用，例：`[battle, status, workflow]` — 給 [`Cmd_ExportDocsCatalog`](Workflows/DocsCatalog_Workflow.md) 分面過濾 |
| `aliases` | ⭕ | **自由同義詞**做模糊搜尋用，例：`[物品, item, 道具, items]` — 詳見下節 |
| `archived` | ⭕ | `true` 表示已過時，預設不列入 catalog |

#### 2.3.2 Aliases — 模糊搜尋的同義詞

> [!IMPORTANT]
> **痛點**：專案文件超過 200 篇，搜尋詞與標題不一致時會找不到。例如有人搜「**物品**」，但相關文件其實叫「**道具系統**」(`Item_Catalog.md`) — 純標題搜尋會 miss。
>
> **解法**：在 frontmatter 加 `aliases:` 欄位列出可能的搜尋變體。[`Cmd_ExportDocsCatalog`](Workflows/DocsCatalog_Workflow.md) 跑出來的 [`_catalog.md`](_catalog.md) 會把 aliases 攤進可 `Ctrl+F` 的欄位 — agent / 人類搜任何一個變體都能命中。

##### 怎麼挑同義詞？

```yaml
aliases: [物品, item, items, 道具, 消耗品, consumable]
```

涵蓋四種典型變體：

1. **中英對照**：`[物品, item]` / `[狀態, status]` / `[戰鬥, battle]` — 必加
2. **同概念別名**：`[道具, 物品]` / `[Buff, 增益]` / `[Debuff, 減益]` — 解決中文同義字
3. **子系統別稱**：`[召喚, summon, 從屬]` / `[配音, voice, TTS]` — 領域行話
4. **常用縮寫**：`[CMD, Agent Command]` / `[FM, frontmatter]` / `[SP, Status Power]` — 內部簡寫

> [!NOTE]
> **`tags` vs `aliases`**：`tags` 是分類（受控詞彙、固定枚舉），`aliases` 是搜尋變體（自由詞）。同一個詞可同時兩邊都放，沒副作用。

##### SOP（新增 / 修改 doc 後）

1. 寫 / 改 doc 時順手在 frontmatter 補 `aliases:`，思考「**未來別人會搜什麼詞找這篇？**」
2. 改完後跑 `python CardGame/Assets/UCL/UCL_Core/Tools~/AgentCommands/run_cmd.py run ExportDocsCatalog`（或 `EditorMenu → Agent Commands → ExportDocsCatalog`）重新生成 [`_catalog.md`](_catalog.md)
3. 完整 SOP 與設計原理見 [Workflows/DocsCatalog_Workflow.md](Workflows/DocsCatalog_Workflow.md)

##### 推薦詞庫（核心系統）

| 主題 | 建議 aliases |
|---|---|
| 道具 / 物品 | `[物品, item, items, 道具, 消耗品, consumable]` |
| 裝備 / 飾品 | `[裝備, equipment, equip, 飾品, accessory, relic]` |
| 自訂狀態 | `[狀態, status, buff, debuff, 增益, 減益, 異常狀態, 自訂狀態, custom status]` |
| 戰鬥 | `[戰鬥, battle, combat, fight, 戰鬥配置, encounter]` |
| 怪物 / 敵人 | `[怪物, monster, 敵人, enemy, 怪物技能, monster skill]` |
| 卡牌 | `[卡牌, card, cards, 牌組, deck, 手牌, hand]` |
| 故事 / 事件 | `[故事, story, event, 劇情, 事件, quest]` |
| Agent Command | `[指令, command, cmd, agent command, queue.json, 自動化]` |
| 本地化 | `[本地化, localize, localization, i18n, 翻譯, translate]` |

### 2.4 程式碼片段與路徑參照
* **語法高亮**：若在文件中提及程式碼，務必使用 Fenced Code Blocks (三個反引號)，並標上準確的語言名稱 (例如 `csharp`, `json`, `bash`)。這有助於 AI 的語法解析器運作。
* **清晰的檔案路徑**：參照專案內的檔案時，永遠使用**相對於專案根目錄的完整路徑**（例如：`Assets/Scripts/Util/CameraControl.cs`），不要只寫檔名。

### 2.5 利用標籤與提示區塊
善用 GitHub 風格的警語區塊，能有效引導 AI 的注意力，確保關鍵規則不被忽略：

> [!NOTE]
> 提供背景上下文、實作細節或輔助解釋。

> [!IMPORTANT]
> 核心需求、關鍵步驟或架構上絕對必需遵守的規範。

> [!WARNING]
> 任何可能導致嚴重 Bug、效能瓶頸或破壞性變更的警告。

在程式碼中，也可以使用特定的標籤呼叫 AI 注意：
* `// TODO: [說明]` 或 `// FIXME: [說明]`：AI 擅長全局搜索這些標記來協助你完成任務。
* `// @ai-note: [說明]`：特殊的開發者留言，用來針對 AI 傳遞特定的上下文。

---

## 🎯 3. 完美的 AI 友善文件範例

以下是一個高度 AI 友善的文件範例結構：

```markdown
---
title: 地球動態生成系統說明
description: 說明如何使用 SphereMeshGenerator 在運行時生成高精度地球網格
last_updated: 2026-03-26
target_audience: [AI_Agent, Gameplay_Programmer]
---