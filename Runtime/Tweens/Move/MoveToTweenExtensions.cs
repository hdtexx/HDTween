using UnityEngine;

namespace HDTween
{
    public static class MoveToTweenExtensions
    {
        public static MoveToTween MoveTo(this Transform transform,
            Vector3 endPosition, float duration, bool unscaled = false, bool local = false)
        {
            return new MoveToTween(transform, endPosition, duration, unscaled, local);
        }

        public static ITween StartTween(this MoveToTween tween)
        {
            return TweenManager.Instance.StartTween(tween);
        }
    }
}