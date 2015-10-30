﻿using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace GoogleTestAdapter.Helpers
{

    public class TestEnvironment
    {

        public enum LogType { Normal, UserDebug, Debug }


        // for developing and testing the test adapter itself
        private static bool DebugMode = false;
        private static bool UnitTestMode = false;

        private static bool AlreadyAskedForDebugger { get; set; } = false;

        private static readonly object Lock = new object();


        public AbstractOptions Options { get; }
        private IMessageLogger Logger { get; }


        public TestEnvironment(AbstractOptions options, IMessageLogger logger)
        {
            this.Options = options;
            this.Logger = logger;
        }


        public void LogInfo(string message, LogType logType = LogType.Normal)
        {
            Log(message, logType, TestMessageLevel.Informational, "");
        }

        public void LogWarning(string message, LogType logType = LogType.Normal)
        {
            Log(message, logType, TestMessageLevel.Warning, "Warning: ");
        }

        public void LogError(string message, LogType logType = LogType.Normal)
        {
            Log(message, logType, TestMessageLevel.Error, "ERROR: ");
        }

        public void CheckDebugModeForExecutionCode()
        {
            CheckDebugMode("execution");
        }

        public void CheckDebugModeForDiscoveryCode()
        {
            CheckDebugMode("discovery");
        }


        private void Log(string message, LogType logType, TestMessageLevel level, string prefix)
        {
            bool log;
            switch (logType)
            {
                case LogType.Normal:
                    log = true;
                    break;
                case LogType.Debug:
                    log = DebugMode;
                    break;
                case LogType.UserDebug:
                    log = Options.UserDebugMode;
                    break;
                default:
                    throw new Exception("Unknown LogType: " + logType);
            }

            if (log)
            {
                lock (Lock)
                {
                    SafeLogMessage(level, prefix + message);
                }
            }
        }

        private void SafeLogMessage(TestMessageLevel level, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = " ";
            }
            Logger.SendMessage(level, message);
        }

        private void CheckDebugMode(string taskType)
        {
            string processName = Process.GetCurrentProcess().MainModule.ModuleName;
            int processId = Process.GetCurrentProcess().Id;

            LogInfo($"Test {taskType} is running in the process '{processName}' with id {processId}.", LogType.Debug);
            if (DebugMode && !UnitTestMode && !Debugger.IsAttached && !AlreadyAskedForDebugger)
            {
                AlreadyAskedForDebugger = true;

                string title = $"Attach debugger to test {taskType}?";
                string message = $"Test {taskType} is spawning a new process."
                    + "\nDo you want to attach the debugger?"
                    + "\n"
                    + "\nNote: if Visual Studio is already attached to some other process,"
                    + " you cannot select it as debugger in the following dialog."
                    + " In order to debug multiple processes at the same time please"
                    + $" attach manually to the process '{processName}' with id {processId}."
                    + "\n"
                    + "\nTip: Starting the Visual Studio experimental instance with 'Debug/Start"
                    + " Debugging (F5)' will attach the debugger to 'devenv.exe' and you cannot"
                    + " automatically attach to test discovery or execution. Use 'Debug/Start"
                    + " Without Debugging (Ctrl+F5)' instead and be happy.";

                DialogResult result = MessageBox.Show(message, title,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                    Debugger.Launch();
            }
        }

    }

}