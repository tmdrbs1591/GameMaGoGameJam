using System.Collections;
using System.Collections.Generic;using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalSceneManager : MonoBehaviour
{
    public GameObject SettingWindow;
    
    public FadeManager fadeManager;
    //메인
    public void OnClickStart()
    {
        StartCoroutine(SceneCor());
    }

    IEnumerator SceneCor()
    {
        fadeManager.FadeInActive();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Scenes/SampleScene");
    }
    
    //설정 창
    public void OnclickSetting()
    {
        SettingWindow.SetActive(true);
    }
    
    //설정 창 닫기
    public void OnclickClose()
    {
        SettingWindow.SetActive(false);
    }
    
    //게임 종료
    public void OnclickQuit()
    {
        Application.Quit();
    }
}