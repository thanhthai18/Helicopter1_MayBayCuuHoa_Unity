using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarFire_HelicopterMinigame1 : MonoBehaviour
{
    public Image fill;
    public float currentValueFX;
    public float currentValue;
    public float maxValue;
    public float ratio;

    private void Start()
    {
        maxValue = 15;
        currentValue = GameController_HelicopterMinigame1.instance.firePower;
        currentValueFX = currentValue;
        ratio = (1 - currentValueFX) / maxValue;
        fill.fillAmount = ratio;
    }

    private void Update()
    {
        if (currentValue > 0)
        {
            if (currentValueFX > currentValue)
            {
                currentValueFX -= Time.deltaTime * 5;
                if (currentValueFX < currentValue)
                {
                    currentValueFX = currentValue;
                }
            }
            else if (currentValueFX < currentValue)
            {
                currentValueFX += Time.deltaTime * 5;
                if (currentValueFX > currentValue)
                {
                    currentValueFX = currentValue;
                }
            }
        }
        else
        {
            currentValueFX = 0;
        }

        if (!GameController_HelicopterMinigame1.instance.isLose && !GameController_HelicopterMinigame1.instance.isWin)
        {
            ratio = (15 - currentValueFX) / maxValue;
            fill.fillAmount = ratio;
        }
    }

}
