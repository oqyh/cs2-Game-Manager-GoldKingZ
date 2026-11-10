using Newtonsoft.Json.Linq;
namespace Game_Manager_GoldKingZ;
// A snapshot of configuration structure, never a cache of player permissions.
public sealed class RuleIndex
{
    public readonly (string Name, JObject? Value)[] Groups;
    public readonly JObject? Any;
    public RuleIndex(JObject root)
    {
        var groups = new List<(string, JObject?)>(); bool foundAny = false;
        foreach (var property in root.Properties())
        {
            if (property.Name.Equals("ANY", StringComparison.OrdinalIgnoreCase))
            {
                if (!foundAny) { Any=property.Value as JObject; foundAny=true; }
            }
            else groups.Add((property.Name, property.Value as JObject));
        }
        Groups=groups.ToArray();
    }
}
public static class SoundRules
{
    public static bool Matches(IReadOnlyList<string> patterns, string sound)
    {
        for(int i=0;i<patterns.Count;i++)
            if(sound.Contains(patterns[i],StringComparison.OrdinalIgnoreCase)) return true;
        return false;
    }
}
