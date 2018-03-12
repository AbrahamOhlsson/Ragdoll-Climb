using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public List<PlayerIndex> playerIndexes;
    public Color[] colors = new Color[4];
}
