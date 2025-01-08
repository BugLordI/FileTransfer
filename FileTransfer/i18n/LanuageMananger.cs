using System.Globalization;
using System.Resources;

namespace FileTransfer.i18n
{
    /// <summary>
    ///  Get Specialed Lanuage String
    /// </summary>
    internal class LanuageMananger
    {
        private static readonly ResourceManager resourceManager = new("FileTransfer.i18n.Resource", typeof(LanuageMananger).Assembly);

        /// <summary>
        /// Get The Specified Lanuage String
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(string key)
        {
            var str = resourceManager.GetString(key, Thread.CurrentThread.CurrentUICulture);
            return str ?? key;
        }

        /// <summary>
        /// Set Lanuage
        /// </summary>
        /// <param name="cultureName">Like zh-cn,en-us ……</param>
        public static void SetLanguage(string cultureName)
        {
            var culture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
        }
    }
}
