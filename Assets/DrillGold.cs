using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrillGold : MonoBehaviour
{
    public int gold;
    public int level = 1; // ���� ����
    public float maxExp = 10; // ���� �ʿ� ����ġ
    public float currentExp;

    [SerializeField] TMP_Text goldText;
    [SerializeField] TMP_Text levelText; // ���� ǥ�ÿ� �ؽ�Ʈ

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
            GainExp(1); // ������ ȹ�� �� ����ġ ���� (��: 5)
        }
        if (other.gameObject.CompareTag("Crystal"))
        {
            gold += 10;
        }
    }
        private void GainExp(float amount)
    {
        currentExp += amount;

        // ������ ���� �˻�
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
        maxExp += 10; // ������ �ʿ� ����ġ 10 ����
        Debug.Log("������! ���� ����: " + level);

        drill.damageAmount += 0.5f;
    }

    private void UpdateUI()
    {
        goldText.text = "DM : " + gold.ToString();
        levelText.text = "LV. " + level.ToString();
    }
}
