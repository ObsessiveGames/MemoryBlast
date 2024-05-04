using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData {
    [field: SerializeField] public int matchScore { get; private set; }
    [field: SerializeField] public List<int> gameSaveIndex { get; private set; } = new List<int>();

    public void SetMatchScore(int amount) => matchScore = amount;
    public void AddIndexToGameSaveList(int randomIndex) => gameSaveIndex.Add(randomIndex);
}