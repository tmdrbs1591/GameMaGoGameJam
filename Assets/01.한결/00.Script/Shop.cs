using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject shopPanel;
    public Image[] upgradeImage;

    public Button drillPower;
    public Button coinGain;
    public Button drillDamage;
    public Button drillHealth;

    public TextMeshProUGUI CryStal;

    [SerializeField]
    private Vector3 detectionBoxSize = new Vector3(5f, 5f, 5f);
    [SerializeField]
    private bool isDrillDetected = false;

    [SerializeField]
    private string targetTag = "Drill";

    private InventoryManager inventoryManager;  // 인벤토리 매니저
    private Drill drill;
    public ToolTip toolTip;

    private int powerUpgradeCost = 5;
    private int coinGainUpgradeCost = 3;
    private int damageUpgradeCost = 7;
    private int healthUpgradeCost = 4;

    private void Start()
    {
        shopPanel.SetActive(false);
        inventoryManager = FindObjectOfType<InventoryManager>();  // InventoryManager 찾기
        drill = FindObjectOfType<Drill>();
        

        AddButtonListeners();
    }

    private void Update()
    {
        FindObjectWithTagInRange();
        ShopOpenClose();

        //// 크리스탈 수량 표시
        //CryStal.text = "Crystal: " + inventoryManager.GetItemById(1).quantity.ToString();
    }

    private void AddButtonListeners()
    {
        AddToolTipEvents(drillPower, "드릴 파워 강화", $"Cost: {powerUpgradeCost} Crystals");
        AddToolTipEvents(coinGain, "코인 휙득량 증가", $"Cost: {coinGainUpgradeCost} Crystals");
        AddToolTipEvents(drillDamage, "드릴 데미지 증가", $"Cost: {damageUpgradeCost} Crystals");
        AddToolTipEvents(drillHealth, "드릴 최대 체력 증가", $"Cost: {healthUpgradeCost} Crystals");
    }

    private void AddToolTipEvents(Button button, string title, string description)
    {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerEnter = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        pointerEnter.callback.AddListener((_) =>
        {
            toolTip.SetupToolTip(title, description);
            toolTip.Show();
        });

        EventTrigger.Entry pointerExit = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        pointerExit.callback.AddListener((_) => toolTip.Hide());

        trigger.triggers.Add(pointerEnter);
        trigger.triggers.Add(pointerExit);
    }

    private void ShopOpenClose()
    {
        if (isDrillDetected && Input.GetKeyDown(KeyCode.F))
        {
            shopPanel.SetActive(!shopPanel.activeSelf);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, detectionBoxSize);
    }

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
        // 크리스탈 아이템을 소비
        if (inventoryManager.SpendItem(1, powerUpgradeCost))  // 크리스탈 아이템 소비 (아이템ID=1, 수량=powerUpgradeCost)
        {
            powerUpgradeCost += 5;  // 업그레이드 비용 증가
            Debug.Log("드릴 파워 업");
        }
        else
        {
            Debug.Log("크리스탈이 부족합니다.");
        }
    }

    public void CoinGainUpgrade()
    {
        if (inventoryManager.SpendItem(1, coinGainUpgradeCost))  // 아이템ID=1은 크리스탈
        {
            coinGainUpgradeCost += 10;
            Debug.Log("코인 획득량 업");
        }
        else
        {
            Debug.Log("크리스탈이 부족합니다.");
        }
    }

    public void DamageUpgrade()
    {
        if (inventoryManager.SpendItem(1, damageUpgradeCost))  // 아이템ID=1은 크리스탈
        {
            damageUpgradeCost += 3;
            Debug.Log("드릴 공격력 업");
        }
        else
        {
            Debug.Log("크리스탈이 부족합니다.");
        }
    }

    public void HealthUpgrade()
    {
        if (inventoryManager.SpendItem(1, healthUpgradeCost))  // 아이템ID=1은 크리스탈
        {
            healthUpgradeCost += 1;
            Debug.Log("드릴 체력 업");
        }
        else
        {
            Debug.Log("크리스탈이 부족합니다.");
        }
    }
    #endregion
}
