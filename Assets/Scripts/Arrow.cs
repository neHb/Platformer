using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour, IObjectDestroyer
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private TriggerDamage triggerDamage;
    [SerializeField] private float force;
    [SerializeField] private float lifeTime;


    private Player player;
    public float Force
    {
        get { return force; }
        set { force = value; }
    }


    public void Destroy(GameObject gameObject)
    {
        player.ReturnArrowToPool(this);
    }


    public void SetImpulse(Vector2 direction, float shootForce, int damageBonus, Player player)
    {
        this.player = player;
        triggerDamage.Init(this);
        triggerDamage.Parent = player.gameObject;
        triggerDamage.Damage = damageBonus;
        rigidbody.AddForce(direction * shootForce, ForceMode2D.Impulse);
        if (force * shootForce < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        StartCoroutine(StartLife());
    }



    private IEnumerator StartLife()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
        yield break;
    }
}
