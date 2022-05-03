/*
 * Author: Alex Manning
 * Date: 26/10/2021
 * Folder Location: Assets/Scripts/Tools/SceneManagement
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControllerInstance : MonoBehaviour
{
    public static SceneControllerInstance instance;

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
    }

    //loads a scene by path
    public void Load(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    //loads a scene by build index
    public void Load(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}