using UnityEngine;

public class UI_Selector : MonoBehaviour {

    UI_Selector_Indicator indicator;
    UI_Selector_Indicator mini_Indicator;

    void Awake()
    {
        UI_Selector_Indicator[] things = GetComponentsInChildren<UI_Selector_Indicator>();

        indicator = things[0];
        mini_Indicator = things[1];
    }

    void Start ()
    {
        Deactivate_All();
    }

    private void Activate_Indicator()
    {
        indicator.gameObject.SetActive(true);
    }

    private void Activate_Mini_Indicator()
    {
        mini_Indicator.gameObject.SetActive(true);
    }

    private void Deactivate_Indicator()
    {
        indicator.gameObject.SetActive(false);
    }

    private void Deactivate_Mini_Indicator()
    {
        mini_Indicator.gameObject.SetActive(false);
    }

    public void Activate_All()
    {
        Activate_Indicator();
        Activate_Mini_Indicator();
    }

    public void Deactivate_All()
    {
        Deactivate_Indicator();
        Deactivate_Mini_Indicator();
    }

    public void Select_Star(Star target)
    {
        Vector3 pos = target.transform.position;

        Activate_All();

        Set_Position(pos);
        Set_Mini_Position(pos);

        Set_Scale(1.6f);
        Set_Mini_Scale(.2f);        
    }

    public void Select_Planet(Planet target)
    {
        Vector3 pos = target.transform.position;
        Vector3 starPos = target.Get_Star().transform.position;

        Set_Position(starPos);
        Set_Scale(1.6f);

        Set_Mini_Position(pos);
        Set_Mini_Scale(.04f);
    }

    public void Set_Mini_Position(Vector3 pos)
    {
        mini_Indicator.transform.position = pos;
    }

    public void Set_Position(Vector3 pos)
    {
        indicator.transform.position = pos;
    }

    private void Reset_Position()
    {
        indicator.transform.localPosition = new Vector3(0, 0, 0);
    }

    private void Reset_Mini_Position()
    {
        mini_Indicator.transform.localPosition = indicator.transform.localPosition;
    }

    private void Set_Scale(float scale)
    {
        Vector3 newScale = new Vector3(scale, scale, scale);

        indicator.transform.localScale = newScale;
    }

    private void Set_Mini_Scale(float scale)
    {
        Vector3 newScale = new Vector3(scale, scale, scale);

        mini_Indicator.transform.localScale = newScale;
    }
}
