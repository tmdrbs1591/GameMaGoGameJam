using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public int value;
    public Sprite icon;
    public ItemType itemType;
    public int quantity; // 수량 관리

    public enum ItemType
    {
        Crystal,
        Health_Potion,
        Exp_Potion
    }
}
