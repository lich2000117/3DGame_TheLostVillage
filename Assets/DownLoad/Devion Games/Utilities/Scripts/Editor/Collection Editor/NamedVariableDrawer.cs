using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevionGames
{
    [CustomPropertyDrawer(typeof(NamedVariable),true)]
    public class NamedVariableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position,property.FindPropertyRelative("m_Name"));
            position.y += EditorGUIUtility.singleLineHeight+EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(position,property.FindPropertyRelative("m_VariableType"), new GUIContent("Type"));
            position.y += EditorGUIUtility.singleLineHeight+EditorGUIUtility.standardVerticalSpacing; ;

            SerializedProperty value = property.FindPropertyRelative((property.GetValue() as NamedVariable).PropertyPath);
            position.height = EditorGUI.GetPropertyHeight(value);
            EditorGUI.PropertyField(position,value,new GUIContent("Value"));
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.standardVerticalSpacing*2+ EditorGUIUtility.singleLineHeight*2+EditorGUI.GetPropertyHeight(property.FindPropertyRelative((property.GetValue() as NamedVariable).PropertyPath));
        }
    }
}