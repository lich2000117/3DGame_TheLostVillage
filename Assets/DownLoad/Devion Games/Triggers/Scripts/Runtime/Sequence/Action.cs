using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames
{
    [System.Serializable]
    public abstract class Action : IAction
    {

        protected PlayerInfo playerInfo;
        protected GameObject gameObject;

        [HideInInspector]
        [SerializeField]
        private bool m_Enabled = true;
        public bool enabled {
            get { return this.m_Enabled; }
            set { this.m_Enabled = value; }
        }

        public bool isActiveAndEnabled { get { return enabled && gameObject.activeSelf; } }

        public void Initialize(GameObject gameObject, PlayerInfo playerInfo) {
            this.gameObject = gameObject;
            this.playerInfo = playerInfo;
        }

        public abstract ActionStatus OnUpdate();

        public virtual void OnStart(){}

        public virtual void OnEnd(){}

        public virtual void OnSequenceStart(){}

        public virtual void OnSequenceEnd(){}

        protected GameObject GetTarget(TargetType type)
        {
            return type == TargetType.Player ? playerInfo.gameObject : gameObject;
        }
    }

    public enum TargetType { 
        Self,
        Player
    }
}
