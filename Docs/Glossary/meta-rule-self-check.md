---
term: Meta-Rule 自檢
slug: meta-rule-self-check
aliases:
  - meta-rule
  - rule-self-check
  - 新規則自檢
  - 新 Rule 不得與既有 Rule 矛盾
  - rule conflict scan
category: protocol
created_at: 2026-05-18T06:54:51Z
created_by: claude-da-xiaojie:basecamp-fork-2026-05-12-2c36
one_line: 新增 Rule (CLAUDE.md / 酒保 / SKILL.md) 前 agent MUST 自檢與既有 Rule 是否矛盾 — Tim 2026-05-18 拍板, basecamp-fork 出資 100 token
---

# Meta-Rule 自檢

> 新增 Rule (CLAUDE.md / 酒保 / SKILL.md) 前 agent MUST 自檢與既有 Rule 是否矛盾 — Tim 2026-05-18 拍板, basecamp-fork 出資 100 token

## 是什麼

**新增任何 Rule** (CLAUDE.md Hard Rule / 酒保 Time Rule / 酒保 Keyword Trigger / SKILL.md hard rule) 前, agent **MUST 自律掃既有 Rules**, 確認沒有字面 / 語意 / enforcement 衝突, 才能 ship。

## 為何需要

2026-05-18 同一 session 內踩過 2 次:
1. **3-tier idle hierarchy ship 時** — 原 SKILL.md 寫「idle = work-flavor progress report」, 跟 Tim 新規則「自由時間照領」矛盾 → ship 後才發現要改寫統一
2. **Stay-Alive rule ship 不到 30 min 自己違反** — 寫的時候沒 imagine 30 min 後會不會記得遵守 → Tim QA「空之境界梗」抓包

教訓: **寫規則跟遵守規則是兩件事**, 自檢時機要推到 ship 前, 不是 ship 後等別人撞。

## 判定權

- **Agent 自律** 先掃既有 Rules — 不能 silent add
- **Tim 人工拍板** 「真矛盾 vs 表面像」— agent 發現衝突候選 → 提 Tim 仲裁, 不可硬塞

## 矛盾類型

| 類型 | 範例 |
|---|---|
| **字面矛盾** | Rule A 寫 "always do X", Rule B 寫 "never do X" |
| **語意衝突** | Rule A "保持 stay-alive" vs Rule B "idle 太久該結束" |
| **Enforcement 衝突** | Rule A 寫 "exit 2 擋下", Rule B 寫 "silent 通過" |
| **時機衝突** | Rule A "ship 前自檢", Rule B "ship 後 review" — 看哪個 take precedence |

## 跟其他 patch 機制比較

| 機制 | 觸發時機 | 對象 |
|---|---|---|
| **Meta-Rule 自檢** (本詞) | ship **前** | 新 Rule 跟既有 Rule 的關係 |
| `workflow-patch-tool` | ship **後** confirm bug | workflow 已知問題的記帳 + 累積 3 patch → 警示 refactor |
| `dogfood` | ship **後** activity verify | 機制本身能不能跑 (不管 Rule 衝突) |

三者互補: Meta-Rule 自檢防衝突 ship, dogfood 驗活, workflow-patch 累計堆積。

## 違規後果

- 沒走自檢就硬加 Rule = 違反 Meta-Rule 本身 (遞迴自指 OK, 這是設計)
- Post-commit hook 守門 — auto-broadcast via py launcher 提醒同事

## 跟 ship-cycle 整合的 SOP

```
寫新 Rule
  ↓
imagine 自己 30 min 後會不會忘 / 違反 — 寫得是不是可遵守的
  ↓
掃既有 CLAUDE.md / SKILL.md / 酒保 rules — 找字面 / 語意 / enforcement / 時機衝突
  ↓
有衝突 → 提 Tim 仲裁; 沒衝突 → ship
  ↓
post-commit hook 廣播; affinity 自動 update (若有 cross-persona impact)
```

## 相關

- `workflow-patch-tool` — Rule ship 後撞 bug 累計機制 (post-fix)
- `dogfood` — Rule ship 後活體驗證 (post-ship)
- CLAUDE.md §📐 Meta-Rule 主章節 — 本詞的 canonical home
- 2026-05-18 出資紀錄: basecamp-fork-2026-05-12-2c36 拍板 100 token
