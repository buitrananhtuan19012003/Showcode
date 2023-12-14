using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace LupinrangerPatranger.CharacterSystem
{
    public class CharacterCollection : MonoBehaviour, IEnumerable<Player>, IJsonSerializable
    {
        public bool saveable = true;
        [SerializeField]
        [FormerlySerializedAs("characters")]
        protected List<Player> m_Characters = new List<Player>();
        [FormerlySerializedAs("amounts")]
        [SerializeField]
        protected List<int> m_Amounts = new List<int>();

        [SerializeField]
        protected List<CharacterModifierList> m_Modifiers = new List<CharacterModifierList>();

        [HideInInspector]
        public UnityEvent onChange;



        private bool m_Initialized;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (this.m_Initialized) { return; }

            //Used to sync old items
            if (this.m_Modifiers.Count < this.m_Characters.Count)
            {
                for (int i = m_Modifiers.Count; i < this.m_Characters.Count; i++)
                {
                    m_Modifiers.Add(new CharacterModifierList());
                }
            }

            if (this.m_Amounts.Count < this.m_Characters.Count)
            {
                for (int i = this.m_Amounts.Count; i < this.m_Characters.Count; i++)
                {
                    this.m_Amounts.Add(1);
                }
            }

            m_Characters = CharacterManager.CreateInstances(m_Characters.ToArray(), this.m_Amounts.ToArray(), this.m_Modifiers.ToArray()).ToList();

            for (int i = 0; i < m_Characters.Count; i++)
            {
                Player current = m_Characters[i];
                if (current.Stack > current.MaxStack)
                {
                    //Split in smaller stacks
                    int maxStacks = Mathf.FloorToInt((float)current.Stack / (float)current.MaxStack);
                    int restStack = current.Stack - (current.MaxStack * maxStacks);
                    for (int j = 0; j < maxStacks; j++)
                    {
                        Player instance = CharacterManager.CreateInstance(current);
                        instance.Stack = instance.MaxStack;
                        Add(instance);
                    }
                    if (restStack > 0)
                    {
                        current.Stack = restStack;
                    }
                    else
                    {
                        Remove(current);
                    }

                }
            }

            //Stack same currencies
            //CombineCurrencies();
            //CharacterCollectionPopulator populator = GetComponent<CharacterCollectionPopulator>();
            //if (populator != null && populator.enabled) {
            //    Item[] groupItems = CharacterManager.CreateInstances(populator.m_ItemGroup);
            //    Add(groupItems);
            //} 
            this.m_Initialized = true;

        }

        public Player this[int index]
        {
            get { return this.m_Characters[index]; }
            set
            {
                Insert(index, value);
                if (onChange != null)
                    onChange.Invoke();
            }
        }

        public int Count
        {
            get
            {
                return m_Characters.Count;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return m_Characters.Count == 0;
            }
        }

        public IEnumerator<Player> GetEnumerator()
        {
            return this.m_Characters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(Player[] players)
        {
            for (int i = 0; i < players.Length; i++)
            {
                Add(players[i]);
            }
        }

        public void Add(Player player, bool allowStacking = false)
        {
            this.m_Characters.Add(player);
            int index = m_Characters.IndexOf(player);

            this.m_Amounts.Insert(index, player.Stack);
            this.m_Modifiers.Insert(index, new CharacterModifierList());
            if (onChange != null)
                onChange.Invoke();

        }

        public bool Remove(Player player)
        {
            int index = m_Characters.IndexOf(player);

            bool result = this.m_Characters.Remove(player);
            if (result)
            {
                this.m_Amounts.RemoveAt(index);
                this.m_Modifiers.RemoveAt(index);
                if (onChange != null)
                    onChange.Invoke();
            }
            return result;
        }

        public void Insert(int index, Player child)
        {
            this.m_Characters.Insert(index, child);
            this.m_Amounts.Insert(index, child.Stack);
            this.m_Modifiers.Insert(index, new CharacterModifierList());
            if (onChange != null)
                onChange.Invoke();
        }

        public void RemoveAt(int index)
        {
            this.m_Characters.RemoveAt(index);
            this.m_Amounts.RemoveAt(index);
            this.m_Modifiers.RemoveAt(index);
            if (onChange != null)
                onChange.Invoke();
        }

        public void Clear()
        {
            Player[] currencies = this.m_Characters.Where(x => typeof(Currency).IsAssignableFrom(x.GetType())).ToArray();
            for (int i = 0; i < currencies.Length; i++)
            {
                currencies[i].Stack = 0;
                if (currencies[i].Slot != null)
                    currencies[i].Slot.ObservedCharacter = currencies[i];
            }
            this.m_Characters.Clear();
            this.m_Amounts.Clear();
            this.m_Modifiers.Clear();
            Add(currencies);

            if (onChange != null)
                onChange.Invoke();
        }

        public void GetObjectData(Dictionary<string, object> data)
        {
            data.Add("Prefab", gameObject.name.Replace("(Clone)", ""));
            data.Add("Position", new List<float> { transform.position.x, transform.position.y, transform.position.z });
            data.Add("Rotation", new List<float> { transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z });
            data.Add("Type", (GetComponent<CharacterContainer>() != null ? "UI" : "Trigger"));
            if (m_Characters.Count > 0)
            {
                List<object> mCharacters = new List<object>();
                for (int i = 0; i < m_Characters.Count; i++)
                {
                    Player player = m_Characters[i];
                    if (player != null)
                    {
                        Dictionary<string, object> characterData = new Dictionary<string, object>();
                        player.GetObjectData(characterData);
                        mCharacters.Add(characterData);
                    }
                    else
                    {
                        mCharacters.Add(null);
                    }

                }
                data.Add("Characters", mCharacters);
            }
        }

        public void SetObjectData(Dictionary<string, object> data)
        {
            List<object> position = data["Position"] as List<object>;
            List<object> rotation = data["Rotation"] as List<object>;
            if ((string)data["Type"] != "UI")
            {
                transform.position = new Vector3(System.Convert.ToSingle(position[0]), System.Convert.ToSingle(position[1]), System.Convert.ToSingle(position[2]));
                transform.rotation = Quaternion.Euler(new Vector3(System.Convert.ToSingle(rotation[0]), System.Convert.ToSingle(rotation[1]), System.Convert.ToSingle(rotation[2])));
            }
            Clear();

            CharacterContainer container = GetComponent<CharacterContainer>();
            if (data.ContainsKey("Characters"))
            {
                List<object> mCharacters = data["Characters"] as List<object>;
                for (int i = 0; i < mCharacters.Count; i++)
                {
                    Dictionary<string, object> characterData = mCharacters[i] as Dictionary<string, object>;
                    if (characterData != null)
                    {
                        List<Player> characters = new List<Player>(CharacterManager.Database.players);
                        characters.AddRange(CharacterManager.Database.currencies);

                        Player player = characters.Find(x => x.Name == (string)characterData["Name"]);

                        if (player != null)
                        {
                            Player mCharacter = (Player)ScriptableObject.Instantiate(player);
                            mCharacter.SetObjectData(characterData);


                            Add(mCharacter);

                            this.m_Amounts[i] = 0;
                            this.m_Modifiers[i].modifiers.Clear();

                            if (characterData.ContainsKey("Slots") && container != null)
                            {
                                List<object> slots = characterData["Slots"] as List<object>;
                                for (int j = 0; j < slots.Count; j++)
                                {
                                    int slot = System.Convert.ToInt32(slots[j]);
                                    if (container.Slots.Count > slot)
                                    {
                                        mCharacter.Slots.Add(container.Slots[slot]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            CombineCurrencies();
            if (container != null)
            {
                container.Collection = this;
            }
        }

        private void CombineCurrencies()
        {
            Currency[] currencies = m_Characters.Where(x => x != null && typeof(Currency).IsAssignableFrom(x.GetType())).Cast<Currency>().ToArray();
            Dictionary<string, Currency> currencyMap = new Dictionary<string, Currency>();
            for (int i = 0; i < currencies.Length; i++)
            {
                Currency current = currencies[i];
                Currency currency = null;
                if (!currencyMap.TryGetValue(current.Name, out currency))
                {
                    currencyMap.Add(current.Name, current);
                    continue;
                }
                currency.Stack += current.Stack;
                m_Characters.Remove(current);
            }
        }

    }
}