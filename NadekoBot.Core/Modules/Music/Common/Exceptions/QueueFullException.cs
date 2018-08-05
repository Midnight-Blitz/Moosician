using System;

namespace NadekoBot.Modules.Music.Common.Exceptions
{
    public class QueueFullException : Exception
    {
        public QueueFullException(string message) : base(message)
        {
        }

        public QueueFullException() : base("The queue is full.")
        {
        }

        public QueueFullException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
