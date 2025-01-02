using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float maxHP;
    [SerializeField]
    public float currentHP;

    [SerializeField] Material dieMaterial;

    public GameObject dieEffect;
    public GameObject goods;

    public Slider hpbar;

    bool isdie;
    public Renderer ren;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        hpbar.value = currentHP/maxHP;
        Die();
    }

    void Die()
    {
        if (currentHP <= 0 && !isdie)
        {
            CameraShake.instance.ShakeCamera(12f, 0.5f);
            ren.material = dieMaterial;
            StartCoroutine(TimeSlow());

            for (int i = 0; i < 5; i++)
            {
                Instantiate(goods, transform.position, Quaternion.identity);
            }

            isdie = true;
        }
    }
    private void OnDestroy()
    {
        GameManager.instance.drill.isCollidingWithEnemy = false;
        GameManager.instance.drill.ResetCameraZoom();
    }
    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        CameraShake.instance.ShakeCamera(7f, 0.1f);

    }

    IEnumerator TimeSlow()
    {
            Time.timeScale = 0.1f;
            yield return new WaitForSecondsRealtime(0.2f);
            Time.timeScale = 1;
        Destroy(Instantiate(dieEffect, transform.position, Quaternion.identity), 3f);
        Destroy(gameObject);

    }

}
