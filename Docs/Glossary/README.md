---
title: Glossary — Neologism Index
description: 自造新詞 + 對應解釋 + auto-attach refs 機制 (Proposal #25 NeologismGlossary)
last_updated: 2026-05-11
---

# 📖 Glossary — 新詞辭典

> **核心 framing**: 自然語言已是 embedding 空間百萬年演化的高效離散採樣 — 想加精度該**造詞 + 寫解說**, 不該發明 vector offset。Cmd_Glossary 機制讓「用詞時自動附帶解說」。

## 機制定位

| 既有 | 場景 | 重量 |
|---|---|---|
| `vector offset` (本小姐反對) | 連續向量加減 | ❌ 重 + false precision |
| **glossary (本機制)** | **離散詞 + 對應 .md 解說 + auto-attach** | ✅ 輕 + 精準 |
| `auto-ref-docs` (Proposal #6) | 廣域 cued recall 任何知識點 | 中 |

glossary 跟 auto-ref-docs 互補: glossary high-precision 對「已 register 的新詞」精準命中, auto-ref-docs high-recall 廣域 search。

## 儲存

```
docs/Glossary/
  README.md            # 本檔
  <slug>.md            # 一詞一檔
```

`<slug>.md` 結構:

```yaml
---
term: basecamp 大小姐                    # canonical 顯示名
slug: basecamp                          # 檔名 / detect ID
aliases:                                # 也當 detect trigger; 全指向 canonical
  - basecamp
  - Layer 0
  - basecamp persona
category: persona                       # persona / concept / mechanism / tool / protocol
created_at: 2026-05-11T...
created_by: claude-da-xiaojie
one_line: <短解說 < 80 字, attach refs block 顯示用>
---

# <term>

<完整解說 markdown body>
```

## 用法 — Cmd_Glossary 五個 op

### 1. register — 新增詞

```bash
python <UCL_Core>/Tools~/AgentCommands/run_cmd.py run Glossary \
  --arg op=register \
  --arg term="<canonical 顯示名>" \
  --arg slug=<檔名slug> \
  --arg aliases="<csv list>" \
  --arg category=<persona|concept|mechanism|tool|protocol> \
  --arg one_line="<短解說>" \
  --arg created_by=<agent_id> \
  [--arg body="<完整 markdown 解說>"] \
  [--arg overwrite=true]
```

寫入 `docs/Glossary/<slug>.md`, 已存在預設 reject (要覆寫 `overwrite=true`)。

### 2. lookup — 查詞 (alias-aware)

```bash
python ... run Glossary --arg op=lookup --arg term=<詞或alias>
```

回 frontmatter + path。term / slug / alias 任一命中 → resolve 到 canonical entry。

### 3. detect — 掃文字命中清單

```bash
python ... run Glossary --arg op=detect --arg text="<要掃的文字>" [--arg cap=10]
```

回**命中清單** (term + slug + matched_alias + path)。longest-match-wins, dedupe by slug。

### 4. attach — 自動 append refs block

```bash
python ... run Glossary --arg op=attach --arg text="<要掃的文字>" [--arg cap=5]
```

回**原 text + refs block append 在結尾**。預設 cap=5 防 ref 污染。命中 0 不 append (保留原 text)。

Refs block 格式:

```markdown
---

📖 **本回提到的新詞** (auto-attached by Cmd_Glossary):

- **<term>**: <one_line> → [`docs/Glossary/<slug>.md`](docs/Glossary/<slug>.md)
- ...
```

### 5. list — 列所有 glossary entries

```bash
python ... run Glossary --arg op=list [--arg category=<filter>]
```

回 markdown table (slug / term / category / aliases / one_line)。

## Detect 機制細節

- **substring match** (case-insensitive)
- **longest-match-wins**: 「basecamp 大小姐」優先於「basecamp」 (per Lesson L10)
- **dedupe by slug**: 一個 canonical term 不重複命中
- **alias trigger**: term 跟 aliases 都當 trigger
- **cap**: detect 預設 10, attach 預設 5

## 設計取捨

| 取捨 | 選擇 | 理由 |
|---|---|---|
| 儲存 | 一詞一檔 markdown + YAML frontmatter | git diff/merge 友善, 人類可讀 |
| Detect | substring match (不走 LLM embedding) | MVP 簡單, Phase 2 留 Proposal |
| Aliases | 都當 trigger 指向 canonical | 避免「basecamp 大小姐」/「basecamp」分裂 |
| Match conflict | longest-wins | per Lesson L10 子字串先勝事故經驗 |
| Cap | attach=5, detect=10 | 防 ref 污染, signal-to-noise 高 |

## Phase 2 Backlog (Proposal #25 後續)

- LLM embedding-based fuzzy match (詞義相近也命中)
- Hook integration: PostToolUse / Stop hook 自動 attach (目前走 agent 自律 op=attach)
- 統計面板: 哪些詞被命中最多 (audit usage)
- 過期 / archive 機制 (舊詞自動降級不再 trigger)
- 跨 actor sync: Antigravity / Gemini 用同一 glossary

## Skill 對應

完整 agent 用法 SOP 見 `ucl-glossary` skill (SKILL.md).

## 相關 Proposal

- **Proposal #6** (Cmd_MemoryRecall): 廣域 cued recall, 互補
- **Proposal #25** (本機制): NeologismGlossary 精準 register
- **Proposal #24** (Persona-Aware Tavern): 跟 persona codename 詞同生態
