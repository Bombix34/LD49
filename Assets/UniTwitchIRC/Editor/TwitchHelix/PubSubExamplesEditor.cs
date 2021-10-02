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

using TwitchHelixAPI;
using UniTwitchIRC.TwitchInterface.TwitchPubSub;
using UnityEditor;
using UnityEngine;

namespace UniTwitchIRCEditor.TwitchHelix
{
    /// <summary>
    /// Drawer for PubSub helix component
    /// </summary>
    [CustomEditor(typeof(PubSubExamples))]
    public class PubSubExamplesEditor : Editor
    {
        PubSubBehaviour m_PubSubBehaviour = null;
        SerializedProperty m_OnPointsProp = null;
        SerializedProperty m_OnWhispersProp = null;

        void OnEnable()
        {
            m_PubSubBehaviour = FindObjectOfType<PubSubBehaviour>();
            m_OnPointsProp = serializedObject.FindProperty("m_OnPoints");
            m_OnWhispersProp = serializedObject.FindProperty("m_OnWhispers");
        }

        /// <summary>
        /// Draws the editor to the inspector
        /// </summary>
        public override void OnInspectorGUI()
        {
            OpenScenesEditorWindow.DrawOpenTryItButton();
            serializedObject.UpdateIfRequiredOrScript();

            Editor.DrawPropertiesExcluding(serializedObject, new[] { "m_Script" });
            if (m_OnPointsProp.boolValue && !m_PubSubBehaviour.channelPointsChannelV1)
            {
                EditorGUILayout.HelpBox($"{Authentication.PubSub.Topics.CHANNEL_POINTS_CHANNEL_V1} topic should be checked on PubSubBehaviour component for this example to work.", MessageType.Warning);
            }
            if (m_OnWhispersProp.boolValue && !m_PubSubBehaviour.whisper)
            {
                EditorGUILayout.HelpBox($"{Authentication.PubSub.Topics.WHISPER} topic should be checked on PubSubBehaviour component for this example to work.", MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
