using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class TutorialHandler : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PlayerManager player;
    [Header("Intro")] [SerializeField] private AudioClip introLine;
    [SerializeField] private AudioClip introLine2;
    [SerializeField] private AudioClip wasdInfoLine;
    [SerializeField] private GameObject wasdAndCameraInfo;
    [SerializeField] private AudioClip sprintInfoLine;
    [SerializeField] private GameObject sprintInfo;
    [SerializeField] private float takingTooLongWaitDuration = 5f;
    private float takingTooLongTimeStamp;
    [SerializeField] private AudioClip takingTooLongLine;
    private bool playedTakingTooLongLine;
    [SerializeField] AudioClip finishedIntroLine;
    [SerializeField] private GameObject doorCheck;
    [SerializeField] private GameObject doorStatusUnlocked;
    [Header("Sliding")] [SerializeField] private AudioClip enterSlideLine;
    [SerializeField] private AudioClip launchSlideLine;
    [SerializeField] private AudioClip enteringPipeLine;

    [Header("JumpGaps")] [SerializeField] private GameObject jumpGapsInfo;
    [SerializeField] private AudioClip enterJumpGapsLine;
    [SerializeField] private AudioClip performedJumpLine;
    [SerializeField] private AudioClip longGapLine;
    [SerializeField] private AudioClip heightJumpLine;
    [SerializeField] private AudioClip angleRampLine;
    [SerializeField] private AudioClip alternatingAngleRampLine;

    [Header("Dashing")] [SerializeField] private PlayerDash playerDash;
    [SerializeField] private GameObject dashInfo;
    [SerializeField] private GameObject dashUI;
    [SerializeField] private AudioClip dashInfoLine;

    [Header("Firewall")] [SerializeField] private AudioClip enterFirewallLine;
    [Header("Platform")] [SerializeField] private AudioClip enterPlatformLine;

    [Header("Moving Wall")] [SerializeField]
    private AudioClip enterMovingWallLine;

    [Header("Sentries")] [SerializeField] private AudioClip enterSentriesLine;
    [Header("Win")] [SerializeField] private AudioClip enterWinLine;

    private void OnValidate()
    {
        if (audioSource == null)
            audioSource = gameObject.GetComponent<AudioSource>();

        if (player == null)
            player = FindObjectOfType<PlayerManager>();

        playerDash = player.PlayerDash;
    }

    private void Start()
    {
        StartCoroutine(StartIntro());
    }

    public void StartTutorialPart(string tutorialPartName)
    {
        switch (tutorialPartName)
        {
            //----------Sliding----------
            case "EnterSlide":
                EnterSlide();
                break;
            case "LaunchSlide":
                LaunchSlide();
                break;
            case "EnterPipe":
                EnterPipe();
                break;
            //----------Jumps----------
            case "StartJumpGaps":
                StartCoroutine(JumpGaps());
                break;
            case "LongGap":
                LongGap();
                break;
            case "HeightJump":
                HeightJump();
                break;
            case "AngleRamp":
                AngleRamp();
                break;
            case "AlternatingAngleRamp":
                AlternatingAngleRamp();
                break;
            //----------Others----------
            case "DashInfo":
                StartCoroutine(DashInfo());
                break;
            case "EnterFirewall":
                EnterFirewall();
                break;
            case "EnterPlatform":
                EnterPlatform();
                break;
            case "EnterMovingWall":
                EnterMovingWall();
                break;
            case "EnterSentries":
                EnterSentries();
                break;
            case "EnterWin":
                EnterWin();
                break;
            default:
                Debug.LogError($"Tutorial part {tutorialPartName} not found.");
                break;
        }
    }

    #region Intro

    private IEnumerator StartIntro()
    {
        audioSource.Stop();
        audioSource.clip = introLine;
        audioSource.Play();

        while (audioSource.isPlaying)
            yield return null;

        audioSource.clip = introLine2;
        audioSource.Play();

        while (audioSource.isPlaying)
            yield return null;

        audioSource.clip = wasdInfoLine;
        audioSource.Play();
        wasdAndCameraInfo.SetActive(true);

        while (audioSource.isPlaying)
            yield return null;

        takingTooLongTimeStamp = Time.time + takingTooLongWaitDuration;

        while (player.PlayerController.Forward == 0 && player.PlayerController.Left == 0)
        {
            if (Time.time > takingTooLongTimeStamp)
            {
                audioSource.Stop();
                audioSource.clip = takingTooLongLine;
                audioSource.Play();
                takingTooLongTimeStamp = Mathf.Infinity;
                playedTakingTooLongLine = true;
            }

            yield return null;
        }

        wasdAndCameraInfo.SetActive(false);
        audioSource.Stop();
        audioSource.clip = sprintInfoLine;
        audioSource.Play();
        sprintInfo.SetActive(true);

        while (audioSource.isPlaying)
            yield return null;

        takingTooLongTimeStamp = Time.time + takingTooLongWaitDuration;

        while (player.PlayerController.SprintInput == false)
        {
            if (!playedTakingTooLongLine && Time.time > takingTooLongTimeStamp)
            {
                audioSource.Stop();
                audioSource.clip = takingTooLongLine;
                audioSource.Play();
                takingTooLongTimeStamp = Mathf.Infinity;
            }

            yield return null;
        }

        sprintInfo.SetActive(false);
        audioSource.Stop();

        audioSource.clip = finishedIntroLine;
        audioSource.Play();

        doorCheck.SetActive(true);
        doorStatusUnlocked.SetActive(true);
    }

    #endregion

    #region Sliding

    private void EnterSlide()
    {
        audioSource.Stop();
        audioSource.clip = enterSlideLine;
        audioSource.Play();
    }

    private void LaunchSlide()
    {
        audioSource.Stop();
        audioSource.clip = launchSlideLine;
        audioSource.Play();
    }

    private void EnterPipe()
    {
        audioSource.Stop();
        audioSource.clip = enteringPipeLine;
        audioSource.Play();
    }

    #endregion

    #region Jump Gaps

    private IEnumerator JumpGaps()
    {
        audioSource.Stop();
        audioSource.clip = enterJumpGapsLine;
        jumpGapsInfo.SetActive(true);
        audioSource.Play();

        while (player.PlayerController.JumpInput == false)
            yield return null;

        audioSource.Stop();
        jumpGapsInfo.SetActive(false);
        audioSource.clip = performedJumpLine;
        audioSource.Play();
    }

    private void LongGap()
    {
        audioSource.Stop();
        audioSource.clip = longGapLine;
        audioSource.Play();
    }

    private void HeightJump()
    {
        audioSource.Stop();
        audioSource.clip = heightJumpLine;
        audioSource.Play();
    }

    private void AngleRamp()
    {
        audioSource.Stop();
        audioSource.clip = angleRampLine;
        audioSource.Play();
    }

    private void AlternatingAngleRamp()
    {
        audioSource.Stop();
        audioSource.clip = alternatingAngleRampLine;
        audioSource.Play();
    }

    #endregion

    #region Others

    private IEnumerator DashInfo()
    {
        audioSource.Stop();
        audioSource.clip = dashInfoLine;
        dashInfo.SetActive(true);
        playerDash.enabled = true;
        dashUI.SetActive(true);
        audioSource.Play();

        while (player.PlayerController.DashInput == false)
            yield return null;

        dashInfo.SetActive(false);
    }

    private void EnterFirewall()
    {
        audioSource.Stop();
        audioSource.clip = enterFirewallLine;
        audioSource.Play();
    }

    private void EnterPlatform()
    {
        audioSource.Stop();
        audioSource.clip = enterPlatformLine;
        audioSource.Play();
    }

    private void EnterMovingWall()
    {
        audioSource.Stop();
        audioSource.clip = enterMovingWallLine;
        audioSource.Play();
    }

    private void EnterSentries()
    {
        audioSource.Stop();
        audioSource.clip = enterSentriesLine;
        audioSource.Play();
    }

    private void EnterWin()
    {
        audioSource.Stop();
        audioSource.clip = enterWinLine;
        audioSource.Play();
    }

    #endregion
}
