using System.Collections;
using System.Collections.Generic;
using LupinrangerPatranger.UIWidgets;
using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [Icon("Condition Character")]
    [ComponentMenu("Character System/Has Category Character")]
    public class HasCategoryCharacter : Action, ICondition
    {
        [SerializeField]
        protected CharacterCondition[] requiredCharacters;

        public override ActionStatus OnUpdate()
        {
            for (int i = 0; i < requiredCharacters.Length; i++)
            {
                CharacterCondition condition = requiredCharacters[i];
                if (condition.category != null && !string.IsNullOrEmpty(condition.stringValue)) { 

                    if (!CharacterContainer.HasCategoryCharacter(condition.stringValue,condition.category))
                    {
                        if (CharacterManager.UI.notification != null)
                        {
                            CharacterManager.UI.notification.AddItem(CharacterManager.Notifications.missingCategoryCharacter, condition.category.Name,condition.stringValue);
                        }
                        return ActionStatus.Failure;
                    }
                }
            }

            return ActionStatus.Success;
        }
    }
}