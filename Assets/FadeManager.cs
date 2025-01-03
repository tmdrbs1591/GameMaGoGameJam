using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    [SerializeField] GameObject FadeIn;
    [SerializeField] GameObject FadeOut;
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        FadeOut.SetActive(false);
        FadeOut.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void FadeInActive()
    {
        FadeIn.SetActive(false);
        FadeIn.SetActive(true);
    }
}
