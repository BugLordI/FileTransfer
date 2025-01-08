using System.Globalization;
using System.Resources;
using System.Text;

namespace FileTransfer.i18n
{
    /// <summary>
    ///  Get Specialed Lanuage String
    /// </summary>
    internal class LanuageMananger
    {
        private static List<Action<String>> actions = new();

        private static readonly ResourceManager resourceManager = new("FileTransfer.i18n.Resource", typeof(LanuageMananger).Assembly);

        private static readonly String LANGUAGE_CONFIG_FILE = "LanguageConfig";

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
        public static void SetLanguage(string cultureName, bool save)
        {
            var culture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            if (save)
            {
                SaveLanguage(cultureName);
                LanguageChanged(cultureName);
            }
        }

        public static void OnLanguageChanged(Action<String> languageChangedCallback)
        {
            actions.Add(languageChangedCallback);
        }

        public static string LoadLanguage()
        {
            using (FileStream fs = new FileStream(LANGUAGE_CONFIG_FILE, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                byte[] buffer = new byte[64];
                _ = fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                String lang = Encoding.UTF8.GetString(buffer);
                SetLanguage(lang, false);
                return lang;
            }
        }

        private static void LanguageChanged(String cultureName)
        {
            foreach (var item in actions)
            {
                item?.Invoke(cultureName);
            }
        }

        private static void SaveLanguage(String cultureNam)
        {
            byte[] contentBytes = Encoding.UTF8.GetBytes(cultureNam);
            FileStream fs = new FileStream(LANGUAGE_CONFIG_FILE, FileMode.Create, FileAccess.ReadWrite);
            fs.Write(contentBytes, 0, contentBytes.Length);
            fs.Close();
        }
    }
}
