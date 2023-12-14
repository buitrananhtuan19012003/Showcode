using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem.CharacterActions
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [Icon("Player")]
    [ComponentMenu("Character System/Add Character")]
    [System.Serializable]
    public class AddCharacter : CharacterAction
    {
        [SerializeField]
        private string m_WindowName = "Character";
        [SerializeField]
        private Player m_Character = null;
        [Range(1, 200)]
        [SerializeField]
        private int m_Amount = 1;

        public override ActionStatus OnUpdate()
        {
            Player instance = CharacterManager.CreateInstance(this.m_Character);
            instance.Stack = this.m_Amount;
            if (CharacterContainer.AddCharacter(this.m_WindowName, instance))
            {
                return ActionStatus.Success;
            }
            return ActionStatus.Failure;
        }
    }
}