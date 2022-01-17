using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall_HelicopterMinigame1 : MonoBehaviour
{
    public int spawnIndex;
    public int currentProgression;
    public int maxGrowth = 5;
    public bool isAutoGrowth;
    public float timeGrow = 0f;
    public bool isBigSize;
    public List<GameObject> fireStage = new List<GameObject>();
    public static event Action<bool> Event_BonusBullet;

    public bool IsAutoGrowth { get => isAutoGrowth; set => isAutoGrowth = value; }


    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            fireStage.Add(transform.GetChild(i).gameObject);
        }
        fireStage.ForEach(s => { s.SetActive(false); });
        if (isBigSize)
        {
            fireStage[fireStage.Count - 1].SetActive(true);
            currentProgression = fireStage.Count - 1;
            GameController_HelicopterMinigame1.instance.firePower += 3;
            GetComponent<CapsuleCollider2D>().size = new Vector2(GetComponent<CapsuleCollider2D>().size.x, GetComponent<CapsuleCollider2D>().size.y * 4);
        }
        if (!isBigSize)
        {
            int ran = UnityEngine.Random.Range(0, 2);
            fireStage[ran].SetActive(true);
            currentProgression = ran;
            GameController_HelicopterMinigame1.instance.firePower += (ran + 1);     

            if(ran == 1)
            {
                GetComponent<CapsuleCollider2D>().size = new Vector2(GetComponent<CapsuleCollider2D>().size.x, GetComponent<CapsuleCollider2D>().size.y * 2);
            }
        }

        

        CallGrow();
        GameController_HelicopterMinigame1.instance.SetPowerBar();
    }

    IEnumerator GrowFire()
    {
        while (currentProgression < 2)
        {
            yield return new WaitForSeconds(1);
            timeGrow += 1;
            if (timeGrow == 10)
            {
                timeGrow = 0;
                gameObject.transform.GetChild(currentProgression).gameObject.SetActive(false);
                currentProgression++;
                GetComponent<CapsuleCollider2D>().size = new Vector2(GetComponent<CapsuleCollider2D>().size.x, GetComponent<CapsuleCollider2D>().size.y * 2);
                gameObject.transform.GetChild(currentProgression).gameObject.SetActive(true);
                GameController_HelicopterMinigame1.instance.firePower++;
                GameController_HelicopterMinigame1.instance.SetPowerBar();
            }
        }
    }

    public void CallGrow()
    {
        StartCoroutine(GrowFire());
    }

    public void Handle_EndGame(bool active)
    {
        if (active)
        {
            StopAllCoroutines();
        }
    }

    private void ClearFireInWindow()
    {
        GameController_HelicopterMinigame1.instance.ClearFireInWindow(spawnIndex);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water") && currentProgression < 3)
        {
            GameController_HelicopterMinigame1.instance.firePower--;
            GameController_HelicopterMinigame1.instance.SetPowerBar();
            Destroy(collision.gameObject);
            IsAutoGrowth = false;
            fireStage[currentProgression].SetActive(false);
            if (currentProgression > 0)
            {
                currentProgression--;
                GetComponent<CapsuleCollider2D>().size = new Vector2(GetComponent<CapsuleCollider2D>().size.x, GetComponent<CapsuleCollider2D>().size.y / 2);
                fireStage[currentProgression].SetActive(true);
                timeGrow = 0;
                CallGrow();               
            }
            else if (currentProgression == 0)
            {
                Destroy(gameObject);
                ClearFireInWindow();
                StopAllCoroutines();
                if (GameController_HelicopterMinigame1.instance.GetCountFireLive() == 0)
                {
                    if (GameController_HelicopterMinigame1.instance.phase < 3)
                    {
                        GameController_HelicopterMinigame1.instance.phase++;
                        GameController_HelicopterMinigame1.instance.SpawnPhase();
                        Event_BonusBullet?.Invoke(true);

                    }
                    else if (GameController_HelicopterMinigame1.instance.phase == 3)
                    {
                        GameController_HelicopterMinigame1.instance.Win();
                    }
                }
            }            
        }
    }

    private void OnEnable()
    {
        GameController_HelicopterMinigame1.Event_EndGame += Handle_EndGame;
    }

    private void OnDisable()
    {
        GameController_HelicopterMinigame1.Event_EndGame -= Handle_EndGame;
    }
}
