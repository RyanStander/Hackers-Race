using Player;
using UnityEngine;

namespace Environment
{
    public class LookAtPlayer : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float rotationSpeed = 5.0f;

        private void OnValidate()
        {
            if (playerTransform == null)
                playerTransform = FindObjectOfType<PlayerManager>().transform;
        }

        private void Update()
        {
            Vector3 direction = playerTransform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            targetRotation *= Quaternion.Euler(80, -90, 15);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
