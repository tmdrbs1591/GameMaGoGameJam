using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawnBox : MonoBehaviour
{
    [SerializeField] Transform mapSpawnPos;
    [SerializeField] GameObject[] map;
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
            Instantiate(
     map[0],
     mapSpawnPos.transform.position,
     transform.rotation * Quaternion.Euler(-0.06f, 2.128f, 0.65f)
 );

        }
    }
}
