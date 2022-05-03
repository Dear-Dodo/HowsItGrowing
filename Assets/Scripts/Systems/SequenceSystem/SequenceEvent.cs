/*
 *  Author: James Greensill
 *  Folder Location: Assets/Scripts/Systems/Sequence System
 */

using System;
using UnityEngine;
using UnityEngine.Events;

namespace HowsItGrowing.Sequencing
{
    /// <summary>
    /// Sequence Event is used by the Sequence Timeline to sequentially play callbacks with time delays.  
    /// </summary>
    [System.Serializable]
    public class SequenceEvent
    {
        [SerializeField] public UnityEvent Action;
        [SerializeField] public float TimeDelay;
        [SerializeField] public int AmountOfTimesToBePlayed;

        public SequenceEvent()
        {
            Action = new UnityEvent();
        }

        public SequenceEvent(Action callback) : this(callback, 0)
        {
        }

        public SequenceEvent(Action callback, float timeDelay) : this()
        {
            if (callback != null)
                Action.AddListener(callback.Invoke);

            TimeDelay = timeDelay;
        }

        public SequenceEvent(Action[] callbacks) : this()
        {
            if (callbacks == null) return;
            foreach (var callback in callbacks)
            {
                if (callback != null)
                    Action.AddListener(callback.Invoke);
            }
        }
    }
}