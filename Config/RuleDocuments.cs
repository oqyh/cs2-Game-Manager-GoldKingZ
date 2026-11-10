using Newtonsoft.Json.Linq;
namespace Game_Manager_GoldKingZ;
internal static class RuleDocuments
{
    internal static JObject Read(string path, bool allowEmpty = false)
    {
        var text = string.Join("\n", File.ReadAllLines(path).Where(l => !l.TrimStart().StartsWith("//")));
        // Older installations intentionally used empty rule files. Accept only at startup;
        // an empty reload remains a rejected edit, preserving the running rules.
        return allowEmpty && string.IsNullOrWhiteSpace(text) ? new JObject() : JObject.Parse(text);
    }
}
