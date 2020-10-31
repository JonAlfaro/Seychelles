using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    private static GameDataManager instance;
    public static GameDataManager Instance => instance;
    public GameData GameData { get; private set; }

    public void Save()
    {
        SaveSystem.SaveGame(GameData);
    }

    void Awake()
    {
        // Handle singleton instantiation
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        GameData = SaveSystem.LoadGame();
    }
}