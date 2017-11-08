using System;
using System.Collections.Generic;
using System.Text;

namespace Xugege.DotnetExtension.Com.Extension
{
    public static class StringExtension
    {

        public static bool IsNullOrEmpty(this String target)
        {
            return String.IsNullOrEmpty(target);
        }

    }
}
