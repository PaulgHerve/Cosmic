using UnityEngine;
using System.Collections.Generic;

public class Planet_Manager : MonoBehaviour {

    public GameObject orbit_Prefab;

    private Star host_Star;
    private Star.starType starType;

    Stellar_Orbit[] orbits; 

    private void Generate_Planets()
    {
        int orbitCount = Random.Range(0, 9);
        orbits = new Stellar_Orbit[orbitCount];
        int range = 0;
        int[] zones = new int[orbitCount];

        for (int i = 0; i < zones.Length; i++)
        {
            zones[i] = Random.Range(0, 5);
        }

        zones = ReOrder_Zones(zones);

        for (int i = 0; i < orbitCount; i++)
        {
            GameObject orbit_Obj = Instantiate(orbit_Prefab);
            Stellar_Orbit orbit = orbit_Obj.GetComponent<Stellar_Orbit>();

            orbits[i] = orbit;

            orbit_Obj.transform.SetParent(transform, false);

            orbit.Activate(host_Star, zones[i], range);            

            range++;
        }
    }    

    public void Initialize(Star star)
    {
        host_Star = star;
        starType = star.Get_Star_Type();

        Generate_Planets();
    }

    public void View_System()
    {
        for (int i = 0; i < orbits.Length; i++)
        {
            Stellar_Orbit item = orbits[i];

            item.View();
        }
    }

    public void Hide_System()
    {
        for (int i = 0; i < orbits.Length; i++)
        {
            Stellar_Orbit item = orbits[i];

            item.Hide();
        }
    }

    private int[] ReOrder_Zones(int[] array)
    {
        int[] return_list = new int[array.Length];
        int index = 0;

        //check from 0 - 4. One run per zone
        for (int val = 0; val < 5; val++)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int item = array[i];

                if (item == val)
                {
                    return_list[index] = item;
                    index++;
                }
            }
        }

        return return_list;
    }
}
