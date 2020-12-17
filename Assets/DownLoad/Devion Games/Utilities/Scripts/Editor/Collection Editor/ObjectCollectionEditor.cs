using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevionGames
{
    public abstract class ObjectCollectionEditor<T> : CollectionEditor<T>
    {
        protected SerializedObject m_SerializedObject;
        protected SerializedProperty m_SerializedProperty;

        public ObjectCollectionEditor(SerializedObject serializedObject, SerializedProperty serializedProperty) {
            this.m_SerializedObject = serializedObject;
            this.m_SerializedProperty = serializedProperty;
            this.sidebarRect.width = EditorPrefs.GetFloat("CollectionEditorSidebarWidth" + ToolbarName, sidebarRect.width);
        }

        protected override void DrawItem(T item)
        {
            int index = Items.IndexOf(item);
          
            this.m_SerializedObject.Update();
            EditorTools.PropertyElementField(this.m_SerializedProperty, index);
            this.m_SerializedObject.ApplyModifiedProperties();
        }

        protected override void Create()
        {
            T value = (T)System.Activator.CreateInstance(typeof(T));
            Items.Add(value);
            EditorUtility.SetDirty(this.m_SerializedObject.targetObject);
          /*  this.m_SerializedObject.Update();
            this.m_SerializedProperty.arraySize++;
            this.m_SerializedProperty.GetArrayElementAtIndex(this.m_SerializedProperty.arraySize - 1).managedReferenceValue = value;
            this.m_SerializedObject.ApplyModifiedProperties();*/
        }

        protected override void Remove(T item)
        {
            this.m_SerializedObject.Update();
            int index = Items.IndexOf(item);
            this.m_SerializedProperty.DeleteArrayElementAtIndex(index);
            this.m_SerializedObject.ApplyModifiedProperties();
        }

        protected override void Duplicate(T item)
        {
            T duplicate = (T)EditorTools.Duplicate(item);
            this.m_SerializedObject.Update();
            this.m_SerializedProperty.arraySize++;
            this.m_SerializedProperty.GetArrayElementAtIndex(this.m_SerializedProperty.arraySize - 1).managedReferenceValue = duplicate;
            this.m_SerializedObject.ApplyModifiedProperties();
        }
    }
}