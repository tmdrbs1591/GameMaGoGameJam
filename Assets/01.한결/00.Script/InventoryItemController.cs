using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    private Item item;
    public Button RemoveButton;

    // �� �޼��带 ����Ͽ� �������� �����մϴ�.
    public void Add(Item newItem)
    {
        item = newItem;
    }

    // �������� ����� �� ȣ��˴ϴ�.
    public void UseItem()
    {
        if (item == null)
        {
            Debug.LogError("�������� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        // ������ Ÿ�Կ� ���� ������ ��� ó��
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

        // ��� �� ������ ����
        InventoryManager.Instance.Remove(item);
    }

    // �������� ������ �� ȣ��˴ϴ�.
    public void RemoveItem()
    {
        if (item == null)
        {
            Debug.LogError("�������� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        InventoryManager.Instance.Remove(item); // �κ��丮���� ������ ����
        Destroy(gameObject); // UI���� ������ ����
    }
}
