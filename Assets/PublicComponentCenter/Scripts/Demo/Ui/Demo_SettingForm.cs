//------------------------------------------------------------
// Copyright © 2020-2021 Lefend. All rights reserved.
//------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using PublicComponentCenter;
using UnityEngine;
using UnityEngine.UI;

public class Demo_SettingForm : UGuiForm
{
    #region Properties

    [SerializeField] private CanvasGroup m_LanguageTipsCanvasGroup = null;
    [SerializeField] private Toggle m_EnglishToggle = null;

    [SerializeField] private Toggle m_ChineseSimplifiedToggle = null;
    private Language m_SelectedLanguage = Language.Unspecified;
    [SerializeField] private GameObject m_LanguageSettings;

    #endregion

    #region Mono

    protected override void Start()
    {
        base.Start();
        m_SelectedLanguage = GameEntry.Localization.Language;
        m_EnglishToggle.onValueChanged.AddListener(OnEnglishSelected);
        m_ChineseSimplifiedToggle.onValueChanged.AddListener(OnChineseSimplifiedSelected);
        switch (m_SelectedLanguage)
        {
            case Language.English:
                m_EnglishToggle.isOn = true;
                break;

            case Language.ChineseSimplified:
                m_ChineseSimplifiedToggle.isOn = true;
                break;
            default:
                break;
        }
    }

    void OnEnable()
    {
        m_LanguageSettings.SetActive(GameEntry.Localization.AllowSetLanguage);
    }

    void Update()
    {
        if (m_LanguageTipsCanvasGroup.gameObject.activeSelf)
        {
            m_LanguageTipsCanvasGroup.alpha = 0.5f + 0.5f * Mathf.Sin(Mathf.PI * Time.time);
        }
    }

    #endregion

    #region BtnEvent

    public void OnBtnSubmitClick()
    {
        if (m_SelectedLanguage == GameEntry.Localization.Language)
        {
            gameObject.SetActive(false);
            return;
        }

        GameEntry.Localization.ChangeLanguage(m_SelectedLanguage);
        gameObject.SetActive(false);
    }

    public void OnChineseSimplifiedSelected(bool isOn)
    {
        OnSelectedLanguageChanged(isOn, Language.ChineseSimplified);
    }

    public void OnEnglishSelected(bool isOn)
    {
        OnSelectedLanguageChanged(isOn, Language.English);
    }

    #endregion

    #region Helper

    private void OnSelectedLanguageChanged(bool isOn, Language language)
    {
        if (!isOn) return;
        m_SelectedLanguage = language;
        //Debug.LogFormat("选择的语言是：{0}", language.ToString());
        RefreshLanguageTips();
    }

    private void RefreshLanguageTips()
    {
        m_LanguageTipsCanvasGroup.gameObject.SetActive(m_SelectedLanguage != GameEntry.Localization.Language);
    }

    #endregion
}