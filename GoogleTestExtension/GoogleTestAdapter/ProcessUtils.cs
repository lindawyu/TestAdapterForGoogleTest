﻿using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace GoogleTestAdapter
{
    public static class ProcessUtils
    {

        /*
                    if (runContext.IsBeingDebugged)
            {
                handle.SendMessage(TestMessageLevel.Informational, "Attaching debugger to " + executable);
                Process.GetProcessById(handle.LaunchProcessWithDebuggerAttached(executable, WorkingDir, Arguments, null)).WaitForExit();
    }
            else
            {
                handle.SendMessage(TestMessageLevel.Informational, "In " + WorkingDir + ", running: " + executable + " " + Arguments);
                consoleOutput = ProcessUtils.GetOutputOfCommand(handle, WorkingDir, executable, Arguments);
            }
            */


        public static List<string> GetOutputOfCommand(IMessageLogger logger, string workingDirectory, string command, string param, bool printTestOutput, bool throwIfError)
        {
            List<string> output = new List<string>();
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(command, param)
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = false,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = workingDirectory
                };
                Process process = Process.Start(processStartInfo);
                try
                {
                    if (printTestOutput)
                    {
                        logger.SendMessage(TestMessageLevel.Informational, ">>>>>>>>>>>>>>> Output of command '" + command + " " + param + "'");
                    }
                    ReadTheStream(throwIfError, process, output, logger, printTestOutput);
                    if (printTestOutput)
                    {
                        logger.SendMessage(TestMessageLevel.Informational, "<<<<<<<<<<<<<<< End of Output");
                    }
                }
                finally
                {
                    process?.Dispose();
                }
            }
            catch (Win32Exception e)
            {
                logger.SendMessage(TestMessageLevel.Error, "Error occured during process start, message: " + e);
            }

            return output;
        }

        // ReSharper disable once UnusedParameter.Local
        private static void ReadTheStream(bool throwIfError, Process process, List<string> streamContent, IMessageLogger logger, bool printTestOutput)
        {
            while (!process.StandardOutput.EndOfStream)
            {
                string Line = process.StandardOutput.ReadLine();
                streamContent.Add(Line);
                if (printTestOutput)
                {
                    logger.SendMessage(TestMessageLevel.Informational, Line);
                }
            }
            if ((throwIfError && process.ExitCode != 0))
            {
                throw new Exception("Process exited with return code " + process.ExitCode);
            }
        }

    }

}