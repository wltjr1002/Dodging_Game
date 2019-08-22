namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class StartUI : MonoBehaviour
    {
        GameObject howtoWindow;

        public void OpenHowto()
        {
            howtoWindow.SetActive(true);
        }

        public void CloseHowto()
        {
            howtoWindow.SetActive(false);
        }
        public void StartGame()
        {
            SceneManager.LoadScene("MainGame");
        }
    }
}

