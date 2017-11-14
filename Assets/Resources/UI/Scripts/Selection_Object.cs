using UnityEngine;

public class Selection_Object : MonoBehaviour {

    UI_Selector ui_Selector;

    static Selection_Object current_Object;

    private void Awake()
    {
        ui_Selector = UI_Controller.Get_UI_Selector();
    }

    public void Select_This_Object()
    {
        current_Object = this;
        ui_Selector.Activate_Indicator(transform.position);

        Star star = GetComponent<Star>();
        Planet planet = GetComponent<Planet>();

        if (star)
        {
            ui_Selector.SetScale(.15f);
            CameraController.SetTarget(star.gameObject);
            star.View_Star_On();
        }

        else if (planet)
        {
            ui_Selector.SetScale(.05f);
        }
    }

    public void Deselect_This_Object()
    {
        Star star = GetComponent<Star>();
        Planet planet = GetComponent<Planet>();

        Planet currentPlanet = null;

        if (current_Object)
        {
            currentPlanet = current_Object.GetComponent<Planet>();
        }

        if (star)
        {
            if (currentPlanet)
            {
                if (currentPlanet.Get_Star() != star)
                {
                    star.View_Star_Off();
                }
            }

            else
            {
                star.View_Star_Off();
            }
        }

        else if (planet)
        {
            //star = planet.Get_Star();

            //Selection_Object s = star.gameObject.GetComponent<Selection_Object>();

            //UI_Controller.SelectNewObject(s);
        }
    }
}
