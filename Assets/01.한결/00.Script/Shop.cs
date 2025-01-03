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

    private InventoryManager inventoryManager;  // �κ��丮 �Ŵ���
    private Drill drill;
    public ToolTip toolTip;

    private int powerUpgradeCost = 5;
    private int coinGainUpgradeCost = 3;
    private int damageUpgradeCost = 7;
    private int healthUpgradeCost = 4;

    private void Start()
    {
        shopPanel.SetActive(false);
        inventoryManager = FindObjectOfType<InventoryManager>();  // InventoryManager ã��
        drill = FindObjectOfType<Drill>();
        

        AddButtonListeners();
    }

    private void Update()
    {
        FindObjectWithTagInRange();
        ShopOpenClose();

        //// ũ����Ż ���� ǥ��
        //CryStal.text = "Crystal: " + inventoryManager.GetItemById(1).quantity.ToString();
    }

    private void AddButtonListeners()
    {
        AddToolTipEvents(drillPower, "�帱 �Ŀ� ��ȭ", $"Cost: {powerUpgradeCost} Crystals");
        AddToolTipEvents(coinGain, "���� �׵淮 ����", $"Cost: {coinGainUpgradeCost} Crystals");
        AddToolTipEvents(drillDamage, "�帱 ������ ����", $"Cost: {damageUpgradeCost} Crystals");
        AddToolTipEvents(drillHealth, "�帱 �ִ� ü�� ����", $"Cost: {healthUpgradeCost} Crystals");
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

    #region ���׷��̵�
    public void PowerUpgrade()
    {
        // ũ����Ż �������� �Һ�
        if (inventoryManager.SpendItem(1, powerUpgradeCost))  // ũ����Ż ������ �Һ� (������ID=1, ����=powerUpgradeCost)
        {
            powerUpgradeCost += 5;  // ���׷��̵� ��� ����
            Debug.Log("�帱 �Ŀ� ��");
        }
        else
        {
            Debug.Log("ũ����Ż�� �����մϴ�.");
        }
    }

    public void CoinGainUpgrade()
    {
        if (inventoryManager.SpendItem(1, coinGainUpgradeCost))  // ������ID=1�� ũ����Ż
        {
            coinGainUpgradeCost += 10;
            Debug.Log("���� ȹ�淮 ��");
        }
        else
        {
            Debug.Log("ũ����Ż�� �����մϴ�.");
        }
    }

    public void DamageUpgrade()
    {
        if (inventoryManager.SpendItem(1, damageUpgradeCost))  // ������ID=1�� ũ����Ż
        {
            damageUpgradeCost += 3;
            Debug.Log("�帱 ���ݷ� ��");
        }
        else
        {
            Debug.Log("ũ����Ż�� �����մϴ�.");
        }
    }

    public void HealthUpgrade()
    {
        if (inventoryManager.SpendItem(1, healthUpgradeCost))  // ������ID=1�� ũ����Ż
        {
            healthUpgradeCost += 1;
            Debug.Log("�帱 ü�� ��");
        }
        else
        {
            Debug.Log("ũ����Ż�� �����մϴ�.");
        }
    }
    #endregion
}
