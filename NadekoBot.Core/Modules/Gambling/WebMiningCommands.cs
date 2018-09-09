///Enables it.
///#if !GLOBAL_NADEKO
///Disables it
#if GLOBAL_NADEKO
using NadekoBot.Core.Modules.Gambling.Services;

namespace NadekoBot.Modules.Gambling
{
    public partial class Gambling
    {
        public class WebMiningCommands : NadekoSubmodule<WebMiningService>
        {

        }
    }
}
#endif