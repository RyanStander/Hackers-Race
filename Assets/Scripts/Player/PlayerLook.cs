using System;
using Cinemachine;
using UnityEngine;

namespace Player
{
    public class PlayerLook : MonoBehaviour
    {
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineVirtualCamera fpsCamera;
        private PlayerController playerController;
        private float originalFov;

        private void OnValidate()
        {
            if (playerManager == null)
                playerManager = GetComponent<PlayerManager>();
            
            if (mainCamera == null)
                mainCamera = Camera.main;
            
            if (fpsCamera == null)
                fpsCamera = FindObjectOfType<CinemachineVirtualCamera>();
        }
        
        private void Start()
        {
            playerController = playerManager.PlayerController;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            originalFov = fpsCamera.m_Lens.FieldOfView;
        }

        private void Update()
        {
            playerManager.transform.rotation = Quaternion.Euler(0, mainCamera.transform.rotation.eulerAngles.y, 0);
            
            if(playerController.SprintInput)
                fpsCamera.m_Lens.FieldOfView = Mathf.Lerp(fpsCamera.m_Lens.FieldOfView, 90, Time.deltaTime * 5);
            else
                fpsCamera.m_Lens.FieldOfView = Mathf.Lerp(fpsCamera.m_Lens.FieldOfView, originalFov, Time.deltaTime * 5);
        }
    }
}
