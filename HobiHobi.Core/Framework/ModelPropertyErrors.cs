using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Framework
{
    public class ModelPropertyErrors
    {
        public List<ModelPropertyError> Properties { get; set; }
        public ModelPropertyErrors()
        {
            Properties = new List<ModelPropertyError>();
        }

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
