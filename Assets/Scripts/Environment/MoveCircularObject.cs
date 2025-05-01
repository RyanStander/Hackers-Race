using UnityEngine;

namespace Environment
{
    public class CircularSentryMotion : MonoBehaviour
    {
        [SerializeField] private Transform centerPoint;
        [SerializeField] private float radius = 5f;
        [SerializeField] private float angularSpeed = 90f; // Degrees per second
        [SerializeField] private float startAngle = 0f;
        [SerializeField] private float rotationAngle = 360f; // How far around the circle to show the path
        [SerializeField] private bool clockwise = true;
        [SerializeField] private bool drawPath = true;
        [SerializeField] private int pathResolution = 60;

        private float currentAngle;

        private void Start()
        {
            if (centerPoint == null)
            {
                centerPoint = new GameObject("SentryCenter").transform;
                centerPoint.position = transform.position - new Vector3(radius, 0, 0);
            }

            currentAngle = startAngle;
        }

        private void Update()
        {
            float deltaAngle = angularSpeed * Time.deltaTime * (clockwise ? -1f : 1f);
            currentAngle += deltaAngle;

            float rad = currentAngle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * radius;
            transform.position = centerPoint.position + offset;
        }

        private void OnDrawGizmos()
        {
            if (!drawPath || centerPoint == null) return;

            Gizmos.color = Color.cyan;
            float step = rotationAngle / pathResolution;
            Vector3 prevPoint = centerPoint.position + Quaternion.Euler(0, -startAngle, 0) * Vector3.forward * radius;

            for (int i = 1; i <= pathResolution; i++)
            {
                float angle = -startAngle + step * i * (clockwise ? -1f : 1f);
                Vector3 nextPoint = centerPoint.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * radius;
                Gizmos.DrawLine(prevPoint, nextPoint);
                prevPoint = nextPoint;
            }

            // Optional: draw center
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(centerPoint.position, 0.1f);
        }
    }
}
