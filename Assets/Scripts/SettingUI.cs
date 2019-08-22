namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SettingUI : MonoBehaviour
    {
        public void SetControlMode(int mode)
        {
            FindObjectOfType<GameOption>().SetControlMode(mode);
        }
    }

}
