using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Galaxy_Generator : MonoBehaviour
{

    private static Sprite[] star_Sprites;

    private float galaxySize = 1f;
    private int age = 0;
    private int armPairs = 1;
    private float armDensity = .5f;
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
        return .5f / armPairs;
    }

    public void Generate_Galaxy()
    {
        Destroy_Galaxy();

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
                Generate_Star(galaxy, a, armVal, armInc);
            }
        }

        galaxy.transform.Rotate(0, 10, 15, Space.Self);
    }

    //Generates a star and determines it's starting location
    private Star Generate_Star(GameObject galaxy, int arm, float armVal, float armInc)
    {
        GameObject starObject = Instantiate(star_Prefab);
        Star star = starObject.GetComponent<Star>();

        stars.Add(star);

        star.Generate(galaxySize, armVal, armInc, armDensity);
        star.transform.SetParent(galaxy.transform, false);

        return star;
    }

    private void Destroy_Galaxy()
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

    public void SetGalaxySize(Slider s)
    {
        int val = (int)s.value;
        float newVal = val / 6.00f;

        galaxySize = newVal;
    }

    public void SetAge(Slider s)
    {
        int val = (int)s.value;

        age = val;

        Update_Star_Locations();
    }

    public void SetArmCount(Slider s)
    {
        int val = (int)s.value;
        armPairs = val;
    }

    public void SetArmDensity(Slider s)
    {
        float val = s.value;
        armDensity = 1 - val;
    }

    public void Update_Star_Locations()
    {
        for (int i = 0; i < stars.Count; i++)
        {
            Star item = stars[i];

            item.Update_Location(age);
        }
    }
}
