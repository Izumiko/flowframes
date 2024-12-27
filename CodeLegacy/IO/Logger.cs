﻿using Flowframes.IO;
using Flowframes.Ui;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DT = System.DateTime;

namespace Flowframes
{
    class Logger
    {
        public static TextBox textbox;
        static string file;
        public const string defaultLogName = "sessionlog";
        public static long id;

        private const int maxLogSize = 100;
        private static Dictionary<string, List<string>> sessionLogs = new Dictionary<string, List<string>>();
        private static string _lastUi = "";
        public static string LastUiLine { get { return _lastUi; } }
        private static string _lastLog = "";
        public static string LastLogLine { get { return _lastLog; } }

        public struct LogEntry
        {
            public string logMessage;
            public bool hidden;
            public bool replaceLastLine;
            public string filename;

            public LogEntry(string logMessageArg, bool hiddenArg = false, bool replaceLastLineArg = false, string filenameArg = "")
            {
                logMessage = logMessageArg;
                hidden = hiddenArg;
                replaceLastLine = replaceLastLineArg;
                filename = filenameArg;
            }
        }

        private static ConcurrentQueue<LogEntry> logQueue = new ConcurrentQueue<LogEntry>();

        public static void Log(string msg, bool hidden = false, bool replaceLastLine = false, string filename = "")
        {
            logQueue.Enqueue(new LogEntry(msg, hidden, replaceLastLine, filename));
            ShowNext();
        }

        public static void ShowNext()
        {
            LogEntry entry;

            if (logQueue.TryDequeue(out entry))
                Show(entry);
        }

        public static void Show(LogEntry entry, bool logToFile = true)
        {
            //if (entry.logMessage.Contains("frame order info"))
            //    Debugger.Break();

            if (entry.logMessage.IsEmpty())
                return;

            string msg = entry.logMessage;

            if (msg == LastUiLine)
                entry.hidden = true; // Never show the same line twice in UI, but log it to file

            _lastLog = msg;

            if (!entry.hidden)
                _lastUi = msg;

            Console.WriteLine(msg);

            try
            {
                if (!entry.hidden && entry.replaceLastLine)
                {
                    textbox.Invoke(new Action(() => {
                        textbox.Suspend();
                        string[] lines = textbox.Text.SplitIntoLines();
                        textbox.Text = string.Join(Environment.NewLine, lines.Take(lines.Count() - 1).ToArray());
                    }));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
                if (Debugger.IsAttached) Debugger.Break();
            }

            msg = msg.Replace("\n", Environment.NewLine);

            if (!entry.hidden && textbox != null)
                textbox.Invoke(() => textbox.AppendText((textbox.Text.Length > 1 ? Environment.NewLine : "") + msg));

            if (entry.replaceLastLine)
            {
                textbox.Invoke(() => textbox.Resume());
                msg = "[^] " + msg;
            }

            if (!entry.hidden)
                msg = "[UI] " + msg;

            if (logToFile)
            {
                LogToFile(msg, false, entry.filename);
            }
        }

        private const int _maxLogFileWriteAttempts = 10;

        public static void LogToFile(string logStr, bool noLineBreak, string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                filename = defaultLogName;

            if (Path.GetExtension(filename) != ".txt")
                filename = Path.ChangeExtension(filename, "txt");

            file = Path.Combine(Paths.GetLogPath(), filename);
            logStr = logStr.Replace(Environment.NewLine, " ").TrimWhitespaces();
            string time = DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss");

            string appendStr = noLineBreak ? $" {logStr}" : $"{Environment.NewLine}[{id.ToString().PadLeft(8, '0')}] [{time}]: {logStr}";
            List<string> sessionLog = sessionLogs.ContainsKey(filename) ? sessionLogs[filename] : new List<string>();
            sessionLog.Add(appendStr);
            if (sessionLog.Count > maxLogSize)
                sessionLog.RemoveAt(0);
            sessionLogs[filename] = sessionLog;

            for(int attempt = 0; attempt < _maxLogFileWriteAttempts; attempt++)
            {
                try
                {
                    File.AppendAllText(file, appendStr);
                    id++;
                    break;
                }
                catch
                {
                    Console.WriteLine($"Failed to write to log file (attempt {attempt+1}/{_maxLogFileWriteAttempts})");
                }
            }
        }

        public static List<string> GetSessionLog(string filename)
        {
            if (!filename.Contains(".txt"))
                filename = Path.ChangeExtension(filename, "txt");

            if (sessionLogs.ContainsKey(filename))
                return sessionLogs[filename];
            else
                return new List<string>();
        }

        public static List<string> GetSessionLogLastLines(string filename, int linesCount = 5)
        {
            var lines = GetSessionLog(filename);
            return lines.ToArray().Reverse().Take(linesCount).Reverse().ToList();
        }

        public static void LogIfLastLineDoesNotContainMsg(string s, bool hidden = false, bool replaceLastLine = false, string filename = "")
        {
            if (!GetLastLine().Contains(s))
                Log(s, hidden, replaceLastLine, filename);
        }

        public static void WriteToFile(string content, bool append, string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                filename = defaultLogName;

            if (Path.GetExtension(filename) != ".txt")
                filename = Path.ChangeExtension(filename, "txt");

            file = Path.Combine(Paths.GetLogPath(), filename);

            string time = DT.Now.Month + "-" + DT.Now.Day + "-" + DT.Now.Year + " " + DT.Now.Hour + ":" + DT.Now.Minute + ":" + DT.Now.Second;

            try
            {
                if (append)
                    File.AppendAllText(file, Environment.NewLine + time + ":" + Environment.NewLine + content);
                else
                    File.WriteAllText(file, Environment.NewLine + time + ":" + Environment.NewLine + content);
            }
            catch
            {

            }
        }

        public static void ClearLogBox()
        {
            textbox.Text = "";
        }

        public static string GetLastLine(bool includeHidden = false)
        {
            return includeHidden ? _lastLog : _lastUi;
        }

        public static void RemoveLastLine()
        {
            textbox.Text = textbox.Text.Remove(textbox.Text.LastIndexOf(Environment.NewLine));
        }
    }
}
