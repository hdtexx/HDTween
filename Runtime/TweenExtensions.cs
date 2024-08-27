using UnityEngine;

namespace HDTween
{
    public static class TweenExtensions
    {
        public static CompositeTween Composite(bool stopOnAnyTweenCancel = true)
        {
            return new CompositeTween(stopOnAnyTweenCancel);
        }
    
        public static SequenceTween Sequence(bool stopOnAnyTweenCancel = true)
        {
            return new SequenceTween(stopOnAnyTweenCancel);
        }

        public static ITween SetEase(this ITween tween, AnimationCurve curve)
        {
            return tween.SetEase(curve);
        }

        public static ITween SetDelay(this ITween tween, float delay)
        {
            return tween.SetDelay(delay);
        }

        public static ITween StartTween(this ITween tween)
        {
            TweenManager.Instance.StartTween(tween);
        
            return tween;
        }

        public static ITween StartTween(this ITween tween, int repeatCount)
        {
            TweenManager.Instance.StartTween(tween, repeatCount);
        
            return tween;
        }
    }
}