
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
        "武器动态生成，位置可以用现在调好的子节点位置",
        "优化刀光接口isEmitting,可以直接MyTrailRenderer.isEmitting"
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
        "怪物火球对象",
        "修复unity酱的渲染不兼容",
        "修复unity酱的曝光渲染",
    };
}