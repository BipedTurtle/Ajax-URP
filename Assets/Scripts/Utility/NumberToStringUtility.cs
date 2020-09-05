using System;

namespace Utility
{
    public static class NumberToStringUtility
    {
        private static string[] nums = new string[2001];
        private static string[] timespans = new string[60 * 35]; // 35 mintues in total, each of which consists of 60 seconds
        static NumberToStringUtility()
        {
            // plain numbers
            for (int i = 0; i < nums.Length; i++)
                nums[i] = i.ToString();

            // timespan
            for (int i = 0; i < timespans.Length; i++) {
                var timespan = new TimeSpan(0, 0, i);
                var timespanString = timespan.ToString().Substring(startIndex: 3);
                timespans[i] = timespanString;
            }
        }


        public static string GetNumAsString(int num)
        {
            if (num >= nums.Length)
                num = nums.Length - 1;
            else if (num < 0)
                num = 0;

            return nums[num];
        }


        public static string GetTimeBySecond(int totalSeconds)
        {
            if (totalSeconds >= timespans.Length)
                totalSeconds = timespans.Length - 1;
            else if (totalSeconds < 0)
                totalSeconds = 0;

            return timespans[totalSeconds];
        }
    }
}
