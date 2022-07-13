using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    static object instanceLock = new object();

    public static T Ins
    {
        get
        {
            lock(instanceLock)
            {
                if(instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));
                    if(instance == null)
                    {
                        GameObject singletonObj = new GameObject();
                        instance = singletonObj.AddComponent<T>();
                        singletonObj.name = typeof(T).ToString() + " (Singleton)";
                        Debug.Log(singletonObj.name);
                    }
                }
                return instance;
            }
        }
    }
}
