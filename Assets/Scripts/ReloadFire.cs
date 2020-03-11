using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadFire : MonoBehaviour
{

    [SerializeField] private Image fireTime;
    [SerializeField] private float delta;
    private float timeValue;
    private float currentTime;
    private Player player;

    private void Start()
    {
        timeValue = 3;
    }

    private void Update()
    {
        currentTime = 3;
        if (currentTime > timeValue)
            timeValue += delta;
        if (currentTime < delta)
            timeValue = currentTime;
        fireTime.fillAmount = timeValue;
    }
    public void ReloadTime()
    {

        timeValue = 0;
    }
}


