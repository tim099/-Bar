// 區塊職責：B3 (讀取副作用 / Heisenbug) 讀取純度測試。
// 物理意義：GetCollection() 對 None type 曾回傳會 mutate 的 GetIDic()，導致唯讀讀取 (Count / ICollection.*)
//          偷偷把 None 變成空 Dictionary。summit T02 (153b2c4) 已改 None→null。本測試驗「讀取不改狀態」。
// 數值影響：純測試。ICollection.* 的空集合語義 (b 案 null-guard) 尚待 summit 補 T02b，暫 [Ignore]。
using System.Collections;
using NUnit.Framework;
using UCL.Core.JsonLib;

namespace Bar.Tests.EditMode
{
    public class JsonDataReadPurityTests
    {
        [Test]
        public void None_Count_IsZero_AndDoesNotMutate()
        {
            var d = new JsonData();
            Assert.AreEqual(JsonType.None, d.JsonType, "新建 JsonData 應為 None");
            Assert.AreEqual(0, d.Count, "None.Count 應為 0");
            Assert.AreEqual(JsonType.None, d.JsonType,
                "讀 Count 不得把 None mutate 成 Dictionary (B3 Heisenbug)");
        }

        [Test]
        public void None_ReadCountThenToJson_DoesNotBecomeEmptyObject()
        {
            var d = new JsonData();
            var _ = d.Count;            // 被下毒的讀取
            var json = d.ToJson();
            Assert.AreNotEqual("{}", json,
                "讀 Count 後 None 變成了空 Dict 並序列化成 {} (B3 副作用復現)");
        }

        [Test]
        public void Dictionary_Count_Correct_NoMutate()
        {
            var d = JsonData.ParseJson("{\"a\":1,\"b\":2}");
            Assert.AreEqual(JsonType.Dictionary, d.JsonType);
            Assert.AreEqual(2, d.Count);
            Assert.AreEqual(2, d.Count, "重複讀 Count 應穩定");
        }

        [Test]
        public void List_Count_Correct()
        {
            var d = JsonData.ParseJson("[1,2,3]");
            Assert.AreEqual(3, d.Count);
        }

        // --- 以下驗 (b) null-guard 空集合語義：等 summit 補 T02b guard 後移除 [Ignore] ---
        [Test]
        [Ignore("等 summit 補 B3 (b) null-guard T02b: None 的 ICollection.* 應為空集合語義而非 NRE")]
        public void None_ICollection_IsSynchronized_False()
        {
            var d = new JsonData();
            Assert.IsFalse(((ICollection)d).IsSynchronized);
            Assert.AreEqual(JsonType.None, d.JsonType, "讀 IsSynchronized 不得 mutate None");
        }

        [Test]
        [Ignore("等 summit 補 B3 (b) null-guard T02b: CopyTo 對 None 應為 no-op")]
        public void None_ICollection_CopyTo_NoOp()
        {
            var d = new JsonData();
            var arr = new object[0];
            Assert.DoesNotThrow(() => ((ICollection)d).CopyTo(arr, 0));
            Assert.AreEqual(JsonType.None, d.JsonType);
        }
    }
}
