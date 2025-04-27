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
        private VolumeProfile clonedProfile;
        private VCRVolume clonedVCRVolume;
        //from this point to 0 is the effect strength modified
        [SerializeField] private float maxDistance = 5f;
        private float stripeMaxValue = 7f;
        private float noisyMaxValue = 0.5f;
        private float verticalShiftMaxValue = 1f;
        private float horizontalShiftMaxValue = 1f;
        
        private void OnValidate()
        {
            if (player == null)
                player = FindObjectOfType<PlayerManager>();

            if (sentryVolume == null)
                sentryVolume = GetComponent<Volume>();
        }

        private void Start()
        {
            //create a clone of the sentry volume
            //clone the profile and any components you want to change
            //I only need to change the vignette so I only clone that
            clonedProfile = Instantiate(sentryVolume.profile);
            clonedProfile.TryGet(out clonedVCRVolume);
            clonedVCRVolume = Instantiate(clonedVCRVolume);

            //assign the component to the cloned profile
            //seems like there should be a better way, but this works
            //you might need to experiment with the index value for other effects
            clonedProfile.components[0] = clonedVCRVolume;
            //assign the profile to the volume
            sentryVolume.profile = clonedProfile;
        }

        private void FixedUpdate()
        {
            if (!sentryVolume.profile.TryGet(out VCRVolume vcrVolume)) 
                return;
            
            float distance = Vector3.Distance(player.transform.position, transform.position);
            float effectStrength = Mathf.Clamp01(1 - (distance / maxDistance));
            
            sentryVolume.priority = effectStrength;
            vcrVolume.Stripes.value = effectStrength * stripeMaxValue;
            vcrVolume.Noisy.value = effectStrength * noisyMaxValue;
            vcrVolume.VerticalShift.value = effectStrength * verticalShiftMaxValue;
            vcrVolume.HorizontalShift.value = effectStrength * horizontalShiftMaxValue;
        }
    }
}
