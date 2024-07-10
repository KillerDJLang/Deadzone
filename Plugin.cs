using BepInEx;
using BepInEx.Configuration;
using DrakiaXYZ.VersionChecker;
using System;

namespace DeadzoneMod;

[BepInPlugin("DJ.Deadzone", "Deadzone", "1.2.0")]
public class Plugin : BaseUnityPlugin
{
    public const int TarkovVersion = 30626;

    public static PluginSettings Settings = new();
    public static bool Enabled => Settings.Enabled != null && Settings.Enabled.Value;

    void Awake()
    {
        if (!VersionChecker.CheckEftVersion(Logger, Info, Config))
        {
            throw new Exception("Invalid EFT Version");
        }

        Settings.Enabled = Config.Bind("Values", "Global deadzone enabled", true, new ConfigDescription("Will deadzone be enabled for any group"));

        Settings.WeaponSettings = new WeaponSettingsGroup(
            new WeaponSettings(
                Config,
                new WeaponSettingsOverrides( // no idea why this being empty breaks it
                    Position: 0.1f
                ),
                "Default"
            )
        );

        Settings.Initialized = true;

        DeadzonePatch.Enable();
    }
}

