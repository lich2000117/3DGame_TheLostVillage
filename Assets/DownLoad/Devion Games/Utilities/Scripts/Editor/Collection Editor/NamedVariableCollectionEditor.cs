using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevionGames
{
    public class NamedVariableCollectionEditor : ObjectCollectionEditor<NamedVariable>
    {
        public override string ToolbarName
        {
            get
            {
                return "Variables";
            }
        }

        protected override List<NamedVariable> Items => this.m_SerializedProperty.GetValue() as List<NamedVariable>;


        public NamedVariableCollectionEditor(SerializedObject serializedObject, SerializedProperty serializedProperty) : base(serializedObject, serializedProperty)
        {

        }

        protected override string GetSidebarLabel(NamedVariable item)
        {
            return item.Name;
        }

        protected override bool MatchesSearch(NamedVariable item, string search)
        {
            return item.Name.ToLower().Contains(search.ToLower());
        }
    }

}