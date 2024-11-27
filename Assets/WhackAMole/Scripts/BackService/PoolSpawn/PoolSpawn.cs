using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class PoolSpawn : MonoBehaviour
    {
        private static PoolSpawn instance;
        public static PoolSpawn Instance => instance;
        private Dictionary<string, Pool<GameObject>> poolDict = new Dictionary<string, Pool<GameObject>>();
        public PoolSpawnID[] particleSpawns;

        private void Awake()
        {
            instance = this;
            for (int i = 0; i < particleSpawns.Length; i++)
            {
                if (!poolDict.ContainsKey(particleSpawns[i].id))
                {
                    var pool = new Pool<GameObject>(0, Pool_OnObjectCreated, Pool_OnObjectRecycle, Pool_OnGet, Pool_OnDestroy, particleSpawns[i].gameObject);
                    poolDict.Add(particleSpawns[i].id, pool);
                }
                else
                {
                    Debug.LogWarning($"[PoolSpawn.Awake] {particleSpawns[i].id} Already exist");
                }
            }
        }

        public GameObject Spawn(string id)
        {
            if (poolDict.ContainsKey(id))
            {
                return poolDict[id].Get();
            }
            else
            {
                Debug.LogWarning($"[PoolSpawn.Spawn] {id} Doesn't exist in Pool Spawn");
                return null;
            }
        }

        public GameObject Spawn(PoolSpawnID poolSpawnId)
        {
            return Spawn(poolSpawnId.id);
        }

        public T Spawn<T>(PoolSpawnID poolSpawnId)
        {
            return Spawn(poolSpawnId.id).GetComponent<T>();
        }

        public void UnSpawn(PoolSpawnID go)
        {
            if (!go.gameObject.activeSelf)
                return;

            if (poolDict.ContainsKey(go.id))
            {
                poolDict[go.id].Recycle(go.gameObject);
            }
            else
            {
                Debug.LogWarning($"[PoolSpawn.UnSpawn] {go.id} Doesn't exist in Pool Spawn");
            }
        }

        public void UnSpawn(GameObject go)
        {
            var poolSpawnID = go.GetComponent<PoolSpawnID>();
            if (poolSpawnID)
            {
                UnSpawn(poolSpawnID);
            }
        }

        private GameObject Pool_OnObjectCreated(GameObject go)
        {
            var goInstance = Instantiate(go);
            goInstance.transform.SetParent(transform);
            return goInstance;
        }

        private void Pool_OnObjectRecycle(GameObject go)
        {
            go.transform.SetParent(transform);
            go.gameObject.SetActive(false);
        }

        private void Pool_OnGet(GameObject go)
        {
            go.gameObject.SetActive(true);
        }

        private void Pool_OnDestroy(GameObject go)
        {
            Destroy(go);
        }

        public void ResetState()
        {
            foreach (var item in poolDict)
            {
                item.Value.Destroy();
            }
        }
    }
}