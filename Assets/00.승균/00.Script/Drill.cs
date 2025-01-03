using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Drill : MonoBehaviour
{
    Camera cam;
     public float raycastLength = 3f; // Raycast�� ����
    public float oriMoveSpeed = 5f;  // �̵� �ӵ�
    public float moveSpeed = 5f;  // �̵� �ӵ�
    public float rotationSpeed = 5f;  // ȸ�� �ӵ�

    public GameObject hitPtc;
    [Header("����")]
    [SerializeField] Vector3 attackBoxSize; // ���� ����
    [SerializeField] Transform attackBoxPos; // ���� ��ġ
    [SerializeField] float damageInterval = 0.2f; // ���ظ� �� ���� (0.2��)
    [SerializeField] public float damageAmount = 10f; // ���ط�
    [SerializeField] public float currentHP;
    [SerializeField] public float maxHP;
    [SerializeField] Slider hpSlider;
    [SerializeField] Slider dashSlider;

    private Rigidbody rb;

    bool isSKill;


    private Coroutine damageCoroutine; // ���� �ڷ�ƾ �����

    public Transform eftPos;

    public bool isCollidingWithEnemy = false; // ���� �浹 ����
    [SerializeField] GameObject damageText;
    [SerializeField] GameObject hitVolume;
    [SerializeField] GameObject skillEft;
    [SerializeField] GameObject hitEft;

    [SerializeField] GameObject attackEffect;

    bool isSkill;

    [Header("Cinemachine ����")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // �ó׸ӽ� ī�޶�
    [SerializeField] private float defaultZoom = 20f; // �⺻ �� (FOV)
    [SerializeField] private float zoomStep = 1f; // �� ���ҷ�
    [SerializeField] private float minZoom = 10f; // �ּ� �� (FOV)

    [SerializeField] GameObject myDamageEft;

    public float AttackCooldownTime = 0.1f; // ��Ÿ�� (��)
    private float AttackNextTime = 0f; // ��� ���� ����

    public float dashCooldownTime = 0.2f; // ��Ÿ�� (��)
    private float dashNextTime = 0f; // ��� ���� ����


    [SerializeField] public int combo;
    [SerializeField] TMP_Text comboText;
    [SerializeField] GameObject comboImage;

    [SerializeField] public Animator comboTextAnim;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;  // ���� ī�޶� ��������
        rb = GetComponent<Rigidbody>();
        comboImage.SetActive(false);

        StartCoroutine(StartCor());

    }

    IEnumerator StartCor()
    {

        this.enabled = false;
        yield return new WaitForSeconds(2f);
        this.enabled = true;

    }
    void Update()
    {
        if (CutSceneManager.instance.isCutScene)
            return;
        if (isCollidingWithEnemy)
            hitVolume.SetActive(true);
        else
            hitVolume.SetActive(false);

        if (Input.GetMouseButton(0))
        {

            attackEffect.SetActive(true);
            // ��ȣ�ۿ� �ڽ� ���� �浹ü Ȯ��
            Collider[] colliders = Physics.OverlapBox(attackBoxPos.position, attackBoxSize / 2f);

            foreach (Collider collider in colliders)
            {
                if (collider != null && collider.CompareTag("Enemy") || collider != null && collider.CompareTag("Boss"))
                {
                    isCollidingWithEnemy = true;
                    Debug.Log(isCollidingWithEnemy);
                }

            }
            if (Time.time >= AttackNextTime)
            {
                Damage(damageAmount, attackBoxPos, attackBoxSize);  // ���� �ֱ�
                AttackNextTime = Time.time + AttackCooldownTime;
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            isCollidingWithEnemy = false;
            Debug.Log("ddd");
            attackEffect.SetActive(false);
        }


        hpSlider.value = currentHP / maxHP;
        comboText.text = combo.ToString() + "Kills";
        // �뽬 ��Ÿ�� �����̴� �� ��� (1���� 0 ���̷� ���̰� ��)
        if (Time.time < dashNextTime)
        {
            // �뽬 ��Ÿ���� ������ �ʾ��� ��
            dashSlider.value = 1 - (Time.time - (dashNextTime - dashCooldownTime)) / dashCooldownTime;
        }
        else
        {
            // �뽬 ��Ÿ���� ������ ��
            dashSlider.value = 0;
        }

        Skill();
        // ���콺 ���� ��ư Ŭ�� �ÿ��� ���콺 �������� �̵�
        if (Input.GetMouseButton(1) && !isCollidingWithEnemy)
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
      
        // Drill �̵� (���� ���� ����)
        rb.velocity = transform.up * -moveSpeed / 3;

    }
    void Skill()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {

            if (Time.time >= dashNextTime)
            {
                CameraShake.instance.ShakeCamera(10f, 0.6f);

                AudioManager.instance.PlaySound(transform.position, 3, Random.Range(1.3f, 1.7f), 1f);

                skillEft.SetActive(false);
                skillEft.SetActive(true);
                StartCoroutine(DashCor());

                dashNextTime = Time.time + dashCooldownTime;

            }
        }
    }


    public void ComboStart()
    {
        comboImage.SetActive(true);
    }

    void ComboEnd()
    {
        combo = 0;
        comboImage.SetActive(false);
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
        direction.z = 0; // z�� ���� (3D���� z���� ����)


        // Wall�� �浹���� ������ �̵�
        rb.velocity = direction.normalized * moveSpeed;

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
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            if (isSKill)
            {
                StartCoroutine(SkillCor());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {


        if (other.CompareTag("EnemyBullet"))
        {
            StartCoroutine(MyDamageCor());
            Destroy(other.gameObject);
        }
    }

    IEnumerator MyDamageCor()
    {
        myDamageEft.SetActive(true);

        ComboEnd();

        currentHP -= 1;

        CameraShake.instance.ShakeCamera(15f, 0.4f);
        AdjustCameraZoom();
        moveSpeed /= 2;

        yield return new WaitForSeconds(0.5f);


        myDamageEft.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        moveSpeed = oriMoveSpeed;
        ResetCameraZoom();


    }


    void Damage(float damage, Transform boxPos, Vector3 boxSize)
    {
        // ��ȣ�ۿ� �ڽ� ���� �浹ü Ȯ��
        Collider[] colliders = Physics.OverlapBox(boxPos.position, boxSize / 2f);

        foreach (Collider collider in colliders)
        {
            if (collider != null && collider.CompareTag("Enemy") || collider.CompareTag("Boss"))
            {
                var enemyScript = collider.GetComponent<Enemy>();

                if (enemyScript != null)
                {

                    if (enemyScript.currentType == Type.Wall)
                    {
                        zoomStep = 0.05f;
                    }
                    else if (enemyScript.currentType == Type.Boss)
                    {
                        zoomStep = 0f;
                    }
                    else
                    {
                        zoomStep = 1;
                    }
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

                    ObjectPool.SpawnFromPool("HitPtc", collider.transform.position, Quaternion.identity);
                    ObjectPool.SpawnFromPool("HitEft", eftPos.transform.position, Quaternion.identity);
                    Debug.Log("����!");

                    // �ó׸ӽ� ī�޶� �� ����
                    AdjustCameraZoom();
                }
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {   
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
        }
        else
        {
            Debug.LogError("Cinemachine Virtual Camera�� �������� �ʾҽ��ϴ�.");
        }
    }

    IEnumerator SkillCor()
    {



        // ��ȣ�ۿ� �ڽ� ���� �浹ü Ȯ��

        isCollidingWithEnemy = true;
        for (int i = 0; i < 14; i++)
        {
            Damage(damageAmount, attackBoxPos, attackBoxSize);  // ���� �ֱ�
            yield return new WaitForSeconds(0.05f);
        }
        isCollidingWithEnemy = false;

    }

    IEnumerator DashCor()
    {
        isSKill = true;
        moveSpeed = 60;
        yield return new WaitForSeconds(0.1f);
        isSKill = false;
        moveSpeed = 17;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(attackBoxPos.position, attackBoxSize);
    }
}
