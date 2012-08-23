using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HobiHobi.Core.Framework
{
    public class Key
    {
        public static Key Generate(string @namespace, string value)
        {
            if (value == null)
                return new Key(@namespace, "");
            else
                return new Key(@namespace, value);
        }

        protected string _namespace;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Key"/> class.
        /// </summary>
        public Key(string @namespace, string value)
        {
            _namespace = @namespace;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value
        {
            get;
            protected set;
        }

        public string NoNamespace()
        {
            if (Value.IsNullOrWhiteSpace())
                return string.Empty;

            return Value.Replace(_namespace, "");
        }

        public string WithNamespace()
        {
            if (Value.IsNullOrWhiteSpace())
                return _namespace;

            if (Value.Contains(_namespace))
                return Value;
            else
                return _namespace + Value;
        }

        public string Identity()
        {
            return _namespace;
        }

        public static string GenerateTicksId()
        {
            return DateTime.Now.Ticks + "";
        }

        public static string GenerateRNGCharacterMaskId()
        {
            int maxSize = 8;
            char[] chars = new char[62];
            string a;
            a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            var crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            var result = new StringBuilder(size);
            foreach (byte b in data)
                result.Append(chars[b % (chars.Length - 1)]);

            return result.ToString();
        }

        public static string GenerateCalendarTicksId()
        {
            var now = DateTime.UtcNow;
            return "{0}-{1}-{2}-{3}".F(now.Year, now.Month, now.Day, now.TimeOfDay.Ticks);
        }
    }

}
