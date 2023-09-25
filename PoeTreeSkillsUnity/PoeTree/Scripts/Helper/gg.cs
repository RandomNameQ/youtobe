using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public static class gg
{
    // возвращает случаный элемент из листа\массива\енама
    private static Random random = new Random();

    public static T ReturnRandomValue<T>(IEnumerable<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        T[] array = collection.ToArray();
        if (array.Length == 0)
            throw new ArgumentException("Collection is empty", nameof(collection));

        int randomIndex = random.Next(0, array.Length);
        return array[randomIndex];
    }

    public static T ReturnRandomValue<T>()
    {
        Type enumType = typeof(T);
        if (!enumType.IsEnum)
            throw new ArgumentException("T must be an enum type");

        Array values = Enum.GetValues(enumType);
        int randomIndex = random.Next(0, values.Length);
        return (T)values.GetValue(randomIndex);
    }

    // случанйо сортируем лист
    public static List<T> Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
        return list;
    }

}
