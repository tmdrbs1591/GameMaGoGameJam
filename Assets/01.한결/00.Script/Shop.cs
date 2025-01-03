using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject shopPanel;
    public Image[] upgradeImage;

    public Button hpPotion;
    public Button speedPotion;
    public Button damagePotion;
    public Button hpUpgrade;
    public Button speedUpgrade;
    public Button damageUpgrade;

    [SerializeField]
    private Vector3 detectionBoxSize = new Vector3(5f, 5f, 5f);
    [SerializeField]
    private bool isDrillDetected = false;

    [SerializeField]
    private string targetTag = "Drill";

    private InventoryManager inventoryManager;  // �κ��丮 �Ŵ���
    private Drill drill;
    public ToolTip toolTip;

    private int hpPotionCost = 2;
    private int speedPotionCost = 3;
    private int damagePotionCost = 3;
    private int hpUpgradeCost = 5;
    private int speedUpgradeCost = 7;
    private int damageUpgradeCost = 10;

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
        AddToolTipEvents(hpPotion, "ü�� ����", $"Cost: {hpPotionCost} Crystals");
        AddToolTipEvents(speedPotion, "���ǵ� ����", $"Cost: {speedPotionCost} Crystals");
        AddToolTipEvents(damagePotion, "���ݷ� ����", $"Cost: {damagePotionCost} Crystals");
        AddToolTipEvents(hpUpgrade, "�帱 �ִ� ü�� ����", $"Cost: {hpUpgradeCost} Crystals");
        AddToolTipEvents(speedUpgrade, "�帱 �ִ� �ӵ� ����", $"Cost: {speedUpgradeCost} Crystals");
        AddToolTipEvents(damageUpgrade, "�帱 �ִ� ���ݷ� ����", $"Cost: {damageUpgradeCost} Crystals");
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
    public void HpPotionBuy()
    {
        // ũ����Ż �������� �Һ�
        if (inventoryManager.SpendItem(1, hpPotionCost))  // ũ����Ż ������ �Һ� (������ID=1, ����=powerUpgradeCost)
        {
            Item hpPotion = new Item
            {
                id = 3,
                itemName = "HealthPotion",
                value = 20,
                quantity = 1,
                itemType = Item.ItemType.Health_Potion
            };
            inventoryManager.Add(hpPotion);
            Debug.Log("ü�� ���� ����");

        }
        else
        {
            Debug.Log("ũ����Ż�� �����մϴ�.");
        }
    }

    public void SpeedPotionBuy()
    {
        if (inventoryManager.SpendItem(1, speedPotionCost))  // ������ID=1�� ũ����Ż
        {
            Item speedPotion = new Item
            {
                id = 2,
                itemName = "SpeedPotion",
                value = 3,
                quantity = 1,
                itemType = Item.ItemType.Speed_Potion
            };
            inventoryManager.Add(speedPotion);
            Debug.Log("���ǵ� ���� ����");
        }
        else
        {
            Debug.Log("ũ����Ż�� �����մϴ�.");
        }
    }

    public void DamagePotionBuy()
    {
        if (inventoryManager.SpendItem(1, damagePotionCost))  // ������ID=1�� ũ����Ż
        {
            Item damagePotion = new Item
            {
                id = 4,
                itemName = "DamagePotion",
                value = 1,
                quantity = 1,
                itemType = Item.ItemType.Damage_Potion
            };
            inventoryManager.Add(damagePotion);
            Debug.Log("���ݷ� ���� ����");
        }
        else
        {
            Debug.Log("ũ����Ż�� �����մϴ�.");
        }
    }

    public void HpUpgrade()
    {
        if (inventoryManager.SpendItem(1, hpUpgradeCost))  // ������ID=1�� ũ����Ż
        {
            hpUpgradeCost += 2;
            Debug.Log("�帱 �ִ� ü�� ��");
        }
        else
        {
            Debug.Log("ũ����Ż�� �����մϴ�.");
        }
    }

    public void SpeedUpgrade()
    {
        if (inventoryManager.SpendItem(1, speedUpgradeCost))  // ������ID=1�� ũ����Ż
        {
            speedUpgradeCost += 3;
            Debug.Log("�帱 �ִ� �ӵ� ��");
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
            damageUpgradeCost += 5;
            Debug.Log("�帱 �ִ� ���ݷ� ��");
        }
        else
        {
            Debug.Log("ũ����Ż�� �����մϴ�.");
        }
    }
    #endregion
}
