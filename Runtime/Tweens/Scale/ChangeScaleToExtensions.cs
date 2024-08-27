using UnityEngine;

public static class ChangeScaleToExtensions
{
    public static ChangeScaleToTween ChangeScaleTo(this Transform transform, Vector3 scaleTo, float duration, bool unscaled = false)
    {
        return new ChangeScaleToTween(transform, scaleTo, duration, unscaled);
    }

    public static ITween StartTween(this ChangeScaleToTween tween)
    {
        return TweenManager.Instance.StartTween(tween);
    }
}