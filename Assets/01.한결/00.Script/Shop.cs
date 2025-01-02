using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    // 현재 돈 시스템이 없어서 돈 시스템이 생기면 돈을 지불하고 업그레이드로 변경
    public GameObject shopPanel;    // 상점 창

    public Image[] upgradeImage;    // 업그레이드 이미지

    public Button drillPower;   // Power는 드릴이 부술 수 있는 힘이 커지는 것
    public Button coinGain;     // 골드 획득량 증가
    public Button drillDamage;  // Damage는 몬스터를 죽을 때 올라가는 데미지
    public Button drillHealth;  // 드릴 체력 증가

    public TextMeshProUGUI Coin;

    [SerializeField]
    private Vector3 detectionBoxSize = new Vector3(5f, 5f, 5f); // 탐지 박스 크기
    [SerializeField]
    private bool isDrillDetected = false;  // 드릴이 감지되었나 확인하는 변수

    [SerializeField]
    private string targetTag = "Drill";    // 타겟의 태그

    private DrillGold drillGold;
    private Drill drill;

    // 강화 비용 변수
    private int powerUpgradeCost = 50;
    private int coinGainUpgradeCost = 30;
    private int damageUpgradeCost = 70;
    private int healthUpgradeCost = 40;

    private void Start()
    {
        shopPanel.SetActive(false);
        drillGold = FindObjectOfType<DrillGold>();
        drill = FindObjectOfType<Drill>();
    }

    private void Update()
    {
        FindObjectWithTagInRange();
        ShopOpenClose();

        Coin.text = "DM : " + drillGold.gold.ToString();
    }

    // 상점 창을 닫고 끄게 해주는 함수
    private void ShopOpenClose()
    {
        if (isDrillDetected && Input.GetKeyDown(KeyCode.F))
        {
            shopPanel.SetActive(!shopPanel.activeSelf);
        }
    }

    // 탐지 범위를 그려주는 기즈모
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, detectionBoxSize);
    }

    // 주변에 있는 태그를 탐지하는 함수
    private void FindObjectWithTagInRange()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, detectionBoxSize / 2, Quaternion.identity);
        isDrillDetected = false;

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(targetTag))
            {
                isDrillDetected = true;
                break;
            }
        }
    }

    #region 업그레이드
    public void PowerUpgrade()
    {
        if (drillGold.gold >= powerUpgradeCost)
        {
            drillGold.gold -= powerUpgradeCost; // 골드 차감
            powerUpgradeCost += 20; // 강화 비용 증가
            Debug.Log("드릴 파워 업");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void CoinGainUpgrade()
    {
        if (drillGold.gold >= coinGainUpgradeCost)
        {
            drillGold.gold -= coinGainUpgradeCost; // 골드 차감
            coinGainUpgradeCost += 10; // 강화 비용 증가
            Debug.Log("코인 획득량 업");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void DamageUpgrade()
    {
        if (drillGold.gold >= damageUpgradeCost)
        {
            drillGold.gold -= damageUpgradeCost; // 골드 차감
            damageUpgradeCost += 30; // 강화 비용 증가
            Debug.Log("드릴 공격력 업");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    public void HealthUpgrade()
    {
        if (drillGold.gold >= healthUpgradeCost)
        {
            drillGold.gold -= healthUpgradeCost; // 골드 차감
            healthUpgradeCost += 15; // 강화 비용 증가
            Debug.Log("드릴 체력 업");
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }
    #endregion
}
