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
using UniTwitchIRC.TwitchInterface.TwitchHelix;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.Events;

namespace UniTwitchIRCEditor.TwitchHelix
{
    /// <summary>
    /// Drawer for Twitch helix examples component
    /// </summary>
    [CustomEditor(typeof(HelixExamples))]
    public class HelixExamplesEditor : Editor
    {
        
        HelixExamples m_HelixExamples = null;

        SerializedProperty m_ModifyChannelInformationProp = null;
        SerializedProperty m_ChannelTitleProp = null;
        
        void OnEnable()
        {
            m_HelixExamples = target as HelixExamples;
            m_ModifyChannelInformationProp = serializedObject.FindProperty("m_ModifyChannelInformation");
            m_ChannelTitleProp = serializedObject.FindProperty("m_ChannelTitle");
        }

        /// <summary>
        /// Draws the editor to the inspector
        /// </summary>
        public override void OnInspectorGUI()
        {
            
            OpenScenesEditorWindow.DrawOpenTryItButton();

            serializedObject.UpdateIfRequiredOrScript();
            
            Editor.DrawPropertiesExcluding(serializedObject,new[] { "m_Script", "m_ModifyChannelInformation", "m_ChannelTitle" });

            EditorGUILayout.PropertyField(m_ModifyChannelInformationProp, true);

            if(m_ModifyChannelInformationProp.boolValue)
            {
                EditorGUILayout.PropertyField(m_ChannelTitleProp, true);
                EditorGUILayout.HelpBox($"Modify channel information will change your stream title: {m_ChannelTitleProp.stringValue}", MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
