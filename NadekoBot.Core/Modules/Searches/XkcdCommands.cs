﻿using Discord;
using Discord.Commands;
using NadekoBot.Extensions;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using NadekoBot.Common;
using NadekoBot.Common.Attributes;

namespace NadekoBot.Modules.Searches
{
    public partial class Searches
    {
        [Group]
        public class XkcdCommands : NadekoSubmodule
        {
            private const string _xkcdUrl = "https://xkcd.com";
            private readonly IHttpClientFactory _httpFactory;

            public XkcdCommands(IHttpClientFactory factory)
            {
                _httpFactory = factory;
            }

            [NadekoCommand, Usage, Description, Aliases]
            [Priority(0)]
            public async Task Xkcd(string arg = null)
            {
                if (arg?.ToLowerInvariant().Trim() == "latest")
                {
                    try
                    {
                        using (var http = _httpFactory.CreateClient())
                        {
                            var res = await http.GetStringAsync($"{_xkcdUrl}/info.0.json").ConfigureAwait(false);
                            var comic = JsonConvert.DeserializeObject<XkcdComic>(res);
                            var embed = new EmbedBuilder().WithColor(NadekoBot.OkColor)
                                                      .WithImageUrl(comic.ImageLink)
                                                      .WithAuthor(eab => eab.WithName(comic.Title).WithUrl($"{_xkcdUrl}/{comic.Num}").WithIconUrl("https://cdn.discordapp.com/avatars/341297873939464193/9f0716501291a996c8783f2af637a16d.png"))
                                                      .AddField(efb => efb.WithName(GetText("comic_number")).WithValue(comic.Num.ToString()).WithIsInline(true))
                                                      .AddField(efb => efb.WithName(GetText("date")).WithValue($"{comic.Month}/{comic.Year}").WithIsInline(true));
                            var sent = await Context.Channel.EmbedAsync(embed)
                                         .ConfigureAwait(false);

                            await Task.Delay(10000).ConfigureAwait(false);

                            await sent.ModifyAsync(m => m.Embed = embed.AddField(efb => efb.WithName("Alt").WithValue(comic.Alt.ToString()).WithIsInline(false)).Build()).ConfigureAwait(false);
                        }
                    }
                    catch (HttpRequestException)
                    {
                        await ReplyErrorLocalizedAsync("comic_not_found").ConfigureAwait(false);
                    }
                    return;
                }
                await Xkcd(new NadekoRandom().Next(1, 1750)).ConfigureAwait(false);
            }

            [NadekoCommand, Usage, Description, Aliases]
            [Priority(1)]
            public async Task Xkcd(int num)
            {
                if (num < 1)
                    return;
                try
                {
                    using (var http = _httpFactory.CreateClient())
                    {
                        var res = await http.GetStringAsync($"{_xkcdUrl}/{num}/info.0.json").ConfigureAwait(false);

                        var comic = JsonConvert.DeserializeObject<XkcdComic>(res);
                        var embed = new EmbedBuilder().WithColor(NadekoBot.OkColor)
                                                      .WithImageUrl(comic.ImageLink)
                                                      .WithAuthor(eab => eab.WithName(comic.Title).WithUrl($"{_xkcdUrl}/{num}").WithIconUrl("https://cdn.discordapp.com/avatars/341297873939464193/9f0716501291a996c8783f2af637a16d.png"))
                                                      .AddField(efb => efb.WithName(GetText("comic_number")).WithValue(comic.Num.ToString()).WithIsInline(true))
                                                      .AddField(efb => efb.WithName(GetText("date")).WithValue($"{comic.Month}/{comic.Year}").WithIsInline(true));
                        var sent = await Context.Channel.EmbedAsync(embed)
                                     .ConfigureAwait(false);

                        await Task.Delay(10000).ConfigureAwait(false);

                        await sent.ModifyAsync(m => m.Embed = embed.AddField(efb => efb.WithName("Alt").WithValue(comic.Alt.ToString()).WithIsInline(false)).Build()).ConfigureAwait(false);
                    }
                }
                catch (HttpRequestException)
                {
                    await ReplyErrorLocalizedAsync("comic_not_found").ConfigureAwait(false);
                }
            }
        }

        public class XkcdComic
        {
            public int Num { get; set; }
            public string Month { get; set; }
            public string Year { get; set; }
            [JsonProperty("safe_title")]
            public string Title { get; set; }
            [JsonProperty("img")]
            public string ImageLink { get; set; }
            public string Alt { get; set; }
        }
    }
}
