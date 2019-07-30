namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public enum ControlMode
    {
        GroundGyro,
        GroundTouch,
        AirGyro
    };

    public class Player : MonoBehaviour
    {
        [SerializeField]
        private GameObject hitPoint;
        public float speed;
        public float jumpHeight;
        public float jumpSpeed;
        private Vector2 playerSize;
        private Vector2 screenSize;
        private bool isJumping;

        public void Initialize()
        {
            speed = 4;

            Sprite sprite = GetComponent<SpriteRenderer>().sprite;

            playerSize = sprite.rect.size;

            Camera camera = FindObjectOfType<Camera>();
            screenSize = camera.ViewportToWorldPoint(new Vector3(1, 1, 0));
            transform.localPosition = new Vector3(0, -1 * screenSize.y + playerSize.y / 100f, 0);
            Debug.Log(screenSize.ToString() + playerSize.ToString());

            hitPoint.SetActive(true);
        }

        public void Move(KeyDowns keys)
        {
            int dirX = (keys.right == keys.left) ? 0 : (keys.right ? 1 : -1);
            int dirY = (keys.up == keys.down) ? 0 : (keys.up ? 1 : -1);
            float curVelocity = keys.shift ? (speed * 0.5f) : speed;
            float dt = Time.deltaTime;
            float ds = curVelocity * dt;

            
            //Vector3 dsVector = new Vector3(dirX * dt * curVelocity, dirY * dt * curVelocity, 0);
            Vector3 dsVector = new Vector3(dirX * dt * curVelocity, 0, 0);
            //Vector3 dsVector = Vector3.Scale(Input.acceleration, new Vector3(ds, ds, 0));

            transform.localPosition = ClampPosition(transform.localPosition + dsVector);
            //hitPoint.SetActive(keys.shift);
            if (keys.space && !isJumping)
            {
                Debug.Log("JUMP");
                StartCoroutine(Jump());
            }
        }

        public void ResetPostion()
        {
            transform.localPosition = new Vector3(0, -1 * screenSize.y + playerSize.y / 100f, 0);
        }

        private Vector3 ClampPosition(Vector3 position)
        {
            //float x = Mathf.Clamp(position.x, -3f+width/2f, 3f-width/2f);
            //float y = Mathf.Clamp(position.y, -5f+height/2f, 5f-height/2f);
            float x = Mathf.Clamp(position.x, -screenSize.x, screenSize.x);
            float y = Mathf.Clamp(position.y, -screenSize.y, screenSize.y);
            float z = position.z;
            Vector3 clampedPosition = new Vector3(x, y, z);
            return clampedPosition;
        }

        public IEnumerator Jump()
        {
            isJumping = true;
            float initialY = transform.localPosition.y;
            float height = 0;
            while (height < jumpHeight)
            {
                height += Time.deltaTime * jumpSpeed;
                transform.localPosition = new Vector3(transform.localPosition.x, initialY + height, transform.localPosition.z);
                yield return null;
            }
            while (height > 0)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, initialY + height, transform.localPosition.z);
                yield return null;
                height -= Time.deltaTime * jumpSpeed;
            }
            transform.localPosition = new Vector3(transform.localPosition.x, initialY, transform.localPosition.z);
            isJumping = false;
        }
    }
}