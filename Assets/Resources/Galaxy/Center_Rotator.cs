using UnityEngine;

public class Center_Rotator : MonoBehaviour {

    public static bool enable_Rotation;
    public float rotation_Speed;

    private void Update()
    {
        if (Galaxy.enable_Rotation)
        { 
            float rSpeed = 2 * rotation_Speed * Time.deltaTime;

            transform.Rotate(0, -rSpeed, 0);
        }
    }
}
