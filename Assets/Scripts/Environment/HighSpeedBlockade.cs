using System;
using Player;
using UnityEngine;

namespace Environment
{
    public class HighSpeedBlockade : MonoBehaviour
    {
        [SerializeField] private Collider blockadeCollider;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Material normalMaterial;
        [SerializeField] private Material vulnerableMaterial;

        private void OnValidate()
        {
            if (blockadeCollider == null)
                blockadeCollider = GetComponent<Collider>();

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
            blockadeCollider.isTrigger = true;
            meshRenderer.material = vulnerableMaterial;
        }

        private void BecomeImpenetrable()
        {
            blockadeCollider.isTrigger = false;
            meshRenderer.material = normalMaterial;
        }
    }
}
