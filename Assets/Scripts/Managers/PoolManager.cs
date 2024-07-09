using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    #region Pool
    private class Pool
    {
        public GameObject Prefab { get; private set; }
        public Transform Root { get; private set; }
        public int Size { get; private set; }
        public int Max { get; private set; }

        private readonly HashSet<GameObject> _actives = new();
        private readonly List<GameObject> _deactives = new();

        public Pool(GameObject prefab, int size, int max)
        {
            Prefab = prefab;
            Root = new GameObject($"{prefab.name}_Root").transform;
            Max = max;

            for (int i = 0; i < size; i++)
            {
                var go = Create();
                if (go == null)
                {
                    break;
                }

                PushToDeactiveContainer(go);
            }
        }

        public bool Push(GameObject go)
        {
            if (!_actives.Remove(go))
            {
                return false;
            }

            PushToDeactiveContainer(go);

            return true;
        }

        public GameObject Pop(Transform parent)
        {
            GameObject go;

            int lastIndex = _deactives.Count - 1;
            if (lastIndex >= 0)
            {
                go = _deactives[lastIndex];
                _deactives.RemoveAt(lastIndex);
            }
            else
            {
                go = Create();
                if (go == null)
                {
                    return null;
                }
            }

            go.SetActive(true);
            go.transform.SetParent(parent == null ? Root : parent);
            _actives.Add(go);

            return go;
        }

        public void Clear()
        {
            foreach (var go in _actives)
            {
                Object.Destroy(go);
            }

            foreach (var go in _deactives)
            {
                Object.Destroy(go);
            }

            Size = 0;
            _actives.Clear();
            _deactives.Clear();
        }

        private GameObject Create()
        {
            if (Size == Max)
            {
                return null;
            }

            Size++;

            return Object.Instantiate(Prefab);
        }

        private void PushToDeactiveContainer(GameObject go)
        {
            go.transform.SetParent(Root);
            go.SetActive(false);
            _deactives.Add(go);
        }
    }
    #endregion

    /// <summary>
    /// If a pool does not exist when the Pop method is called, a pool is automatically created.
    /// </summary>
    public static bool AutoCreate { get; set; } = true;

    private Transform _root;
    private readonly Dictionary<string, Pool> _pools = new();

    public void Init()
    {
        if (_root == null)
        {
            _root = Util.FindOrInstantiate("Pool_Root").transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    public void CreatePool(GameObject prefab, int size = 1, int max = -1)
    {
        if (prefab == null)
        {
            Debug.Log($"[PoolManager/CreatePool] Prefab is null.");
            return;
        }

        string name = prefab.name;
        if (_pools.ContainsKey(name))
        {
            Debug.Log($"[PoolManager/CreatePool] {name} pool already exist.");
            return;
        }

        var pool = new Pool(prefab, size, max);
        pool.Root.SetParent(_root);
        _pools.Add(name, pool);
    }

    public bool Push(GameObject go)
    {
        if (go == null)
        {
            Debug.Log($"[PoolManager/Push] GameObject is null.");
            return false;
        }

        string name = go.name;
        if (!_pools.ContainsKey(name))
        {
            Debug.Log($"[PoolManager/Push] {name} pool no exist.");
            return false;
        }

        _pools[name].Push(go);

        return true;
    }

    public GameObject Pop(GameObject prefab, Transform parent = null)
    {
        string name = prefab.name;
        if (!_pools.TryGetValue(name, out var pool))
        {
            if (AutoCreate)
            {
                CreatePool(prefab);
                pool = _pools[name];
            }
            else
            {
                Debug.Log($"[PoolManager/Pop] {name} pool no exist.");
                return null;
            }
        }

        return pool.Pop(parent);
    }

    public void ClearPool(string name)
    {
        if (_pools.TryGetValue(name, out var pool))
        {
            pool.Clear();
        }
        else
        {
            Debug.Log($"[PoolManager/ClearPool] {name} pool no exist.");
        }
    }

    public void Clear()
    {
        foreach (var kvp in _pools)
        {
            kvp.Value.Clear();
        }

        foreach (Transform child in _root)
        {
            Object.Destroy(child.gameObject);
        }

        _pools.Clear();
    }
}
