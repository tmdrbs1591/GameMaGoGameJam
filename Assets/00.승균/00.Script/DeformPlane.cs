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
        // 'Drill' �±׸� ���� ��ü�� �浹�ϸ� ������ ����
        if (other.CompareTag("Drill"))
        {
            DeformThisPlane(other.transform.position);
        }

        // 'EnemyBox' �±׸� ���� ��ü�� �浹�ϸ� ���� 3���� �ձ�
        if (other.CompareTag("EnemyBox"))
        {
            // EnemyBox ��ġ�� �������� �ݰ� 3 ������ �ձ�
            DeformThisPlane(other.transform.position, 0.6f);
        }
    }

    // Plane�� �����ϴ� �Լ� (����)
    public void DeformThisPlane(Vector3 PositionToDeform)
    {
        PositionToDeform = transform.InverseTransformPoint(PositionToDeform);

        for (int i = 0; i < verts.Length; i++)
        {
            float dist = (verts[i] - PositionToDeform).sqrMagnitude;

            if (dist < Radius * Radius) // ���� üũ (SqrMagnitude�� ���� ����)
            {
                verts[i] -= Vector3.up * Power; // �޽� ����
            }
        }

        PlaneMesh.vertices = verts;
        PlaneMesh.RecalculateNormals(); // ���� ��� (���� �� �޽� ���� ǥ��)
    }

    // EnemyBox�� Deform �Լ� (�ݰ� 3���� �ձ�)
    public void DeformThisPlane(Vector3 PositionToDeform, float deformRadius)
    {
        PositionToDeform = transform.InverseTransformPoint(PositionToDeform);

        for (int i = 0; i < verts.Length; i++)
        {
            float dist = (verts[i] - PositionToDeform).sqrMagnitude;

            if (dist < deformRadius * deformRadius) // �ݰ� 3 üũ
            {
                verts[i] -= Vector3.up * Power; // �޽� ����
            }
        }

        PlaneMesh.vertices = verts;
        PlaneMesh.RecalculateNormals(); // ���� ��� (���� �� �޽� ���� ǥ��)
    }
}
