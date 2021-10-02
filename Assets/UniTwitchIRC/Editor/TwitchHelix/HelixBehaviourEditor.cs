#region Author
/*
     
     Jones St. Lewis Cropper (caLLow)
     
     Another caLLowCreation
     
     Visit us on Google+ and other social media outlets @caLLowCreation
     
     Thanks for using our product.
     
     Send questions/comments/concerns/requests to 
      e-mail: caLLowCreation@gmail.com
      subject: UniTwirchIRC
     
*/
#endregion

using System.Collections.Generic;
using TwitchHelixAPI;
using UniTwitchIRC.TwitchInterface.TwitchHelix;
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEditorInternal;

namespace UniTwitchIRCEditor.TwitchHelix
{
    /// <summary>
    /// Drawer for Twitch helix component
    /// </summary>
    [CustomEditor(typeof(HelixBehaviour))]
    public class HelixBehaviourEditor : Editor
    {
        static string[] s_DisplayOptions = null;

        HelixBehaviour m_HelixBehaviour = null;
        SerializedProperty m_ShowDebugLogsProp = null;
        SerializedProperty m_ScopesProp = null;

        ReorderableList m_ReorderableList = null;

        void OnEnable()
        {
            List<Authentication.Scoped> items = Authentication.Helix.ScopeMap().ToList<Authentication.Scoped>();

            items.AddRange(Authentication.PubSub.ScopeMap());

            IEnumerable<string> scopes = items
                .Where(x => !string.IsNullOrWhiteSpace(x.scope))
                .GroupBy(x => x.scope)
                .Select(x => x.First().scope)
                .ToList();

            s_DisplayOptions = scopes.ToArray();

            m_HelixBehaviour = target as HelixBehaviour;
            m_ShowDebugLogsProp = serializedObject.FindProperty("m_ShowDebugLogs");
            m_ScopesProp = serializedObject.FindProperty("m_Scopes");
            
            m_ReorderableList = new ReorderableList(serializedObject, m_ScopesProp, true, true, true, true);

            m_ReorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                Color color = GUI.color;
                SerializedProperty prop = m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);
                
                int selectedIndex = s_DisplayOptions.ToList().FindIndex(x => x == prop.stringValue);
                int lastIndex = selectedIndex;


                for (int i = 0; i < m_ScopesProp.arraySize; i++)
                {
                    if (i == index) continue;
                    if (m_ScopesProp.GetArrayElementAtIndex(i).stringValue == prop.stringValue)
                    {
                        GUI.color = Color.red;
                    }
                }

                selectedIndex = EditorGUI.Popup(rect, selectedIndex, s_DisplayOptions);

                if (lastIndex != selectedIndex)
                {
                    prop.stringValue = s_DisplayOptions[selectedIndex];
                }


                GUI.color = color;
            };
            
        }


        /// <summary>
        /// Draws the editor to the inspector
        /// </summary>
        public override void OnInspectorGUI()
        {
            OpenScenesEditorWindow.DrawOpenTryItButton();
            serializedObject.UpdateIfRequiredOrScript();
            Editor.DrawPropertiesExcluding(serializedObject, new[] { "m_Script", "m_ShowDebugLogs", "m_Scopes" });
            
            m_ReorderableList.DoLayoutList();

            //EditorGUILayout.PropertyField(m_ScopesProp, true);

            EditorGUILayout.PropertyField(m_ShowDebugLogsProp);

            serializedObject.ApplyModifiedProperties();
        }

        bool HasScope(string scope)
        {
            return m_HelixBehaviour.scopes.Where(x => x == scope).Count() > 0;
        }

        void OnItemClicked(string scope)
        {            
            serializedObject.UpdateIfRequiredOrScript();
            
            int index = m_HelixBehaviour.scopes.ToList().FindIndex(x => x == scope);
            
            if(index == -1)
            {
                int insertIndex = m_ScopesProp.arraySize;
                m_ScopesProp.InsertArrayElementAtIndex(insertIndex);
                m_ScopesProp.GetArrayElementAtIndex(insertIndex).stringValue = scope.ToString();
            }
            else
            {
                int deleteIndex = index;
                m_ScopesProp.DeleteArrayElementAtIndex(deleteIndex);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
