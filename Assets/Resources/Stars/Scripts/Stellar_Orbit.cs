using UnityEngine;
using System.Collections;

public class Stellar_Orbit : MonoBehaviour {

    private Planet planet;
    SpriteRotator planet_Rotator;
    private Orbit_Drawer orbit_Draw;

    private float orbit_Distance;
    private Star star;
    private int zone;
    private Quaternion startRotation;

    private IEnumerator currentAnimation;

    public void Activate(Star host_Star, int orbit_Zone, int ring_Index) 
    {
        planet = GetComponentInChildren<Planet>();
        planet_Rotator = planet.GetComponent<SpriteRotator>();
        orbit_Draw = gameObject.AddComponent<Orbit_Drawer>();

        star = host_Star;
        zone = orbit_Zone;

        Set_Orbit_Size(ring_Index);

        Generate_Planet();
    }

    private void Draw_Orbit()
    {
        orbit_Draw.Draw_Orbit(orbit_Distance);
    }

    private void Generate_Planet()
    {
        planet.Set_Star(star);

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
        orbit_Distance = 15 + (6 * val);

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
