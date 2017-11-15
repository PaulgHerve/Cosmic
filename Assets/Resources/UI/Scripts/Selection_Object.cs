using UnityEngine;

public class Selection_Object : MonoBehaviour {

    UI_Selector ui_Selector;

    static Selection_Object current_Object;

    private void Awake()
    {
        ui_Selector = UI_Controller.Get_UI_Selector();
    }

    public void Focus_Object()
    {
        Star star = GetComponent<Star>();

        if (star)
        {
            CameraController.SetTarget(star.gameObject);
            star.View_Star_On();
        }
    }

    public void Select_This_Object()
    {
        if (current_Object != this)
        {
            current_Object = this;

            Star star = GetComponent<Star>();
            Planet planet = GetComponent<Planet>();

            if (star)
            {
                ui_Selector.Select_Star(star);
                CameraController.SetTarget(star.gameObject);
                star.View_Star_On();
            }

            else if (planet)
            {
                ui_Selector.Select_Planet(planet);
            }
            else
            {
                star.View_Star_Off();
            }
        }

        else
        {
            Focus_Object();
        }
    }

    public void Select_Parent_Object()
    {
        Selection_Object target = null;

        Planet planet = GetComponent<Planet>();

        if (planet)
        {
            target = planet.Get_Star().gameObject.GetComponent<Selection_Object>();
        }

        if (target)
        {
            target.Select_This_Object();
        }

        else
        {
            
            CameraController.SetTarget(null);
        }
    }

    private void Deselect_This_Object()
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
