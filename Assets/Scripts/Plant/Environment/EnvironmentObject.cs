/*
 *  Author: Calvin Soueid
 *  Date:   25/11/2021
 */

using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for interactable diegetic UI objects in the plant environment.
/// </summary>
public abstract class EnvironmentObject : MonoBehaviour, IDirtyable, IDisableable
{
    [SerializeField]
    protected GameObject lockUI;

    [SerializeField]
    protected ParticleSystem sparkles;

    [SerializeField]
    protected Renderer[] toSetGrayscale;

    [Space(10)]

    protected bool isActive;

    public virtual void SetActive(bool enabled)
    {
        isActive = enabled;
        lockUI?.SetActive(!enabled);
        
        foreach(Renderer renderer in toSetGrayscale)
        {
            renderer.material.SetFloat("Saturation", enabled ? 1 : 0);
        }
        
        if (enabled)
        {
            sparkles?.Play();
        }
        else
        {
            sparkles?.Stop();
        }
    }

    public abstract void SetDirty();
}
