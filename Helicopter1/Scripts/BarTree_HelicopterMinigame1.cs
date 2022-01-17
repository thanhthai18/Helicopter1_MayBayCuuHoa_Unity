using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarTree_HelicopterMinigame1 : MonoBehaviour
{
    public Image fill;
    public float currentValueFX;
    public float currentValue;
    public float maxValue;
    public float ratio;
    public BarFire_HelicopterMinigame1 barFire;


    private void Start()
    {
        currentValue = 1;
        currentValueFX = 1;
        maxValue = 1;
        ratio = (1 - currentValueFX) / maxValue;
        fill.fillAmount = ratio;
        StartCoroutine(ChangeFill());
    }

    public IEnumerator ChangeFill()
    {
        while (currentValue > 0)
        {
            yield return new WaitForSeconds(5);
            currentValue -= (1 - barFire.ratio) / 10;
            if(currentValue <= 0)
            {
                GameController_HelicopterMinigame1.instance.Lose();
                StopAllCoroutines();
            }
        }
    }

    private void Update()
    {
        if (currentValueFX > currentValue)
        {
            currentValueFX -= Time.deltaTime;
            if (currentValueFX < currentValue)
            {
                currentValueFX = currentValue;
            }
        }
        else if (currentValueFX < currentValue)
        {
            currentValueFX += Time.deltaTime;
            if (currentValueFX > currentValue)
            {
                currentValueFX = currentValue;
            }
        }

        if (!GameController_HelicopterMinigame1.instance.isLose && !GameController_HelicopterMinigame1.instance.isWin)
        {
            ratio = (1 - currentValueFX) / maxValue;
            fill.fillAmount = ratio;
        }
    }


}
