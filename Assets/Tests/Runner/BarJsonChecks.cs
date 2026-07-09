// 區塊職責：JsonData 改動的「同步」驗證 harness — 不依賴 NUnit TestRunnerApi 的 async callback
//          (該路徑在「編輯器已開 + Cmd watcher」環境會觸發測試組件重編/domain 擾動而不穩)。
// 物理意義：Cmd_Invoke 同步呼叫 RunChecks() → inline 跑完所有檢查 → 立即寫 Temp/bar_checks.json。
//          涵蓋 A1(byte-identical 反射對拍) / B3(讀取純度) / locale(文化不變) / getter(UInt/ULong)。
// 數值影響：僅寫 Temp/bar_checks.json;純讀 JsonData,不改 runtime 狀態。
using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;
using UnityEngine;
using UCL.Core.JsonLib;

namespace Bar.TestRunnerEditor
{
    public static class BarJsonChecks
    {
        const string OutPath = "Temp/bar_checks.json";

        static readonly MethodInfo s_SerializeValue =
            typeof(JsonData).GetMethod("SerializeValue", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly MethodInfo s_SerializeValueBeautify =
            typeof(JsonData).GetMethod("SerializeValueBeautify", BindingFlags.NonPublic | BindingFlags.Instance);

        static readonly string[] Corpus = new string[]
        {
            "true","false","null","0","-1","123","3.14","-2.5","\"hello\"","\"\"",
            "4000000000","9223372036854775807","18446744073709551615",
            "{}","[]",
            "{\"a\":1,\"b\":[1,2,3],\"c\":{\"d\":true}}",
            "[1,2,3]","42","\"str\"",
            "{\"x\":null,\"y\":1}",
            "{\"k1\":1,\"k2\":2,\"k3\":3,\"k4\":4,\"k5\":5}",
            "\"a\\\"b\\\\c\\nd\\te\"",
            "\"中文テスト\"",
        };

        static int s_Pass, s_Fail;
        static readonly StringBuilder s_Fails = new StringBuilder();

        static void Check(bool cond, string name, string detail = "")
        {
            if (cond) s_Pass++;
            else { s_Fail++; s_Fails.Append(name).Append(detail.Length > 0 ? " (" + detail + ")" : "").Append(" | "); }
        }

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

        // Cmd_Invoke 入口: type=Bar.TestRunnerEditor.BarJsonChecks;member=RunChecks
        public static void RunChecks()
        {
            s_Pass = 0; s_Fail = 0; s_Fails.Clear();
            var orig = Thread.CurrentThread.CurrentCulture;
            try
            {
                CheckReflectionAvailable();
                CheckA1ByteIdentical();
                CheckA1DeepAndLong();
                CheckRoundTripIdempotent();
                CheckB3ReadPurity();
                CheckLocale();
                CheckGetters();
            }
            catch (Exception e)
            {
                s_Fail++;
                s_Fails.Append("HARNESS EXCEPTION: ").Append(e.GetType().Name).Append(": ").Append(e.Message).Append(" | ");
            }
            finally { Thread.CurrentThread.CurrentCulture = orig; }

            var json = "{\"pass\":" + s_Pass + ",\"fail\":" + s_Fail +
                       ",\"fails\":\"" + Esc(s_Fails.ToString()) + "\"}";
            try
            {
                System.IO.Directory.CreateDirectory("Temp");
                System.IO.File.WriteAllText(OutPath, json);
            }
            catch (Exception e) { Debug.LogError("[BarJsonChecks] write failed: " + e); }
            Debug.Log($"[BarJsonChecks] DONE pass={s_Pass} fail={s_Fail}");
        }

        static void CheckReflectionAvailable()
        {
            Check(s_SerializeValue != null, "reflect_SerializeValue_found");
            Check(s_SerializeValueBeautify != null, "reflect_SerializeValueBeautify_found");
        }

        // A1: 新 ToJson / ToJsonBeautify 與 pre-A1 舊路徑輸出 byte-identical。
        static void CheckA1ByteIdentical()
        {
            foreach (var s in Corpus)
            {
                JsonData d = JsonData.ParseJson(s);
                Check(OldToJson(d) == d.ToJson(), "A1_ToJson", s);
                Check(OldToJsonBeautify(d) == d.ToJsonBeautify(), "A1_Beautify", s);
            }
        }

        static void CheckA1DeepAndLong()
        {
            var deep = new StringBuilder();
            for (int i = 0; i < 50; i++) deep.Append('[');
            deep.Append('1');
            for (int i = 0; i < 50; i++) deep.Append(']');
            var dd = JsonData.ParseJson(deep.ToString());
            Check(OldToJson(dd) == dd.ToJson(), "A1_deep50");

            var ls = "\"" + new string('x', 5000) + "\"";
            var dl = JsonData.ParseJson(ls);
            Check(OldToJson(dl) == dl.ToJson(), "A1_longstring");
        }

        static void CheckRoundTripIdempotent()
        {
            foreach (var s in Corpus)
            {
                var once = JsonData.ParseJson(s).ToJson();
                var twice = JsonData.ParseJson(once).ToJson();
                Check(once == twice, "roundtrip_idempotent", s);
            }
        }

        // B3: None 讀 Count 回 0 且不 mutate。
        static void CheckB3ReadPurity()
        {
            var d = new JsonData();
            Check(d.JsonType == JsonType.None, "B3_fresh_is_None");
            Check(d.Count == 0, "B3_None_Count_zero");
            Check(d.JsonType == JsonType.None, "B3_Count_no_mutate");

            var d2 = new JsonData();
            var _ = d2.Count;
            Check(d2.ToJson() != "{}", "B3_no_phantom_empty_obj");

            var dic = JsonData.ParseJson("{\"a\":1,\"b\":2}");
            Check(dic.Count == 2, "B3_dict_count");
            var lst = JsonData.ParseJson("[1,2,3]");
            Check(lst.Count == 3, "B3_list_count");
        }

        // locale: de-DE / fr-FR 下 double 不得存成逗號小數。
        static void CheckLocale()
        {
            foreach (var c in new[] { "de-DE", "fr-FR" })
            {
                var d = JsonData.ParseJson("1.5");
                Thread.CurrentThread.CurrentCulture = new CultureInfo(c);
                var json = d.ToJson();
                Check(!json.Contains(","), "locale_ToJson_no_comma", c + ":" + json);
                Check(json.Contains("1.5"), "locale_ToJson_keeps_1.5", c + ":" + json);
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

                var d2 = JsonData.ParseJson("{\"v\":2.5}");
                Thread.CurrentThread.CurrentCulture = new CultureInfo(c);
                Check(!d2.ToJsonBeautify().Contains("2,5"), "locale_Beautify_no_comma", c);
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            }
        }

        // getter: GetDouble/GetFloat 對 UInt/ULong + 跨型矩陣。
        static void CheckGetters()
        {
            Check(new JsonData((uint)4000000000).GetDouble() == 4000000000d, "GetDouble_UInt");
            Check(new JsonData((ulong)5000000000).GetDouble() == 5000000000d, "GetDouble_ULong");
            Check(new JsonData((ulong)123456).GetFloat() == 123456f, "GetFloat_ULong");
            Check(new JsonData((int)5).GetInt() == 5, "GetInt_Int");
            Check(new JsonData((uint)5).GetInt() == 5, "GetInt_UInt");
            Check(new JsonData((long)5).GetInt() == 5, "GetInt_Long");
            Check(new JsonData((ulong)5).GetInt() == 5, "GetInt_ULong");
            Check(new JsonData("hello").GetInt(-1) == -1, "GetInt_default_on_string");
            Check(new JsonData().GetInt(-1) == -1, "GetInt_default_on_None");
        }

        static string Esc(string s)
        {
            return s.Replace("\\", "\\\\").Replace("\"", "\\\"")
                    .Replace("\r", " ").Replace("\n", " ").Replace("\t", " ");
        }
    }
}
