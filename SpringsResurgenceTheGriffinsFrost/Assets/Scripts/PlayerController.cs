using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public HeaderInfo headerInfo;

    [SerializeField] AudioClip[] _clips;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
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
        }
        // play attack animation
        anim.SetTrigger("Attack");
    }

    public void Heal(int amountToHeal)
    {
        curHp = Mathf.Clamp(curHp + amountToHeal, 0, maxHp);
        // update the health bar
        //headerInfo.photonView.RPC("UpdateHealthBar", RpcTarget.All, curHp);
        headerInfo.UpdateHealthBar(curHp);
    }

    public void GiveItem(int itemToGive)
    {
        item += itemToGive;

        // update the ui
        GameUI.instance.UpdateItemText(item);
    }

    /*private void OnMouseDown()
    {
        GetComponent<AudioSource>().Play();
    }*/
}