using System;
using UnityEngine;

namespace Environment
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private Vector3 spawnPosition;
        [SerializeField] private Quaternion spawnRotation;
        [SerializeField] private bool setSpawnPointToCurrentPosition = false;
        [SerializeField] private GameManager gameManager;
        private bool checkpointReached;

        private void OnValidate()
        {
            if (gameManager == null)
                gameManager = FindObjectOfType<GameManager>();

            if (setSpawnPointToCurrentPosition)
            {
                spawnPosition = transform.position;
                spawnRotation = transform.rotation;
                setSpawnPointToCurrentPosition = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!checkpointReached && other.CompareTag("Player"))
            {
                gameManager.UpdateSpawnPoint(spawnPosition, spawnRotation);
                checkpointReached = true;
            }
        }
    }
}
