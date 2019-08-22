namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    [System.Serializable]
    struct NamedField
    {
        public string name;
        public Text textBox;

        public NamedField(string _name, Text _text)
        {
            name = _name;
            textBox = _text;
        }
    };
    public class UIManager : MonoBehaviour
    {
        public GameObject touchControl;
        public GameObject buttonControl;
        public GameObject HPbar;
        [SerializeField]
        private List<NamedField> textBoxes;
        private KeyDowns keyDowns;
        public void Initialize(ControlMode controlMode)
        {
            List<Text> texts = new List<Text>();
            GetComponentsInChildren<Text>(false, texts);
            foreach (Text text in texts)
            {
                string name = text.gameObject.name;
                textBoxes.Add(new NamedField(name, text));
            }
            keyDowns = new KeyDowns(false, false, false, false, false, false);

            switch (controlMode)
            {
                case ControlMode.Gyro:
                    {
                        touchControl.SetActive(false);
                        buttonControl.SetActive(false);
                        break;
                    }
                case ControlMode.Buttons:
                    {
                        touchControl.SetActive(false);
                        buttonControl.SetActive(true);
                        break;
                    }
                case ControlMode.Touch:
                    {
                        touchControl.SetActive(true);
                        buttonControl.SetActive(false);
                        break;
                    }
                default: break;
            }
        }
        public void SetText(string name, string content)
        {
            int index = textBoxes.FindIndex(item => { return item.name == name; });
            if ((index != -1))
            {
                textBoxes[index].textBox.text = content;
            }
        }
        public void SetKeyDowns(int key)
        {
            switch (Mathf.Abs(key))
            {
                case 1: { keyDowns.left = key > 0; break; }
                case 2: { keyDowns.right = key > 0; break; }
                case 3: { keyDowns.space = key > 0; break; }
                case 4: { keyDowns.shift = key > 0; break; }
                default: break;
            }
        }
        public KeyDowns GetButtonStates()
        {
            return keyDowns;
        }

        public void SetBossHP(float percent)
        {
           RectTransform rect = HPbar.GetComponent<RectTransform>();
           rect.anchoredPosition = new Vector2((percent-100)*2,0);
           rect.sizeDelta = new Vector2((percent-100)*4,-10);
        }
    }
}