using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private Animator animator;
    public float CurrentHealth
    {
        get { return health; }
    }

    private void Start()
    {
        GameManager.Instance.healthContainer.Add(gameObject, this);
    }

    public void TakeHit(int damage)
    {
        health -= damage;
        if (animator != null)
        animator.SetTrigger("GetDamage");
        if (health <= 0)
            Destroy(gameObject);
    }

    public void SetHealth(int bonusHealth)
    {
        health += bonusHealth;

        if (health > 100)
            health = 100;
    }
}
