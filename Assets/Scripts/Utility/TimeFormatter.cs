using UnityEngine;

public static class TimeFormatter
{
    /// <summary>
    /// 초를 MM:SS 형식의 문자열로 변환
    /// </summary>
    public static string ToMinuteSecondFormat(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60f);
        int seconds = Mathf.FloorToInt(totalSeconds % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
    
    /// <summary>
    /// 초를 HH:MM:SS 형식의 문자열로 변환
    /// </summary>
    public static string ToHourMinuteSecondFormat(float totalSeconds)
    {
        int hours = Mathf.FloorToInt(totalSeconds / 3600f);
        int minutes = Mathf.FloorToInt((totalSeconds % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(totalSeconds % 60f);
        
        if (hours > 0)
            return $"{hours:00}:{minutes:00}:{seconds:00}";
        else
            return $"{minutes:00}:{seconds:00}";
    }
    
    /// <summary>
    /// 초를 분과 초로 분리하여 반환
    /// </summary>
    public static (int minutes, int seconds) ToMinuteSecond(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60f);
        int seconds = Mathf.FloorToInt(totalSeconds % 60f);
        return (minutes, seconds);
    }
} 