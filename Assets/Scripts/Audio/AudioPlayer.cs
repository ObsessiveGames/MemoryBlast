using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour {
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioItem[] audioArray;
    // create a list of SFX items and then use the string passed in to find the List item to play it one shot like below.

    private float musicVolume = 1f;
    private float sfxVolume = 1f;

    private void Awake() {
        GlobalAudioPlayer.audioPlayer = this;
    }

    public void PlaySFX(string sfxName) {
        foreach (AudioItem item in audioArray) {
            if (item.name == sfxName) {
                int random = Random.Range(0, item.clip.Length);
                audioSource.volume = item.volume * sfxVolume;
                audioSource.PlayOneShot(item.clip[random]);
            }
        }
    }

    public void PlayMusic(string musicName) {
        // Create a separate GameObject designated for playing music.
        // This can be changed if we do need reference of this music player as the scene will always only need 1 music.
        GameObject music = new GameObject("Music");
        AudioSource source = music.AddComponent<AudioSource>();

        foreach (AudioItem item in audioArray) {
            if (item.name == musicName) {
                source.clip = item.clip[0];
                source.loop = true;
                source.volume = item.volume * musicVolume;
                source.Play();
            }
        }
    }
}