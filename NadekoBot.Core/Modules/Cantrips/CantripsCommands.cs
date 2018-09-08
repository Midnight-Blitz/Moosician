using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using NadekoBot.Common;
using NadekoBot.Common.Attributes;
using NadekoBot.Common.Replacements;
using NadekoBot.Core.Modules.Searches.Common;
using NadekoBot.Core.Services;
using NadekoBot.Extensions;
///using NadekoBot.Modules.Cantrips.Common;
///using NadekoBot.Modules.Cantrips.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Configuration = AngleSharp.Configuration;

namespace NadekoBot.Modules.Cantrips
{
    public partial class Cantrips : NadekoTopLevelModule
    {
        [Group]
        public class NoContext : NadekoSubmodule
        {
            private readonly IBotCredentials _creds;
            private readonly IImageCache _images;
            private static readonly NadekoRandom _rng = new NadekoRandom();

            public NoContext(IBotCredentials creds, IDataCache data)
            {
                _creds = creds;
                _images = data.LocalImages;
            }

            ///public Uri GetRandomBlackmail()
            public byte[] GetRandomBlackmail()
            {
                var rng = new NadekoRandom();
                ///var blk = _images.ImageUrls.NoContext;
                ///return blk[rng.Next(0, blk.Length)];
                return _images.NoContext[rng.Next(0, _images.NoContext.Count)];
            }

            [NadekoCommand, Usage, Description, Aliases]
            [RequireContext(ContextType.Guild)]
            public async Task Blackmail()
            {
                ///    var file = GetRandomBlackmail();

                ///await Context.Channel.EmbedAsync(new EmbedBuilder()
                ///        .WithOkColor()
                ///        .WithImageUrl(file.ToString())).ConfigureAwait(false);
                var channel = (ITextChannel)Context.Channel;

                using (var stream = GetRandomBlackmail().ToStream())
                {
                    await channel.SendFileAsync(stream, "nc.png").ConfigureAwait(false);
                }
            }
        }
    }
}