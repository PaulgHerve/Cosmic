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

    public void SetScale(float scale)
    {
        Vector3 newScale = new Vector3(scale, scale, scale);

        transform.localScale = newScale;
    }

    public void Activate_Indicator(Vector3 pos)
    {
        indicator.Activate();

        transform.position = pos;
    }

    public void Deactivate_Indicator()
    {
        indicator.Deactivate();
    }
}
