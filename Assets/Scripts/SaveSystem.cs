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
    public string currentLevel;
    public List<GameSessionData> frogger { get; set; }
    public List<GameSessionData> car{ get; set; }
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
        return new GameData(); // Retorna dados padrão se não existir
    }
}