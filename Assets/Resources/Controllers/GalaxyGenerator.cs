using UnityEngine;
using System.Collections.Generic;

public class GalaxyGenerator : MonoBehaviour {

    private static Sprite[] star_Sprites;

    public float galaxySize = 1;
    private int starCount;
    private GameObject galaxy_Prefab;
    private GameObject star_Prefab;

    private List<Star> stars = new List<Star>();
    
    private void Awake()
    {
        galaxy_Prefab = Resources.Load<GameObject>("Prefabs/Galaxy");
        star_Sprites = Resources.LoadAll<Sprite>("Stars/Sprites");
        star_Prefab = Resources.Load<GameObject>("Stars/Prefabs/Star");
    }

    public void GenerateGalaxy()
    {
        DestroyGalaxy();

        int Randomizer = Random.Range(500, 601);
        starCount = (int)(galaxySize * Randomizer);

        GameObject galaxy = Instantiate(galaxy_Prefab);

        for (int i = 0; i < starCount; i++)
        {
            GenerateStar(galaxy);
        }

        galaxy.transform.Rotate(0, 10, 15, Space.Self);
    }

    private Star GenerateStar(GameObject galaxy)
    {
        GameObject starObject = Instantiate(star_Prefab);
        Star star = starObject.GetComponent<Star>();

        stars.Add(star);

        star.Generate(galaxySize);
        star.transform.SetParent(galaxy.transform);

        return star;
    }

    private void DestroyGalaxy()
    {
        Galaxy g = FindObjectOfType<Galaxy>();

        if (g)
        {
            Destroy(g.gameObject);
        }

        stars.Clear();
    }

    public static Sprite[] Get_Star_Sprites()
    {
        return star_Sprites;
    }

    public static Sprite Get_Star_Sprite(int index)
    {
        return star_Sprites[index];
    }
}
