using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Dynamic;
using System.Web.Mvc;

namespace HobiHobi.Core.Framework
{
    public static class ObjectExtension
    {
        /// <summary>
        /// A simple and effective cloning mechanism
        /// </summary>
        /// <remarks>
        /// http://weblogs.asp.net/esanchez/archive/2008/05/18/cloning-objects-in-net.aspx
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="originalObject"></param>
        /// <returns></returns>
        public static T BinaryClone<T>(this T originalObject)
        {
            using (var stream = new System.IO.MemoryStream())
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, originalObject);
                stream.Position = 0;

                return (T)binaryFormatter.Deserialize(stream);
            }
        }


        /// <summary>
        /// Dumps all the public properties of the object into string.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string Dump(this Object self, int level = 0, int depth = 0)
        {
            if (level > depth)
                return "";

            if (self == null)
                return "";

            try
            {
                var sb = new StringBuilder();

                // Include the type of the object
                System.Type type = self.GetType();
                sb.Append("Type: " + type.Name);

                // Include information for each Field
                sb.Append("\r\n\r\nFields:");
                System.Reflection.FieldInfo[] fi = type.GetFields();
                if (fi.Length > 0)
                {
                    foreach (FieldInfo f in fi)
                    {
                        sb.Append("\r\n " + f.ToString() + " = " +
                                  f.GetValue(self));
                    }
                }
                else
                    sb.Append("\r\n None");

                // Include information for each Property
                sb.Append("\r\n\r\nProperties:");
                System.Reflection.PropertyInfo[] pi = type.GetProperties();
                if (pi.Length > 0)
                {
                    var levelUp = level + 1;

                    foreach (PropertyInfo p in pi)
                    {
                        var tmp = p.GetValue(self, null);

                        sb.Append("\r\n " + (p.ToString() + " = " + tmp).PadLeft(level * 4, '_'));


                        if (p.PropertyType.IsClass
                            && p.PropertyType != typeof(string)
                            && !p.PropertyType.IsArray
                            && !(tmp is System.Collections.ICollection))
                            sb.Append("\r\n\r\nProperty {0} at depth {1}\r\n".F(tmp, levelUp) + tmp.Dump(levelUp, depth));
                    }
                }
                else
                    sb.Append("\r\n None");

                return sb.ToString();
            }
            catch
            {
                return "";
            }
        }

        public static ExpandoObject ToExpando(this object anonymousObject)
        {
            IDictionary<string, object> anonymousDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(anonymousObject);
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var item in anonymousDictionary)
                expando.Add(item);
            return (ExpandoObject)expando;
        }
    }
}
