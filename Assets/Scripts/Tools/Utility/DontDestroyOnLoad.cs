using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    [SerializeField] public bool Active = true;

    private void Awake()
    {
        if (Active)
            DontDestroyOnLoad(this.gameObject);
    }
}