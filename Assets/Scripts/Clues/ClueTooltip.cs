using System.Collections;
using General;
using TMPro;
using UnityEngine;

namespace Clues
{
    public class ClueTooltip : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        private TextMeshProUGUI _tmp;

        private void Awake()
        {
            Hub.Register(this);
            rectTransform = rectTransform ? rectTransform : GetComponent<RectTransform>();
            SetActive(false);
            _tmp = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        }

        private void Update()
        {
            // if (!rectTransform)
            //     return;
            //
            // rectTransform.position = Mouse.current.position.ReadValue() + new Vector2(0, offsetY);
        }

        public void SetText(string text, Vector3 position)
        {
            _tmp.text = text;
            _tmp.color = new Color(_tmp.color.r, _tmp.color.g, _tmp.color.b, 0);

            rectTransform.position = position;
            SetActive(true);
        }

        public void SetActive(bool active)
        {
            rectTransform.gameObject.SetActive(active);
            StartCoroutine(FadeInWithDelay());
        }

        private IEnumerator FadeInWithDelay()
        {
            yield return null;
            _tmp.color = new Color(_tmp.color.r, _tmp.color.g, _tmp.color.b, 1);
        }
    }
}