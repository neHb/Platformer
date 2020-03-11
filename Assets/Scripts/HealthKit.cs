using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : MonoBehaviour
{
    [SerializeField] private int healthKit;
    [SerializeField] private Animator animator;


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (GameManager.Instance.healthContainer.ContainsKey(col.gameObject))
        {
            var health = GameManager.Instance.healthContainer[col.gameObject];
            health.SetHealth(healthKit);
            StartDestroy();
        }
    }

    public void StartDestroy()
    {
        animator.SetTrigger("StartDestroy");
    }

    public void EndDestroy()
    {
        Destroy(gameObject);
    }
}
