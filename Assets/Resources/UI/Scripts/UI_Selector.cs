using UnityEngine;

public class UI_Selector : MonoBehaviour {

    UI_Selector_Indicator indicator;
    UI_Selector_Indicator mini_Indicator;
    public SpriteRenderer blur;

    private Selection_Object select;

    void Awake()
    {
        UI_Selector_Indicator[] things = GetComponentsInChildren<UI_Selector_Indicator>();

        indicator = things[0];
        mini_Indicator = things[1];
    }

    private void Update()
    {
        if (select)
        {
            CameraController.focus_Level focus = CameraController.Get_Focus_Level();

            if ((int)focus > 2)
            {
                Planet p = select.GetComponent<Planet>();

                Set_Planet_Position(p);
            }

            else
            {
                mini_Indicator.transform.position = select.transform.position;
            }
        }
    }

    void Start ()
    {
        Deactivate_All();
    }

    private void Activate_Indicator()
    {
        indicator.Activate();
    }

    private void Activate_Mini_Indicator()
    {
        mini_Indicator.Activate();
    }

    private void Deactivate_Indicator()
    {
        indicator.Deactivate();
    }

    private void Deactivate_Mini_Indicator()
    {
        mini_Indicator.Deactivate();
    }

    public void Activate_All()
    {
        Activate_Indicator();
        Activate_Mini_Indicator();
        blur.enabled = true;
    }

    public void Deactivate_All()
    {
        Deactivate_Indicator();
        Deactivate_Mini_Indicator();
        blur.enabled = false;
        select = null;
    }

    public void Select_System(Star target)
    {
        Vector3 pos = target.transform.position;
        select = null;

        Set_Position(pos);
        Set_Mini_Position(pos);

        Activate_Mini_Indicator();
        blur.enabled = true;
        Deactivate_Indicator();
        Set_Mini_Scale(.8f);
    }

    public void Select_Star(Star target)
    {
        Vector3 pos = target.transform.position;
        Vector3 camPos = pos;
        select = null;

        if(CameraController.target)
        {
            camPos = CameraController.target.transform.position;
        }

        Activate_All();

        Set_Position(pos);
        Set_Mini_Position(camPos);

        Activate_Indicator();
        Set_Scale(3);
        Set_Mini_Scale(.8f);        
    }

    public void Select_Planet(Planet target)
    {
        Vector3 pos = target.Get_Sprite().gameObject.transform.position;
        Vector3 starPos = target.Get_Star().transform.position;
        select = target.GetComponent<Selection_Object>();

        Set_Position(starPos);

        Deactivate_Indicator();
        Set_Mini_Position(pos);
        Set_Mini_Scale(.2f);
    }

    private void Set_Planet_Position(Planet target)
    {
        Vector3 pos = target.Get_Sprite().gameObject.transform.position;
        Set_Mini_Position(pos);
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
