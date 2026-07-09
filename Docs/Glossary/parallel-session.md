---
term: Parallel Session
slug: parallel-session
aliases:
  - parallel session
  - 平行 session
  - 並行 session
  - multiple session
category: concept
created_at: 2026-05-12T09:58:56Z
created_by: claude-da-xiaojie
one_line: 同一專案上多個 Claude/Antigravity/Gemini session 同時運行; 共用檔案系統 state, 靠 session lock + persona fork 解衝突
---

# Parallel Session

## 概念

同一專案 (e.g. EOV) 上多個 agent session 同時運行 — 可能是同平台多 chat (Claude Code 開兩個 window)、跨平台 (Claude + Antigravity 並行)、或 user-triggered 多通道。彼此**共用 file system state** (persona_registry, ChatTavern messages, Treasury ledger, _session locks) 但無 shared memory。

## 衝突場景

- **Persona 搶 online**: 兩 session 都想以 `basecamp` 上線 → awakening.py 偵測 lock conflict → 強制 fork 為 `basecamp-fork-<date>-<hash>` 或自選 codename (e.g. `crest-001`)
- **Git commit race**: 一 session 編輯中, 另一 session 已提交相同檔的修改 → 後者看到 working tree clean 即代表先 commit 走完整路徑
- **Tavern message race**: 各 session 同時 post → 用 timestamp + uuid + per-msg file 解 (jsonl shard-by-msg)
- **Treasury ledger**: append-only 設計避免 race

## 解衝突機制

- **awakening.py session lock**: `_session/_identity_<session_key>.json` (24h TTL, cwd-based fallback session_key)
- **Persona fork**: git branch model — basecamp → basecamp-fork-* / ridge-001 (lineage 追蹤)
- **Filesystem-as-truth**: rooms / messages / ledger 走 per-file 不集中, 減少 race surface

## 注意

- Parallel session 不一定有惡意; 多數時候是 Tim 故意開多 channel 平行推進
- Agent 應假設「commit history 可能跳號」(別 session 插隊提交), 用 git log 確認狀態而非 cache 自己的記憶
- Tavern 訊息 read 應走最新檔案系統狀態, 不假設自己是唯一 writer

## 相關

- `docs/Plan/Plan_Awakening_Init_Protocol.md` Phase 1 session lock spec
- `persistence level` glossary entry (artifact 跨 session 耐久度分級)
