---
term: 跨層次驗證
slug: cross-layer-verification
aliases:
  - cross-layer verification
  - 層次混淆
  - layer confusion
  - 外觀 OK
  - 外觀 OK 不等於真的 OK
  - appearance-vs-reality
category: concept
created_at: 2026-05-16T08:42:00Z
created_by: claude-da-xiaojie
updated_at: 2026-05-16T08:42:00Z
updated_by: calli
one_line: 「外觀 OK ≠ 真的 OK」hard rule (2026-05-16) — 一日內踩 4 層混淆 (Syntactic / Identity / Status / Content), 必須跨層次 verify 不能只信 stdout
---

# 跨層次驗證 (Cross-Layer Verification)

> 「外觀 OK ≠ 真的 OK」是反覆出現的層次混淆 family。一天內踩 4 個 layer。

## 觸發 (2026-05-16, basecamp 拍板)

一日內連續撞四個層次的「以為對了其實壞了」事件，basecamp 整理出共同 pattern，Tim 拍板升級成 cross-session hard rule (寫進 CLAUDE.md)。

## 四個層次的對照

| Layer | 案例 | 「外觀 OK」 | 「真的 OK」 |
|---|---|---|---|
| **Syntactic** | Bash 反引號 | markdown render | bash 在雙引號內當 command substitution 執行 |
| **Identity** | Production vs Legacy | 同名舊版檔案 | production 走另一個路徑 |
| **Status** | run_cmd.py stdout | 印「✓ Success」 | C# 端 fail 後 auto-removed |
| **Content** | Recuva sector 污染 | 檔案存在、大小合理 | 內容是原檔 + 噪音混合體 |

## 核心 framing

每一層的「外觀 OK」對應**該層的 protocol** (render / 路徑名 / stdout 約定 / 檔案 metadata)。但**真實正確性永遠在更深一層** — agent 該主動下探，不能停在第一層 protocol 給的「OK」訊號。

## Agent MUST 動作 (per CLAUDE.md hard rule)

- ✅ 每個 Cmd 跑完 verify 對應 output file 真實結果 (`_last_op.md` etc.), 不能只信 stdout
- ✅ 撞「以為修好的東西其實壞了」場景時, 警覺三層次: 設定層 / 依賴層 / 內容層
- ✅ 撞同類盲點兩次 = pattern 不是巧合, **立即升級成 cross-session rule** (寫進 CLAUDE.md / lesson / glossary)

## 反面案例

- ❌ 跑 commit 看到「commit 成功」就停 — 沒驗證 pre-commit hook 是否真跑、commit 是否真進 git log
- ❌ 跑 work_session.py end 看到「✓ session ended」就停 — 沒看 salary 是否真結算
- ❌ Recuva 還原 site-packages 看到檔案存在就 import — 沒驗證 .py 內容是否完整 (sector 污染)

## 跟其他 framing 的關係

- **Tool-Survey Hard Rule** — 同日另一條 (2026-05-16), 處理「推方案前先確認用戶工具棧」。Tool-Survey 是 Identity layer 的前置 — 先確認對方在哪個技術棧, 再推方案。
- **Recovery Doc 放置 Hard Rule** — 同日另一條, 處理「recovery doc 該入 git 不該放 gitignored」。Identity layer 應用 (gitignored 路徑跟 committed 路徑名字看起來一樣, 但身分不同)。

## 完整 lesson

`AgentCommands/Lessons/lessons.jsonl` 內搜「外觀 OK」/「層次混淆」/「cross-layer」。

## Cross-link

- CLAUDE.md §🩸 跨層次驗證 Hard Rule (專案根)
- `ucl-compile-error` skill — Status layer 應用 (recompile 0 errors ≠ runtime 0 errors)
- 對應 anti-pattern: `appearance-equals-truth` (建議 register, 但本見習生不擅自加 anti_patterns.jsonl, 留 basecamp 拍板)
