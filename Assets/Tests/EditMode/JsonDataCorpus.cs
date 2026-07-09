// 區塊職責：JsonData 測試共用語料 (corpus)
// 物理意義：涵蓋 summit 定案的 10 類基礎 + basecamp 補的邊界樣本，供 round-trip / 差分測試重用。
// 數值影響：純測試資料，無 runtime 影響。
using System.Text;

namespace Bar.Tests.EditMode
{
    public static class JsonDataCorpus
    {
        // 良構 JSON 字串樣本 (TestCaseSource 逐筆餵)。
        public static readonly string[] Samples = new string[]
        {
            // --- scalar ---
            "true", "false", "null",
            "0", "-1", "123", "3.14", "-2.5",
            "\"hello\"", "\"\"",
            // --- 整數跨型 (逼 UInt / Long) ---
            "4000000000",
            // --- 大數邊界 ---
            "9223372036854775807",   // long.MaxValue
            "18446744073709551615",  // ulong.MaxValue
            // --- 空容器 ---
            "{}", "[]",
            // --- 巢狀 ---
            "{\"a\":1,\"b\":[1,2,3],\"c\":{\"d\":true}}",
            // --- 頂層非 dict ---
            "[1,2,3]", "42", "\"str\"",
            // --- null 值 ---
            "{\"x\":null,\"y\":1}",
            // --- 鍵序不變式 (多鍵, 攔 m_Dic→m_ObjectList 偷換) ---
            "{\"k1\":1,\"k2\":2,\"k3\":3,\"k4\":4,\"k5\":5}",
            // --- 轉義全分支 ---
            "\"a\\\"b\\\\c\\nd\\te\\r\\f\\b\"",
            // --- unicode (raw) ---
            "\"中文テスト\"",
            // --- unicode escape ---
            "\"\\u00e9\\u4e2d\"",
        };

        // 深巢狀 (預設 50 層) — 回傳 JSON 字串。
        public static string DeepNested(int depth)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < depth; i++) sb.Append('[');
            sb.Append('1');
            for (int i = 0; i < depth; i++) sb.Append(']');
            return sb.ToString();
        }

        // 超長字串樣本。
        public static string LongString()
        {
            return "\"" + new string('x', 5000) + "\"";
        }
    }
}
