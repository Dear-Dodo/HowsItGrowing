/*
 *  Author: James Greensill
 *  Date:   25.10.2021
 *  Folder Location: Assets/Scripts/Tools/Debugging/StackTracer.cs
 */

using System.Diagnostics;

public class StackTracer
{
    private readonly StackTrace m_Trace;

    public StackTracer()
    {
        m_Trace = new StackTrace();
    }

    /// <summary>
    /// Gets the current threads stack trace from creation to deletion.
    /// </summary>
    /// <returns></returns>
    public string GetTrace()
    {
        return m_Trace != null ? m_Trace.ToString() : "trace is invalid.";
    }
}