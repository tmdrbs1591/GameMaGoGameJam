using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;   // ������ �Ʒ��� �������� �ӵ�

    private void Update()
    {
        // ������ �Ʒ��� �������� �Լ�
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;
    }
}
