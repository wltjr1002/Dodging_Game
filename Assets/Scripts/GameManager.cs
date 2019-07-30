namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public struct KeyDowns
    {
        public bool up, down, right, left, shift, space;

        public KeyDowns(bool _up, bool _down, bool _right, bool _left, bool _shift, bool _space)
        {
            up = _up;
            down = _down;
            right = _right;
            left = _left;
            shift = _shift;
            space = _space;
        }
    };

    public delegate Vector3 Movement();

    public class GameManager : MonoBehaviour
    {
        public Player player;
        public BulletManager bulletManager;
        public EnemyManager enemyManager;
        public UIManager uiManager;
        private const float UI_UPDATE_FREQENCY = 0.2f;
        private float lastUIUpdate;
        [SerializeField]
        private float sensitivity;
        void Awake()
        {
            int width = Screen.width;
            Screen.SetResolution(width, (int)(width / 9f * 16f), true);
        }

        void Start()
        {
            player = FindObjectOfType<Player>();
            bulletManager = FindObjectOfType<BulletManager>();
            enemyManager = FindObjectOfType<EnemyManager>();
            uiManager = FindObjectOfType<UIManager>();
            lastUIUpdate = float.MinValue;

            InitializeComponents();
            StartCoroutine(Makebullets());
        }

        // Update is called once per frame
        void Update()
        {
            // input 처리
            KeyDowns keyDowns = GetKeyDowns();

            // bullet 생성
            //if (keyDowns.space) Makebullets();

            // player 이동
            MovePlayer(keyDowns);

            if (IsGameOver())
            {
                // gameOver 처리
                player.ResetPostion();
                bulletManager.DestroyAllBullet();
            }

            SetUIs();

            
        }

        void InitializeComponents()
        {
            player.Initialize();
            bulletManager.Initialize();
            enemyManager.Initialize();
            uiManager.Initialize();
        }

        private void MovePlayer(KeyDowns keys)
        {
            player.Move(keys);
        }

        private bool IsGameOver()
        {
            Vector3 playerPosition = player.transform.localPosition;
            return bulletManager.isBulletInPosition(playerPosition);
        }

        private IEnumerator Makebullets()
        {
            bulletManager.ChangeBulletSprite(0);
            bulletManager.MakeBullet(new Vector3(-3,player.transform.localPosition.y,0),Vector3.right,2);
            yield return new WaitForSeconds(1.0f);
        }

        private void SetUIs()
        {
            if(Time.time < lastUIUpdate + UI_UPDATE_FREQENCY) return;
            else lastUIUpdate = Time.time;
            uiManager.SetText("FPS", "FPS: " + ((int)(1f / Time.deltaTime)).ToString("##"));
            uiManager.SetText("Debug", Input.acceleration.ToString());
        }

        private KeyDowns GetKeyDowns()
        {
            bool up, down, right, left, shift, space;

            up = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.acceleration.y > sensitivity;
            down = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.acceleration.y < -sensitivity;
            right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.acceleration.x > sensitivity;
            left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.acceleration.x < -sensitivity;
            shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.Mouse0);
            space = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0);

            KeyDowns keyDowns = new KeyDowns(up, down, right, left, shift, space);
            return keyDowns;
        }
    }
}
