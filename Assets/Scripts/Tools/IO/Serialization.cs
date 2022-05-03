/*
 *  Author:     James Greensill
 *  Date:       25/10/2021
 *  Location:   Assets/Scripts/Tools/IO/Serialization.cs
 */


using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class Serialization
{
    /// <summary>
    /// Deserializes data and inserted to value passed by reference.
    /// </summary>
    /// <typeparam name="T">Can be any type.</typeparam>
    /// <param name="path">The specified directory.</param>
    /// <param name="fileName">The file name.</param>
    /// <param name="value">Value passed by reference.</param>
    /// <returns>Returns true if the data was successfully deserialized.</returns>
    public static bool Deserialize<T>(string path, string fileName, ref T value)
    {
        try
        {
            // Concatenates the final path.
            var newPath = $"{path}/{fileName}";

            // Verifies that the file exists.
            if (!File.Exists(newPath))
            {
                return false;
            }

            // Binary Formatter.
            var formatter = new BinaryFormatter();

            // Opens verified file.
            var deserializeStream = File.Open(newPath, FileMode.Open);

            // Reads the binary data from the file.
            value = (T)formatter.Deserialize(deserializeStream);

            deserializeStream.Flush();
            deserializeStream.Close();
            deserializeStream.Dispose();

            return value != null;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed To Deserialize: {e.Data}");
            return false;
        }
    }

    /// <summary>
    ///  Serializes any object of type T into the specified location.
    /// </summary>
    /// <typeparam name="T">Can be any type.</typeparam>
    /// <param name="input">The Input data of Type T</param>
    /// <param name="path">The specified directory.</param>
    /// <param name="fileName">The file name.</param>
    /// <returns>Returns true if the data was successfully serialized.</returns>
    public static bool Serialize<T>(T input, string path, string fileName)
    {
        try
        {
            // Concatenates the final path string.
            var newPath = $"{path}/{fileName}";

            var formatter = new BinaryFormatter();
            // Opens or Creates a file for writing.
            var serializeStream = File.OpenWrite(newPath);

            // Serializes the stream to the file.
            formatter.Serialize(serializeStream, input);

            // End stream.
            serializeStream.Flush();
            serializeStream.Close();
            serializeStream.Dispose();
            return true;
        }
        catch (Exception e)
        {
            Debug.Log($"Failed To Serialize: {e.Data}.");
            return false;
        }
    }
}