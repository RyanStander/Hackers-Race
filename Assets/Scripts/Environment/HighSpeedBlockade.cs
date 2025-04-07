using System;
using Player;
using UnityEngine;

namespace Environment
{
    public class HighSpeedBlockade : MonoBehaviour
    {
        [SerializeField] private Collider collider;
        [SerializeField] private MeshRenderer meshRenderer;

        private void OnValidate()
        {
            if (collider == null)
                collider = GetComponent<Collider>();

            if (meshRenderer == null)
                meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnEnable()
        {
            PlayerMovement.HighSpeedAchieved += BecomeVulnerable;
            PlayerMovement.ReturnedToLowSpeed += BecomeImpenetrable;
        }

        private void OnDisable()
        {
            PlayerMovement.HighSpeedAchieved -= BecomeVulnerable;
            PlayerMovement.ReturnedToLowSpeed -= BecomeImpenetrable;
        }

        private void BecomeVulnerable()
        {
            collider.isTrigger = true;
            meshRenderer.enabled = false;
        }

        private void BecomeImpenetrable()
        {
            collider.isTrigger = false;
            meshRenderer.enabled = true;
        }
    }
}
