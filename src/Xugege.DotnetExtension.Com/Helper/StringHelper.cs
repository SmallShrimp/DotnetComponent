using System;

namespace Xugege.DotnetExtension.Com.Helper
{
    public class StringHelper
    {
        public static string CreateIdByTime()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();

        }

        public static string CreateIdByGuidAndTime(int count, char split = '-')
        {
            string result = string.Empty;
            for (int i = 0; i < count; i++)
            {
                if (i == count - 1) result += CreateIdByGuidAndTime();
                else result += CreateIdByGuidAndTime() + split;
            }
            return result;
        }

        public static string CreateIdByGuidAndTime()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        public static long CreateId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }
    }
}
