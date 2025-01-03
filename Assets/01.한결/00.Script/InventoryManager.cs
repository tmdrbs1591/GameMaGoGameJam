using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();  // 인벤토리 아이템 리스트

    public Transform ItemContent;  // 아이템 UI를 배치할 부모 객체
    public GameObject InventoryItem;  // 아이템 UI 프리팹

    public Toggle EnableRemove;  // 아이템 삭제 여부를 조정하는 Toggle

    private void Awake()
    {
        Instance = this;
    }

    // 아이템을 추가합니다.
    public void Add(Item item)
    {
        // 기존 아이템을 ID로 찾아서 수량을 증가시킵니다.
        var existingItem = Items.Find(i => i.id == item.id);

        if (existingItem != null)
        {
            // 기존 아이템이 있다면 수량만 증가
            existingItem.quantity += 1;
        }
        else
        {
            // 새로운 아이템 추가 (아이템의 id, 이름, 값 등을 복사)
            var newItem = ScriptableObject.CreateInstance<Item>();
            newItem.id = item.id;
            newItem.itemName = item.itemName;
            newItem.value = item.value;
            newItem.icon = item.icon;
            newItem.itemType = item.itemType;
            newItem.quantity = 1;

            Items.Add(newItem);
        }

        // UI 갱신
        ListItems();
    }

    // 아이템을 제거합니다.
    public void Remove(Item item)
    {
        // 해당 아이템을 ID로 찾고, 수량을 감소시킵니다.
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

        // UI 갱신
        ListItems();
    }

    // 인벤토리 UI를 갱신합니다.
    public void ListItems()
    {
        // 기존 UI 요소들을 삭제합니다.
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        // 아이템을 UI에 새로 추가합니다.
        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var removeButton = obj.transform.Find("RemoveButton").GetComponent<Button>();

            itemName.text = $"{item.itemName} x{item.quantity}";
            itemIcon.sprite = item.icon;

            // 삭제 버튼의 활성화 여부를 설정
            removeButton.gameObject.SetActive(EnableRemove.isOn);

            // 아이템 클릭 시 `InventoryItemController`에 아이템 설정
            var itemController = obj.GetComponent<InventoryItemController>();
            itemController.Add(item);
        }
    }

    // 아이템 삭제 기능 활성화/비활성화
    public void EnableItemsRemove()
    {
        foreach (Transform item in ItemContent)
        {
            item.Find("RemoveButton").gameObject.SetActive(EnableRemove.isOn);
        }
    }
}
