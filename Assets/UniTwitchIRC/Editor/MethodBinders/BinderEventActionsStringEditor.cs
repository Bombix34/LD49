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

using UniTwitchIRC.TwitchInterface.MethodBinders;
using UnityEditor;
using UnityEngine;

namespace UniTwitchIRCEditor.MethodBinders
{
    /// <summary>
    /// Drawer for binder of String Method
    /// </summary>
    [CustomEditor(typeof(BinderEventActionsString))]
    public class BinderEventActionsStringEditor : BinderEventActionsHelper
    {
        public override GUIContent label => new GUIContent("String Binder");
    }
}
