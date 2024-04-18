using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public void Init()
    {
        
    }

    public T Load<T>(string path) where T : Object
    {
        T resource = Resources.Load<T>(path);

        if (resource == null)
        {
            Debug.Log($"Failed to load Resource : {path}");
            return null;
        }

        return resource;
    }

    public T[] LoadAll<T>(string path) where T : Object
    {
        T[] resources = Resources.LoadAll<T>(path);

        if (resources == null || resources.Length == 0)
        {
            Debug.Log($"Failed to load Resources : {path}");
            return null;
        }

        return resources;
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");

        if (prefab == null)
        {
            Debug.Log($"Failed to Instantiate : {path}");
            return null;
        }

        GameObject gameObject = Instantiate(prefab, parent);
        int cloneIndex = gameObject.name.IndexOf("(Clone)");
        if (cloneIndex != -1)
            gameObject.name = gameObject.name.Substring(0, cloneIndex);

        return gameObject;
    }

    public void Destroy(GameObject gameObject)
    {
        if (gameObject == null)
        {
            Debug.Log($"Failed to Destroy : {gameObject.name}");
            return;
        }
        Object.Destroy(gameObject);
    }
}
