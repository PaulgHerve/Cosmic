using UnityEngine;

public class Selection_Object : MonoBehaviour {

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
        CameraController.SetTarget(star.gameObject);
        star.Star_View_On();
    }
    
    private static void Select_New_Planet(Planet planet)
    {
        current_Object = planet.GetComponent<Selection_Object>();
        ui_Selector.Select_Planet(planet);
        CameraController.Set_Focus_Level(CameraController.focus_Level.PLANET);
    }

    private static void StarToSystem(Star star)
    {
        current_Object = star.GetComponent<Selection_Object>();
        ui_Selector.Select_Star(star);
        CameraController.Set_Focus_Level(CameraController.focus_Level.SYSTEM);
        CameraController.SetTarget(star.gameObject);

        float minDepth = -360;

        //Zooms camera to SYSTEM view
        if (CameraController.Get_Depth() < minDepth)
        {
            CameraController.Zoom_To_Selection_Object(current_Object, minDepth, 50);
        }

        star.System_View_On();
    }

    private static void PlanetToSurface(Planet planet)
    {

    }

    private static void SurfaceToPlanet(Planet planet)
    {
        current_Object = planet.GetComponent<Selection_Object>();
        ui_Selector.Select_Planet(planet);
        CameraController.Set_Focus_Level(CameraController.focus_Level.PLANET);
    }

    private static void PlanetToSystem(Planet planet)
    {
        Star star = planet.Get_Star();

        current_Object = star.GetComponent<Selection_Object>();
        ui_Selector.Select_Star(star);
        CameraController.Set_Focus_Level(CameraController.focus_Level.SYSTEM);
        CameraController.SetTarget(star.gameObject);
    }

    private static void SystemToStar(Star star)
    {
        current_Object = star.GetComponent<Selection_Object>();
        ui_Selector.Select_Star(star);
        CameraController.Set_Focus_Level(CameraController.focus_Level.STAR);
        CameraController.SetTarget(star.gameObject);

        float minDepth = -550;

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
                if (prevPlanet != this)                                 //Previous Planet is not this planet
                {
                    Select_New_Planet(planet);
                }

                else                                                    //Previous planet is this planet
                {
                    //Zoom to structure
                }
            }
        }

        if (focus_Level == CameraController.focus_Level.SURFACE)
        {

        }
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
                    StarToSystem(star);                                 //Previous star is this star (Double click)
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

            if (current_Object)
            {
                prevStar = current_Object.GetComponent<Star>();
            }
            
            if (prevStar)                                               //Previous Object was a Star
            {
                if (prevStar != star)                                   //Previous Object was not this Star
                {
                    Select_New_Star(star);
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
                    current_Object = this;
                    ui_Selector.Select_Star(star);
                    CameraController.Set_Focus_Level(CameraController.focus_Level.SYSTEM);
                    CameraController.SetTarget(star.gameObject);
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
