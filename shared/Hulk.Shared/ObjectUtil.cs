using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Hulk.Shared
{
    public static class ObjectUtil
    {
        public static string[] Types =
        {
            "System.Int32",
            "System.Int64",
            "System.String",
            "System.Boolean",
            "System.DateTime",
            "System.Double",
            "System.Decimal"
        };

        /// <summary>
        ///     Copies the automatic.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static TTarget CopyTo<TSource, TTarget>(this TSource source, TTarget target)
        {
            PropertyInfo[] sourceProperties = source.GetType().GetProperties();
            foreach (PropertyInfo sourceProperty in sourceProperties)
            {
                if (sourceProperty.GetSetMethod() == null) continue;

                //get property
                PropertyInfo targetProperty = target.GetType().GetProperty(sourceProperty.Name);
                if (targetProperty == null) continue;

                //get value
                object value = sourceProperty.GetValue(source, null);
                if (value != null && !Types.Contains(value.GetType().ToString())) continue;

                targetProperty.SetValue(target, value, null);
            }

            return target;
        }

        /// <summary>
        ///     Saves the asynchronous automatic XML.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="path">The path.</param>
        public static void SaveAsToXml<TSource>(this TSource source, string path)
        {
            using (var file = new StreamWriter(path))
            {
                file.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                file.WriteLine("<{0}>", source.GetClassName());
                PropertyInfo[] properties = source.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.GetSetMethod() == null) continue;
                    //file.WriteLine("<{0}><![CDATA[{1}]]></{0}>", property.Name, property.GetValue(source, null));

                    object value = property.GetValue(source, null);
                    if (value == null)
                    {
                        file.WriteLine("<{0} />", property.Name);
                    }
                    else if (Types.Contains(value.GetType().ToString()))
                    {
                        file.WriteLine("<{0}>{1}</{0}>", property.Name,
                            value.GetType().ToString().Equals("System.DateTime")
                                ? ((DateTime) value).ToString(((DateTime) value).Hour == 0 &&
                                                              ((DateTime) value).Minute == 0
                                    ? "dd-MM-yyyy"
                                    : "dd-MM-yyyy HH:mm")
                                : value);
                    }
                }
                file.WriteLine("</{0}>", source.GetClassName());
            }
        }

        /// <summary>
        ///     Gets the name of the class.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static string GetClassName<TSource>(this TSource source)
        {
            string[] names = source.GetType().ToString().Split('.');
            return names[names.Length - 1];
        }

        /// <summary>
        ///     Gets the name space.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static string GetNameSpace<TSource>(this TSource source)
        {
            string[] names = source.GetType().ToString().Split('.');
            string rootName = names[names.Length - 1];
            return source.GetType().ToString().Replace("." + rootName, string.Empty);
        }
    }
}