using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager Instance { get; private set; }
    #endregion

    [SerializeField] private Text soundText;
    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject inventoryPanel;
    private int sound = 1;

    public Dictionary<GameObject, Health> healthContainer;
    public Dictionary<GameObject, Coin> coinContainer;
    public Dictionary<GameObject, BuffReciever> buffRecieverContainer;
    public Dictionary<GameObject, ItemComponent> itemsContainer;
    public PlayerInventory inventory;
    public ItemBase itemDataBase;
    private void Awake()
    {
        Instance = this;
        healthContainer = new Dictionary<GameObject, Health>();
        coinContainer = new Dictionary<GameObject, Coin>();
        buffRecieverContainer = new Dictionary<GameObject, BuffReciever>();
        itemsContainer = new Dictionary<GameObject, ItemComponent>();
        if (PlayerPrefs.HasKey("Sound"))
            sound = PlayerPrefs.GetInt("Sound");
    }

    public void OnClickPause()
    {
            Time.timeScale = 0;
    }

    public void OnClickContinue()
    {
            Time.timeScale = 1;
    }

    public void OnClickExitInMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void OnClickSettingSound()
    {
        if (sound == 0)
        {
            soundText.text = "Звук Вкл";
            PlayerPrefs.SetInt("Sound", 1);
            sound = 1;
        }
        else if (sound == 1)
        {
            soundText.text = "Звук Выкл";
            PlayerPrefs.SetInt("Sound", 0);
            sound = 0;
        }
    }
}
