using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] Cell[] cells;
    [SerializeField] private int cellCount;
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Transform rootParent;


    void Init()
    {
        cells = new Cell[cellCount];
        for (int i = 0; i < cellCount; i++)
        {
            cells[i] = Instantiate(cellPrefab, rootParent);
            cells[i].UpdateCell += UpdateInv;
        }

        cellPrefab.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (cells == null || cells.Length <= 0)
            Init();
        UpdateInv();
    }

    public void UpdateInv()
    {
        var inventory = GameManager.Instance.inventory;
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Init(null);
        }
        for (int i = 0; i < inventory.Items.Count; i++)
        {
            if (i < cells.Length)
                cells[i].Init(inventory.Items[i]);
        }
    }
}
