using HDTween;
using UnityEngine;

public static class ChangeShaderColorToExtensions
{
    public static ChangeShaderColorToTween ChangeShaderColorTo(this Material material, string shaderKey, Color targetColor, float duration, bool unscaled = false)
    {
        return new ChangeShaderColorToTween(material, shaderKey, targetColor, duration, unscaled);
    }

    public static ITween StartTween(this ChangeShaderColorToTween tween)
    {
        return TweenManager.Instance.StartTween(tween);
    }
}