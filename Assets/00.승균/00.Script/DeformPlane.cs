using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformPlane : MonoBehaviour
{
    MeshFilter meshFilter;
    Mesh PlaneMesh;
    Vector3[] verts;

    [SerializeField]
    float Radius;
    [SerializeField]
    float Power;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        PlaneMesh = meshFilter.mesh;
        verts = PlaneMesh.vertices;
    }

    private void OnTriggerStay(Collider other)
    {
        // 'Drill' 태그를 가진 객체가 충돌하면 변형을 실행
        if (other.CompareTag("Drill"))
        {
            DeformThisPlane(other.transform.position);
        }

        // 'EnemyBox' 태그를 가진 객체가 충돌하면 범위 3으로 뚫기
        if (other.CompareTag("EnemyBox"))
        {
            // EnemyBox 위치를 기준으로 반경 3 내에서 뚫기
            DeformThisPlane(other.transform.position, 0.6f);
        }
    }

    // Plane을 변형하는 함수 (기존)
    public void DeformThisPlane(Vector3 PositionToDeform)
    {
        PositionToDeform = transform.InverseTransformPoint(PositionToDeform);

        for (int i = 0; i < verts.Length; i++)
        {
            float dist = (verts[i] - PositionToDeform).sqrMagnitude;

            if (dist < Radius * Radius) // 범위 체크 (SqrMagnitude로 성능 개선)
            {
                verts[i] -= Vector3.up * Power; // 메시 변형
            }
        }

        PlaneMesh.vertices = verts;
        PlaneMesh.RecalculateNormals(); // 법선 계산 (변형 후 메시 정상 표시)
    }

    // EnemyBox용 Deform 함수 (반경 3으로 뚫기)
    public void DeformThisPlane(Vector3 PositionToDeform, float deformRadius)
    {
        PositionToDeform = transform.InverseTransformPoint(PositionToDeform);

        for (int i = 0; i < verts.Length; i++)
        {
            float dist = (verts[i] - PositionToDeform).sqrMagnitude;

            if (dist < deformRadius * deformRadius) // 반경 3 체크
            {
                verts[i] -= Vector3.up * Power; // 메시 변형
            }
        }

        PlaneMesh.vertices = verts;
        PlaneMesh.RecalculateNormals(); // 법선 계산 (변형 후 메시 정상 표시)
    }
}
