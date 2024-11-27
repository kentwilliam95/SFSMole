using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class PoolSpawnID : MonoBehaviour
    {
        public string id;
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(id))
                id = gameObject.name;
        }
    }
}
