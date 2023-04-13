using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Extensions
{
    public static class UIExtensions
    {
        public static void Fade(this CanvasGroup grp, bool value, bool instant = false, float delay = 0.3f)
        {
            if (instant)
                grp.alpha = value ? 1 : 0;
            else
                grp.DOFade(value ? 1 : 0, delay);
        }

        public static void Fade(this Image grp, bool value, bool instant = false, float delay = 0.3f)
        {
            if (instant)
            {
                var col = grp.color;
                grp.color = new Color(col.r, col.g, col.b, value ? 1 : 0);
            }
            else
            {
                grp.DOFade(value ? 1 : 0, delay);
            }
        }

        public static void ScaleX(this RectTransform rectTransform, bool value, bool instant = false,
            float delay = 0.3f)
        {
            if (instant)
            {
                var scale = rectTransform.localScale;
                scale.x = value ? 1 : 0;
                rectTransform.localScale = scale;
            }
            else
            {
                rectTransform.DOScaleX(value ? 1 : 0, delay);
            }
        }
    }
}