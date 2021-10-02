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
    public class ToggleFieldAttribute : PropertyAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly string[] fieldsNames;
        /// <summary>
        /// 
        /// </summary>
        public readonly bool isOrOperator;

        /// <summary>
        /// 
        /// </summary>
        public readonly bool readOnly;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldsNames"></param>
        /// <param name="isOrOperator">Are the fields boolean values comapred using '&&' or '||' operator</param>
        public ToggleFieldAttribute(string[] fieldsNames, bool isOrOperator = false)
        {
            this.isOrOperator = isOrOperator;
            this.fieldsNames = fieldsNames;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldsName"></param>
        /// <param name="readOnly">Makes field readonly instead of hiding it</param>
        public ToggleFieldAttribute(string fieldsName, bool readOnly = false)
        {
            this.readOnly = readOnly;
            this.fieldsNames = new[] { fieldsName };
        }
    }
}