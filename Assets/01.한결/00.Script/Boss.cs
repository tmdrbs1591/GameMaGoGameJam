using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;   // 보스가 아래로 내려가는 속도

    private void Update()
    {
        // 보스가 아래로 내려가는 함수
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;
    }
}
