using UnityEngine;

public static class ScaleToExtensions
{
    public static ScaleToTween ScaleTo(this Transform transform, Vector3 scaleMultiplier, float duration, bool unscaled = false)
    {
        return new ScaleToTween(transform, scaleMultiplier, duration, unscaled);
    }

    public static ITween StartTween(this ScaleToTween tween)
    {
        return TweenManager.Instance.StartTween(tween);
    }
}