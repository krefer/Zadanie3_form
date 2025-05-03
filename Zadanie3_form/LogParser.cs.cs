using System;
using System.Globalization;
using System.Text.RegularExpressions;
using LogStandardizer.Data;

namespace LogStandardizer.Services
{
    public static class LogParser
    {
        private static readonly Regex Format1Regex = new Regex(
            @"^(\d{2}\.\d{2}\.\d{4})\s(\d{2}:\d{2}:\d{2}\.\d{3})\s+(\w+)\s+(.+)$",
            RegexOptions.Compiled);

        private static readonly Regex Format2Regex = new Regex(
            @"^(\d{4}-\d{2}-\d{2})\s(\d{2}:\d{2}:\d{2}\.\d+)\|\s*(\w+)\|\d+\|([^|]*)\|(.+)$",
            RegexOptions.Compiled);

        public static bool TryParseFormat1(string line, out LogEntry logEntry)
        {
            var match = Format1Regex.Match(line);
            if (!match.Success)
            {
                logEntry = null;
                return false;
            }

            logEntry = new LogEntry
            {
                OriginalDate = match.Groups[1].Value,
                Time = match.Groups[2].Value,
                LogLevel = NormalizeLogLevel(match.Groups[3].Value),
                Method = "DEFAULT",
                Message = match.Groups[4].Value.Trim()
            };
            return true;
        }

        public static bool TryParseFormat2(string line, out LogEntry logEntry)
        {
            var match = Format2Regex.Match(line);
            if (!match.Success)
            {
                logEntry = null;
                return false;
            }

            logEntry = new LogEntry
            {
                OriginalDate = match.Groups[1].Value,
                Time = match.Groups[2].Value,
                LogLevel = NormalizeLogLevel(match.Groups[3].Value),
                Method = string.IsNullOrWhiteSpace(match.Groups[4].Value)
                    ? "DEFAULT"
                    : match.Groups[4].Value.Trim(),
                Message = match.Groups[5].Value.Trim()
            };
            return true;
        }

        public static string NormalizeLogLevel(string level)
        {
            return level.ToUpper() switch
            {
                "INFORMATION" => "INFO",
                "WARNING" => "WARN",
                _ => level.ToUpper()
            };
        }

        public static string FormatDate(string originalDate)
        {
            if (DateTime.TryParseExact(originalDate, "dd.MM.yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                return date.ToString("dd-MM-yyyy");
            }

            if (DateTime.TryParseExact(originalDate, "yyyy-MM-dd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return date.ToString("dd-MM-yyyy");
            }

            return "INVALID_DATE";
        }
    }
}
