using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    // ���� �� �ý����� ��� �� �ý����� ����� ���� �����ϰ� ���׷��̵�� ����
    public GameObject shopPanel;

    public Image[] upgradeImage;

    public Button drillPower;   // Power�� �帱�� �μ� �� �ִ� ���� Ŀ���� ��
    public Button coinGain;
    public Button drillDamage;  // Damage�� ���͸� ���� �� �ö󰡴� ������
    public Button drillHealth;

    [SerializeField]
    private float detectionRadius;

    [SerializeField]
    private bool isPlayerDetected = false;

    [SerializeField]
    private string targetTag = "Player";

    private void Start()
    {
        shopPanel.SetActive(false);
    }

    private void Update()
    {
        FindObjectWithTagInRange();
        ShopOpenClose();
    }

    private void ShopOpenClose()
    {
        if (isPlayerDetected && Input.GetKeyDown(KeyCode.F))
        {
            shopPanel.SetActive(!shopPanel.activeSelf);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    private void FindObjectWithTagInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        isPlayerDetected = false;

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(targetTag))
            {
                isPlayerDetected = true;
                break;
            }
        }
    }

    #region ���׷��̵�
    public void PowerUpgrade()
    {
        // �̰��� �帱 �Ŀ� �ø��� �ڵ� �ۼ� ������ ���Ƿ� �����
        Debug.Log("�帱 �Ŀ� ��");
    }
    public void CoinGainUpgrade()
    {
        // �̰��� ���� ȹ�淮 �ø��� �ڵ� �ۼ� ������ ���Ƿ� �����
        Debug.Log("���� ȹ�淮 ��");
    }
    public void DamageUpgrade()
    {
        // �̰��� �帱 ���ݷ� �ø��� �ڵ� �ۼ� ������ ���Ƿ� �����
        Debug.Log("�帱 ���ݷ� ��");
    }
    public void HealthUpgrade()
    {
        // �̰��� �帱 ü�� �ø��� �ڵ� �ۼ� ������ ���Ƿ� �����
        Debug.Log("�帱 ü�� ��");
    }
    #endregion
}
