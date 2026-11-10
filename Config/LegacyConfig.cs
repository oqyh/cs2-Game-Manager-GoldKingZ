using System.Text.Json.Nodes;
namespace Game_Manager_GoldKingZ.Config;
public static class LegacyConfig
{
    public static void Upgrade(JsonObject root)
    {
        void Move(JsonObject obj, string oldKey, string newKey)
        {
            if (!obj.ContainsKey(oldKey)) return;
            if (!obj.ContainsKey(newKey)) obj[newKey] = obj[oldKey]?.DeepClone();
            obj.Remove(oldKey);
        }
        if (root["BlockPing"] is JsonValue v && v.TryGetValue<bool>(out var ping)) root["BlockPing"] = JsonNode.Parse(ping ? "1" : "0");
        if (root["HideBloodAndHsSpark"] is {} blood)
        {
            root["HideBloodDecals"] ??= blood.DeepClone(); root["HideHeadShotSpark"] ??= blood.DeepClone(); root.Remove("HideBloodAndHsSpark");
        }
        Move(root, "Ignore_BombPlantedHUDMessages", "Ignore_BombPlantedHUDMessagesAndSound");
        Move(root, "Cookies_AutoRemovePlayerOlderThanXDays", "Cookies_AutoRemoveInactivePlayersOlderThanDays");
        Move(root, "MySql_AutoRemovePlayerOlderThanXDays", "MySql_AutoRemoveInactivePlayersOlderThanDays");
        void Sounds(JsonObject obj)
        {
            foreach (var entry in obj.ToArray())
                if (entry.Key.Contains("_SoundeventHash") && !entry.Key.Contains("HashAndString") && entry.Value is JsonArray array)
                {
                    var replacement = new JsonArray();
                    foreach (var item in array) replacement.Add(item?.ToString());
                    var key = entry.Key.Replace("_SoundeventHash", "_SoundeventHashAndString");
                    obj[key] ??= replacement; obj.Remove(entry.Key);
                }
        }
        Sounds(root);
        foreach (var entry in root.ToArray()) if (entry.Value is JsonObject child) Sounds(child);
        // Removed geolocation options must not disappear silently.
        if (root["AutoSetPlayerLanguage"] is JsonValue language && language.TryGetValue<bool>(out var enabled) && enabled)
            throw new InvalidDataException("AutoSetPlayerLanguage is unsupported in upstream 2.1.6; config retained. Disable explicitly before upgrading.");
    }
}
