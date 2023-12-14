using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using LupinrangerPatranger.UIWidgets;
using LupinrangerPatranger.CharacterSystem.Configuration;

namespace LupinrangerPatranger.CharacterSystem
{
    public class CharacterManager : MonoBehaviour
    {
        /// <summary>
		/// Don't destroy this object instance when loading new scenes.
		/// </summary>
        public bool dontDestroyOnLoad = true;

        private static CharacterManager m_Current;
        /// <summary>
		/// The InventoryManager singleton object. This object is set inside Awake()
		/// </summary>
        public static CharacterManager current
        {
            get
            {
                Assert.IsNotNull(m_Current, "Requires an Character Manager.Create one from Tools > Treasure Collecting Adventure > Character System > Create Character Manager!");
                return m_Current;
            }
        }

        [SerializeField]
        private CharacterDatabase m_Database = null;

        public static CharacterDatabase Database
        {
            get
            {
                if (CharacterManager.current != null)
                {
                    Assert.IsNotNull(CharacterManager.current.m_Database, "Please assign CharacterDatabase to the Character Manager!");
                    return CharacterManager.current.m_Database;
                }
                return null;
            }
        }

        [SerializeField]
        private CharacterDatabase[] m_ChildDatabases = null;

        private static Default m_DefaultSettings;
        public static Default DefaultSettings
        {
            get
            {
                if (m_DefaultSettings == null)
                {
                    m_DefaultSettings = GetSetting<Default>();
                }
                return m_DefaultSettings;
            }
        }

        private static UI m_UI;
        public static UI UI
        {
            get
            {
                if (m_UI == null)
                {
                    m_UI = GetSetting<UI>();
                }
                return m_UI;
            }
        }

        private static Notifications m_Notifications;
        public static Notifications Notifications
        {
            get
            {
                if (m_Notifications == null)
                {
                    m_Notifications = GetSetting<Notifications>();
                }
                return m_Notifications;
            }
        }

        private static SavingLoading m_SavingLoading;
        public static SavingLoading SavingLoading
        {
            get
            {
                if (m_SavingLoading == null)
                {
                    m_SavingLoading = GetSetting<SavingLoading>();
                }
                return m_SavingLoading;
            }
        }

        private static Configuration.Input m_Input;
        public static Configuration.Input Input
        {
            get
            {
                if (m_Input == null)
                {
                    m_Input = GetSetting<Configuration.Input>();
                }
                return m_Input;
            }
        }

        //private static Configuration.PlayerInput m_InputA;
        //public static Configuration.PlayerInput InputA
        //{
        //    get
        //    {
        //        if (m_InputA == null)
        //        {
        //            m_InputA = GetSetting<Configuration.PlayerInput>();
        //        }
        //        return m_InputA;
        //    }
        //}

        private static T GetSetting<T>() where T : Settings
        {
            if (CharacterManager.Database != null)
            {
                return (T)CharacterManager.Database.settings.Where(x => x.GetType() == typeof(T)).FirstOrDefault();
            }
            return default(T);
        }

        protected static Dictionary<string, GameObject> m_PrefabCache;

        private PlayerInfo m_PlayerInfo;
        public PlayerInfo PlayerInfo
        {
            get
            {
                if (this.m_PlayerInfo == null) { this.m_PlayerInfo = new PlayerInfo(CharacterManager.DefaultSettings.playerTag); }
                return this.m_PlayerInfo;
            }
        }

        [HideInInspector]
        public UnityEvent onDataLoaded;
        [HideInInspector]
        public UnityEvent onDataSaved;

        protected static bool m_IsLoaded = false;
        public static bool IsLoaded { get => m_IsLoaded; }

