using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.Remoting.Contexts;

namespace DevionGames{
	/// <summary>
	/// Base class for a collection of items.
	/// </summary>
	public abstract class CollectionEditor<T>: ICollectionEditor {
		private const  float LIST_MIN_WIDTH = 200f;
		private const  float LIST_MAX_WIDTH = 400f;
		private const float LIST_RESIZE_WIDTH = 10f;


		protected Rect sidebarRect = new Rect(0,30,200,1000);
		protected Vector2 scrollPosition;
		protected string searchString=string.Empty;
		protected Vector2 sidebarScrollPosition;

		protected T selectedItem;
		protected abstract List<T> Items {get;}
        protected virtual bool CanAdd{
            get { return true; }
        }
        protected virtual bool CanRemove
        {
            get { return true; }
        }

		protected virtual bool CanDuplicate
		{
			get { return true; }
		}

		public virtual string ToolbarName
        {
            get{
                Type type = GetType();
                if (type.IsGenericType)
                {
                    return ObjectNames.NicifyVariableName(type.GetGenericArguments()[0].Name);
                }
                else
                {
                    return ObjectNames.NicifyVariableName(type.Name.Replace("Editor", ""));
                }
            }
        }

        public void OnGUI(Rect position){
			if (selectedItem == null)
			{
				int index = EditorPrefs.GetInt("CollectionEditorItemIndex", -1);
				if (index != -1 && index < Items.Count)
				{
					Select(Items[index]);
				}
			}
			DrawSidebar(new Rect(position.x, position.y, sidebarRect.width, position.height));
			DrawContent(new Rect(sidebarRect.width, sidebarRect.y, position.width - sidebarRect.width, position.height));
			ResizeSidebar();
		}

