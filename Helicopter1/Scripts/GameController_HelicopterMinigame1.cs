using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController_HelicopterMinigame1 : MonoBehaviour
{
    public static GameController_HelicopterMinigame1 instance;

    public bool isWin, isLose, isBegin;
    public Camera mainCamera;
    public FireBall_HelicopterMinigame1 fireBallPrefab;
    public WaterTank_HelicopterMinigame1 waterTankPrefab;

    [SerializeField] private Transform[] fireSpawnPos;
    public bool _bigSize;
    public bool[] windowHaveFire;
    public int phase;
    public int firePower;
    public BarFire_HelicopterMinigame1 barFire;
    public static event Action<bool> Event_EndGame;




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(instance);

        isWin = false;
        isLose = false;
        isBegin = true;
    }

    private void Start()
    {
        SetSizeCamera();
        windowHaveFire = new bool[fireSpawnPos.Length];
        for (int i = 0; i < fireSpawnPos.Length; i++)
        {
            windowHaveFire[i] = false;
        }
        phase = 1;
        SpawnPhase();
        StartCoroutine(SpawnWaterTank());
    }

    void SetSizeCamera()
    {
        float f1, f2;
        f1 = 16.0f / 9;
        f2 = Screen.width * 1.0f / Screen.height;
        mainCamera.orthographicSize *= f1 / f2;
    }

    public void SetPowerBar()
    {
        barFire.currentValue = firePower;
    }
   

    public void SpawnRandomFireBall(bool bigSize)
    {
        List<int> posNotHaveFire = new List<int>();
        for (int i = 0; i < fireSpawnPos.Length; i++)
        {
            if (windowHaveFire[i] == false)
            {
                posNotHaveFire.Add(i);
            }
        }

        int randomIndexListWindow = UnityEngine.Random.Range(0, posNotHaveFire.Count);
        int randomIndex = posNotHaveFire[randomIndexListWindow];
        Vector3 spawnPos = new Vector3(fireSpawnPos[randomIndex].position.x, fireSpawnPos[randomIndex].position.y, 0);

        var fireball = Instantiate(fireBallPrefab, spawnPos, Quaternion.identity);

        windowHaveFire[randomIndex] = true;

        FireBall_HelicopterMinigame1 fireballScript = fireball.GetComponent<FireBall_HelicopterMinigame1>();
        fireballScript.spawnIndex = randomIndex;
        fireballScript.isBigSize = bigSize;
    }

    public void ClearFireInWindow(int index)
    {
        windowHaveFire[index] = false;
    }


    public int GetCountFireLive()
    {
        int result = 0;
        for (int i = 0; i < fireSpawnPos.Length; i++)
        {
            if (windowHaveFire[i]) result++;
        }
        return result;
    }

    public IEnumerator SpawnWaterTank()
    {
        while(!isLose && !isWin)
        {
            yield return new WaitForSeconds(3);
            Vector3 v = new Vector3(UnityEngine.Random.Range(-7.0f, 7.0f), 7, 0);
            Instantiate(waterTankPrefab, v, Quaternion.identity);
        }       
    }

    public void Win()
    {
        isWin = true;
        Debug.Log("Win");
        StopAllCoroutines();
    }

    public void Lose()
    {
        isLose = true;
        Debug.Log("Thua");
        StopAllCoroutines();
        Event_EndGame?.Invoke(true);
    }

    public void SpawnPhase()
    {
        if(phase == 1)
        {
            SpawnRandomFireBall(false);
            SpawnRandomFireBall(false);
            SpawnRandomFireBall(false);
            SpawnRandomFireBall(false);
            SpawnRandomFireBall(false);
        }
        else if(phase == 2)
        {
            SpawnRandomFireBall(true);
            SpawnRandomFireBall(false);
            SpawnRandomFireBall(false);
            SpawnRandomFireBall(false);
            SpawnRandomFireBall(false);
        }
        else if(phase == 3)
        {
            SpawnRandomFireBall(true);
            SpawnRandomFireBall(true);
            SpawnRandomFireBall(false);
            SpawnRandomFireBall(false);
            SpawnRandomFireBall(false);
        }        
    }
}
