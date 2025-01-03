using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Drill drill;

    [SerializeField] TMP_Text text;

    public GameObject shopPanel;
    public float score;

    private float lowestY; // 드릴의 최저 y값 저장

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
                Debug.Log("Score: " + score);

                // 최저 y값 갱신
                lowestY = currentY;
            }
        }

        text.text = ((int)score).ToString() + "m";

    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        StartCoroutine(TimeScale());
    }

    IEnumerator TimeScale()
    {
        yield return new WaitForSecondsRealtime(0.6f);
        Time.timeScale = 0;
    }
}