		private void DrawSidebar(Rect position) {
			sidebarRect = position;
			GUILayout.BeginArea(sidebarRect, "", Styles.leftPane);
			GUILayout.BeginHorizontal();
			DoSearchGUI();

			if (CanAdd)
			{
				GUIStyle style = new GUIStyle("ToolbarCreateAddNewDropDown");
				GUIContent content = EditorGUIUtility.IconContent("CreateAddNew");

				if (GUILayout.Button(content, style, GUILayout.Width(35f)))
				{
					GUI.FocusControl("");
					Create();
					if (Items.Count > 0)
					{
						Select(Items[Items.Count - 1]);
					}
				}
			}
			GUILayout.Space(1f);
			GUILayout.EndHorizontal();
			EditorGUILayout.Space();


			sidebarScrollPosition = GUILayout.BeginScrollView(sidebarScrollPosition);

			for (int i = 0; i < Items.Count; i++)
			{
				T currentItem = Items[i];
				if (!MatchesSearch(currentItem, searchString) && Event.current.type == EventType.Repaint)
				{
					continue;
				}

				using (var h = new EditorGUILayout.HorizontalScope(Styles.selectButton, GUILayout.Height(25)))
				{
					Color backgroundColor = GUI.backgroundColor;
					Color textColor = Styles.selectButtonText.normal.textColor;
					GUI.backgroundColor = Styles.normalColor;

					if (selectedItem != null && selectedItem.Equals(currentItem))
					{
						GUI.backgroundColor = Styles.activeColor;
						Styles.selectButtonText.normal.textColor = Color.white;
						Styles.selectButtonText.fontStyle = FontStyle.Bold;
					}else if (h.rect.Contains(Event.current.mousePosition))
					{
						GUI.backgroundColor = Styles.hoverColor;
						Styles.selectButtonText.normal.textColor = textColor;
						Styles.selectButtonText.fontStyle = FontStyle.Normal;
					}

					if (HasConfigurationErrors(currentItem)) {
						GUI.backgroundColor = Styles.warningColor;
					}


					if (h.rect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown && Event.current.button==1)
					{
						GenericMenu contextMenu = new GenericMenu();
						if(CanRemove)
							contextMenu.AddItem(new GUIContent("Delete"), false, delegate { Remove(currentItem); });
						if (CanDuplicate)
							contextMenu.AddItem(new GUIContent("Duplicate"), false, delegate { Duplicate(currentItem); });
						int oldIndex = Items.IndexOf(currentItem);
						if (CanMove(currentItem, oldIndex - 1))
						{
							contextMenu.AddItem(new GUIContent("Move Up"), false, delegate { MoveUp(currentItem); });
						}else {
							contextMenu.AddDisabledItem(new GUIContent("Move Up"));
						}
						if (CanMove(currentItem, oldIndex + 1))
						{
							contextMenu.AddItem(new GUIContent("Move Down"), false, delegate { MoveDown(currentItem); });
						}else {
							contextMenu.AddDisabledItem(new GUIContent("Move Down"));
						}
						contextMenu.ShowAsContext();
						Event.current.Use();
					}

					if (GUI.Button(h.rect, GUIContent.none, Styles.selectButton))
					{
						GUI.FocusControl("");
						selectedItem = currentItem;
						EditorPrefs.SetInt("CollectionEditorItemIndex", i);
						Select(selectedItem);
					}
					GUILayout.Label(ButtonLabel(i, currentItem), Styles.selectButtonText);
					GUI.backgroundColor = backgroundColor;
					Styles.selectButtonText.normal.textColor = textColor;
					Styles.selectButtonText.fontStyle = FontStyle.Normal;
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}

		protected virtual void DrawContent(Rect position) {
			
			GUILayout.BeginArea(position, "", Styles.centerPane);
			scrollPosition.y = EditorPrefs.GetFloat("CollectionEditorContentScrollY" + ToolbarName);
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, EditorStyles.inspectorDefaultMargins);
			EditorPrefs.SetFloat("CollectionEditorContentScrollY"+ToolbarName, scrollPosition.y);
			if (selectedItem != null && Items.Contains(selectedItem))
			{
				DrawItem(selectedItem);
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}

        public virtual void OnDestroy() { }

		/// <summary>
		/// Select an item.
		/// </summary>
		/// <param name="item">Item.</param>
		protected virtual void Select(T item){
			selectedItem = item;
		}

		/// <summary>
		/// Create an item.
		/// </summary>
		protected virtual void Create(){}

		/// <summary>
		/// Does the specified item has configuration errors
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected virtual bool HasConfigurationErrors(T item) {
			return false;
		}

		/// <summary>
		/// Remove the specified item from collection.
		/// </summary>
		/// <param name="item">Item.</param>
		protected virtual void Remove(T item){}

		/// <summary>
		/// Duplicates the specified item in collection
		/// </summary>
		/// <param name="item"></param>
		protected virtual void Duplicate(T item) { }

		/// <summary>
		/// Moves the item in database up.
		/// </summary>
		/// <param name="item"></param>
		protected virtual void MoveUp(T item) {
			int oldIndex = Items.IndexOf(item);
			MoveItem(oldIndex, oldIndex - 1);
			EditorPrefs.SetInt("CollectionEditorItemIndex", oldIndex-1);
		}

		/// <summary>
		/// Moves the item in database down.
		/// </summary>
		/// <param name="item"></param>
		protected virtual void MoveDown(T item) {
			int oldIndex = Items.IndexOf(item);
			MoveItem(oldIndex, oldIndex + 1);
			EditorPrefs.SetInt("CollectionEditorItemIndex", oldIndex + 1);
		}

		protected virtual bool CanMove(T item, int newIndex) {
			int oldIndex = Items.IndexOf(item);

			if ((oldIndex == newIndex) || (0 > oldIndex) || (oldIndex >= Items.Count) || (0 > newIndex) || (newIndex >= Items.Count))
				return false;
			return true;
		}

		protected void MoveItem(int oldIndex, int newIndex)
		{
			if ((oldIndex == newIndex) || (0 > oldIndex) || (oldIndex >= Items.Count) || (0 > newIndex) ||
				(newIndex >= Items.Count)) return;

			T tmp = Items[oldIndex];
			if (oldIndex < newIndex)
			{
				for (int i = oldIndex; i < newIndex; i++)
				{
					Items[i] = Items[i + 1];
				}
			}
			else
			{
				for (int i = oldIndex; i > newIndex; i--)
				{
					Items[i] = Items[i - 1];
				}
			}
			Items[newIndex] = tmp;
		}


		/// <summary>
		/// Draws the item properties.
		/// </summary>
		/// <param name="item">Item.</param>
		protected virtual void DrawItem(T item){}

		/// <summary>
		/// Gets the sidebar label displayed in sidebar.
		/// </summary>
		/// <returns>The sidebar label.</returns>
		/// <param name="item">Item.</param>
		protected abstract string GetSidebarLabel(T item);

        protected virtual string ButtonLabel(int index, T item) {
            return index + ":  " + GetSidebarLabel(item);
        }

		/// <summary>
		/// Checks for search.
		/// </summary>
		/// <returns><c>true</c>, if search was matchesed, <c>false</c> otherwise.</returns>
		/// <param name="item">Item.</param>
		/// <param name="search">Search.</param>
		protected abstract bool MatchesSearch (T item, string search);

		protected virtual void DoSearchGUI(){
			searchString = EditorTools.SearchField (searchString);
		}

		private void ResizeSidebar(){
			Rect rect = new Rect (sidebarRect.width - LIST_RESIZE_WIDTH*0.5f, sidebarRect.y, LIST_RESIZE_WIDTH, sidebarRect.height);
			EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeHorizontal);
			int controlID = GUIUtility.GetControlID(FocusType.Passive);
			Event ev = Event.current;
			switch (ev.rawType) {
			case EventType.MouseDown:
				if(rect.Contains(ev.mousePosition)){
					GUIUtility.hotControl = controlID;
					ev.Use();
				}
				break;
			case EventType.MouseUp:
				if (GUIUtility.hotControl == controlID)
				{
					GUIUtility.hotControl = 0;
					ev.Use();
				}
				break;
			case EventType.MouseDrag:
				if (GUIUtility.hotControl == controlID)
				{
					sidebarRect.width=ev.mousePosition.x;
					sidebarRect.width=Mathf.Clamp(sidebarRect.width,LIST_MIN_WIDTH,LIST_MAX_WIDTH);
                    EditorPrefs.SetFloat("CollectionEditorSidebarWidth"+ToolbarName,sidebarRect.width);
					ev.Use();
				}
				break;
			}
		}

        public static class Styles{
			public static GUIStyle minusButton;
			public static GUIStyle selectButton;
			public static GUIStyle background;

			private static GUIStyle m_LeftPaneDark;
			private static GUIStyle m_LeftPaneLight;
			public static GUIStyle leftPane {
				get {return EditorGUIUtility.isProSkin ? m_LeftPaneDark : m_LeftPaneLight;}
			}

			private static GUIStyle m_CenterPaneDark;
			private static GUIStyle m_CenterPaneLight;
			public static GUIStyle centerPane {
				get { return EditorGUIUtility.isProSkin ? m_CenterPaneDark : m_CenterPaneLight; }
			}

			public static GUIStyle selectButtonText;
			public static Color normalColor;
			public static Color hoverColor;
			public static Color activeColor;
			public static Color warningColor;

			private static GUISkin skin;


			static Styles(){
				skin = Resources.Load<GUISkin>("EditorSkin");
				m_LeftPaneLight = skin.GetStyle("Left Pane");
				m_CenterPaneLight = skin.GetStyle("Center Pane");
				m_LeftPaneDark = skin.GetStyle("Left Pane Dark");
				m_CenterPaneDark = skin.GetStyle("Center Pane Dark");

				normalColor = EditorGUIUtility.isProSkin? new Color(0.219f, 0.219f, 0.219f, 1f) : new Color(0.796f, 0.796f, 0.796f, 1f);
				hoverColor = EditorGUIUtility.isProSkin ? new Color(0.266f, 0.266f, 0.266f, 1f):new Color(0.69f,0.69f,0.69f,1f);
				activeColor = EditorGUIUtility.isProSkin ? new Color(0.172f, 0.364f, 0.529f, 1f):new Color(0.243f,0.459f,0.761f,1f);
				warningColor = new Color(0.9f,0.37f,0.32f,1f);

				minusButton = new GUIStyle ("OL Minus"){
					margin=new RectOffset(0,0,4,0)
				};
				selectButton = new GUIStyle("MeTransitionSelectHead")
				{
					alignment = TextAnchor.MiddleLeft,
					padding = new RectOffset(5, 0, 0, 0),
					overflow = new RectOffset(0, -1, 0, 0),
				};
				selectButton.normal.background = ((GUIStyle)"ColorPickerExposureSwatch").normal.background;

				selectButtonText = new GUIStyle("MeTransitionSelectHead")
				{
					alignment = TextAnchor.MiddleLeft,
					padding = new RectOffset(5, 0, 0, 0),
					overflow = new RectOffset(0, -1, 0, 0),
					richText = true
				};
				selectButtonText.normal.background = null;
				selectButtonText.normal.textColor = EditorGUIUtility.isProSkin ? new Color(0.788f, 0.788f, 0.788f, 1f) : new Color(0.047f, 0.047f, 0.047f, 1f);
				background = new GUIStyle("PopupCurveSwatchBackground");
			}
		}
	}
}