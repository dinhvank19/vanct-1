using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace POS.Shared
{
    public static class ObjectHelper
    {
        private static readonly string[] Types =
        {
            "System.Int16",
            "System.Int32",
            "System.Int64",
            "System.String",
            "System.Boolean",
            "System.DateTime",
            "System.TimeSpan",
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
        public static TTarget Clone<TSource, TTarget>(this TSource source, TTarget target)
        {
            var sourceProperties = source.GetType().GetProperties();
            foreach (var sourceProperty in sourceProperties)
            {
                if (sourceProperty.GetSetMethod() == null) continue;

                //get property
                var targetProperty = target.GetType().GetProperty(sourceProperty.Name);
                if (targetProperty == null || !targetProperty.CanWrite) continue;

                //get value
                var value = sourceProperty.GetValue(source, null);
                if (value != null && !Types.Contains(value.GetType().ToString())) continue;

                targetProperty.SetValue(target, value, null);
            }

            return target;
        }

        /// <summary>
        ///     Jsons the text to object.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static TTarget JsonTextTo<TTarget>(this string text)
        {
            return JsonConvert.DeserializeObject<TTarget>(text);
        }

        /// <summary>
        ///     To the json.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <returns></returns>
        public static string ToJson(this object self)
        {
            return JsonConvert.SerializeObject(self);
        }

        /// <summary>
        ///     To the enum.
        /// </summary>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static TTarget ToEnum<TTarget>(this string text)
        {
            return (TTarget) Enum.Parse(typeof (TTarget), text);
        }
    }
}