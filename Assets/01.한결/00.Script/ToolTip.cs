using TMPro;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    public void SetupToolTip(string title, string description)
    {
        titleText.text = title;
        descriptionText.text = description;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
