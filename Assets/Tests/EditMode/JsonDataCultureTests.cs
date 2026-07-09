// 區塊職責：locale / 文化不變式測試 (T01 InvariantCulture 修復)。
// 物理意義：逗號小數語系 (de-DE / fr-FR) 下，double 序列化若走 CurrentCulture 會存出 "1,5" 非法 JSON。
//          T01 已把 ToJson/ToJsonBeautify 全走 InvariantCulture。本測試釘死「換語系輸出不變」。
// 數值影響：純測試；SetUp/TearDown 存還原 CurrentCulture 避免污染其他測試。
using System.Globalization;
using System.Threading;
using NUnit.Framework;
using UCL.Core.JsonLib;

namespace Bar.Tests.EditMode
{
    public class JsonDataCultureTests
    {
        CultureInfo _orig;

        [SetUp] public void SetUp() { _orig = Thread.CurrentThread.CurrentCulture; }
        [TearDown] public void TearDown() { Thread.CurrentThread.CurrentCulture = _orig; }

        [TestCase("de-DE")]
        [TestCase("fr-FR")]
        public void Serialize_Double_UsesInvariantDecimalPoint(string culture)
        {
            // 先在預設語系 parse，再切語系序列化 → 隔離出「序列化端」的 locale 行為 (T01 修的那半)。
            var d = JsonData.ParseJson("1.5");
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            var json = d.ToJson();
            Assert.IsFalse(json.Contains(","),
                "在 " + culture + " 下不得輸出逗號小數 (非法 JSON): " + json);
            StringAssert.Contains("1.5", json, "應保留 1.5");
        }

        [TestCase("de-DE")]
        [TestCase("fr-FR")]
        public void RoundTrip_CultureInvariant(string culture)
        {
            var baseline = JsonData.ParseJson("{\"pi\":3.14159,\"e\":2.71828}").ToJson();
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            var underCulture = JsonData.ParseJson("{\"pi\":3.14159,\"e\":2.71828}").ToJson();
            Assert.AreEqual(baseline, underCulture, "序列化必須文化不變");
        }

        [TestCase("de-DE")]
        [TestCase("fr-FR")]
        public void Beautify_Double_UsesInvariantDecimalPoint(string culture)
        {
            var d = JsonData.ParseJson("{\"v\":2.5}");
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            var json = d.ToJsonBeautify();
            Assert.IsFalse(json.Contains("2,5"),
                "ToJsonBeautify 在 " + culture + " 下不得輸出逗號小數: " + json);
        }
    }
}
