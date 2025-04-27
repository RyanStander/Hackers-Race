using System;
using Player;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Environment
{
    public class SentryEffectPower : MonoBehaviour
    {
        [SerializeField] private PlayerManager player;
        [SerializeField] private Volume sentryVolume;
        //from this point to 0 is the effect strength modified
        [SerializeField] private float maxDistance = 5f;
        private float stripeMaxValue = 7f;
        private float noisyMaxValue = 1f;
        private float verticalShiftMaxValue = 1f;
        private float horizontalShiftMaxValue = 1f;
        
        private void OnValidate()
        {
            if (player == null)
                player = FindObjectOfType<PlayerManager>();

            if (sentryVolume == null)
                sentryVolume = GetComponent<Volume>();
        }

        private void FixedUpdate()
        {
            if (!sentryVolume.profile.TryGet(out VCRVolume vcrVolume)) 
                return;
            
            float distance = Vector3.Distance(player.transform.position, transform.position);
            float effectStrength = Mathf.Clamp01(1 - (distance / maxDistance));
                
            vcrVolume.Stripes.value = effectStrength * stripeMaxValue;
            vcrVolume.Noisy.value = effectStrength * noisyMaxValue;
            vcrVolume.VerticalShift.value = effectStrength * verticalShiftMaxValue;
            vcrVolume.HorizontalShift.value = effectStrength * horizontalShiftMaxValue;
        }
    }
}
