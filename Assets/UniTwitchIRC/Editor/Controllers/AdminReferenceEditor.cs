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

using UniTwitchIRC.Controllers;
using UnityEditor;

namespace UniTwitchIRCEditor.Controllers
{
    /// <summary>
    /// Editor to draw admin reference object with/without try it button
    /// </summary>
    [CustomEditor(typeof(AdminReference))]
    public class AdminReferenceEditor : Editor
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
