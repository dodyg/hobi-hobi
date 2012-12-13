using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Framework.Json
{
    public class TitleCaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(propertyName);
        }
    }
}
