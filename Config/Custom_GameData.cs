using System.Runtime.InteropServices;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
using CounterStrikeSharp.API.Core;
using Newtonsoft.Json.Linq;
using Game_Manager_GoldKingZ.Config;

namespace Game_Manager_GoldKingZ;

public class CustomGameData
{
    private static CustomGameData? _instance;
    public static CustomGameData? Instance => _instance;

    private readonly Dictionary<string, string> _signatures = new();
    private readonly Dictionary<string, string> _libraries  = new();
    private readonly Dictionary<string, int>    _offsets    = new();
    private bool _isLoaded = false;
    internal static string Status => _instance == null ? "unloaded" : _instance._isLoaded ? "loaded" : "failed";

    private static string? GetModulePath(string library) => library.ToLowerInvariant() switch
    {
        "host"                   => Path.Join(Server.GameDirectory, Constants.GameBinaryPath, $"{Constants.ModulePrefix}host{Constants.ModuleSuffix}"),
        "matchmaking"            => Path.Join(Server.GameDirectory, Constants.GameBinaryPath, $"{Constants.ModulePrefix}matchmaking{Constants.ModuleSuffix}"),
        "server"                 => Path.Join(Server.GameDirectory, Constants.GameBinaryPath, $"{Constants.ModulePrefix}server{Constants.ModuleSuffix}"),

        "animationsystem"        => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}animationsystem{Constants.ModuleSuffix}"),
        "avcodec"                => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}avcodec{Constants.ModuleSuffix}"),
        "avformat"               => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}avformat{Constants.ModuleSuffix}"),
        "avresample"             => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}avresample{Constants.ModuleSuffix}"),
        "avutil"                 => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}avutil{Constants.ModuleSuffix}"),
        "cairo"                  => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}cairo{Constants.ModuleSuffix}"),
        "engine2"                => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}engine2{Constants.ModuleSuffix}"),
        "filesystem_stdio"       => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}filesystem_stdio{Constants.ModuleSuffix}"),
        "fontconfig"             => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}fontconfig{Constants.ModuleSuffix}"),
        "freetype"               => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}freetype{Constants.ModuleSuffix}"),
        "inputsystem"            => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}inputsystem{Constants.ModuleSuffix}"),
        "localize"               => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}localize{Constants.ModuleSuffix}"),
        "materialsystem2"        => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}materialsystem2{Constants.ModuleSuffix}"),
        "meshsystem"             => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}meshsystem{Constants.ModuleSuffix}"),
        "mpg123"                 => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}mpg123{Constants.ModuleSuffix}"),
        "networksystem"          => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}networksystem{Constants.ModuleSuffix}"),
        "ogg"                    => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}ogg{Constants.ModuleSuffix}"),
        "pango-1.0"              => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}pango-1.0{Constants.ModuleSuffix}"),
        "pangoft2-1.0"           => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}pangoft2-1.0{Constants.ModuleSuffix}"),
        "panorama"               => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}panorama{Constants.ModuleSuffix}"),
        "panorama_text_pango"    => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}panorama_text_pango{Constants.ModuleSuffix}"),
        "panoramauiclient"       => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}panoramauiclient{Constants.ModuleSuffix}"),
        "particles"              => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}particles{Constants.ModuleSuffix}"),
        "phonon"                 => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}phonon{Constants.ModuleSuffix}"),
        "pulse_system"           => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}pulse_system{Constants.ModuleSuffix}"),
        "rendersystemempty"      => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}rendersystemempty{Constants.ModuleSuffix}"),
        "rendersystemvulkan"     => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}rendersystemvulkan{Constants.ModuleSuffix}"),
        "resourcesystem"         => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}resourcesystem{Constants.ModuleSuffix}"),
        "scenefilecache"         => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}scenefilecache{Constants.ModuleSuffix}"),
        "scenesystem"            => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}scenesystem{Constants.ModuleSuffix}"),
        "schemasystem"           => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}schemasystem{Constants.ModuleSuffix}"),
        "sdl3"                   => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}SDL3{Constants.ModuleSuffix}"),
        "soundsystem"            => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}soundsystem{Constants.ModuleSuffix}"),
        "steam_api"              => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}steam_api{Constants.ModuleSuffix}"),
        "steamaudio"             => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}steamaudio{Constants.ModuleSuffix}"),
        "steamnetworkingsockets" => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}steamnetworkingsockets{Constants.ModuleSuffix}"),
        "swscale"                => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}swscale{Constants.ModuleSuffix}"),
        "tier0"                  => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}tier0{Constants.ModuleSuffix}"),
        "v8"                     => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}v8{Constants.ModuleSuffix}"),
        "v8_icui18n"             => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}v8_icui18n{Constants.ModuleSuffix}"),
        "v8_icuuc"               => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}v8_icuuc{Constants.ModuleSuffix}"),
        "v8_libbase"             => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}v8_libbase{Constants.ModuleSuffix}"),
        "v8_libcpp"              => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}v8_libcpp{Constants.ModuleSuffix}"),
        "v8_libplatform"         => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}v8_libplatform{Constants.ModuleSuffix}"),
        "v8_zlib"                => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}v8_zlib{Constants.ModuleSuffix}"),
        "v8system"               => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}v8system{Constants.ModuleSuffix}"),
        "vconcomm"               => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}vconcomm{Constants.ModuleSuffix}"),
        "video"                  => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}video{Constants.ModuleSuffix}"),
        "vorbis"                 => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}vorbis{Constants.ModuleSuffix}"),
        "vorbisenc"              => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}vorbisenc{Constants.ModuleSuffix}"),
        "vorbisfile"             => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}vorbisfile{Constants.ModuleSuffix}"),
        "vphysics2"              => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}vphysics2{Constants.ModuleSuffix}"),
        "vpx"                    => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}vpx{Constants.ModuleSuffix}"),
        "vscript"                => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}vscript{Constants.ModuleSuffix}"),
        "worldrenderer"          => Path.Join(Server.GameDirectory, Constants.RootBinaryPath, $"{Constants.ModulePrefix}worldrenderer{Constants.ModuleSuffix}"),
        _                        => null
    };

    public static void Load()
    {
        if (_instance != null) return;

        _instance = new CustomGameData();
        _instance.LoadFromJson();

        if (!_instance._isLoaded)
        {
            Helper.DebugMessage("GameData failed to load", 0);
            _instance = null;
            return;
        }

        CustomHooks.Init(_instance);

        Helper.DebugMessage("GameData loaded");
    }

    public static void Unload()
    {
        if (_instance == null) return;
        
        CustomHooks.Cleanup();

        _instance = null;
        Helper.DebugMessage("GameData unloaded");
    }

    public string GetSignature(string key)  => _signatures.TryGetValue(key, out var s) ? s : string.Empty;
    public int    GetOffset(string key)     => _offsets.TryGetValue(key, out var v) ? v : -1;
    public string GetLibrary(string key)    => _libraries.GetValueOrDefault(key, "server");

    public T? CreateFunction<T>(string key) where T : class
    {
        string sig = GetSignature(key);
        if (string.IsNullOrEmpty(sig))
        {
            Helper.DebugMessage($"{key} Signature Missing", 0);
            return null;
        }

        var module = GetModulePath(GetLibrary(key));

        try
        {
            return module != null
                ? (T)Activator.CreateInstance(typeof(T), sig, module)!
                : (T)Activator.CreateInstance(typeof(T), sig)!;
        }
        catch (Exception ex)
        {
            Helper.DebugMessage($"{key} CreateFunction Error: {ex.Message}", 0);
            return null;
        }
    }

    private void LoadFromJson()
    {
        string jsonFilePath = Path.Combine(MainPlugin.Instance.ModuleDirectory, "gamedata/gamedata.json");
        if (!File.Exists(jsonFilePath))
        {
            Helper.DebugMessage("gamedata.json Not Found", 0);
            return;
        }

        try
        {
            var jsonObject = JObject.Parse(File.ReadAllText(jsonFilePath));
            string platformKey = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "linux" : "windows";

            _signatures.Clear();
            _libraries.Clear();
            _offsets.Clear();

            foreach (var item in jsonObject.Properties())
            {
                string key  = item.Name;
                var    data = item.Value;

                if (data["signatures"]?[platformKey] is { } sig) _signatures[key] = sig.ToString();
                _libraries[key] = data["signatures"]?["library"]?.ToString() ?? "server";
                if (data["offsets"]?[platformKey]  is { } off)   _offsets[key]   = off.Value<int>();
            }

            _isLoaded = true;
        }
        catch (Exception ex)
        {
            Helper.DebugMessage($"LoadFromJson Error: {ex.Message}", 0);
        }
    }
}