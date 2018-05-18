using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface_Manager : MonoBehaviour {

    private static Planet veiwed_Planet = null;

    private void Populate(Planet planet)
    {

    }

    public void Activate(Planet planet)
    {
        veiwed_Planet = planet;

        gameObject.SetActive(true);
        Populate(planet);
    }

    public void Deactivate()
    {
        veiwed_Planet = null;

        gameObject.SetActive(false);
    }
}
