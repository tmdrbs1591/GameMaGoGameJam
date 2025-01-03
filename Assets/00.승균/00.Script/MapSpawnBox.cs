using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawnBox : MonoBehaviour
{
    [SerializeField] Transform mapSpawnPos;
    [SerializeField] GameObject[] map;

    [SerializeField] GameObject nextMap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Drill"))
        {
     
            MapManager.instance.currentMap++;

            if(MapManager.instance.currentMap % 5 == 0)
            {
                var nextMaps = Instantiate(nextMap, mapSpawnPos.transform.position, Quaternion.identity);
                MapManager.instance.maps.Add(nextMaps);
            }
            else
            {
                var maps = Instantiate(map[0], mapSpawnPos.transform.position, Quaternion.identity);
                MapManager.instance.maps.Add(maps);

            }

            Destroy(gameObject);

        }
    }
}
