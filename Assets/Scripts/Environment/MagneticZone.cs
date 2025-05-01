using UnityEngine;

namespace Environment
{
    public class MagneticZone : MonoBehaviour
    {
        [SerializeField] private enum MagneticDirection { Left, Right }
        [SerializeField] private MagneticDirection direction = MagneticDirection.Left;

        [Tooltip("How strong the magnetic force is.")]
        [SerializeField] private float forceStrength = 10f;

        [Tooltip("Apply force every frame the player is inside the zone.")]
        [SerializeField] private bool continuousForce = true;

        private void OnTriggerStay(Collider other)
        {
            if (!continuousForce) return;

            Rigidbody rb = other.attachedRigidbody;
            if (rb != null && other.CompareTag("Player"))
            {
                Vector3 force = (direction == MagneticDirection.Left ? Vector3.left : Vector3.right) * forceStrength;
                rb.AddForce(force, ForceMode.Acceleration);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (continuousForce) return;

            Rigidbody rb = other.attachedRigidbody;
            if (rb != null && other.CompareTag("Player"))
            {
                Vector3 force = (direction == MagneticDirection.Left ? Vector3.left : Vector3.right) * forceStrength;
                rb.AddForce(force, ForceMode.Impulse);
            }
        }
    }
}
