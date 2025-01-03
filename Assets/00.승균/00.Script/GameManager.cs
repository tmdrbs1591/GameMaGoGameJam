using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Drill drill;

    [SerializeField] TMP_Text text;

    public GameObject shopPanel;
    public GameObject clearPanel;
    public float score;

    private float lowestY; // 드릴의 최저 y값 저장

    public bool isDamageTrue;
    public GameObject hpLockPanel;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (drill != null)
        {
            // 초기 드릴의 y값을 최저값으로 설정
            lowestY = drill.transform.position.y;


        }
    }

  
 
    void Update()
    {

        if (drill != null)
        {
            float currentY = drill.transform.position.y;

            // 드릴이 최저 y값보다 더 낮아질 때만 점수 추가
            if (currentY < lowestY)
            {
                score += 0.2f;

                // 최저 y값 갱신
                lowestY = currentY;
            }
        }

        text.text = ((int)score).ToString() + "m";

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseShop();
        }

    }
    public void Stun()
    {
        StartCoroutine(TongueCor());
    }
    IEnumerator TongueCor()
    {
        isDamageTrue = true;
        yield return new WaitForSeconds(4f);
        isDamageTrue = false;
    }
    public void OpenShop()
    {
        shopPanel.SetActive(true);
        InventoryManager.Instance.InvenOpen();
        StartCoroutine(TimeScale());
        
    }
    public void CloseShop()
    {
        shopPanel.SetActive(false);
        Time.timeScale = 1;

    }
    IEnumerator TimeScale()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 0;
    }

    public void ReStart()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
