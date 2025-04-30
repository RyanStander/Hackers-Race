using System;
using UnityEngine;

namespace Environment
{
    public class RotatingObject : MonoBehaviour
    {
        [SerializeField] private float speed = 50f; // Degrees per second

        private void FixedUpdate()
        {
            transform.Rotate(Vector3.up, speed * Time.deltaTime);
        }
    }
}
