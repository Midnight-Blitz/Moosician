﻿using System;

namespace NadekoBot.Modules.Music.Common.Exceptions
{
    public class SongNotFoundException : Exception
    {
        public SongNotFoundException(string message) : base(message)
        {
        }
        public SongNotFoundException() : base("Song wasn't found.") { }

        public SongNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
