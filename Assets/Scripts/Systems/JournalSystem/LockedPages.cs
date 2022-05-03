using System.Collections.Generic;

public static class LockedPages
{
    public struct PageLink
    {
        public string JournalLink;
        public bool Locked;

        public PageLink(string journalName, bool locked)
        {
            JournalLink = journalName;
            Locked = locked;
        }
    }

    public static Dictionary<string, bool> Pages = new Dictionary<string, bool>()
    {
        ["Ranunculus"] = true,
        ["Banksia"] = true,
        ["Waratah"] = true,
    };

    public static bool Verify()
    {
        bool flag = false;
        foreach (var page in Pages)
        {
            flag = page.Value;
            if (flag)
                return false;
        }
        return true;
    }
}