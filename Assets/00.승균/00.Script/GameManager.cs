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

    private float lowestY; // �帱�� ���� y�� ����

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (drill != null)
        {
            // �ʱ� �帱�� y���� ���������� ����
            lowestY = drill.transform.position.y;
        }
    }

    void Update()
    {

        if (drill != null)
        {
            float currentY = drill.transform.position.y;

            // �帱�� ���� y������ �� ������ ���� ���� �߰�
            if (currentY < lowestY)
            {
                score += 0.2f;

                // ���� y�� ����
                lowestY = currentY;
            }
        }

        text.text = ((int)score).ToString() + "m";

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseShop();
        }

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
}
