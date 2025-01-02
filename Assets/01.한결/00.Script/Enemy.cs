using UnityEngine;

public class Enemys : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f; 

    private void Update()
    {
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;
    }
}
