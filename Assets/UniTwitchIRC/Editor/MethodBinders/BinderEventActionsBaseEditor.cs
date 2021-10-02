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

using UnityEditor;
using UnityEngine;

namespace UniTwitchIRCEditor.MethodBinders
{
    /// <summary>
    /// Drawer for Method binder
    /// </summary>
    public abstract class BinderEventActionsHelper : Editor
    {
        protected SerializedProperty m_BinderProp = null;

        public abstract GUIContent label { get; }

        protected virtual void OnEnable()
        {
            m_BinderProp = serializedObject.FindProperty("binder");
        }

        public override void OnInspectorGUI()
        {
            OpenScenesEditorWindow.DrawOpenTryItButton();
            serializedObject.Update();

            Editor.DrawPropertiesExcluding(serializedObject, new[] { "m_Script", "binder" });

            EditorGUILayout.PropertyField(m_BinderProp, label, true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
