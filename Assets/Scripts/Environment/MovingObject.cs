using System;
using UnityEngine;

namespace Environment
{
    public class MovingObject : MonoBehaviour
    {
        [SerializeField] float speed = 1.0f;
        private Vector3 startingPosition;
        private Vector3 endingPosition;
        [SerializeField] private Vector3 moveAmounts;
        private bool hasReachedEnd = false;
        private void Start()
        {
            startingPosition = transform.position;
            endingPosition = startingPosition + moveAmounts;
        }

        private void FixedUpdate()
        {
            if (hasReachedEnd)
            {
                transform.position = Vector3.MoveTowards(transform.position, startingPosition, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, startingPosition) < 0.01f)
                {
                    hasReachedEnd = false;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, endingPosition, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, endingPosition) < 0.01f)
                {
                    hasReachedEnd = true;
                }
            }
        }
    }
}
