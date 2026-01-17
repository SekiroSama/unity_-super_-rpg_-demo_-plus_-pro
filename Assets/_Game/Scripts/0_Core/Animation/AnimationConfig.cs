using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationConfig
{
    public static class Parameters
    {
        public static readonly int Speed = Animator.StringToHash("Speed");
    }

    public static class StateHashes
    {
        public static readonly int Locomotion = Animator.StringToHash("Locomotion");
        public static readonly int Idle = Animator.StringToHash("Idle");
        public static readonly int WalkBack = Animator.StringToHash("WalkBack");
        public static readonly int Rest = Animator.StringToHash("Rest");
        public static readonly int Attack01 = Animator.StringToHash("Attack01");
        public static readonly int Attack02 = Animator.StringToHash("Attack02");
        public static readonly int Attack03 = Animator.StringToHash("Attack03");
    }

    public static class StatePriority
    {
        public const int Idle = 0;
        public const int Move = 1;
        public const int Attack = 2;
    }

    public static readonly Dictionary<System.Type, int[]> StateToParameters = new Dictionary<System.Type, int[]>
    {
        { typeof(PlayerIdleState), new int[] { Parameters.Speed } },
        { typeof(PlayerMoveState), new int[] { Parameters.Speed } },
        { typeof(PlayerAttackState), new int[] { Parameters.Speed } },
    };

    public static class TransitionSettings
    {
        public const float NormalTransitionDuration = 0.1f;
        public const float AttackTransitionDuration = 0.05f;
        public const float AttackOverTransitionDuration = 0.5f;
        public const float SuperTransitionDuration = 1f;
    }

}
