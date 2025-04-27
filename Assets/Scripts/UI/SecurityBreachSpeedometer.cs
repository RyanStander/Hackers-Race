using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SecurityBreachSpeedometer : MonoBehaviour
    {
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private Slider speedometerSlider;
        [SerializeField] private TextMeshProUGUI speedText;
        [SerializeField] private float maxAchieveableSpeed = 50f;
        private float breachSpeed;
        [SerializeField] private Color defaultSliderColor = Color.green;
        [SerializeField] private Color breachSliderColor = Color.red;

        private void OnValidate()
        {
            if (playerManager == null)
                playerManager = FindObjectOfType<PlayerManager>();

            if (speedometerSlider == null)
                speedometerSlider = GetComponentInChildren<Slider>();

            if (speedText == null)
                speedText = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            speedometerSlider.maxValue = maxAchieveableSpeed;
            breachSpeed = playerManager.PlayerMovement.GetSecurityBreachSpeed();
            speedometerSlider.value = 0;
        }

        private void FixedUpdate()
        {
            //get current speed with y axis ignored
            float currentSpeed = Mathf.Sqrt(
                Mathf.Pow(playerManager.Rigidbody.velocity.x, 2) +
                Mathf.Pow(playerManager.Rigidbody.velocity.z, 2));
            speedometerSlider.value = currentSpeed;
            speedText.text = $"{currentSpeed:F2} u/s";

            ChangeColorBasedOnSpeed();
        }

        private void ChangeColorBasedOnSpeed()
        {
            speedometerSlider.fillRect.GetComponent<Image>().color = defaultSliderColor *
                                                                     (1 - speedometerSlider.value /
                                                                         maxAchieveableSpeed) +
                                                                     breachSliderColor * speedometerSlider.value /
                                                                     maxAchieveableSpeed;
        }
    }
}
