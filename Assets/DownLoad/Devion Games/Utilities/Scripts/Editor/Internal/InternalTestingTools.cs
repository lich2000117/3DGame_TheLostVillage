using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DevionGames
{
    public class InternalTestingTools 
    {
        [MenuItem("Tools/Devion Games/Internal/Delete PlayerPrefs")]
        public static void DeletePlayerPrefs() {
            PlayerPrefs.DeleteAll();
        }
        [MenuItem("Tools/Devion Games/Internal/Delete EditorPrefs")]
        public static void DeleteEditorPrefs()
        {
            EditorPrefs.DeleteAll();
        }
    }
}