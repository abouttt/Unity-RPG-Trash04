using System.Linq;
using UnityEngine;

public static class Util
{
    public static GameObject FindOrInstantiate(string name)
    {
        var go = GameObject.Find(name);
        if (go == null)
        {
            go = new GameObject(name);
        }

        return go;
    }

    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        if (!go.TryGetComponent(out T component))
        {
            component = go.AddComponent<T>();
        }

        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        var transform = FindChild<Transform>(go, name, recursive);
        return transform == null ? null : transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : Object
    {
        if (go == null)
        {
            return null;
        }

        if (recursive)
        {
            return go.GetComponentsInChildren<T>().FirstOrDefault(component => string.IsNullOrEmpty(name) || component.name.Equals(name));
        }
        else
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                var transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name.Equals(name))
                {
                    if (transform.TryGetComponent<T>(out var component))
                    {
                        return component;
                    }
                }
            }
        }
        return null;
    }

    public static string GetStringAfterLastSlash(string str)
    {
        string result = str;
        int index = str.LastIndexOf('/');
        if (index >= 0)
        {
            result = str[(index + 1)..];
        }

        return result;
    }

    public static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle > 180f)
        {
            return lfAngle - 360f;
        }

        if (lfAngle < -180f)
        {
            return lfAngle + 360f;
        }

        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public static bool CheckLayers(GameObject go, LayerMask layerMask)
    {
        if ((layerMask.value & (1 << go.layer)) == 0)
        {
            return false;
        }

        return true;
    }
}
