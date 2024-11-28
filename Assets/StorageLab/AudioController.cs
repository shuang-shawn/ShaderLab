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
    public float defaultVolume = 0.2f;
    public float fogVolumeFactor = 1f;
    public float maxVolume = 0.7f;
    public float currentVolume = 0.2f;
    public float minVolume = 0.1f;
    public float maxDistance = 5f;
    public Transform player;
    public Transform currentEnemy;

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
    private void FixedUpdate() {
        if (MazeGenerator.mazeGenerator.currentMonster != null) {

            currentEnemy = MazeGenerator.mazeGenerator.currentMonster.transform;
        }
        if (player != null && currentEnemy != null && musicList[currentPlayingIndex] != null)
        {
            // Calculate distance between the player and the enemy
            float distance = Vector3.Distance(player.position, currentEnemy.position);

            // Modulate volume based on distance
            float currentVolume = Mathf.Lerp(maxVolume, minVolume, distance / maxDistance) * fogVolumeFactor;

            // Clamp the volume to the range [minVolume, maxVolume]
            musicList[currentPlayingIndex].volume = Mathf.Clamp(currentVolume, minVolume, maxVolume);
        }
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
            // if (GlobalAmbientControl.ambientControl.isFogOn) {
            //     currentVolume = currentVolume / 2;
            // }
            if (isBackgroundPlaying) {
                musicList[currentPlayingIndex].Play();
                // musicList[currentPlayingIndex].volume = currentVolume;
                
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

    public void ToggleFogVolume() {
        if (GlobalAmbientControl.ambientControl.isFogOn) {
            fogVolumeFactor = 0.5f;
            
        } else {
            fogVolumeFactor = 1f;
        }
        // musicList[currentPlayingIndex].volume = currentVolume * fogVolumeFactor;
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