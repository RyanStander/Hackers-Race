using System;
using UnityEngine;

namespace Environment
{
    using UnityEngine;
    using System.Collections;

    public class MovingObject : MonoBehaviour
    {
        [SerializeField] private float speed = 1.0f;
        [SerializeField] private Vector3 moveAmounts;
        [SerializeField] private float startDelay;
        [SerializeField] private float pauseDuration = 0f; // Delay between direction changes

        private Vector3 startingPosition;
        private Vector3 endingPosition;
        private bool hasReachedEnd = false;
        private bool isWaiting = false;

        private void Start()
        {
            startingPosition = transform.position;
            endingPosition = startingPosition + moveAmounts;
            StartCoroutine(InitialWait());
        }

        private void FixedUpdate()
        {
            if (isWaiting) return;

            if (hasReachedEnd)
            {
                transform.position = Vector3.MoveTowards(transform.position, startingPosition, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, startingPosition) < 0.01f)
                {
                    StartCoroutine(WaitThenSwitch(false));
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, endingPosition, speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, endingPosition) < 0.01f)
                {
                    StartCoroutine(WaitThenSwitch(true));
                }
            }
        }

        private IEnumerator WaitThenSwitch(bool toEnd)
        {
            isWaiting = true;
            yield return new WaitForSeconds(pauseDuration);
            hasReachedEnd = toEnd;
            isWaiting = false;
        }

        private IEnumerator InitialWait()
        {
            isWaiting = true;
            yield return new WaitForSeconds(startDelay);
            
            isWaiting = false;
        }
    }

}
