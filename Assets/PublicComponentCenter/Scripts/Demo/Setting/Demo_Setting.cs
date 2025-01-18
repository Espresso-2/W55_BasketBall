//------------------------------------------------------------
// Copyright © 2020-2021 Lefend. All rights reserved.
//------------------------------------------------------------

using PublicComponentCenter;
using UnityEngine;

public class Demo_Setting : MonoBehaviour
{
    private readonly string m_KeyInt = "Test Int";
    private readonly string m_KeyFloat = "Test Float";
    private readonly string m_KeyString = "Test String";
    private readonly string m_KeyBool = "Test Bool";

    private SettingTestObj m_DefaultSettingTestObj;

    void Start()
    {
        m_DefaultSettingTestObj = new SettingTestObj();
    }

    // Update is called once per frame
    void Update()
    {
        //--------------------------------- Bool ----------------------------------
        if (GetKey(KeyCode.LeftControl) && GetKeyDown(KeyCode.Q))
        {
            var res = GameEntry.Setting.GetBool(m_KeyBool, false);
            Debug.LogFormat("获取到的 Bool 为：{0}", res);
        }

        if (GetKey(KeyCode.LeftShift) && GetKeyDown(KeyCode.Q))
        {
            GameEntry.Setting.SetBool(m_KeyBool, true);
            Debug.LogFormat("设置 Bool 为：{0}", true);
        }

        //--------------------------------- String ----------------------------------
        if (GetKey(KeyCode.LeftControl) && GetKeyDown(KeyCode.W))
        {
            var res = GameEntry.Setting.GetString(m_KeyString, string.Empty);
            Debug.LogFormat("获取到的 String 为：{0}", res);
        }

        if (GetKeyDown(KeyCode.LeftShift) && GetKeyDown(KeyCode.W))
        {
            GameEntry.Setting.SetString(m_KeyString, "TestString");
            Debug.LogFormat("设置 String 为：{0}", true);
        }


        //--------------------------------- Int ----------------------------------
        if (GetKey(KeyCode.LeftControl) && GetKeyDown(KeyCode.E))
        {
            var res = GameEntry.Setting.GetInt(m_KeyInt, 0);
            Debug.LogFormat("获取到的 Int 为：{0}", res);
        }

        if (GetKey(KeyCode.LeftShift) && GetKeyDown(KeyCode.E))
        {
            GameEntry.Setting.SetInt(m_KeyInt, 10);
            Debug.LogFormat("设置 Int 为：{0}", 10);
        }

        //--------------------------------- Float ----------------------------------
        if (GetKey(KeyCode.LeftControl) && GetKeyDown(KeyCode.R))
        {
            var res = GameEntry.Setting.GetInt(m_KeyFloat, 0);
            Debug.LogFormat("获取到的 Float 为：{0}", res);
        }

        if (GetKey(KeyCode.LeftShift) && GetKeyDown(KeyCode.R))
        {
            GameEntry.Setting.SetInt(m_KeyFloat, 10);
            Debug.LogFormat("设置 Float 为：{0}", 10);
        }

        //--------------------------------- Object ----------------------------------
        if (GetKey(KeyCode.LeftControl) && GetKeyDown(KeyCode.T))
        {
            var res = GameEntry.Setting.GetObject(m_KeyFloat, m_DefaultSettingTestObj);
            Debug.LogFormat("获取到的 Object 为：{0}", res);
        }

        if (GetKey(KeyCode.LeftShift) && GetKeyDown(KeyCode.T))
        {
            SettingTestObj obj = new SettingTestObj("Jerry", 22);
            GameEntry.Setting.SetObject(m_KeyFloat, obj);
            Debug.LogFormat("设置 Object 为：{0}", obj);
        }

        //--------------------------------- Delete ----------------------------------
        if (GetKey(KeyCode.LeftShift) && GetKeyDown(KeyCode.I))
        {
            GameEntry.Setting.RemoveSetting(m_KeyInt);
            Debug.LogFormat("删除 key为{0}的 值", m_KeyInt);
        }

        if (GetKey(KeyCode.LeftShift) && GetKeyDown(KeyCode.A))
        {
            GameEntry.Setting.RemoveAllSettings();
            Debug.LogFormat("删除所有配置.");
        }
    }

    private bool GetKeyDown(KeyCode keyCode)
    {
        return Input.GetKeyDown(keyCode);
    }

    private bool GetKey(KeyCode keyCode)
    {
        return Input.GetKey(keyCode);
    }
}

public class SettingTestObj
{
    public string Name { get; set; }

    public int Age { get; set; }

    public SettingTestObj()
    {
    }

    public SettingTestObj(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public override string ToString()
    {
        return string.Format("Name is :{0},Age is:{1}", Name, Age);
    }
}