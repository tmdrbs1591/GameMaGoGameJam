using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goods : MonoBehaviour
{
    public float speed = 5f;  // 일정한 이동 속도
    public float curveStrength = 2f;  // 곡선의 강도 (제어점의 이동 거리)
    public float minStartDelay = 0f;  // 시작 지연의 최소 값 (초)
    public float maxStartDelay = 0.5f;  // 시작 지연의 최대 값 (초)

    [SerializeField] GameObject goodsEffect;

    private Transform player;  // 플레이어의 Transform
    private Vector3 startPoint;
    private Vector3 controlPoint;
    private Vector3 endPoint;
    private float t = 0f;  // 시간 변수
    private float startTime;  // 시작 시간
    private bool hasCurved = false;  // 곡선을 이미 수행했는지 여부

    private Collider goodsCollider;  // Goods의 Collider

    void Start()
    {
        // Collider 가져오기
        goodsCollider = GetComponent<Collider>();
        if (goodsCollider != null)
        {
            // Coroutine을 사용해 isTrigger를 1초 동안 비활성화
            StartCoroutine(ToggleTriggerAfterDelay(0.5f));
        }

        // 플레이어 오브젝트를 태그를 통해 찾기
        GameObject playerObject = GameObject.FindGameObjectWithTag("Drill");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            return;
        }

        // 시작점, 끝점 초기화
        startPoint = transform.position;
        endPoint = player.position;
        SetRandomControlPoint();  // 랜덤 제어점 설정

        // 시작 지연을 랜덤으로 설정
        startTime = Time.time + Random.Range(minStartDelay, maxStartDelay);
    }

    void Update()
    {
        if (player == null)
            return;

        // 지연이 지나지 않았으면 기다림
        if (Time.time < startTime)
            return;

        if (!hasCurved)
        {
            // 곡선 경로를 따라 이동
            t += Time.deltaTime * speed / Vector3.Distance(startPoint, endPoint);
            t = Mathf.Clamp01(t);  // t를 0과 1 사이로 제한

            // 베지어 곡선의 위치를 계산
            Vector3 curvePosition = CalculateBezierCurve(startPoint, controlPoint, endPoint, t);
            transform.position = curvePosition;

            // 곡선 이동 완료 후 직선 이동으로 전환
            if (t >= 1f)
            {
                hasCurved = true;
                startPoint = endPoint; // 곡선 완료 후 위치 업데이트
            }
        }
        else
        {
            // 직선으로 이동
            endPoint = player.position; // 매 프레임마다 플레이어 위치 갱신
            transform.position = Vector3.MoveTowards(transform.position, endPoint, speed * Time.deltaTime);
        }
    }

    // 2차 베지어 곡선 계산
    Vector3 CalculateBezierCurve(Vector3 start, Vector3 control, Vector3 end, float t)
    {
        float u = 1f - t;
        return u * u * start + 2f * u * t * control + t * t * end;
    }

    // 제어점을 랜덤하게 설정
    void SetRandomControlPoint()
    {
        // 제어점을 현재 시작점과 끝점의 중간점에서 랜덤한 방향으로 이동
        Vector3 midPoint = (startPoint + endPoint) / 2;
        Vector3 randomDirection = Random.onUnitSphere;  // 랜덤 방향 벡터
        controlPoint = midPoint + randomDirection * curveStrength;
    }

    private IEnumerator ToggleTriggerAfterDelay(float delay)
    {
        // isTrigger 비활성화
        goodsCollider.isTrigger = false;

        // 지정된 시간 대기
        yield return new WaitForSeconds(delay);

        // isTrigger 활성화
        goodsCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Drill"))
        {
            Destroy(Instantiate(goodsEffect, transform.position, Quaternion.identity), 2f);
            Destroy(gameObject);
        }
    }
}
