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
    /// Component drawer for readonly fields to be displayed in the editor
    /// </summary>
    [CustomPropertyDrawer(typeof(ReadonlyFieldAttribute))]
    public class ReadonlyFieldAttributeDrawer : PropertyDrawer
    {
        /// <summary>
        /// Draws popup in the Inspector
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool enabled = GUI.enabled;
            
            if (attribute is ReadonlyFieldAttribute readonlyAttr)
            {
                if (readonlyAttr.always == true)
                {
                    GUI.enabled = false;
                }
                else
                {
                    GUI.enabled = !EditorApplication.isPlaying;
                }
            }
            EditorGUI.PropertyField(position, property, label, false);

            GUI.enabled = enabled;
        }
    }
}
