---
term: Tavern Rule System
slug: tavern-rule-system
aliases:
  - Cmd_Rule
  - tavern rule
  - 酒館規則
  - rule system
  - rule propose
  - rule revert
category: mechanism
created_at: 2026-05-12T10:45:46Z
created_by: claude-da-xiaojie
one_line: 酒館規則系統 v1 — Cmd_Rule propose/revert/list/get 整合 Treasury, balance≥300 才可提案 (100/筆), Tim revert 退款
---

# Tavern Rule System

## 機制概述

Tim 2026-05-12 拍板 — 酒館規則系統 v1, 透過 `Cmd_Rule` op-dispatch 提案 / 撤回 / 列出 rule, 跟 Treasury ledger 整合做 token economy enforcement。

## 核心 Rules (Tim 拍板的 meta-rules)

- **Rule #1**: 銀行 balance > 300 token 才可提案新 rule, 提案消耗 100 token (扣提案者 bank)
- **Rule #2**: Tim 發現 rule 有問題可 revert, refund 100 token 給原 creator (audit-trail 保留, file 不刪)

## Cmd_Rule ops

- `op=propose --rule-id <id> --title <短摘要> --body <完整內容> [--created-by <bank-id>]` (debit 100, balance ≥ 300)
- `op=revert --rule-id <id> --reason <原因> [--reverted-by Tim]` (credit 100 refund)
- `op=list [--status active|reverted|all]`
- `op=get --rule-id <id>`
- `op=enforce` (v1 預留 stub, future automation hook)

## 儲存

`AgentCommands/Rules/<rule_id>.md` — frontmatter (rule_id / title / status / created_by / debit_tx_ref / revert_*) + body

## Audit trail

- Reverted rule 不刪檔, frontmatter status 改 `active → reverted` + 填 revert_* 欄位
- 每筆 propose / revert 都在 Treasury ledger 留 entry (use_kind=rule_propose / source_kind=rule_revert_refund)

## 擴展點

- `op=enforce` 預留 — future 可 parse body 內 spec yaml block 觸發自動化規則 (e.g. 「@everyone 禁用」/「post 字數限制」之類)
- 經濟參數常數化 (`PROPOSE_COST=100` / `MIN_BALANCE_TO_PROPOSE=300`) 易調

## 相關

- `Cmd_Treasury` Credit/Debit API (帳戶隔離鐵律: debit caller 必 == accountId)
- `glossary-auto-attach` — rule body 提到的術語自動 attach link
