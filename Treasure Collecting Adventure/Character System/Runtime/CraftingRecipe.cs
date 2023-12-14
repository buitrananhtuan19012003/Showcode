using LupinrangerPatranger.UIWidgets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    [System.Serializable]
    public class CraftingRecipe : ScriptableObject, INameable
    {
        [SerializeField]
        private new string name = "New Crafting Recipe";
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        [Tooltip("How long does it take to craft. This value is also used to display the progressbar in crafting UI.")]
        [SerializeField]
        private float m_Duration = 2f;

        public float Duration
        {
            get { return this.m_Duration; }
        }

        [Tooltip("State in animator controller to play when crafting. If you don't want to play any animation, delete this value.")]
        [SerializeField]
        private string m_AnimatorState = "Blacksmithy";

        public string AnimatorState
        {
            get { return this.m_AnimatorState; }
        }

        //[Tooltip("A skill is used to calculate fails.")]
        //[AcceptNull]
        //[SerializeField]
        //private Skill m_Skill = null;
        //public Skill Skill
        //{
        //    get { return this.m_Skill; }
        //}

        [Tooltip("Remove the ingredients when crafting fails.")]
        [SerializeField]
        private bool m_RemoveIngredientsWhenFailed = false;
        public bool RemoveIngredientsWhenFailed
        {
            get { return this.m_RemoveIngredientsWhenFailed; }
        }

        [Tooltip("Required ingredients to craft.")]
        [SerializeField]
        private List<CharacterAmountDefinition> m_Ingredients = new List<CharacterAmountDefinition>();
        public List<CharacterAmountDefinition> Ingredients
        {
            get { return this.m_Ingredients; }
        }

        [SerializeField]
        private CharacterModifierList m_CraftingModifier = new CharacterModifierList();
        public CharacterModifierList CraftingModifier
        {
            get { return this.m_CraftingModifier; }
            set { this.m_CraftingModifier = value; }
        }

        [SerializeReference]
        public List<ICondition> conditions = new List<ICondition>();

        public bool CheckConditions()
        {

            for (int i = 0; i < conditions.Count; i++)
            {
                ICondition condition = conditions[i];
                condition.Initialize(CharacterManager.current.PlayerInfo.gameObject, CharacterManager.current.PlayerInfo, CharacterManager.current.PlayerInfo.gameObject.GetComponent<Blackboard>());
                condition.OnStart();
                if (condition.OnUpdate() == ActionStatus.Failure)
                {
                    condition.OnEnd();
                    return false;
                }
            }
            return true;
        }


        [System.Serializable]
        public class CharacterAmountDefinition
        {
            public Player player;
            public int amount = 1;
        }
    }
}