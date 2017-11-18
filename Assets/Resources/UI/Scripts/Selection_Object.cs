using UnityEngine;

public class Selection_Object : MonoBehaviour {

    static UI_Selector ui_Selector;

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

        float minDepth = -350;

        //Zooms camera to SYSTEM view
        if (CameraController.Get_Depth() < minDepth)
        {
            CameraController.Zoom_To_Selection_Object(this, minDepth, 120);
        }

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
    }

    private void Select_New_Planet(Planet planet)
    {
        current_Object = this;
        ui_Selector.Select_Planet(planet);
        CameraController.Set_Focus_Level(CameraController.focus_Level.PLANET);
    }

    public void Select_Object()
    {
        Star star = GetComponent<Star>();
        Planet planet = GetComponent<Planet>();

        if (planet)
        {
            Select_Planet(planet);
        }

        else if(star)
        {
            Select_Star(star);
        }
    }

    private void Select_Planet(Planet planet)
    {
        CameraController.focus_Level focus_Level = CameraController.Get_Focus_Level();
        Star hostStar = planet.Get_Star();

        if (focus_Level == CameraController.focus_Level.STAR) { }
        if (focus_Level == CameraController.focus_Level.SYSTEM) { }
        if (focus_Level == CameraController.focus_Level.PLANET) { }
        if (focus_Level == CameraController.focus_Level.STRUCTURE) { }
    }

    private void Select_Star(Star star)
    {
        CameraController.focus_Level focus_Level = CameraController.Get_Focus_Level();

        if (focus_Level == CameraController.focus_Level.NONE) { Select_New_Star(star); }
        if (focus_Level == CameraController.focus_Level.STAR)
        {
            Star prevStar = null;

            if (current_Object)
            {
                prevStar = current_Object.GetComponent<Star>();
            }

            if (prevStar)
            {
                if (prevStar == star)
                {
                    Zoom_To_Star(star);                                 //Previous star is this star (Double click)
                }

                else
                {
                    Select_New_Star(star);                              //Previously selected star is not this star
                }
            }

            else { Select_New_Star(star); }                             //No previously selected star
        }

        if (focus_Level == CameraController.focus_Level.SYSTEM)
        {
            Star prevStar = null;
            Planet prevPlanet = null;

            if (current_Object)
            {
                prevStar = current_Object.GetComponent<Star>();
                prevPlanet = current_Object.GetComponent<Planet>();
            }
            
            if (prevStar)                                               //Previous Object was a Star
            {
                if (prevStar != star)                                   //Previous Object was not this Star
                {
                    Select_New_Star(star);
                }
            }

            else if(prevPlanet)                                         //Prev object was a planet
            {
                prevStar = prevPlanet.Get_Star();

                if (prevStar != star)                                   //Previous Host Star was not this Star
                {
                    Select_New_Star(star);
                }

                else                                                    //Previous Host Star was this Star
                {                                                       //Same as zoom to star without the zooming or re-org of planet manager
                    current_Object = this;
                    ui_Selector.Select_Star(star);
                    CameraController.SetTarget(star.gameObject);
                }
            }
        }

        if (focus_Level == CameraController.focus_Level.PLANET) { }
        if (focus_Level == CameraController.focus_Level.STRUCTURE) { }
    }

    private void Select_This_Object()
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

    //Selects the parent object in the stellar_hierarchy (NONE, STAR, PLANET, SURFACE)
    public static void Backup_Current_Selection()
    {
        Selection_Object sHit = current_Object;

        if (sHit)
        {
            Planet planet = sHit.GetComponent<Planet>();
            Star star = sHit.GetComponent<Star>();

            if (planet)
            {
                star = planet.Get_Star();

                sHit.Select_Planets_Host_Star();
            }

            else if (star)
            {
                float minDepth = -560;

                star.Check_Star_On();

                if (CameraController.Get_Depth() > minDepth)
                {
                    CameraController.Zoom_To_Selection_Object(sHit, minDepth, 120);
                }

                else
                {
                    sHit.Deselect_This_Object();
                    ui_Selector.Deactivate_All();
                    CameraController.SetTarget(null);
                }
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

    public static Selection_Object Get_Current_Selection_Object()
    {
        return current_Object;
    }
}
