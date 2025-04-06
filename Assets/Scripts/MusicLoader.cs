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
    private readonly string[] supportedExtensions = { ".mp3", ".wav", ".ogg" };

    private void OnValidate()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(LoadFirstMusicFile());
    }

       private IEnumerator LoadFirstMusicFile()
    {
        string musicFolderPath = Path.Combine(Application.streamingAssetsPath, "Music");
        
        string[] files = Directory.GetFiles(musicFolderPath);
        string firstMusicFile = files.FirstOrDefault(f => supportedExtensions.Contains(Path.GetExtension(f).ToLower()));

        if (firstMusicFile == null)
        {
            Debug.LogWarning("No audio file found.");
            yield break;
        }

        string filePath = "file://" + firstMusicFile.Replace("\\", "/");

        // Load the clip
        AudioType audioType = GetAudioType(Path.GetExtension(firstMusicFile));
        UnityWebRequest unityWebRequest = UnityWebRequestMultimedia.GetAudioClip(filePath, audioType);
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Failed to load audio: {unityWebRequest.error}");
        }
        else
        {
            if (audioSource.clip != null)
            {
                audioSource.Stop();
                AudioClip currentClip = audioSource.clip;
                audioSource.clip = null;
                currentClip.UnloadAudioData();
                DestroyImmediate(currentClip,false);
            }

            audioSource.loop = true;
            audioSource.volume = 0.2f;
            AudioClip clip = DownloadHandlerAudioClip.GetContent(unityWebRequest);
            audioSource.clip = clip;
            audioSource.Play();
            yield return null;
        }
    }

    AudioType GetAudioType(string extension)
    {
        switch (extension.ToLower())
        {
            case ".mp3": return AudioType.MPEG;
            case ".wav": return AudioType.WAV;
            case ".ogg": return AudioType.OGGVORBIS;
            default: return AudioType.UNKNOWN;
        }
    }
}
