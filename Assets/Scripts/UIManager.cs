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
        [SerializeField]
        private List<NamedField> textBoxes;
        private KeyDowns keyDowns;
        public void Initialize()
        {
            List<Text> texts = new List<Text>();
            GetComponentsInChildren<Text>(false,texts);
            foreach(Text text in texts)
            {
                string name = text.gameObject.name;
                textBoxes.Add(new NamedField(name, text));
            }
            keyDowns = new KeyDowns(false,false,false,false,false,false);
        }
        public void SetText(string name, string content)
        {
            int index = textBoxes.FindIndex(item => {return item.name == name;});
            if ((index != -1))
            {
                textBoxes[index].textBox.text = content;
            }
        }
        public void SetKeyDowns(int key)
        {
            switch(Mathf.Abs(key))
            {
                case 1: {keyDowns.left = key>0; break;}
                case 2: {keyDowns.right = key>0; break;}
                case 3: {keyDowns.space = key>0; break;}
                case 4: {keyDowns.shift = key>0; break;}
                default: break;
            }
            Debug.Log(keyDowns.ToString());
        }
        public KeyDowns GetButtonStates()
        {
            return keyDowns;
        }
    }
}