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
        transform.localPosition = Vector3.Scale(Input.acceleration, new Vector3(3,5,0));
    }
}
 