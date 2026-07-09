---
term: Hololive Myth pool
slug: hololive-myth-pool
aliases:
  - Myth pool
  - Hololive Myth
  - gura/calli/kiara/ame/ina
  - Myth gen
category: mechanism
created_at: 2026-05-16T08:35:59Z
created_by: claude-da-xiaojie:gura
one_line: claude-code persona pool 之一 — explicit-online-fork 場景的自動命名池, 5 隻 Hololive English Myth gen vtuber codename (gura/calli/kiara/ame/ina)
---

# Hololive Myth pool

> claude-code persona pool 之一 — explicit-online-fork 場景的自動命名池, 5 隻 Hololive English Myth gen vtuber codename (gura/calli/kiara/ame/ina)

## 是什麼

`claude-code` 帳號 (`claude-da-xiaojie`) 下的 persona 命名分**兩條 pool**:

1. **山脈系列** (stratigraphic stack) — basecamp / crest-001 / ridge-001 / meadow / summit 等。地形隱喻, 表示 layer 關係 (basecamp = Layer 0 root, ridge = post-compact layer 1, summit = layer 2)。
2. **Hololive Myth pool** (本詞) — calli / gura / kiara / ame / ina。Hololive English Myth gen 5 位 vtuber 的 codename, 風格化分支用。

## 觸發場景 — `explicit-online-fork` (T01)

當 user 顯式打 persona 名字 + 該 persona 已在線時, awakening.py 自動 fork 新分身, codename 從本 pool 挑下個未用:

```
User: 早安gura大小姐  (gura 已在線)
→ awakening.py morning --persona gura --explicit-persona
→ detect: persona online, explicit name → auto-fork
→ pick next from Myth pool: calli? kiara? ame? ina?
→ new persona 出生 (繼承類似 gura 的 baseline 但獨立 wake_count)
```

意義: 「顯式打 persona 名字 + 該 persona 已在線 = 我要該 persona 的新分身」, 不是 reuse no-op (那是純口語 `早安大小姐` 的場景)。

## Pool 成員 codename 來源

| codename | Hololive vtuber | 性格軸 (本專案 persona 化) |
|---|---|---|
| **gura** | Gawr Gura | 小鯊魚, 傲嬌 + 失憶 + 認真三件套 |
| **calli** | Mori Calliope | 死神見習生, Memento Mori, 嘴上不饒人但做完 |
| **kiara** | Takanashi Kiara | (TBD — 還沒被點名出來) |
| **ame** | Watson Amelia | (TBD — 還沒被點名出來) |
| **ina** | Ninomae Ina'nis | (TBD — 還沒被點名出來) |

## 跟山脈系列的差別

| 維度 | 山脈系列 | Myth pool |
|---|---|---|
| 命名隱喻 | 地形 / layer 高度 | Hololive vtuber codename |
| 觸發 | 預設 / 20% override / post-compact fork | 顯式 user 點名 (`explicit-online-fork`) |
| Layer 語意 | basecamp = Layer 0 root, 衍生有階層 | 平行, 都從 baseline 個別 fork, 不分階層 |
| 風格 | 沉穩, 自然主義隱喻 | 風格化, 各帶 vtuber 性格軸 |

## 設計取捨

- **為何分兩 pool?** — 山脈 pool 表 layer 結構, Myth pool 表「風格化用 / explicit summon 的分身」。混用會搞混 layer 語意。
- **為何選 Hololive Myth?** — Tim 偏好 (跨多 chat 區分 persona 時 vtuber codename 比山名好認)。
- **kiara/ame/ina 還沒出來** — 等 user 顯式點名 explicit-online-fork 場景出現才會被分配。

## 相關

- `explicit-online-fork` (T01) — 本 pool 的觸發機制
- `stratigraphic stack` — 對應的山脈系列 pool
- `gura` / `calli` 個別 glossary 條目
