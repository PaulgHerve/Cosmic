using UnityEngine;

public class Selection_Object : MonoBehaviour {

    UI_Selector ui_Selector;

    static Selection_Object current_Object;

    private void Awake()
    {
        ui_Selector = UI_Controller.Get_UI_Selector();
    }

    private void Select_New_Star(Star star)
    {
        if (current_Object)
        {
            current_Object.Deselect_This_Object();
        }

        current_Object = this;
        ui_Selector.Select_Star(star);
        CameraController.Set_Focus_Level(CameraController.focus_Level.STAR);
        CameraController.SetTarget(star.gameObject);
        star.Check_Star_On();
    }

    private void Zoom_To_Star(Star star)
    {
        current_Object = this;
        ui_Selector.Select_Star(star);
        CameraController.Set_Focus_Level(CameraController.focus_Level.SYSTEM);
        CameraController.SetTarget(star.gameObject);
        star.View_Star_On();
    }

    public void Select_Planets_Host_Star()
    {
        Planet planet = GetComponent<Planet>();
        Star star = planet.Get_Star();

        current_Object = this;
        ui_Selector.Select_Star(star);
        CameraController.Set_Focus_Level(CameraController.focus_Level.SYSTEM);
        CameraController.SetTarget(star.gameObject);

        UI_Controller.SelectNewObject(star.GetComponent<Selection_Object>());

        print("Host Star");
    }

    public void Select_This_Object()
    {
        Star star = GetComponent<Star>();
        Planet planet = GetComponent<Planet>();

        //This is a newly selected Object
        if (current_Object != this)
        {
            Selection_Object prev = current_Object;

            //There is currently a selected object
            if (prev)
            {
                Planet prevPlanet = prev.GetComponent<Planet>();
                Star prevStar = prev.GetComponent<Star>();

                print(prev);

                //This is a newly selected star
                if (star)
                {
                    if (current_Object)
                    {
                        //The previously selected object was a star
                        if (prevStar)
                        {
                            //The previous Object was This star
                            if (star == prevStar)
                            {
                                Zoom_To_Star(star);
                            }

                            else
                            {
                                Select_New_Star(star);
                            }
                        }

                        //The previously object is a planet
                        else if (prevPlanet)
                        {
                            Star planet_Host = prevPlanet.Get_Star();

                            //The current parent belongs to this star
                            if (planet_Host = star)
                            {
                                current_Object.Select_Planets_Host_Star();
                            }

                            //This Star belongs to a different Planet
                            else
                            {
                                Select_New_Star(star);
                            }
                        }
                    }     
                }

                //This is a planet
                else if (planet)
                {
                    //This is now the Host_Star for this planet
                    star = planet.Get_Star();
                      
                    //The previous object was a star                  
                    if (prevStar)
                    {
                        //The Previous star was this planet's host star
                        if (prevStar == star)
                        {
                            current_Object = this;
                            CameraController.Set_Focus_Level(CameraController.focus_Level.PLANET);
                            ui_Selector.Select_Planet(planet);
                        }

                        else
                        {
                            current_Object.Deselect_This_Object();
                        }
                    }

                    //The previous Object was not a star
                    else
                    {
                        current_Object = this;
                        CameraController.Set_Focus_Level(CameraController.focus_Level.PLANET);
                        ui_Selector.Select_Planet(planet);
                    }
                }
            }

            //No currently selected object
            else
            {
                Select_New_Star(star);
            }
        }

        //this is a double click
        else
        {
            //Is this a zoom in for the selected star?
            if (star)
            {
                if (CameraController.Get_Focus_Level() == CameraController.focus_Level.STAR)
                {
                    CameraController.Set_Focus_Level(CameraController.focus_Level.SYSTEM);

                    star.View_Star_On();

                    float minDepth = -350;

                    if (CameraController.Get_Depth() < minDepth)
                    {
                        CameraController.Zoom_To_Selection_Object(this, minDepth, 120);
                    }
                }
            }

            else if (planet)
            {
                CameraController.Set_Focus_Level(CameraController.focus_Level.STRUCTURE);
            }
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
