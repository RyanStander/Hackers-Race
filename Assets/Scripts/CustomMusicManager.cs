using System.IO;
using System.Linq;
using System.Collections;
using System.Diagnostics;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CustomMusicManager : MonoBehaviour
{
    [SerializeField] private Transform songListParent; // Parent object for song buttons
    [SerializeField] private GameObject songButtonPrefab; // Prefab with a Text and Button
    [SerializeField] private AudioSource previewAudioSource;
    [SerializeField] private TextMeshProUGUI selectedSongText;

    private string musicFolderPath;

    private void Start()
    {
        musicFolderPath = Path.Combine(Application.persistentDataPath, "CustomMusic");
        if (!Directory.Exists(musicFolderPath))
            Directory.CreateDirectory(musicFolderPath);
        
        selectedSongText.text = PlayerPrefs.GetString("SelectedSongName", "");
    }

    public void OpenMusicFolder()
    {
        Process.Start(musicFolderPath);
    }

    public void ReloadMusicList()
    {
        // Clear previous list
        foreach (Transform child in songListParent)
            Destroy(child.gameObject);

        string[] files = Directory.GetFiles(musicFolderPath, "*.*")
            .Where(f => f.EndsWith(".mp3") || f.EndsWith(".wav") || f.EndsWith(".ogg"))
            .ToArray();

        foreach (string filePath in files)
        {
            string fileName = Path.GetFileName(filePath);
            GameObject buttonGO = Instantiate(songButtonPrefab, songListParent);
            SongButton songButton = buttonGO.GetComponent<SongButton>();
                
            songButton.SongNameText.text = fileName;

            songButton.PlayButton.onClick.AddListener(() =>
            {
                StartCoroutine(PreviewSong(filePath));
            });
            
            songButton.StopButton.onClick.AddListener(StopPreview);
            
            songButton.SelectSongButton.onClick.AddListener(() =>
            {
                PlayerPrefs.SetString("SelectedSong", filePath);
                PlayerPrefs.SetString("SelectedSongName", fileName);
                PlayerPrefs.Save();
                selectedSongText.text = fileName;
            });
        }
    }

    public IEnumerator PreviewSong(string filePath)
    {
        if (previewAudioSource.isPlaying)
            previewAudioSource.Stop();

        string url = "file://" + filePath.Replace("\\", "/");
        using var www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
            previewAudioSource.clip = clip;
            previewAudioSource.Play();
        }
        else
        {
            UnityEngine.Debug.LogError("Failed to load song: " + www.error);
        }
    }

    public void StopPreview()
    {
        if (previewAudioSource.isPlaying)
            previewAudioSource.Stop();
    }
}
