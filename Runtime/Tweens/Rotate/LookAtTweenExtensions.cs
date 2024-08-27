using HDTween;
using UnityEngine;

public static class LookAtTweenExtensions
{
    public static LookAtTween LookAt(this Transform transform,
        Transform lookAtTarget, float duration, bool unscaled = false, bool local = false)
    {
        return new LookAtTween(transform, lookAtTarget, duration, unscaled, local);
    }

    public static LookAtTween LookAt(this Transform transform,
        Vector3 lookAtPoint, float duration, bool unscaled = false, bool local = false)
    {
        return new LookAtTween(transform, lookAtPoint, duration, unscaled, local);
    }

    public static ITween StartTween(this LookAtTween tween)
    {
        return TweenManager.Instance.StartTween(tween);
    }
}