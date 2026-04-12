using System;
using System.Text;

namespace MauiLearn.Core.Extensions
{
    /// <summary>
    /// Common string helper extensions for positional and value-based replacements.
    /// Designed to be small, safe, and easy to read.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Replaces the character at the specified index with <paramref name="newChar"/>.
        /// </summary>
        public static string ReplaceAt(this string source, int index, char newChar)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if ((uint)index >= (uint)source.Length) throw new ArgumentOutOfRangeException(nameof(index));

            // Fast path for single-character strings
            if (source.Length == 1)
            {
                return newChar.ToString();
            }

            var arr = source.ToCharArray();
            arr[index] = newChar;
            return new string(arr);
        }

        /// <summary>
        /// Replaces a range of characters starting at <paramref name="index"/> with <paramref name="replacement"/>.
        /// If replacement is null it is treated as empty string.
        /// </summary>
        public static string ReplaceRange(this string source, int index, int length, string replacement)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (index < 0 || length < 0 || index + length > source.Length) throw new ArgumentOutOfRangeException();

            replacement ??= string.Empty;
            if (length == 0) return source.Insert(index, replacement);
            return source.Remove(index, length).Insert(index, replacement);
        }

        /// <summary>
        /// Replaces the first occurrence of <paramref name="search"/> with <paramref name="replace"/>.
        /// If search is null or empty the original string is returned.
        /// </summary>
        public static string ReplaceFirst(this string source, string search, string replace)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrEmpty(search)) return source;

            int pos = source.IndexOf(search, StringComparison.Ordinal);
            return pos < 0 ? source : source.Remove(pos, search.Length).Insert(pos, replace ?? string.Empty);
        }

        /// <summary>
        /// Replaces the last occurrence of <paramref name="search"/> with <paramref name="replace"/>.
        /// If search is null or empty the original string is returned.
        /// </summary>
        public static string ReplaceLast(this string source, string search, string replace)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrEmpty(search)) return source;

            int pos = source.LastIndexOf(search, StringComparison.Ordinal);
            return pos < 0 ? source : source.Remove(pos, search.Length).Insert(pos, replace ?? string.Empty);
        }

        /// <summary>
        /// Replaces characters at multiple indices with the corresponding characters in <paramref name="replacements"/>.
        /// Indices and replacements arrays must be the same length.
        /// This method uses a single char[] allocation and is efficient for a handful of edits.
        /// </summary>
        public static string ReplaceAtMany(this string source, int[] indices, char[] replacements)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (indices is null) throw new ArgumentNullException(nameof(indices));
            if (replacements is null) throw new ArgumentNullException(nameof(replacements));
            if (indices.Length != replacements.Length) throw new ArgumentException("indices and replacements must have the same length");

            var arr = source.ToCharArray();
            for (int i = 0; i < indices.Length; i++)
            {
                int idx = indices[i];
                if ((uint)idx >= (uint)arr.Length) throw new ArgumentOutOfRangeException(nameof(indices));
                arr[idx] = replacements[i];
            }
            return new string(arr);
        }

        /// <summary>
        /// Replaces all occurrences of <paramref name="oldValue"/> with <paramref name="newValue"/> but only up to <paramref name="maxReplacements"/>.
        /// If maxReplacements is null or less than 1, all occurrences are replaced.
        /// </summary>
        public static string ReplaceLimit(this string source, string oldValue, string newValue, int? maxReplacements = null)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrEmpty(oldValue)) return source;
            newValue ??= string.Empty;

            if (maxReplacements is null || maxReplacements < 1)
            {
                return source.Replace(oldValue, newValue, StringComparison.Ordinal);
            }

            var sb = new StringBuilder();
            int start = 0;
            int count = 0;
            while (count < maxReplacements)
            {
                int pos = source.IndexOf(oldValue, start, StringComparison.Ordinal);
                if (pos < 0) break;
                sb.Append(source, start, pos - start);
                sb.Append(newValue);
                start = pos + oldValue.Length;
                count++;
            }
            sb.Append(source, start, source.Length - start);
            return sb.ToString();
        }

        /// <summary>
        /// Convenience overload for String.Replace with StringComparison.
        /// Replaces all occurrences using the provided comparison.
        /// </summary>
        public static string Replace(this string source, string oldValue, string newValue, StringComparison comparison)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (string.IsNullOrEmpty(oldValue)) return source;
            newValue ??= string.Empty;

            int startIndex = 0;
            var sb = new StringBuilder();
            while (true)
            {
                int pos = source.IndexOf(oldValue, startIndex, comparison);
                if (pos < 0)
                {
                    sb.Append(source, startIndex, source.Length - startIndex);
                    break;
                }
                sb.Append(source, startIndex, pos - startIndex);
                sb.Append(newValue);
                startIndex = pos + oldValue.Length;
            }
            return sb.ToString();
        }
    }
}
