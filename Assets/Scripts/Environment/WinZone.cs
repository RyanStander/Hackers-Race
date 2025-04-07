using System;
using UnityEngine;

namespace Environment
{
    public class WinZone : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;

        private void OnValidate()
        {
            if (gameManager == null)
                gameManager = FindObjectOfType<GameManager>();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                gameManager.PlayerWin();
            }
        }
    }
}
