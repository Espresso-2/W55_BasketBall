using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AdGeneric;
using AdGeneric.Ext;
using AdGeneric.Operation;
using AdGeneric.SO;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AdTotalManager : BaseOperation
{
    public static AdTotalManager Instance { get; private set; }
    [Header("不用开编辑器也能修改")]
    [Header("从AdGeneric/Operation/Impl下选择运营脚本")]
    [Header("记得换标题")]
    [Header("白包屏蔽时间")] 
    [SerializeField] private AdDateTime adTime=AdUtils.GetShieldTime();
    [SerializeField] private Orientation 屏幕方向=global::AdGeneric.Operation.Orientation.横屏 ;

    public string ShowDataTime => $"{adTime:yyyy-MM-dd HH:mm:ss}";
    public int Orientation => (int) 屏幕方向;
    
    #region operation

    public Operation Operation { get; private set; }

    #endregion

    #region misc

    [Header("其他修改项")] [SerializeField] private Addition 附加项=Addition.宝箱|Addition.护眼;

    public Addition Addition => 附加项;

    public AdSO Database { get; private set; }
    
    private BaseOperation CurrentOperation { get; set; }
    #endregion

    private void Awake()
    {
        Database = Resources.Load<AdSO>("AdSO");
        if (Instance == null) Instance = this;
        DontDestroyOnLoad(gameObject);
        SetCurrentOperation();
    }

    private void SetCurrentOperation()
    {
        
        CurrentOperation= 
            gameObject
                .GetComponents<BaseOperation>()
                .First(e => e.GetType()
                    .IsDefined(typeof(OperationAttribute), true));
        Operation = CustomAttributeExtensions.GetCustomAttribute<OperationAttribute>(CurrentOperation.GetType()).Operation;
    }

    private Operation GetCurrentOperationType() => 
        CurrentOperation == null 
            ? Operation.Invalid 
            : CustomAttributeExtensions
                .GetCustomAttribute<OperationAttribute>(CurrentOperation
                    .GetType())
                .Operation;

    protected override void Start() => Init();

    public override void Init()
    {
        print(nameof(Init));
        CurrentOperation.Init();
    }

    public override void ShowBlackAd(AdSource source=AdSource.Generic)
    {
        if (!AdStarted) return;
        print(nameof(ShowBlackAd));
        CurrentOperation.ShowBlackAd(source);
    }

    public override void ShowWhiteAd(AdSource source=AdSource.Generic)
    {
        if (!AdStarted) return;
        print(nameof(ShowWhiteAd));
        CurrentOperation.ShowWhiteAd(source);
    }

    public override void Show(Addition addition)
    {
        if (!AdStarted) return;
        addition &=附加项;
        print($"{nameof(Show)} : {addition}");
        CurrentOperation.Show(addition);
    }

    public override void ShowRewardAd(string callBackObjectName, string callBackMethodName, string callBackParam = null,AdSource source=AdSource.Generic)
    {
        if (!AdStarted) return;
        print(nameof(ShowRewardAd));
        CurrentOperation.ShowRewardAd(callBackObjectName, callBackMethodName, callBackParam);
    }

    public override void CreateShortcutBlack()
    {
        if (!AdStarted) return;
        print(nameof(CreateShortcutBlack));
        CurrentOperation.CreateShortcutBlack();
    }

    public override void SimpleShortCurBlack()
    {
       CurrentOperation.SimpleShortCurBlack();
    }

    public void ShowBox() => Show(Addition.宝箱);
    public void ShowEye() => Show(Addition.护眼);
    public bool AdStarted { get;private set; }
    public void AdStart() => AdStarted = true;
	
	private void RewardCallback()=>ShowBlackAd();

#if UNITY_EDITOR
    [AdInspectorButton("设置屏蔽时间")]
    public void SetShieldTime()
    {
        Undo.RecordObject(this,nameof(SetShieldTime));
        adTime = AdUtils.GetShieldTime();
    }
#endif
    #region smart

#if false
    [SerializeField]private Operation 运营;
    private static IDictionary<Operation,Type> GetOperations()
    {
        var types = Assembly
            .GetAssembly(typeof(OperationAttribute))
            .GetTypes()
            .Where(e=>typeof(BaseOperation).IsAssignableFrom(e))
            .Where(e => e.IsDefined(typeof(OperationAttribute)));
        try
        {
            return types
                .ToDictionary(e => e.GetCustomAttribute<OperationAttribute>().Operation, e => e);
        }
        catch
        {
            $"存在重复的{typeof(OperationAttribute)}标记".LogError();
            return new Dictionary<Operation, Type>();
        }
    }


    private void OnValidate()
    {
        Operation curOp = 运营, preOp = GetCurrentOperationType();
        if (preOp==curOp) return;
        var operations = GetOperations();
        if (!operations.TryGetValue(curOp,out var type))
        {
            $"未定义的{curOp}".LogError();
            运营 = GetCurrentOperationType();
            return;
        }
        if (curOp == Operation.Invalid)
        {
            // Destroy(CurrentOperation);
            return;
        }
        var json = JsonUtility.ToJson(CurrentOperation);
        // Destroy(CurrentOperation);
        CurrentOperation= (BaseOperation)gameObject.AddComponent(type);
        try
        {
            JsonUtility.FromJsonOverwrite(json,CurrentOperation);
        }
        finally
        {
            "数据填充结束".Log();
        }
    }
#endif

    #endregion
}






