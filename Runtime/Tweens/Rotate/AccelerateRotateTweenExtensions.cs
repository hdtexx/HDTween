using UnityEngine;

namespace HDTween
{
    public static class AccelerateRotateTweenExtensions
    {
        public static AccelerateRotateTween AccelerateRotate(this Transform target, float duration, float accelerationDuration, float decelerationDuration, Vector3 rotationSpeeds, bool unscaled = false, bool local = false)
        {
            return new AccelerateRotateTween(target, duration, accelerationDuration, decelerationDuration, rotationSpeeds, unscaled, local);
        }
    }
}