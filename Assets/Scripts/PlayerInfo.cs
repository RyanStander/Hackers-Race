using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerInfoText;
    [SerializeField] private PlayerManager playerManager;

    private void OnValidate()
    {
        if (playerInfoText == null)
            playerInfoText = GetComponent<TextMeshProUGUI>();
        
        if (playerManager == null)
            playerManager = FindObjectOfType<PlayerManager>();
    }

    private void Update()
    {
        playerInfoText.text = "Velocity: " + playerManager.Rigidbody.velocity + "\n" +
                              "Magnitude: " + playerManager.Rigidbody.velocity.magnitude.ToString("F2") + "\n";
    }
}
