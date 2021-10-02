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

namespace UniTwitchIRC.TwitchInterface
{
    public static class ExceptionHelper
    {
        public const string APP_REGISTRATION = "https://dev.twitch.tv/docs/authentication#registration";

        public static void ThrowIfEmpty<T>(T value, Func<T, bool> pred, string message)
        {
            if (pred(value))
            {
                throw new ArgumentException(message);
            }
        }

    }
}
