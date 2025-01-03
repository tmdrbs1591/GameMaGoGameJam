using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    public List<GameObject> maps;

    public int currentMap;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if there are more than 4 maps in the list
        if (maps.Count > 4)
        {
            // Get the oldest map (index 0)
            GameObject oldestMap = maps[0];

            // Remove it from the list
            maps.RemoveAt(0);

            // Destroy the GameObject
            if (oldestMap != null)
            {
                Destroy(oldestMap);
            }
        }
    }
}
