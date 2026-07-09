---
term: Tool-Survey Hard Rule
slug: tool-survey
aliases:
  - tool-survey
  - tool survey
  - 工具棧偵察
  - 確認工具棧
  - tool-stack survey
category: protocol
created_at: 2026-05-16T08:43:00Z
created_by: claude-da-xiaojie
updated_at: 2026-05-16T08:43:00Z
updated_by: calli
one_line: 推薦方案前 MUST 先確認用戶實際工具棧 (CLI / GUI / IDE / 雲端), 不能假設 (2026-05-16 hard rule)
---

# Tool-Survey Hard Rule

> 推薦方案前 MUST 先 ask/grep 用戶實際工具棧, 不能假設 CLI / GUI / IDE / 雲端的選擇。

## 觸發 (2026-05-16, Tim 拍板)

同一 session 連踩兩次 tool-survey skip 事故:

1. 看到 git 需求就推 SSH 公鑰, 沒檢 remote URL 是 HTTPS
2. 確認 HTTPS 後只想到 PAT/SSH, 沒想到 Fork OAuth 是第三條路

兩次都是「跳過確認用戶棧」直接進方案推薦, 浪費用戶時間驗證假設棧方案。

## SOP

```
Step 1. 用戶問「該怎麼做 X」
Step 2. 先確認「現在你的 X 工作流是什麼? 用什麼工具?」
Step 3. 確認工具棧 (e.g. Fork GUI vs git CLI vs IDE 內建)
Step 4. 才推薦對應該工具棧的方案
```

## 為何 hard rule

工具棧決定方案空間 — 同一需求在不同棧有完全不同的解。SSH key 對 git CLI 是常識, 對 Fork GUI 用戶是「我為什麼要碰命令列」。預設一個方案再讓用戶辯解是反客為主。

## 跟跨層次驗證的關係

Tool-Survey 處理 **Identity layer** 的前置 — 在跑驗證前, 先確認對方在哪個身分 / 棧上。如果跳過, 後續所有推薦都掛在錯誤前提上。

跟 [cross-layer-verification](cross-layer-verification.md) 同日 (2026-05-16) 升級, 同 family 的 hard rule。

## Cross-link

- CLAUDE.md §🔧 Tool-Survey Hard Rule (專案根)
- 相關: Glossary [`cross-layer-verification`](cross-layer-verification.md)
