using UnityEngine;

namespace HDTween
{
    public static class EaseType
    {
        public static readonly AnimationCurve Linear = AnimationCurve.Linear(0, 0, 1, 1);

        public static readonly AnimationCurve EaseInSine = new AnimationCurve(
            new Keyframe(0, 0),
            new Keyframe(1, 1, 1.57f, 1.57f)
        );

        public static readonly AnimationCurve EaseOutSine = new AnimationCurve(
            new Keyframe(0, 0, 1.57f, 1.57f),
            new Keyframe(1, 1)
        );

        public static readonly AnimationCurve EaseInOutSine = new AnimationCurve(
            new Keyframe(0, 0),
            new Keyframe(0.5f, 0.5f),
            new Keyframe(1, 1)
        );

        public static readonly AnimationCurve EaseInQuad = new AnimationCurve(
            new Keyframe(0, 0),
            new Keyframe(1, 1, 2, 2)
        );

        public static readonly AnimationCurve EaseOutQuad = new AnimationCurve(
            new Keyframe(0, 0, 0, 0),
            new Keyframe(1, 1, -2, -2)
        );

        public static readonly AnimationCurve EaseInOutQuad = new AnimationCurve(
            new Keyframe(0, 0),
            new Keyframe(0.5f, 0.5f, 2, 2),
            new Keyframe(1, 1)
        );

        public static readonly AnimationCurve EaseInCubic = new AnimationCurve(
            new Keyframe(0, 0),
            new Keyframe(1, 1, 3, 3)
        );

        public static readonly AnimationCurve EaseOutCubic = new AnimationCurve(
            new Keyframe(0, 0, 0, 0),
            new Keyframe(1, 1, -3, -3)
        );

        public static readonly AnimationCurve EaseInOutCubic = new AnimationCurve(
            new Keyframe(0, 0),
            new Keyframe(0.5f, 0.5f, 3, 3),
            new Keyframe(1, 1)
        );

        public static readonly AnimationCurve EaseInElastic = new AnimationCurve(
            new Keyframe(0, 0),
            new Keyframe(0.7f, -0.1f),
            new Keyframe(0.9f, -0.05f),
            new Keyframe(1, 1)
        );

        public static readonly AnimationCurve EaseOutBounce = new AnimationCurve(
            new Keyframe(0, 0),
            new Keyframe(0.2f, 1.1f),
            new Keyframe(0.4f, 0.9f),
            new Keyframe(0.6f, 1.03f),
            new Keyframe(0.8f, 0.97f),
            new Keyframe(1, 1)
        );

        public static readonly AnimationCurve EaseInSlowOutFast = new AnimationCurve(
            new Keyframe(0, 0, 0, 0),
            new Keyframe(0.7f, 0.1f, 0.5f, 0.5f),
            new Keyframe(1, 1, 2, 2)
        );

        public static readonly AnimationCurve EaseInFastOutSlow = new AnimationCurve(
            new Keyframe(0, 0, 2, 2),
            new Keyframe(0.3f, 0.9f, 0.5f, 0.5f),
            new Keyframe(1, 1, 0, 0)
        );

        public static readonly AnimationCurve EaseInVerySlowOutNormal = new AnimationCurve(
            new Keyframe(0, 0, 0, 0),
            new Keyframe(0.8f, 0.2f, 0.5f, 0.5f),
            new Keyframe(1, 1, 1, 1)
        );

        public static readonly AnimationCurve EaseInOutEmphasisMid = new AnimationCurve(
            new Keyframe(0, 0, 0, 0),
            new Keyframe(0.4f, 0.1f, 1, 1),
            new Keyframe(0.6f, 0.9f, 1, 1),
            new Keyframe(1, 1, 0, 0)
        );

        public static readonly AnimationCurve EaseStepwise = new AnimationCurve(
            new Keyframe(0, 0),
            new Keyframe(0.2f, 0.2f),
            new Keyframe(0.4f, 0.4f),
            new Keyframe(0.6f, 0.6f),
            new Keyframe(0.8f, 0.8f),
            new Keyframe(1, 1)
        );
    
        public static readonly AnimationCurve EaseSpring = new AnimationCurve(
            new Keyframe(0, 0),
            new Keyframe(0.3f, 1.1f),
            new Keyframe(0.5f, 0.9f),
            new Keyframe(0.7f, 1.05f),
            new Keyframe(0.9f, 0.98f),
            new Keyframe(1, 1)
        );

        public static readonly AnimationCurve EaseElasticPunch = new AnimationCurve(
            new Keyframe(0, 0),
            new Keyframe(0.4f, 1.4f),
            new Keyframe(0.6f, 0.8f),
            new Keyframe(0.8f, 1.1f),
            new Keyframe(1, 1)
        );
    
        public static readonly AnimationCurve ClassicJump = new AnimationCurve(
            new Keyframe(0, 0),
            new Keyframe(0.5f, 1, 0, 0),
            new Keyframe(1, 0)
        );

        public static readonly AnimationCurve FloatyJump = new AnimationCurve(
            new Keyframe(0, 0),
            new Keyframe(0.4f, 0.9f, 2, 2),
            new Keyframe(0.6f, 0.9f, -2, -2),
            new Keyframe(1, 0)
        );

        public static readonly AnimationCurve QuickRiseSlowFallJump = new AnimationCurve(
            new Keyframe(0, 0, 4, 4),
            new Keyframe(0.3f, 1),
            new Keyframe(1, 0, -0.5f, -0.5f)
        );

        public static readonly AnimationCurve BouncyJump = new AnimationCurve(
            new Keyframe(0, 0),
            new Keyframe(0.4f, 1),
            new Keyframe(0.7f, 0.05f),
            new Keyframe(0.8f, 0.2f),
            new Keyframe(0.9f, 0.05f),
            new Keyframe(1, 0)
        );

        public static readonly AnimationCurve DoubleJump = new AnimationCurve(
            new Keyframe(0, 0),
            new Keyframe(0.3f, 0.7f),
            new Keyframe(0.5f, 0.3f),
            new Keyframe(0.7f, 1),
            new Keyframe(1, 0)
        );

        public static readonly AnimationCurve SlowApexJump = new AnimationCurve(
            new Keyframe(0, 0, 3, 3),
            new Keyframe(0.4f, 0.9f, 0.2f, 0.2f),
            new Keyframe(0.6f, 0.9f, -0.2f, -0.2f),
            new Keyframe(1, 0, -3, -3)
        );

        public static readonly AnimationCurve JerkJump = new AnimationCurve(
            new Keyframe(0, 0),
            new Keyframe(0.2f, 0.8f, 5, 5),
            new Keyframe(0.4f, 1),
            new Keyframe(1, 0, -1, -1)
        );
    }
}