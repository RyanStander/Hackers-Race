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
    [SerializeField] private float outOfBoundsY = -50f;
    [SerializeField] private bool restartOnDeath = true;

    private Vector3 spawnPoint;
    private Quaternion spawnRotation;

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
        spawnRotation = player.transform.rotation;
    }

    private void FixedUpdate()
    {
        if (player.transform.position.y < outOfBoundsY)
            Respawn();
    }

    private void RestartLevel()
    {
        timeTracker.StartCounting();
    }

    public void Respawn()
    {
        player.transform.position = spawnPoint;
        player.transform.rotation = spawnRotation;
        player.Rigidbody.velocity = Vector3.zero;
        player.Rigidbody.angularVelocity = Vector3.zero;
        player.PlayerController.ResetAllInputs();
        player.PlayerDash.ResetDashes();

        player.PlayerLook.fpsCamera.ForceCameraPosition(player.transform.position, player.transform.rotation);

        if (restartOnDeath)
            RestartLevel();
    }

    public void PlayerWin()
    {
        timeTracker.gameObject.SetActive(false);

        player.PlayerController.ResetAllInputs();
        player.PlayerController.ToggleInputs(false);

        inputProvider.enabled = false;

        leaderboard.OpenLeaderboard(TimeSpanConverter.ConvertToFloat(timeTracker.GetTotalTime()));
    }

    public void UpdateSpawnPoint(Vector3 newSpawnPoint, Quaternion newSpawnRotation)
    {
        spawnPoint = newSpawnPoint;
        spawnRotation = newSpawnRotation;
    }
}
