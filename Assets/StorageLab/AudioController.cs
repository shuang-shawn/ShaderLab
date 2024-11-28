using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioController : MonoBehaviour
{
    public static AudioController aCtrl;
    public GameObject bgMusic1;
    public AudioSource sfxSrc;
    private AudioSource levelMusic;
    public List<AudioSource> musicList;
    private int currentTrackIndex = 0;
    private AudioSource currentSong;
    public void Awake()
    {
        if (aCtrl == null)
        {
            aCtrl = this;
        } else if (aCtrl != this)
        {
            Destroy(gameObject); // Ensure only one instance exists
        }

        DontDestroyOnLoad(gameObject); // Optional: persists across scenes
    }
    public void PlaySFX()
    {
        //aCtrl.sfxSrc.Play() //this does the same thing
        sfxSrc.Play();
    }
    public void StopMusic()
    {
        musicList[currentTrackIndex].Stop();
    }
    public void PauseMusic()
    {
        musicList[currentTrackIndex].Pause();
    }
    public void PlayMusic()
    {
        musicList[currentTrackIndex].Play();
    }

    public void ChangeMusic(){
        musicList[currentTrackIndex].Stop();

        // Increment the track index
        currentTrackIndex = (currentTrackIndex + 1) % musicList.Count;

        // Play the next track
        musicList[currentTrackIndex].Play();
    }

    //more functions to dynamically add new sounds
}