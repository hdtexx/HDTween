using UnityEngine;

public static class ConstantRotateTweenExtensions
{
    public static ConstantRotateTween ConstantRotate(this Transform transform,
        float duration, bool unscaled = false, bool local = false)
    {
        return new ConstantRotateTween(transform, duration, unscaled, local);
    }

    public static ITween StartTween(this ConstantRotateTween tween)
    {
        return TweenManager.Instance.StartTween(tween);
    }
}