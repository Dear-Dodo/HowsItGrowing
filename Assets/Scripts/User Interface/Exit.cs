/*
 * Authour: Calvin Soueid
 * Date: 22/11/2021
 * Location: Assets/Scripts/User Interface
 * 
 * Refactor: Ronan Richardson
 * Date: 1/12/2021
 */

using UnityEngine;

public class Exit : MonoBehaviour
{
    /// <summary>
    /// Awake() simply sets the exit object to be inactive in both the editor as well
    /// as WebGL builds, as quitting the application is not relevant.
    /// </summary>
    private void Awake()
    {
#if UNITY_WEBGL || UNITY_EDITOR
        gameObject.SetActive(false);        
#endif
    }

    /// <summary>
    /// Every frame, check for escape input and close the application if input is found.
    /// </summary>
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// QuitGame() simply calls Application.Quit. This is a public
    /// function that is called by the X button's OnClick event.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
