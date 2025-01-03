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
     public float raycastLength = 3f; // Raycast의 길이
    public float oriMoveSpeed = 5f;  // 이동 속도
    public float moveSpeed = 5f;  // 이동 속도
    public float rotationSpeed = 5f;  // 회전 속도

    public GameObject hitPtc;
    [Header("공격")]
    [SerializeField] Vector3 attackBoxSize; // 공격 범위
    [SerializeField] Transform attackBoxPos; // 공격 위치
    [SerializeField] float damageInterval = 0.2f; // 피해를 줄 간격 (0.2초)
    [SerializeField] public float damageAmount = 10f; // 피해량
    [SerializeField] public float currentHP;
    [SerializeField] public float maxHP;
    [SerializeField] Slider hpSlider;
    [SerializeField] Slider dashSlider;

    private Rigidbody rb;

    bool isSKill;


    private Coroutine damageCoroutine; // 피해 코루틴 저장용

    public Transform eftPos;

    public bool isCollidingWithEnemy = false; // 적과 충돌 여부
    [SerializeField] GameObject damageText;
    [SerializeField] GameObject hitVolume;
    [SerializeField] GameObject skillEft;
    [SerializeField] GameObject hitEft;

    [SerializeField] GameObject attackEffect;

    bool isSkill;

    [Header("Cinemachine 설정")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // 시네머신 카메라
    [SerializeField] private float defaultZoom = 20f; // 기본 줌 (FOV)
    [SerializeField] private float zoomStep = 1f; // 줌 감소량
    [SerializeField] private float minZoom = 10f; // 최소 줌 (FOV)

    [SerializeField] GameObject myDamageEft;

    public float AttackCooldownTime = 0.1f; // 쿨타임 (초)
    private float AttackNextTime = 0f; // 사용 가능 시점

    public float dashCooldownTime = 0.2f; // 쿨타임 (초)
    private float dashNextTime = 0f; // 사용 가능 시점


    [SerializeField] public int combo;
    [SerializeField] TMP_Text comboText;
    [SerializeField] GameObject comboImage;

    [SerializeField] public Animator comboTextAnim;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;  // 메인 카메라 가져오기
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
            // 상호작용 박스 내의 충돌체 확인
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
                Damage(damageAmount, attackBoxPos, attackBoxSize);  // 피해 주기
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
        // 대쉬 쿨타임 슬라이더 값 계산 (1에서 0 사이로 보이게 함)
        if (Time.time < dashNextTime)
        {
            // 대쉬 쿨타임이 끝나지 않았을 때
            dashSlider.value = 1 - (Time.time - (dashNextTime - dashCooldownTime)) / dashCooldownTime;
        }
        else
        {
            // 대쉬 쿨타임이 끝났을 때
            dashSlider.value = 0;
        }

        Skill();
        // 마우스 왼쪽 버튼 클릭 시에만 마우스 방향으로 이동
        if (Input.GetMouseButton(1) && !isCollidingWithEnemy)
        {
            MoveTowardsMouseDirection();
            RotateAwayFromMouseDirection();  // 마우스 반대 방향으로 회전하는 함수
        }
        // 마우스 왼쪽 버튼을 누르지 않았을 때 현재 방향으로 천천히 내려가기
        else
        {
            MoveDownSlowly();
        }
    }
    void MoveDownSlowly()
    {
      
        // Drill 이동 (현재 방향 기준)
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
        // 마우스 위치를 월드 좌표로 변환
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;  // 카메라와의 거리 (조정 필요)

        // 월드 좌표로 변환
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);

        // Drill 객체가 마우스를 향하는 방향 계산
        Vector3 direction = worldPos - transform.position;
        direction.z = 0; // z축 고정 (3D에서 z값을 고정)


        // Wall과 충돌하지 않으면 이동
        rb.velocity = direction.normalized * moveSpeed;

    }

    void RotateAwayFromMouseDirection()
    {
        // 마우스 위치를 월드 좌표로 변환
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;  // 카메라와의 거리 (조정 필요)

        // 월드 좌표로 변환
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);

        // Drill 객체가 마우스 반대 방향을 향하는 방향 계산
        Vector3 direction = transform.position - worldPos;  // 반대 방향 계산
        direction.z = 0; // z축 고정 (2D 게임에서만 적용)

        // 회전 각도 계산 (마우스 반대 방향으로 180도 회전)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + -90f;  // 180도를 더해줌

        // 회전 적용
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
        // 상호작용 박스 내의 충돌체 확인
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
                    float randomX = Random.Range(-2f, 2f);  // X 값 범위
                    float randomY = Random.Range(0.5f, 1.5f); // Y 값 범위

                    Vector3 randomPosition = new Vector3(
                        enemyScript.transform.position.x + randomX,
                        enemyScript.transform.position.y + randomY,
                        enemyScript.transform.position.z - 2
                    );

                    var damageTextScript = Instantiate(damageText, randomPosition, Quaternion.identity).GetComponent<TMP_Text>();
                    damageTextScript.text = damage.ToString();

                    Destroy(damageTextScript.gameObject, 2f);


                    // 적에게 피해 주기
                    enemyScript.TakeDamage(damage);

                    ObjectPool.SpawnFromPool("HitPtc", collider.transform.position, Quaternion.identity);
                    ObjectPool.SpawnFromPool("HitEft", eftPos.transform.position, Quaternion.identity);
                    Debug.Log("공격!");

                    // 시네머신 카메라 줌 감소
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
            // 현재 FOV 가져오기
            float currentFOV = virtualCamera.m_Lens.FieldOfView;

            // 줌 감소 (최소 줌 한도 확인)
            float newFOV = Mathf.Max(currentFOV - zoomStep, minZoom);
            virtualCamera.m_Lens.FieldOfView = newFOV;

        }
        else
        {
            Debug.LogError("Cinemachine Virtual Camera가 설정되지 않았습니다.");
        }
    }
    public void ResetCameraZoom()
    {
        if (virtualCamera != null)
        {
            // 기본 줌으로 복원
            virtualCamera.m_Lens.FieldOfView = defaultZoom;
        }
        else
        {
            Debug.LogError("Cinemachine Virtual Camera가 설정되지 않았습니다.");
        }
    }

    IEnumerator SkillCor()
    {



        // 상호작용 박스 내의 충돌체 확인

        isCollidingWithEnemy = true;
        for (int i = 0; i < 14; i++)
        {
            Damage(damageAmount, attackBoxPos, attackBoxSize);  // 피해 주기
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
