namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Player : MonoBehaviour
    {

        [SerializeField]
        private GameObject hitPoint;
        public float speed;
        public ControlMode controlMode;
        private Vector3 InitialPosition;
        private Vector2 playerSize;
        private Vector2 screenSize;

        #region Jump
        public float jumpDuration;
        public float jumpSpeed;
        private const int MAXJUMP = 2;
        private int jumpState; // 0: Ground, 1: Ascending, 2: Timeout Decending, 3: Decending
        private float jumpStartTime;
        #endregion

        public void Initialize()
        {
            Sprite sprite = GetComponent<SpriteRenderer>().sprite;
            Camera camera = FindObjectOfType<Camera>();

            // 플레이어와 화면의 크기 계산
            playerSize = sprite.rect.size / sprite.pixelsPerUnit;
            screenSize = camera.ViewportToWorldPoint(new Vector3(1, 1, 0));
            Debug.Log(playerSize.ToString() + screenSize.ToString());
            // 플레이어 초기 위치 설정
            InitialPosition = new Vector3(0, playerSize.y - screenSize.y, 0);
            transform.localPosition = InitialPosition;
            jumpState = 0;

            // 플레이어 피격포인트 스프라이트 ON/OFF
            hitPoint.SetActive(false);
        }

        public void Move(KeyDowns keys)
        {
            // 점프 관련 계산
            bool space = keys.space;
            bool ground = GroundCheck();
            bool timeout = Time.time > jumpStartTime + jumpDuration;
            int prevstate = jumpState;
            switch (jumpState % 4)
            {
                case 0: // Ground
                    {
                        if (space)
                        {
                            jumpState = 1;
                            jumpStartTime = Time.time;
                        }
                        break;
                    }
                case 1: // Ascending
                    {
                        if (!space)
                        {
                            jumpState += 2;
                            jumpStartTime = Time.time;
                        }
                        else if (timeout)
                        {
                            jumpState += 1;
                            jumpStartTime = Time.time;
                        }
                        break;
                    }
                case 2: // Timeout Decending
                    {
                        if (!space) { jumpState += 1; }
                        else if (ground) { jumpState = 0; }
                        break;
                    }
                case 3: // Decending
                    {
                        if (space && (jumpState + 1) / 4 < MAXJUMP)
                        {
                            jumpState += 2;
                            jumpStartTime = Time.time;
                        }
                        else if (ground) { jumpState = 0; }
                        break;
                    }
                default: break;
            }
            //if (jumpState != prevstate) Debug.Log(space.ToString() + jumpState);


            // 기본상수 계산
            int dirX = (keys.right == keys.left) ? 0 : (keys.right ? 1 : -1);
            int dirY = (keys.up == keys.down) ? 0 : (keys.up ? 1 : -1);
            float curVelocity = keys.shift ? (speed * 0.5f) : speed;
            float dt = Time.deltaTime;
            float ds = curVelocity * dt;

            // 움직임 벡터 계산
            Vector3 dsVector;
            switch (controlMode)
            {
                case ControlMode.GroundGyro:
                    {
                        float vRatio;
                        if (jumpState % 4 == 0)
                        {
                            dirY = 0;
                            vRatio = 0;
                        }
                        else if (jumpState % 4 == 1)
                        {
                            dirY = 1;
                            vRatio = 1f - (Time.time - jumpStartTime) / jumpDuration;
                        }
                        else if (jumpState % 4 == 2)
                        {
                            dirY = -1;
                            vRatio = Mathf.Min((Time.time - jumpStartTime) / jumpDuration * 2, 1f);
                        }
                        else
                        {
                            dirY = -1;
                            vRatio = Mathf.Min((Time.time - jumpStartTime) / jumpDuration * 2, 1f);
                        }

                        dsVector = new Vector3(dirX * dt * curVelocity, dirY * dt * curVelocity * vRatio * jumpSpeed, 0);
                        break;
                    }
                case ControlMode.GroundTouch:
                    {
                        dsVector = new Vector3(dirX * dt * curVelocity, dirY * dt * curVelocity, 0);
                        break;
                    }
                case ControlMode.AirGyro:
                    {
                        dsVector = new Vector3(dirX * dt * curVelocity, dirY * dt * curVelocity, 0);
                        break;
                    }
                default:
                    {
                        dsVector = new Vector3(dirX * dt * curVelocity, dirY * dt * curVelocity, 0);
                        break;
                    }
            }

            // 스프라이트 조정
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.flipX = dirX < 0;
            Animator animator = GetComponent<Animator>();
            if (dirY == 0)
            {
                if (dirX == 0) animator.Play("Stop");
                else animator.Play("Walk");
            }
            else if(dirY == 1)
            {
                animator.Play("Jump");
            }
            else animator.Play("Fall");


            // 캐릭터 위치 조정
            transform.localPosition = ClampPosition(transform.localPosition + dsVector);
        }

        public void ResetPostion()
        {
            transform.localPosition = new Vector3(0, -1 * screenSize.y + playerSize.y, 0);
        }

        private bool GroundCheck()
        {
            return transform.localPosition.y <= InitialPosition.y;
        }

        private Vector3 ClampPosition(Vector3 position)
        {
            float x = Mathf.Clamp(position.x, playerSize.x - screenSize.x, screenSize.x - playerSize.x);
            float y = Mathf.Clamp(position.y, playerSize.y - screenSize.y, screenSize.y - playerSize.y);
            float z = position.z;
            Vector3 clampedPosition = new Vector3(x, y, z);
            return clampedPosition;
        }
    }
}