using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCam : MonoBehaviour
{

    Ray ray;
    RaycastHit hit;

    Camera cam;

    private void Start()
    {
        cam = transform.gameObject.GetComponent<Camera>();
        
    }
    private void Update()
    {
    }
 
   
    void DeformMesh()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) { 
        
            DeformPlane deforPlane = hit.transform.GetComponent<DeformPlane>();

            deforPlane.DeformThisPlane(hit.point);
        }
    }
}
