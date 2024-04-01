using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;

    private int currentHealth;
    private bool _hasDied;

    /*void Start()
    {
        currentHealth = maxHealth;
    }*/

    IEnumerator Start()
    {
        currentHealth = maxHealth;

        while (_hasDied == false)
        {
            float delay = UnityEngine.Random.Range(5, 30);
            yield return new WaitForSeconds(delay);
            if (_hasDied == false)
            {
                GetComponent<AudioSource>().Play();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= maxHealth)
        {
            Die();
        }
    }

    void Die()
    {
        _hasDied = true;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        gameObject.SetActive(false);
    }

}