using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> playlist;

    [Header("UI Mute Settings")]
    public Image muteButtonImage;
    public Sprite iconSoundOn;
    public Sprite iconSoundOff;

    private int currentTrackIndex = 0;
    private bool isMuted = false;

    void Start()
    {
        if (playlist.Count > 0)
        {
            PlayTrack(currentTrackIndex);
        }
        UpdateMuteIcon();
    }

    void PlayTrack(int index)
    {
        if (playlist.Count == 0) return; // Mencegah error kalau list kosong

        audioSource.clip = playlist[index];
        audioSource.Play();
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        audioSource.mute = isMuted;
        UpdateMuteIcon();
    }

    void UpdateMuteIcon()
    {
        if (muteButtonImage != null)
        {
            muteButtonImage.sprite = isMuted ? iconSoundOff : iconSoundOn;
        }
    }

    // --- BAGIAN YANG TADI MUNGKIN HILANG ---

    public void NextTrack()
    {
        currentTrackIndex++;
        if (currentTrackIndex >= playlist.Count)
        {
            currentTrackIndex = 0; // Balik ke lagu pertama
        }
        PlayTrack(currentTrackIndex);
    }

    public void PreviousTrack()
    {
        currentTrackIndex--;
        if (currentTrackIndex < 0)
        {
            currentTrackIndex = playlist.Count - 1; // Balik ke lagu terakhir
        }
        PlayTrack(currentTrackIndex);
    }
}