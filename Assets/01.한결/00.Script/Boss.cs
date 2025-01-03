using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;   // ������ �Ʒ��� �������� �ӵ�

    [SerializeField] Transform[] bulletPos;  // �Ѿ� ���� ��ġ �迭
    [SerializeField] Transform tonguePos; 
    [SerializeField] GameObject[] dangerLine; // ��� ���� �迭

    [SerializeField] GameObject bulletPrefab; // �Ѿ� ������
    [SerializeField] GameObject tongue;       // tongue ��ü

    [SerializeField] private float fireInterval = 2f;  // �Ѿ� �߻� ����
    [SerializeField] private float tongueInterval = 15f;  // tongue ���� ����

    private float fireTimer;
    private float tongueTimer;

    Enemy enemy;
 
    private void Update()
    {
        if (GameManager.instance.isDamageTrue)
            return;
        
        // ������ �Ʒ��� �������� �Լ�
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;

        if (CutSceneManager.instance.isCutScene)
            return;

        // �Ѿ� �߻� Ÿ�̸� ������Ʈ
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireInterval)
        {
            FireRandomBullet();
            fireTimer = 0f;
        }

        // tongue ���� Ÿ�̸� ������Ʈ
        tongueTimer += Time.deltaTime;
        if (tongueTimer >= tongueInterval)
        {
            SpawnTongue();
            tongueTimer = 0f;
        }
    }

    private void FireRandomBullet()
    {
        if (bulletPos.Length == 0 || dangerLine.Length == 0 || bulletPos.Length != dangerLine.Length)
        {
            Debug.LogWarning("bulletPos�� dangerLine �迭�� ���̰� ���� �ʽ��ϴ�.");
            return;
        }

        // ���� �ε��� ����
        int randomIndex = Random.Range(0, bulletPos.Length);

        // �ش� �ε����� DangerLine Ȱ��ȭ
        dangerLine[randomIndex].SetActive(true);

        // ��� �� DangerLine ��Ȱ��ȭ �ڷ�ƾ ����
        StartCoroutine(DeactivateDangerLineAfterDelay(dangerLine[randomIndex], 0.5f));

        // �ش� ��ġ�� �Ѿ� ����
        Instantiate(bulletPrefab, bulletPos[randomIndex].position, Quaternion.identity);
    }

    private void SpawnTongue()
    {
        if (tongue != null)
        {
            // ���� ������ ��ġ���� tongue ����
           Destroy( Instantiate(tongue, tonguePos.transform.position + new Vector3 (0, -20, 0), Quaternion.identity),7);
        }
        else
        {
            Debug.LogWarning("tongue �������� �������� �ʾҽ��ϴ�.");
        }
    }

    private System.Collections.IEnumerator DeactivateDangerLineAfterDelay(GameObject dangerLine, float delay)
    {
        yield return new WaitForSeconds(delay);
        dangerLine.SetActive(false);
    }
}
