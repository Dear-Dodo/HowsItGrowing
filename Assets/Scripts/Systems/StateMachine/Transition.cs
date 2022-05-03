/*
 *  Author: Calvin Soueid
 *  Date:   27/10/2021
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public struct TestResult
    {
        public State Target;

        public List<(string, ConditionResult)> FailedConditions;

        public TestResult(State target, int maxConditions)
        {
            Target = target;
            FailedConditions = new List<(string, ConditionResult)>(maxConditions);
        }
    }
    /// <summary>
    /// Possible outcomes of transition test states. TooHigh indicates that the tested value is above the expected value while TooLow indicates the opposite.
    /// Correct indicates a successful test, and invalid is a catch-all fail state for when TooHigh/Low are not applicable
    /// </summary>
    public enum ConditionResult
    {
        TooLow = -1,
        Correct = 0,
        TooHigh = 1,
        Incorrect
    }

    public class Transition
    {
        State targetState;
        // As much as I would like to change this to a custom delegate, the .Net standards recommend func for uniformity.
        // ~Cal
        List<Func<(string, ConditionResult)>> conditions;

        /// <summary>
        /// Tests conditions for this transition and returns the target state if all succeed.
        /// </summary>
        /// <returns>The target state on success; null on failure</returns>
        public TestResult TestConditions()
        {
            TestResult testResult = new TestResult(targetState, conditions.Count);
            for(int i = 0; i < conditions.Count; i++)
            {
                (string, ConditionResult) result = conditions[i]();
                if (result.Item2 != ConditionResult.Correct)
                {
                    testResult.Target = null;
                    testResult.FailedConditions.Add(result);
                }
            }
            return testResult;
        }

        /// <summary>
        /// Add the specified condition to this transition
        /// </summary>
        public void AddConditions(params Func<(string, ConditionResult)>[] toAdd)
        {
            conditions.AddRange(toAdd);
        }

        public Transition(State next, params Func<(string, ConditionResult)>[] transitionConditions)
        {
            if (next == null)
            {
                throw new ArgumentNullException("Target state of transition may not be null.");
            }
            targetState = next;

            conditions = new List<Func<(string, ConditionResult)>>(transitionConditions);
        }

        public Transition(State next)
        {
            //I needed it to be able to be null in case there was no next state, will come up with a better solution later - A
            /*if (next == null)
            {
                throw new ArgumentNullException("Target state of transition may not be null.");
            }*/
            targetState = next;

            conditions = new List<Func<(string, ConditionResult)>>();
        }
    } 
}
