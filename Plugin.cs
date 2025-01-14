using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SongOrganizer.Data;
using UnityEngine;
using UnityEngine.UI;

namespace SongOrganizer;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static Plugin Instance;
    public static ManualLogSource Log;
    public static Toggle Toggle;
    public static Button Button;
    public static Options Options;

    public static Dictionary<string, Track> TrackDict = new Dictionary<string, Track>();
    public static KeyCode[] KeyCodes = new KeyCode[36];

    public const int TRACK_SCORE_LENGTH = 5;
    public static Button[] DeleteButtons = new Button[TRACK_SCORE_LENGTH + 1];

    private const string FILTER_SECTION = "Filter";
    private const string SORT_SECTION = "Sort";

    private void Awake()
    {
        Instance = this;
        Log = Logger;
        Options = new Options
        {
            ShowDefault = Config.Bind(FILTER_SECTION, nameof(Options.ShowDefault), true),
            ShowCustom = Config.Bind(FILTER_SECTION, nameof(Options.ShowCustom), true),
            ShowUnplayed = Config.Bind(FILTER_SECTION, nameof(Options.ShowUnplayed), true),
            ShowPlayed = Config.Bind(FILTER_SECTION, nameof(Options.ShowPlayed), true),
            ShowNotSRank = Config.Bind(FILTER_SECTION, nameof(Options.ShowNotSRank), true),
            ShowSRank = Config.Bind(FILTER_SECTION, nameof(Options.ShowSRank), true),
            SortMode = Config.Bind(SORT_SECTION, nameof(Options.SortMode), "default"),
        };
        for (int i = (int)KeyCode.A, j = 0; i <= (int)KeyCode.Z; i++, j++)
        {
            KeyCodes[j] = (KeyCode)i;
        }
        for (int i = (int)KeyCode.Alpha0, j = 0; i <= (int)KeyCode.Alpha9; i++, j++)
        {
            KeyCodes[j + 26] = (KeyCode)i;
        }
        new Harmony(PluginInfo.PLUGIN_GUID).PatchAll();
    }

    public static bool IsCustomTrack(string trackReference)
    {
        return !File.Exists(Path.Combine(Application.dataPath, "StreamingAssets", "leveldata", $"{trackReference}.tmb"));
    }
}