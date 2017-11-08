using UnityEngine;
using UnityEngine.UI;

public class Value_Text : MonoBehaviour {

    public float multiplier;
    public string[] textValues;

    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    public void Set_Value_INT(Slider slider)
    {
        float val = slider.value * multiplier;

        text.text = val.ToString();
    }

    public void Set_Value_Percent(Slider slider)
    {
        float val = slider.value * multiplier;

        val *= 100;
        val = (int)val;

        text.text = val.ToString() + "%";
    }

    public void Set_Text_Value(Slider slider)
    {
        int val = (int)(slider.value * multiplier);

        if (textValues.Length > val) { text.text = textValues[val]; }
        else { text.text = "NO VALID VALUE"; }
    }
}
