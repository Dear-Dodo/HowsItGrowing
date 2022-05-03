/*
 * Author: Alex Manning
 * Date: 26/10/2021
 * Folder Location: Assets/Scripts/Tools/SceneManagement
 */

using UnityEngine.SceneManagement;

public static class SceneController
{
    //loads a scene by path
    public static void Load(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    //loads a scene by build index
    public static void Load(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}