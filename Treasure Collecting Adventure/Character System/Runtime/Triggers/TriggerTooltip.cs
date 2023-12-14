using LupinrangerPatranger.UIWidgets;
using UnityEngine;
using UnityEngine.UI;

namespace LupinrangerPatranger.CharacterSystem
{
    public class TriggerTooltip : UIWidget
    {
        [Header("References")]
        [SerializeField]
        protected Text m_Title;
        [SerializeField]
        protected Text m_Instruction;

        public void Show(string title, string instruction) {
            this.m_Title.text = title;
            this.m_Instruction.text = instruction;
            base.Show();
        }
    }
}