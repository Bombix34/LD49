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

using UniTwitchIRC.TwitchInterface.TwitchHelix;
using UniTwitchIRC.TwitchInterface.TwitchPubSub;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using TwitchHelixAPI;

namespace UniTwitchIRCEditor.TwitchHelix
{
    /// <summary>
    /// Drawer for PubSub helix component
    /// </summary>
    [CustomEditor(typeof(PubSubBehaviour))]
    public class PubSubBehaviourEditor : Editor
    {
        SerializedProperty m_TwitchChatProp = null;

        SerializedProperty m_ChannelBitsEventsV1Prop = null;
        SerializedProperty m_ChannelBitsEventsV2Prop = null;
        SerializedProperty m_ChannelBitsBadgeUnlocksProp = null;
        SerializedProperty m_ChannelPointsChannelV1Prop = null;
        SerializedProperty m_ChannelSubscribeEventsV1Prop = null;
        SerializedProperty m_ChatModeratorActionsProp = null;
        SerializedProperty m_WhisperProp = null;

        HelixBehaviour m_HelixBehaviour = null;

        void OnEnable()
        {
            m_HelixBehaviour = FindObjectOfType<HelixBehaviour>();

            m_TwitchChatProp = serializedObject.FindProperty("m_TwitchChat");

            m_ChannelBitsEventsV1Prop = serializedObject.FindProperty("m_ChannelBitsEventsV1");
            m_ChannelBitsEventsV2Prop = serializedObject.FindProperty("m_ChannelBitsEventsV2");
            m_ChannelBitsBadgeUnlocksProp = serializedObject.FindProperty("m_ChannelBitsBadgeUnlocks");
            m_ChannelPointsChannelV1Prop = serializedObject.FindProperty("m_ChannelPointsChannelV1");
            m_ChannelSubscribeEventsV1Prop = serializedObject.FindProperty("m_ChannelSubscribeEventsV1");
            m_ChatModeratorActionsProp = serializedObject.FindProperty("m_ChatModeratorActions");
            m_WhisperProp = serializedObject.FindProperty("m_Whisper");
        }

        /// <summary>
        /// Draws the editor to the inspector
        /// </summary>
        public override void OnInspectorGUI()
        {
            OpenScenesEditorWindow.DrawOpenTryItButton();
            serializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.PropertyField(m_TwitchChatProp);

            GUIStyle style = new GUIStyle(EditorStyles.label);
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.MiddleLeft;

            EditorGUILayout.LabelField("Topics", style);
            /*
            m_ChannelBitsEventsV1Prop.boolValue = EditorGUILayout.ToggleLeft(Topics.CHANNEL_BITS_EVENTS_V1, m_ChannelBitsEventsV1Prop.boolValue);
            m_ChannelBitsEventsV2Prop.boolValue = EditorGUILayout.ToggleLeft(Topics.CHANNEL_BITS_EVENTS_V2, m_ChannelBitsEventsV2Prop.boolValue);
            m_ChannelBitsBadgeUnlocksProp.boolValue = EditorGUILayout.ToggleLeft(Topics.CHANNEL_BITS_BADGE_UNLOCKS, m_ChannelBitsBadgeUnlocksProp.boolValue);
            m_ChannelPointsChannelV1Prop.boolValue = EditorGUILayout.ToggleLeft(Topics.CHANNEL_POINTS_CHANNEL_V1, m_ChannelPointsChannelV1Prop.boolValue);
            m_ChannelSubscribeEventsV1Prop.boolValue = EditorGUILayout.ToggleLeft(Topics.CHANNEL_SUBSCRIBE_EVENTS_V1, m_ChannelSubscribeEventsV1Prop.boolValue);
            m_ChatModeratorActionsProp.boolValue = EditorGUILayout.ToggleLeft(Topics.CHAT_MODERATOR_ACTIONS, m_ChatModeratorActionsProp.boolValue);
            m_WhisperProp.boolValue = EditorGUILayout.ToggleLeft(Topics.WHISPER, m_WhisperProp.boolValue);
            */

            /*if (m_ChannelPointsChannelV1Prop.boolValue)
            {
                string scope = m_HelixBehaviour.scopes.FirstOrDefault(x => x == Topics.ScopeMap[Topics.CHANNEL_POINTS_CHANNEL_V1]);
                if (scope == null)
                {
                    EditorGUILayout.HelpBox($"{Topics.ScopeMap[Topics.CHANNEL_POINTS_CHANNEL_V1]} scope is missing on game object HelixBehaviour component for this topic to work.", MessageType.Error);
                }
            }

            if (m_WhisperProp.boolValue)
            {
                string scope = m_HelixBehaviour.scopes.FirstOrDefault(x => x == Topics.ScopeMap[Topics.WHISPER]);
                if (scope == null)
                {
                    EditorGUILayout.HelpBox($"{Topics.ScopeMap[Topics.WHISPER]} scope is missing on game object HelixBehaviour component for this topic to work.", MessageType.Error);
                }
            }*/

            Dictionary<string, string> map = new Dictionary<string, string>()
            {
                { m_ChannelBitsEventsV1Prop.name, Authentication.PubSub.Topics.CHANNEL_BITS_EVENTS_V1 },
                { m_ChannelBitsEventsV2Prop.name, Authentication.PubSub.Topics.CHANNEL_BITS_EVENTS_V2 },
                { m_ChannelBitsBadgeUnlocksProp.name, Authentication.PubSub.Topics.CHANNEL_BITS_BADGE_UNLOCKS },
                { m_ChannelPointsChannelV1Prop.name, Authentication.PubSub.Topics.CHANNEL_POINTS_CHANNEL_V1 },
                { m_ChannelSubscribeEventsV1Prop.name, Authentication.PubSub.Topics.CHANNEL_SUBSCRIBE_EVENTS_V1 },
                { m_ChatModeratorActionsProp.name, Authentication.PubSub.Topics.CHAT_MODERATOR_ACTIONS },
                { m_WhisperProp.name, Authentication.PubSub.Topics.WHISPER },
            };

            foreach (var item in map)
            {
                SerializedProperty prop = serializedObject.FindProperty(item.Key);
                prop.boolValue = EditorGUILayout.ToggleLeft(item.Value, prop.boolValue);
                if (prop.boolValue)
                {
                    Dictionary<string, string> scopeMap = Authentication.PubSub.ScopeMap().ToDictionary(x => x.key, x => x.scope);
                    string scope = m_HelixBehaviour.scopes.FirstOrDefault(x => x == scopeMap[item.Value]);
                    if (scope == null)
                    {
                        GUILayout.Button($"{scopeMap[item.Value]} scope is missing on Twitch Helix API game object HelixBehaviour component for this topic to work.", EditorStyles.helpBox);
                        EditorGUILayout.HelpBox($"{scopeMap[item.Value]} scope is missing on Twitch Helix API game object HelixBehaviour component for this topic to work.", MessageType.Error);
                    }
                }
            }

            Editor.DrawPropertiesExcluding(serializedObject, new[] { "m_Script",
                "m_TwitchChat",
                "m_ChannelBitsEventsV1",
                "m_ChannelBitsEventsV2",
                "m_ChannelBitsBadgeUnlocks",
                "m_ChannelPointsChannelV1",
                "m_ChannelSubscribeEventsV1",
                "m_ChatModeratorActions",
                "m_Whisper"
            });

            serializedObject.ApplyModifiedProperties();
        }
    }
}
