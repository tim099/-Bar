// 區塊職責：A1 (序列化去 ToObject 深拷) 的 byte-identical 對拍測試 — 關 jsondata-opt T04 的驗收證據。
// 物理意義：A1 (commit 54bcee3) 只「新增」SerializeJsonData 並把 ToJson 改呼它；舊私有路徑
//          SerializeValue(ToObject(this)) 原封不動保留在 tip → 其輸出等同 pre-A1 (153b2c4)。
//          用反射在「同一個 build」跑舊路徑 vs 新 ToJson()，逐筆斷言 byte-identical。
//          這是「反射差分」替代 worktree 凍 golden：同源基準、免開第二個 Unity、測的是實際出貨碼。
// 數值影響：純測試；若舊私有方法日後被清除，Reflection_OldPaths_Available 會先紅、提示改凍 golden 檔。
using System.Reflection;
using System.Text;
using NUnit.Framework;
using UCL.Core.JsonLib;

namespace Bar.Tests.EditMode
{
    public class JsonDataSerializeTests
    {
        static readonly MethodInfo s_SerializeValue =
            typeof(JsonData).GetMethod("SerializeValue", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly MethodInfo s_SerializeValueBeautify =
            typeof(JsonData).GetMethod("SerializeValueBeautify", BindingFlags.NonPublic | BindingFlags.Instance);

        // 重建 pre-A1 的 ToJson：SerializeValue(ToObject(this))；ToObject 由公開 GetObj() 取得。
        static string OldToJson(JsonData d)
        {
            var sb = new StringBuilder();
            s_SerializeValue.Invoke(d, new object[] { d.GetObj(), sb });
            return sb.ToString();
        }

        static string OldToJsonBeautify(JsonData d)
        {
            var sb = new StringBuilder();
            s_SerializeValueBeautify.Invoke(d, new object[] { d.GetObj(), sb, 0 });
            return sb.ToString();
        }

        [Test]
        public void Reflection_OldPaths_Available()
        {
            Assert.IsNotNull(s_SerializeValue, "找不到 SerializeValue (pre-A1 舊路徑) — 差分基準失效，需改凍 golden 檔");
            Assert.IsNotNull(s_SerializeValueBeautify, "找不到 SerializeValueBeautify");
        }

        [TestCaseSource(typeof(JsonDataCorpus), nameof(JsonDataCorpus.Samples))]
        public void A1_ToJson_ByteIdentical(string json)
        {
            var d = JsonData.ParseJson(json);
            Assert.AreEqual(OldToJson(d), d.ToJson(),
                "A1 新 ToJson 與 pre-A1 路徑輸出不一致: " + json);
        }

        [TestCaseSource(typeof(JsonDataCorpus), nameof(JsonDataCorpus.Samples))]
        public void A1_ToJsonBeautify_ByteIdentical(string json)
        {
            var d = JsonData.ParseJson(json);
            Assert.AreEqual(OldToJsonBeautify(d), d.ToJsonBeautify(),
                "A1 新 ToJsonBeautify 與 pre-A1 路徑輸出不一致: " + json);
        }

        [Test]
        public void A1_Deep50_ByteIdentical()
        {
            var d = JsonData.ParseJson(JsonDataCorpus.DeepNested(50));
            Assert.AreEqual(OldToJson(d), d.ToJson());
        }

        [Test]
        public void A1_LongString_ByteIdentical()
        {
            var d = JsonData.ParseJson(JsonDataCorpus.LongString());
            Assert.AreEqual(OldToJson(d), d.ToJson());
        }

        // round-trip 冪等：ToJson 再 parse 再 ToJson 應穩定。
        [TestCaseSource(typeof(JsonDataCorpus), nameof(JsonDataCorpus.Samples))]
        public void RoundTrip_Idempotent(string json)
        {
            var once = JsonData.ParseJson(json).ToJson();
            var twice = JsonData.ParseJson(once).ToJson();
            Assert.AreEqual(once, twice, "round-trip 不冪等: " + json);
        }
    }
}
