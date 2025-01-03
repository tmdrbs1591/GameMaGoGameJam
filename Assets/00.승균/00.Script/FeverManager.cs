using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FeverManager : MonoBehaviour
{
    public static FeverManager instance;

    [SerializeField] public float feverValue; // 현재 피버 값
    [SerializeField] private float maxFeverValue; // 최대 피버 값
    [SerializeField] private GameObject feverEffect; // 피버 이펙트 게임 오브젝트
    [SerializeField] private Slider feverSlider; // 피버 슬라이더 UI

    [SerializeField] GameObject feverPrefab;

    [SerializeField] private Color feverColor = Color.red; // 피버 상태의 색깔
    private Color originalColor; // 기본 색깔

    private bool isFeverActive = false; // 피버 효과가 활성화되어 있는지 여부
    private Coroutine feverCoroutine; // 피버 값 감소를 위한 코루틴

    private Image fillImage; // 슬라이더 Fill 이미지

    [SerializeField] Drill drill;

    private void Awake()
    {
        instance = this; // 싱글톤 인스턴스 설정
    }

    private void Start()
    {
        // Fill 이미지 컴포넌트 가져오기
        fillImage = feverSlider.fillRect.GetComponent<Image>();
        // Fill 이미지의 기본 색상 저장
        originalColor = fillImage.color;
    }

    private void Update()
    {
        // 피버 슬라이더 값을 현재 피버 값에 따라 보간 처리
        feverSlider.value = Mathf.Lerp(feverSlider.value, feverValue / maxFeverValue, Time.deltaTime * 40f);
        HandleFeverEffect(); // 피버 효과 처리
    }

    private void HandleFeverEffect()
    {
        // 피버 값이 최대값 이상일 때
        if (feverValue >= maxFeverValue)
        {
            if (!isFeverActive)
            {
                isFeverActive = true; // 피버 효과 활성화 상태로 변경
                feverEffect.SetActive(true); // 피버 이펙트 활성화
                fillImage.color = feverColor; // 슬라이더 색상을 피버 색깔로 변경
                feverPrefab.SetActive(true);

                drill.moveSpeed += 12;

                // 이전에 실행 중인 코루틴이 있으면 중지하고 새로 시작
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
        float initialFeverValue = feverValue; // 초기 피버 값 저장
        float duration = 7f; // 피버 값 감소 시간 (초)
        float elapsed = 0f; // 경과 시간

        // 3초 동안 피버 값을 감소시키는 루프
        while (elapsed < duration)
        {
            feverValue = Mathf.Lerp(initialFeverValue, 0f, elapsed / duration); // 피버 값을 선형 보간
            elapsed += Time.deltaTime; // 경과 시간 업데이트
            yield return null; // 다음 프레임까지 대기
        }

        feverValue = 0f; // 피버 값이 0으로 설정
        feverEffect.SetActive(false); // 피버 이펙트 비활성화
        feverPrefab.SetActive(false);
        isFeverActive = false; // 피버 효과 비활성화 상태로 변경
        fillImage.color = originalColor; // 슬라이더 색상을 기본 색깔로 복원
        drill.moveSpeed -= 12;
    }
}
