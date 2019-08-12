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

        public override string ToString()
        {
            return string.Format(
                "UP:{0,6}, DOWN:{1,6}, RIGHT:{2,6}\nLEFT:{3,6}, SHIFT:{4,6}, SPACE:,{5,6}",
                up.ToString(), down.ToString(), right.ToString(), left.ToString(), shift.ToString(), space.ToString());
        }
    };

    public enum ControlMode
    {
        GroundGyro,
        GroundTouch,
        AirGyro
    };

    public class GameManager : MonoBehaviour
    {
        public Player player;
        public Magician magician;
        public BulletManager bulletManager;
        public EnemyManager enemyManager;
        public UIManager uiManager;
        private const float UI_UPDATE_FREQENCY = 0.2f;
        private float lastUIUpdate;
        [SerializeField]
        private float sensitivity;
        private KeyDowns keyDowns;
        void Awake()
        {
            int width = Screen.width;
            Screen.SetResolution(width, (int)(width / 16f * 9f), true);
        }

        void Start()
        {
            player = FindObjectOfType<Player>();
            magician = FindObjectOfType<Magician>();
            bulletManager = FindObjectOfType<BulletManager>();
            enemyManager = FindObjectOfType<EnemyManager>();
            uiManager = FindObjectOfType<UIManager>();
            lastUIUpdate = float.MinValue;

            InitializeComponents();
        }

        void Update()
        {
            // input 처리
            keyDowns = GetKeyDowns();

            // 플레이어 이동
            MovePlayer(keyDowns);

            // 게임오버 처리
            if (IsGameOver()) Gameover();

            // UI 설정
            SetUIs();
        }
        void Gameover()
        {
            player.ResetPostion();
            magician.Reset();
            bulletManager.DestroyAllBullet();
            foreach (Enemy enemy in FindObjectsOfType<Enemy>())
            {
                enemy.StopAllCoroutines();
                Destroy(enemy.gameObject);
            }
        }
        void InitializeComponents()
        {
            player.Initialize();
            bulletManager.Initialize();
            enemyManager.Initialize();
            uiManager.Initialize();
            magician.Initialize();
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

        private void SetUIs()
        {
            if (Time.time < lastUIUpdate + UI_UPDATE_FREQENCY) return;
            else lastUIUpdate = Time.time;
            uiManager.SetText("FPS", "FPS: " + ((int)(1f / Time.deltaTime)).ToString("##"));
            uiManager.SetText("Debug", Input.acceleration.ToString());
            uiManager.SetText("Log", keyDowns.ToString());
        }

        private KeyDowns GetKeyDowns()
        {
            bool up, down, right, left, shift, space;

            up = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.acceleration.y > sensitivity;
            down = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.acceleration.y < -sensitivity;
            right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.acceleration.x > sensitivity;
            left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.acceleration.x < -sensitivity;
            shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.Mouse0);
            space = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0);

            KeyDowns keyDowns = new KeyDowns(up, down, right, left, shift, space);
            return keyDowns;
        }
    }
}
