using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTips : MonoBehaviour
{
    public TextMeshProUGUI tooltipText;
    public GameObject tooltipObject;

    private void Start()
    {
        tooltipObject.SetActive(false);
    }

    public void ShowTooltip(string text)
    {
        tooltipObject.SetActive(true);
        tooltipText.text = text;
    }

    public void HideTooltip()
    {
        tooltipObject.SetActive(false);
    }
}
