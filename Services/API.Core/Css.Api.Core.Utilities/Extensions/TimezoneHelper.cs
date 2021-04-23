using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using TimeZoneConverter;

namespace Css.Api.Core.Utilities.Extensions
{
    /// <summary>
    /// A helper class for timezone conversions
    /// </summary>
    public static class TimezoneHelper
    {
        #region Public Methods

        /// <summary>
        /// A helper method to get timezone offset based on input Iana Timezone name
        /// </summary>
        /// <param name="ianaTimezoneAbbreviation"></param>
        /// <returns>UTC offset value. Returns null if invalid</returns>
        public static TimeSpan? GetOffset(string ianaTimezoneAbbreviation)
        {
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            if(isWindows)
            {
                var standardWindowsTime = GetWindowsTimeZoneName(ianaTimezoneAbbreviation);
                if (string.IsNullOrWhiteSpace(standardWindowsTime))
                {
                    return null;
                }
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(standardWindowsTime);
                return timeZoneInfo.GetUtcOffset(DateTime.UtcNow);
            }
            else
            {
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(ianaTimezoneAbbreviation);
                return timeZoneInfo.GetUtcOffset(DateTime.UtcNow);
            }
        }

        /// <summary>
        /// A helper to get corresponding windows timezone for input Iana timezone
        /// </summary>
        /// <param name="timezoneAbbreviation"></param>
        /// <returns>The windows timezone name string. Returns empty string if invalid</returns>
        public static string GetWindowsTimeZoneName(string timezoneAbbreviation)
        {
            string standardWindowsTime;
            try
            {
                standardWindowsTime = TZConvert.IanaToWindows(timezoneAbbreviation);
            }
            catch
            {
                standardWindowsTime = string.Empty;
            }
            return standardWindowsTime;
        }
        #endregion
    }
}
