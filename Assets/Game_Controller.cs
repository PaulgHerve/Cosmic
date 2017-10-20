using UnityEngine;

public class Game_Controller : MonoBehaviour {

    public bool enable_Galaxy_Rotation;

    public void RotateAll90()
    {
        Star[] allStars = GalaxyGenerator.GetStars();

        foreach (Star item in allStars)
        {
            item.Rotate90Degrees();
        }
    }
}
