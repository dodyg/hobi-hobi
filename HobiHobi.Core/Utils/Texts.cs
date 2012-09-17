using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace HobiHobi.Core.Utils
{
    public static class Texts
    {
        public static string LimitTextForRiverJs(string str)
        {
            if (str == null)
                return str;

            if (str.Length <= 280)
                return str;
            else
            {
                str = str.Substring(0, 279); //take only 280 according to riverjs.org
                var lastEmpty = str.LastIndexOf(' ');
                if (lastEmpty != 279)
                    str = str.Substring(0, lastEmpty);

                str += "...";

                return str;
            }
        }

        //dirty code
        public static string TruncateHTMLSafeishChar(string text, int charCount)
        {
            bool inTag = false;
            int cntr = 0;
            int cntrContent = 0;

            // loop through html, counting only viewable content
            foreach (Char c in text)
            {
                if (cntrContent == charCount) break;
                cntr++;
                if (c == '<')
                {
                    inTag = true;
                    continue;
                }

                if (c == '>')
                {
                    inTag = false;
                    continue;
                }
                if (!inTag) cntrContent++;
            }

            string substr = text.Substring(0, cntr);

            //search for nonclosed tags        
            MatchCollection openedTags = new Regex("<[^/](.|\n)*?>").Matches(substr);
            MatchCollection closedTags = new Regex("<[/](.|\n)*?>").Matches(substr);

            // create stack          
            Stack<string> opentagsStack = new Stack<string>();
            Stack<string> closedtagsStack = new Stack<string>();

            // to be honest, this seemed like a good idea then I got lost along the way 
            // so logic is probably hanging by a thread!! 
            foreach (Match tag in openedTags)
            {
                string openedtag = tag.Value.Substring(1, tag.Value.Length - 2);
                // strip any attributes, sure we can use regex for this!
                if (openedtag.IndexOf(" ") >= 0)
                {
                    openedtag = openedtag.Substring(0, openedtag.IndexOf(" "));
                }

                // ignore brs as self-closed
                if (openedtag.Trim() != "br")
                {
                    opentagsStack.Push(openedtag);
                }
            }

            foreach (Match tag in closedTags)
            {
                string closedtag = tag.Value.Substring(2, tag.Value.Length - 3);
                closedtagsStack.Push(closedtag);
            }

            if (closedtagsStack.Count < opentagsStack.Count)
            {
                while (opentagsStack.Count > 0)
                {
                    string tagstr = opentagsStack.Pop();

                    if (closedtagsStack.Count == 0 || tagstr != closedtagsStack.Peek())
                    {
                        substr += "</" + tagstr + ">";
                    }
                    else
                    {
                        closedtagsStack.Pop();
                    }
                }
            }

            return substr;
        }

        //dirty code
        public static string TruncateHTMLSafeishWord(string text, int wordCount)
        {
            bool inTag = false;
            int cntr = 0;
            int cntrWords = 0;
            Char lastc = ' ';

            // loop through html, counting only viewable content
            foreach (Char c in text)
            {
                if (cntrWords == wordCount) break;
                cntr++;
                if (c == '<')
                {
                    inTag = true;
                    continue;
                }

                if (c == '>')
                {
                    inTag = false;
                    continue;
                }
                if (!inTag)
                {
                    // do not count double spaces, and a space not in a tag counts as a word
                    if (c == 32 && lastc != 32)
                        cntrWords++;
                }
            }

            string substr = text.Substring(0, cntr) + " ...";

            //search for nonclosed tags        
            MatchCollection openedTags = new Regex("<[^/](.|\n)*?>").Matches(substr);
            MatchCollection closedTags = new Regex("<[/](.|\n)*?>").Matches(substr);

            // create stack          
            Stack<string> opentagsStack = new Stack<string>();
            Stack<string> closedtagsStack = new Stack<string>();

            foreach (Match tag in openedTags)
            {
                string openedtag = tag.Value.Substring(1, tag.Value.Length - 2);
                // strip any attributes, sure we can use regex for this!
                if (openedtag.IndexOf(" ") >= 0)
                {
                    openedtag = openedtag.Substring(0, openedtag.IndexOf(" "));
                }

                // ignore brs as self-closed
                if (openedtag.Trim() != "br")
                {
                    opentagsStack.Push(openedtag);
                }
            }

            foreach (Match tag in closedTags)
            {
                string closedtag = tag.Value.Substring(2, tag.Value.Length - 3);
                closedtagsStack.Push(closedtag);
            }

            if (closedtagsStack.Count < opentagsStack.Count)
            {
                while (opentagsStack.Count > 0)
                {
                    string tagstr = opentagsStack.Pop();

                    if (closedtagsStack.Count == 0 || tagstr != closedtagsStack.Peek())
                    {
                        substr += "</" + tagstr + ">";
                    }
                    else
                    {
                        closedtagsStack.Pop();
                    }
                }
            }

            return substr;
        }

        //dirty code
        public static string TruncateHTMLSafeishCharXML(string text, int charCount)
        {
            // your data, probably comes from somewhere, or as params to a methodint 
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(text);
            // create a navigator, this is our primary tool
            XPathNavigator navigator = xml.CreateNavigator();
            XPathNavigator breakPoint = null;

            // find the text node we need:
            while (navigator.MoveToFollowing(XPathNodeType.Text))
            {
                string lastText = navigator.Value.Substring(0, Math.Min(charCount, navigator.Value.Length));
                charCount -= navigator.Value.Length;
                if (charCount <= 0)
                {
                    // truncate the last text. Here goes your "search word boundary" code:        
                    navigator.SetValue(lastText);
                    breakPoint = navigator.Clone();
                    break;
                }
            }

            // first remove text nodes, because Microsoft unfortunately merges them without asking
            while (navigator.MoveToFollowing(XPathNodeType.Text))
            {
                if (navigator.ComparePosition(breakPoint) == XmlNodeOrder.After)
                {
                    navigator.DeleteSelf();
                }
            }

            // moves to parent, then move the rest
            navigator.MoveTo(breakPoint);
            while (navigator.MoveToFollowing(XPathNodeType.Element))
            {
                if (navigator.ComparePosition(breakPoint) == XmlNodeOrder.After)
                {
                    navigator.DeleteSelf();
                }
            }

            // moves to parent
            // then remove *all* empty nodes to clean up (not necessary):
            // TODO, add empty elements like <br />, <img /> as exclusion
            navigator.MoveToRoot();
            while (navigator.MoveToFollowing(XPathNodeType.Element))
            {
                while (!navigator.HasChildren && (navigator.Value ?? "").Trim() == "")
                {
                    navigator.DeleteSelf();
                }
            }

            // moves to parent
            navigator.MoveToRoot();
            return navigator.InnerXml;
        }

        public static string FromUriHost(Uri uri)
        {
#if DEBUG
            if (uri.IsDefaultPort)
                return uri.Scheme + "://" + uri.DnsSafeHost;
            else
                return uri.Scheme + "://" + uri.DnsSafeHost + ":" + uri.Port;
#else
            return uri.Scheme + "://" + uri.DnsSafeHost;
#endif
        }

        public static string ConvertTitleToName(string title)
        {
            return title.Replace(" ", "_").Replace("#", "_").Replace("'", "_").Replace(".", "_")
                .Replace("/", "_").Replace("\\", "_");
        }
    }
}
