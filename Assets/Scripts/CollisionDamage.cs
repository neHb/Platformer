using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool isDestroyAfterCollision;
    private IObjectDestroyer destroyer;
    private Health health;
    private float direction;
    private GameObject parent;
    public GameObject Parent
    {
        get { return parent; }
        set { parent = value; }
    }
    public float Direction
    {
        get { return direction;  }
    }
    public float _Damage
    {
        get { return damage; }
        set
        {
            if (value <= 1000 && value > 0)
                damage = (int)value;
        }
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject == parent)
            return;
        if (GameManager.Instance.healthContainer.ContainsKey(col.gameObject))
        {
            health = GameManager.Instance.healthContainer[col.gameObject];
            direction = ((col.transform.position - transform.position).x);
            animator.SetFloat("Direction", Mathf.Abs(direction));
        }

    }

    public void SetDamage()
    {
        if (health != null)
            health.TakeHit(damage);
        health = null;
        direction = 0;
        animator.SetFloat("Direction", 0.0f);
    }

}
