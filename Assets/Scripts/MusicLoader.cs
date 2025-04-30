using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class MusicLoader : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float volume = 0.1f;

    private void OnValidate()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        string savedPath = PlayerPrefs.GetString("SelectedSong", "");
        if (!string.IsNullOrEmpty(savedPath))
            StartCoroutine(LoadAndPlaySelectedSong(savedPath));
    }

    IEnumerator LoadAndPlaySelectedSong(string filePath)
    {
        string url = "file://" + filePath.Replace("\\", "/");
        using var www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
            GetComponent<AudioSource>().clip = clip;
            GetComponent<AudioSource>().Play();
            GetComponent<AudioSource>().volume = volume;
        }
    }

}
