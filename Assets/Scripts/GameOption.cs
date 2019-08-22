namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameOption : MonoBehaviour
    {
        private static GameOption instance;
        public ControlMode controlMode;
        void Awake()
        {
            if (!instance)
            {
                instance = this as GameOption;
                DontDestroyOnLoad(transform.gameObject);
            }
            else Destroy(transform.gameObject);
        }
        void Start()
        {
            controlMode = ControlMode.Buttons;
        }
        public void SetControlMode(int mode)
        {
            switch (mode)
            {
                case 1:
                    {
                        controlMode = ControlMode.Gyro;
                        break;
                    }
                case 2:
                    {
                        controlMode = ControlMode.Buttons;
                        break;
                    }
                case 3:
                    {
                        controlMode = ControlMode.Touch;
                        break;
                    }
                default: break;
            }
        }
    }
}

