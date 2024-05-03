using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : Manager {
    [field: SerializeField] public PlayerData data { get; private set; }

    [SerializeField] private float saveTimer = 15f;

    private float saveTimerInterval;
    private string fileName = "savefile.json";
    private string filePath;

    private void Awake() {
        saveTimerInterval = saveTimer;
    }

    public override void Setup(AppManager appManager) {
        base.Setup(appManager);
        filePath = Path.Combine(Application.persistentDataPath, fileName);
        data = SaveExists() ? data = LoadGame() : new PlayerData();
    }

    private void Update() {
        if (saveTimer > 0) {
            saveTimer -= Time.deltaTime;
        } else {
            saveTimer = saveTimerInterval;
            SaveGameAsync(data);
        }
    }

    public string SerializeSaveData(PlayerData data) => JsonUtility.ToJson(data);

    public async void SaveGameAsync(PlayerData data) {
        try {
            string json = SerializeSaveData(data);
            await File.WriteAllTextAsync(filePath, json);
        }
        catch (Exception ex) {
            Debug.LogError($"ERROR: SaveGameAsync didn't save. Exception: {ex}");
        }
    }

    public PlayerData LoadGame() {
        if (File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            return data;
        }
        return null;
    }

    private bool SaveExists() => File.Exists(filePath);
}