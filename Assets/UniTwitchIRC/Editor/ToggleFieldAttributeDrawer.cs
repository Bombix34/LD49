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
using System.Linq;
using System.Collections.Generic;

namespace UniTwitchIRCEditor
{
    /// <summary>
    /// Component drawer for readonly fields to be displayed in the editor
    /// </summary>
    [CustomPropertyDrawer(typeof(ToggleFieldAttribute))]
    public class ToggleFieldAttributeDrawer : PropertyDrawer
    {
        bool m_Hide = false;

        /// <summary>
        /// Draws popup in the Inspector
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool enabled = GUI.enabled;
            
            if (attribute is ToggleFieldAttribute toggleFielsAttr)
            {
                string[] fieldnames = toggleFielsAttr.fieldsNames;

                List<SerializedProperty> props = new List<SerializedProperty>();

                for (int i = 0; i < toggleFielsAttr.fieldsNames.Length; i++)
                {
                    string propName = fieldnames[i];
                    SerializedProperty p = property.serializedObject.FindProperty(propName);
                    props.Add(p);
                }

                if (toggleFielsAttr.isOrOperator)
                {
                    m_Hide = !(props.Where(x => x.boolValue).Count() > 0);
                }
                else
                {
                    m_Hide = !(props.Where(x => x.boolValue).Count() == toggleFielsAttr.fieldsNames.Length);
                }

                if (toggleFielsAttr.readOnly && m_Hide)
                {
                    m_Hide = false;
                    GUI.enabled = false;
                }
            }

            if(!m_Hide)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }

            GUI.enabled = enabled;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if(m_Hide)
            {
                return 0.0f;
            }
            return base.GetPropertyHeight(property, label);
        }
    }
}
