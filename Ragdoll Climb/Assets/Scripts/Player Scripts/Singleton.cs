using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using XInputDotNetPure;

public class Singleton : MonoBehaviour
{
    private static Singleton m_instance;

    public static Singleton instance
    {
        get
        {
            // If an instance of this script doesn't exist already
            if (m_instance == null)
            {
                // Creates a GameObject based of this class
                GameObject prefab = new GameObject();
                prefab.AddComponent<Singleton>();
                // Instantiates the created GameObject
                GameObject created = Instantiate(prefab);
                created.name = "Singleton";
                // Prevents the object from being destroyed when changing scenes
                DontDestroyOnLoad(created);
                // Assigns an instance of this script
                m_instance = created.GetComponent<Singleton>();
            }

            return m_instance;
        }
    }

    public bool debug = true;
	public int playerAmount = 0;
    public List<PlayerIndex> playerIndexes;
    public Color[] colors = new Color[4];
    public int[] colorindex = new int[4];
    public int[] characterIndex = new int[4];
    public string selectedLevel = "RandomGeneratedLevelWithPrefabs";
    public enum Modes { None, Single, Multi }
    public Modes mode = Modes.None;

    // Multiplayer
    public enum Difficulties { VeryEasy, Easy, Medium, Hard, VeryHard, Mix }
    public enum Lengths { Short = 1,  Medium, Long, Humongous, Gigantic }
    public Difficulties levelDifficulty = Difficulties.Mix;
    public Lengths levelLength = Lengths.Medium;

    // Singleplayer
    public int currSpLevelIndex = 0;
    public string currSpWorld = "NULL";
    public SP_LevelStats currLevelStats;
    public List<SP_LevelStats> levelStats_ice = new List<SP_LevelStats>();
    public List<SP_LevelStats> levelStats_volcano = new List<SP_LevelStats>();
    public List<SP_LevelStats> levelStats_woods = new List<SP_LevelStats>();
    public List<SP_LevelStats> levelStats_metal = new List<SP_LevelStats>();

    // OPTIONS
    public bool fullscreen = true;
    public int qualityIndex = 0;
    public int resIndex = 0;
    public float masterVol = 1f;
    public float sfxVol = 1f;
    public float musicVol = 1f;


    public void Save()
    {
        // Binary formatter that will serialize the Data class
        BinaryFormatter bf = new BinaryFormatter();

        // The file that the data will be stored in
        FileStream file = File.Create(Application.dataPath + "/savefile.dat");

        // Creates an instance of PlayerData
        Data data = new Data();

        // Assigns all variables in Data to its correspondants values in this singleton
        data.levelStats_ice = levelStats_ice;
        data.levelStats_volcano = levelStats_volcano;
        data.levelStats_woods = levelStats_woods;
        data.levelStats_metal = levelStats_metal;

        // Stores the data in the file
        bf.Serialize(file, data);

        // Closes file
        file.Close();
    }


    public void Load()
    {
        // Checks if save file exists
        if (File.Exists(Application.dataPath + "/savefile.dat"))
        {
            // Binary formatter that will deserialize the save file
            BinaryFormatter bf = new BinaryFormatter();

            // An instance of the save file
            FileStream file = File.Open(Application.dataPath + "/savefile.dat", FileMode.Open);

            // An instance of the PlayerData class that is initialized with the savefiles data
            Data data = (Data)bf.Deserialize(file);

            // Closes file
            file.Close();

            // Assigns all variables to be saved/loaded in this singleton to its correspondant values in Data
            levelStats_ice = data.levelStats_ice;
            levelStats_volcano = data.levelStats_volcano;
            levelStats_woods = data.levelStats_woods;
            levelStats_metal = data.levelStats_metal;
        }
    }


    public void SaveOptions()
    {
        Options options = new Options
        {
            qualityIndex = qualityIndex,
            resIndex = resIndex,
            fullscreen = fullscreen,
            masterVol = masterVol,
            sfxVol = sfxVol,
            musicVol = musicVol
            
        };

        string jsonData = JsonUtility.ToJson(options, true);
        File.WriteAllText(Application.dataPath + "/options.json", jsonData);
    }


    public void LoadOptions()
    {
        if (File.Exists(Application.dataPath + "/options.json"))
        {
            Options options = new Options();
            options = JsonUtility.FromJson<Options>(File.ReadAllText(Application.dataPath + "/options.json"));

            qualityIndex = options.qualityIndex;
            resIndex = options.resIndex;
            fullscreen = options.fullscreen;
            masterVol = options.masterVol;
            sfxVol = options.sfxVol;
            musicVol = options.musicVol;
        }
    }


    public bool SavefileExists()
    {
        return File.Exists(Application.dataPath + "/savefile.dat");
    }


    public bool OptionsFileExists()
    {
        return File.Exists(Application.dataPath + "/options.json");
    }


    [System.Serializable]
    class Data
    {
        public List<SP_LevelStats> levelStats_ice = new List<SP_LevelStats>();
        public List<SP_LevelStats> levelStats_volcano = new List<SP_LevelStats>();
        public List<SP_LevelStats> levelStats_woods = new List<SP_LevelStats>();
        public List<SP_LevelStats> levelStats_metal = new List<SP_LevelStats>();
    }


    [System.Serializable]
    struct Options
    {
        public bool fullscreen;
        public int qualityIndex;
        public int resIndex;
        public float masterVol;
        public float sfxVol;
        public float musicVol;
    }
}
