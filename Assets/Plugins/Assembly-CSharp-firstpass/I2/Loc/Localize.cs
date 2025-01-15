using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
    [AddComponentMenu("I2/Localization/Localize")]
    public class Localize : MonoBehaviour
    {
        public enum TermModification
        {
            DontModify = 0,
            ToUpper = 1,
            ToLower = 2,
            ToUpperFirst = 3,
            ToTitle = 4
        }

        public delegate void DelegateSetFinalTerms(string Main, string Secondary, out string primaryTerm, out string secondaryTerm,
            bool RemoveNonASCII);

        public delegate void DelegateDoLocalize(string primaryTerm, string secondaryTerm);

        public string mTerm = string.Empty;

        public string mTermSecondary = string.Empty;

        [NonSerialized] public string FinalTerm;

        [NonSerialized] public string FinalSecondaryTerm;

        public TermModification PrimaryTermModifier;

        public TermModification SecondaryTermModifier;

        public string TermPrefix;

        public string TermSuffix;

        public bool LocalizeOnAwake = true;

        private string LastLocalizedLanguage;

        public UnityEngine.Object mTarget;

        public DelegateSetFinalTerms EventSetFinalTerms;

        public DelegateDoLocalize EventDoLocalize;

        public bool CanUseSecondaryTerm;

        public bool AllowMainTermToBeRTL;

        public bool AllowSecondTermToBeRTL;

        public bool IgnoreRTL;

        public int MaxCharactersInRTL;

        public bool IgnoreNumbersInRTL = true;

        public bool CorrectAlignmentForRTL = true;

        public UnityEngine.Object[] TranslatedObjects;

        public EventCallback LocalizeCallBack = new EventCallback();

        public static string MainTranslation;

        public static string SecondaryTranslation;

        public static string CallBackTerm;

        public static string CallBackSecondaryTerm;

        public static Localize CurrentLocalizeComponent;

        public bool AlwaysForceLocalize;

        public bool mGUI_ShowReferences;

        public bool mGUI_ShowTems = true;

        public bool mGUI_ShowCallback;

        private Text mTarget_uGUI_Text;

        private Image mTarget_uGUI_Image;

        private RawImage mTarget_uGUI_RawImage;

        private TextAnchor mAlignmentUGUI_RTL = TextAnchor.UpperRight;

        private TextAnchor mAlignmentUGUI_LTR;

        private bool mAlignmentUGUIwasRTL;

        private Text mTarget_GUIText;

        private TextMesh mTarget_TextMesh;

        private AudioSource mTarget_AudioSource;

        private Image mTarget_GUITexture;

        private GameObject mTarget_Child;

        private SpriteRenderer mTarget_SpriteRenderer;

        private bool mInitializeAlignment = true;

        private TextAlignment mAlignmentStd_LTR;

        private TextAlignment mAlignmentStd_RTL = TextAlignment.Right;

        public string Term
        {
            get { return mTerm; }
            set { SetTerm(value); }
        }

        public string SecondaryTerm
        {
            get { return mTermSecondary; }
            set { SetTerm(null, value); }
        }

        public event Action EventFindTarget;

        private void Awake()
        {
            RegisterTargets();
            if (HasTargetCache())
            {
                this.EventFindTarget();
            }
            if (LocalizeOnAwake)
            {
                OnLocalize();
            }
        }

        private void RegisterTargets()
        {
            if (this.EventFindTarget == null)
            {
                RegisterEvents_NGUI();
                RegisterEvents_DFGUI();
                RegisterEvents_UGUI();
                RegisterEvents_2DToolKit();
                RegisterEvents_TextMeshPro();
                RegisterEvents_UnityStandard();
                RegisterEvents_SVG();
            }
        }

        private void OnEnable()
        {
            OnLocalize();
        }

        public void OnLocalize(bool Force = false)
        {
            if ((!Force && (!base.enabled || base.gameObject == null || !base.gameObject.activeInHierarchy)) ||
                string.IsNullOrEmpty(LocalizationManager.CurrentLanguage) || (!AlwaysForceLocalize && !Force && !LocalizeCallBack.HasCallback() &&
                                                                              LastLocalizedLanguage == LocalizationManager.CurrentLanguage))
            {
                return;
            }
            LastLocalizedLanguage = LocalizationManager.CurrentLanguage;
            if (!HasTargetCache())
            {
                FindTarget();
            }
            if (!HasTargetCache())
            {
                return;
            }
            if (string.IsNullOrEmpty(FinalTerm) || string.IsNullOrEmpty(FinalSecondaryTerm))
            {
                GetFinalTerms(out FinalTerm, out FinalSecondaryTerm);
            }
            bool flag = Application.isPlaying && LocalizeCallBack.HasCallback();
            if (!flag && string.IsNullOrEmpty(FinalTerm) && string.IsNullOrEmpty(FinalSecondaryTerm))
            {
                return;
            }
            CallBackTerm = FinalTerm;
            CallBackSecondaryTerm = FinalSecondaryTerm;
            MainTranslation = ((!string.IsNullOrEmpty(FinalTerm) && !(FinalTerm == "-"))
                ? LocalizationManager.GetTermTranslation(FinalTerm, false)
                : null);
            SecondaryTranslation = ((!string.IsNullOrEmpty(FinalSecondaryTerm) && !(FinalSecondaryTerm == "-"))
                ? LocalizationManager.GetTermTranslation(FinalSecondaryTerm, false)
                : null);
            if (!flag && string.IsNullOrEmpty(FinalTerm) && string.IsNullOrEmpty(SecondaryTranslation))
            {
                return;
            }
            CurrentLocalizeComponent = this;
            if (Application.isPlaying)
            {
                LocalizeCallBack.Execute(this);
                LocalizationManager.ApplyLocalizationParams(ref MainTranslation, base.gameObject);
            }
            bool flag2 = LocalizationManager.IsRight2Left && !IgnoreRTL;
            if (flag2)
            {
                if (AllowMainTermToBeRTL && !string.IsNullOrEmpty(MainTranslation))
                {
                    MainTranslation = LocalizationManager.ApplyRTLfix(MainTranslation, MaxCharactersInRTL, IgnoreNumbersInRTL);
                }
                if (AllowSecondTermToBeRTL && !string.IsNullOrEmpty(SecondaryTranslation))
                {
                    SecondaryTranslation = LocalizationManager.ApplyRTLfix(SecondaryTranslation);
                }
            }
            if (PrimaryTermModifier != 0)
            {
                MainTranslation = MainTranslation ?? string.Empty;
            }
            switch (PrimaryTermModifier)
            {
                case TermModification.ToUpper:
                    MainTranslation = MainTranslation.ToUpper();
                    break;
                case TermModification.ToLower:
                    MainTranslation = MainTranslation.ToLower();
                    break;
                case TermModification.ToUpperFirst:
                    MainTranslation = GoogleTranslation.UppercaseFirst(MainTranslation);
                    break;
                case TermModification.ToTitle:
                    MainTranslation = GoogleTranslation.TitleCase(MainTranslation);
                    break;
            }
            if (SecondaryTermModifier != 0)
            {
                SecondaryTranslation = SecondaryTranslation ?? string.Empty;
            }
            switch (SecondaryTermModifier)
            {
                case TermModification.ToUpper:
                    SecondaryTranslation = SecondaryTranslation.ToUpper();
                    break;
                case TermModification.ToLower:
                    SecondaryTranslation = SecondaryTranslation.ToLower();
                    break;
                case TermModification.ToUpperFirst:
                    SecondaryTranslation = GoogleTranslation.UppercaseFirst(SecondaryTranslation);
                    break;
                case TermModification.ToTitle:
                    SecondaryTranslation = GoogleTranslation.TitleCase(SecondaryTranslation);
                    break;
            }
            if (!string.IsNullOrEmpty(TermPrefix))
            {
                MainTranslation = ((!flag2) ? (TermPrefix + MainTranslation) : (MainTranslation + TermPrefix));
            }
            if (!string.IsNullOrEmpty(TermSuffix))
            {
                MainTranslation = ((!flag2) ? (MainTranslation + TermSuffix) : (TermSuffix + MainTranslation));
            }
            EventDoLocalize(MainTranslation, SecondaryTranslation);
            CurrentLocalizeComponent = null;
        }

        public bool FindTarget()
        {
            if (HasTargetCache())
            {
                return true;
            }
            if (this.EventFindTarget == null)
            {
                RegisterTargets();
            }
            this.EventFindTarget();
            return HasTargetCache();
        }

        public void FindAndCacheTarget<T>(ref T targetCache, DelegateSetFinalTerms setFinalTerms, DelegateDoLocalize doLocalize,
            bool UseSecondaryTerm, bool MainRTL, bool SecondRTL) where T : Component
        {
            if (mTarget != null)
            {
                targetCache = mTarget as T;
            }
            else
            {
                mTarget = (targetCache = GetComponent<T>());
            }
            if (targetCache != null)
            {
                EventSetFinalTerms = setFinalTerms;
                EventDoLocalize = doLocalize;
                CanUseSecondaryTerm = UseSecondaryTerm;
                AllowMainTermToBeRTL = MainRTL;
                AllowSecondTermToBeRTL = SecondRTL;
            }
        }

        private void FindAndCacheTarget(ref GameObject targetCache, DelegateSetFinalTerms setFinalTerms, DelegateDoLocalize doLocalize,
            bool UseSecondaryTerm, bool MainRTL, bool SecondRTL)
        {
            if (mTarget != targetCache && (bool)targetCache)
            {
                UnityEngine.Object.Destroy(targetCache);
            }
            if (mTarget != null)
            {
                targetCache = mTarget as GameObject;
            }
            else
            {
                Transform transform = base.transform;
                mTarget = (targetCache = ((transform.childCount >= 1) ? transform.GetChild(0).gameObject : null));
            }
            if (targetCache != null)
            {
                EventSetFinalTerms = setFinalTerms;
                EventDoLocalize = doLocalize;
                CanUseSecondaryTerm = UseSecondaryTerm;
                AllowMainTermToBeRTL = MainRTL;
                AllowSecondTermToBeRTL = SecondRTL;
            }
        }

        private bool HasTargetCache()
        {
            return EventDoLocalize != null;
        }

        public void GetFinalTerms(out string primaryTerm, out string secondaryTerm)
        {
            if (EventSetFinalTerms == null || (!mTarget && !HasTargetCache()))
            {
                FindTarget();
            }
            primaryTerm = string.Empty;
            secondaryTerm = string.Empty;
            if (mTarget != null && (string.IsNullOrEmpty(mTerm) || string.IsNullOrEmpty(mTermSecondary)) && EventSetFinalTerms != null)
            {
                EventSetFinalTerms(mTerm, mTermSecondary, out primaryTerm, out secondaryTerm, true);
            }
            if (!string.IsNullOrEmpty(mTerm))
            {
                primaryTerm = mTerm;
            }
            if (!string.IsNullOrEmpty(mTermSecondary))
            {
                secondaryTerm = mTermSecondary;
            }
            if (primaryTerm != null)
            {
                primaryTerm = primaryTerm.Trim();
            }
            if (secondaryTerm != null)
            {
                secondaryTerm = secondaryTerm.Trim();
            }
        }

        public string GetMainTargetsText(bool RemoveNonASCII)
        {
            string primaryTerm = null;
            string secondaryTerm = null;
            if (EventSetFinalTerms != null)
            {
                EventSetFinalTerms(null, null, out primaryTerm, out secondaryTerm, RemoveNonASCII);
            }
            return (!string.IsNullOrEmpty(primaryTerm)) ? primaryTerm : mTerm;
        }

        private void SetFinalTerms(string Main, string Secondary, out string primaryTerm, out string secondaryTerm, bool RemoveNonASCII)
        {
            primaryTerm = ((!RemoveNonASCII || string.IsNullOrEmpty(Main)) ? Main : Regex.Replace(Main, "[^a-zA-Z0-9_ ]+", " "));
            secondaryTerm = Secondary;
        }

        public void SetTerm(string primary)
        {
            if (!string.IsNullOrEmpty(primary))
            {
                FinalTerm = (mTerm = primary);
            }
            OnLocalize(true);
        }

        public void SetTerm(string primary, string secondary)
        {
            if (!string.IsNullOrEmpty(primary))
            {
                FinalTerm = (mTerm = primary);
            }
            FinalSecondaryTerm = (mTermSecondary = secondary);
            OnLocalize(true);
        }

        private T GetSecondaryTranslatedObj<T>(ref string mainTranslation, ref string secondaryTranslation) where T : UnityEngine.Object
        {
            string value;
            string secondary;
            DeserializeTranslation(mainTranslation, out value, out secondary);
            T val = (T)null;
            if (!string.IsNullOrEmpty(secondary))
            {
                val = GetObject<T>(secondary);
                if (val != null)
                {
                    mainTranslation = value;
                    secondaryTranslation = secondary;
                }
            }
            if (val == null)
            {
                val = GetObject<T>(secondaryTranslation);
            }
            return val;
        }

        private T GetObject<T>(string Translation) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(Translation))
            {
                return (T)null;
            }
            T translatedObject = GetTranslatedObject<T>(Translation);
            if (translatedObject == null)
            {
                translatedObject = GetTranslatedObject<T>(Translation);
            }
            return translatedObject;
        }

        private T GetTranslatedObject<T>(string Translation) where T : UnityEngine.Object
        {
            return FindTranslatedObject<T>(Translation);
        }

        private void DeserializeTranslation(string translation, out string value, out string secondary)
        {
            if (!string.IsNullOrEmpty(translation) && translation.Length > 1 && translation[0] == '[')
            {
                int num = translation.IndexOf(']');
                if (num > 0)
                {
                    secondary = translation.Substring(1, num - 1);
                    value = translation.Substring(num + 1);
                    return;
                }
            }
            value = translation;
            secondary = string.Empty;
        }

        public T FindTranslatedObject<T>(string value) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(value))
            {
                return (T)null;
            }
            if (TranslatedObjects != null)
            {
                int i = 0;
                for (int num = TranslatedObjects.Length; i < num; i++)
                {
                    if (TranslatedObjects[i] is T && value.EndsWith(TranslatedObjects[i].name, StringComparison.OrdinalIgnoreCase) &&
                        string.Compare(value, TranslatedObjects[i].name, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return (T)TranslatedObjects[i];
                    }
                }
            }
            T val = LocalizationManager.FindAsset(value) as T;
            if ((bool)val)
            {
                return val;
            }
            return ResourceManager.pInstance.GetAsset<T>(value);
        }

        public bool HasTranslatedObject(UnityEngine.Object Obj)
        {
            if (Array.IndexOf(TranslatedObjects, Obj) >= 0)
            {
                return true;
            }
            return ResourceManager.pInstance.HasAsset(Obj);
        }

        public void AddTranslatedObject(UnityEngine.Object Obj)
        {
            Array.Resize(ref TranslatedObjects, TranslatedObjects.Length + 1);
            TranslatedObjects[TranslatedObjects.Length - 1] = Obj;
        }

        public void SetGlobalLanguage(string Language)
        {
            LocalizationManager.CurrentLanguage = Language;
        }

        public static void RegisterEvents_2DToolKit()
        {
        }

        public static void RegisterEvents_DFGUI()
        {
        }

        public static void RegisterEvents_NGUI()
        {
        }

        public static void RegisterEvents_SVG()
        {
        }

        public static void RegisterEvents_TextMeshPro()
        {
        }

        public void RegisterEvents_UGUI()
        {
            EventFindTarget += FindTarget_uGUI_Text;
            EventFindTarget += FindTarget_uGUI_Image;
            EventFindTarget += FindTarget_uGUI_RawImage;
        }

        private void FindTarget_uGUI_Text()
        {
            FindAndCacheTarget(ref mTarget_uGUI_Text, SetFinalTerms_uGUI_Text, DoLocalize_uGUI_Text, true, true, false);
        }

        private void FindTarget_uGUI_Image()
        {
            FindAndCacheTarget(ref mTarget_uGUI_Image, SetFinalTerms_uGUI_Image, DoLocalize_uGUI_Image, false, false, false);
        }

        private void FindTarget_uGUI_RawImage()
        {
            FindAndCacheTarget(ref mTarget_uGUI_RawImage, SetFinalTerms_uGUI_RawImage, DoLocalize_uGUI_RawImage, false, false, false);
        }

        private void SetFinalTerms_uGUI_Text(string Main, string Secondary, out string primaryTerm, out string secondaryTerm, bool RemoveNonASCII)
        {
            string secondary = ((!(mTarget_uGUI_Text.font != null)) ? string.Empty : mTarget_uGUI_Text.font.name);
            SetFinalTerms(mTarget_uGUI_Text.text, secondary, out primaryTerm, out secondaryTerm, RemoveNonASCII);
        }

        public void SetFinalTerms_uGUI_Image(string Main, string Secondary, out string primaryTerm, out string secondaryTerm, bool RemoveNonASCII)
        {
            SetFinalTerms((!mTarget_uGUI_Image.mainTexture) ? string.Empty : mTarget_uGUI_Image.mainTexture.name, null, out primaryTerm,
                out secondaryTerm, false);
        }

        public void SetFinalTerms_uGUI_RawImage(string Main, string Secondary, out string primaryTerm, out string secondaryTerm, bool RemoveNonASCII)
        {
            SetFinalTerms((!mTarget_uGUI_RawImage.texture) ? string.Empty : mTarget_uGUI_RawImage.texture.name, null, out primaryTerm,
                out secondaryTerm, false);
        }

        public static T FindInParents<T>(Transform tr) where T : Component
        {
            if (!tr)
            {
                return (T)null;
            }
            T component = tr.GetComponent<T>();
            while (!component && (bool)tr)
            {
                component = tr.GetComponent<T>();
                tr = tr.parent;
            }
            return component;
        }

        public void DoLocalize_uGUI_Text(string mainTranslation, string secondaryTranslation)
        {
            Font secondaryTranslatedObj = GetSecondaryTranslatedObj<Font>(ref mainTranslation, ref secondaryTranslation);
            if (secondaryTranslatedObj != null && secondaryTranslatedObj != mTarget_uGUI_Text.font)
            {
                mTarget_uGUI_Text.font = secondaryTranslatedObj;
            }
            if (mInitializeAlignment)
            {
                mInitializeAlignment = false;
                mAlignmentUGUIwasRTL = LocalizationManager.IsRight2Left;
                InitAlignment_UGUI(mAlignmentUGUIwasRTL, mTarget_uGUI_Text.alignment, out mAlignmentUGUI_LTR, out mAlignmentUGUI_RTL);
            }
            else
            {
                TextAnchor alignLTR;
                TextAnchor alignRTL;
                InitAlignment_UGUI(mAlignmentUGUIwasRTL, mTarget_uGUI_Text.alignment, out alignLTR, out alignRTL);
                if ((mAlignmentUGUIwasRTL && mAlignmentUGUI_RTL != alignRTL) || (!mAlignmentUGUIwasRTL && mAlignmentUGUI_LTR != alignLTR))
                {
                    mAlignmentUGUI_LTR = alignLTR;
                    mAlignmentUGUI_RTL = alignRTL;
                }
                mAlignmentUGUIwasRTL = LocalizationManager.IsRight2Left;
            }
            if (mainTranslation != null && mTarget_uGUI_Text.text != mainTranslation)
            {
                if (CurrentLocalizeComponent.CorrectAlignmentForRTL)
                {
                    mTarget_uGUI_Text.alignment = ((!LocalizationManager.IsRight2Left) ? mAlignmentUGUI_LTR : mAlignmentUGUI_RTL);
                }
                mTarget_uGUI_Text.text = mainTranslation;
                mTarget_uGUI_Text.SetVerticesDirty();
            }
        }

        private void InitAlignment_UGUI(bool isRTL, TextAnchor alignment, out TextAnchor alignLTR, out TextAnchor alignRTL)
        {
            alignLTR = (alignRTL = alignment);
            if (isRTL)
            {
                switch (alignment)
                {
                    case TextAnchor.UpperRight:
                        alignLTR = TextAnchor.UpperLeft;
                        break;
                    case TextAnchor.MiddleRight:
                        alignLTR = TextAnchor.MiddleLeft;
                        break;
                    case TextAnchor.LowerRight:
                        alignLTR = TextAnchor.LowerLeft;
                        break;
                    case TextAnchor.UpperLeft:
                        alignLTR = TextAnchor.UpperRight;
                        break;
                    case TextAnchor.MiddleLeft:
                        alignLTR = TextAnchor.MiddleRight;
                        break;
                    case TextAnchor.LowerLeft:
                        alignLTR = TextAnchor.LowerRight;
                        break;
                    case TextAnchor.UpperCenter:
                    case TextAnchor.MiddleCenter:
                    case TextAnchor.LowerCenter:
                        break;
                }
            }
            else
            {
                switch (alignment)
                {
                    case TextAnchor.UpperRight:
                        alignRTL = TextAnchor.UpperLeft;
                        break;
                    case TextAnchor.MiddleRight:
                        alignRTL = TextAnchor.MiddleLeft;
                        break;
                    case TextAnchor.LowerRight:
                        alignRTL = TextAnchor.LowerLeft;
                        break;
                    case TextAnchor.UpperLeft:
                        alignRTL = TextAnchor.UpperRight;
                        break;
                    case TextAnchor.MiddleLeft:
                        alignRTL = TextAnchor.MiddleRight;
                        break;
                    case TextAnchor.LowerLeft:
                        alignRTL = TextAnchor.LowerRight;
                        break;
                    case TextAnchor.UpperCenter:
                    case TextAnchor.MiddleCenter:
                    case TextAnchor.LowerCenter:
                        break;
                }
            }
        }

        public void DoLocalize_uGUI_Image(string mainTranslation, string secondaryTranslation)
        {
            Sprite sprite = mTarget_uGUI_Image.sprite;
            if (sprite == null || sprite.name != mainTranslation)
            {
                mTarget_uGUI_Image.sprite = FindTranslatedObject<Sprite>(mainTranslation);
            }
        }

        public void DoLocalize_uGUI_RawImage(string mainTranslation, string secondaryTranslation)
        {
            Texture texture = mTarget_uGUI_RawImage.texture;
            if (texture == null || texture.name != mainTranslation)
            {
                mTarget_uGUI_RawImage.texture = FindTranslatedObject<Texture>(mainTranslation);
            }
        }

        public void RegisterEvents_UnityStandard()
        {
            EventFindTarget += FindTarget_GUIText;
            EventFindTarget += FindTarget_TextMesh;
            EventFindTarget += FindTarget_AudioSource;
            EventFindTarget += FindTarget_GUITexture;
            EventFindTarget += FindTarget_Child;
            EventFindTarget += FindTarget_SpriteRenderer;
        }

        private void FindTarget_GUIText()
        {
            FindAndCacheTarget(ref mTarget_GUIText, SetFinalTerms_GUIText, DoLocalize_GUIText, true, true, false);
        }

        private void FindTarget_TextMesh()
        {
            FindAndCacheTarget(ref mTarget_TextMesh, SetFinalTerms_TextMesh, DoLocalize_TextMesh, true, true, false);
        }

        private void FindTarget_AudioSource()
        {
            FindAndCacheTarget(ref mTarget_AudioSource, SetFinalTerms_AudioSource, DoLocalize_AudioSource, false, false, false);
        }

        private void FindTarget_GUITexture()
        {
            FindAndCacheTarget(ref mTarget_GUITexture, SetFinalTerms_GUITexture, DoLocalize_GUITexture, false, false, false);
        }

        private void FindTarget_Child()
        {
            FindAndCacheTarget(ref mTarget_Child, SetFinalTerms_Child, DoLocalize_Child, false, false, false);
        }

        private void FindTarget_SpriteRenderer()
        {
            FindAndCacheTarget(ref mTarget_SpriteRenderer, SetFinalTerms_SpriteRenderer, DoLocalize_SpriteRenderer, false, false, false);
        }

        public void SetFinalTerms_GUIText(string Main, string Secondary, out string PrimaryTerm, out string secondaryTranslation, bool RemoveNonASCII)
        {
            if (string.IsNullOrEmpty(Secondary) && mTarget_GUIText.font != null)
            {
                Secondary = mTarget_GUIText.font.name;
            }
            SetFinalTerms(mTarget_GUIText.text, Secondary, out PrimaryTerm, out secondaryTranslation, RemoveNonASCII);
        }

        public void SetFinalTerms_TextMesh(string Main, string Secondary, out string PrimaryTerm, out string secondaryTranslation,
            bool RemoveNonASCII)
        {
            string secondary = ((!(mTarget_TextMesh.font != null)) ? string.Empty : mTarget_TextMesh.font.name);
            SetFinalTerms(mTarget_TextMesh.text, secondary, out PrimaryTerm, out secondaryTranslation, RemoveNonASCII);
        }

        public void SetFinalTerms_GUITexture(string Main, string Secondary, out string PrimaryTerm, out string secondaryTranslation,
            bool RemoveNonASCII)
        {
            if (!mTarget_GUITexture || !mTarget_GUITexture.sprite)
            {
                SetFinalTerms(string.Empty, string.Empty, out PrimaryTerm, out secondaryTranslation, false);
            }
            else
            {
                SetFinalTerms(mTarget_GUITexture.sprite.name, string.Empty, out PrimaryTerm, out secondaryTranslation, false);
            }
        }

        public void SetFinalTerms_AudioSource(string Main, string Secondary, out string PrimaryTerm, out string secondaryTranslation,
            bool RemoveNonASCII)
        {
            if (!mTarget_AudioSource || !mTarget_AudioSource.clip)
            {
                SetFinalTerms(string.Empty, string.Empty, out PrimaryTerm, out secondaryTranslation, false);
            }
            else
            {
                SetFinalTerms(mTarget_AudioSource.clip.name, string.Empty, out PrimaryTerm, out secondaryTranslation, false);
            }
        }

        public void SetFinalTerms_Child(string Main, string Secondary, out string PrimaryTerm, out string secondaryTranslation, bool RemoveNonASCII)
        {
            SetFinalTerms(mTarget_Child.name, string.Empty, out PrimaryTerm, out secondaryTranslation, false);
        }

        public void SetFinalTerms_SpriteRenderer(string Main, string Secondary, out string PrimaryTerm, out string secondaryTranslation,
            bool RemoveNonASCII)
        {
            SetFinalTerms((!(mTarget_SpriteRenderer.sprite != null)) ? string.Empty : mTarget_SpriteRenderer.sprite.name, string.Empty,
                out PrimaryTerm, out secondaryTranslation, false);
        }

        private void DoLocalize_GUIText(string mainTranslation, string secondaryTranslation)
        {
            Font secondaryTranslatedObj = GetSecondaryTranslatedObj<Font>(ref mainTranslation, ref secondaryTranslation);
            if (secondaryTranslatedObj != null && mTarget_GUIText.font != secondaryTranslatedObj)
            {
                mTarget_GUIText.font = secondaryTranslatedObj;
            }
            if (mInitializeAlignment)
            {
                mInitializeAlignment = false;
                mAlignmentStd_LTR = (mAlignmentStd_RTL = (TextAlignment)mTarget_GUIText.alignment);
                if (LocalizationManager.IsRight2Left && mAlignmentStd_RTL == TextAlignment.Right)
                {
                    mAlignmentStd_LTR = TextAlignment.Left;
                }
                if (!LocalizationManager.IsRight2Left && mAlignmentStd_LTR == TextAlignment.Left)
                {
                    mAlignmentStd_RTL = TextAlignment.Right;
                }
            }
            if (mainTranslation != null && mTarget_GUIText.text != mainTranslation)
            {
                if (CurrentLocalizeComponent.CorrectAlignmentForRTL && mTarget_GUIText.alignment != (TextAnchor)TextAlignment.Center)
                {
                    mTarget_GUIText.alignment = (TextAnchor)((!LocalizationManager.IsRight2Left) ? mAlignmentStd_LTR : mAlignmentStd_RTL);
                }
                mTarget_GUIText.text = mainTranslation;
            }
        }

        private void DoLocalize_TextMesh(string mainTranslation, string secondaryTranslation)
        {
            Font secondaryTranslatedObj = GetSecondaryTranslatedObj<Font>(ref mainTranslation, ref secondaryTranslation);
            if (secondaryTranslatedObj != null && mTarget_TextMesh.font != secondaryTranslatedObj)
            {
                mTarget_TextMesh.font = secondaryTranslatedObj;
                GetComponent<Renderer>().sharedMaterial = secondaryTranslatedObj.material;
            }
            if (mInitializeAlignment)
            {
                mInitializeAlignment = false;
                mAlignmentStd_LTR = (mAlignmentStd_RTL = mTarget_TextMesh.alignment);
                if (LocalizationManager.IsRight2Left && mAlignmentStd_RTL == TextAlignment.Right)
                {
                    mAlignmentStd_LTR = TextAlignment.Left;
                }
                if (!LocalizationManager.IsRight2Left && mAlignmentStd_LTR == TextAlignment.Left)
                {
                    mAlignmentStd_RTL = TextAlignment.Right;
                }
            }
            if (mainTranslation != null && mTarget_TextMesh.text != mainTranslation)
            {
                if (CurrentLocalizeComponent.CorrectAlignmentForRTL && mTarget_TextMesh.alignment != TextAlignment.Center)
                {
                    mTarget_TextMesh.alignment = ((!LocalizationManager.IsRight2Left) ? mAlignmentStd_LTR : mAlignmentStd_RTL);
                }
                mTarget_TextMesh.text = mainTranslation;
            }
        }

        private void DoLocalize_AudioSource(string mainTranslation, string secondaryTranslation)
        {
            bool isPlaying = mTarget_AudioSource.isPlaying;
            AudioClip clip = mTarget_AudioSource.clip;
            AudioClip audioClip = FindTranslatedObject<AudioClip>(mainTranslation);
            if (clip != audioClip)
            {
                mTarget_AudioSource.clip = audioClip;
            }
            if (isPlaying && (bool)mTarget_AudioSource.clip)
            {
                mTarget_AudioSource.Play();
            }
        }

        private void DoLocalize_GUITexture(string mainTranslation, string secondaryTranslation)
        {
            Sprite texture = mTarget_GUITexture.sprite;
            if (texture != null && texture.name != mainTranslation)
            {
                mTarget_GUITexture.sprite = FindTranslatedObject<Sprite>(mainTranslation);
            }
        }

        private void DoLocalize_Child(string mainTranslation, string secondaryTranslation)
        {
            if (!mTarget_Child || !(mTarget_Child.name == mainTranslation))
            {
                GameObject gameObject = mTarget_Child;
                GameObject gameObject2 = FindTranslatedObject<GameObject>(mainTranslation);
                if ((bool)gameObject2)
                {
                    mTarget_Child = UnityEngine.Object.Instantiate(gameObject2);
                    Transform transform = mTarget_Child.transform;
                    Transform transform2 = ((!gameObject) ? gameObject2.transform : gameObject.transform);
                    transform.SetParent(base.transform);
                    transform.localScale = transform2.localScale;
                    transform.localRotation = transform2.localRotation;
                    transform.localPosition = transform2.localPosition;
                }
                if ((bool)gameObject)
                {
                    UnityEngine.Object.Destroy(gameObject);
                }
            }
        }

        private void DoLocalize_SpriteRenderer(string mainTranslation, string secondaryTranslation)
        {
            Sprite sprite = mTarget_SpriteRenderer.sprite;
            if (sprite == null || sprite.name != mainTranslation)
            {
                mTarget_SpriteRenderer.sprite = FindTranslatedObject<Sprite>(mainTranslation);
            }
        }
    }
}