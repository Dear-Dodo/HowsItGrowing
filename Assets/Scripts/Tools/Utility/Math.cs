/*
 *  Author: Calvin Soueid
 *  Date:   27/10/2021
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class Math
    {
        /// <summary>
        /// Compare two integers and return a value based on their relative size.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>0 for equality, 1 for a > b, -1 for a < b</returns>
        public static int Compare(int a, int b)
        {
            if (a == b)
            {
                return 0;
            }
            if (a > b)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    } 
}
