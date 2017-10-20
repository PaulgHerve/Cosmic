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
        float inc = DetermineArmIncrement(armPairs);
        float val = inc * arm;
        return val;
    }

    private float DetermineArmIncrement(int armPairs)
    {
        return 1.00f / armPairs;
    }

    public void GenerateGalaxy()
    {
        DestroyGalaxy();

        int totalArms = armPairs;
        int Randomizer = Random.Range(500, 601);
        float armInc = DetermineArmIncrement(armPairs);
        float armVal;
        starCount = (int)(galaxySize * Randomizer);

        GameObject galaxy = Instantiate(galaxy_Prefab);

        for (int a = 0; a < totalArms; a++)
        {
            int stars = (starCount / armPairs);
            armVal = DetermineArmValue(armPairs, a);

            for (int i = 0; i < stars; i++)
            {
                GenerateStar(galaxy, a, armVal, armInc);
            }            
        }

        galaxy.transform.Rotate(0, 10, 15, Space.Self);
    }

    private Star GenerateStar(GameObject galaxy, int arm, float armVal, float armInc)
    {      
        GameObject starObject = Instantiate(star_Prefab);
        Star star = starObject.GetComponent<Star>();

        stars.Add(star);

        star.Generate(galaxySize, armVal, armInc);
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
