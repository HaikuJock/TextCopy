using System;
using System.Threading.Tasks;

namespace TextCopy
{
    public static partial class Clipboard
    {
        static Func<string> getFunc;
        static Func<Task<string>> getFuncAsync;

        /// <summary>
        /// Retrieves text data from the Clipboard.
        /// </summary>
        public static string GetText()
        {
           return getFunc();
        }

        /// <summary>
        /// Retrieves text data from the Clipboard.
        /// </summary>
        public static Task<string> GetTextAsync()
        {
           return getFuncAsync();
        }
    }
}