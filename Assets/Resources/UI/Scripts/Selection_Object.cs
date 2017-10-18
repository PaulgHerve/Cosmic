using UnityEngine;
using System.Collections;

public class Selection_Object : MonoBehaviour {

    UI_Selector ui_Selector;

    private void Awake()
    {
        ui_Selector = GetComponentInChildren<UI_Selector>();
    }

    public void Select_This_Object()
    {
        ui_Selector.Activate_Indicator();
    }

    public void Deselect_This_Object()
    {
        ui_Selector.Deactivate_Indicator();
    }
}
