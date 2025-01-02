using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrillGold : MonoBehaviour
{
    public int gold;
    public int level = 1; // 시작 레벨
    public float maxExp = 10; // 최초 필요 경험치
    public float currentExp;

    [SerializeField] TMP_Text goldText;
    [SerializeField] TMP_Text levelText; // 레벨 표시용 텍스트

    [SerializeField] Slider expSlider;

    public Drill drill;
    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        goldText.text = "DM : " + gold.ToString();
        levelText.text = "LV." + level.ToString();
        expSlider.value = currentExp / maxExp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goods"))
        {
            GainExp(1); // 아이템 획득 시 경험치 증가 (예: 5)
        }
        if (other.gameObject.CompareTag("Crystal"))
        {
            gold += 10;
        }
    }
        private void GainExp(float amount)
    {
        currentExp += amount;

        // 레벨업 조건 검사
        while (currentExp >= maxExp)
        {
            currentExp -= maxExp;
            LevelUp();
        }

        UpdateUI();
    }

    private void LevelUp()
    {
        level++;
        maxExp += 10; // 레벨당 필요 경험치 10 증가
        Debug.Log("레벨업! 현재 레벨: " + level);

        drill.damageAmount += 0.5f;
    }

    private void UpdateUI()
    {
        goldText.text = "DM : " + gold.ToString();
        levelText.text = "LV. " + level.ToString();
    }
}
