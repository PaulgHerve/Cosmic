using UnityEngine;

public class Selection_Object : MonoBehaviour {

    public int selection_Priority;

    static UI_Selector ui_Selector;

    static Selection_Object current_Object;

    private void Awake()
    {
        ui_Selector = UI_Controller.Get_UI_Selector();
    }

    private static void Select_New_Star(Star star)
    {
        if (current_Object)
        {
            current_Object.Deselect_This_Object();
        }

        current_Object = star.GetComponent<Selection_Object>();
        ui_Selector.Select_Star(star);
        CameraController.Set_Focus_Level(CameraController.focus_Level.STAR);
        star.Star_View_On();
    }
    
    private static void Select_New_Planet(Planet planet)
    {
        current_Object = planet.GetComponent<Selection_Object>();
        ui_Selector.Select_Planet(planet);
        CameraController.Set_Focus_Level(CameraController.focus_Level.PLANET);

        planet.Activate_Preview();
    }

    private static void StarToSystem(Star star)
    {
        ui_Selector.Select_System(star);
        CameraController.Set_Focus_Level(CameraController.focus_Level.SYSTEM);
        CameraController.SetTarget(star.gameObject);
                
        star.System_View_On();

        ZoomToStar(star);
    }

    private static void ZoomToStar(Star star)
    {
        current_Object = star.GetComponent<Selection_Object>();
        float minDepth = -300;
        float speed = 50;

        speed = speed * CameraController.Get_Current_Depth_Ratio();

        //Zooms camera to SYSTEM view
        if (CameraController.Get_Depth() != minDepth)
        {
            CameraController.Zoom_To_Selection_Object(current_Object, minDepth, speed);
        }
    }

    private static void PlanetToSurface(Planet planet)
    {
        //Debug.Log("PlanetToSurface");

        current_Object = planet.GetComponent<Selection_Object>();
        ui_Selector.Select_Planet(planet);
        CameraController.Set_Focus_Level(CameraController.focus_Level.SURFACE);

        UI_Controller.surface_Manager.Activate(planet);
    }

    private static void SurfaceToPlanet(Planet planet)
    {
        current_Object = planet.GetComponent<Selection_Object>();
        ui_Selector.Select_Planet(planet);
        CameraController.Set_Focus_Level(CameraController.focus_Level.PLANET);

        UI_Controller.surface_Manager.Deactivate();
    }

    private static void PlanetToSystem(Planet planet)
    {
        Star star = planet.Get_Star();

        current_Object = star.GetComponent<Selection_Object>();
        ui_Selector.Select_System(star);
        CameraController.Set_Focus_Level(CameraController.focus_Level.SYSTEM);
        CameraController.SetTarget(star.gameObject);
    }

    private static void SystemToStar(Star star)
    {
        current_Object = star.GetComponent<Selection_Object>();
        ui_Selector.Select_Star(star);
        CameraController.Set_Focus_Level(CameraController.focus_Level.STAR);
        CameraController.SetTarget(star.gameObject);

        float minDepth = -150;

        if (CameraController.Get_Depth() > minDepth)
        {
            CameraController.Zoom_To_Selection_Object(current_Object, minDepth, 45);
        }

        star.Star_View_On();
    }

    private static void StarToGalaxy()
    {
        if (current_Object)
        {
            current_Object.Deselect_This_Object();
        }

        current_Object = null;
        ui_Selector.Deactivate_All();
        CameraController.Set_Focus_Level(CameraController.focus_Level.NONE);
        CameraController.SetTarget(null);
    }

    public void Select_Object()
    {
        Star star = GetComponent<Star>();
        Planet planet = GetComponent<Planet>();

        //Debug.Log(gameObject.name);

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

        if (focus_Level == CameraController.focus_Level.STAR) { Select_New_Planet(planet); }
        if (focus_Level == CameraController.focus_Level.SYSTEM) { Select_New_Planet(planet); }
        if (focus_Level == CameraController.focus_Level.PLANET)
        {
            Planet prevPlanet = null;

            if (current_Object)
            {
                prevPlanet = current_Object.GetComponent<Planet>();
            }

            if (prevPlanet)
            {
                if (current_Object != this)                             //Previous Planet is not this planet
                {
                    Select_New_Planet(planet);
                }

                else                                                    //Previous planet is this planet
                {
                    //Zoom to structure

                    PlanetToSurface(planet);
                }
            }

            else                                                        //No Previous Planet was selected, This shouldn't ever happen
            {
                Select_New_Planet(planet);
            }
        }

        if (focus_Level == CameraController.focus_Level.SURFACE)
        {

        }
    }

