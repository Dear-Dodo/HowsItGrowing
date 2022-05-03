/*
 *  Author: James Greensill
 *  Folder Location: Assets/Scripts/Systems/Sequence System
 */

using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HowsItGrowing.Sequencing
{
    /// <summary>
    /// SequenceTimeline is a system that sequentially plays callbacks via time delays.
    /// It piggy backs off of the DoTween Tweening Library.
    /// </summary>
    [System.Serializable]
    public class SequenceTimeline : MonoBehaviour
    {
        public List<SequenceEvent> SequenceEvents;

        public float CollatedTime;

        public Sequence DtSequence;

        /// <summary>
        /// Adds a single sequence event.
        /// </summary>
        /// <param name="sequenceEvent"></param>
        public void AddSequenceEvent(SequenceEvent sequenceEvent)
        {
            if (sequenceEvent == null)
                throw new NullReferenceException("Provided Sequence event is null.");

            for (var i = 0; i < sequenceEvent.AmountOfTimesToBePlayed; i++)
            {
                SequenceEvents.Add(sequenceEvent);
                CollatedTime += sequenceEvent.TimeDelay;
            }
        }

        /// <summary>
        /// Adds a collection of sequence events.
        /// </summary>
        /// <param name="sequenceEvents"></param>
        public void AddSequenceEvents(SequenceEvent[] sequenceEvents)
        {
            foreach (var sequenceEvent in sequenceEvents)
            {
                if (sequenceEvent == null)
                {
                    Debug.LogWarning("Provided Sequence Event is null.");
                    continue;
                }
                for (var i = 0; i < sequenceEvent.AmountOfTimesToBePlayed; i++)
                {
                    SequenceEvents.Add(sequenceEvent);
                    CollatedTime += sequenceEvent.TimeDelay;
                }
            }
        }


        /// <summary>
        /// Creates the sequence.
        /// </summary>
        public void Create()
        {
            DtSequence = DOTween.Sequence();

            foreach (var sequenceEvent in SequenceEvents)
            {
                DtSequence.AppendInterval(sequenceEvent.TimeDelay);
                DtSequence.AppendCallback(() => { sequenceEvent.Action?.Invoke(); });
            }
        }

        /// <summary>
        /// Plays the sequence.
        /// </summary>
        public void Play()
        {
            SequenceEvent ev = new SequenceEvent(() => { Debug.Log("Test"); }, .1f) { AmountOfTimesToBePlayed = 10 };

            AddSequenceEvent(ev);
            Create();
            DtSequence.Play();
        }
    }
}