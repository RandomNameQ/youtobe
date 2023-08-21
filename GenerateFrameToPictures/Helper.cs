using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    private static List<UnityEngine.Sprite> _spritesController = new();

    public static List<UnityEngine.Sprite> SpritesController
    {
        get { return _spritesController; }
        set { _spritesController = value; }
    }


    private static System.Random rng = new System.Random();
    public static int RandomBetween(int minNumber, int maxNumber)
    {
        int rand = 0;
        rand = Random.RandomRange(minNumber, maxNumber);
        return rand;
    }

    public static GameObject MainParent(GameObject child)
    {

        GameObject parent = child.transform.parent.gameObject;
        for (int i = 0; i < 100; i++)
        {
            int boxColliderCount = parent.GetComponents<BoxCollider>().Length;

            if (boxColliderCount >= 2)
            {
                return parent;
            }
            parent = parent.transform.parent.gameObject;
        }
        return null;
    }

    public static void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


}
