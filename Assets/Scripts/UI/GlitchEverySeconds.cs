using UnityEngine;

namespace UI
{
    public class GlitchEverySeconds : MonoBehaviour
    {
        [SerializeField] GameObject glitchEffect;
        
        [SerializeField] float glitchInterval = 10f;
        [SerializeField] float glitchDuration = 2f;
        
        private void Start()
        {
            InvokeRepeating(nameof(TriggerGlitch), glitchInterval, glitchInterval);
        }
        
        private void TriggerGlitch()
        {
            StartCoroutine(GlitchCoroutine());
        }
        
        private System.Collections.IEnumerator GlitchCoroutine()
        {
            glitchEffect.SetActive(true);
            yield return new WaitForSeconds(glitchDuration);
            glitchEffect.SetActive(false);
        }
    }
}
