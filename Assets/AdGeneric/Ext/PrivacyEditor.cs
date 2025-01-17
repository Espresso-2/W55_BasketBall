#if UNITY_EDITOR

using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using AdGeneric;
using AdGeneric.SO;
using UnityEditor;
using UnityEngine;

public class PrivacyEditor : EditorWindow
{
    [MenuItem(AdUtils.PrivacyPath)]
    public static void ShowWindow()
    {
        var wnd = GetWindow<PrivacyEditor>();
        wnd.titleContent = new GUIContent("PrivacyEditor");
    }

    private AdSO Database { get; set; }

    private void Awake()
    {
        Database = Resources.Load<AdSO>("AdSO");
        privacy = Database.PrivacyText;
    }

    private static readonly Regex CompanyMatcher =
        new Regex(@"(?<=(选择由|规定了))([\u4e00-\u9fa5]*有限公司)");

    private static readonly Regex[] EmailMatcher =
        new[]
        {
            new Regex(@"(?<=客服邮箱[：\:]</strong>)(.*@.*)(?=</p>)"),
            new Regex(@"(?<=联系邮箱[：\:](</span><span>))(.*@.*)(?=</span></span>)"),
            new Regex(@"((?<=请通过)(.*@.*)(?=与我们联系))"),
        };


    private const string Email1 = "3082464425@qq.com";
    private const string Email2 = "gamekf_666@sina.com";
    private string www;
    private bool useFormat;
    private string privacy = "";
    private string company = "";
    private string email = "";
    private string phone = "";
    private Vector2 pos;

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        var titleStyle = GUI.skin.label;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.fontSize = 20;
        titleStyle.stretchWidth = true;
        GUILayout.Label("隐私政策", titleStyle);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        useFormat = GUILayout.Toggle(useFormat, "使用模板");
        GUILayout.EndHorizontal();


        GUILayout.Space(10);
        if (useFormat)
        {
            var label = GUI.skin.label;
            label.alignment = TextAnchor.MiddleRight;
            label.fontSize = 14;
            label.fixedWidth = 120;

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("网址", label);
            www = GUILayout.TextField(www);
            if (GUILayout.Button("访问")) Http(www);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Company", label);
            company = GUILayout.TextField(company);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Email", label);
            email = GUILayout.TextField(email);
            GUILayout.Space(40);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(Email1)) email = Email1;
            if (GUILayout.Button(Email2)) email = Email2;
            GUILayout.EndHorizontal();

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Phone", label);
            phone = GUILayout.TextField(phone);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            pos = EditorGUILayout.BeginScrollView(pos, false, true);
            privacy = EditorGUILayout.TextArea(privacy);
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();
        }

    }

    private async void Http(string s)
    {
        var client = new HttpClient();
        client.Timeout = new TimeSpan(0, 0, 5);
        var stringAsync = await client.GetStringAsync(s);
        var array = stringAsync.Split('\r', '\n').Where(e => !string.IsNullOrWhiteSpace(e)).ToArray();
        ShowNotification(new GUIContent("访问完成"));
        client.Dispose();
        new Thread(() =>
        {
            email = EmailMatcher.SelectMany(reg => array.Select(e => reg.Match(e)))
                .FirstOrDefault(match => match.Success)?.Value ?? "";
        }).Start();
        new Thread(() =>
        {
            company = array.Select(e => CompanyMatcher.Match(e)).FirstOrDefault(match => match.Success)?.Value ??
                      "";
        }).Start();
    }

    private void OnLostFocus() => Save();

    private void Save()
    {
        string s = useFormat
            ? string.Format(
                AdSO.PrimaryFormat,
                string.IsNullOrWhiteSpace(company) ? "" : company,
                string.IsNullOrWhiteSpace(email) ? "" : string.Format(AdSO.EmailFormat, email),
                string.IsNullOrWhiteSpace(phone) ? "" : string.Format(AdSO.PhoneFormat, phone)
            )
            : privacy;
#if UNITY_2019_4_OR_NEWER
        EditorUtility.ClearDirty(Database);
#endif
        Database.PrivacyText = s;
        EditorUtility.SetDirty(Database);
    }

    private void OnDestroy() => Save();
}
#endif
