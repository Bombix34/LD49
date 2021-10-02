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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UniTwitchIRC.TwitchInterface;
using UniTwitchIRC.TwitchInterface.MethodBinders;
using UniTwitchIRC.TwitchInterface.PollManagement;
using UniTwitchIRC.TwitchInterface.TwitchHelix;
using UniTwitchIRC.TwitchInterface.TwitchPubSub;
using UniTwitchIRCEditor.MethodBinders;
using UniTwitchIRCEditor.PollManagement;
using UniTwitchIRCEditor.TwitchHelix;
using UnityEditor;
using UnityEngine;

namespace UniTwitchIRCEditor
{
    /// <summary>
    /// UniTwitch IRC drawer for combined basic components
    /// </summary>
    [CustomEditor(typeof(UniTwitchIRCBehaviours))]
    public class UniTwitchIRCBehavioursEditor : Editor
    {
        const string BOX_STYLE_NAME = "Box";
        const string OPEN_CHAT_POPUP_BUTTON_LABEL = "Open UniTwitch ChatPopup";
        const string DOWNLOAD_CHAT_POPUP_BUTTON_LABEL = "Download UniTwitch ChatPopup";

        static GUIContent s_ViewAboutChatButtonLabel = new GUIContent("?", "Read About UniTwitch Chat Popup");
        static GUIContent s_OpenOnRunLabel = new GUIContent("On Run", "Open the chat popu on playmode start.");
        
        static string s_ChatPopupURL = "http://callowcreation.com/products/unity/unitwitch-irc/";
        static string s_ChatPopupInformationURL = "http://callowcreation.com/home/unitwitch-irc-chat-popup/";
        static string s_ChatPopupName = Path.GetFileName("UniTwitchChatPopup");
        static string s_ChatPopupFileName = Path.GetFileName(s_ChatPopupName + ".exe");
        static string s_ChatPopupZipFileName = Path.GetFileName(s_ChatPopupName + ".zip");
        static string s_ChatPopupUrlZipFileName = s_ChatPopupURL + s_ChatPopupName + ".zip";
        static Process[] m_ChatPopupProcesses;

        UniTwitchIRCBehaviours m_UniTwitchIRCBehaviours = null;

        SerializedProperty m_ChannelProperty = null;
        [System.Obsolete("Discontiued the chat popup support", true)]
        SerializedProperty m_OpenOnRunProperty = null;

        bool m_Downloading = false;
        bool m_StartingProcess = false;

        static Dictionary<string, bool> s_Foldouts = new Dictionary<string, bool>();

        List<Editor> m_Editors = null;

        void OnEnable()
        {
            m_UniTwitchIRCBehaviours = target as UniTwitchIRCBehaviours;

            CreateEditorsIfNeeded();
        }

        void OnDisable()
        {
            //UnSubscribeFromUpdate();
            DestroyEditorsIfNeeded();
        }

        void OnDestroy()
        {
            //UnSubscribeFromUpdate();
            DestroyEditorsIfNeeded();
        }

        /// <summary>
        /// Draws the editor to the inspector
        /// </summary>
        public override void OnInspectorGUI()
        {
            OpenScenesEditorWindow.DrawOpenTryItButton();

            serializedObject.UpdateIfRequiredOrScript();

            Editor.DrawPropertiesExcluding(serializedObject, new[] { "m_Script" });           

            foreach (Editor editor in m_Editors)
            {
                DrawEditorInspector(editor);
            }

            serializedObject.ApplyModifiedProperties();
        }
        
        void CreateEditorsIfNeeded()
        {
            m_Editors = new List<Editor>()
            {
                CreateEditor<TwitchChatEditor, TwitchChat>(),
                CreateEditor<TwitchConnectEditor, TwitchConnect>(),
                CreateEditor<TwitchEmotesEditor, TwitchEmotes>(),
                CreateEditor<PollManagerEditor, PollManager>(),
                
                CreateEditor<BinderEventActionsMonitorArgsEditor, BinderEventActionsMonitorArgs>(),

                CreateEditor<HelixBehaviourEditor, HelixBehaviour>(),
                CreateEditor<Editor, HelixExamples>(),
                CreateEditor<Editor, PubSubExamples>()
            };

            m_ChannelProperty = m_Editors[0].serializedObject.FindProperty("m_Channel");
        }

        void DestroyEditorsIfNeeded()
        {
            foreach (Editor editor in m_Editors)
            {
                DestroyEditor(editor);
            }
        }

