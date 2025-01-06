using Intersect.Config;
using Intersect.Config.Guilds;
using Intersect.Framework.Core.Config;
using Intersect.Logging;
using Newtonsoft.Json;

namespace Intersect;

public partial class Options
{
    public static int MaxMail => Instance.PlayerOpts.MaxMail;
}
