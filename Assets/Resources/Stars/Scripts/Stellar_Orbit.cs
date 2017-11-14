using UnityEngine;

public class Stellar_Orbit : MonoBehaviour {

    private ParticleSystem ring;
    private Planet planet;
    private SpriteRenderer orbit_Sprite;

    private float orbit_Size;
    private float orbit_distance;
    private Star star;
    private int zone;

    public void Activate(Star host_Star, int orbit_Zone, int ring_Index) 
    {
        ring = GetComponentInChildren<ParticleSystem>();
        planet = GetComponentInChildren<Planet>();
        orbit_Sprite = GetComponentsInChildren<SpriteRenderer>()[1];

        star = host_Star;
        zone = orbit_Zone;

        Set_Orbit_Size(ring_Index + 1);

        Generate_Planet();
    }

    private void Generate_Planet()
    {
        planet.Set_Star(star);
    }

    public float Get_Orbit_Size()
    {
        return orbit_Size;
    }

    private void Set_Orbit_Size(float val)
    {
        orbit_distance = val;
        orbit_Size = 16 * val;

        Update_Orbital_Ring();
    }

    private void Update_Orbital_Ring()
    {
        Vector3 localPos = planet.transform.localPosition;
        Vector3 orbitScale = new Vector3(orbit_Size, orbit_Size, orbit_Size);

        orbit_Sprite.transform.localScale = orbitScale;
        localPos.x = orbit_Size;

        planet.transform.localPosition = localPos;
    }

    public void View()
    {
        transform.gameObject.SetActive(true);
    }

    public void Hide()
    {
        transform.gameObject.SetActive(false);
    }
}
