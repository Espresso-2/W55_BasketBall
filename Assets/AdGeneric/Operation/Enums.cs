using System;

namespace AdGeneric.Operation
{
    public enum Orientation
    {
        横屏=0,
        竖屏=1,
    }

    public enum Operation
    {
        Invalid,
        通用,
        玩伴,
        指点天下,
        烨子,
        万世荣,
        陈志晴,
        博恒,
        厦门,
        标越,
        深圳路总
    }
    
    public enum AdSource
    {
        Generic=0,
        GameWin=1<<0,
        GameLose=1<<1,
        SignUp=1<<2,
        Setting=1<<3,
        Pause=1<<4,
        Repeat=1<<5,
    }
}
[Flags]
public enum Addition
{
    护眼=1,
    宝箱=1<<1,
}