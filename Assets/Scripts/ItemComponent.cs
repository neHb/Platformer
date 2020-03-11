using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemComponent : MonoBehaviour, IObjectDestroyer
{
    [SerializeField] private ItemType type;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    private Item item;
    public Item Item
    {
        get { return item; }
    }

    public void Destroy(GameObject gameObject)
    {
        animator.SetTrigger("StartDestroy");
    }

    private void Start()
    {
        item = GameManager.Instance.itemDataBase.GetItemOfType((int)type);
        spriteRenderer.sprite = item.Icon;
        GameManager.Instance.itemsContainer.Add(gameObject, this);
    }

    public void EndDestroy()
    {
        MonoBehaviour.Destroy(gameObject);
    }

}


public enum ItemType
{
    ForcePotion = 0, DamagePotion = 1, ArmorPotion = 2
}