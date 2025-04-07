using System;
using Cinemachine;
using Leaderboard;
using Player;
using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PlayerManager player;
        [SerializeField] private TimeTracker timeTracker;
        [SerializeField] private Leaderboard.Leaderboard leaderboard;
        [SerializeField] private CinemachineInputProvider inputProvider;

        private Vector3 spawnPoint;
        private void OnValidate()
        {
            if (player == null)
                player = FindObjectOfType<PlayerManager>();
            
            if (timeTracker == null)
                timeTracker = FindObjectOfType<TimeTracker>();
            
            if (leaderboard == null)
                leaderboard = FindObjectOfType<Leaderboard.Leaderboard>();
            
            if (inputProvider == null)
                inputProvider = FindObjectOfType<CinemachineInputProvider>();
        }

        private void Start()
        {
            spawnPoint = player.transform.position;
        }

        public void RestartLevel()
        {
            player.transform.position = spawnPoint;
            player.transform.rotation = Quaternion.identity;
            player.Rigidbody.velocity = Vector3.zero;
            player.Rigidbody.angularVelocity = Vector3.zero;
            player.PlayerController.ResetAllInputs();
            player.PlayerDash.ResetDashes();

            player.PlayerLook.fpsCamera.ForceCameraPosition(player.transform.position, player.transform.rotation);
            
            timeTracker.StartCounting();
        }
        
        public void PlayerWin()
        {
            timeTracker.gameObject.SetActive(false);
            
            player.PlayerController.ResetAllInputs();
            player.PlayerController.ToggleInputs(false);
            
            inputProvider.enabled = false;
            
            leaderboard.OpenLeaderboard(TimeSpanConverter.ConvertToFloat(timeTracker.GetTotalTime()));
        }
    }
