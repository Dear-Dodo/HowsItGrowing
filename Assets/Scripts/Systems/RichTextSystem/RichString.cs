/*
 *  Author: James Greensill
 *  Folder Location: Assets/Scripts/Systems/RichTextSystem
 */

using UnityEngine;

[System.Serializable]
public class RichString
{
    [TextArea]
    public string Text;

    public bool Bold;
    public bool Italic;

    public RichString(string text)
    {
        Text = text;
    }

    public static implicit operator RichString(string value)
    {
        return new RichString(value);
    }

    /// <summary>
    /// Parses string depending on boolean conditions.
    /// </summary>
    /// <returns>Parsed Strings.</returns>
    public string GetParsed()
    {
        string tempString = "";

        if (Bold)
        {
            tempString += "<b>";
        }

        if (Italic)
        {
            tempString += "<i>";
        }

        tempString += Text;

        if (Italic)
        {
            tempString += "</i>";
        }

        if (Bold)
        {
            tempString += "</b>";
        }

        return tempString;
    }
}