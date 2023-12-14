﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LupinrangerPatranger.CharacterSystem.CharacterActions;
using System.Linq;

namespace LupinrangerPatranger.CharacterSystem
{
    [System.Serializable]
    public class UsableCharacter : Player
    {
        [SerializeField]
        private bool m_UseCategoryCooldown = true;
        [SerializeField]
        private float m_Cooldown = 1f;
        public float Cooldown
        {
            get { return this.m_UseCategoryCooldown ? Category.Cooldown : this.m_Cooldown; }
        }

        [SerializeReference]
        public List<Action> actions = new List<Action>();

        private Sequence m_ActionSequence;
        private IEnumerator m_ActionBehavior;

        protected override void OnEnable()
        {
            base.OnEnable();

            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i] is CharacterAction)
                {
                    CharacterAction action = actions[i] as CharacterAction;
                    action.player = this;
                }
            }
        }

        public override void Use()
        {
            if (this.m_ActionSequence == null)
            {
                GameObject gameObject = CharacterManager.current.PlayerInfo.gameObject;
                this.m_ActionSequence = new Sequence(gameObject, CharacterManager.current.PlayerInfo, gameObject != null ? gameObject.GetComponent<Blackboard>() : null, actions.Cast<IAction>().ToArray());
            }
            if (this.m_ActionBehavior != null)
            {
                UnityTools.StopCoroutine(m_ActionBehavior);
            }
            this.m_ActionBehavior = SequenceCoroutine();
            UnityTools.StartCoroutine(this.m_ActionBehavior);
        }

        protected IEnumerator SequenceCoroutine()
        {
            this.m_ActionSequence.Start();
            while (this.m_ActionSequence.Tick())
            {
                yield return null;
            }
        }
    }
}