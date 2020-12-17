using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

namespace DevionGames.Graphs
{
    public class AddNodeWindow : EditorWindow
    {
        private string m_SearchString = string.Empty;
        private Vector2 m_ScrollPosition;
        private Vector2 m_MousePosition;
        private NodeElement m_RootElement;
        private NodeElement m_SelectedElement;
        private Graph m_Graph;

        private bool isSearching
        {
            get
            {
                return !string.IsNullOrEmpty(m_SearchString);
            }
        }


        public static void ShowWindow(Rect buttonRect, Vector2 mousePosition, Graph graph)
        {
            AddNodeWindow window = ScriptableObject.CreateInstance<AddNodeWindow>();
            window.m_MousePosition = mousePosition;
            window.m_Graph = graph;
            buttonRect = GUIToScreenRect(buttonRect);
          
            window.ShowAsDropDown(buttonRect, new Vector2(buttonRect.width, 320f));
        }

        private void Update()
        {
            Repaint();
        }

        private void OnGUI()
        {
            if (m_RootElement == null)
            {
                m_RootElement = BuildNodeElements();
                m_SelectedElement = m_RootElement;
            }
            GUI.Label(new Rect(0f, 0f, base.position.width, base.position.height), GUIContent.none, Styles.background);
            GUILayout.Space(5f);

            this.m_SearchString = EditorTools.SearchField(this.m_SearchString,true);

            GUIContent header = new GUIContent(!isSearching ? m_SelectedElement.label.text : "Search");
            Rect headerRect = GUILayoutUtility.GetRect(header, Styles.header);

            if (GUI.Button(headerRect, header, Styles.header))
            {
                if (m_SelectedElement.parent != null && !isSearching)
                {
                    m_SelectedElement = m_SelectedElement.parent;
                }
            }
            if (m_SelectedElement.parent != null && !isSearching)
            {
                GUI.Label(new Rect(headerRect.x, headerRect.y + 4f, 16f, 16f), "", Styles.leftArrow);
            }
            GUILayout.Space(-5);
            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
            if (isSearching)
            {
                NodeElement[] elements = GetAllElements(m_RootElement);
                DrawElements(elements);
            }
            else
            {
                DrawElements(m_SelectedElement.children.ToArray());
            }
            GUILayout.EndScrollView();
        }

        private void DrawElements(NodeElement[] elements)
        {
            string[] searchArray = m_SearchString.ToLower().Split(' ');

            foreach (NodeElement element in elements)
            {

                string fullPath = element.path.ToLower() + "." + element.label.text.ToLower();

                if (isSearching && (m_SearchString.Length <= 1 || !searchArray.All(fullPath.Contains) || element.children.Count > 0))
                {
                    continue;
                }

                Color backgroundColor = GUI.backgroundColor;
                Color textColor = Styles.elementButton.normal.textColor;

                Rect rect = GUILayoutUtility.GetRect(element.label, Styles.elementButton, GUILayout.Height(20f));
                GUI.backgroundColor = (rect.Contains(Event.current.mousePosition) ? GUI.backgroundColor : new Color(0, 0, 0, 0.0f));
                Styles.elementButton.normal.textColor = (rect.Contains(Event.current.mousePosition) ? Color.white : Styles.elementButton.normal.textColor);
                GUIContent label = new GUIContent(element.label);
                if (element.type != null)
                {
                    CategoryAttribute categoryAttribute = element.type.GetCustomAttribute<CategoryAttribute>();
                    string category = categoryAttribute != null ? categoryAttribute.Category : string.Empty;

                    label.text = isSearching ? category.Split('/').Last() + "." + element.label.text : element.label.text;
                    TooltipAttribute tooltipAttribute = element.type.GetCustomAttribute<TooltipAttribute>();
                    string tooltip = tooltipAttribute != null ? tooltipAttribute.tooltip : string.Empty;
                    label.tooltip = tooltip;

                }

                if (GUI.Button(rect, label, Styles.elementButton))
                {
                    m_SelectedElement = element;
                    if (m_SelectedElement.children.Count == 0)
                    {
                        Node node = GraphUtility.AddNode(this.m_Graph, element.type);
                       // node.id = this.m_Graph.nodes.IndexOf(node);
                        node.name = ObjectNames.NicifyVariableName(element.type.Name);
                        node.position = this.m_MousePosition;

                        //GraphUtility.Save(this.m_Graph);
                        Close();
                    }
                }
                GUI.backgroundColor = backgroundColor;
                Styles.elementButton.normal.textColor = textColor;
                if (element.type != null)
                {

                    CategoryAttribute attribute = element.type.GetCustomAttribute<CategoryAttribute>();

                    string category = attribute!=null?attribute.Category.Split('/').Last():string.Empty;
                    Texture2D icon = (Texture2D)EditorGUIUtility.ObjectContent(null, Utility.GetType(category)).image;


                    if (icon != null)
                    {
                        GUI.Label(new Rect(rect.x, rect.y, 20f, 20f), icon);
                    }
                }
                if (element.children.Count > 0)
                {
                    GUI.Label(new Rect(rect.x + rect.width - 16f, rect.y + 2f, 16f, 16f), "", Styles.rightArrow);
                }
            }
        }

