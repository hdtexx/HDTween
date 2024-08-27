using UnityEngine;

namespace HDTween
{
    public static class RotateOffsetToTweenExtensions
    {
        public static RotateOffsetToTween RotateOffsetTo(this Transform transform,
            Quaternion offsetRotation, float duration, bool unscaled = false, bool local = false)
        {
            return new RotateOffsetToTween(transform, offsetRotation, duration, unscaled, local);
        }

        public static ITween StartTween(this RotateOffsetToTween tween)
        {
            return TweenManager.Instance.StartTween(tween);
        }
    }
}