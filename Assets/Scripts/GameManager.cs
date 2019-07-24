﻿namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

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
        public BulletManager bulletManager;
        public Player player;
        public UIManager uiManager;
        private const float UI_UPDATE_FREQENCY = 0.2f;
        private float lastUIUpdate;
        void Awake()
        {
            int width = Screen.width;
            Screen.SetResolution(width, (int)(width / 9f * 16f), true);
        }

        void Start()
        {
            player = FindObjectOfType<Player>();
            bulletManager = FindObjectOfType<BulletManager>();
            uiManager = FindObjectOfType<UIManager>();
            lastUIUpdate = float.MinValue;

            InitializeComponents();
        }

        // Update is called once per frame
        void Update()
        {
            // input 처리
            KeyDowns keyDowns = GetKeyDowns();

            // bullet 생성
            if (keyDowns.space) Makebullets();

            // player 이동
            MovePlayer(keyDowns);

            if (IsGameOver())
            {
                // gameOver 처리
                player.ResetPostion();
            }

            SetUIs();
        }

        void InitializeComponents()
        {
            player.Initialize();
            bulletManager.Initialize();
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

        private void Makebullets()
        {
            bulletManager.MakeCircleBullet(20);
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

            up = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.acceleration.y > 0.2f;
            down = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.acceleration.y < -0.2f;
            right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.acceleration.x > 0.2f;
            left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.acceleration.x < -0.2f;
            shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.Mouse0);
            space = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0);

            KeyDowns keyDowns = new KeyDowns(up, down, right, left, shift, space);
            return keyDowns;
        }


        [MenuItem("Util/CleanCache")]
        public static void CleanCache()
        {
            if (Caching.ClearCache())
            {
                EditorUtility.DisplayDialog("알림", "캐시가 삭제되었습니다.", "확인");
            }
            else
            {
                EditorUtility.DisplayDialog("오류", "캐시 삭제에 실패했습니다.", "확인");
            }
        }
    }
}
