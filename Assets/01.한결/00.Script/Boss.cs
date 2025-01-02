using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f; 

    private void Update()
    {
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;
    }
}
