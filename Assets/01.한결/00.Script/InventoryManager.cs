using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();  // �κ��丮 ������ ����Ʈ

    public Transform ItemContent;  // ������ UI�� ��ġ�� �θ� ��ü
    public GameObject InventoryItem;  // ������ UI ������

    public Toggle EnableRemove;  // ������ ���� ���θ� �����ϴ� Toggle

    public GameObject inVenPanel;

    bool isInven;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isInven)
        {
            inVenPanel.SetActive(true);
            isInven = true;
            ListItems();
        }
        else if (Input.GetKeyDown(KeyCode.E) && isInven)
        {
            isInven = false;
            inVenPanel.SetActive(false);

        }
    }

    public Item GetItemById(int id)
    {
        return Items.Find(item => item.id == id);  // �������� ã�� ��ȯ
    }

    public void CreateAndAddItem(int id, string name, int value, Sprite icon, string type)
    {
        // ���ڿ��� ���������� ��ȯ
        if (!System.Enum.TryParse(type, out Item.ItemType parsedType))
        {
            Debug.LogError($"�߸��� ������ Ÿ��: {type}");
            return;
        }

        // ���� ������ Ȯ��
        var existingItem = Items.Find(i => i.id == id);

        if (existingItem != null)
        {
            // ���� �������� ������ ���� ����
            existingItem.quantity += 1;
        }
        else
        {
            // ���ο� ������ ����
            var newItem = new Item
            {
                id = id,
                itemName = name,
                value = value,
                icon = icon,
                itemType = parsedType, // ��ȯ�� ������ Ÿ�� ���
                quantity = 1
            };

            // ������ ����Ʈ�� �߰�
            Items.Add(newItem);
        }

        // UI ����
        ListItems();
    }


    public bool SpendItem(int itemId, int quantity)
    {
        Item item = GetItemById(itemId); // ������ ã��
        if (item != null && item.quantity >= quantity) // ������ ���� Ȯ��
        {
            item.quantity -= quantity; // ���� ����

            // ������ 0�� �Ǹ� �������� ��Ͽ��� ����
            if (item.quantity <= 0)
            {
                Items.Remove(item); // ������ ����
            }

            // UI ��� ����
            ListItems();

            return true;
        }
        else
        {
            Debug.Log("�������� �����ϰų� �������� �ʽ��ϴ�.");
            return false;
        }
    }


    // �������� �߰��մϴ�.
    public void Add(Item item)
    {
        // ���� �������� ID�� ã�Ƽ� ������ ������ŵ�ϴ�.
        var existingItem = Items.Find(i => i.id == item.id);

        if (existingItem != null)
        {
            // ���� �������� �ִٸ� ������ ����
            existingItem.quantity += 1;
        }
        else
        {
            // ���ο� ������ �߰� (�������� id, �̸�, �� ���� �״�� ����)
            // ���⼭�� ScriptableObject.CreateInstance ������� �ʰ�, �ܼ��� �ν��Ͻ��� �߰��մϴ�.
            var newItem = new Item
            {
                id = item.id,
                itemName = item.itemName,
                value = item.value,
                icon = item.icon,
                itemType = item.itemType,
                quantity = 1
            };

            Items.Add(newItem);
        }

        // UI ����
        ListItems();
    }

    // �������� �����մϴ�.
    public void Remove(Item item)
    {
        // �ش� �������� ID�� ã��, ������ ���ҽ�ŵ�ϴ�.
        var existingItem = Items.Find(i => i.id == item.id);

        if (existingItem != null)
        {
            if (existingItem.quantity > 1)
            {
                existingItem.quantity -= 1;
            }
            else
            {
                Items.Remove(existingItem);
            }
        }

        // UI ����
        ListItems();
    }

    // �κ��丮 UI�� �����մϴ�.
    public void ListItems()
    {
        // ���� UI ��ҵ��� �����մϴ�.
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        // �������� UI�� ���� �߰��մϴ�.
        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var removeButton = obj.transform.Find("RemoveButton").GetComponent<Button>();

            itemName.text = $"{item.itemName} x{item.quantity}";
            itemIcon.sprite = item.icon;

            // ���� ��ư�� Ȱ��ȭ ���θ� ����
            removeButton.gameObject.SetActive(EnableRemove.isOn);

            // ������ Ŭ�� �� `InventoryItemController`�� ������ ����
            var itemController = obj.GetComponent<InventoryItemController>();
            itemController.Add(item);
        }
    }

    // ������ ���� ��� Ȱ��ȭ/��Ȱ��ȭ
    public void EnableItemsRemove()
    {
        foreach (Transform item in ItemContent)
        {
            item.Find("RemoveButton").gameObject.SetActive(EnableRemove.isOn);
        }
    }
}
