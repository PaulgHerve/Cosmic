using UnityEngine;
using System.Collections.Generic;

public class GalaxyGenerator : MonoBehaviour {

    private static Sprite[] star_Sprites;

    public float galaxySize = 1;
    public int armPairs;
    public GameObject arm_Prefab;
    private int starCount;
    private GameObject galaxy_Prefab;
    private GameObject star_Prefab;

    private static List<Star> stars = new List<Star>();
    
    private void Awake()
    {
        galaxy_Prefab = Resources.Load<GameObject>("Prefabs/Galaxy");
        star_Sprites = Resources.LoadAll<Sprite>("Stars/Sprites");
        star_Prefab = Resources.Load<GameObject>("Stars/Prefabs/Star");
    }

    private float DetermineArmValue(int armPairs, int arm)
    {
        float val = 0.00f;
        val += arm;
        val /= (armPairs * 2);

        if (val < 0) { val = 0; }

        return val;
    }

    public void GenerateGalaxy()
    {
        DestroyGalaxy();

        int armVal = armPairs * 2;
        int Randomizer = Random.Range(500, 601);
        starCount = (int)(galaxySize * Randomizer);

        GameObject galaxy = Instantiate(galaxy_Prefab);

        for (int a = 0; a < armVal; a++)
        {
            int stars = (int)(starCount / armPairs);
            
            for (int i = 0; i < stars; i++)
            {
                Star item = GenerateStar(galaxy, a);
            }            
        }

        galaxy.transform.Rotate(0, 10, 15, Space.Self);
    }

    private Star GenerateStar(GameObject galaxy, int arm)
    {
        float armVal = DetermineArmValue(armPairs, arm);

        GameObject starObject = Instantiate(star_Prefab);
        Star star = starObject.GetComponent<Star>();

        stars.Add(star);

        star.Generate(galaxySize, armVal);
        star.transform.SetParent(galaxy.transform, false);

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

    public static Star[] GetStars()
    {
        return stars.ToArray();
    }
}
