using System;
using Cinemachine;
using UnityEngine;

namespace Player
{
    public class PlayerLook : MonoBehaviour
    {
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private Camera mainCamera;
        private PlayerController playerController;

        private void OnValidate()
        {
            if (playerManager == null)
                playerManager = GetComponent<PlayerManager>();
            
            if (mainCamera == null)
                mainCamera = Camera.main;
        }
        
        private void Start()
        {
            playerController = playerManager.PlayerController;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            playerManager.transform.rotation = Quaternion.Euler(0, mainCamera.transform.rotation.eulerAngles.y, 0);
        }
    }
}
