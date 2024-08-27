using HDTween;
using UnityEngine;

public static class MoveOffsetToAxisEasingTweenExtensions
{
    public static MoveOffsetToAxisEasingTween MoveOffsetToAxisEasing(this Transform transform, 
        Vector3 offset, float duration, bool unscaled = false, bool local = false)
    {
        return new MoveOffsetToAxisEasingTween(transform, offset, duration, unscaled, local);
    }

    public static ITween StartTween(this MoveOffsetToAxisEasingTween tween)
    {
        return TweenManager.Instance.StartTween(tween);
    }

    public static ITween SetEaseX(this ITween tween, AnimationCurve easeX)
    {
        if (tween is MoveOffsetToAxisEasingTween moveOffsetToAxisEasingTween)
        {
            moveOffsetToAxisEasingTween.SetEaseX(easeX);
        }
        
        return tween;
    }

    public static ITween SetEaseY(this ITween tween, AnimationCurve easeY)
    {
        if (tween is MoveOffsetToAxisEasingTween moveOffsetToAxisEasingTween)
        {
            moveOffsetToAxisEasingTween.SetEaseY(easeY);
        }
        
        return tween;
    }

    public static ITween SetEaseZ(this ITween tween, AnimationCurve easeZ)
    {
        if (tween is MoveOffsetToAxisEasingTween moveOffsetToAxisEasingTween)
        {
            moveOffsetToAxisEasingTween.SetEaseZ(easeZ);
        }
        
        return tween;
    }
}