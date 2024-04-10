using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;
    private float dirX = 0f;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private LayerMask jumpableGround;

    private enum MovementState { idle, running, jumping, runningR }

    public int damage;
    public float attackRange;
    public float attackRate;
    private float lastAttackTime;
    public int curHp;
    public int maxHp;
    public bool dead;
    public int item;
    public HealthBar healthBar;

    [SerializeField] AudioClip[] clips;

    private void OnMouseDown()
    {
        int index = UnityEngine.Random.Range(0, clips.Length);
        AudioClip clip = clips[index];
        GetComponent<AudioSource>().PlayOneShot(clip);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        healthBar.SetMaxHealth(maxHp);
    }

    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }

        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime > attackRate)
            Attack();

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;
        if (dirX < 0f)
        {
            state = MovementState.running;
            //sprite.flipX = false;
        }

        else if (dirX > 0f)
        {
            state = MovementState.runningR;
            //sprite.flipX = true;
        }

        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        /*else if (rb.velocity.y < -.1)
        {
            state = MovementState.jumping;
        }*/

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        // calculate the direction
        Vector3 dir = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
        // shoot a raycast in the direction
        RaycastHit2D hit = Physics2D.Raycast(transform.position + dir, dir, attackRange);
        // did we hit an enemy?
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Enemy"))
        {
            // get the enemy and damage them
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            //enemy.photonView.RPC("TakeDamage", RpcTarget.MasterClient, damage);
            enemy.TakeDamage(damage);
        }
        else if (hit.collider != null && hit.collider.gameObject.CompareTag("Boss"))
        {
            //Debug.Log("yo mama");
            Boss boss = hit.collider.GetComponent<Boss>();
            boss.TakeDamage(damage);
        }
        // play attack animation
        anim.SetTrigger("Attack");
    }

    public void Heal(int amountToHeal)
    {
        curHp = Mathf.Clamp(curHp + amountToHeal, 0, maxHp);
        // update the health bar
        healthBar.SetHealth(curHp);
    }

    public void GiveItem(int itemToGive)
    {
        item += itemToGive;

        // update the ui
        GameUI.instance.UpdateItemText(item);
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;
        // update the health bar
        healthBar.SetHealth(curHp);
        if (curHp <= 0)
        { 
            Die();
        }
        else
        {
            FlashDamage();
        }
    }

    void FlashDamage()
    {
        StartCoroutine(DamageFlash());
        IEnumerator DamageFlash()
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            sprite.color = Color.white;
        }
    }

    private void Die()
    {
        Debug.Log("OwO");
        rb.bodyType = RigidbodyType2D.Static;
        Destroy(gameObject);
        RestartLevel();
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /*private void OnMouseDown()
    {
        GetComponent<AudioSource>().Play();
    }*/
}