using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Drill : MonoBehaviour
{
    Camera cam;
    public float moveSpeed = 5f;  // 이동 속도
    public float rotationSpeed = 5f;  // 회전 속도

    public GameObject hitPtc;
    [Header("공격")]
    [SerializeField] Vector3 attackBoxSize; // 공격 범위
    [SerializeField] Transform attackBoxPos; // 공격 위치
    [SerializeField] float damageInterval = 0.2f; // 피해를 줄 간격 (0.2초)
    [SerializeField] float damageAmount = 10f; // 피해량

    private Coroutine damageCoroutine; // 피해 코루틴 저장용

    public bool isCollidingWithEnemy = false; // 적과 충돌 여부
    [SerializeField] GameObject damageText;

    [Header("Cinemachine 설정")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // 시네머신 카메라
    [SerializeField] private float defaultZoom = 20f; // 기본 줌 (FOV)
    [SerializeField] private float zoomStep = 1f; // 줌 감소량
    [SerializeField] private float minZoom = 10f; // 최소 줌 (FOV)


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;  // 메인 카메라 가져오기
    }

    void Update()
    {
        // 마우스 왼쪽 버튼 클릭 시에만 마우스 방향으로 이동
        if (Input.GetMouseButton(0) && !isCollidingWithEnemy)
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
        if (isCollidingWithEnemy)
            return;
        // 현재 방향으로 아래로 천천히 이동
        transform.position += transform.up * -moveSpeed * Time.deltaTime / 3;  // -moveSpeed로 아래 방향으로 이동
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
        direction.z = 0; // z축 고정 (2D 게임에서만 적용)

        // 방향으로 이동
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
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
        if (other.CompareTag("Enemy"))
        {
            isCollidingWithEnemy = true;
            // 적과 처음 충돌 시 Damage Coroutine 시작
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
            // 적과 충돌이 끝났을 때 Damage Coroutine 중지
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator DamageOverTime(Transform enemy)
    {
        // 'Enemy'와 닿아 있을 때 0.2초마다 피해를 주는 코루틴
        while (true)
        {
            Damage(damageAmount, attackBoxPos, attackBoxSize);  // 피해 주기

            yield return new WaitForSeconds(damageInterval);  // 0.2초 대기
        }
    }

    void Damage(float damage, Transform boxPos, Vector3 boxSize)
    {
        // 상호작용 박스 내의 충돌체 확인
        Collider[] colliders = Physics.OverlapBox(boxPos.position, boxSize / 2f);

        foreach (Collider collider in colliders)
        {
            if (collider != null && collider.CompareTag("Enemy"))
            {
                var enemyScript = collider.GetComponent<Enemy>();

                if (enemyScript != null)
                {
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
                    Instantiate(hitPtc, collider.transform.position, Quaternion.identity);

                    Debug.Log("공격!");

                    // 시네머신 카메라 줌 감소
                    AdjustCameraZoom();
                }
            }
        }
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

            Debug.Log($"Camera Zoom Adjusted: {newFOV}");
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
            Debug.Log($"Camera Zoom Reset to Default: {defaultZoom}");
        }
        else
        {
            Debug.LogError("Cinemachine Virtual Camera가 설정되지 않았습니다.");
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(attackBoxPos.position, attackBoxSize);
    }
}
