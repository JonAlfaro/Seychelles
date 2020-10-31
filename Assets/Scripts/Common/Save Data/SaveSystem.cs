using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string SaveFilePath = Path.Combine(Application.persistentDataPath, Constants.SaveFileName);
    
    public static void SaveGame(GameData gameData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        
        using (FileStream stream = new FileStream(SaveFilePath, FileMode.Create))
        {
            formatter.Serialize(stream, gameData);
        }
    }

    public static GameData LoadGame()
    {
        if (File.Exists(SaveFilePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            
            using (FileStream stream = new FileStream(SaveFilePath, FileMode.Open))
            {
                if (stream.Length == 0)
                {
                    return new GameData();
                }
                
                GameData data = formatter.Deserialize(stream) as GameData;

                return data;
            }
        }
        
        return new GameData();
    }
}
