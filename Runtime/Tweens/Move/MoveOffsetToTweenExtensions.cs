using UnityEngine;

public static class MoveOffsetToTweenExtensions
{
    public static MoveOffsetToTween MoveOffsetTo(this Transform transform, 
        Vector3 offset, float duration, bool unscaled = false, bool local = false)
    {
        return new MoveOffsetToTween(transform, offset, duration, unscaled, local);
    }

    public static ITween StartTween(this MoveOffsetToTween tween)
    {
        return TweenManager.Instance.StartTween(tween);
    }
}