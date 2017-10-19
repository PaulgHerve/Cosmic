using UnityEngine;

public class Galaxy : MonoBehaviour {

    Game_Controller game_Control;
    public static bool enable_Rotation;

    private void Awake()
    {
        game_Control = FindObjectOfType<Game_Controller>();
    }

    void Update () {
        enable_Rotation = game_Control.enable_Galaxy_Rotation;

        if (enable_Rotation)
        {
            Rotate();
        }
	}

    void Rotate()
    {
        //transform.Rotate(0, -.1f, 0, Space.Self);
    }
}
