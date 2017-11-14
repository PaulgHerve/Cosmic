using UnityEngine;

public class Icon_Controller : MonoBehaviour {

    public Color32[] star_Glow_Colors;
    public Color32[] star_Haze_Colors;

    private static Color32[] star_Glows;
    private static Color32[] star_Hazes;

    private void Awake()
    {
        star_Glows = star_Glow_Colors;
        star_Hazes = star_Haze_Colors;
    }

    public static Color32[] Get_Star_Glow_Colors()
    {
        return star_Glows;
    }

    public static Color32 Get_Star_Glow_Color(Star.starType type)
    {
        return star_Glows[(int)type];
    }

    public static Color32[] Get_Star_Haze_Colors()
    {
        return star_Hazes;
    }

    public static Color32 Get_Star_Haze_Color(Star.starType type)
    {
        return star_Hazes[(int)type];
    }
}
