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

    private List<Star> stars = new List<Star>();
    
    private void Awake()
    {
        galaxy_Prefab = Resources.Load<GameObject>("Prefabs/Galaxy");
        star_Sprites = Resources.LoadAll<Sprite>("Stars/Sprites");
        star_Prefab = Resources.Load<GameObject>("Stars/Prefabs/Star");
    }

    private float DetermineArmValue(int armPairs, int arm)
    {
        float val = (float)armPairs / (float)arm;
        val *= 360;

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

            GameObject newArm = Instantiate(arm_Prefab);
            newArm.transform.SetParent(galaxy.transform);

            for (int i = 0; i < stars; i++)
            {
                Star item = GenerateStar(galaxy, a);

                item.transform.SetParent(newArm.transform, false);
            }

            newArm.transform.Rotate(0, (180 * a), 0, Space.Self);
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
}
