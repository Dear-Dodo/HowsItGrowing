using UnityEngine;
using UnityEngine.UIElements;

public class ParticleHandler : MonoBehaviour
{
    [SerializeField] public ParticleSystem ParticleSystemToPlay;

    [SerializeField] public bool PlayOnMouseDown;
    [SerializeField] public bool PlayOnUpdate;
    [SerializeField] public bool PlayOnAwake;
    [SerializeField] public bool PlayOnStart;

    public void Play() => ParticleSystemToPlay?.Play();

    private void Update()
    {
        if (PlayOnMouseDown)
            if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
            {
                Play();
                ParticleSystemToPlay.gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            }

        if (!PlayOnUpdate) return;
        if (!ParticleSystemToPlay.isPlaying)
            ParticleSystemToPlay?.Play();
    }

    private void Awake()
    {
        if (PlayOnAwake)
            ParticleSystemToPlay?.Play();
    }

    private void Start()
    {
        if (PlayOnStart)
            ParticleSystemToPlay?.Play();
    }
}