using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace DevionGames
{
	public abstract class CustomDrawer
	{
		public object declaringObject;
		public FieldInfo fieldInfo;
		public object value;

		public virtual void OnGUI(GUIContent label)
		{

		}
	}
}
