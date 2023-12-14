using System.Collections;
using System.Collections.Generic;
using LupinrangerPatranger.UIWidgets;
using UnityEngine;

namespace LupinrangerPatranger.CharacterSystem
{
    [UnityEngine.Scripting.APIUpdating.MovedFromAttribute(true, null, "Assembly-CSharp")]
    [Icon("Player")]
    [ComponentMenu("Character System/Save")]
    public class Save : Action
    {
        public override ActionStatus OnUpdate()
        {
            CharacterManager.Save(); 
            return ActionStatus.Success;
        }
    }
}
