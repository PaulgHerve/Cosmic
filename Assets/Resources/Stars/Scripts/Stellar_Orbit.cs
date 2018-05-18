using UnityEngine;
using System.Collections;

public class Stellar_Orbit : MonoBehaviour {

    public GameObject bodyObject;

    private Planet planet;
    private Planet_Info_Preview preview;
    private Planet_Overview overview;
    private SpriteRotator planet_Rotator;
    private Orbit_Drawer orbit_Draw;

    private float orbit_Distance;
    private Star star;
    private int zone;
    private Quaternion startRotation;

    private IEnumerator currentAnimation;

    public void Activate(Star host_Star, int orbit_Zone, int ring_Index) 
    {
        planet = GetComponentInChildren<Planet>();
        preview = GetComponentInChildren<Planet_Info_Preview>();
        overview = GetComponentInChildren<Planet_Overview>();
        planet_Rotator = bodyObject.GetComponent<SpriteRotator>();
        orbit_Draw = gameObject.AddComponent<Orbit_Drawer>();

        Deactivate_Planet_Preview();

        star = host_Star;
        zone = orbit_Zone;

        Set_Orbit_Size(ring_Index);

        Generate_Planet();
    }

    private void Draw_Orbit()
    {
        orbit_Draw.Draw_Orbit(orbit_Distance + .2f);
    }

    private void Generate_Planet()
    {
        planet.Set_Stellar_Orbit(this);
        planet.Set_Star(star);
        planet.GenerateSize();

        Initialize_Start_Rotation();
    }

    private void Initialize_Start_Rotation()
    {
        float yRotation = Random.Range(0, 360);
        startRotation = new Quaternion();

        startRotation = Quaternion.Euler(0, yRotation, 0);

        transform.localRotation = startRotation;
    }

    public float Get_Orbit_Size()
    {
        return orbit_Distance;
    }

    private void Set_Orbit_Size(float val)
    {
        orbit_Distance = 15 + (7.5f * val);

        Update_Orbital_Ring();
    }

    private void Update_Orbital_Ring()
    {
        Vector3 localPos = bodyObject.transform.localPosition;
        //Vector3 orbitScale = new Vector3(orbit_Distance, orbit_Distance, orbit_Distance);

        localPos.x = orbit_Distance;

        bodyObject.transform.localPosition = localPos;
    }

    public void Set_Check_Rotation()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
            currentAnimation = null;
        }

        Quaternion endRotation = startRotation;

        IEnumerator rotate = RotateToQuaternion(endRotation);

        currentAnimation = rotate;
        planet_Rotator.enabled = true;
        StartCoroutine(rotate);
    }

    public void Set_View_Rotation()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
            currentAnimation = null;
        }

        Quaternion endRotation = Quaternion.Euler(340, 30, 0);

        IEnumerator rotate = RotateToQuaternion(endRotation);

        currentAnimation = rotate;
        planet_Rotator.enabled = false;
        StartCoroutine(rotate);
    }

    private IEnumerator RotateToQuaternion(Quaternion endRotation)
    {
        Quaternion currentRotation = transform.localRotation;

        while (currentRotation != endRotation)
        {
            currentRotation = transform.localRotation;
            transform.localRotation = Quaternion.Slerp(currentRotation, endRotation, .25f);

            planet_Rotator.TriggerRotator();

            yield return new WaitForSeconds(.02f);
        }

        currentAnimation = null;

        yield return null;
    }

    public void Activate_Planet_Preview()
    {
        preview.Activate(planet);
    }

    public void Deactivate_Planet_Preview()
    {
        preview.Deactivate();
    }

    public void Toggle_Planet_Preview()
    {
        preview.Toggle(planet);
    }

    public void View()
    {
        transform.gameObject.SetActive(true);

        Draw_Orbit();
        overview.Populate(planet);
    }

    public void Hide()
    {
        Deactivate_Planet_Preview();

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
