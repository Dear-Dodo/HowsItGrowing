/*
 *  Author: Calvin Soueid
 *  Date:   15/11/2021
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class Reflection
    {
        public static FieldInfo FindFieldByType(object source, Type target)
        {
            FieldInfo[] fields = source.GetType().GetFields();

            return fields.First(f => f.FieldType == target);

        }
    }
}
