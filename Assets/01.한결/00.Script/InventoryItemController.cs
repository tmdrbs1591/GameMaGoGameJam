using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class InventoryItemController : MonoBehaviour
{
    private Item item;
    public Button RemoveButton;
    private Drill drill;

    private float originHealth;
    private float originSpeed;
    private float originDamage;


    private void Awake()
    {
        drill = FindObjectOfType<Drill>();

        originDamage = drill.damageAmount;
        originHealth = drill.currentHP;
        originSpeed = drill.moveSpeed;
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
                StartCoroutine(IncreaseHealth());
                break;

            case Item.ItemType.Speed_Potion:
                StartCoroutine(IncreaseSpeed());
                break;
            case Item.ItemType.Damage_Potion:
                StartCoroutine(IncreaseDamage());
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

    private IEnumerator IncreaseHealth()
    {
        drill.currentHP += item.value;
        yield return null;
    }

    private IEnumerator IncreaseSpeed()
    {
        drill.moveSpeed += item.value;
        yield return new WaitForSeconds(3);
        drill.moveSpeed = originSpeed;
    }

    private IEnumerator IncreaseDamage()
    {
        drill.damageAmount += item.value;
        yield return new WaitForSeconds(3);
        drill.damageAmount = originDamage;
    }
}
