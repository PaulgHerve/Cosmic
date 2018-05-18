using UnityEngine;

public class Planet_Info_Preview : MonoBehaviour {

    static Planet previewed_Planet;

    public void Activate(Planet planet)
    {
        if (previewed_Planet)
        {
            previewed_Planet.Deactivate_Preview();
        }

        previewed_Planet = planet;

        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Toggle(Planet planet)
    {
        if (gameObject.activeInHierarchy)
        {
            Deactivate();
        }

        else
        {
            Activate(planet);
        }
    }
}
