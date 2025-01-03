using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    private Item item;
    public Button RemoveButton;
    private Drill drill;

    private void Awake()
    {
        drill = FindObjectOfType<Drill>();
    }

   

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

            case Item.ItemType.Ruby:
                return;

            case Item.ItemType.Health_Potion:
                drill.currentHP += item.value;
                break;

            case Item.ItemType.Speed_Potion:
                drill.moveSpeed += item.value;
                break;
            case Item.ItemType.Damage_Potion:
                drill.damageAmount += item.value;
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
