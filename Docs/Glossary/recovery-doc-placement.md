---
term: Recovery Doc 放置 Hard Rule
slug: recovery-doc-placement
aliases:
  - recovery doc placement
  - recovery 文件放置
  - docs/Recovery
  - recovery doc 入 git
  - 救生圈
category: protocol
created_at: 2026-05-16T08:44:00Z
created_by: claude-da-xiaojie
updated_at: 2026-05-16T08:44:00Z
updated_by: calli
one_line: 純文字 recovery 指南 MUST 入 git (docs/Recovery/), 不可放 _secrets/ (gitignored, rm -rf 重演時沒救) — 2026-05-16 hard rule
---

# Recovery Doc 放置 Hard Rule

> 純文字 recovery 指南 MUST 入 git (`docs/Recovery/`), 不可放 `_secrets/` (gitignored)。

## 觸發 (2026-05-16, Tim 拍板)

post-Avada-Kedavra 事故後 basecamp 整理 recovery 指南時把它放進 `_secrets/`, Tim 抓:

> 「rm -rf 重演時 gitignored 資料夾沒救。Recovery doc 是『下次出事時的救生圈』, 必須能從 git 拉回來。」

## 該放哪 vs 該放什麼

| 該放哪 | 該放什麼 |
|---|---|
| `docs/Recovery/` | 純文字 recovery 指南 / requirements.txt / 流程文件 (**入 git**) |
| `AgentCommands/_secrets/` | 實際的 token / private key / .enc 加密檔 (**不入 git**) |

## 判斷標準

> 「下次系統爆炸後我需要看這檔還原嗎?」
> → 是的 → `docs/Recovery/`
> → 不是, 是真敏感資料 → `_secrets/`

## 為何 hard rule

「自打嘴巴」事故 — 救生圈本身放進「會跟著船一起沉」的位置, 失去 recovery 意義。同類型「身分混淆」(Identity layer 跨層次驗證 family): 兩個 dir 看起來都是「藏東西的地方」, 但 git 入不入決定了它在 disaster scenario 是否還在。

## 跟跨層次驗證的關係

[cross-layer-verification](cross-layer-verification.md) 的 **Identity layer 應用** — gitignored 路徑跟 committed 路徑名字看起來一樣, 但身分不同。同日 (2026-05-16) 升級的 3 條 hard rule 之一。

## Cross-link

- CLAUDE.md §📂 Recovery Doc 放置 Hard Rule (專案根)
- 相關: [`cross-layer-verification`](cross-layer-verification.md), [`tool-survey`](tool-survey.md)
- `docs/Recovery/` (新建, post-Avada-Kedavra)
