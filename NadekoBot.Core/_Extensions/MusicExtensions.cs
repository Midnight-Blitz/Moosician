using Discord;

namespace NadekoBot.Extensions
{
    public static class MusicExtensions
    {
        public static EmbedAuthorBuilder WithMusicIcon(this EmbedAuthorBuilder eab) =>
            eab.WithIconUrl("https://i.imgur.com/tZDnx9B.png");
    }
}
