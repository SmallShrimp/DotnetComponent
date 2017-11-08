using System;
using System.Collections.Generic;
using System.Text;

namespace Xugege.DotnetExtension.Com.Dto
{
    public class NameValue<T>
    {

        public NameValue() { }

        public NameValue(string name, T value)
        {
            this.Name = name;
            this.Value = value;
        }


        public string Name { get; set; }

        public T Value { get; set; } 
    }
}
