using HDTween;
using UnityEngine;
using UnityEngine.UI;

public static class ChangeColorToExtensions
{
    public static ChangeColorToTween ChangeColorTo(this Graphic graphic, Color targetColor, float duration, bool unscaled = false)
    {
        return new ChangeColorToTween(graphic, targetColor, duration, unscaled);
    }

    public static ChangeColorToTween ChangeColorTo(this SpriteRenderer spriteRenderer, Color targetColor, float duration, bool unscaled = false)
    {
        return new ChangeColorToTween(spriteRenderer, targetColor, duration, unscaled);
    }

    public static ChangeColorToTween ChangeColorTo(this Light light, Color targetColor, float duration, bool unscaled = false)
    {
        return new ChangeColorToTween(light, targetColor, duration, unscaled);
    }

    public static ChangeColorToTween ChangeColorTo(this Material material, Color targetColor, float duration, bool unscaled = false)
    {
        return new ChangeColorToTween(material, targetColor, duration, unscaled);
    }

    public static ITween StartTween(this ChangeColorToTween tween)
    {
        return TweenManager.Instance.StartTween(tween);
    }
}