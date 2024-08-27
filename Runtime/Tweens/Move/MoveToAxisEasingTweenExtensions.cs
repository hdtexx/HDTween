using HDTween;
using UnityEngine;

public static class MoveToAxisEasingTweenExtensions
{
    public static MoveToAxisEasingTween MoveToAxisEasing(this Transform transform, 
        Vector3 endPosition, float duration, bool unscaled = false, bool local = false)
    {
        return new MoveToAxisEasingTween(transform, endPosition, duration, unscaled, local);
    }

    public static ITween StartTween(this MoveToAxisEasingTween tween)
    {
        return TweenManager.Instance.StartTween(tween);
    }

    public static ITween SetEaseX(this ITween tween, AnimationCurve easeX)
    {
        if (tween is MoveToTween moveToTween)
        {
            moveToTween.SetEase(easeX);
        }
        
        return tween;
    }

    public static ITween SetEaseY(this ITween tween, AnimationCurve easeY)
    {
        if (tween is MoveToTween moveToTween)
        {
            moveToTween.SetEase(easeY);
        }
        
        return tween;
    }
}