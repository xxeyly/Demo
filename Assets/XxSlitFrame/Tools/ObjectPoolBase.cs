using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace XxSlitFrame.Tools
{
    public class ObjectPoolBase : MonoBehaviour
    {
        private Dictionary<int, Stack<GameObject>> m_GameObjectPool = new Dictionary<int, Stack<GameObject>>();
        private Dictionary<int, int> m_InstantiatedGameObjects = new Dictionary<int, int>();
        private Dictionary<int, GameObject> m_OriginalObjectIDs = new Dictionary<int, GameObject>();
        private static ObjectPoolBase s_Instance;
        private static bool s_Initialized;

        [Tooltip("Specifies any prefabs that should be preloaded upon start.")] [HideInInspector] [SerializeField]
        protected ObjectPoolBase.PreloadedPrefab[] m_PreloadedPrefabs;

        private static ObjectPoolBase Instance
        {
            get
            {
                if (!ObjectPoolBase.s_Initialized)
                {
                    ObjectPoolBase.s_Instance = new GameObject("Object Pool").AddComponent<ObjectPoolBase>();
                    ObjectPoolBase.s_Initialized = true;
                }

                return ObjectPoolBase.s_Instance;
            }
        }

        public static void DomainReset()
        {
            ObjectPoolBase.s_Initialized = false;
            ObjectPoolBase.s_Instance = (ObjectPoolBase) null;
        }

        public ObjectPoolBase.PreloadedPrefab[] PreloadedPrefabs
        {
            get { return this.m_PreloadedPrefabs; }
        }

        private void OnEnable()
        {
            if (!((UnityEngine.Object) ObjectPoolBase.s_Instance == (UnityEngine.Object) null))
                return;
            ObjectPoolBase.s_Instance = this;
            ObjectPoolBase.s_Initialized = true;
            SceneManager.sceneUnloaded -= new UnityAction<Scene>(this.SceneUnloaded);
        }

        private void Start()
        {
            if (this.m_PreloadedPrefabs == null)
                return;
            List<GameObject> gameObjectList = new List<GameObject>();
            for (int index1 = 0; index1 < this.m_PreloadedPrefabs.Length; ++index1)
            {
                if (!(m_PreloadedPrefabs[index1].Prefab == null) && m_PreloadedPrefabs[index1].Count != 0)
                {
                    for (int index2 = 0; index2 < this.m_PreloadedPrefabs[index1].Count; ++index2)
                    {
                        if (index2 < gameObjectList.Count)
                            gameObjectList[index2] = Instantiate(this.m_PreloadedPrefabs[index1].Prefab);
                        else
                            gameObjectList.Add(Instantiate(this.m_PreloadedPrefabs[index1].Prefab));
                    }

                    for (int index2 = 0; index2 < this.m_PreloadedPrefabs[index1].Count; ++index2)
                        Destroy(gameObjectList[index2]);
                }
            }
        }

        public static GameObject Instantiate(GameObject original)
        {
            return Instantiate(original, Vector3.zero, Quaternion.identity, (Transform) null);
        }

        public static GameObject Instantiate(GameObject original, Transform parent)
        {
            return Instantiate(original, parent.position, parent.rotation, parent);
        }

        public static GameObject Instantiate(
            GameObject original,
            Transform parent,
            bool worldPositionStays)
        {
            return worldPositionStays ? Instantiate(original, Vector3.zero, Quaternion.identity, parent) : Instantiate(original, parent.position, parent.rotation, parent);
        }

        public static GameObject Instantiate(
            GameObject original,
            Vector3 position,
            Quaternion rotation,
            Transform parent = (Transform) null)
        {
            return ObjectPoolBase.Instance.InstantiateInternal(original, position, rotation, parent);
        }

        private GameObject InstantiateInternal(
            GameObject original,
            Vector3 position,
            Quaternion rotation,
            Transform parent)
        {
            int instanceId = original.GetInstanceID();
            GameObject gameObject = this.ObjectFromPool(instanceId, position, rotation, parent);
            if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
            {
                gameObject = UnityEngine.Object.Instantiate<GameObject>(original, position, rotation, parent);
                this.m_InstantiatedGameObjects.Add(gameObject.GetInstanceID(), instanceId);
                if (!this.m_OriginalObjectIDs.ContainsKey(instanceId))
                    this.m_OriginalObjectIDs.Add(instanceId, original);
            }
            else
            {
                gameObject.transform.position = position;
                gameObject.transform.rotation = rotation;
                gameObject.transform.SetParent(parent);
            }

            return gameObject;
        }

        private GameObject ObjectFromPool(
            int originalInstanceID,
            Vector3 position,
            Quaternion rotation,
            Transform parent)
        {
            Stack<GameObject> gameObjectStack;
            if (this.m_GameObjectPool.TryGetValue(originalInstanceID, out gameObjectStack))
            {
                while (gameObjectStack.Count > 0)
                {
                    GameObject gameObject = gameObjectStack.Pop();
                    if (!(gameObject == null))
                    {
                        gameObject.transform.position = position;
                        gameObject.transform.rotation = rotation;
                        gameObject.transform.SetParent(parent);
                        gameObject.SetActive(true);
                        this.m_InstantiatedGameObjects.Add(gameObject.GetInstanceID(), originalInstanceID);
                        return gameObject;
                    }
                }
            }

            return (GameObject) null;
        }

        public static bool InstantiatedWithPool(GameObject instantiatedObject)
        {
            return ObjectPoolBase.Instance.InstantiatedWithPoolInternal(instantiatedObject);
        }

        private bool InstantiatedWithPoolInternal(GameObject instantiatedObject)
        {
            return this.m_InstantiatedGameObjects.ContainsKey(instantiatedObject.GetInstanceID());
        }

        public static int OriginalInstanceID(GameObject instantiatedObject)
        {
            return ObjectPoolBase.Instance.OriginalInstanceIDInternal(instantiatedObject);
        }

        private int OriginalInstanceIDInternal(GameObject instantiatedObject)
        {
            int num;
            if (this.m_InstantiatedGameObjects.TryGetValue(instantiatedObject.GetInstanceID(), out num))
                return num;
            Debug.LogError((object) string.Format("Unable to get the original instance ID of {0}: has the object been placed in the ObjectPool?", (object) instantiatedObject));
            return -1;
        }

        public static void Destroy(GameObject instantiatedObject)
        {
            if ((UnityEngine.Object) ObjectPoolBase.Instance == (UnityEngine.Object) null)
                return;
            ObjectPoolBase.Instance.DestroyInternal(instantiatedObject);
        }

        private void DestroyInternal(GameObject instantiatedObject)
        {
            int instanceId = instantiatedObject.GetInstanceID();
            int originalInstanceID;
            if (!this.m_InstantiatedGameObjects.TryGetValue(instanceId, out originalInstanceID))
            {
                Debug.LogError((object) string.Format("Unable to pool {0} (instance {1}): the GameObject was not instantiated with ObjectPool.Instantiate.", (object) instantiatedObject.name,
                    (object) instanceId));
            }
            else
            {
                this.m_InstantiatedGameObjects.Remove(instanceId);
                this.DestroyLocal(instantiatedObject, originalInstanceID);
            }
        }

        private void DestroyLocal(GameObject instantiatedObject, int originalInstanceID)
        {
            instantiatedObject.SetActive(false);
            instantiatedObject.transform.SetParent(this.transform);
            Stack<GameObject> gameObjectStack1;
            if (this.m_GameObjectPool.TryGetValue(originalInstanceID, out gameObjectStack1))
            {
                gameObjectStack1.Push(instantiatedObject);
            }
            else
            {
                Stack<GameObject> gameObjectStack2 = new Stack<GameObject>();
                gameObjectStack2.Push(instantiatedObject);
                this.m_GameObjectPool.Add(originalInstanceID, gameObjectStack2);
            }
        }

        public static bool IsPooledObject(GameObject instantiateObject)
        {
            return ObjectPoolBase.Instance.IsPooledObjectInternal(instantiateObject);
        }

        public bool IsPooledObjectInternal(GameObject instantiateObject)
        {
            return this.m_InstantiatedGameObjects.ContainsKey(instantiateObject.GetInstanceID());
        }

        public static GameObject GetOriginalObject(GameObject instantiatedObject)
        {
            return (UnityEngine.Object) ObjectPoolBase.Instance == (UnityEngine.Object) null ? (GameObject) null : ObjectPoolBase.Instance.GetOriginalObjectInternal(instantiatedObject);
        }

        private GameObject GetOriginalObjectInternal(GameObject instantiatedObject)
        {
            int key;
            if (!this.m_InstantiatedGameObjects.TryGetValue(instantiatedObject.GetInstanceID(), out key))
                return (GameObject) null;
            GameObject gameObject;
            return !this.m_OriginalObjectIDs.TryGetValue(key, out gameObject) ? (GameObject) null : gameObject;
        }

        private void SceneUnloaded(Scene scene)
        {
            s_Initialized = false;
            s_Instance = null;
            SceneManager.sceneUnloaded -= this.SceneUnloaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneUnloaded += this.SceneUnloaded;
        }

        [Serializable]
        public struct PreloadedPrefab
        {
#pragma warning disable 649
            [Tooltip("The prefab that should be preloaded.")] [SerializeField]
            private GameObject m_Prefab;

            [Tooltip("Number of prefabs to instantiate upon start.")] [SerializeField]
            private int m_Count;
#pragma warning restore 649
            public GameObject Prefab
            {
                get { return this.m_Prefab; }
            }

            public int Count
            {
                get { return this.m_Count; }
            }
        }
    }
}