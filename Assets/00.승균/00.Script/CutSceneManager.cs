using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    public static CutSceneManager instance;

    public bool isCutScene;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }


    public void CutSceneStart()
    {
        isCutScene = true;
    }

    public void CutSceneEnd()
    {
        isCutScene = false;
    }
}
