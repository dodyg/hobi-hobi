using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HobiHobi.Core
{
    public class HostAndPath
    {
        public string Host { get; set; }
        public string Path { get; set; }

        public HostAndPath(string host, string path)
        {
            Host = host;
            Path = path;
        }
    }
}
