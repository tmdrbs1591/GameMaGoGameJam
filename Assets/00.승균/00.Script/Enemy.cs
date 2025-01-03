using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

public enum Type
{
    Enemy,
    CryStal,
    Wall,
    Shop,
    Boss
}
public class Enemy : MonoBehaviour
{
    public Type currentType;
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
            if (goods != null)
                ren.material = dieMaterial;
            StartCoroutine(TimeSlow());

            FeverManager.instance.feverValue++;
            GameManager.instance.drill.combo++;

            GameManager.instance.drill.comboTextAnim.SetTrigger("Kill");
            GameManager.instance.drill.ComboStart();
            if (currentType == Type.Shop)
            {
                GameManager.instance.OpenShop();
                Debug.Log("¤·¤·¤·");
            }
            for (int i = 0; i < 5; i++)
            {
                if (goods == null)
                    return;
                Instantiate(goods, transform.position, Quaternion.identity);
            }
            if (currentType == Type.CryStal)
            {
                AudioManager.instance.PlaySound(transform.position, 2, Random.Range(1.3f, 1.7f), 1f);
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

        if (currentType == Type.Enemy)
        {
            AudioManager.instance.PlaySound(transform.position, 0, Random.Range(1.3f, 1.7f), 1f);
        }
        else if (currentType == Type.CryStal)
        {
            AudioManager.instance.PlaySound(transform.position, 1, Random.Range(1.3f, 1.7f), 1f);
        }
       else  if (currentType == Type.Wall)
        {
            AudioManager.instance.PlaySound(transform.position, 0, Random.Range(1.3f, 1.7f), 1f);
        }
        else
        {
            AudioManager.instance.PlaySound(transform.position, 0, Random.Range(1.3f, 1.7f), 1f);
        }
    }
        IEnumerator TimeSlow()
    {
            Time.timeScale = 0.1f;
            yield return new WaitForSecondsRealtime(0.2f);
            Time.timeScale = 1;
        if (goods != null)
        Destroy(Instantiate(dieEffect, transform.position, Quaternion.identity), 3f);
        Destroy(gameObject);

    }

}
