using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Text.Json.Serialization;

namespace SlurmFortress.Web.Security;

/// <summary>
/// Custom User Account class.
/// Identity token properties map automatically 
/// </summary>
public class CustomUserAccount : RemoteUserAccount
{
    [JsonPropertyName("roles")]
    public string[] Roles { get; set; } = Array.Empty<string>();

    [JsonPropertyName("groups")]
    public string[] Groups { get; set; } = Array.Empty<string>();
}
