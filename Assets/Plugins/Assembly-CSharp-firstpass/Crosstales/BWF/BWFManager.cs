using System;
using System.Collections.Generic;
using System.Linq;
using Crosstales.BWF.Data;
using Crosstales.BWF.Manager;
using Crosstales.BWF.Model;
using Crosstales.BWF.Util;
using Crosstales.Common.Util;
using UnityEngine;

namespace Crosstales.BWF
{
    [ExecuteInEditMode]
    [HelpURL("https://www.crosstales.com/media/data/assets/badwordfilter/api/class_crosstales_1_1_b_w_f_1_1_b_w_f_manager.html")]
    public class BWFManager : MonoBehaviour
    {
        public delegate void BWFReady();

        private GameObject root;

        private bool sentReady;

        private static BWFReady _onBWFReady;

        public static bool isReady
        {
            get { return BadWordManager.isReady && DomainManager.isReady && CapitalizationManager.isReady && PunctuationManager.isReady; }
        }

        public static event BWFReady OnBWFReady
        {
            add { _onBWFReady = (BWFReady)Delegate.Combine(_onBWFReady, value); }
            remove { _onBWFReady = (BWFReady)Delegate.Remove(_onBWFReady, value); }
        }

        public void OnEnable()
        {
            root = base.transform.root.gameObject;
        }

        public void Update()
        {
            if (BaseHelper.isEditorMode && root != null && Config.ENSURE_NAME)
            {
                root.name = "BWF";
            }
            if (!sentReady && isReady)
            {
                sentReady = true;
                onBWFReady();
            }
        }

        public static void Load(ManagerMask mask = ManagerMask.All)
        {
            if ((mask & ManagerMask.BadWord) == ManagerMask.BadWord || (mask & ManagerMask.All) == ManagerMask.All)
            {
                BadWordManager.Load();
            }
            if ((mask & ManagerMask.Domain) == ManagerMask.Domain || (mask & ManagerMask.All) == ManagerMask.All)
            {
                DomainManager.Load();
            }
            if ((mask & ManagerMask.Capitalization) == ManagerMask.Capitalization || (mask & ManagerMask.All) == ManagerMask.All)
            {
                CapitalizationManager.Load();
            }
            if ((mask & ManagerMask.Punctuation) == ManagerMask.Punctuation || (mask & ManagerMask.All) == ManagerMask.All)
            {
                PunctuationManager.Load();
            }
        }

        public static List<Crosstales.BWF.Data.Source> Sources(ManagerMask mask = ManagerMask.All)
        {
            List<Crosstales.BWF.Data.Source> list = new List<Crosstales.BWF.Data.Source>(30);
            if ((mask & ManagerMask.BadWord) == ManagerMask.BadWord || (mask & ManagerMask.All) == ManagerMask.All)
            {
                list.AddRange(BadWordManager.Sources);
            }
            if ((mask & ManagerMask.Domain) == ManagerMask.Domain || (mask & ManagerMask.All) == ManagerMask.All)
            {
                list.AddRange(DomainManager.Sources);
            }
            return (from x in list.Distinct()
                orderby x.Name
                select x).ToList();
        }

        public static bool Contains(string text, ManagerMask mask = ManagerMask.All, params string[] sourceNames)
        {
            return (((mask & ManagerMask.BadWord) == ManagerMask.BadWord || (mask & ManagerMask.All) == ManagerMask.All) &&
                    BadWordManager.Contains(text, sourceNames)) ||
                   (((mask & ManagerMask.Domain) == ManagerMask.Domain || (mask & ManagerMask.All) == ManagerMask.All) &&
                    DomainManager.Contains(text, sourceNames)) ||
                   (((mask & ManagerMask.Capitalization) == ManagerMask.Capitalization || (mask & ManagerMask.All) == ManagerMask.All) &&
                    CapitalizationManager.Contains(text)) ||
                   (((mask & ManagerMask.Punctuation) == ManagerMask.Punctuation || (mask & ManagerMask.All) == ManagerMask.All) &&
                    PunctuationManager.Contains(text));
        }

