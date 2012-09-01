using HobiHobi.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core.Feeds
{
    public class BundledText
    {
        Func<string, string> _processor;

        public string Text
        {
            get;
            set;
        }

        public string ProcessedText { get; set; }
        public string ETag { get; set; }

        public BundledText()
        {

        }
        
        public BundledText(Func<string, string> processor) : this()
        {
            _processor = processor;
        }

        public BundledText SetText(string text)
        {
            ETag = Stamp.ETag();
            Text = text;
            if (_processor != null)
                ProcessedText = _processor(text);

            return this;
        }
        
        public string GetText()
        {
            if (string.IsNullOrWhiteSpace(ProcessedText))
                return Text;
            else
                return ProcessedText;
        }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrWhiteSpace(Text);
            }
        }

        public bool IsFileModifiedByETag(string etag)
        {
            var tag = "\"" + ETag + "\"";
            return tag != etag;
        }
    }
}
