using UnityEngine;

public class Planet : MonoBehaviour {

    private Stellar_Orbit orbit_Controller;

    private bool is_Habitable;
    private int atmosphere;
    private int climate;
    private int size;
    private int temp;
    private Star star;
    private SpriteRenderer sprite;

    public void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    public void GenerateSize()
    {
        size = Random.Range(7, 25);
        float scaler = (2 * size) / 25.00f;
        Vector3 newScale = new Vector3(scaler, scaler, scaler);

        transform.localScale = newScale;
    }

    public void Set_Stellar_Orbit(Stellar_Orbit stellarOrbit)
    {
        orbit_Controller = stellarOrbit;
    }
    
    public void Activate_Preview()
    {
        orbit_Controller.Activate_Planet_Preview();
    }

    public void Deactivate_Preview()
    {
        orbit_Controller.Deactivate_Planet_Preview();
    }

    public void Toggle_Preview()
    {
        orbit_Controller.Toggle_Planet_Preview();
    }

    public int Get_Atmosphere()
    {
        return atmosphere;
    }

    public int Get_Climate()
    {
        return climate;
    }

    public bool Get_Is_Habitable()
    {
        return is_Habitable;
    }

    public int Get_Size()
    {
        return size;
    }

    public Star Get_Star()
    {
        return star;
    }

    public int Get_Temp()
    {
        return temp;
    }

    public void Set_Atmosphere(int val)
    {
        atmosphere = val;
    }

    public void Set_Climate(int val)
    {
        climate = val;
    }

    public void Set_Is_Habitable(bool val)
    {
        is_Habitable = val;
    }

    public void Set_Size(int val)
    {
        size = val;
    }

    public void Set_Star(Star newStar)
    {
        star = newStar;
    }

    public void Set_Temp(int val)
    {
        temp = val;
    }

    public SpriteRenderer Get_Sprite()
    {
        return sprite;
    }
}
