using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class picsScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem particlesDeath;
    [SerializeField] private GameObject spawn;

    void Start()
    {
        particlesDeath.Stop();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "traps")
        {
            StartCoroutine(DeathCoroutine());
        }
    }

    IEnumerator DeathCoroutine()
    {
        particlesDeath.gameObject.transform.position = new Vector2(gameObject.transform.position.x,
            particlesDeath.gameObject.transform.position.y);
        var graphic = gameObject.GetComponent<SpriteRenderer>();
        var rb = gameObject.GetComponent<Rigidbody2D>();
        graphic.enabled = false; 
        rb.simulated = false;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        particlesDeath.Play();
        yield return new WaitForSeconds(2);
        gameObject.transform.position = new Vector2(spawn.transform.position.x, spawn.transform.position.y);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        graphic.enabled = true;
        rb.simulated = true;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}