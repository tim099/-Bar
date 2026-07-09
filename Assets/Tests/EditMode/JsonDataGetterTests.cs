// 區塊職責：數值 getter 跨型強制轉換矩陣 (T03 補 GetDouble/GetFloat 對 UInt/ULong)。
// 物理意義：GetInt/GetUInt/GetLong/GetULong 已支援全 Int/UInt/Long/ULong 矩陣；T03 應補齊
//          GetDouble/GetFloat 對 UInt/ULong 來源。本測試釘死跨型取值正確 + 型別不符回 default。
// 數值影響：純測試；若 GetDouble(UInt/ULong) 仍回 default，代表 T03 未落實 → 測試紅 = 真發現。
using NUnit.Framework;
using UCL.Core.JsonLib;

namespace Bar.Tests.EditMode
{
    public class JsonDataGetterTests
    {
        [Test]
        public void GetDouble_FromUInt()
        {
            var d = new JsonData((uint)4000000000);
            Assert.AreEqual(4000000000d, d.GetDouble(), 0.0, "GetDouble 須支援 UInt (T03)");
        }

        [Test]
        public void GetDouble_FromULong()
        {
            var d = new JsonData((ulong)5000000000);
            Assert.AreEqual(5000000000d, d.GetDouble(), 0.0, "GetDouble 須支援 ULong (T03)");
        }

        [Test]
        public void GetFloat_FromULong()
        {
            var d = new JsonData((ulong)123456);
            Assert.AreEqual(123456f, d.GetFloat(), 0.0f, "GetFloat 須支援 ULong (T03)");
        }

        [Test]
        public void GetInt_CrossTypeMatrix()
        {
            Assert.AreEqual(5, new JsonData((int)5).GetInt(), "Int→Int");
            Assert.AreEqual(5, new JsonData((uint)5).GetInt(), "UInt→Int");
            Assert.AreEqual(5, new JsonData((long)5).GetInt(), "Long→Int");
            Assert.AreEqual(5, new JsonData((ulong)5).GetInt(), "ULong→Int");
        }

        [Test]
        public void GetLong_CrossTypeMatrix()
        {
            Assert.AreEqual(7L, new JsonData((int)7).GetLong(), "Int→Long");
            Assert.AreEqual(7L, new JsonData((uint)7).GetLong(), "UInt→Long");
            Assert.AreEqual(7L, new JsonData((ulong)7).GetLong(), "ULong→Long");
        }

        [Test]
        public void GetInt_DefaultOnMismatch()
        {
            Assert.AreEqual(-1, new JsonData("hello").GetInt(-1), "String→Int 應回 default");
            Assert.AreEqual(-1, new JsonData().GetInt(-1), "None→Int 應回 default");
        }

        [Test]
        public void UInt_Boundary_RoundTrip()
        {
            var d = new JsonData((uint)4000000000);
            var back = JsonData.ParseJson(d.ToJson());
            Assert.AreEqual(4000000000d, back.GetDouble(), 0.0, "大 uint round-trip 不得溢位");
        }
    }
}
