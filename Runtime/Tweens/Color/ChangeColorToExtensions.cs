using UnityEngine;
using UnityEngine.UI;

namespace HDTween
{
    public static class ChangeColorToExtensions
    {
        public static ChangeColorToTween ChangeColorTo(this Graphic graphic, UnityEngine.Color targetColor, float duration, bool unscaled = false)
        {
            return new ChangeColorToTween(graphic, targetColor, duration, unscaled);
        }

        public static ChangeColorToTween ChangeColorTo(this SpriteRenderer spriteRenderer, UnityEngine.Color targetColor, float duration, bool unscaled = false)
        {
            return new ChangeColorToTween(spriteRenderer, targetColor, duration, unscaled);
        }

        public static ChangeColorToTween ChangeColorTo(this Light light, UnityEngine.Color targetColor, float duration, bool unscaled = false)
        {
            return new ChangeColorToTween(light, targetColor, duration, unscaled);
        }

        public static ChangeColorToTween ChangeColorTo(this Material material, UnityEngine.Color targetColor, float duration, bool unscaled = false)
        {
            return new ChangeColorToTween(material, targetColor, duration, unscaled);
        }

        public static ITween StartTween(this ChangeColorToTween tween)
        {
            return TweenManager.Instance.StartTween(tween);
        }
    }
}