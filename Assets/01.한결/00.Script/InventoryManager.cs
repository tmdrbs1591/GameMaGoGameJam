using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();  // �κ��丮 ������ ����Ʈ

    public Transform ItemContent;  // ������ UI�� ��ġ�� �θ� ��ü
    public GameObject InventoryItem;  // ������ UI ������

    public Toggle EnableRemove;  // ������ ���� ���θ� �����ϴ� Toggle

    private void Awake()
    {
        Instance = this;
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
            // ���ο� ������ �߰� (�������� id, �̸�, �� ���� ����)
            var newItem = ScriptableObject.CreateInstance<Item>();
            newItem.id = item.id;
            newItem.itemName = item.itemName;
            newItem.value = item.value;
            newItem.icon = item.icon;
            newItem.itemType = item.itemType;
            newItem.quantity = 1;

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
