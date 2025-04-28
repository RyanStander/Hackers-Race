using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Player
{
    public class PlayerLook : MonoBehaviour
    {
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private Camera mainCamera;
        public CinemachineVirtualCamera fpsCamera;
        private PlayerController playerController;
        private float originalFov;
        [SerializeField] private float dashFov = 120f;
        [SerializeField] private float sprintFov = 90f;

        [SerializeField] private float dashFovDuration = 0.5f;
        private Coroutine dashFovCoroutine;

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

            if (playerController.SprintInput && (playerController.Forward != 0 || playerController.Left != 0))
                fpsCamera.m_Lens.FieldOfView = Mathf.Lerp(fpsCamera.m_Lens.FieldOfView, sprintFov, Time.deltaTime * 5);
            else
                fpsCamera.m_Lens.FieldOfView =
                    Mathf.Lerp(fpsCamera.m_Lens.FieldOfView, originalFov, Time.deltaTime * 5);

            if (playerController.DashInput && playerManager.PlayerDash.CanDash())
            {
                if (dashFovCoroutine != null)
                {
                    StopCoroutine(dashFovCoroutine);
                }

                dashFovCoroutine = StartCoroutine(DashFovEffect());
            }
        }

        private IEnumerator DashFovEffect()
        {
            float elapsed = 0f;
            float startFov = fpsCamera.m_Lens.FieldOfView;
            float lerpDuration = 0.2f; // Adjust this to control how fast the FOV shift happens

            // Smoothly lerp the FOV to the dash FOV
            while (elapsed < lerpDuration)
            {
                fpsCamera.m_Lens.FieldOfView = Mathf.Lerp(startFov, dashFov, elapsed / lerpDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Make sure it ends exactly at dashFov
            fpsCamera.m_Lens.FieldOfView = dashFov;

            // Wait while dash is active
            yield return new WaitForSeconds(dashFovDuration);

            // After dash, snap back to sprint FOV or original FOV
            if (playerController.SprintInput)
                fpsCamera.m_Lens.FieldOfView = sprintFov;
            else
                fpsCamera.m_Lens.FieldOfView = originalFov;
        }
    }
}
