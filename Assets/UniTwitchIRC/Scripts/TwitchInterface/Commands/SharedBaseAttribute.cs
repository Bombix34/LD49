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

using IRCnect.Channel.Monitor.Replies.Inbounds.Commands;
using UniTwitchIRC.TwitchInterface.Commands;

namespace UniTwitchIRC.TwitchInterface.Commands
{
    /// <summary>
    /// Will not be added to the CommandManager
    /// <para>Use for base class for commands because the abstract keyword will not work</para>
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class SharedBaseAttribute : System.Attribute { }
}