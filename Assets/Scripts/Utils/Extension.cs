using UnityEngine;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    public static GameObject FindChild(this GameObject go, string name = null, bool recursive = false)
    {
        return Util.FindChild(go, name, recursive);
    }

    public static T FindChild<T>(this GameObject go, string name = null, bool recursive = false) where T : Object
    {
        return Util.FindChild<T>(go, name, recursive);
    }

    public static string GetStringAfterLastSlash(this string str)
    {
        return Util.GetStringAfterLastSlash(str);
    }

    public static bool CheckLayers(this GameObject go, LayerMask layerMask)
    {
        return Util.CheckLayers(go, layerMask);
    }
}
