using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace VFS.Server.Core
{
    /// <summary>
    /// Contain extention methods
    /// </summary>
    static class ExtenstionMethods
    {
        /// <summary>
        /// Compare string values in ordinal and ignore case
        /// </summary>
        /// <param name="value1">value 1 for compare</param>
        /// <param name="value2">value 2 for compare</param>
        /// <returns><c>true</c> - if equals</returns>        
        public static bool EqualByOrdinalIgnoreCase(this string value1, string value2)
        {
            return String.Compare(value1, value2, StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        /// Detemines whether the <paramref name="source"/> or it childs contain elements,
        /// that match the condition define by specified predicate
        /// </summary>
        /// <typeparam name="T">Type of elements</typeparam>
        /// <param name="source">Elements to search</param>
        /// <param name="selector">Define condition to extract child elements</param>
        /// <param name="predicate">Define condition to search element</param>
        /// <returns><c>true</c> - contain elements, that match the condition <paramref name="predicate"/></returns>
        /// 
        /// <exception cref="NullReferenceException">if <paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="selector"/> or <paramref name="predicate"/> is null
        /// </exception>
        public static bool Exists<T>(this IEnumerable<T> source,
            Func<T, IEnumerable<T>> selector, Func<T, bool> predicate)
        {
            if (source == null)
            {
                throw new NullReferenceException();
            }
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            bool returnValue = false;
            foreach (T item in source)
            {
                returnValue |= predicate(item);
                returnValue |= selector(item).Exists(selector, predicate);
            }
            return returnValue;
        }

        /// <summary>
        /// Detemines whether the <paramref name="source"/> or it childs contain elements,
        /// that match the condition define by specified predicate
        /// </summary>
        /// <typeparam name="T">Type of elements</typeparam>
        /// <param name="source">Element to search</param>
        /// <param name="selector">Define condition to extract child elements</param>
        /// <param name="predicate">Define condition to search element</param>
        /// <returns><c>true</c> - contain elements, that match the condition <paramref name="predicate"/></returns>
        /// 
        /// <exception cref="NullReferenceException">if <paramref name="source"/> is null</exception>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="selector"/> or <paramref name="predicate"/> is null
        /// </exception>
        public static bool Exists<T>(this T source,
            Func<T, IEnumerable<T>> selector, Func<T, bool> predicate)
        {
            if (source == null)
            {
                throw new NullReferenceException();
            }
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            bool returnValue = false;
            returnValue |= predicate(source);
            returnValue |= selector(source).Exists(selector, predicate);
            return returnValue;
        }

        /// <summary>
        /// Repeat string value
        /// </summary>
        /// <param name="valueToRepeat">value to repeat</param>
        /// <param name="count">count repetitions</param>
        /// <returns>new string value</returns>
        public static string Repeat(this string valueToRepeat, int count)
        {
            if (valueToRepeat == null)
            {
                valueToRepeat = String.Empty;
            }
            StringBuilder returnValue = new StringBuilder(valueToRepeat.Length * count);
            for (int i = 0; i < count; i++)
            {
                returnValue.Append(valueToRepeat);
            }
            return returnValue.ToString();
        }

        /// <summary>
        /// Execute deep copy of the object
        /// </summary>
        /// <param name="current">Object to deep copy</param>
        ///
        /// <exception cref="ArgumentNullException">if <paramref name="current"/> is null</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">
        /// During serialization or deserialization of the <paramref name="current"/>
        /// </exception>
        public static object DeepCopy(this object current)
        {            
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, current);
                stream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(stream);
            }
        }
    }
}
