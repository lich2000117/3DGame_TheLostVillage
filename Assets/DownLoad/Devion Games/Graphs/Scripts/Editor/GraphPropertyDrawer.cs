using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DevionGames.Graphs
{
    public class GraphPropertyDrawer<T> : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            if (GUI.Button(position, label,EditorStyles.objectField)) {
                GraphEditorWindow window = GraphEditorWindow.ShowWindow();
                IBehavior behavior = (IBehavior)GetParent(property);
                window.Load<T>(behavior, property.serializedObject.targetObject);
                
            }
            EditorGUI.EndProperty();
        }


        protected object GetCurrent(SerializedProperty property)
        {
            object current;
            Type fieldType = fieldInfo.FieldType;
            object targetObject = GetParent(property);
            if (typeof(IEnumerable).IsAssignableFrom(fieldType))
            {
                int currentIndex = System.Convert.ToInt32(System.Text.RegularExpressions.Regex.Match(property.propertyPath, @"(\d+)(?!.*\d)").Value);
                IEnumerable<object> array = (IEnumerable<object>)fieldInfo.GetValue(targetObject);
                List<object> list = new List<object>(array);
                if (list.Count - 1 < currentIndex)
                {
                    for (int i = list.Count - 1; i < currentIndex; i++)
                    {
                        list.Add(default);
                    }
                    if (fieldInfo.FieldType.IsArray)
                    {
                        fieldInfo.SetValue(targetObject, list.ToArray());
                    }
                    else
                    {
                        fieldInfo.SetValue(targetObject, list);
                    }
                }
                current = list[currentIndex];
            }
            else
            {
                current = fieldInfo.GetValue(targetObject);
            }
            return current;
        }

        protected object GetParent(SerializedProperty prop)
        {
            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');
            foreach (var element in elements.Take(elements.Length - 1))
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue(obj, elementName, index);
                }
                else
                {
                    obj = GetValue(obj, element);
                }
            }
            return obj;
        }

        protected object GetValue(object source, string name)
        {
            if (source == null)
                return null;
            var type = source.GetType();
            var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (f == null)
            {
                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p == null)
                    return null;
                return p.GetValue(source, null);
            }
            return f.GetValue(source);
        }

        protected object GetValue(object source, string name, int index)
        {
            var enumerable = GetValue(source, name) as IEnumerable;
            var enm = enumerable.GetEnumerator();
            while (index-- >= 0)
                enm.MoveNext();

            return enm.Current;
        }
    }
}