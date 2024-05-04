using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioItem {
    public string name;
    public float volume = 1f;
    public AudioClip[] clip;
}