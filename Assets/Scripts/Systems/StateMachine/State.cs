/*
 *  Author: Calvin Soueid
 *  Date:   27/10/2021
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// Base class for StateMachine states.
    /// </summary>
    public abstract class State
    {
        protected Transition[] transitions;

        /// <summary>
        /// Run the begin behaviour for this state
        /// </summary>
        public abstract void Start();
        /// <summary>
        /// Run constant behaviour for this state
        /// </summary>
        public abstract void Update();
        /// <summary>
        /// Run the ending behaviour for this state
        /// </summary>
        public abstract void End();

        /// <summary>
        /// Check for the first valid (if any) transition within this state.
        /// Returns null on fail.
        /// </summary>
        /// <returns>Target state on success; null on failure.</returns>
        public State TestTransitions(out TestResult result)
        {
            result = new TestResult();
            foreach(Transition transition in transitions)
            {
                result = transition.TestConditions();
                if (result.Target != null)
                {
                    return result.Target;
                }
            }

            return null;
        }
    }

}