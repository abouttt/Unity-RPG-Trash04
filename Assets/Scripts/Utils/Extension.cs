using System.Text.RegularExpressions;
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

    public static string ToSnake(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        // �ҹ��� �Ǵ� ���ڿ� �빮�� ���̿� ������ھ �߰�
        str = Regex.Replace(str, "([a-z0-9])([A-Z])", "$1_$2");

        // �빮�ڰ� ���ӵ� ��, ������ ������ �빮�ڿ� �ҹ��� �Ǵ� ���� ���̿� ������ھ �߰�
        str = Regex.Replace(str, "([A-Z]+)([A-Z][a-z0-9])", "$1_$2");

        return str;
    }
}
