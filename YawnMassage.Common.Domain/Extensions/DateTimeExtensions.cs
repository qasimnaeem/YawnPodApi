using System;
namespace YawnMassage.Common.Domain.Extensions
{
    /// <summary>
    /// Defines the <see cref="DateTimeExtensions" />
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Defines the epoch
        /// </summary>
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// The FromEpochMilliseconds
        /// </summary>
        /// <param name="epochMilliseconds">The epochMilliseconds<see cref="long"/></param>
        /// <returns>The <see cref="DateTime"/></returns>
        public static DateTime FromEpochMilliseconds(this long epochMilliseconds)
        {
            return epoch.AddMilliseconds(epochMilliseconds);
        }

        /// <summary>
        /// The ToEpochMilliseconds
        /// </summary>
        /// <param name="date">The date<see cref="DateTime"/></param>
        /// <returns>The <see cref="long"/></returns>
        public static long ToEpochMilliseconds(this DateTime date)
        {
            return Convert.ToInt64((date - epoch).TotalMilliseconds);
        }
    }
}
