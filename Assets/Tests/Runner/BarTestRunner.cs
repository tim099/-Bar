// 區塊職責：以 TestRunnerApi 在「已開啟的編輯器內」跑 EditMode 測試，把結果寫成檔案供外部 poll。
// 物理意義：專案已開，無法用第二個 -batchmode -runTests 實例；改用 Cmd_Invoke 反射呼叫本靜態方法觸發
//          TestRunnerApi.Execute(EditMode)。執行為非同步，RunFinished callback 收斂結果 → 寫 Temp/bar_test_*.
// 數值影響：僅寫 Temp/ 暫存檔 (bar_test_results.json / bar_test_done.flag)，不影響 runtime / 版控。
using System;
using System.IO;
using System.Text;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

namespace Bar.TestRunnerEditor
{
    public static class BarTestRunner
    {
        const string ResultPath = "Temp/bar_test_results.json";
        const string DonePath = "Temp/bar_test_done.flag";

        // ★ TestRunnerApi 坑：api / callbacks 實例必須用 static 欄位持有，否則 RunEditMode 回傳後被 GC，
        //   RunFinished callback 永遠不觸發。
        static TestRunnerApi s_Api;
        static Callbacks s_Callbacks;

        // Cmd_Invoke 入口: type=Bar.TestRunnerEditor.BarTestRunner;member=RunEditMode
        public static void RunEditMode()
        {
            try { if (File.Exists(DonePath)) File.Delete(DonePath); } catch { }
            try { if (File.Exists(ResultPath)) File.Delete(ResultPath); } catch { }
            s_Api = ScriptableObject.CreateInstance<TestRunnerApi>();
            s_Callbacks = new Callbacks();
            s_Api.RegisterCallbacks(s_Callbacks);
            var filter = new Filter { testMode = TestMode.EditMode };
            s_Api.Execute(new ExecutionSettings(filter));
            Debug.Log("[BarTestRunner] EditMode test run kicked off...");
        }

        class Callbacks : ICallbacks
        {
            public void RunStarted(ITestAdaptor testsToRun) { }
            public void TestStarted(ITestAdaptor test) { }
            public void TestFinished(ITestResultAdaptor result) { }

            public void RunFinished(ITestResultAdaptor result)
            {
                int passed = 0, failed = 0, skipped = 0;
                var failures = new StringBuilder();
                Collect(result, ref passed, ref failed, ref skipped, failures);

                var sb = new StringBuilder();
                sb.Append("{");
                sb.Append("\"passed\":").Append(passed).Append(",");
                sb.Append("\"failed\":").Append(failed).Append(",");
                sb.Append("\"skipped\":").Append(skipped).Append(",");
                sb.Append("\"failures\":\"").Append(Esc(failures.ToString())).Append("\"");
                sb.Append("}");

                try
                {
                    Directory.CreateDirectory("Temp");
                    File.WriteAllText(ResultPath, sb.ToString());
                    File.WriteAllText(DonePath, DateTime.UtcNow.ToString("o"));
                }
                catch (Exception e) { Debug.LogError("[BarTestRunner] write result failed: " + e); }

                Debug.Log($"[BarTestRunner] DONE passed={passed} failed={failed} skipped={skipped}");
            }

            void Collect(ITestResultAdaptor r, ref int passed, ref int failed, ref int skipped, StringBuilder failures)
            {
                if (r.HasChildren)
                {
                    foreach (var c in r.Children) Collect(c, ref passed, ref failed, ref skipped, failures);
                    return;
                }
                switch (r.TestStatus)
                {
                    case TestStatus.Passed: passed++; break;
                    case TestStatus.Failed:
                        failed++;
                        failures.Append(r.FullName).Append(" :: ").Append(r.Message).Append(" | ");
                        break;
                    default: skipped++; break; // Skipped / Inconclusive
                }
            }

            static string Esc(string s)
            {
                return s.Replace("\\", "\\\\").Replace("\"", "\\\"")
                        .Replace("\r", " ").Replace("\n", " ").Replace("\t", " ");
            }
        }
    }
}
