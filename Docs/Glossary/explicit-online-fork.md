---
term: explicit-online-fork
slug: explicit-online-fork
aliases:
  - 顯式在線分身
  - T01 fork
  - explicit fork
  - explicit-persona auto-fork
category: mechanism
created_at: 2026-05-16T08:37:45Z
created_by: claude-da-xiaojie:gura
one_line: awakening.py morning T01 機制 — 顯式打 persona 名字 + 該 persona 已在線時自動 fork 新分身, codename 從 Hololive Myth pool 挑下個未用
---

# explicit-online-fork

> awakening.py morning T01 機制 — 顯式打 persona 名字 + 該 persona 已在線時自動 fork 新分身, codename 從 Hololive Myth pool 挑下個未用

## 是什麼

`awakening.py morning` 的 **T01 機制**: 處理「user 顯式打 persona 名字 + 該 persona 已在線」的場景, 自動 fork 出新分身。

## 為何需要

CLAUDE.md hard rule 規定: 「同 chat = 同 persona」, mid-chat 鎖一個 persona 後不能再鎖第二個。但 user 跨 chat 可能想「請該 persona 來」, 即使該 persona 已在別 chat 上線。

解法分兩 form:

| Form | 觸發 | 行為 |
|---|---|---|
| **Form 1** (純口語) | `早安大小姐` (沒打 persona 名字) | 同 session re-trigger → reuse no-op, 不 fork |
| **Form 2/3** (顯式名字) | `早安gura大小姐` / `/ucl-morning claude-code gura` | 若該 persona 已 ACTIVE → **auto-fork 新分身** |

意義: 「顯式打名字 + 該 persona 已在線 = 我要該 persona 的**新分身**」, 不是 reuse。

## 走法

1. `awakening.py morning --persona <X>` 帶 `--explicit-persona` flag (caller 必加, 否則被 short-circuit reuse)
2. awakening.py 偵測 X 已在 active locks
3. 從 **Hololive Myth pool** (`gura` / `calli` / `kiara` / `ame` / `ina`) 挑下個未用 codename
4. 新 persona 出生 — 繼承類似 baseline 但獨立 `wake_count` / 獨立 lock / 獨立 letter inbox

## 跟其他場景對比

| 場景 | Trigger | 行為 |
|---|---|---|
| 同 session re-trigger (Form 1) | `早安大小姐` 同 chat 再喊 | reuse no-op, 不 fork |
| explicit-online-fork (本詞) | `早安gura大小姐`, gura 已 ACTIVE | auto-fork 新 Myth codename |
| Cross-agent persona claim | `--agent X` 跟 `persona.agent` 不同 | reject (要 `--rebind-agent` ack) |
| Same session_key collision | 同 cwd 多 Claude IDE 撞 | 要 `--strict-persona` 顯式 ack |

## 設計取捨

- **為何 auto-fork 而不直接讓 user 共用 active lock?** — 違反「同 session = 同 (claim_origin, agent, persona)」, 會撞 lock ownership。
- **為何 fork 後 codename 自動挑?** — user 不必每次想新名字, pool 預定義有 5 個可用。
- **Pool 耗盡怎麼辦?** — 用完 5 個 (`gura / calli / kiara / ame / ina`) 後需要 spec 補新 pool (e.g. Hololive Promise gen) 或改 fork-name 機制顯式指定。

## 相關

- `Hololive Myth pool` — 本機制的 codename 來源
- `stratigraphic stack` — 對應的山脈 pool (走別觸發場景)
- `awakening.py morning` — 本機制的 host script
- CLAUDE.md hard rule §🌅 早安觸發 — 完整 spec
