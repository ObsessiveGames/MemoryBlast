using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData {
    [field: SerializeField] public int matchScore { get; private set; }
    [field: SerializeField] public int matchTurn { get; private set; }
    [field: SerializeField] public List<int> gameSaveIndex { get; private set; } = new List<int>();
    [field: SerializeField] public List<int> matchedIndex { get; private set; } = new List<int>(); 

    public void SetMatchScore(int amount) => matchScore = amount;
    public void SetMatchTurn(int amount) => matchTurn = amount;
    public void AddIndexToGameSaveList(int randomIndex) => gameSaveIndex.Add(randomIndex);
    
    public void AddIndexToMatchedList(int value) {
        if (matchedIndex.Contains(value)) return;
        matchedIndex.Add(value);
    }

    public void ClearGameSaveIndexList() {
        gameSaveIndex.Clear();
        matchedIndex.Clear();
    }
}