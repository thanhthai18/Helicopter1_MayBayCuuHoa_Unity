using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelicopterObj_HelicopterMinigame1 : MonoBehaviour
{
    public float speed;
    public VariableJoystick variableJoystick;
    public Rigidbody2D rb;
    public Vector2 direction;
    public Camera mainCamera;
    public Vector2 maxPosCam;
    public Vector3 lastPos;
    public GameObject waterPrefab;
    public Button btnWater;
    public int bulletWater;
    public Text txtWater;



    private void Start()
    {
        speed = 10;
        bulletWater = 5;
        txtWater.text = bulletWater.ToString();
        maxPosCam = new Vector2(mainCamera.orthographicSize * Screen.width * 1.0f / Screen.height, mainCamera.orthographicSize);
        btnWater.onClick.AddListener(ClickButtonWater);

    }

    public virtual void FlipCar()
    {

        if (transform.position.x > lastPos.x)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        if (transform.position.x < lastPos.x)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }

        lastPos = transform.position;
    }

    void ClickButtonWater()
    {
        if(bulletWater > 0)
        {
            bulletWater--;
            txtWater.text = bulletWater.ToString();
            var tmpWater = Instantiate(waterPrefab, transform.position, Quaternion.identity);
            float distance = Mathf.Abs(transform.position.y - maxPosCam.y - 3);
            tmpWater.transform.DOMoveY(-maxPosCam.y - 3, 6.0f / distance).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (tmpWater != null)
                {
                    Destroy(tmpWater.gameObject);
                }
            });
        }       
    }

    void Handle_EventBonusBullet(bool active)
    {
        if (active)
        {
            bulletWater += 10;
        }
    }


    public void FixedUpdate()
    {
        if (!GameController_HelicopterMinigame1.instance.isWin && !GameController_HelicopterMinigame1.instance.isLose)
        {
            if (variableJoystick.Horizontal != 0 || variableJoystick.Vertical != 0)
            {
                // tutorial
                FlipCar();             
            }
            direction = new Vector2(variableJoystick.Horizontal, variableJoystick.Vertical * 0.7f);
            rb.velocity = direction * speed;

            transform.position = new Vector2(Mathf.Clamp(transform.position.x, -maxPosCam.x + 1.5f, maxPosCam.x - 1.5f), Mathf.Clamp(transform.position.y, -maxPosCam.y + 1, maxPosCam.y - 1));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trash"))
        {
            bulletWater++;
            txtWater.text = bulletWater.ToString();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Finish"))
        {
            GameController_HelicopterMinigame1.instance.Lose();
        }
    }

    private void OnEnable()
    {
        FireBall_HelicopterMinigame1.Event_BonusBullet += Handle_EventBonusBullet;
    }
    private void OnDisable()
    {
        FireBall_HelicopterMinigame1.Event_BonusBullet -= Handle_EventBonusBullet;
    }
}