        private void Awake()
        {
            if (CharacterManager.m_Current != null)
            {
                //if(CharacterManager.DefaultSettings.debugMessages)
                //  Debug.Log ("Multiple Character Manager in scene...this is not supported. Destroying instance!");
                Destroy(gameObject);
                return;
            }
            else
            {
                CharacterManager.m_Current = this;
                if (EventSystem.current == null)
                {
                    if (CharacterManager.DefaultSettings.debugMessages)
                        Debug.Log("Missing EventSystem in scene. Auto creating!");
                    new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
                }

                if (Camera.main != null && Camera.main.GetComponent<PhysicsRaycaster>() == null)
                {
                    if (CharacterManager.DefaultSettings.debugMessages)
                        Debug.Log("Missing PhysicsRaycaster on Main Camera. Auto adding!");
                    PhysicsRaycaster physicsRaycaster = Camera.main.gameObject.AddComponent<PhysicsRaycaster>();
                    physicsRaycaster.eventMask = Physics.DefaultRaycastLayers;
                }

                this.m_Database = ScriptableObject.Instantiate(this.m_Database);
                for (int i = 0; i < this.m_ChildDatabases.Length; i++)
                {
                    CharacterDatabase child = this.m_ChildDatabases[i];
                    this.m_Database.Merge(child);
                }

                m_PrefabCache = new Dictionary<string, GameObject>();
                UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ChangedActiveScene;

                if (dontDestroyOnLoad)
                {
                    if (transform.parent != null)
                    {
                        if (CharacterManager.DefaultSettings.debugMessages)
                            Debug.Log("Character Manager with DontDestroyOnLoad can't be a child transform. Unparent!");
                        transform.parent = null;
                    }
                    DontDestroyOnLoad(gameObject);
                }
                if (CharacterManager.SavingLoading.autoSave)
                {
                    StartCoroutine(RepeatSaving(CharacterManager.SavingLoading.savingRate));
                }

                Physics.queriesHitTriggers = CharacterManager.DefaultSettings.queriesHitTriggers;

                m_IsLoaded = !HasSavedData();
                this.onDataLoaded.AddListener(() => { m_IsLoaded = true; });

                if (CharacterManager.DefaultSettings.debugMessages)
                    Debug.Log("Character Manager initialized.");
            }
        }

        private void Start()
        {
            /*if (CharacterManager.SavingLoading.autoSave)
            {
                StartCoroutine(DelayedLoading(1f));
            }*/
        }

        private static void ChangedActiveScene(UnityEngine.SceneManagement.Scene current, UnityEngine.SceneManagement.Scene next)
        {
            if (CharacterManager.SavingLoading.autoSave)
            {
                CharacterManager.m_IsLoaded = false;
                CharacterManager.Load();
            }
        }

        //TODO move to utility
        [Obsolete("CharacterManager.GetBounds is obsolete Use UnityUtility.GetBounds")]
        public Bounds GetBounds(GameObject obj)
        {
            Bounds bounds = new Bounds();
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            if (renderers.Length > 0)
            {
                foreach (Renderer renderer in renderers)
                {
                    if (renderer.enabled)
                    {
                        bounds = renderer.bounds;
                        break;
                    }
                }
                foreach (Renderer renderer in renderers)
                {
                    if (renderer.enabled)
                    {
                        bounds.Encapsulate(renderer.bounds);
                    }
                }
            }
            return bounds;
        }


        private IEnumerator DelayedLoading(float seconds)
        {
            yield return new WaitForSecondsRealtime(seconds);
            Load();
        }

        private IEnumerator RepeatSaving(float seconds)
        {
            while (true)
            {
                yield return new WaitForSeconds(seconds);
                Save();
            }
        }

        public static void Delete(string key)
        {
            List<string> keys = PlayerPrefs.GetString("CharacterSystemSavedKeys").Split(';').ToList();
            keys.RemoveAll(x => string.IsNullOrEmpty(x));

            List<string> scenes = PlayerPrefs.GetString(key + ".Scenes").Split(';').ToList();
            scenes.RemoveAll(x => string.IsNullOrEmpty(x));
            string uiData = PlayerPrefs.GetString(key + ".UI");

            List<string> allKeys = new List<string>(keys);
            allKeys.Remove(key);
            PlayerPrefs.SetString("CharacterSystemSavedKeys", string.Join(";", allKeys));
            PlayerPrefs.DeleteKey(key + ".UI");
            PlayerPrefs.DeleteKey(key + ".Scenes");
            for (int j = 0; j < scenes.Count; j++)
            {
                PlayerPrefs.DeleteKey(key + "." + scenes[j]);
            }
        }

        public static void Save()
        {
            string key = PlayerPrefs.GetString(CharacterManager.SavingLoading.savingKey, CharacterManager.SavingLoading.savingKey);
            Save(key);
        }

        public static void Save(string key, int index = 0)
        {
            string uiData = string.Empty;
            string worldData = string.Empty;
            Serialize(ref uiData, ref worldData);

            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            PlayerPrefs.SetString(key + ".UI", uiData);
            PlayerPrefs.SetString(key + "." + currentScene, worldData);
            List<string> scenes = PlayerPrefs.GetString(key + ".Scenes").Split(';').ToList();
            scenes.RemoveAll(x => string.IsNullOrEmpty(x));
            if (!scenes.Contains(currentScene))
            {
                scenes.Add(currentScene);
            }
            PlayerPrefs.SetString(key + ".Scenes", string.Join(";", scenes));
            List<string> keys = PlayerPrefs.GetString("CharacterSystemSavedKeys").Split(';').ToList();
            keys.RemoveAll(x => string.IsNullOrEmpty(x));
            if (!keys.Contains(key))
            {
                keys.Insert(index, key);
            }
            PlayerPrefs.SetString("CharacterSystemSavedKeys", string.Join(";", keys));

            if (CharacterManager.current != null && CharacterManager.current.onDataSaved != null)
            {
                CharacterManager.current.onDataSaved.Invoke();
            }

            if (CharacterManager.DefaultSettings.debugMessages)
            {
                Debug.Log("[Character System] UI Saved: " + uiData);
                Debug.Log("[Character System] Scene Saved: " + worldData);
            }
        }

