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
using UnityEngine;

namespace UniTwitchIRC.TwitchInterface
{
    /// <summary>
    /// Attribute to specify that the string field will be disabled
    /// <para>Use for visual members only</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public class ReadonlyFieldAttribute : PropertyAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly bool always;

        public ReadonlyFieldAttribute(bool always = true)
        {
            this.always = always;
        }
    }
}