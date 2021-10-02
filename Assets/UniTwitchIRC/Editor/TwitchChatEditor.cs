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

using UniTwitchIRC.TwitchInterface;
using UnityEditor;
using UnityEngine;

namespace UniTwitchIRCEditor
{
    /// <summary>
    /// Drawer for Twitch chat component
    /// </summary>
    [CustomEditor(typeof(TwitchChat))]
    public class TwitchChatEditor : Editor
    {

        TwitchChat m_TwitchChat = null;
        void OnEnable()
        {
            m_TwitchChat = (target as TwitchChat);
        }
        
        /// <summary>
        /// Draws the editor to the inspector
        /// </summary>
        public override void OnInspectorGUI()
        {
            OpenScenesEditorWindow.DrawOpenTryItButton();
            serializedObject.Update();
            SerializedProperty hideRunInBackgroundMessageProp = serializedObject.FindProperty("m_HideRunInBackgroundMessage");
            if(!hideRunInBackgroundMessageProp.boolValue)
            {
                if(!Application.runInBackground)
                {
                    EditorGUILayout.HelpBox("Player run in background is not enabled and will require the program to be showing to use the chat client.", MessageType.Warning);
                    EditorGUILayout.PropertyField(hideRunInBackgroundMessageProp, new GUIContent("Hide This Message"));
                    if(GUILayout.Button("Enable runInBackground Now"))
                    {
                        Application.runInBackground = true;
                    }
                }
            }
            Editor.DrawPropertiesExcluding(serializedObject, new[] { "m_Script" });
            if (GUILayout.Button("Join Room"))
            {
                m_TwitchChat.JoinRoom();
            }
            if (GUILayout.Button("Part Room"))
            {
                m_TwitchChat.PartRoom();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