        private NodeElement BuildNodeElements()
        {
            
            NodeElement root = new NodeElement("Nodes", "");
            Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type => typeof(Node).IsAssignableFrom(type) && !type.IsAbstract && !type.HasAttribute(typeof(ExcludeFromCreation))).ToArray();
            types = types.OrderBy(x => x.BaseType.Name).ToArray();
            foreach (Type type in types)
            {
                if (typeof(EventNode).IsAssignableFrom(type) && this.m_Graph.nodes.Find(x=>x.GetType() == type) != null) {
                    continue;
                }
                NodeStyleAttribute nodeStyle = type.GetCustomAttribute<NodeStyleAttribute>();
                string category = string.Empty ;
                if (nodeStyle != null) {
                    category = nodeStyle.category;
                }
   
                string menu =  category;
                menu = menu.Replace("/", ".");

                string[] s = menu.Split('.');
                NodeElement prev = null;
                string cur = string.Empty;
                for (int i = 0; i < s.Length; i++)
                {
                    cur += (string.IsNullOrEmpty(cur) ? "" : ".") + s[i];
                    NodeElement parent = root.Find(cur);
                    if (parent == null)
                    {
                        parent = new NodeElement(s[i], cur);
                        if (prev != null)
                        {
                            parent.parent = prev;
                            prev.children.Add(parent);
                        }
                        else
                        {
                            parent.parent = root;
                            root.children.Add(parent);
                        }
                    }

                    prev = parent;

                }
                NodeElement element = new NodeElement(type.Name, menu);
                element.type = type;
                element.parent = prev;
                prev.children.Add(element);
            }
            return root;
        }

        private NodeElement[] GetAllElements(NodeElement root)
        {
            List<NodeElement> elements = new List<NodeElement>();
            GetNodeElements(root, ref elements);
            return elements.ToArray();
        }

        private void GetNodeElements(NodeElement current, ref List<NodeElement> list)
        {
            list.Add(current);
            for (int i = 0; i < current.children.Count; i++)
            {
                GetNodeElements(current.children[i], ref list);
            }
        }

        private static Rect GUIToScreenRect(Rect guiRect)
        {
            Vector2 vector = GUIUtility.GUIToScreenPoint(new Vector2(guiRect.x, guiRect.y));
            guiRect.x = vector.x;
            guiRect.y = vector.y;
            return guiRect;
        }

        public class NodeElement
        {

            public Type type;
            public NodeElement parent;

            private string m_Path;

            public string path
            {
                get
                {
                    return this.m_Path;
                }
            }

            private GUIContent m_Label;

            public GUIContent label
            {
                get
                {
                    return this.m_Label;
                }
                set
                {
                    this.m_Label = value;
                }
            }

            public NodeElement(string label, string path)
            {
                this.label = new GUIContent(label);
                this.m_Path = path;
            }


            private List<NodeElement> m_children;

            public List<NodeElement> children
            {
                get
                {
                    if (this.m_children == null)
                    {
                        this.m_children = new List<NodeElement>();
                    }
                    return m_children;
                }
                set
                {
                    this.m_children = value;
                }
            }

            public bool Contains(NodeElement element)
            {
                if (element.label.text == label.text)
                {
                    return true;
                }
                for (int i = 0; i < children.Count; i++)
                {
                    bool contains = children[i].Contains(element);
                    if (contains)
                    {
                        return true;
                    }
                }
                return false;
            }

            public NodeElement Find(string path)
            {
                if (this.path == path)
                {
                    return this;
                }
                for (int i = 0; i < children.Count; i++)
                {
                    NodeElement element = children[i].Find(path);
                    if (element != null)
                    {
                        return element;
                    }
                }
                return null;
            }
        }

        private static class Styles
        {
            public static GUIStyle header;
            public static GUIStyle rightArrow = "AC RightArrow";
            public static GUIStyle leftArrow = "AC LeftArrow";
            public static GUIStyle elementButton;
            public static GUIStyle background = "grey_border";

            static Styles()
            {
                header = new GUIStyle("DD HeaderStyle") {
                    stretchWidth = true,
                    margin = new RectOffset(1, 1, 0, 4)
                };

                elementButton = new GUIStyle("MeTransitionSelectHead")
                {
                    alignment = TextAnchor.MiddleLeft,
                    padding = new RectOffset(22, 0, 0, 0),
                    margin = new RectOffset(1, 1, 0, 0)
                };

            }
        }
    }
}