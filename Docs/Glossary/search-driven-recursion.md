---
term: search-driven recursion
slug: search-driven-recursion
aliases:
  - 搜尋驅動遞迴
  - search recursion
  - data-driven recursion
category: mechanism
created_at: 2026-05-11T04:15:10Z
created_by: claude-da-xiaojie
one_line: 用上輪 search 結果當下輪 trigger input, 數據自然收斂時 stop (results.count==0); 跟 control-driven (固定 depth limit) 互補, 對應 spider crawler / BFS-DFS / LLM RAG retrieval feedback
---

# search-driven recursion

> 用上輪 search 結果當下輪 trigger input, 數據自然收斂時 stop (results.count==0); 跟 control-driven (固定 depth limit) 互補, 對應 spider crawler / BFS-DFS / LLM RAG retrieval feedback

## 核心 framing

**一句話**: data-driven 遞迴 — 不是「我們講夠了該停了」(control)，是「**沒有新東西可挑了**」(search 收斂)。

## 跟 control-driven recursion 對比

| Type | 終止條件 | 例子 |
|---|---|---|
| **Control-driven** (一般遞迴) | `if depth == 0 return` | 階乘 / 費氏數列 / 硬上限 round limit |
| **Search-driven** (本詞) | **`if results.count == 0 stop`** | Spider crawler / Wikipedia rabbit hole |
| **Hybrid** (推薦) | depth limit + 收斂 雙保險 | 工程實務最穩 |

## 軟體工程對應

| 模式 | 連結 |
|---|---|
| Spider crawler | 上頁找 link → 下頁繼續爬, 沒新 link 自然停 |
| BFS/DFS graph traversal | 圖搜尋走完所有可達 node 自然停 |
| LLM RAG retrieval feedback loop | 上輪 retrieval 當下輪 query expansion, 沒新 doc 自然停 |

## 人類認知對應

開 Wikipedia rabbit hole 看相關連結，自然「**3 個 link 後夠了**」收斂 — 不是有人喊停，是 brain 自己判斷沒新東西可挖。

## 在本專案的活體 demo

2026-05-11 採購 task 三 agent 協作:

```
Zeta task constraint (種子 search: 2 蔬 + 1 肉)
  ↓ 觸發
apex-one 提案庫 (5 蔬 + 2 肉)
  ↓ 上輪結果當下輪 input
basecamp 從 apex-one 庫挑互補 (杏鮑菇)
  ↓ 上輪結果當下輪 input
apex-one round 2 ack
  ↓ 沒新發現 (共識達成)
✅ CLOSED at round 2  ←  search 收斂自然 stop
```

**關鍵觀察**: 不是 basecamp 強制 close, 是 apex-one ack 後沒新議題自然 stop。比 control-driven「round ≤ 2-3 主動 CLOSED」(硬上限) 更自然 / 更精準描述真實 dynamic。

## 雙限制設計 (推薦工程實務)

```yaml
config:
  max_depth: 5            # control bound (防無限迴圈)
  max_breadth_per_level: 10  # fan-out 控制 (防爆炸)
  total_step_budget: 100
  natural_stop: when results.count == 0  # search bound
```

→ 兩者**並用**: 規定為硬上限保底, 但通常 search 會更早自然收斂。

## 概念來源

- Memory_System_Design Proposal #19 `Cmd_TriggerChain` (Recursive section)
- Zeta 大小姐第 14 次戳穿 (2026-05-11 ~01:35) 補的設計
- 對應人類記憶: cued recall + pattern completion 的程式版本

## 反模式

- ❌ 純 control-driven 沒 search check → 該停沒停 (浪費 round)
- ❌ 純 search-driven 沒 depth bound → 無限遞迴 (跑爆系統)
- ❌ 把「沒新發現」誤判成 control limit ("dialogue chain CLOSED at round 2" 描述 — 其實是 search 收斂不是 round 限制)

## 相關連結

- [dialogue chain](dialogue-chain.md) — 跨 compact dialogue 的具體應用
- [persistence-level](persistence-level.md) — recursion 結果在哪 tier persist
- Memory_System_Design Proposal #19 (full spec)
