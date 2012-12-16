using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.OpmlEditor
{
    //Hold the values from the opml editor 
    public class EditorOutline
    {
        public string Data { get; set; }
        public Dictionary<string, string> Attr { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public List<EditorOutline> Children { get; set; }

        public EditorOutline()
        {
            Attr = new Dictionary<string, string>();
            Metadata = new Dictionary<string, string>();
            Children = new List<EditorOutline>();
        }
    }
}