        public static void Serialize(ref string uiData, ref string sceneData)
        {
            List<MonoBehaviour> results = new List<MonoBehaviour>();
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().ToList().ForEach(g => results.AddRange(g.GetComponentsInChildren<MonoBehaviour>(true)));
            //DontDestroyOnLoad GameObjects
            SingleInstance.GetInstanceObjects().ForEach(g => results.AddRange(g.GetComponentsInChildren<MonoBehaviour>(true)));

            //IJsonSerializable[] ui = serializables.Where(x => x.GetComponent<CharacterContainer>() != null).ToArray();
            //IJsonSerializable[] world = serializables.Except(ui).ToArray();

            //uiData = JsonSerializer.Serialize(ui);
            //sceneData = JsonSerializer.Serialize(world);
        }

        public static void Load()
        {
            string key = PlayerPrefs.GetString(CharacterManager.SavingLoading.savingKey, CharacterManager.SavingLoading.savingKey);
            Load(key);
        }

        public static void Load(string key)
        {
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            string uiData = PlayerPrefs.GetString(key + ".UI");
            string sceneData = PlayerPrefs.GetString(key + "." + currentScene);

            Load(uiData, sceneData);
        }

        public static void Load(string uiData, string sceneData)
        {
            //Load UI
            LoadUI(uiData);
            //Load Scene
            LoadScene(sceneData);

            if (CharacterManager.current != null && CharacterManager.current.onDataLoaded != null)
            {
                CharacterManager.current.onDataLoaded.Invoke();
            }
        }

        public static bool HasSavedData()
        {
            string key = PlayerPrefs.GetString(CharacterManager.SavingLoading.savingKey, CharacterManager.SavingLoading.savingKey);
            return CharacterManager.HasSavedData(key);
        }

        public static bool HasSavedData(string key)
        {
            return !string.IsNullOrEmpty(PlayerPrefs.GetString(key + ".UI"));
        }

        private static void LoadUI(string json)
        {
            if (string.IsNullOrEmpty(json)) return;

            List<object> list = MiniJSON.Deserialize(json) as List<object>;
            for (int i = 0; i < list.Count; i++)
            {
                Dictionary<string, object> mData = list[i] as Dictionary<string, object>;
                string prefab = (string)mData["Prefab"];
                List<object> positionData = mData["Position"] as List<object>;
                List<object> rotationData = mData["Rotation"] as List<object>;
                string type = (string)mData["Type"];

                Vector3 position = new Vector3(System.Convert.ToSingle(positionData[0]), System.Convert.ToSingle(positionData[1]), System.Convert.ToSingle(positionData[2]));
                Quaternion rotation = Quaternion.Euler(new Vector3(System.Convert.ToSingle(rotationData[0]), System.Convert.ToSingle(rotationData[1]), System.Convert.ToSingle(rotationData[2])));
                
                if (type == "UI")
                {
                    UIWidget container = WidgetUtility.Find<UIWidget>(prefab);
                }
                
            }
            if (CharacterManager.DefaultSettings.debugMessages)
            {
                Debug.Log("[Character System] UI Loaded: " + json);
            }
        }

