using UnityEngine;

namespace HDTween
{
    public static class ChangeShaderColorToExtensions
    {
        public static ChangeShaderColorToTween ChangeShaderColorTo(this Material material, string shaderKey, UnityEngine.Color targetColor, float duration, bool unscaled = false)
        {
            return new ChangeShaderColorToTween(material, shaderKey, targetColor, duration, unscaled);
        }

        public static ITween StartTween(this ChangeShaderColorToTween tween)
        {
            return TweenManager.Instance.StartTween(tween);
        }
    }
}