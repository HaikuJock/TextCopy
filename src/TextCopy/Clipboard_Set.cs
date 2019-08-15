using System;
using System.Threading.Tasks;

namespace TextCopy
{
    public static partial class Clipboard
    {
        static Action<string> setAction;
        static Func<string, Task> setActionAsync;

        /// <summary>
        /// Clears the Clipboard and then adds text data to it.
        /// </summary>
        public static void SetText(string text)
        {
            Guard.AgainstNull(text, nameof(text));
            setAction(text);
        }

        /// <summary>
        /// Clears the Clipboard and then adds text data to it.
        /// </summary>
        public static Task SetTextAsync(string text)
        {
            Guard.AgainstNull(text, nameof(text));
            return setActionAsync(text);
        }
    }
}