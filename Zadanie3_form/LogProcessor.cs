using System;
using System.IO;
using LogStandardizer.Services;
using LogStandardizer.Data;

namespace LogStandardizer
{
    public static class LogProcessor
    {
        public static int ProcessFiles(string inputPath, string outputPath, string problemsPath)
        {
            int processedCount = 0;

            using (var reader = new StreamReader(inputPath))
            using (var writer = new StreamWriter(outputPath))
            using (var problemsWriter = new StreamWriter(problemsPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    if (LogParser.TryParseFormat1(line, out var logEntry) ||
                        LogParser.TryParseFormat2(line, out logEntry))
                    {
                        WriteStandardizedLog(writer, logEntry);
                        processedCount++;
                    }
                    else
                    {
                        problemsWriter.WriteLine(line);
                    }
                }
            }

            return processedCount;
        }

        private static void WriteStandardizedLog(StreamWriter writer, LogEntry logEntry)
        {
            writer.WriteLine(LogParser.FormatDate(logEntry.OriginalDate));
            writer.WriteLine(logEntry.Time);
            writer.WriteLine(logEntry.LogLevel);
            writer.WriteLine(logEntry.Method);
            writer.WriteLine(logEntry.Message);
            writer.WriteLine(); 
        }
    }
}