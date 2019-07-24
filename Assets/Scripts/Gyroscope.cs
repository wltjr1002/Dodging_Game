using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyroscope : MonoBehaviour
{
    private BossShooter.GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<BossShooter.GameManager>();
    }
   
    void Update() {
        Vector3 pos = transform.position;
        Vector3 acc = Input.gyro.userAcceleration;
        Vector3 acc2 = Input.acceleration;
        transform.localPosition = Vector3.Scale(Input.acceleration, new Vector3(3,5,0));
        gameManager.uiManager.SetText("Debug",acc.ToString()+acc2.ToString());
    }
}
