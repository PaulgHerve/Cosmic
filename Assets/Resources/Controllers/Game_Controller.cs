using UnityEngine;

public class Game_Controller : MonoBehaviour {

    public enum gameState { MENU, PLAY, SETUP };

    public bool enable_Galaxy_Rotation;

    private gameState current_Game_State;
}
