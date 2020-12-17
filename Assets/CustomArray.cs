using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public class CustomArray
{
    public static Cell[] GetColumn(Cell[,] matrix, int columnNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(0))
                .Select(x => matrix[x, columnNumber])
                .ToArray();
    }

    public static Cell[] GetRow(Cell[,] matrix, int rowNumber)
    {
        return Enumerable.Range(0, matrix.GetLength(1))
                .Select(x => matrix[rowNumber, x])
                .ToArray();
    }
}



public static class Extensions
{
    public static Cell[] Clone<Cell>(this Cell[] array) where Cell : ICloneable<Cell>
    {
        var newArray = new Cell[array.Length];
        for (var i = 0; i < array.Length; i++)
            newArray[i] = array[i].Clone();
        return newArray;
    }
    public static IEnumerable<T> Clone<T>(this IEnumerable<T> items) where T : ICloneable<T>
    {
        foreach (var item in items)
            yield return item.Clone();
    }


    public interface ICloneable<T> where T : ICloneable<T>
    {
       T Clone();
    }

    public static T Clone<T>(this T source)
    {
        if (!typeof(T).IsSerializable)
        {
            throw new ArgumentException("The type must be serializable.", "source");
        }

        // Don't serialize a null object, simply return the default for that object
        if (System.Object.ReferenceEquals(source, null))
        {
            return default(T);
        }

        IFormatter formatter = new BinaryFormatter();
        using (Stream stream = new MemoryStream())
        {
            formatter.Serialize(stream, source);
            stream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(stream);
        }
    }
}

