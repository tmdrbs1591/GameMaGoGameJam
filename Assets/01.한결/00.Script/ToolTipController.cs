//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class ToolTipController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
//{
//    public ToolTip toolTip;   // ������ ��Ÿ�� UI ������Ʈ
//    private Item item;         // ������ ǥ���� Item ��ü�� ���� �Ҵ�

//    public void ItemAdd(Item newitem)
//    {
//        item = newitem;
//    }

//    // ���콺�� ������ ���� �÷����� ��
//    public void OnPointerEnter(PointerEventData eventData)
//    {
//        if (item != null)
//        {
//            toolTip.gameObject.SetActive(true);  // ���� Ȱ��ȭ
//            toolTip.SetupTooltip(item.itemName, item.Description);  // ���� ����
//        }
//    }

//    // ���콺�� �����ۿ��� ����� ��
//    public void OnPointerExit(PointerEventData eventData)
//    {
//        toolTip.gameObject.SetActive(false);  // ���� ��Ȱ��ȭ
//    }
//}
