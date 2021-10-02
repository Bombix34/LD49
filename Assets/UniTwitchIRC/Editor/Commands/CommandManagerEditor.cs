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

using UniTwitchIRC.TwitchInterface.Commands;
using UnityEditor;

namespace UniTwitchIRCEditor.Commands
{
    /// <summary>
    /// Drawer for Command Manager
    /// </summary>
    [CustomEditor(typeof(CommandManager))]
    public class CommandManagerEditor : Editor
    {
        /// <summary>
        /// Draws the editor to the inspector
        /// </summary>
        public override void OnInspectorGUI()
        { 
            OpenScenesEditorWindow.DrawOpenTryItButton();

            serializedObject.UpdateIfRequiredOrScript();
            Editor.DrawPropertiesExcluding(serializedObject, new[] { "m_Script" });
            serializedObject.ApplyModifiedProperties();
        }
    }
}
