using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [Icon("Condition Character")]
    [ComponentMenu("Character System/Has Character")]
    public class HasCharacter : Action, ICondition
    {
        [SerializeField]
        protected CharacterCondition[] requiredCharacters;

        public override ActionStatus OnUpdate()
        {
            for (int i = 0; i < requiredCharacters.Length; i++)
            {
                CharacterCondition condition = requiredCharacters[i];
                if (condition.player != null && !string.IsNullOrEmpty(condition.stringValue))
                {
                    if (!CharacterContainer.HasCharacter(condition.stringValue, condition.player, 1))
                    {
                        if (CharacterManager.UI.notification != null)
                        {
                            CharacterManager.UI.notification.AddCharacter(CharacterManager.Notifications.missingCharacter, condition.player.Name, condition.stringValue);
                        }
                        return ActionStatus.Failure;
                    }
                }
            }

            return ActionStatus.Success;
        }
    }
}