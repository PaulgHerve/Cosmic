using UnityEngine;

public class Stellar_Orbit : MonoBehaviour {

    private Planet planet;
    private Orbit_Drawer orbit_Draw;

    private float orbit_Distance;
    private Star star;
    private int zone;

    public void Activate(Star host_Star, int orbit_Zone, int ring_Index) 
    {
        planet = GetComponentInChildren<Planet>();
        orbit_Draw = gameObject.AddComponent<Orbit_Drawer>();

        star = host_Star;
        zone = orbit_Zone;

        Set_Orbit_Size(ring_Index + 2);

        Generate_Planet();
    }

    private void Draw_Orbit()
    {
        orbit_Draw.Draw_Orbit(orbit_Distance);
    }

    private void Generate_Planet()
    {
        planet.Set_Star(star);
    }

    public float Get_Orbit_Size()
    {
        return orbit_Distance;
    }

    private void Set_Orbit_Size(float val)
    {
        orbit_Distance = 8 + (8 * val);

        Update_Orbital_Ring();
    }

    private void Update_Orbital_Ring()
    {
        Vector3 localPos = planet.transform.localPosition;
        Vector3 orbitScale = new Vector3(orbit_Distance, orbit_Distance, orbit_Distance);

        localPos.x = orbit_Distance;

        planet.transform.localPosition = localPos;
    }

    public void Set_Check_Rotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void Set_View_Rotation()
    {
        transform.rotation = Quaternion.Euler(340, 30, 0);
    }

    public void View()
    {
        transform.gameObject.SetActive(true);

        Draw_Orbit();
    }

    public void Hide()
    {
        transform.gameObject.SetActive(false);
    }

    public Star Get_Star()
    {
        return star;
    }

    public int Get_Zone()
    {
        return zone;
    }
}
