# 專案規則 — Codex 入口

> [!IMPORTANT]
> **本檔只是指路牌。** 開始工作前先閱讀
> [`Docs/AI_READABILITY_GUIDELINES.md`](Docs/AI_READABILITY_GUIDELINES.md)。

## 共用規則（全 agent 適用）

| 主題 | 文件 |
|---|---|
| 共用規則與文件撰寫 | [`Docs/AI_READABILITY_GUIDELINES.md`](Docs/AI_READABILITY_GUIDELINES.md) |
| 程式碼註解規範 | [`Docs/Agent/Code_Comment_Standards.md`](Docs/Agent/Code_Comment_Standards.md) |
| Tavern Share（opt-in） | [`Docs/Agent/Tavern_Share_Policy.md`](Docs/Agent/Tavern_Share_Policy.md) |
| 專案文件索引 | [`Docs/DOC_INDEX.md`](Docs/DOC_INDEX.md) |

## Codex 專屬

Codex 不支援 Claude Code 的 `@<path>` inline 載入語法。需要 UCL_Core 的跨專案 agent
規則時，請顯式讀取
[`Assets/Plugins/UCL_Core/AgentEntry/UCL_Core_Entry.md`](Assets/Plugins/UCL_Core/AgentEntry/UCL_Core_Entry.md)。

### Windows 終端文字編碼

專案內的文字檔一律視為 UTF-8。以 Windows PowerShell 5.1 讀取沒有 BOM 的檔案時，必須明確
指定 `-Encoding utf8`；否則 PowerShell 會以系統碼頁解讀而使中文顯示為亂碼。Python 指令則維持
`.claude/settings.json` 的 `PYTHONUTF8=1` 設定。

個人化偏好放 `Codex.local.md`（不入版控）；專案規則不寫在那裡。
