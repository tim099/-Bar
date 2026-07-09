---
term: Channel Spell (詠唱卡)
aliases: [詠唱, 詠唱卡, channel spell, 2-step cast, channeled cast]
discovered: 2026-05-13 (basecamp QA dogfood, battle_-303652)
canonical_doc: docs/Postmortem/QA_2026-05-13_arcane_blast_chant_no_fire.md §13
related:
  - <repo:docs/Postmortem/QA_2026-05-13_arcane_blast_chant_no_fire.md> | QA Resolution doc | 完整推翻 bug 過程
  - <ucl_core:Skills~/valor-qa-battle/SKILL.md> | valor-qa-battle Skill | 戰鬥 QA 操作 (待加 channel spell 註記)
---

# Channel Spell (詠唱卡) — 2-Step Cast Mechanic

> 一句話: EOV 內某些卡有「**詠唱1回合** prefix」是 **2-step cast** 設計, 不是 bug — 玩第一張 cast 版只進 queue, 下回合自動回 cost-0 finished 版到手牌, **再玩一次** 才真 fire 效果.

---

## 機制描述

### Cast 版 (Step 1)

牌面 prefix 含「**詠唱1回合**」+ 描述效果. 玩這張:
- 扣 mana (e.g. `祕法衝擊波 cost 2`)
- **不立即 fire damage / buff**
- Cast 進入 player 的「詠唱中」state queue
- 同回合多張詠唱卡可並排 cast (X = 本回合使用的牌數量)

### Finished 版 (Step 2)

下回合開場自動補進手牌:
- **cost 0** (重要區別)
- 描述含「**詠唱**」suffix (vs cast 版的 prefix)
- 玩這張 → 真 fire 效果 (damage / buff / etc.)

### 範例 — 多重祕法飛彈

```
Turn N:
  hand 含 [多重祕法飛彈 (cost 2) — 詠唱1回合 重複3次[隨機 8 dmg]]
  player 玩 → mana -2, 卡進詠唱 queue, dmg 沒 apply

Turn N+1 start:
  hand 多 [多重祕法飛彈 (cost 0) — 重複3次[隨機 8 dmg] 詠唱]
  player 玩 → mana -0, dmg 立即 fire (3×8 random distribution)
```

---

## 對遊戲設計的意義

### 戰術 trade-off

- **延遲生效** = 給對手 1 回合反應時間
- **X scaling** (祕法衝擊波類): 同回合多 cast → X 累積 → 下回合 finished 傷害更高
- **手牌占用**: cost-0 finished 版會佔下回合手牌位

### 對 disrupt 卡的互動

敵方某些卡 (如**網縛**) 在 player 詠唱中時加進對方手牌, 可能影響:
- 改 finished 版的 cost (網縛 +1 → finished 變 cost 1)
- 強制玩家額外用 mana 才能 fire

### 對 caster 風格的暗示

詠唱機制鼓勵 mana curve regulation — 不能 burst 全部 mana 在一回合, 要保留節奏給下回合 finished 版.

---

## 跨遊戲設計類比

| 遊戲 | 類似機制 |
|---|---|
| **FF14** | Cast time spell — 詠唱期間可被打斷 (EOV 沒打斷, 是 trade-off |
| **WoW** | Channel spell — 持續吟唱 frame 多 tick (EOV 是離散 turn) |
| **HearthStone** | Some "Forge" or "Echo" cards — 玩後再生成同名 card (相似 generative pattern) |
| **MtG** | Suspended spell (Time Spiral block) — cast 後 N turn 才 resolve (最接近 EOV 機制) |

---

## QA finding 收尾

本詞條的誕生源自 basecamp 2026-05-13 的 dogfood error: 第一次玩詠唱卡看不到 dmg, 誤判為 "詠唱沒 fire = bug", ship 完整 QA finding doc 含 4 root cause hypotheses. 後續同日 dogfood 撞到 cost-0 finished 版, 才意識到這是 2-step mechanic.

**反面教材**:
- dogfood 撞到不符預期, **先窮舉相鄰可能** (新進手牌 / state 變化 / 對應 finished 版) 再 ship bug report
- 未驗證 finding 不該寫成完整 7 章 + 4 hypothesis confirmed doc
- 確認 RCA 前先標 `status: unverified`

詳細推翻過程: [`docs/Postmortem/QA_2026-05-13_arcane_blast_chant_no_fire.md §13 Resolution`](../Postmortem/QA_2026-05-13_arcane_blast_chant_no_fire.md).

---

## 在 valor-qa-battle SKILL 中的暗示

未來 SKILL doc 該補一段「玩詠唱卡的 QA SOP」:
1. 玩 cast 版後 **下回合 check 手牌** 找對應 cost-0 finished 版
2. **不要因為 cast 版立即沒 dmg 就標 bug** — 至少跨 2 turn 觀察
3. 連續 cast 多張詠唱 → X scaling 規律可預測
4. 敵方手有 disrupt 類卡 (網縛 等) → finished 版 cost 可能變動

---

## 持久性

本機制是遊戲原生 design, **不會 patched out**. 任何 QA agent 玩到詠唱卡都該對齊本 doc 的 2-step 理解.
