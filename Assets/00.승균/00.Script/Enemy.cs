using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float maxHP;
    [SerializeField]
    public float currentHP;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        GameManager.instance.drill.isCollidingWithEnemy = false;
    }
    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        CameraShake.instance.ShakeCamera(7f, 0.1f);

    }
}
