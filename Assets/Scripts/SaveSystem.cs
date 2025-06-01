using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class GameSessionData {
    public int score;
    public string timestamp;
}

public class GameData {
    public int currentHighScore;
    public string currentTimestamp;
    public string currentLevelName;
    public List<GameSessionData> currentLevel;
    public List<GameSessionData> frogger;
    public List<GameSessionData> car;
}

public class SaveSystem {
    private static string filePath = Path.Combine(Application.persistentDataPath, "saveData.json");

    public static void Save(GameData data) {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
    }

    public static GameData Load() {
        if (File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<GameData>(json);
        }
        var gd = new GameData();
        gd.frogger = new List<GameSessionData>();
        gd.car = new List<GameSessionData>();
        return gd; // Retorna dados padrão se não existir
    }
}