using UnityEngine;

public class Game_Controller : MonoBehaviour {

    public enum gameState { MENU, PLAY, SETUP };

    public bool enable_Galaxy_Rotation;

    private static gameState current_Game_State = gameState.MENU;
    
    public static gameState Get_Game_State()
    {
        return current_Game_State;
    }

    public static void Set_Game_State(gameState newState)
    {
        current_Game_State = newState;
    }    
}
