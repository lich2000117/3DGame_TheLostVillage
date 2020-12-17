using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames
{
    [ComponentMenu("Debug/Log")]
    public class Log : Action
    {
        [SerializeField]
        private string m_Message = string.Empty;

        public override ActionStatus OnUpdate()
        {
            Debug.Log(this.m_Message);
            return ActionStatus.Success;
        }
    }
}