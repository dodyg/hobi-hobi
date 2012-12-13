using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.OpmlEditor
{
    public class EditorDocument
    {
        public string Id { get; set; }
        public List<EditorOutline> Outlines { get; set; }
    }
}
