using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DashDisplay : MonoBehaviour
{
    [SerializeField] private GameObject dashIcon1;
    [SerializeField] private GameObject dashIcon2;
    [SerializeField] private GameObject dashIcon3;
    [SerializeField] private Slider dashCooldownSlider;
    [SerializeField] private PlayerManager playerManager;

    private void OnValidate()
    {
        if (playerManager == null)
            playerManager = FindObjectOfType<PlayerManager>();

        if (dashCooldownSlider == null)
            dashCooldownSlider = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        dashCooldownSlider.maxValue = playerManager.PlayerDash.GetDashCooldownDuration();
        dashCooldownSlider.value = dashCooldownSlider.maxValue;
    }

    private void FixedUpdate()
    {
        if (playerManager.PlayerDash.GetDashCount() > 2)
        {
            dashIcon1.SetActive(true);
            dashIcon2.SetActive(true);
            dashIcon3.SetActive(true);
        }
        else if (playerManager.PlayerDash.GetDashCount() > 1)
        {
            dashIcon1.SetActive(true);
            dashIcon2.SetActive(true);
            dashIcon3.SetActive(false);
        }
        else if (playerManager.PlayerDash.GetDashCount() > 0)
        {
            dashIcon1.SetActive(true);
            dashIcon2.SetActive(false);
            dashIcon3.SetActive(false);
        }
        else
        {
            dashIcon1.SetActive(false);
            dashIcon2.SetActive(false);
            dashIcon3.SetActive(false);
        }

        dashCooldownSlider.value = playerManager.PlayerDash.IsCooldownActive()
            ? dashCooldownSlider.maxValue - playerManager.PlayerDash.GetDashRemainingCooldown()
            : dashCooldownSlider.maxValue;
    }
}
