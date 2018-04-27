using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using XInputDotNetPure;

public class PlayerInfoSingleton : MonoBehaviour
{
    private static PlayerInfoSingleton m_instance;

    public static PlayerInfoSingleton instance
    {
        get
        {
            // If an instance of this script doesn't exist already
            if (m_instance == null)
            {
                // Creates a GameObject based of this class
                //GameObject prefab = (GameObject)Resources.Load("PlayerInfoSingleton");
                GameObject prefab = new GameObject();
                prefab.AddComponent<PlayerInfoSingleton>();
                // Instantiates the created GameObject
                GameObject created = Instantiate(prefab);
                created.name = "PlayerInfoSingleton";
                // Prevents the object from being destroyed when changing scenes
                DontDestroyOnLoad(created);
                // Assigns an instance of this script
                m_instance = created.GetComponent<PlayerInfoSingleton>();
            }

            return m_instance;
        }
    }

    public bool debug = true;
	public int playerAmount = 0;
    public List<PlayerIndex> playerIndexes;
    public Color[] colors = new Color[4];
    public int[] characterIndex = new int[4];
    public string selectedLevel = "RandomGeneratedLevelWithPrefabs";

    // Multiplayer
    public enum Difficulties { VeryEasy, Easy, Medium, Hard, VeryHard, Mix }
    public enum Lengths { Short = 1,  Medium, Long, Humongous, Gigantic }
    public Difficulties levelDifficulty = Difficulties.Mix;
    public Lengths levelLength = Lengths.Medium;

    // Singleplayer
    public List<SP_LevelStats> levelStats_ice = new List<SP_LevelStats>();
    public List<SP_LevelStats> levelStats_volcano = new List<SP_LevelStats>();
    public List<SP_LevelStats> levelStats_woods = new List<SP_LevelStats>();


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
        }
    }


    public bool SavefileExists()
    {
        return File.Exists(Application.dataPath + "/savefile.dat");
    }


    [System.Serializable]
    class Data
    {
        public List<SP_LevelStats> levelStats_ice = new List<SP_LevelStats>();
        public List<SP_LevelStats> levelStats_volcano = new List<SP_LevelStats>();
        public List<SP_LevelStats> levelStats_woods = new List<SP_LevelStats>();
        public string test;
    }
}
