using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goods : MonoBehaviour
{
    public float speed = 5f;  // ������ �̵� �ӵ�
    public float curveStrength = 2f;  // ��� ���� (�������� �̵� �Ÿ�)
    public float minStartDelay = 0f;  // ���� ������ �ּ� �� (��)
    public float maxStartDelay = 0.5f;  // ���� ������ �ִ� �� (��)

    [SerializeField] GameObject goodsEffect;

    private Transform player;  // �÷��̾��� Transform
    private Vector3 startPoint;
    private Vector3 controlPoint;
    private Vector3 endPoint;
    private float t = 0f;  // �ð� ����
    private float startTime;  // ���� �ð�
    private bool hasCurved = false;  // ��� �̹� �����ߴ��� ����

    private Collider goodsCollider;  // Goods�� Collider

    void Start()
    {
        // Collider ��������
        goodsCollider = GetComponent<Collider>();
        if (goodsCollider != null)
        {
            // Coroutine�� ����� isTrigger�� 1�� ���� ��Ȱ��ȭ
            StartCoroutine(ToggleTriggerAfterDelay(0.5f));
        }

        // �÷��̾� ������Ʈ�� �±׸� ���� ã��
        GameObject playerObject = GameObject.FindGameObjectWithTag("Drill");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            return;
        }

        // ������, ���� �ʱ�ȭ
        startPoint = transform.position;
        endPoint = player.position;
        SetRandomControlPoint();  // ���� ������ ����

        // ���� ������ �������� ����
        startTime = Time.time + Random.Range(minStartDelay, maxStartDelay);
    }

    void Update()
    {
        if (player == null)
            return;

        // ������ ������ �ʾ����� ��ٸ�
        if (Time.time < startTime)
            return;

        if (!hasCurved)
        {
            // � ��θ� ���� �̵�
            t += Time.deltaTime * speed / Vector3.Distance(startPoint, endPoint);
            t = Mathf.Clamp01(t);  // t�� 0�� 1 ���̷� ����

            // ������ ��� ��ġ�� ���
            Vector3 curvePosition = CalculateBezierCurve(startPoint, controlPoint, endPoint, t);
            transform.position = curvePosition;

            // � �̵� �Ϸ� �� ���� �̵����� ��ȯ
            if (t >= 1f)
            {
                hasCurved = true;
                startPoint = endPoint; // � �Ϸ� �� ��ġ ������Ʈ
            }
        }
        else
        {
            // �������� �̵�
            endPoint = player.position; // �� �����Ӹ��� �÷��̾� ��ġ ����
            transform.position = Vector3.MoveTowards(transform.position, endPoint, speed * Time.deltaTime);
        }
    }

    // 2�� ������ � ���
    Vector3 CalculateBezierCurve(Vector3 start, Vector3 control, Vector3 end, float t)
    {
        float u = 1f - t;
        return u * u * start + 2f * u * t * control + t * t * end;
    }

    // �������� �����ϰ� ����
    void SetRandomControlPoint()
    {
        // �������� ���� �������� ������ �߰������� ������ �������� �̵�
        Vector3 midPoint = (startPoint + endPoint) / 2;
        Vector3 randomDirection = Random.onUnitSphere;  // ���� ���� ����
        controlPoint = midPoint + randomDirection * curveStrength;
    }

    private IEnumerator ToggleTriggerAfterDelay(float delay)
    {
        // isTrigger ��Ȱ��ȭ
        goodsCollider.isTrigger = false;

        // ������ �ð� ���
        yield return new WaitForSeconds(delay);

        // isTrigger Ȱ��ȭ
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
