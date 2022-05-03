/*
 *  Author: Calvin Soueid
 *  Date:   9/11/2021
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plant
{
    [Serializable]
    public struct EnvironmentSelector<T> where T : Enum
    {
        public Interactable TriggerObject;
        public T TargetValue;
    }
}
