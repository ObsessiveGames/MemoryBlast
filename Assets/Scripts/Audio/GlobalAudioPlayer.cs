using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalAudioPlayer {
    public static AudioPlayer audioPlayer;

    public static void PlaySFX(string sfxName) {
        if (audioPlayer != null && sfxName != "") audioPlayer.PlaySFX(sfxName);
    }

    public static void PlayMusic(string musicName) {
        if (audioPlayer != null && musicName != "") audioPlayer.PlayMusic(musicName);
    }
}