        public static void ContainsMT(out bool result, ref string text, ManagerMask mask = ManagerMask.All, params string[] sourceNames)
        {
            result =
                (((mask & ManagerMask.BadWord) == ManagerMask.BadWord || (mask & ManagerMask.All) == ManagerMask.All) &&
                 BadWordManager.Contains(text, sourceNames)) ||
                (((mask & ManagerMask.Domain) == ManagerMask.Domain || (mask & ManagerMask.All) == ManagerMask.All) &&
                 DomainManager.Contains(text, sourceNames)) ||
                (((mask & ManagerMask.Capitalization) == ManagerMask.Capitalization || (mask & ManagerMask.All) == ManagerMask.All) &&
                 CapitalizationManager.Contains(text)) ||
                (((mask & ManagerMask.Punctuation) == ManagerMask.Punctuation || (mask & ManagerMask.All) == ManagerMask.All) &&
                 PunctuationManager.Contains(text));
        }

        public static List<string> GetAll(string text, ManagerMask mask = ManagerMask.All, params string[] sourceNames)
        {
            List<string> list = new List<string>();
            if ((mask & ManagerMask.BadWord) == ManagerMask.BadWord || (mask & ManagerMask.All) == ManagerMask.All)
            {
                list.AddRange(BadWordManager.GetAll(text, sourceNames));
            }
            if ((mask & ManagerMask.Domain) == ManagerMask.Domain || (mask & ManagerMask.All) == ManagerMask.All)
            {
                list.AddRange(DomainManager.GetAll(text, sourceNames));
            }
            if ((mask & ManagerMask.Capitalization) == ManagerMask.Capitalization || (mask & ManagerMask.All) == ManagerMask.All)
            {
                list.AddRange(CapitalizationManager.GetAll(text));
            }
            if ((mask & ManagerMask.Punctuation) == ManagerMask.Punctuation || (mask & ManagerMask.All) == ManagerMask.All)
            {
                list.AddRange(PunctuationManager.GetAll(text));
            }
            return (from x in list.Distinct()
                orderby x
                select x).ToList();
        }

        public static void GetAllMT(out List<string> result, ref string text, ManagerMask mask = ManagerMask.All, params string[] sourceNames)
        {
            List<string> list = new List<string>();
            if ((mask & ManagerMask.BadWord) == ManagerMask.BadWord || (mask & ManagerMask.All) == ManagerMask.All)
            {
                list.AddRange(BadWordManager.GetAll(text, sourceNames));
            }
            if ((mask & ManagerMask.Domain) == ManagerMask.Domain || (mask & ManagerMask.All) == ManagerMask.All)
            {
                list.AddRange(DomainManager.GetAll(text, sourceNames));
            }
            if ((mask & ManagerMask.Capitalization) == ManagerMask.Capitalization || (mask & ManagerMask.All) == ManagerMask.All)
            {
                list.AddRange(CapitalizationManager.GetAll(text));
            }
            if ((mask & ManagerMask.Punctuation) == ManagerMask.Punctuation || (mask & ManagerMask.All) == ManagerMask.All)
            {
                list.AddRange(PunctuationManager.GetAll(text));
            }
            result = (from x in list.Distinct()
                orderby x
                select x).ToList();
        }

        public static string ReplaceAll(string text, ManagerMask mask = ManagerMask.All, params string[] sourceNames)
        {
            string text2 = text ?? string.Empty;
            if ((mask & ManagerMask.BadWord) == ManagerMask.BadWord || (mask & ManagerMask.All) == ManagerMask.All)
            {
                text2 = BadWordManager.ReplaceAll(text2, false, string.Empty, string.Empty, sourceNames);
            }
            if ((mask & ManagerMask.Domain) == ManagerMask.Domain || (mask & ManagerMask.All) == ManagerMask.All)
            {
                text2 = DomainManager.ReplaceAll(text2, false, string.Empty, string.Empty, sourceNames);
            }
            if ((mask & ManagerMask.Capitalization) == ManagerMask.Capitalization || (mask & ManagerMask.All) == ManagerMask.All)
            {
                text2 = CapitalizationManager.ReplaceAll(text2, false, string.Empty, string.Empty);
            }
            if ((mask & ManagerMask.Punctuation) == ManagerMask.Punctuation || (mask & ManagerMask.All) == ManagerMask.All)
            {
                text2 = PunctuationManager.ReplaceAll(text2, false, string.Empty, string.Empty);
            }
            return text2;
        }

