using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames
{
    [System.Serializable]
    public class NamedVariable : INameable
    {
        [SerializeField]
        private string m_Name = "New Variable";
        public string Name { get => this.m_Name; set => this.m_Name = value; }

		[SerializeField]
		private VariableType m_VariableType = 0;

		public VariableType Type
		{
			get
			{
				return this.m_VariableType;
			}
		}

		public string stringValue = string.Empty;
		public int intValue = 0;
		public float floatValue = 0f;
		public Color colorValue = Color.white;
		public bool boolValue = false;
		public UnityEngine.Object objectReferenceValue = null;
		public Vector2 vector2Value = Vector2.zero;
		public Vector3 vector3Value = Vector3.zero;

		public object GetValue()
		{
			switch (Type) {
				case VariableType.Bool:
					return boolValue;
				case VariableType.Color:
					return colorValue;
				case VariableType.Float:
					return floatValue;
				case VariableType.Int:
					return intValue;
				case VariableType.Object:
					return objectReferenceValue;
				case VariableType.String:
					return stringValue;
				case VariableType.Vector2:
					return vector2Value;
				case VariableType.Vector3:
					return vector3Value;
			}
			return null;
		}

		public void SetValue(object value)
		{
			
			if (value is string)
			{
				m_VariableType = VariableType.String;
				stringValue = (string)value;
			}
			else if (value is bool)
			{
				m_VariableType = VariableType.Bool;
				boolValue = (bool)value;
			}
			else if (value is Color)
			{
				m_VariableType = VariableType.Color;
				colorValue = (Color)value;
			}
			else if (value is float || value is double)
			{
				m_VariableType = VariableType.Float;
				floatValue = System.Convert.ToSingle(value);
			}
			else if (typeof(UnityEngine.Object).IsAssignableFrom(value.GetType()))
			{
				m_VariableType = VariableType.Object;
				objectReferenceValue = (UnityEngine.Object)value;
			}
			else if (value is int
					 || value is uint
					 || value is long
					 || value is sbyte
					 || value is byte
					 || value is short
					 || value is ushort
					 || value is ulong)
			{
				m_VariableType = VariableType.Int;
				intValue = System.Convert.ToInt32(value);
			}
			else if (value is Vector2)
			{
				m_VariableType = VariableType.Vector2;
				vector2Value = (Vector2)value;
			}
			else if (value is Vector3)
			{
				m_VariableType = VariableType.Vector3;
				vector3Value = (Vector3)value;
			}
		}

		public string PropertyPath
		{
			get
			{
				switch (m_VariableType)
				{
					case VariableType.Bool:
						return "boolValue";
					case VariableType.Color:
						return "colorValue";
					case VariableType.Float:
						return "floatValue";
					case VariableType.Int:
						return "intValue";
					case VariableType.Object:
						return "objectReferenceValue";
					case VariableType.String:
						return "stringValue";
					case VariableType.Vector2:
						return "vector2Value";
					case VariableType.Vector3:
						return "vector3Value";
				}
				return string.Empty;
			}
		}

		public enum VariableType : int { 
			String = 0,
			Bool = 2,
			Color = 3,
			Float = 4,
			Object = 5,
			Int = 6,
			Vector2 = 7,
			Vector3 = 8
		}
	}
}