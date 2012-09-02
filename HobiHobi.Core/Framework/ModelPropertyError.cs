using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Framework
{
    public class ModelPropertyError
    {
        public string Key { get; set; }
        public List<string> Errors { get; set; }
        public ModelPropertyError()
        {
            Errors = new List<string>();
        }
    }
}
