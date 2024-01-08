using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using ModelReplacement;
using BepInEx.Configuration;
using System;
using BepInEx.Logging;
using UnityEngine.Assertions;

namespace LethalCroc
{
    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInDependency("meow.ModelReplacementAPI", BepInDependency.DependencyFlags.HardDependency)]
    public class LethalCrocBase : BaseUnityPlugin
    {
        private const string modGUID = "Aze.LethalCroc";
        private const string modName = "Lethal Croc";
        private const string modVersion = "1.0.0.0";

        public static ConfigFile config;

        // Universal config options 
        public static ConfigEntry<bool> enableCrocForAllSuits { get; private set; }
        public static ConfigEntry<bool> enableCrocAsDefault { get; private set; }
        public static ConfigEntry<string> suitNamesToEnableCroc { get; private set; }

        
        private void Awake()
        {

            config = base.Config;
            InitConfig();
            Assets.PopulateAssets();

            // Plugin startup logic
            if (enableCrocForAllSuits.Value)
            {
                ModelReplacementAPI.RegisterModelReplacementOverride(typeof(AvatarReplacement));

            }
            if (enableCrocAsDefault.Value)
            {
                ModelReplacementAPI.RegisterModelReplacementDefault(typeof(AvatarReplacement));

            }

            var commaSepList = suitNamesToEnableCroc.Value.Split(',');
            foreach (var item in commaSepList)
            {
                ModelReplacementAPI.RegisterSuitModelReplacement(item, typeof(AvatarReplacement));
            }

            Harmony harmony = new Harmony("Aze.LethalCroc");
            harmony.PatchAll();
            Logger.LogInfo($"Plugin {"Aze.LethalCroc"} is loaded!");
        }

        private static void InitConfig()
        {
            enableCrocForAllSuits = config.Bind<bool>("Suits to Replace Settings", "Enable Croc for all Suits", false, "Enable to replace every suit with Croc. Set to false to specify suits");
            enableCrocAsDefault = config.Bind<bool>("Suits to Replace Settings", "Enable Croc as default", false, "Enable to replace every suit that hasn't been otherwise registered with Croc.");
            suitNamesToEnableCroc = config.Bind<string>("Suits to Replace Settings", "Suits to enable Croc for", "Default,Orange suit", "Enter a comma separated list of suit names.(Additionally, [Green suit,Pajama suit,Hazard suit])");
        }
    }

    public static class Assets
    {
        // Replace mbundle with the Asset Bundle Name from your unity project 
        public static string mainAssetBundleName = "lethalcrocbundle";
        public static AssetBundle MainAssetBundle = null;

        private static string GetAssemblyName() => Assembly.GetExecutingAssembly().GetName().Name;
        public static void PopulateAssets()
        {
            if (MainAssetBundle == null)
            {
                Console.WriteLine(GetAssemblyName() + "." + mainAssetBundleName);
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(GetAssemblyName() + "." + mainAssetBundleName))
                {
                    MainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                }

            }
        }
    }
}
