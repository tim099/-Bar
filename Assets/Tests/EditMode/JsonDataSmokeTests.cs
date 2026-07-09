// 區塊職責：測試組件煙霧測試 — 證明 Bar.Tests.EditMode 編得過、Test Runner 跑得起來、能存取 UCL_Core 的 JsonData。
using NUnit.Framework;
using UCL.Core.JsonLib;

namespace Bar.Tests.EditMode
{
    public class JsonDataSmokeTests
    {
        [Test]
        public void AssemblyWiredUp()
        {
            Assert.Pass("Bar.Tests.EditMode 組件編譯 + Test Runner 執行 OK。");
        }

        [Test]
        public void Parse_And_Accessors_Basic()
        {
            var d = JsonData.ParseJson("{\"hello\":\"world\",\"n\":42}");
            Assert.AreEqual(JsonType.Dictionary, d.JsonType);
            Assert.AreEqual("world", d.GetString("hello"));
            Assert.AreEqual(42, d.GetInt("n"));
        }
    }
}
