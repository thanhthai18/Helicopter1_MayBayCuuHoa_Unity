using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTank_HelicopterMinigame1 : MonoBehaviour
{
    private void Start()
    {
        transform.DOMoveY(-7.3f, 8).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        });
    }
}
