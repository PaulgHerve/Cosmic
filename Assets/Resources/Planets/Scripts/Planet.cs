using UnityEngine;

public class Planet : MonoBehaviour {

    private bool is_Habitable;
    private int atmosphere;
    private int climate;
    private int size;
    private int temp;

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

    public void Set_Temp(int val)
    {
        temp = val;
    }
}
