using System.Collections;
using UnityEngine;

namespace UI
{
    public class CRTPushEffect : MonoBehaviour
    {
        [SerializeField] private RectTransform topImage;
        [SerializeField] private RectTransform bottomImage;
        [SerializeField] private float duration = 1f;

        public void TriggerCollapseEffect()
        {
            StartCoroutine(CollapseEffect());
        }

        private IEnumerator CollapseEffect()
        {
            Vector2 topStart = topImage.anchoredPosition;
            Vector2 bottomStart = bottomImage.anchoredPosition;

            float screenHeight = ((RectTransform)topImage.parent).rect.height;
            float centerY = 0f; // anchoredPosition.y of center

            float elapsed = 0f;
            while (elapsed < duration)
            {
                float t = elapsed / duration;
                float y = Mathf.Lerp(topStart.y, centerY, t);

                topImage.anchoredPosition = new Vector2(topImage.anchoredPosition.x, y);
                bottomImage.anchoredPosition = new Vector2(bottomImage.anchoredPosition.x, -y);

                elapsed += Time.deltaTime;
                yield return null;
            }

            // Snap to final position
            topImage.anchoredPosition = new Vector2(topImage.anchoredPosition.x, centerY);
            bottomImage.anchoredPosition = new Vector2(bottomImage.anchoredPosition.x, centerY);
        }
    }
}
