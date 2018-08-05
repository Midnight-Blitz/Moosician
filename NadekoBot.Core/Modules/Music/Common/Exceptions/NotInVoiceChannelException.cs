using System;

namespace NadekoBot.Modules.Music.Common.Exceptions
{
    public class NotInVoiceChannelException : Exception
    {
        public NotInVoiceChannelException() : base("You're not in a voice channel!") { }

        public NotInVoiceChannelException(string message) : base(message)
        {
        }

        public NotInVoiceChannelException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
