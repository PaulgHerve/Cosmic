using UnityEngine;

public class UI_Selector : MonoBehaviour {

    UI_Selector_Indicator indicator;

    void Awake()
    {
        indicator = GetComponentInChildren<UI_Selector_Indicator>();
    }

    void Start ()
    {
        Deactivate_Indicator();
    }

    public void Activate_Indicator()
    {
        indicator.Activate();
    }

    public void Deactivate_Indicator()
    {
        indicator.Deactivate();
    }
}
