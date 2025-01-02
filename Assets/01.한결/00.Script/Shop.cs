using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    // ���� �� �ý����� ��� �� �ý����� ����� ���� �����ϰ� ���׷��̵�� ����
    public GameObject shopPanel;    // ���� â

    public Image[] upgradeImage;    // ���׷��̵� �̹���

    public Button drillPower;   // Power�� �帱�� �μ� �� �ִ� ���� Ŀ���� ��
    public Button coinGain;     // ��� ȹ�淮 ����
    public Button drillDamage;  // Damage�� ���͸� ���� �� �ö󰡴� ������
    public Button drillHealth;  // �帱 ü�� ����

    public TextMeshProUGUI Coin;

    [SerializeField]
    private Vector3 detectionBoxSize = new Vector3(5f, 5f, 5f); // Ž�� �ڽ� ũ��
    [SerializeField]
    private bool isDrillDetected = false;  // �帱�� �����Ǿ��� Ȯ���ϴ� ����

    [SerializeField]
    private string targetTag = "Drill";    // Ÿ���� �±�

    private DrillGold drillGold;
    private Drill drill;

    // ��ȭ ��� ����
    private int powerUpgradeCost = 50;
    private int coinGainUpgradeCost = 30;
    private int damageUpgradeCost = 70;
    private int healthUpgradeCost = 40;

    private void Start()
    {
        shopPanel.SetActive(false);
        drillGold = FindObjectOfType<DrillGold>();
        drill = FindObjectOfType<Drill>();
    }

    private void Update()
    {
        FindObjectWithTagInRange();
        ShopOpenClose();

        Coin.text = "DM : " + drillGold.gold.ToString();
    }

    // ���� â�� �ݰ� ���� ���ִ� �Լ�
    private void ShopOpenClose()
    {
        if (isDrillDetected && Input.GetKeyDown(KeyCode.F))
        {
            shopPanel.SetActive(!shopPanel.activeSelf);
        }
    }

    // Ž�� ������ �׷��ִ� �����
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, detectionBoxSize);
    }

    // �ֺ��� �ִ� �±׸� Ž���ϴ� �Լ�
    private void FindObjectWithTagInRange()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, detectionBoxSize / 2, Quaternion.identity);
        isDrillDetected = false;

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(targetTag))
            {
                isDrillDetected = true;
                break;
            }
        }
    }

    #region ���׷��̵�
    public void PowerUpgrade()
    {
        if (drillGold.gold >= powerUpgradeCost)
        {
            drillGold.gold -= powerUpgradeCost; // ��� ����
            powerUpgradeCost += 20; // ��ȭ ��� ����
            Debug.Log("�帱 �Ŀ� ��");
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }

    public void CoinGainUpgrade()
    {
        if (drillGold.gold >= coinGainUpgradeCost)
        {
            drillGold.gold -= coinGainUpgradeCost; // ��� ����
            coinGainUpgradeCost += 10; // ��ȭ ��� ����
            Debug.Log("���� ȹ�淮 ��");
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }

    public void DamageUpgrade()
    {
        if (drillGold.gold >= damageUpgradeCost)
        {
            drillGold.gold -= damageUpgradeCost; // ��� ����
            damageUpgradeCost += 30; // ��ȭ ��� ����
            Debug.Log("�帱 ���ݷ� ��");
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }

    public void HealthUpgrade()
    {
        if (drillGold.gold >= healthUpgradeCost)
        {
            drillGold.gold -= healthUpgradeCost; // ��� ����
            healthUpgradeCost += 15; // ��ȭ ��� ����
            Debug.Log("�帱 ü�� ��");
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }
    #endregion
}
