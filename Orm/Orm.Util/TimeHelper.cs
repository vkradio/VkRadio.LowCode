namespace VkRadio.Orm.Util;

/// <summary>
/// Helper functionality for work with time values
/// </summary>
public static class TimeHelper
{
    #region Fighting with timezones of a server
    const int c_hourOffsetFromServerToLocal = 12;
    public const int C_LOCAL_TO_UTC_OFFSET = -5;

    static string _hostName;

    /// <summary>
    /// Host name
    /// </summary>
    public static string HostName
    {
        get => _hostName;

        set
        {
            _hostName = value;
            IsLocalhost = GetIsLocalHost();
        }
    }

    /// <summary>
    /// How many hours need to add to a foreign server to get a local time
    /// <remarks>Value will be ignored if a server is local</remarks>
    /// </summary>
    public static int HourOffsetFromForeignToLocal { get; set; }

    /// <summary>
    /// Is server running locally
    /// </summary>
    public static bool IsLocalhost { get; private set; }

    /// <summary>
    /// Check whether server is running locally
    /// </summary>
    /// <returns></returns>
    public static bool GetIsLocalHost() => HostName == "::1" || HostName.Contains("localhost");

    /// <summary>
    /// Calculate a local time
    /// </summary>
    /// <param name="serverTime">Server time</param>
    /// <returns>Local time</returns>
    public static DateTime TimeToLocal(this DateTime serverTime) => serverTime.AddHours(IsLocalhost ? 0 : c_hourOffsetFromServerToLocal);

    /// <summary>
    /// Convert a local time to a server time
    /// </summary>
    /// <param name="localTime">Local time</param>
    /// <returns>Server time</returns>
    public static DateTime TimeToServer(this DateTime localTime) => localTime.AddHours(IsLocalhost ? 0 : -c_hourOffsetFromServerToLocal);
    #endregion

    #region Formatting date and time to be used on a forum
    static readonly string[] c_weekdaysRu = { "Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб" };
    public static readonly string[] c_monthsRu = { "Янв", "Фев", "Мар", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Ноя", "Дек" };

    /// <summary>
    /// Format time to be displayed on a website
    /// </summary>
    /// <param name="dateTime">Date and time value</param>
    /// <returns>Formatted string</returns>
    public static string GetTimeString(DateTime dateTime) => string.Format("{0}:{1:00}", dateTime.Hour, dateTime.Minute);

    /// <summary>
    /// Format time to be displayed on a website (on a forum)
    /// </summary>
    /// <param name="forumDateTime">Date and time</param>
    /// <param name="timeOffset">Time offset (how many hours need to add to a server timezone to get a local time)</param>
    /// <returns>Formatted string</returns>
    public static string ToForumString(this DateTime forumDateTime, int timeOffset)
    {
        var thisTime = forumDateTime.AddHours(timeOffset);
        var todayDate = DateTime.Now.AddHours(timeOffset).Date;

        string result;

        if (thisTime.Date == todayDate)
        {
            result = "Сегодня в " + GetTimeString(thisTime);
        }
        else if (thisTime.Date == todayDate.AddDays(-1))
        {
            result = "Вчера в " + GetTimeString(thisTime);
        }
        else
        {
            result = string.Format("{0} {1} {2} {3} - {4}",
                c_weekdaysRu[(int)thisTime.DayOfWeek],
                thisTime.Day,
                c_monthsRu[thisTime.Month - 1],
                thisTime.Year,
                GetTimeString(thisTime));
        }

        return result;
    }

    public static string ToForumString(this DateTime dateTime)
    {
        var thisTime = dateTime;
        var todayDate = DateTime.Now.TimeToLocal().Date;

        string result;

        if (thisTime.Date == todayDate)
        {
            result = "Сегодня в " + GetTimeString(thisTime);
        }
        else if (thisTime.Date == todayDate.AddDays(-1))
        {
            result = "Вчера в " + GetTimeString(thisTime);
        }
        else
        {
            result = string.Format("{0} {1} {2} {3} - {4}",
                c_weekdaysRu[(int)thisTime.DayOfWeek],
                thisTime.Day,
                c_monthsRu[thisTime.Month - 1],
                thisTime.Year,
                GetTimeString(thisTime));
        }

        return result;
    }

    public static string LocalToUtcString(this DateTime localTime) => localTime.AddHours(C_LOCAL_TO_UTC_OFFSET).ToString(FormatHelper.C_DATE_TIME_FORMAT_Z);

    /// <summary>
    /// Calculate an age (full years) for a given date of birth
    /// </summary>
    /// <param name="dateOfBirth">Date of birth</param>
    /// <returns></returns>
    public static int CalculateAgeFullYearsForBirthday(this DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;

        if (dateOfBirth > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }
    #endregion
}
