using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler {
    private List<GameObject> pooledObjects;
    private readonly GameObject objectToPool;
    private int amountToPool;
    public bool shouldExpand;
    private readonly Transform parent = null;
    public ObjectPooler(GameObject objectToPool, Transform parent = null, int amountToPool = 10, bool shouldExpand = true) {
        this.objectToPool = objectToPool;
        this.amountToPool = amountToPool;
        this.shouldExpand = shouldExpand;
        this.parent = parent;

        pooledObjects = new List<GameObject>();
        IntantiateObjects(amountToPool);
    }
    public string Info() {
        return $"{objectToPool.name} amount:{pooledObjects.Count}";
    }
    public void IncreaseAmount(int amountToAdd) {
        IntantiateObjects(amountToAdd);
    }
    public void DisableAllInactive() {
        for (int i = amountToPool; i > 0; i--) {
            if (pooledObjects[i].gameObject.activeInHierarchy) {
                amountToPool--;
                Object.Destroy(pooledObjects[i]);
                pooledObjects.RemoveAt(i);
            }
        }
    }
    public void DisableInactive(int amountToDisable) {
        for (int i = amountToPool; i > 0; i--) {
            if (amountToDisable >= 0)
                break;

            if (pooledObjects[i].gameObject.activeInHierarchy) {
                amountToDisable--;
                amountToPool--;
                Object.Destroy(pooledObjects[i]);
                pooledObjects.RemoveAt(i);
            }
        }
    }
    void IntantiateObjects(int amount) {
        for (int i = 0; i < amount; i++) {
            GameObject obj = Object.Instantiate(objectToPool, parent);
            obj.gameObject.SetActive(false);
            obj.gameObject.name += $"{i}";
            pooledObjects.Add(obj);
            //if (i % objectPerFrame == 0)  //5 objects per frame
            //    yield return null;
        }
    }
    public GameObject GetPooledObject(bool setActive = false) {
        for (int i = 0; i < amountToPool; i++) {
            if (!pooledObjects[i].gameObject.activeInHierarchy) {
                if (setActive)
                    pooledObjects[i].gameObject.SetActive(setActive);
                return pooledObjects[i];
            }
        }

        if (shouldExpand) {
            GameObject obj = Object.Instantiate(objectToPool, parent);
            obj.gameObject.name += $"{amountToPool++}";
            obj.gameObject.SetActive(setActive);
            pooledObjects.Add(obj);
            return obj;
        }
        else return null;
    }


    public GameObject GetPooledObject(Vector3 position, bool setActive = false, bool worldSpace = true) {
        return GetPooledObject(position, Quaternion.identity, setActive, worldSpace);
    }

    public GameObject GetPooledObject(Vector3 position, Quaternion rotation, bool setActive = false, bool worldSpace = true) {
        GameObject obj = GetPooledObject(false);
        if (worldSpace)
            obj.transform.position = position;
        else
            obj.transform.localPosition = position;
        obj.transform.rotation = rotation;
        obj.gameObject.SetActive(setActive);
        return obj;
    }

    public void DisablePooledObjects() {
        foreach (GameObject obj in pooledObjects) {
            obj.gameObject.SetActive(false);
        }
    }

    public void Destroy() {
        foreach (GameObject obj in pooledObjects) {
            Object.Destroy(obj);
        }
        pooledObjects.Clear();
        pooledObjects = null;
    }
    ~ObjectPooler() {
        if (pooledObjects != null && pooledObjects.Count > 0) {
            Destroy();
        }
    }
}
public class ObjectPooler<T> where T : Component {
    private List<T> pooledObjects;
    private readonly T objectToPool;
    private int amountToPool;
    public bool shouldExpand;
    private readonly Transform parent = null;
    public ObjectPooler(T objectToPool, Transform parent = null, int amountToPool = 10, bool shouldExpand = true) {
        this.objectToPool = objectToPool;
        this.amountToPool = amountToPool;
        this.shouldExpand = shouldExpand;
        this.parent = parent;

        pooledObjects = new List<T>();
        IntantiateObjects(amountToPool);
    }
    public string Info() {
        return $"{objectToPool.name} amount:{pooledObjects.Count}";
    }
    public void IncreaseAmount(int amountToAdd) {
        IntantiateObjects(amountToAdd);
    }
    public void DisableAllInactive() {
        for (int i = amountToPool; i > 0; i--) {
            if (pooledObjects[i].gameObject.activeInHierarchy) {
                amountToPool--;
                Object.Destroy(pooledObjects[i]);
                pooledObjects.RemoveAt(i);
            }
        }
    }
    public void DisableInactive(int amountToDisable) {
        for (int i = amountToPool; i > 0; i--) {
            if (amountToDisable >= 0)
                break;

            if (pooledObjects[i].gameObject.activeInHierarchy) {
                amountToDisable--;
                amountToPool--;
                Object.Destroy(pooledObjects[i]);
                pooledObjects.RemoveAt(i);
            }
        }
    }
    void IntantiateObjects(int amount) {
        for (int i = 0; i < amount; i++) {
            T obj = Object.Instantiate(objectToPool, parent);
            obj.gameObject.SetActive(false);
            obj.gameObject.name += $"{i}";
            pooledObjects.Add(obj);
            //if (i % objectPerFrame == 0)  //5 objects per frame
            //    yield return null;
        }
    }
    public T GetPooledObject(bool setActive = false) {
        for (int i = 0; i < amountToPool; i++) {
            if (!pooledObjects[i].gameObject.activeInHierarchy) {
                pooledObjects[i].gameObject.SetActive(setActive);
                return pooledObjects[i];
            }
        }

        if (shouldExpand) {
            T obj = Object.Instantiate(objectToPool, parent);
            obj.gameObject.name += $"{amountToPool++}";
            obj.gameObject.SetActive(setActive);
            pooledObjects.Add(obj);
            return obj;
        }
        else return null;
    }


    public T GetPooledObject(Vector3 position, bool setActive = false, bool worldSpace = true) {
        return GetPooledObject(position, Quaternion.identity, setActive, worldSpace);
    }

    public T GetPooledObject(Vector3 position, Quaternion rotation, bool setActive = false, bool worldSpace = true) {
        T obj = GetPooledObject(false);
        if (worldSpace)
            obj.transform.position = position;
        else
            obj.transform.localPosition = position;
        obj.transform.rotation = rotation;
        obj.gameObject.SetActive(setActive);
        return obj;
    }

    public void DisablePooledObjects() {
        foreach (T obj in pooledObjects) {
            obj.gameObject.SetActive(false);
        }
    }

    public void Destroy() {
        foreach (T obj in pooledObjects) {
            Object.Destroy(obj);
        }
        pooledObjects.Clear();
        pooledObjects = null;
    }
    ~ObjectPooler() {
        if (pooledObjects != null && pooledObjects.Count > 0) {
            Destroy();
        }
    }
}