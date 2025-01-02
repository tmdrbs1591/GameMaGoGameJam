using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    // 현재 돈 시스템이 없어서 돈 시스템이 생기면 돈을 지불하고 업그레이드로 변경
    public GameObject shopPanel;

    public Image[] upgradeImage;

    public Button drillPower;   // Power는 드릴이 부술 수 있는 힘이 커지는 것
    public Button coinGain;
    public Button drillDamage;  // Damage는 몬스터를 죽을 때 올라가는 데미지
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

    #region 업그레이드
    public void PowerUpgrade()
    {
        // 이곳에 드릴 파워 올리는 코드 작성 지금은 임의로 디버그
        Debug.Log("드릴 파워 업");
    }
    public void CoinGainUpgrade()
    {
        // 이곳에 코인 획득량 올리는 코드 작성 지금은 임의로 디버그
        Debug.Log("코인 획득량 업");
    }
    public void DamageUpgrade()
    {
        // 이곳에 드릴 공격력 올리는 코드 작성 지금은 임의로 디버그
        Debug.Log("드릴 공격력 업");
    }
    public void HealthUpgrade()
    {
        // 이곳에 드릴 체력 올리는 코드 작성 지금은 임의로 디버그
        Debug.Log("드릴 체력 업");
    }
    #endregion
}
