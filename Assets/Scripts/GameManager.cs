namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    public struct KeyDowns
    {
        public bool up, down, right, left, shift, attack;

        public KeyDowns(bool _up, bool _down, bool _right, bool _left, bool _shift, bool _attack)
        {
            up = _up;
            down = _down;
            right = _right;
            left = _left;
            shift = _shift;
            attack = _attack;
        }
    };

    public delegate Vector3 Movement();

    public class GameManager : MonoBehaviour
    {
        public BulletManager bulletManager;
        public Player player;
        public UIManager uiManager;
        // Start is called before the first frame update
        void Start()
        {
            player = FindObjectOfType<Player>();
            bulletManager = FindObjectOfType<BulletManager>();
            uiManager = FindObjectOfType<UIManager>();

            player.Initialize();
            bulletManager.Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            bool up, down, right, left, shift, attack, space;

            up = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            down = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
            right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
            left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
            shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            attack = false;
            space = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0);

            KeyDowns keys = new KeyDowns(up, down, right, left, shift, attack);

            player.Move(keys);
            if (space)
            {
                bulletManager.MakeCircleBullet(20);
            }
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
