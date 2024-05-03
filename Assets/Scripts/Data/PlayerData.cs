using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData {
    [field: SerializeField] public int matchesScore { get; private set; }

    public void IncrementMatchesScore(int amount) => matchesScore += amount;
}