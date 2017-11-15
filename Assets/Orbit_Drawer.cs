using UnityEngine;

public class Orbit_Drawer : MonoBehaviour
{
    public float ThetaScale = 0.01f;
    private int Size;
    private LineRenderer lineDrawer;
    private float Theta = 0f;

    private void Awake()
    {
        lineDrawer = GetComponent<LineRenderer>();        
    }

    public void Draw_Orbit(float radius)
    {
        Color32 color = new Color32(255, 255, 255, 220);

        Theta = 0f;
        Size = (int)((1f / ThetaScale) + 1f);
        
        for (int i = 0; i < Size; i++)
        {
            Theta += (2.0f * Mathf.PI * ThetaScale);

            float x = radius * Mathf.Cos(Theta);
            float y = 0;
            float z = radius * Mathf.Sin(Theta);

            lineDrawer.SetVertexCount(Size);
            lineDrawer.SetPosition(i, new Vector3(x, y, z));
        }
    }
}
