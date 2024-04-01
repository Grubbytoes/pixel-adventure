using System;
using System.Collections.Generic;
using System.IO;
using Godot;

namespace GoConductor;

public class SettingsReader
{
    private static readonly Dictionary<string, string> _settingsDict = ReadSettings("GoConductor/settings.txt");
    public static string WelcomeMessage => _settingsDict["welcomeMessage"];
    public static string MusicBus => _settingsDict["musicBus"];

    private static Dictionary<string, string> ReadSettings(string path)
    {
        string[] raw;
        try
        {
            raw = File.ReadAllLines(path);
        }
        catch (Exception e)
        {
            GD.PushError("GoConductor: Could not read settings.txt!" + e.Message);
            return null;
        }

        var dict = new Dictionary<string, string>();

        // Iterate through each line
        foreach (var line in raw)
        {
            var split = line.Replace(" ", "").Split(':');
            dict.Add(split[0], split[1]);
        }

        return dict;
    }
}