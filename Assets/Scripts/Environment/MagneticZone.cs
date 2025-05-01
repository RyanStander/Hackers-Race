using UnityEngine;

namespace Environment
{
    public class MagneticZone : MonoBehaviour
    {
        [SerializeField] private float pullForce = 10f;
        [SerializeField] private LayerMask playerLayer;

        private void OnTriggerStay(Collider other)
        {
            if ((playerLayer.value & (1 << other.gameObject.layer)) == 0) return;

            Rigidbody rb = other.attachedRigidbody;
            if (rb == null) return;
            
            // Calculate the direction to the center of the magnetic zone
            Vector3 direction = (transform.position - other.transform.position).normalized;
            // Apply a force towards the center of the magnetic zone
            rb.AddForce(direction * pullForce, ForceMode.Acceleration);
        }

        private void Reset()
        {
            BoxCollider box = GetComponent<BoxCollider>();
            box.isTrigger = true;
        }
    }
}
