using Microsoft.AspNetCore.Components.Routing;

namespace Goke.Blazor.Components.Models;

public sealed class NavMenuItem
{
    public required string Title { get; init; }
    public required string Href { get; init; }
    public string? IconCssClass { get; init; }
    public NavLinkMatch Match { get; init; } = NavLinkMatch.Prefix;
    public List<NavMenuItem>? Dropdown { get; set; }
    public bool IsDisabled { get; internal set; }
    public string Icon { get; set; } = string.Empty;
    public bool IsDivider { get; set; }
}