    private void Select_Star(Star star)
    {
        CameraController.focus_Level focus_Level = CameraController.Get_Focus_Level();
        GameObject cameraTarget = null;

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
                    cameraTarget = CameraController.target;

                    if (cameraTarget)                                           //The camera has a target
                    {
                        if (cameraTarget != star.gameObject)                    //The camera target is not this star
                        {
                            CameraController.SetTarget(star.gameObject);
                            ui_Selector.Set_Mini_Position(star.gameObject.transform.position);
                            ui_Selector.Deactivate_Mini_Indicator();
                        }

                        else
                        {
                            StarToSystem(star);                                 //The camera target and previous target are this star (Double click)
                        }
                    }

                    CameraController.SetTarget(star.gameObject);                //The previous object is thist star, but not the camera target. Make this object the camera target
                }

                else
                {
                    Select_New_Star(star);                                      //Previously selected star is not this star
                }
            }

            else { Select_New_Star(star); }                                     //No previously selected star
        }

        if (focus_Level == CameraController.focus_Level.SYSTEM)
        {
            Star prevStar = null;

            if (current_Object)
            {
                prevStar = current_Object.GetComponent<Star>();
            }
            
            if (prevStar)                                                       //Previous Object was a Star
            {
                if (prevStar != star)                                           //Previous Object was not this Star
                {
                    Select_New_Star(star);
                }

                else
                {
                    ZoomToStar(star);                                 //Previous Object was this star
                }
            }
        }

        if (focus_Level == CameraController.focus_Level.PLANET)         //Peevious object was a planet
        {
            Planet prevPlanet = null;
            Star hostStar = null;            

            if (current_Object)
            {
                prevPlanet = current_Object.GetComponent<Planet>();
            }

            if (prevPlanet)                                             //Prev object was a planet
            {
                hostStar = prevPlanet.Get_Star();
                
                if (hostStar != star)                                   //Previous Host Star was not this Star
                {
                    Select_New_Star(star);
                }

                else                                                    //Previous Host Star was this Star
                {                                                       //Same as zoom to star without the zooming or re-org of planet manager
                    Planet_Manager.ringState current = GetComponentInChildren<Planet_Manager>().Get_Current_RingState();

                    if (current == Planet_Manager.ringState.STAR)
                    {
                        current_Object = this;
                        ui_Selector.Select_Star(star);
                        CameraController.Set_Focus_Level(CameraController.focus_Level.STAR);
                        //CameraController.SetTarget(star.gameObject);
                    }

                    else
                    {
                        current_Object = this;
                        ui_Selector.Select_System(star);
                        CameraController.Set_Focus_Level(CameraController.focus_Level.SYSTEM);
                        CameraController.SetTarget(star.gameObject);
                    }
                }
            }
        }

        if (focus_Level == CameraController.focus_Level.SURFACE)
        {

        }
    }

    //Selects the parent object in the stellar_hierarchy (NONE, STAR, PLANET, SURFACE)
    public static void Backup_Current_Selection()
    {
        Selection_Object current = current_Object;

        if (current_Object)
        {
            CameraController.focus_Level focus_Level = CameraController.Get_Focus_Level();

            Planet planet = current.GetComponent<Planet>();
            Star star = current.GetComponent<Star>();

            if (focus_Level == CameraController.focus_Level.STAR) { StarToGalaxy(); }
            else if (focus_Level == CameraController.focus_Level.SYSTEM) { SystemToStar(star); }
            else if (focus_Level == CameraController.focus_Level.PLANET) { SystemToStar(planet.Get_Star()); }
            else if (focus_Level == CameraController.focus_Level.SURFACE) { SurfaceToPlanet(planet); }
        }
    }

    public void Deselect_This_Object()
    {        
        Star star = GetComponent<Star>();
        Planet planet = GetComponent<Planet>();

        if (planet)
        {
            star = planet.Get_Star();

            star.System_View_Off();
        }

        else if (star)
        {
            star.System_View_Off();            
        }        

        current_Object = null;
    }    

    public static Selection_Object Get_Current_Selection_Object()
    {
        return current_Object;
    }
}
