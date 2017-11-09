using UnityEngine;

public class Panel_Controller : MonoBehaviour {

	public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Toggle()
    {
        if (gameObject.activeInHierarchy)
        {
            Deactivate();
        }

        else
        {
            Activate();
        }
    }
}
