
using System.Collections.Generic;
using UnityChan;

public class 开发日志_Sekiro
{
    void 高亮显示()
    {
        BababoiToDoList[0] = null;
        SekiroTODoList[0] = null;
        SekiroTODoListFrist[0] = null;
    }

    List<string> BababoiToDoList = new List<string>()
    {
        //"预制体会放在Resources_Build文件夹下",
        //"武器的刀光效果：需要在动画事件中控制TrailRenderer的开启和关闭",
        //"刀光效果更新，使用MyTrailRenderer预制体渲染，在动画事件控制生成或销毁，需要保证世界坐标为000，需要调用InitMyTrailRenderer传入首尾位置",
        //"优化刀光接口isEmitting,可以直接MyTrailRenderer.isEmitting",
        //"找了个刀的命中特效，合适的话可以用上",
    };

    List<string> SekiroTODoList = new List<string>()
    {
        //"行为树基类",
        //"行为树拓展",
        //"行为树逻辑完结",
        "敌人控制器完善,索敌方法优化",
        "通用插值方法",
        "怪物技能特效",
    };

    List<string> SekiroTODoListFrist = new List<string>()
    {
        //"找个地图",
        //"怪物火球对象",
        //"修复unity酱的渲染不兼容",
        //"修复unity酱的曝光渲染",
        //"远程攻击释放火球",
        //"动画事件调用火球",
        //"火球运动逻辑",
        //"火球碰撞逻辑",
        //"火球伤害逻辑",
        "伤害检测添加白名单抽象",
        //"现在需要闪避的一个残影，按住Ctrl角色会向后闪避一段距离，这个是直接用的CC的Move方法进行强制移动的",
        //"残影效果微调",
        //"蓄力斩需要一个特殊的刀光，换个颜色都可以",
        //"后处理：径向模糊",
    };
}