        public static void ReplaceAllMT(out string result, ref string text, ManagerMask mask = ManagerMask.All, params string[] sourceNames)
        {
            result = text ?? string.Empty;
            if ((mask & ManagerMask.BadWord) == ManagerMask.BadWord || (mask & ManagerMask.All) == ManagerMask.All)
            {
                result = BadWordManager.ReplaceAll(result, false, string.Empty, string.Empty, sourceNames);
            }
            if ((mask & ManagerMask.Domain) == ManagerMask.Domain || (mask & ManagerMask.All) == ManagerMask.All)
            {
                result = DomainManager.ReplaceAll(result, false, string.Empty, string.Empty, sourceNames);
            }
            if ((mask & ManagerMask.Capitalization) == ManagerMask.Capitalization || (mask & ManagerMask.All) == ManagerMask.All)
            {
                result = CapitalizationManager.ReplaceAll(result, false, string.Empty, string.Empty);
            }
            if ((mask & ManagerMask.Punctuation) == ManagerMask.Punctuation || (mask & ManagerMask.All) == ManagerMask.All)
            {
                result = PunctuationManager.ReplaceAll(result, false, string.Empty, string.Empty);
            }
        }

        public static string Mark(string text, List<string> unwantedWords, string prefix = "<b><color=red>", string postfix = "</color></b>")
        {
            string text2 = text;
            string text3 = prefix;
            string text4 = postfix;
            if (string.IsNullOrEmpty(text))
            {
                if (BaseConstants.DEV_DEBUG)
                {
                    Debug.LogWarning("Parameter 'text' is null or empty!" + Environment.NewLine + "=> 'Mark()' will return an empty string.");
                }
                text2 = string.Empty;
            }
            else
            {
                if (string.IsNullOrEmpty(prefix))
                {
                    if (BaseConstants.DEV_DEBUG)
                    {
                        Debug.LogWarning("Parameter 'prefix' is null!" + Environment.NewLine + "=> Using an empty string as prefix.");
                    }
                    text3 = string.Empty;
                }
                if (string.IsNullOrEmpty(postfix))
                {
                    if (BaseConstants.DEV_DEBUG)
                    {
                        Debug.LogWarning("Parameter 'postfix' is null!" + Environment.NewLine + "=> Using an empty string as postfix.");
                    }
                    text4 = string.Empty;
                }
                if (unwantedWords == null || unwantedWords.Count == 0)
                {
                    if (BaseConstants.DEV_DEBUG)
                    {
                        Debug.LogWarning("Parameter 'unwantedWords' is null or empty!" + Environment.NewLine +
                                         "=> 'Mark()' will return the original string.");
                    }
                }
                else
                {
                    foreach (string unwantedWord in unwantedWords)
                    {
                        if (!string.IsNullOrEmpty(unwantedWord))
                        {
                            text2 = text2.Replace(unwantedWord, text3 + unwantedWord + text4);
                        }
                    }
                }
            }
            return text2;
        }

        public static string Mark(string text, bool replace = false, string prefix = "<b><color=red>", string postfix = "</color></b>",
            ManagerMask mask = ManagerMask.All, params string[] sourceNames)
        {
            string text2 = text ?? string.Empty;
            if ((mask & ManagerMask.BadWord) == ManagerMask.BadWord || (mask & ManagerMask.All) == ManagerMask.All)
            {
                text2 = BadWordManager.Mark(text2, replace, prefix, postfix, sourceNames);
            }
            if ((mask & ManagerMask.Domain) == ManagerMask.Domain || (mask & ManagerMask.All) == ManagerMask.All)
            {
                text2 = DomainManager.Mark(text2, replace, prefix, postfix, sourceNames);
            }
            if ((mask & ManagerMask.Capitalization) == ManagerMask.Capitalization || (mask & ManagerMask.All) == ManagerMask.All)
            {
                text2 = CapitalizationManager.Mark(text2, replace, prefix, postfix);
            }
            if ((mask & ManagerMask.Punctuation) == ManagerMask.Punctuation || (mask & ManagerMask.All) == ManagerMask.All)
            {
                text2 = PunctuationManager.Mark(text2, replace, prefix, postfix);
            }
            return text2;
        }

        public static string Unmark(string text, string prefix = "<b><color=red>", string postfix = "</color></b>")
        {
            string text2 = text ?? string.Empty;
            return BadWordManager.Unmark(text2, prefix, postfix);
        }

        private void onBWFReady()
        {
            if (_onBWFReady != null)
            {
                _onBWFReady();
            }
        }
    }
}