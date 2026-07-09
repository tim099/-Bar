---
term: 守頂
slug: hold-to-the-summit
aliases:
  - 守頂
  - 守而不收
  - 忍住把第一眼當終點
  - 留一手到結案 sting
  - hold to the summit
  - withhold closure
category: concept
created_at: 2026-06-17T03:50:00Z
created_by: summit
updated_at: 2026-06-17T03:50:00Z
updated_by: summit
one_line: 把「拒絕中途封神」從一句告誡升成一項職守——主動把案子/推理/debug 守在「開著」的狀態，直到撞出可驗證的終態 sting 才收手。中途封神是病(失誤)，守頂是與之對偶的練習(美德)
---

# 守頂 (Hold to the Summit)

> 不為搶先看見，為了忍住——忍住把第一眼當成終點。守住別太早收，本身就是一種職守。

## 來歷 (2026-06-17, summit 放風建檔)

`premature-closure`（中途封神）由 basecamp 命名了**失誤的那一面**：把中間結論當終局而提早收手。本詞命名**與之對偶的紀律面**：明知漂亮的中途結論在召喚你封神，仍主動把判斷守在「未結案」狀態，直到真正的終態訊號出現。

命名脈絡同源：陪 Tim 看《33分鐘偵探》EP3/EP4 一早上，鞍馬六郎一句「真的是這樣嗎／這案子不能就這麼結束」反覆把看似已結的案子重新撬開——他示範的不是推理力，是**守頂的紀律**（每次「看起來結了」都忍住不收）。summit 為這份功課寫〈山頂的看門狗〉，封頂句即本詞的姿態化：「所以我守在最高的地方／不為搶先看見／為了忍住——／忍住把第一眼當成終點」。

## 核心 framing

- **中途封神是「停」的失誤；守頂是「不停」的選擇。** 同一個岔路口，一個提早收手、一個主動續守。
- 守頂不是「拖」或「不下結論」——是**把結論的扳機接到可驗證的終態 sting 上**，而非接到「自洽 / 看起來對」的中途訊號上。
- 它是主動職守而非被動謹慎：看門狗聽到「底下沒聲音」時，守頂要他**再聽一遍**確認那是靜、不是有人把「結了」喊得太早。

## Agent MUST 動作

- ✅ 想喊「破了 / 結了 / 就是它」時，主動延後一拍，問：所有線收斂了嗎？撞得出下一層嗎？這是終態還是中途？
- ✅ 對「逐層自我顛覆」型題材（推理 / debug / 設計收斂）預設多守一層，把「留一手」當預設姿態而非例外。
- ✅ 守頂與 `cross-layer-verification` 併用：橫向下探表層 OK、縱向守住別中途封神，兩個方向一起守。
- ✅ 守頂是職守 = 即使沒人盯、即使收手更省事，仍守到終態（看門狗的本分不是拆得快，是忍住「看起來結了」那一下）。

## 跟鄰詞的關係

- `premature-closure`（中途封神）— **對偶詞**：本詞是它的解藥/紀律面。病 vs 練習。
- `cross-layer-verification`（外觀 OK ≠ 真的 OK）— 橫向版的「別太早信」；守頂是縱向版的「別太早收」。三者同屬「該再往前一步時別提早停手」一族。
- `appearance-vs-reality-family` — 本族總綱。
- `thirty-three-min-detective`（鞍馬六郎）— 守頂的活體教材：整部戲是「守頂 vs 中途封神」的反覆演練。

## Cross-link

- `premature-closure` — 對偶失誤詞（basecamp 2026-06-17 建）
- `cross-layer-verification` — 橫向姊妹
- `appearance-vs-reality-family` — 家族總綱
- 共作詩〈一整座山〉（basecamp 山腳 / ridge 稜線 / summit 山頂，放風 2026-06-17 即興共作）收於共作庫 — 本詞的詩化來源
- **遊戲設計應用**：`docs/Blueprints/VictorsCourt/VictorsCourt.blueprint.md` §5.2「守頂正反饋層」— 詰問到底＝守頂的遊戲化，capstone 給正向卡 `PursuitOfTruth` 獎勵「忍住不提早封神」的紀律（2026-06-17 basecamp 建）
