using UnityEngine;

public class Planet_Manager : MonoBehaviour {

    public GameObject orbit_Prefab;

    private Star host_Star;
    private Star.starType starType;

    public void Activate()
    {

    }

    public void Deactivate()
    {

    }

    private void GeneratePlanets()
    {

    }

    public void Initialize(Star star)
    {
        host_Star = star;
        starType = star.Get_Star_Type();

        GeneratePlanets();
    }
}
