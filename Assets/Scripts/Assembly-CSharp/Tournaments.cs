using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tournaments : MonoBehaviour
{
    public static int numChampionships;

    public static int numTrophies;

    private List<Tournament> tournaments;

    public virtual void Awake()
    {
        InstantiateTournaments();
    }

    public virtual List<Tournament> GetTournaments()
    {
        return tournaments;
    }

    public virtual Tournament GetTournament(int num)
    {
        return tournaments[num];
    }

    public static int GetCurrentTournamentNum()
    {
        return PlayerPrefs.GetInt("TOURNAMENT_CURRENT_NUM");
    }

    public static void SetCurrentTournamentNum(int num)
    {
        PlayerPrefsHelper.SetInt("TOURNAMENT_CURRENT_NUM", num, true);
    }

    public static int GetCurrentRound(int num)
    {
        return PlayerPrefs.GetInt(Tournament.TOURNAMENT_CURRENT_ROUND_KEY + num);
    }

    public static bool IsReigningChamp(int num)
    {
        return PlayerPrefs.GetInt(Tournament.TOURNAMENT_REIGNING_CHAMP + num) == 1;
    }

    public static bool TournamentIsCompleted(int num)
    {
        return GetNumCompletions(num) >= 1;
    }

    public static int GetNumCompletions(int num)
    {
        return PlayerPrefs.GetInt(Tournament.TOURNAMENT_COMPLETED_KEY + num);
    }

    public virtual void InstantiateTournaments()
    {
        Debug.Log("--------Tournaments.InstantiateTournaments()----------------------");
        numChampionships = 0;
        numTrophies = 0;
        tournaments = new List<Tournament>();
        AddTournament(false, "明尼苏达州", -1, 800, 0, tournamentTypeEnum.ThreeRound, "小巷歌剧", "曼巴团队", "三巴勒兹", "色调转换器",
            "球王", "毒蛇队", "扣篮大师", ArenaChooser.GetMinnesotaArena());
        AddTournament(false, "威斯康星州", 0, 900, 0, tournamentTypeEnum.ThreeRound, "旋转蜘蛛", "扣篮者", "压哨高手", "狙击手",
            "纯投篮", "火球队", "伟大射手", 10);
        AddTournament(false, "密歇根州", 1, 1100, 0, tournamentTypeEnum.ThreeRound, "熊队", "篮球", "大扣篮2020", "红蟑螂",
            "投篮兄弟", "加州骑士", "扣篮联合", 33);
        AddTournament(false, "伊利诺伊州", 1, 1200, 0, tournamentTypeEnum.ThreeRound, "扣篮海报", "擦板得分", "蟒蛇队", "技术犯规者",
            "扣篮队", "野猪2018", "狙击王", 68);
        AddTournament(true, "夏洛特", 0, 1600, 0, tournamentTypeEnum.ThreeRound, "你会输", "欧洲队", "郊狼队", "幸运女士",
            "皇家袭击者", "交叉突破队", "旋转者", 12);
        AddTournament(true, "印第安纳州", 4, 1700, 0, tournamentTypeEnum.ThreeRound, "篮球粉碎者", "全明星", "蓝蟹队", "鳄鱼队",
            "野狗队", "科迪亚克熊队", "狼队", 26);
        AddTournament(false, "波士顿", 3, 1900, 0, tournamentTypeEnum.ThreeRound, "蟋蟀队", "泼水队", "投篮兄弟", "渔夫队",
            "袋熊队", "篮网鲨鱼", "斗牛犬队", 65);
        AddTournament(false, "辛辛那提", 5, 1200, 0, tournamentTypeEnum.ThreeRound, "突破脚踝者", "嚎叫者", "鲨鱼饵", "食人鱼队",
            "反转者", "轰炸者", "回旋队", 32);
        AddTournament(false, "匹兹堡", 6, 1100, 0, tournamentTypeEnum.ThreeRound, "篮筐破坏者", "鹦鹉队", "投篮爆破者", "寒意队",
            "大狗队", "轻松得分", "灵魂队", 66);
        AddTournament(false, "纽约市", 6, 2000, 0, tournamentTypeEnum.ThreeRound, "伟大者", "水母队", "场地风暴者", "海盗队",
            "篮板粉碎者", "篮球领袖", "翠鸟队", 46);
        AddTournament(true, "西弗吉尼亚州", 9, 1500, 0, tournamentTypeEnum.ThreeRound, "三分与防守", "风帆队", "捕手队", "英雄队",
            "蟒蛇队", "破坏者", "影子95队", 14);
        AddTournament(false, "费城", 9, 1200, 0, tournamentTypeEnum.ThreeRound, "团队计划", "阻挡者", "大球员", "影子队",
            "滑行者", "供应者", "细节大师", 64);
        AddTournament(true, "巴尔的摩", 11, 1900, 0, tournamentTypeEnum.ThreeRound, "大猩猩队", "三双团队", "猞猁队", "寻回者",
            "阻挡者", "36号队", "响尾蛇", 62);
        AddTournament(false, "肯塔基州", 4, 800, 0, tournamentTypeEnum.ThreeRound, "鸽子队", "蜗牛队", "辣椒99", "名人堂", "虎鲸队",
            "侏儒队", "小精灵队", 39);
        AddTournament(true, "弗吉尼亚州", 10, 900, 0, tournamentTypeEnum.ThreeRound, "野兔队", "眼镜蛇队", "甲虫队", "33号队",
            "野狗队", "章鱼队", "极速队", 28);
        AddTournament(false, "大西洋沿岸", 8, 1200, 0, tournamentTypeEnum.ThreeRound, "猎鹰队", "蓝蟹队", "篮球员", "疾风队", "斗牛犬队",
            "59号队", "尾巴队", 2);
        AddTournament(true, "田纳西州", 12, 1200, 0, tournamentTypeEnum.ThreeRound, "99号队", "蟑螂42", "红队", "掠食者", "饼干罐",
            "英雄队", "响尾蛇队", 7);
        AddTournament(false, "北卡罗来纳州", 16, 1200, 0, tournamentTypeEnum.ThreeRound, "蝙蝠队", "真正的家伙", "龙队", "猫头鹰队", "乌贼队",
            "巨嘴鸟98", "羚羊队", 29);
        AddTournament(true, "亚特兰大", 17, 1500, 0, tournamentTypeEnum.ThreeRound, "罗马队", "轰炸者", "英雄4队", "蓝蟹队", "蟑螂队",
            "九头蛇队", "冠军队", 1);
        AddTournament(false, "查尔斯顿", 17, 1100, 0, tournamentTypeEnum.ThreeRound, "笛手队", "伟人队", "雀队", "袋熊队", "变色龙队",
            "咆哮者", "43号队", 3);
        AddTournament(false, "杰克逊维尔", 16, 1200, 0, tournamentTypeEnum.ThreeRound, "彩虹吸蜜鹦鹉", "灵魂队", "伟大壁虎", "海盗队", "雉鸡队",
            "飞镖队", "鲨鱼队", 8);
        AddTournament(true, "新奥尔良", 16, 1400, 0, tournamentTypeEnum.ThreeRound, "毒蛇队", "鹦鹉队", "爆破者", "潜水员3000", "大狗队",
            "推特者", "狼蛛队", 16);
        AddTournament(false, "迈阿密", 10, 1500, 0, tournamentTypeEnum.ThreeRound, "蜘蛛队", "辣椒队", "爬行动物队", "猫头鹰队", "狮子队",
            "躲避者", "风暴者", 67);
        AddTournament(true, "阿拉巴马州", 22, 800, 0, tournamentTypeEnum.ThreeRound, "猎犬队", "秋田犬队", "郊狼队", "眼镜蛇队", "袭击者",
            "恐龙鸟队", "酷蟋蟀", 63);
        AddTournament(false, "密西西比", 20, 900, 0, tournamentTypeEnum.ThreeRound, "逃兵队", "移位者", "训练师", "忙碌者",
            "元素队", "弹跳者", "老鹰队", 20);
        AddTournament(false, "塔拉哈西", 20, 800, 0, tournamentTypeEnum.ThreeRound, "野狗队", "咆哮者", "猎豹队", "狙击手", "中子队",
            "狼队", "响尾蛇队", 54);
        AddTournament(true, "阿肯色州", 21, 800, 0, tournamentTypeEnum.ThreeRound, "蝙蝠队", "水母队", "龙队", "网猫头鹰", "乌贼队",
            "巨嘴鸟队", "羚羊队", 5);
        AddTournament(false, "堪萨斯城", 26, 1200, 0, tournamentTypeEnum.ThreeRound, "鲱鱼队", "帆船队", "捕手队", "英雄队", "蟒蛇队",
            "破碎者", "短吻鳄队", 31);
        AddTournament(false, "休斯顿", 26, 1600, 0, tournamentTypeEnum.ThreeRound, "蜘蛛队", "辣椒队", "爬行动物队", "猫头鹰队", "狮子队",
            "躲避者", "风暴者", 36);
        AddTournament(true, "达拉斯", 27, 1200, 0, tournamentTypeEnum.ThreeRound, "大猩猩队", "二传手", "猞猁队", "寻回者", "阻挡者",
            "36号队", "响尾蛇队", 45);
        AddTournament(false, "圣安东尼奥", 27, 1700, 0, tournamentTypeEnum.ThreeRound, "猎犬队", "秋田犬队", "郊狼队", "短吻鳄%", "袭击者",
            "恐龙鸟队", "猎人队", 47);
        AddTournament(true, "俄克拉荷马城", 28, 1600, 0, tournamentTypeEnum.ThreeRound, "长颈鹿队", "袋鼠队", "蓝蟹队", "鳄鱼队", "野狗队",
            "科迪亚克熊", "狼队", 49);
        AddTournament(false, "密苏里", 28, 1200, 0, tournamentTypeEnum.ThreeRound, "鸽子队", "蜗牛队", "辣椒队", "罗马队", "虎鲸队",
            "侏儒队", "小精灵队", 6);
        AddTournament(true, "内布拉斯加", 29, 1400, 0, tournamentTypeEnum.ThreeRound, "咆哮者", "袭击者", "斑马队", "老虎队", "火球队",
            "毒蛇队", "海豚队", 11);
        AddTournament(false, "爱荷华州", 29, 900, 0, tournamentTypeEnum.ThreeRound, "龙队", "帆船队", "捕手队", "英雄队", "蟒蛇99",
            "眼镜蛇队", "短吻鳄队", 15);
        AddTournament(false, "洛杉矶", 21, 1700, 0, tournamentTypeEnum.ThreeRound, "大狗队", "鹦鹉队", "爆破者", "别想赢", "破坏者",
            "推特者", "灵魂队", 34);
        AddTournament(true, "拉伯克", 34, 750, 0, tournamentTypeEnum.ThreeRound, "发电机队", "治疗者", "冒险者", "老虎队", "巨人队",
            "勇敢者", "冠军队", 35);
        AddTournament(false, "芝加哥", 25, 1400, 0, tournamentTypeEnum.ThreeRound, "蜘蛛队", "辣椒队", "爬行动物队", "猫头鹰队", "狮子队",
            "躲避者", "风暴者", 37);
        AddTournament(true, "北达科他州", 37, 700, 0, tournamentTypeEnum.ThreeRound, "追随者", "爱国者99", "信任者", "西部人", "梦想者",
            "毒蛇队", "海豚队", 52);
        AddTournament(false, "克利夫兰", 30, 1400, 0, tournamentTypeEnum.ThreeRound, "长颈鹿队", "狼队", "蓝蟹队", "鳄鱼队", "野狗队",
            "科迪亚克熊", "袋鼠队", 42);
        AddTournament(false, "奥克兰", 32, 1500, 0, tournamentTypeEnum.ThreeRound, "黑豹队", "谜团队", "斑马队", "老虎队", "火球队",
            "神奇篮球手", "海豚队", 38);
        AddTournament(true, "北加州", 36, 450, 0, tournamentTypeEnum.ThreeRound, "彩虹吸蜜鹦鹉", "灵魂队", "壁虎队", "海盗队", "雉鸡99",
            "飞镖X", "鲨鱼队", 17);
        AddTournament(false, "波特兰", 37, 1100, 0, tournamentTypeEnum.ThreeRound, "轰炸者", "嚎叫者", "金鱼", "织布工", "牧羊犬",
            "孔雀", "食人鱼", 29);
        AddTournament(true, "博伊西", 38, 1400, 0, tournamentTypeEnum.ThreeRound, "储物柜88", "猛禽", "龙", "狼蛛", "猎犬",
            "火烈鸟", "虎鲸", 40);
        AddTournament(true, "蒙大拿", 39, 650, 0, tournamentTypeEnum.ThreeRound, "蝙蝠", "水母", "龙", "猫头鹰", "鱿鱼", "巨嘴鸟88",
            "羚羊", 30);
        AddTournament(false, "盐湖城", 32, 1100, 0, tournamentTypeEnum.ThreeRound, "咆哮者X", "哇队", "斑马", "老虎", "音乐家",
            "毒蛇", "海豚", 53);
        AddTournament(true, "堪萨斯", 36, 1600, 0, tournamentTypeEnum.ThreeRound, "浣熊", "爬行者77", "乌鸦", "鱿鱼", "爬行动物", "大象",
            "青蛙", 19);
        AddTournament(false, "菲尼克斯", 38, 1400, 0, tournamentTypeEnum.ThreeRound, "钻石先生", "矿工", "升级者", "观察者", "牧羊人7",
            "扣篮者34", "狮子", 43);
        AddTournament(true, "丹佛", 37, 1200, 0, tournamentTypeEnum.ThreeRound, "爸爸队", "妈妈队34", "讲话者", "策划者", "指责者",
            "选秀坦克", "战斗者", 40);
        AddTournament(false, "拉斯维加斯", 39, 1200, 0, tournamentTypeEnum.ThreeRound, "拳击手2", "真实者", "治疗者", "付款者", "穿越者",
            "绘图者", "滑雪者", 4);
        AddTournament(false, "西雅图", 30, 1500, 0, tournamentTypeEnum.ThreeRound, "喷气机", "叶子", "石油工人", "幼崽", "时髦者", "发射者",
            "蜥蜴", 55);
        AddTournament(true, "俄勒冈", 41, 1200, 0, tournamentTypeEnum.ThreeRound, "企鹅", "猎鹰", "亡命之徒", "伐木工", "三叶草",
            "黑狼", "神枪手", 44);
        AddTournament(true, "里诺", 42, 1400, 0, tournamentTypeEnum.ThreeRound, "黑鹰", "军刀", "渡鸦队", "参议员", "流星队",
            "忍者", "靶手", 6);
        AddTournament(false, "内华达", 43, 1250, 0, tournamentTypeEnum.ThreeRound, "响尾蛇", "游侠", "勇士队", "比尔队", "豌豆射手34",
            "混乱者", "手榴弹队", 19);
        AddTournament(false, "萨克拉门托", 44, 1400, 0, tournamentTypeEnum.ThreeRound, "翅膀99", "怪物", "蜂拥者", "野猫", "德州人",
            "斯巴顿", "海狸", 41);
        AddTournament(true, "死亡谷", 35, 2400, 0, tournamentTypeEnum.ThreeRound, "蛇队", "山猫队", "蜘蛛", "响尾蛇", "野牛",
            "勇士", "美洲狮", 9);
        AddTournament(false, "圣迭戈", 36, 1400, 0, tournamentTypeEnum.ThreeRound, "火焰", "皇家队", "长螺母", "德州人", "松鼠",
            "边界者", "海蝙蝠", 8);
        AddTournament(true, "海峡群岛", 47, 800, 0, tournamentTypeEnum.ThreeRound, "游泳者", "海峡者", "鸭子", "潜水者", "捕鱼者",
            "鲸鱼", "冲浪者", 3);
        AddTournament(false, "卡塔利娜岛", 48, 900, 0, tournamentTypeEnum.ThreeRound, "岛民", "游泳者2000", "滨鸟", "光线队",
            "珊瑚礁", "太空人", "激流", 22);
        AddTournament(true, "圣尼古拉斯岛", 49, 1000, 0, tournamentTypeEnum.ThreeRound, "道奇队", "酿酒师", "鲨鱼", "海盗", "水手",
            "捕蟹者", "深海者", 23);
        AddTournament(false, "图森", 50, 1100, 0, tournamentTypeEnum.ThreeRound, "海盗", "老虎", "美洲虎！", "贪婪山羊", "反叛者队",
            "骆驼", "强盗", 24);
        AddTournament(true, "阿尔伯克基", 51, 1200, 0, tournamentTypeEnum.ThreeRound, "马林鱼", "海豚", "泰坦", "饼干", "跳跃母鸡",
            "熊猫队", "大黄蜂", 18);
        AddTournament(false, "亚利桑那", 52, 1300, 0, tournamentTypeEnum.ThreeRound, "闪电", "总统队", "开拓者", "游侠", "冲锋队！",
            "青蛙", "渡鸦", 48);
        AddTournament(false, "新墨西哥", 53, 1400, 0, tournamentTypeEnum.ThreeRound, "奇瓦瓦犬", "叛军", "潜行者", "火焰者", "捕手", "海盗", "道奇队", 1);
        AddTournament(true, "大峡谷", 38, 1300, 0, tournamentTypeEnum.ThreeRound, "雪崩队", "巨石队", "深潜者", "摇滚者", "河蛇", "沙龙队", "狂野者", 50);
        AddTournament(true, "鱼湖", 55, 1200, 0, tournamentTypeEnum.ThreeRound, "湾鹰队", "鲑鱼腹队", "海狗", "海猫", "鳟鱼2020", "湖猫", "湖熊", 2);
        AddTournament(false, "战山", 32, 1100, 0, tournamentTypeEnum.ThreeRound, "粗脖子", "攻击者", "冲锋者", "战士", "斗士", "胜者", "硬汉", 5);
        AddTournament(false, "盐沼地", 56, 1000, 0, tournamentTypeEnum.ThreeRound, "海猫", "海鱼", "海狼", "海鳟鱼", "海蛇", "海熊", "海岩", 11);
        AddTournament(true, "埃尔帕索", 57, 900, 0, tournamentTypeEnum.ThreeRound, "恶魔", "狙击手", "山羊", "教父队", "强盗", "德州人", "蛇", 13);
        AddTournament(true, "银城", 55, 800, 0, tournamentTypeEnum.ThreeRound, "银俱乐部", "美洲虎", "银背猩猩", "银蜂群", "银射手", "银蟒蛇", "银狗", 18);
        AddTournament(true, "泰坦堡", 58, 800, 0, tournamentTypeEnum.ThreeRound, "泰坦队", "湾鹰队", "手榴弹", "奇瓦瓦犬", "海盗", "潜行者", "叛军", 21);
        AddTournament(false, "战溪", 50, 900, 0, tournamentTypeEnum.ThreeRound, "战狗", "战猫", "战猪", "战蝙蝠", "战车", "坦克", "战童", 63);
        AddTournament(true, "特拉弗斯市", 60, 1000, 0, tournamentTypeEnum.ThreeRound, "火焰者", "军刀", "游侠", "发射者", "母鸡", "斯巴顿队", "猫头鹰", 51);
        AddTournament(false, "布法罗", 62, 1100, 0, tournamentTypeEnum.ThreeRound, "亡命之徒", "加拿大人", "猎鹰", "奇瓦瓦犬", "斗牛犬", "英雄", "水牛", 62);
        AddTournament(true, "锡拉丘兹", 63, 1200, 0, tournamentTypeEnum.ThreeRound, "巨龙", "火焰队", "总统队", "摇滚者", "跑者", "母鸡", "游侠", 69);
        AddTournament(true, "缅因州", 64, 1400, 0, tournamentTypeEnum.ThreeRound, "潜鸟", "军刀", "冲锋者", "乌鸦", "麋鹿", "坏猫", "缅因秀", 50);
        AddTournament(false, "佛蒙特州", 60, 1500, 0, tournamentTypeEnum.ThreeRound, "忍者", "牛仔", "发射者", "投球手", "斯巴顿队", "比尔队", "滑雪者", 72);
        AddTournament(true, "百慕大", 65, 1400, 0, tournamentTypeEnum.ThreeRound, "游泳者", "游客", "喷气鹰", "划船者", "海洋蛙", "冲锋者", "海狙击手", 67);
        AddTournament(false, "奥兰多", 66, 1300, 0, tournamentTypeEnum.ThreeRound, "蜂群", "大钞票", "投球手1985", "坏狗", "饼干队", "发射者", "游侠", 28);
        AddTournament(true, "代托纳海滩", 60, 1400, 0, tournamentTypeEnum.ThreeRound, "派对狂", "赛车手", "晒太阳者", "沙滩猪", "沙滩虎", "阳光射线", "游泳者", 34);
        AddTournament(true, "坦帕", 68, 1400, 0, tournamentTypeEnum.ThreeRound, "奇瓦瓦犬", "斯巴顿队", "眼镜蛇", "乌鸦", "风车手", "母鸡", "捕蟹人", 35);
        AddTournament(false, "弗里波特", 69, 1100, 0, tournamentTypeEnum.ThreeRound, "冲锋者", "美洲虎", "亡命之徒", "饼干队", "投球手", "喷气者", "摇滚者", 36);
        AddTournament(true, "沙角", 70, 1400, 0, tournamentTypeEnum.ThreeRound, "沙蛙", "捕蟹人", "指向者", "沙地车", "沙蛇", "沙鼠", "猎鹰", 37);
        AddTournament(false, "科尼库森林", 71, 1200, 0, tournamentTypeEnum.ThreeRound, "鳐鱼", "母鸡", "短吻鳄", "牛仔", "斯巴顿队", "蟒蛇", "发射者", 74);
        AddTournament(true, "德索塔森林", 72, 1250, 0, tournamentTypeEnum.ThreeRound, "摇滚者", "赛车手", "游侠", "比尔队", "忍者", "饼干队", "蜂群", 75);
        AddTournament(false, "黄石", 73, 1400, 0, tournamentTypeEnum.ThreeRound, "探险家", "徒步者", "喷泉", "忠实者", "露营者", "狼", "灰熊", 76);
        AddTournament(true, "冰川国家公园", 50, 3100, 0, tournamentTypeEnum.ThreeRound, "露营者", "旅行者", "游客", "冰川", "冰山", "拖车", "登山者", 77);
        AddTournament(false, "拱门国家公园", 75, 1100, 0, tournamentTypeEnum.ThreeRound, "黑熊", "狼", "野猪", "探险家", "游客", "观鸟者", "徒步者", 1);
        AddTournament(false, "金斯维尔", 76, 1400, 0, tournamentTypeEnum.ThreeRound, "军刀", "亡命之徒", "冲锋者", "眼镜蛇", "骑士", "国王队", "国王", 2);
        AddTournament(true, "拉雷多", 77, 1400, 0, tournamentTypeEnum.ThreeRound, "比尔队", "牛仔", "鳐鱼", "猫头鹰1999", "短吻鳄", "击球手", "山猫", 3);
        AddTournament(false, "查尔斯湖", 78, 1400, 0, tournamentTypeEnum.ThreeRound, "忍者", "游侠", "水蛇", "美洲虎", "投球手", "水网", "湖人", 4);
        AddTournament(true, "什里夫波特", 79, 1400, 0, tournamentTypeEnum.ThreeRound, "饼干队", "超级蛇", "斯巴顿队", "搬运工", "赛车手", "麋鹿", "猎鹰", 5);
        AddTournament(false, "松崖", 80, 1200, 0, tournamentTypeEnum.ThreeRound, "亡命之徒", "松鼠", "摇滚者", "母鸡", "蟒蛇", "崖顶", "发射者", 6);
        AddTournament(true, "魔鬼湖", 81, 1200, 0, tournamentTypeEnum.ThreeRound, "黑湖人", "蜂群", "恶魔", "湖狗", "亵渎者", "黑鸟", "湖头骨", 7);
        AddTournament(false, "萨卡卡威亚湖", 82, 1400, 0, tournamentTypeEnum.ThreeRound, "投球手", "短吻鳄队", "英雄", "狼", "忍者", "萨卡队", "比尔队", 8);
        AddTournament(true, "派克堡", 83, 1500, 0, tournamentTypeEnum.ThreeRound, "山猫", "狼", "国王", "比尔队", "鳐鱼", "冰熊", "游侠", 9);
        AddTournament(false, "卡森谷", 84, 1250, 0, tournamentTypeEnum.ThreeRound, "眼镜蛇", "骑士", "水蛇", "猎鹰", "比尔队", "母鸡", "骑士", 10);
        AddTournament(false, "迈尔斯城", 84, 1400, 0, tournamentTypeEnum.ThreeRound, "赛车手", "山猫", "忍者", "快车", "冲锋者", "智捷队", "军刀队", 10);
        AddTournament(false, "雷霆盆地", 85, 1400, 0, tournamentTypeEnum.ThreeRound, "眼镜蛇", "破坏者", "亡命徒", "飞龙", "母鸡", "斗牛犬", "猎鹰", 11);
        AddTournament(true, "拉皮德城", 86, 1200, 0, tournamentTypeEnum.ThreeRound, "牛仔", "探险者", "乌鸦", "山猫", "球员", "蟒蛇", "急流", 12);
        AddTournament(false, "大角山", 87, 1400, 0, tournamentTypeEnum.ThreeRound, "爱国者", "绵羊", "摇滚乐队", "角队", "亡命徒", "编织者", "赛车手", 13);
        AddTournament(true, "大叉", 88, 1400, 0, tournamentTypeEnum.ThreeRound, "突袭者", "莫霍克人", "粉碎者", "叉叉队", "蓝爪队", "穿越者", "英雄", 15);
        AddTournament(false, "林肯", 89, 1100, 0, tournamentTypeEnum.ThreeRound, "海鹰", "游侠", "吉娃娃", "飞龙", "乌鸦队", "美元队", "短吻鳄", 16);
        AddTournament(true, "情人城", 90, 1100, 0, tournamentTypeEnum.ThreeRound, "老鹰", "发射者", "母鸡", "奶牛", "忍者", "智者", "情人队", 17);
        AddTournament(false, "联盟城", 91, 900, 0, tournamentTypeEnum.ThreeRound, "冲锋者", "巨人", "捕食者", "星星队", "赛车手", "比尔队", "亡命徒", 18);
        AddTournament(true, "麦库克", 92, 1200, 0, tournamentTypeEnum.ThreeRound, "摇滚乐队", "野马", "蓝爪队", "斯巴达人", "厨师", "球员", "厨具队", 19);
        AddTournament(true, "威奇托瀑布", 93, 1400, 0, tournamentTypeEnum.ThreeRound, "瀑布者", "维京人", "游侠", "飓风队", "粉碎者", "乌鸦", "飞龙", 20);
        AddTournament(false, "落基山脉", 94, 1000, 0, tournamentTypeEnum.ThreeRound, "粉碎者", "钢铁队", "亡命徒", "游侠", "山地队", "猫头鹰", "猎鹰", 21);
        AddTournament(true, "亚特兰蒂斯", 55, 900, 0, tournamentTypeEnum.ThreeRound, "美人鱼", "海星队", "海狼", "宝藏队", "海洋之王", "古老者", "海后队", 22);
        AddTournament(false, "锡达礁", 96, 1400, 0, tournamentTypeEnum.ThreeRound, "骑士", "包装工", "美洲豹", "母鸡", "探险者", "飞龙", "锡达狼", 23);
        AddTournament(true, "德尔里奥", 97, 1100, 0, tournamentTypeEnum.ThreeRound, "山猫", "捕虾者", "红雀", "比尔队", "飞龙", "幼狮", "德尔里奥队", 24);
        AddTournament(false, "瓦拉瓦拉", 98, 1100, 0, tournamentTypeEnum.ThreeRound, "瓦拉队", "熊队", "蓝爪队", "赛车手", "发射者", "超级蛇", "乌鸦", 25);
        AddTournament(true, "西温多弗", 99, 1400, 0, tournamentTypeEnum.ThreeRound, "夜鹰", "斯巴达人", "公羊", "哨兵", "幼狮", "西部队", "粉碎者", 26);
        AddTournament(false, "威奇托", 100, 1400, 0, tournamentTypeEnum.ThreeRound, "比尔队", "亡命徒", "游侠", "圣徒", "无所不能队", "蟒蛇", "飞龙", 40);
        AddTournament(true, "花园城", 101, 900, 0, tournamentTypeEnum.ThreeRound, "落叶者", "大树队", "军刀队", "守护者", "充电者", "种植者", "园丁", 28);
        AddTournament(true, "德克斯霍马", 90, 1200, 0, tournamentTypeEnum.ThreeRound, "钻井者", "传教士", "喷气鹰", "山猫", "幼狮", "狮队", "怪咖队", 29);
        AddTournament(false, "奥古斯塔", 103, 1200, 0, tournamentTypeEnum.ThreeRound, "充电者", "粉碎者", "削片者", "驾驶者", "冲锋者", "推杆队", "高尔夫队", 30);
        AddTournament(true, "圣路易斯", 104, 900, 0, tournamentTypeEnum.ThreeRound, "抢断鹰", "拱门队", "猎鹰", "探险者", "美洲豹", "飞龙", "圣徒", 31);
        AddTournament(false, "杜兰戈", 115, 1100, 0, tournamentTypeEnum.ThreeRound, "比尔队", "母鸡", "杜兰戈队", "幼狮", "斯巴达人", "斑马队", "英雄", 32);
        AddTournament(false, "大春城", 116, 1200, 0, tournamentTypeEnum.ThreeRound, "春队", "喷气鹰", "发射者", "游侠", "飞龙", "射手队", "处理者", 33);
        AddTournament(true, "埃利", 117, 1400, 0, tournamentTypeEnum.ThreeRound, "吸烟者", "光线队", "充电者", "河蛇队", "赛车手", "蟒蛇", "粉碎者", 34);
        AddTournament(false, "贝克城", 118, 800, 0, tournamentTypeEnum.ThreeRound, "比尔队", "游侠", "蓝爪队", "探险者", "斑马队", "美洲豹", "贝克队", 35);
        AddTournament(true, "哈特山", 100, 1200, 0, tournamentTypeEnum.ThreeRound, "蓝爪队", "斯巴达人", "山地狗", "幼狮", "美洲豹", "探险者", "充电者", 36);
        AddTournament(false, "美国瀑布", 120, 800, 0, tournamentTypeEnum.ThreeRound, "粉碎者", "亡命徒", "美国人", "边缘袭击者", "喷气鹰", "河蛇队", "瀑布者", 37);
        AddTournament(true, "摇滚泉", 121, 900, 0, tournamentTypeEnum.ThreeRound, "摇滚狗", "摇滚鸟", "母鸡", "摇滚猫", "摇滚游侠", "黑岩队", "白岩队", 38);
        AddTournament(false, "斯普林格", 122, 1400, 0, tournamentTypeEnum.ThreeRound, "突袭者", "河蛇队", "幼狮", "斯巴达人", "游侠", "充电者", "发射者", 39);
        bool flag = Players.GetActiveStarterNum(true) > -1;
        AddTournament(PlayerPrefs.GetInt("LB_VERSION") % 2 == 0 && flag, ArenaChooser.GetLiveEventName(), 1, 0, 0, tournamentTypeEnum.LiveEvent,
            "投篮高手", "火力全开!", "超级明星", "电梯队", "扣篮王", "精英队", "恐龙猛兽",
            ArenaChooser.GetLiveEventArena());
    }

    private void AddTournament(bool isFemale, string name, int prereq, int cash, int gold, tournamentTypeEnum type, string o1, string o2, string o3,
        string o4, string o5, string o6, string o7, int arena)
    {
        Tournament tournament = new Tournament();
        tournament.isFemale = isFemale;
        tournament.name = name;
        tournament.prereq = prereq;
        tournament.type = type;
        tournament.num = tournaments.Count;
        tournament.arena = arena;
        int xpPrize = (tournament.xpPrizeForFirstWin = ((tournament.num <= 1) ? 250 : 100));
        int numCompletions = GetNumCompletions(tournament.num);
        if (numCompletions > 0)
        {
            xpPrize = 85;
            gold = 0;
        }
        numChampionships += numCompletions;
        if (numCompletions >= 3 && type == tournamentTypeEnum.ThreeRound)
        {
            numTrophies++;
        }
        tournament.xpPrize = xpPrize;
        tournament.cashPrize = cash;
        tournament.goldPrize = gold;
        tournament.locked = false;
        tournament.completed = numCompletions > 0;
        tournament.currentRound = tournament.GetCurrentRound();
        tournament.opponentNames.Add(o1);
        tournament.opponentNames.Add(o2);
        tournament.opponentNames.Add(o3);
        tournament.opponentNames.Add(o4);
        tournament.opponentNames.Add(o5);
        tournament.opponentNames.Add(o6);
        tournament.opponentNames.Add(o7);
        tournaments.Add(tournament);
    }
}