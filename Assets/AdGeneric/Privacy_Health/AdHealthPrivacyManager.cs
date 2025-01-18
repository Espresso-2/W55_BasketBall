using System;
using System.Collections;
using AdGeneric;
using AdGeneric.Ext;
using AdGeneric.SO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DisplayAttribute=AdGeneric.Ext.DisplayAttribute;


public class AdHealthPrivacyManager : MonoBehaviour
{
    #region 绑定

    [Header("绑定")]
    
    [SerializeField] private GameObject privacyObj;
    [SerializeField] private GameObject healthObj;
    [SerializeField] private GameObject eventSystemObj;

    [SerializeField] private GameObject imageObj;
    
    [SerializeField] private GameObject healthTextObj;
    [SerializeField] private GameObject companyObj;
    [SerializeField] private GameObject codeObj;
    [SerializeField] private GameObject ageObj;
    
    [SerializeField] private Text privacyText;
    [SerializeField] private GameObject privacyAgreeObj;
    [SerializeField] private GameObject privacyRejectObj;

    #endregion

#if UNITY_EDITOR
    [Header("如果没有下方按钮,隐私政策在"+AdUtils.PrivacyPath)]
#endif
    [Header("类型")]

    [SerializeField]private HealthType type=HealthType.Text;

    #region Image

    [SerializeField,Display(nameof(type),nameof(HealthType.Image))] private Sprite image;

    #endregion

    #region Text

    [SerializeField,Display(nameof(type),nameof(HealthType.Text))] private string 著作权人="";
    [SerializeField,Display(nameof(type),nameof(HealthType.Text))] private string 登记号="";
    [SerializeField,Display(nameof(type),nameof(HealthType.Text))] private Age age=Age.Twelve;
    
    private string CompanyText => $"著作权人:{著作权人}";
    private string CodeText => $"登记号:{登记号}";

    #endregion

    private const string KeyAgree = "1001";
    public static AdHealthPrivacyManager Instance { get; private set; }
    private AdSO Database => AdTotalManager.Instance.Database;
    private void Awake()
    {
        Instance = this;
        PlayerPrefs.SetInt(KeyAgree,PlayerPrefs.GetInt(KeyAgree));
    }

    private void Start()
    {
        Init();
        privacyObj.SetActive(false);
        healthObj.SetActive(false);
        bool isAgree = PlayerPrefs.GetInt(KeyAgree) == 1;
        if (isAgree) StartCoroutine(ShowHealthUI());
        else ShowPrivacy(PrivacyOption.Agree|PrivacyOption.Reject);
    }

    public void ShowPrivacy(PrivacyOption option=PrivacyOption.Agree|PrivacyOption.Reject)
    {
        privacyAgreeObj.SetActive((option & PrivacyOption.Agree) != 0);
        privacyRejectObj.SetActive((option & PrivacyOption.Reject) != 0);
        privacyObj.SetActive(true);
    }
    public void Agree()
    {
        bool isAgree = PlayerPrefs.GetInt(KeyAgree) == 1;
        privacyObj.SetActive(false);
        if (isAgree) return;
        PlayerPrefs.SetInt(KeyAgree, 1);
		if(AdTotalManager.Instance.AdStarted)return;
        StartCoroutine(ShowHealthUI());
    }

    public void Reject() => AdAdapter.ExitApplication();

    private IEnumerator ShowHealthUI()
    {
        healthObj.SetActive(true);
        bool completed = false;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex+1).completed += op => completed = op.isDone;
        yield return new WaitForSeconds(3);
        while (!completed) yield return null;
        healthObj.SetActive(false);
        AdTotalManager.Instance.AdStart();
        AdTotalManager.Instance.CreateShortcutBlack();
    }
    private void CorrectEventSystem(Scene s, LoadSceneMode m)
    {
        var esCount = GameObject.FindObjectsOfType<EventSystem>().Length;
        if (esCount < 1) eventSystemObj.SetActive(true);
        else if (esCount > 1) eventSystemObj.SetActive(false);
    }

    private void OnEnable() => SceneManager.sceneLoaded += CorrectEventSystem;

    private void OnDisable() => SceneManager.sceneLoaded -= CorrectEventSystem;
    private void Init()
    {
        privacyText.text = Database.PrivacyText;
        healthTextObj.GetComponent<Text>().text = Database.HealthText;
        var healthImage = imageObj.GetComponent<Image>();
        switch (type)
        {
            case HealthType.Image:
                healthImage.color=Color.white;
                healthImage.sprite = image;
                healthTextObj.SetActive(false);
                companyObj.SetActive(false);
                codeObj.SetActive(false);
                ageObj.SetActive(false);
                break;
            case HealthType.Text:
                healthImage.color=Color.black;
                companyObj.GetComponent<Text>().text = CompanyText;
                codeObj.GetComponent<Text>().text = CodeText;
                switch (age)
                {
                    case Age.Twelve:
                        ageObj.GetComponent<Image>().sprite = Database.Twelve;
                        break;
                    case Age.Sixteen:
                        ageObj.GetComponent<Image>().sprite = Database.Sixteen;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                healthTextObj.SetActive(true);
                companyObj.SetActive(true);
                codeObj.SetActive(true);
                ageObj.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    

#if UNITY_EDITOR
    [AdInspectorButton("打开编辑器窗口")]
    public static void OpenPrivacyWindow()
    {
        PrivacyEditor.ShowWindow();
    }
#endif
}

public enum HealthType
{
    Image,
    Text
}

public enum Age
{
    Twelve,
    Sixteen
}
[Flags]
public enum PrivacyOption
{
    Agree=1,
    Reject=1<<1,
}

