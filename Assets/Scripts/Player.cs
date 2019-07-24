namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Player : MonoBehaviour
    {
        public float speed;
        private float width;
        private float height;
    
        public void Initialize()
        {
            speed = 4;
            
            Sprite sprite = GetComponent<SpriteRenderer>().sprite;
            width =  sprite.rect.size.x;
            height =  sprite.rect.size.y;
        }
        
        public void Move(KeyDowns keys)
        {
            int dirX = (keys.right == keys.left) ? 0 : (keys.right ? 1 : -1);
            int dirY = (keys.up == keys.down) ? 0 : (keys.up ? 1 : -1);
            //float curVelocity = keys.shift ? (speed*0.5f) : speed;
            float curVelocity = FindObjectOfType<GameManager>().uiManager.GetSliderValue();
            float dt = Time.deltaTime;
            float ds = curVelocity * dt;
            //Vector3 dsVector = new Vector3(dirX * dt * curVelocity, dirY * dt * curVelocity, 0);
            Vector3 dsVector = Vector3.Scale(Input.acceleration, new Vector3(ds, ds, 0));

            transform.localPosition = ClampPosition(transform.localPosition + dsVector);
        }

        private Vector3 ClampPosition(Vector3 position)
        {
            //float x = Mathf.Clamp(position.x, -3f+width/2f, 3f-width/2f);
            //float y = Mathf.Clamp(position.y, -5f+height/2f, 5f-height/2f);
            float x = Mathf.Clamp(position.x, -3f, 3f);
            float y = Mathf.Clamp(position.y, -5f, 5f);
            float z = position.z;
            Vector3 clampedPosition = new Vector3(x, y, z);
            return clampedPosition;
        }
    }
}