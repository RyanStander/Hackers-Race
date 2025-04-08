using Player;
using TMPro;
using UnityEngine;

public class DashDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dashText;
    [SerializeField] private PlayerManager playerManager;

    private void OnValidate()
    {
        if (dashText == null)
            dashText = GetComponent<TextMeshProUGUI>();

        if (playerManager == null)
            playerManager = FindObjectOfType<PlayerManager>();
    }

    private void FixedUpdate()
    {
        dashText.text= playerManager.PlayerDash.GetDashCount().ToString();
    }
}
