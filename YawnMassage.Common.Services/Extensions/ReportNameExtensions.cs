using System;
using System.Diagnostics.CodeAnalysis;

namespace YawnMassage.Common.Services.Extensions
{
    public static class ReportNameExtensions
    {
        [ExcludeFromCodeCoverage]
        public static string ToStandardReportNameFormatByDate(this DateTime date, string namePrefix, string extension)
        {
            return $"{namePrefix}_{date.ToString("dd MM yyyy HH mm ss")}.{extension}".Replace(' ', '_');
        }
    }
}
