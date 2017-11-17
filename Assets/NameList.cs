using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class NameList : MonoBehaviour
{
    private static string[] star_Names;

    private string star_Names_File_Path = "Assets/Resources/NameLists/stars.txt";

    void Awake()
    {
        star_Names = Pull_Star_Names();
    }

    private string[] Pull_Star_Names()
    {
        string[] names = File.ReadAllLines(star_Names_File_Path);

        return names;
    }

    public static string[] Get_Star_Names(int num_Of_Items)
    {
        List<string> stars = new List<string>();
        List<string> names = new List<string>();
        string current = "";

        //Populates stars list for ref
        for (int i = 0; i < num_Of_Items; i++)
        {
            if (star_Names.Length > i)
            {
                stars.Add(star_Names[i]);
            }

            else
            {
                stars.Add("Nameless Star");
            }
        }

        for (int i = 0; i < num_Of_Items; i++)
        {
            int index = Random.Range(0, stars.Count);

            if (stars.Count > 1)
            {
                current = stars[index];
            }

            names.Add(current);
            stars.Remove(current);
            stars.TrimExcess();
        }

        return names.ToArray();
    }
}