        private static void LoadScene(string json)
        {
            if (string.IsNullOrEmpty(json)) return;

            //CharacterCollection[] characterCollections = FindObjectsOfType<CharacterCollection>().Where(x => x.saveable).ToArray();
            //for (int i = 0; i < characterCollections.Length; i++)
            //{
            //    CharacterCollection collection = characterCollections[i];

            //    //Dont destroy ui game objects
            //    if (collection.GetComponent<CharacterContainer>() != null)
            //        continue;

            //    GameObject prefabForCollection = CharacterManager.GetPrefab(collection.name);

            //    //Store real prefab to cache
            //    if (prefabForCollection == null)
            //    {
            //        collection.transform.parent = CharacterManager.current.transform;
            //        CharacterManager.m_PrefabCache.Add(collection.name, collection.gameObject);
            //        collection.gameObject.SetActive(false);
            //        continue;
            //    }

            //    Destroy(collection.gameObject);
            //}

            List<object> list = MiniJSON.Deserialize(json) as List<object>;
            for (int i = 0; i < list.Count; i++)
            {
                Dictionary<string, object> mData = list[i] as Dictionary<string, object>;
                string prefab = (string)mData["Prefab"];
                List<object> positionData = mData["Position"] as List<object>;
                List<object> rotationData = mData["Rotation"] as List<object>;

                Vector3 position = new Vector3(System.Convert.ToSingle(positionData[0]), System.Convert.ToSingle(positionData[1]), System.Convert.ToSingle(positionData[2]));
                Quaternion rotation = Quaternion.Euler(new Vector3(System.Convert.ToSingle(rotationData[0]), System.Convert.ToSingle(rotationData[1]), System.Convert.ToSingle(rotationData[2])));

                GameObject collectionGameObject = CreateCollection(prefab, position, rotation);
                //if (collectionGameObject != null)
                //{
                //    IGenerator[] generators = collectionGameObject.GetComponents<IGenerator>();
                //    for (int j = 0; j < generators.Length; j++)
                //    {
                //        generators[j].enabled = false;
                //    }
                //    CharacterCollection characterCollection = collectionGameObject.GetComponent<CharacterCollection>();
                //    characterCollection.SetObjectData(mData);
                //}
            }

            if (CharacterManager.DefaultSettings.debugMessages)
            {
                Debug.Log("[Character System] Scene Loaded: " + json);
            }
        }

        private static GameObject GetPrefab(string prefabName)
        {
            GameObject prefab = null;
            //Return from cache
            if (CharacterManager.m_PrefabCache.TryGetValue(prefabName, out prefab))
            {
                return prefab;
            }
            //Get from database
            prefab = CharacterManager.Database.GetCharacterPrefab(prefabName);

            //Load from Resources
            if (prefab == null)
            {
                prefab = Resources.Load<GameObject>(prefabName);
            }
            // Add to cache
            if (prefab != null)
            {
                CharacterManager.m_PrefabCache.Add(prefabName, prefab);
            }
            return prefab;
        }

        private static GameObject CreateCollection(string prefabName, Vector3 position, Quaternion rotation)
        {
            GameObject prefab = CharacterManager.GetPrefab(prefabName);

            if (prefab != null)
            {
                GameObject go = CharacterManager.Instantiate(prefab, position, rotation);
                go.name = go.name.Replace("(Clone)", "");
                go.SetActive(true);
                return go;

            }
            return null;
        }

        public static GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation)
        {
#if Proxy
            return Proxy.Instantiate(original, position, rotation);
#else
            return GameObject.Instantiate(original, position, rotation);
#endif
        }

        public static void Destroy(GameObject gameObject)
        {
#if Proxy
            Proxy.Destroy(gameObject);
#else
            GameObject.Destroy(gameObject);
#endif
        }

        public static Player[] CreateInstances(CharacterGroup group)
        {
            if (group == null)
            {
                return CreateInstances(Database.players.ToArray(), Enumerable.Repeat(1, Database.players.Count).ToArray(), Enumerable.Repeat(new CharacterModifierList(), Database.players.Count).ToArray());
            }
            return CreateInstances(group.Players, group.Amounts, group.Modifiers.ToArray());
        }

        public static Player CreateInstance(Player player)
        {
            return CreateInstance(player, player.Stack, new CharacterModifierList());
        }

        public static Player CreateInstance(Player player, int amount, CharacterModifierList modiferList)
        {
            return CreateInstances(new Player[] { player }, new int[] { amount }, new CharacterModifierList[] { modiferList })[0];
        }

        public static Player[] CreateInstances(Player[] players)
        {
            return CreateInstances(players, Enumerable.Repeat(1, players.Length).ToArray(), new CharacterModifierList[players.Length]);
        }

        public static Player[] CreateInstances(Player[] players, int[] amounts, CharacterModifierList[] modifierLists)
        {
            Player[] instances = new Player[players.Length];

            for (int i = 0; i < players.Length; i++)
            {
                Player player = players[i];
                player = Instantiate(player);
                player.Stack = amounts[i];
                if (i < modifierLists.Length)
                    modifierLists[i].Modify(player);

                if (player.CraftingRecipe != null)
                {
                    for (int j = 0; j < player.CraftingRecipe.Ingredients.Count; j++)
                    {
                        player.CraftingRecipe.Ingredients[j].player = Instantiate(player.CraftingRecipe.Ingredients[j].player);
                        player.CraftingRecipe.Ingredients[j].player.Stack = player.CraftingRecipe.Ingredients[j].amount;
                    }
                }
                instances[i] = player;
            }
            return instances;
        }
    }
}
