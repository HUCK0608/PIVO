using UnityEngine;

public static class CUtility
{
    /// <summary>string to bool</summary>
    public static bool ToBoolean(this string str)
    {
        if (str.Equals("True"))
            return true;
        else
            return false;
    }
}
