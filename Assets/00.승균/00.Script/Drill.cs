using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Drill : MonoBehaviour
{
    Camera cam;
    public float moveSpeed = 5f;  // �̵� �ӵ�
    public float rotationSpeed = 5f;  // ȸ�� �ӵ�

    public GameObject hitPtc;
    [Header("����")]
    [SerializeField] Vector3 attackBoxSize; // ���� ����
    [SerializeField] Transform attackBoxPos; // ���� ��ġ
    [SerializeField] float damageInterval = 0.2f; // ���ظ� �� ���� (0.2��)
    [SerializeField] float damageAmount = 10f; // ���ط�

    private Coroutine damageCoroutine; // ���� �ڷ�ƾ �����

    public bool isCollidingWithEnemy = false; // ���� �浹 ����
    [SerializeField] GameObject damageText;

    [Header("Cinemachine ����")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // �ó׸ӽ� ī�޶�
    [SerializeField] private float defaultZoom = 20f; // �⺻ �� (FOV)
    [SerializeField] private float zoomStep = 1f; // �� ���ҷ�
    [SerializeField] private float minZoom = 10f; // �ּ� �� (FOV)


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;  // ���� ī�޶� ��������
    }

    void Update()
    {
        // ���콺 ���� ��ư Ŭ�� �ÿ��� ���콺 �������� �̵�
        if (Input.GetMouseButton(0) && !isCollidingWithEnemy)
        {
            MoveTowardsMouseDirection();
            RotateAwayFromMouseDirection();  // ���콺 �ݴ� �������� ȸ���ϴ� �Լ�
        }
        // ���콺 ���� ��ư�� ������ �ʾ��� �� ���� �������� õõ�� ��������
        else
        {

            MoveDownSlowly();
        }
    }

    void MoveDownSlowly()
    {
        if (isCollidingWithEnemy)
            return;
        // ���� �������� �Ʒ��� õõ�� �̵�
        transform.position += transform.up * -moveSpeed * Time.deltaTime / 3;  // -moveSpeed�� �Ʒ� �������� �̵�
    }



    void MoveTowardsMouseDirection()
    {
        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;  // ī�޶���� �Ÿ� (���� �ʿ�)

        // ���� ��ǥ�� ��ȯ
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);

        // Drill ��ü�� ���콺�� ���ϴ� ���� ���
        Vector3 direction = worldPos - transform.position;
        direction.z = 0; // z�� ���� (2D ���ӿ����� ����)

        // �������� �̵�
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
    }

    void RotateAwayFromMouseDirection()
    {
        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;  // ī�޶���� �Ÿ� (���� �ʿ�)

        // ���� ��ǥ�� ��ȯ
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);

        // Drill ��ü�� ���콺 �ݴ� ������ ���ϴ� ���� ���
        Vector3 direction = transform.position - worldPos;  // �ݴ� ���� ���
        direction.z = 0; // z�� ���� (2D ���ӿ����� ����)

        // ȸ�� ���� ��� (���콺 �ݴ� �������� 180�� ȸ��)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + -90f;  // 180���� ������

        // ȸ�� ����
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            isCollidingWithEnemy = true;
            // ���� ó�� �浹 �� Damage Coroutine ����
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DamageOverTime(other.transform));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            isCollidingWithEnemy = false;
            // ���� �浹�� ������ �� Damage Coroutine ����
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator DamageOverTime(Transform enemy)
    {
        // 'Enemy'�� ��� ���� �� 0.2�ʸ��� ���ظ� �ִ� �ڷ�ƾ
        while (true)
        {
            Damage(damageAmount, attackBoxPos, attackBoxSize);  // ���� �ֱ�

            yield return new WaitForSeconds(damageInterval);  // 0.2�� ���
        }
    }

    void Damage(float damage, Transform boxPos, Vector3 boxSize)
    {
        // ��ȣ�ۿ� �ڽ� ���� �浹ü Ȯ��
        Collider[] colliders = Physics.OverlapBox(boxPos.position, boxSize / 2f);

        foreach (Collider collider in colliders)
        {
            if (collider != null && collider.CompareTag("Enemy"))
            {
                var enemyScript = collider.GetComponent<Enemy>();

                if (enemyScript != null)
                {
                    float randomX = Random.Range(-2f, 2f);  // X �� ����
                    float randomY = Random.Range(0.5f, 1.5f); // Y �� ����

                    Vector3 randomPosition = new Vector3(
                        enemyScript.transform.position.x + randomX,
                        enemyScript.transform.position.y + randomY,
                        enemyScript.transform.position.z - 2
                    );

                    var damageTextScript = Instantiate(damageText, randomPosition, Quaternion.identity).GetComponent<TMP_Text>();
                    damageTextScript.text = damage.ToString();

                    Destroy(damageTextScript.gameObject, 2f);

                    // ������ ���� �ֱ�
                    enemyScript.TakeDamage(damage);
                    Instantiate(hitPtc, collider.transform.position, Quaternion.identity);

                    Debug.Log("����!");

                    // �ó׸ӽ� ī�޶� �� ����
                    AdjustCameraZoom();
                }
            }
        }
    }

    void AdjustCameraZoom()
    {
        if (virtualCamera != null)
        {
            // ���� FOV ��������
            float currentFOV = virtualCamera.m_Lens.FieldOfView;

            // �� ���� (�ּ� �� �ѵ� Ȯ��)
            float newFOV = Mathf.Max(currentFOV - zoomStep, minZoom);
            virtualCamera.m_Lens.FieldOfView = newFOV;

            Debug.Log($"Camera Zoom Adjusted: {newFOV}");
        }
        else
        {
            Debug.LogError("Cinemachine Virtual Camera�� �������� �ʾҽ��ϴ�.");
        }
    }
    public void ResetCameraZoom()
    {
        if (virtualCamera != null)
        {
            // �⺻ ������ ����
            virtualCamera.m_Lens.FieldOfView = defaultZoom;
            Debug.Log($"Camera Zoom Reset to Default: {defaultZoom}");
        }
        else
        {
            Debug.LogError("Cinemachine Virtual Camera�� �������� �ʾҽ��ϴ�.");
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(attackBoxPos.position, attackBoxSize);
    }
}
