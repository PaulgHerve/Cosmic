using UnityEngine;
using System.Collections.Generic;

public class Planet_Manager : MonoBehaviour {

    public GameObject orbit_Prefab;

    private Star host_Star;
    private Star.starType starType;

    List<Stellar_Orbit> orbits = new List<Stellar_Orbit>();

    public void Activate()
    {

    }

    public void Deactivate()
    {

    }

    private void GeneratePlanets()
    {
        int OrbitCount = Random.Range(0, 9);
        int index = 0;


    }

    public void Initialize(Star star)
    {
        host_Star = star;
        starType = star.Get_Star_Type();

        GeneratePlanets();
    }
}
