using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioController : MonoBehaviour
{
    public static AudioController aCtrl;
    public GameObject bgMusic1;
    public AudioSource sfxSrc;
    public AudioSource enemyDie;

    public AudioSource enemyRespawn;
    public AudioSource walk;
    public AudioSource bump;

    

    private AudioSource levelMusic;
    public List<AudioSource> musicList;
    private int currentTrackIndex = 0;
    private AudioSource currentSong;
    public bool isBackgroundPlaying = false;
    private int currentPlayingIndex = 0;
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
    public void PlayBumpIntoWall() {
        bump.Play();
    }
    public void PlayWalk() {
        walk.Play();
    }
    public void PlaySFX()
    {
        //aCtrl.sfxSrc.Play() //this does the same thing
        sfxSrc.Play();
    }
    public void PlayEnemyDie() {
        enemyDie.Play();
    }
    public void PlayEnemyRespawn() {
        enemyRespawn.Play();
    }

    public void ToggleBackgroundMusic() {
        isBackgroundPlaying = !isBackgroundPlaying;
        if (GlobalAmbientControl.ambientControl != null) {
            if (GlobalAmbientControl.ambientControl.isDay) {
                currentPlayingIndex = 0;
            } else {
                currentPlayingIndex = 1;
            }
            if (isBackgroundPlaying) {
                musicList[currentPlayingIndex].Play();
            } else {
                musicList[currentPlayingIndex].Stop();
            }
        }
    }

    public void SwitchBackgroundMusic() {
        musicList[currentPlayingIndex].Stop();
        currentPlayingIndex = (currentPlayingIndex + 1) % 2;
        musicList[currentPlayingIndex].Play();

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