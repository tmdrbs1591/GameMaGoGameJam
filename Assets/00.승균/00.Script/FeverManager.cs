using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FeverManager : MonoBehaviour
{
    public static FeverManager instance;

    [SerializeField] public float feverValue; // ���� �ǹ� ��
    [SerializeField] private float maxFeverValue; // �ִ� �ǹ� ��
    [SerializeField] private GameObject feverEffect; // �ǹ� ����Ʈ ���� ������Ʈ
    [SerializeField] private Slider feverSlider; // �ǹ� �����̴� UI

    [SerializeField] GameObject feverPrefab;

    [SerializeField] private Color feverColor = Color.red; // �ǹ� ������ ����
    private Color originalColor; // �⺻ ����

    private bool isFeverActive = false; // �ǹ� ȿ���� Ȱ��ȭ�Ǿ� �ִ��� ����
    private Coroutine feverCoroutine; // �ǹ� �� ���Ҹ� ���� �ڷ�ƾ

    private Image fillImage; // �����̴� Fill �̹���

    [SerializeField] Drill drill;

    private void Awake()
    {
        instance = this; // �̱��� �ν��Ͻ� ����
    }

    private void Start()
    {
        // Fill �̹��� ������Ʈ ��������
        fillImage = feverSlider.fillRect.GetComponent<Image>();
        // Fill �̹����� �⺻ ���� ����
        originalColor = fillImage.color;
    }

    private void Update()
    {
        // �ǹ� �����̴� ���� ���� �ǹ� ���� ���� ���� ó��
        feverSlider.value = Mathf.Lerp(feverSlider.value, feverValue / maxFeverValue, Time.deltaTime * 40f);
        HandleFeverEffect(); // �ǹ� ȿ�� ó��
    }

    private void HandleFeverEffect()
    {
        // �ǹ� ���� �ִ밪 �̻��� ��
        if (feverValue >= maxFeverValue)
        {
            if (!isFeverActive)
            {
                isFeverActive = true; // �ǹ� ȿ�� Ȱ��ȭ ���·� ����
                feverEffect.SetActive(true); // �ǹ� ����Ʈ Ȱ��ȭ
                fillImage.color = feverColor; // �����̴� ������ �ǹ� ����� ����
                feverPrefab.SetActive(true);

                drill.moveSpeed += 12;

                // ������ ���� ���� �ڷ�ƾ�� ������ �����ϰ� ���� ����
                if (feverCoroutine != null)
                {
                    StopCoroutine(feverCoroutine);
                }
                feverCoroutine = StartCoroutine(ReduceFeverOverTime());
            }
        }
    }

    private IEnumerator ReduceFeverOverTime()
    {
        float initialFeverValue = feverValue; // �ʱ� �ǹ� �� ����
        float duration = 7f; // �ǹ� �� ���� �ð� (��)
        float elapsed = 0f; // ��� �ð�

        // 3�� ���� �ǹ� ���� ���ҽ�Ű�� ����
        while (elapsed < duration)
        {
            feverValue = Mathf.Lerp(initialFeverValue, 0f, elapsed / duration); // �ǹ� ���� ���� ����
            elapsed += Time.deltaTime; // ��� �ð� ������Ʈ
            yield return null; // ���� �����ӱ��� ���
        }

        feverValue = 0f; // �ǹ� ���� 0���� ����
        feverEffect.SetActive(false); // �ǹ� ����Ʈ ��Ȱ��ȭ
        feverPrefab.SetActive(false);
        isFeverActive = false; // �ǹ� ȿ�� ��Ȱ��ȭ ���·� ����
        fillImage.color = originalColor; // �����̴� ������ �⺻ ����� ����
        drill.moveSpeed -= 12;
    }
}
