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
        public Text texbox;
    };
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private List<NamedField> textBoxes;
        [SerializeField]
        private Slider sensitivity;
        //public Dictionary<string, Text> textBoxes;
        // Start is called before the first frame update
        public void SetText(string name, string content)
        {
            int index = textBoxes.FindIndex(item => {return item.name == name;});
            Debug.Log(index);
            if ((index != -1))
            {
                textBoxes[index].texbox.text = content;
            }
        }
        public float GetSliderValue()
        {
            return sensitivity.value;
        }
    }
}