using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    void Move(Vector2 input)
    {

    }
}

//🧠 逻辑步骤 1：计算世界移动方向 (坐标系转换)
//场景：
//你的输入 input.y (W键) 代表的是“屏幕上方”，也就是摄像机的正前方。
//你的输入 input.x (D键) 代表的是“屏幕右方”，也就是摄像机的正右方。
//数学陷阱 (面试考点)：
//ARPG 的摄像机通常是俯视的（比如往下看 45 度）。
//如果你直接用 Camera.main.transform.forward，这个向量是指向地面的。
//后果：角色移动时会试图钻进地底，导致移动速度变慢（因为垂直分量被碰撞体挡住了，水平分量变小）。
//你需要实现的代码逻辑：
//拿到摄像机的 forward (前) 和 right (右) 向量。
//投影到水平面：把这两个向量的 y 轴分量全部强制设为 0。
//再次归一化 (关键)：因为把 y 设为 0 后，向量变短了（勾股定理），必须重新 Normalize，否则视角越垂直，跑得越慢。
//向量合成：
//最终方向 = (处理后的相机前向量 × 输入的纵向值) + (处理后的相机右向量 × 输入的横向值)。