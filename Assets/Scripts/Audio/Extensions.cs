public static class Extensions
{
    public static bool TryAdd<TKey, TValue>(this System.Collections.Generic.Dictionary<TKey, TValue> dict, TKey key, TValue value)
    {
        if (dict.ContainsKey(key))
            return false;

        dict.Add(key, value);
        return true;
    }
}