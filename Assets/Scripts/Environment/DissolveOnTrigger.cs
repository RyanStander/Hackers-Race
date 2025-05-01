using System;
using UnityEngine;

namespace Environment
{
    public class DissolvePlatform : MonoBehaviour
    {
        [SerializeField] private float dissolveSpeed = 1f;
        [SerializeField] private float resetDelay = 10f;
        [SerializeField] private string dissolveProperty = "_Dissolve";
        [SerializeField] private float startDelay = 0f;

        private Material material;
        private float dissolveAmount = 0f;
        private bool isDissolving = false;
        private bool isRestoring = false;
        [SerializeField] private Collider dissolveCollider;
        [SerializeField] private Renderer dissolveRenderer;

        private void OnValidate()
        {
            if (dissolveCollider == null)
                dissolveCollider = GetComponent<Collider>();

            if (dissolveRenderer == null)
                dissolveRenderer = GetComponent<Renderer>();
        }

        private void Start()
        {
            material = dissolveRenderer.material;
            material.SetFloat(dissolveProperty, 0f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isDissolving && !isRestoring)
            {
                Invoke(nameof(StartDissolve), startDelay);
            }
        }

        private void StartDissolve()
        {
            isDissolving = true;
        }

        private void Update()
        {
            if (isDissolving)
            {
                dissolveAmount += Time.deltaTime * dissolveSpeed;
                material.SetFloat(dissolveProperty, dissolveAmount);

                if (dissolveAmount >= 1f)
                {
                    isDissolving = false;
                    dissolveCollider.enabled = false;
                    dissolveRenderer.enabled = false;

                    Invoke(nameof(StartRestore), resetDelay);
                }
            }

            if (isRestoring)
            {
                dissolveAmount -= Time.deltaTime * dissolveSpeed;
                material.SetFloat(dissolveProperty, dissolveAmount);

                if (dissolveAmount <= 0f)
                {
                    dissolveAmount = 0f;
                    isRestoring = false;
                }
            }
        }

        private void StartRestore()
        {
            dissolveRenderer.enabled = true;
            dissolveCollider.enabled = true;
            isRestoring = true;
        }
    }
}

