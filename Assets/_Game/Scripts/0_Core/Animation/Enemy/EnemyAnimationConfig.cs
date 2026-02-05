using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationConfig
{
    public static class Parameters
    {
        public static readonly int Speed = Animator.StringToHash("Speed"); 
        public static readonly int Die = Animator.StringToHash("Die");
        public static readonly int IsDowned = Animator.StringToHash("IsDowned"); 
        public static readonly int isSleeping = Animator.StringToHash("isSleeping"); 
        public static readonly int DragonShout = Animator.StringToHash("DragonShout");
        public static readonly int ProjectileAttack = Animator.StringToHash("ProjectileAttack");
    }

    public static class FatFatDragonSettings
    {
        public const float FatFatDragonWalkSpeedRatio = 0.33f;
        public const float FatFatDragonRunSpeedRatio = 1f;
    }

    public static class StateHashes
    {
        //public static readonly int Locomotion = Animator.StringToHash("Locomotion");
        //public static readonly int Idle = Animator.StringToHash("Idle");
        //public static readonly int WalkBack = Animator.StringToHash("WalkBack");
        //public static readonly int Rest = Animator.StringToHash("Rest");
        //public static readonly int Attack01 = Animator.StringToHash("Attack01");
        //public static readonly int Attack02 = Animator.StringToHash("Attack02");
        //public static readonly int Attack03 = Animator.StringToHash("Attack03");
    }

    public static class StatePriority
    {
        //public const int Idle = 0;
        //public const int Move = 1;
        //public const int Attack = 2;
    }

    public static readonly Dictionary<System.Type, int[]> StateToParameters = new Dictionary<System.Type, int[]>
    {
        //{ typeof(PlayerIdleState), new int[] { Parameters.Speed } },
        //{ typeof(PlayerMoveState), new int[] { Parameters.Speed } },
        //{ typeof(PlayerAttackState), new int[] { Parameters.Speed } },
    };


}
