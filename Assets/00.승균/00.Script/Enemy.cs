using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float maxHP;
    [SerializeField]
    public float currentHP;

    [SerializeField] Material dieMaterial;

    public GameObject dieEffect;

    private Renderer ren;
    // Start is called before the first frame update
    void Start()
    {
        ren = gameObject.GetComponent<Renderer>();
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        Die();
    }

    void Die()
    {
        if (currentHP <= 0)
        {
            CameraShake.instance.ShakeCamera(12f, 0.5f);
            ren.material = dieMaterial;
            StartCoroutine(TimeSlow());
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
