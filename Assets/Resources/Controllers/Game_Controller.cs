using UnityEngine;

public class Game_Controller : MonoBehaviour {

    public enum gameState { MENU, PLAY, SETUP };

    public bool enable_Galaxy_Rotation;

    private static gameState current_Game_State = gameState.MENU;
    
    void Start()
    {
        current_Game_State = gameState.SETUP;
    }

    public static gameState Get_Game_State()
    {
        return current_Game_State;
    }

    public void Start_Game()
    {
        current_Game_State = gameState.PLAY;
    }

    public static void Set_Game_State(gameState newState)
    {
        current_Game_State = newState;
    }    
}
