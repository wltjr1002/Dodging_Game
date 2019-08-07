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
        public void Initialize()
        {
            List<Text> texts = new List<Text>();
            GetComponentsInChildren<Text>(false,texts);
            foreach(Text text in texts)
            {
                string name = text.gameObject.name;
                textBoxes.Add(new NamedField(name, text));
            }
        }
        public void SetText(string name, string content)
        {
            int index = textBoxes.FindIndex(item => {return item.name == name;});
            if ((index != -1))
            {
                textBoxes[index].textBox.text = content;
            }
        }
    }
}