        T CreateEditor<T, V>() where T : Editor where V : MonoBehaviour
        {
            T editor = null;
            V component = m_UniTwitchIRCBehaviours.GetComponentInChildren<V>();
            if (component)
            {
                editor = Editor.CreateEditor(component) as T;
                string keyTitle = GetKeyTitle(editor);
                if (s_Foldouts.TryGetValue(keyTitle, out bool value) == false)
                {
                    s_Foldouts.Add(keyTitle, EditorPrefs.GetBool(keyTitle, true));
                }
            }
            return editor;
        }

        static void DestroyEditor<T>(T editor) where T : Editor
        {
            if (editor != null)
            {
                DestroyImmediate(editor);
                editor = null;
            }
        }

        static void DrawEditorInspector<T>(T editor) where T : Editor
        {
            if (editor)
            {
                GUIStyle style = new GUIStyle(EditorStyles.foldoutHeader);
                style.fontStyle = FontStyle.Bold;
                style.alignment = TextAnchor.MiddleLeft;
                string keyTitle = GetKeyTitle(editor);
                bool oldFoldout = s_Foldouts[keyTitle];
                s_Foldouts[keyTitle] = EditorGUILayout.BeginFoldoutHeaderGroup(s_Foldouts[keyTitle], keyTitle, style);
                if (s_Foldouts[keyTitle])
                {
                    editor.OnInspectorGUI();
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                if(oldFoldout != s_Foldouts[keyTitle])
                {
                    EditorPrefs.SetBool(keyTitle, s_Foldouts[keyTitle]);
                }
            }
        }

        static string GetKeyTitle(Editor editor)
        {
            return $"{editor.target.name} - {editor.target.GetType().Name}";
        }

        [System.Obsolete("Discontiued the chat popup support", true)]
        void OnUpdateEditor()
        {
            MonitorChatPopupProcess();
        }

        [System.Obsolete("Discontiued the chat popup support", true)]
        void UnSubscribeFromUpdate()
        {
            EditorApplication.update -= OnUpdateEditor;
        }

        [System.Obsolete("Discontiued the chat popup support", true)]
        void SubscribeToUpdate()
        {
            EditorApplication.update -= OnUpdateEditor;
            EditorApplication.update += OnUpdateEditor;
        }

        [System.Obsolete("Discontiued the chat popup support", true)]
        void MonitorChatPopupProcess()
        {
#if UNITY_EDITOR_WIN
            m_ChatPopupProcesses = Process.GetProcessesByName(s_ChatPopupName);
            if(m_ChatPopupProcesses.Length == 0)
            {
                Repaint();
            }
#endif
        }

        [System.Obsolete("Discontiued the chat popup support", true)]
        void ShowOpenProcessButton()
        {
#if UNITY_EDITOR_WIN
            if(m_ChatPopupProcesses != null && m_ChatPopupProcesses.Length == 0 && !m_StartingProcess)
            {
                if(File.Exists(s_ChatPopupFileName) && !File.Exists(s_ChatPopupZipFileName))
                {
                    if(!string.IsNullOrEmpty(m_ChannelProperty.stringValue))
                    {
                        GUILayout.BeginHorizontal(BOX_STYLE_NAME);
                        {
                            if(GUILayout.Button(OPEN_CHAT_POPUP_BUTTON_LABEL))
                            {
                                StartChatPopupProcess();
                            }
                            m_OpenOnRunProperty.boolValue = EditorGUILayout.ToggleLeft(s_OpenOnRunLabel, m_OpenOnRunProperty.boolValue);
                            if(m_OpenOnRunProperty.boolValue && EditorApplication.isPlayingOrWillChangePlaymode)
                            {
                                if(EditorApplication.isPlaying)
                                {
                                    StartChatPopupProcess();
                                }
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                else if(!m_Downloading)
                {
                    GUILayout.BeginHorizontal(BOX_STYLE_NAME);
                    {
                        if(GUILayout.Button(DOWNLOAD_CHAT_POPUP_BUTTON_LABEL, GUI.skin.button))
                        {
                            m_Downloading = true;
                            ZipUtility.Compression.Download(s_ChatPopupUrlZipFileName, s_ChatPopupZipFileName);
                            ZipUtility.Compression.UnZip(s_ChatPopupZipFileName);
                            File.Delete(s_ChatPopupZipFileName);
                            m_Downloading = false;
                        }
                        if(GUILayout.Button(s_ViewAboutChatButtonLabel, GUILayout.MaxWidth(20)))
                        {
                            Application.OpenURL(s_ChatPopupInformationURL);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
#endif
        }

        [System.Obsolete("Discontiued the chat popup support", true)]
        void StartChatPopupProcess()
        {
            m_StartingProcess = true;
            Process.Start(s_ChatPopupFileName, m_ChannelProperty.stringValue);
        }    
    }
}
