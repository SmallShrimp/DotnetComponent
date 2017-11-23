using System;
using Xugege.DotnetExtension.Com.Helper;
using Xunit;

namespace Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
           Assert.NotEmpty(StringHelper.CreateIdByTime()); 
        }
    }
}
