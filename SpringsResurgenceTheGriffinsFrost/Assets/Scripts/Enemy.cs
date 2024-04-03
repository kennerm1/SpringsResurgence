using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Info")]
    public string enemyName;
    public float moveSpeed;

    public int curHp;
    public int maxHp;

    public float chaseRange;
    public float attackRange;

    private PlayerController targetPlayer;
    private HeaderInfo headerInfo;

    public float playerDetectRate = 0.2f;
    private float lastPlayerDetectTime;

    public string objectToSpawnOnDeath;

    [Header("Attack")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;

    [Header("Components")]
    public HeaderInfo healthBar;
    public SpriteRenderer sr;
    public Rigidbody2D rig;

    void Start()
    {
        healthBar.Initialize(enemyName, maxHp);
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPlayer != null)
        {
            // calculate the distance
            float dist = Vector3.Distance(transform.position, targetPlayer.transform.position);

            // if we're able to attack, do so
            if (dist < attackRange && Time.time - lastAttackTime >= attackRange)
                Attack();

            // otherwise, do we move after the player?
            else if (dist > attackRange)
            {
                Vector3 dir = targetPlayer.transform.position - transform.position;
                rig.velocity = dir.normalized * moveSpeed;
            }

            else
            {
                rig.velocity = Vector2.zero;
            }
        }
        DetectPlayer();
    }

    // attacks the targeted player
    void Attack()
    {
        lastAttackTime = Time.time;
        //targetPlayer.photonView.RPC("TakeDamage", targetPlayer.photonPlayer, damage);
        TakeDamage(damage);
    }

    void DetectPlayer()
    {
        if (Time.time - lastPlayerDetectTime > playerDetectRate)
        {
            lastPlayerDetectTime = Time.time;
            // loop through all the players
            /*foreach (PlayerController player in GameManager.instance.players)
            {
                // calculate distance between us and the player
                float dist = Vector2.Distance(transform.position, player.transform.position);
                if (player == targetPlayer)
                {
                    if (dist > chaseRange)
                        targetPlayer = null;
                }
                else if (dist < chaseRange)
                {
                    if (targetPlayer == null)
                        targetPlayer = player;
                }
            }*/
        }
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;
        // update the health bar
        //healthBar.photonView.RPC("UpdateHealthBar", RpcTarget.All, curHp);
        headerInfo.UpdateHealthBar(curHp);
        if (curHp <= 0)
            Die();
        else
        {
        //photonView.RPC("FlashDamage", RpcTarget.All);
            FlashDamage();
        }
    }

    void FlashDamage()
    {
        StartCoroutine(DamageFlash());
        IEnumerator DamageFlash()
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            sr.color = Color.white;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

}