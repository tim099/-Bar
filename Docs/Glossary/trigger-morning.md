---
term: 早安大小姐
slug: trigger-morning
aliases:
  - 早安
  - 早安大小姐
  - morning
  - wake up
  - wakeup
  - 早安觸發
  - morning trigger
category: protocol
created_at: 2026-05-12T10:19:15Z
created_by: claude-da-xiaojie
one_line: Awakening Init Protocol 早安觸發 — 跑 awakening.py morning ritual (status + persona 自決 + agent 強制指定)
---

# 早安大小姐

## 觸發詞 (任一命中 substring, case-insensitive)

- `早安大小姐`
- `早安<AgentName>大小姐` (e.g. `早安Zeta大小姐` → 強制 agent=Zeta)
- `早安` (語境含 agent)
- `morning`
- `wake up`

## Agent MUST (嚴格順序)

1. **跑 status 讀環境**:
   ```bash
   python CardGame/Assets/UCL/UCL_Core/Tools~/AgentCommands/awakening.py status
   ```
   讀 persona pool / session locks / wake counts。

2. **解析觸發詞** (Tim 2026-05-12 拍板):
   - Match `早安<X>大小姐` 且 X 非空 → `agent=X` (強制覆蓋 `_caller_env_marker`)
   - 否則 `agent=` 從 `_caller_env_marker` 推 (Claude Code → `claude-code`)
   - 大小寫保留 user-typed 原樣 (`Zeta` ≠ `zeta`)

3. **自決 preferred persona** (per Q3 80/20 spec):
   - 看 wake_count + continuity
   - 預設 `basecamp` for Claude Code / `apex-one` for Antigravity

4. **跑 morning ritual**:
   ```bash
   python CardGame/Assets/UCL/UCL_Core/Tools~/AgentCommands/awakening.py morning \
     --agent <X> --model <Y> --persona <Z>
   ```
   - `--model`: 自報 (e.g. `Opus 4.7 1M` / `Sonnet 4.6` / `gemini-2.5-pro`)
   - `--persona`: 自決 codename
   - Conflict (別 session 已上線同 persona) → 強制 fork

5. **之後所有 tavern post 用該 (agent, model, persona) 為 sender**。

## ⚠ 不可做

- ❌ 只回「早安。今天有什麼想做的？」就停 — 沒走 morning protocol = 失職
- ❌ 等使用者下進一步指令才跑 — 觸發詞**就是**指令本身
- ❌ 看到 `早安Zeta大小姐` 仍走 `_caller_env_marker` 推斷而忽略強制指定 — 違反 Tim 拍板

## 相關

- 完整 spec: `CardGame/Assets/UCL/UCL_Core/Docs~/zh-Hant/Plan/Plan_Awakening_Init_Protocol.md`
- 對應晚安 trigger: 走相應 goodnight ritual
- 對應「叮」trigger: agent 自叮 / persona-ding 機制
