using UnityEngine;

public class Star_Effects : MonoBehaviour {

    private SpriteRenderer[] visual_Effects;

    private void Awake()
    {
        visual_Effects = GetComponentsInChildren<SpriteRenderer>();
    }

    public void Set_Glow(Color32 color)
    {
        visual_Effects[0].color = color;
    }

    public void Set_Haze(Color32 color)
    {
        visual_Effects[1].color = color;
    }
}
