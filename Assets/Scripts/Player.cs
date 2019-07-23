namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Player : MonoBehaviour
    {
        public float speed;
        public void Initialize()
        {
            speed = 4;
        }
        
        public void Move(KeyDowns keys)
        {
            int dirX = (keys.right == keys.left) ? 0 : (keys.right ? 1 : -1);
            int dirY = (keys.up == keys.down) ? 0 : (keys.up ? 1 : -1);
            float velocity = keys.shift ? (speed*0.5f) : speed;
            float dt = Time.deltaTime;

            Vector3 newPosition = transform.localPosition + new Vector3(dirX * dt * velocity, dirY * dt * velocity, 0);
            transform.localPosition = newPosition;
        }
    }
}