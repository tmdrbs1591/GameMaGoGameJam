using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawnBox : MonoBehaviour
{
    [SerializeField] Transform mapSpawnPos;
    [SerializeField] GameObject[] map;

    [SerializeField] GameObject nextMap;

    [SerializeField] Transform shopSpawnPos;
    [SerializeField] GameObject shopPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        // 4歳税 1 溌懸稽 雌繊 持失
        if (Random.value <= 1f)
        {
            Instantiate(shopPrefabs, shopSpawnPos.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnCollisionEnter(Collision collision)
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Drill"))
        {
            MapManager.instance.currentMap++;

            if (MapManager.instance.currentMap % 5 == 0)
            {
                var nextMaps = Instantiate(nextMap, mapSpawnPos.transform.position, Quaternion.identity);

                Debug.Log("けいし");
                MapManager.instance.maps.Add(nextMaps);
            }
            else
            {
                var maps = Instantiate(map[0], mapSpawnPos.transform.position, Quaternion.identity);
                Debug.Log("けいしいい");

                MapManager.instance.maps.Add(maps);
            }

            Destroy(gameObject);
        }
    }
}
