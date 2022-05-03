using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GifAnimationEnd : MonoBehaviour
{
    public Animator Animator;
    public UnityEvent EndCallback;
    public string ClipName;

    public bool playing = false;

    public void Update()
    {
        if (this.Animator.GetCurrentAnimatorStateInfo(0).IsName(ClipName) && !playing)
        {
            playing = true;
            StartCoroutine(EndOfAnimation(Animator.GetCurrentAnimatorStateInfo(0).length));
        }
    }

    private IEnumerator EndOfAnimation(float time)
    {
        yield return new WaitForSeconds(time);
        EndCallback?.Invoke();
    }
}