using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationConfig_UnityChan
{
    public static class Parameters
    {
        public static readonly int XSpeed = Animator.StringToHash("XSpeed");
        public static readonly int YSpeed = Animator.StringToHash("YSpeed");
    }

    public static class StateHashes
    {
        public static readonly int HorLocomotion = Animator.StringToHash("HorLocomotion");
        public static readonly int VerLocomotion = Animator.StringToHash("VerLocomotion");
        public static readonly int Idle = Animator.StringToHash("Idle");
        public static readonly int WalkBack = Animator.StringToHash("WalkBack");
        public static readonly int Rest = Animator.StringToHash("Rest");
        public static readonly int Attack01 = Animator.StringToHash("Attack01");
        public static readonly int Attack02 = Animator.StringToHash("Attack02");
        public static readonly int Attack03 = Animator.StringToHash("Attack03");
        public static readonly int AttackDodge = Animator.StringToHash("AttackDodge");
        public static readonly int Dodge = Animator.StringToHash("Dodge");
    }

    public static class StatePriority
    {
        public const int Idle = 0;
        public const int Run = 1;
        public const int Move = 2;
        public const int Dodge = 3;
        public const int Attack = 4;
        public const int Jump = 5;
    }

    public static readonly Dictionary<System.Type, int[]> StateToParameters = new Dictionary<System.Type, int[]>
    {
        { typeof(PlayerIdleState), new int[] { Parameters.XSpeed } },
        { typeof(PlayerMoveState), new int[] { Parameters.XSpeed } },
        { typeof(PlayerAttackState), new int[] { Parameters.XSpeed } },
        { typeof(PlayerJumpUpState),new int[] {Parameters.YSpeed} },
        { typeof(PlayerJumpDownState),new int[] {Parameters.YSpeed} },
    };

    public static class TransitionSettings
    {
        public const float NormalTransitionDuration = 0.1f;
        public const float AttackTransitionDuration = 0.05f;
        public const float AttackOverTransitionDuration = 0.5f;
        public const float SuperTransitionDuration = 1f;
    }

}
