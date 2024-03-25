using System;
using System.Collections.Generic;
using System.IO;
using Godot;

namespace GoConductor;

public class SettingsReader
{
    public static string WelcomeMessage => _settingsDict["welcomeMessage"];
    public static string MusicBus => _settingsDict["musicBus"];

    private static Dictionary<string, string> _settingsDict = ReadSettings("GoConductor/settings.txt");
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
            _settingsDict.Add(split[0], split[1]);
        }

        return dict;
    }
}