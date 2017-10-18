using UnityEngine;
using System.Collections;

public class UI_Selector : MonoBehaviour {

    UI_Selector_Indicator indicator;

    void Awake()
    {
        indicator = GetComponentInChildren<UI_Selector_Indicator>();
    }

    void Start ()
    {
        SphereCollider c = GetComponent<SphereCollider>();

        c.enabled = true;

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
