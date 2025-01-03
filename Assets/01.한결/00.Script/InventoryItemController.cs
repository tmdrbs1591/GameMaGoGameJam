using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    private Item item;
    public Button RemoveButton;

    // 이 메서드를 사용하여 아이템을 설정합니다.
    public void Add(Item newItem)
    {
        item = newItem;
    }

    // 아이템을 사용할 때 호출됩니다.
    public void UseItem()
    {
        if (item == null)
        {
            Debug.LogError("아이템이 할당되지 않았습니다.");
            return;
        }

        // 아이템 타입에 따라 적절한 사용 처리
        switch (item.itemType)
        {
            case Item.ItemType.Crystal:
                return;

            case Item.ItemType.Health_Potion:
                Player2.instance.IncreaseHealth(item.value);
                break;

            case Item.ItemType.Exp_Potion:
                Player2.instance.IncreaseExp(item.value);
                break;
        }

        // 사용 후 아이템 제거
        InventoryManager.Instance.Remove(item);
    }

    // 아이템을 제거할 때 호출됩니다.
    public void RemoveItem()
    {
        if (item == null)
        {
            Debug.LogError("아이템이 할당되지 않았습니다.");
            return;
        }

        InventoryManager.Instance.Remove(item); // 인벤토리에서 아이템 제거
        Destroy(gameObject); // UI에서 아이템 제거
    }
}
