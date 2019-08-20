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

        private bool isJumpEnabled;
        private bool isDashEnabled;
        private bool isBombEnabled;

        #region Jump
        public float jumpDuration;
        public float jumpSpeed;
        private const int MAXJUMP = 2;
        private int jumpState; // 0: Ground, 1: Ascending, 2: Timeout Decending, 3: Decending
        private float jumpStartTime;
        #endregion

        #region Dash
        [HideInInspector]
        public bool isDashing;
        [SerializeField]
        private float dashTime;
        [SerializeField]
        private float dashCooldown;
        private float dashTimeRemain;
        private float dashCooldownRemain;
        #endregion

        #region Bomb
        private float bombCooldown;
        #endregion

        public void Initialize()
        {
            Sprite sprite = GetComponent<SpriteRenderer>().sprite;
            Camera camera = FindObjectOfType<Camera>();

            // 플레이어와 화면의 크기 계산
            playerSize = sprite.rect.size / sprite.pixelsPerUnit;
            screenSize = camera.ViewportToWorldPoint(new Vector3(1, 1, 0));
            // 플레이어 초기 위치 설정
            InitialPosition = new Vector3(0, playerSize.y - screenSize.y, 0);
            transform.localPosition = InitialPosition;
            jumpState = 0;

            // 플레이어 피격포인트 스프라이트 ON/OFF
            hitPoint.SetActive(false);

            // 컨트롤모드에 따른 점프/대쉬 기능 활성화
            switch (controlMode)
            {
                case ControlMode.Gyro:
                    {
                        isJumpEnabled = true;
                        isDashEnabled = false;
                        isBombEnabled = false;
                        break;
                    }
                case ControlMode.Touch:
                    {
                        isJumpEnabled = true;
                        isDashEnabled = false;
                        isBombEnabled = false;
                        break;
                    }
                case ControlMode.Buttons:
                    {
                        isJumpEnabled = true;
                        isDashEnabled = true;
                        isBombEnabled = false;
                        break;
                    }
                default: break;
            }

        }

        public void Move(KeyDowns keys)
        {
            // 기본상수 계산
            float velocity = speed;
            float vFactor_X = (keys.right == keys.left) ? 0 : (keys.right ? 1 : -1);
            float vFactor_Y = (keys.up == keys.down) ? 0 : (keys.up ? 1 : -1);
            float deltaTime = Time.deltaTime;

            if (isJumpEnabled)// 점프 관련 계산
            {
                bool space = keys.space;
                bool ground = GroundCheck();
                bool timeout = Time.time > jumpStartTime + jumpDuration;
                int prevstate = jumpState;

                // JumpState 변경
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
                // JumpState 변경에 따른 상수 계산
                switch (jumpState % 4)
                {
                    case 0: // Ground
                        {
                            vFactor_Y = 0;
                            break;
                        }
                    case 1: // Ascending
                        {
                            vFactor_Y = 1f - (Time.time - jumpStartTime) / jumpDuration;
                            break;
                        }
                    case 2: // Timeout Decending
                        {
                            vFactor_Y = -1 * Mathf.Min((Time.time - jumpStartTime) / jumpDuration * 2, 1f);
                            break;
                        }
                    case 3: // Decending
                        {
                            vFactor_Y = -1 * Mathf.Min((Time.time - jumpStartTime) / jumpDuration * 2, 1f);
                            break;
                        }
                    default: break;
                }
                vFactor_Y *= jumpSpeed;
            }
            if (isDashEnabled)// 대쉬 관련 계산
            {
                bool shift = keys.shift;
                if (shift)
                {
                    if (isDashing) //대쉬계속
                    {
                        dashTimeRemain -= deltaTime;
                        if (dashTimeRemain < 0) // 지속시간 끝나면 대쉬종료
                        {
                            isDashing = false;
                            dashTimeRemain = 0;
                            dashCooldownRemain = dashCooldown;
                        }
                    }
                    else if (dashCooldownRemain == 0) // 대쉬 시작
                    {
                        dashTimeRemain = dashTime;
                        isDashing = true;
                    }
                }
                else if (isDashing) //대쉬종료
                {
                    isDashing = false;
                    dashTimeRemain = 0;
                    dashCooldownRemain = dashCooldown;
                }
                // 쿨다운
                if (dashCooldownRemain > 0) dashCooldownRemain = Mathf.Max(0f, dashCooldownRemain - deltaTime);
                if (isDashing) vFactor_X *= 2;
            }

            // 움직임 벡터 계산
            Vector3 dsVector;
            dsVector = new Vector3(vFactor_X * deltaTime * velocity, vFactor_Y * deltaTime * velocity, 0);

            // 스프라이트 조정
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.flipX = vFactor_X < 0;
            sprite.color = isDashing ? new Color(1, 1, 1, 0.5f) : Color.white;
            Animator animator = GetComponent<Animator>();
            
            if (vFactor_Y == 0 && vFactor_X == 0) animator.Play("Stop");
            else if (vFactor_Y == 0 && vFactor_X != 0) animator.Play("Walk");
            else if (vFactor_Y > 0) animator.Play("Jump");
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