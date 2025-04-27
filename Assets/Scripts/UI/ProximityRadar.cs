using System;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProximityRadar : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Transform mainCamera;

        [SerializeField] RectTransform radarUI; // The UI parent (circle) for the radar
        [SerializeField] GameObject blipPrefab; // Prefab for the blip (simple Image or Sprite)

        [Header("Radar Settings")] [SerializeField]
        float detectionRadius = 30f; // How far to detect sentries

        [SerializeField] LayerMask sentryMask; // What layers count as sentries
        [SerializeField] float radarRadius = 100f; // UI size radius (in pixels)

        [Header("Blip Scaling")] [SerializeField]
        float minBlipSize = 10f;

        [SerializeField] float maxBlipSize = 30f;
        [SerializeField] Color farColor = Color.green;
        [SerializeField] Color nearColor = Color.red;
        [SerializeField] float minDangerDistance = 5f; // Danger zone for max alert

        private List<GameObject> activeBlips = new List<GameObject>();

        private void OnValidate()
        {
            if (mainCamera == null)
                mainCamera = Camera.main.transform;
        }

        void FixedUpdate()
        {
            UpdateRadar();
        }

        void UpdateRadar()
        {
            // Clear old blips
            foreach (var blip in activeBlips)
            {
                Destroy(blip);
            }

            activeBlips.Clear();

            // Find all sentries
            Collider[] sentries = Physics.OverlapSphere(mainCamera.position, detectionRadius, sentryMask);

            foreach (var sentry in sentries)
            {
                Vector3 dirToSentry = (sentry.transform.position - mainCamera.position);
                dirToSentry.y = 0f; // Flatten for horizontal plane

                float distance = dirToSentry.magnitude;
                if (distance < 0.1f) continue; // Skip if somehow too close

                // Calculate angle relative to player forward
                float angle = Vector3.SignedAngle(mainCamera.forward, dirToSentry.normalized, Vector3.up);

                // Convert angle to position on radar
                float rad = angle * Mathf.Deg2Rad;
                Vector2 blipPos = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad)) * radarRadius;

                // Instantiate blip
                GameObject blip = Instantiate(blipPrefab, radarUI);
                RectTransform blipRect = blip.GetComponent<RectTransform>();
                blipRect.anchoredPosition = blipPos;

                // Scale blip based on distance
                float proximityFactor = Mathf.InverseLerp(detectionRadius, minDangerDistance, distance);
                float size = Mathf.Lerp(minBlipSize, maxBlipSize, proximityFactor);
                blipRect.sizeDelta = new Vector2(size, size);

                // Color blip based on distance
                Color color = Color.Lerp(farColor, nearColor, proximityFactor);
                blip.GetComponent<Image>().color = color;

                activeBlips.Add(blip);
            }
        }
    }
}
