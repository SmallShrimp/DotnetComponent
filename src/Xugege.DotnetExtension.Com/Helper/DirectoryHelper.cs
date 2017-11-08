using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Xugege.DotnetExtension.Com.Helper
{
    public class DirectoryHelper
    {
        public static void CreateIfNotExists(string path)
        {

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
