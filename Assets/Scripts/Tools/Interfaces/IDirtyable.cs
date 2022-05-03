/*
 *  Author: Calvin Soueid
 *  Date:   15/11/2021
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    /// <summary>
    /// An object implementing this interface is expected to refresh data or visuals on SetDirty()
    /// </summary>
    public interface IDirtyable
    {
        void SetDirty();
    }
}
