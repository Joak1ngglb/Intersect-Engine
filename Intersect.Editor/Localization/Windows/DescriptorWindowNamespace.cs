using Intersect.Localization;

using Newtonsoft.Json;

namespace Intersect.Editor.Localization.Windows;

internal partial class DescriptorWindowNamespace : LocaleNamespace
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public readonly LocalizedString Title = @"{00} Editor";

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public readonly LocalizedString SearchQueryHint = @"Search for {00:PluralLower}...";
}
