using UnityEngine;

public static class RotateToTweenExtensions
{
    public static RotateToTween RotateTo(this Transform transform,
        Quaternion endRotation, float duration, bool unscaled = false, bool local = false)
    {
        return new RotateToTween(transform, endRotation, duration, unscaled, local);
    }

    public static ITween StartTween(this RotateToTween tween)
    {
        return TweenManager.Instance.StartTween(tween);
    }
}