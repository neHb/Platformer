using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public int coinsCount;
    public Text coinsText;
    public BuffReciever buffReciever;
    public List<Item> items;
    public List<Item> Items
    {
        get { return items; }
    }
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.inventory = this;
        coinsText.text = coinsCount.ToString();
        items = new List<Item>();
    }

    public static PlayerInventory Instance { get; set; }
}
