using System.Collections;
using System.Collections.Generic;
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
            Debug.Log("oui");
        }
    }

    IEnumerator DeathCoroutine()
    {
        particlesDeath.gameObject.transform.position = new Vector2(gameObject.transform.position.x,
            particlesDeath.gameObject.transform.position.y);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        particlesDeath.Play();
        yield return new WaitForSeconds(2);
        gameObject.transform.position = new Vector2(spawn.transform.position.x, spawn.transform.position.y);
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}