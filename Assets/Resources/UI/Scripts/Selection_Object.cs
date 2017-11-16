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
        Star star = GetComponent<Star>();
        Planet planet = GetComponent<Planet>();

        //This is a newly selected Object
        if (current_Object != this)
        {
            //This is a newly selected star
            if (star)
            {
                if (current_Object)
                {
                    current_Object.Deselect_This_Object();
                }

                current_Object = this;
                ui_Selector.Select_Star(star);
                CameraController.Set_Focus_Level(CameraController.focus_Level.STAR);
                CameraController.SetTarget(star.gameObject);
                star.View_Star_On();
            }

            else if (planet)
            {
                //Checks if the previous selection was this planet's star or another planet with the same host star
                if (current_Object)
                {
                    star = planet.Get_Star();

                    if (current_Object != star.GetComponent<Selection_Object>())
                    {
                        Planet prevPlanet = current_Object.GetComponent<Planet>();

                        if (prevPlanet)
                        {
                            if(prevPlanet.Get_Star() != star)
                            {
                                current_Object.Deselect_This_Object();
                            }
                        }

                        else
                        {
                            current_Object.Deselect_This_Object();
                        }
                    }
                }

                current_Object = this;
                CameraController.Set_Focus_Level(CameraController.focus_Level.PLANET);
                //CameraController.Zoom_To_Selection_Object(planet.Get_Star().GetComponent<Selection_Object>(), -26);
                ui_Selector.Select_Planet(planet);
            }
        }

        //this is a double click
        else
        {
            //Is this a zoom in for the selected star?
            if (star)
            {
                CameraController.Set_Focus_Level(CameraController.focus_Level.SYSTEM);

                float minDepth = -26;

                if (CameraController.Get_Depth() < minDepth)
                {
                    CameraController.Zoom_To_Selection_Object(this, minDepth, 36);
                }
            }

            else if (planet)
            {
                CameraController.Set_Focus_Level(CameraController.focus_Level.STRUCTURE);
            }
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

    public void Deselect_This_Object()
    {        
        Star star = GetComponent<Star>();
        Planet planet = GetComponent<Planet>();

        if (planet)
        {
            star = planet.Get_Star();

            star.View_Star_Off();
        }

        else if (star)
        {
            star.View_Star_Off();            
        }        

        current_Object = null;
    }    
}
