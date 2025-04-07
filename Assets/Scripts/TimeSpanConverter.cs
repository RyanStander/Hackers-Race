    using System;

    public static class TimeSpanConverter
    {
        public static TimeSpan ConvertToTimeSpan(float timeFloat)
        {
            int minutes = (int)timeFloat / 60;
            int seconds = (int)timeFloat % 60;
            int milliseconds = (int)((timeFloat - (minutes * 60 + seconds)) * 1000);
            

            return new TimeSpan(0,0, minutes, seconds, milliseconds);
        }

        public static float ConvertToFloat(TimeSpan timeSpan)
        {
            float floatTimeSpan;
            int minutes, seconds, milliseconds;
            seconds = timeSpan.Seconds;
            minutes = timeSpan.Minutes;
            milliseconds = timeSpan.Milliseconds;
            return (float)minutes * 60 + seconds + (float)milliseconds / 1000;
        }
    }
