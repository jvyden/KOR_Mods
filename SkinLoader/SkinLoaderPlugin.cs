using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SkinLoader.Patches;

namespace SkinLoader
{
    [BepInPlugin("KOR_Mods.SkinLoader", "Skin Loader", "1.0.1")]
    public class SkinLoaderPlugin : BaseUnityPlugin
    {
        private Harmony _harmony;
        
        // ReSharper disable once InconsistentNaming
        internal static ManualLogSource _Logger;

        private void Start()
        {
            _harmony = new Harmony("KOR_Mods.SkinLoader");
            _harmony.PatchAll(typeof(CosmeticManagerPatches));
            
            _Logger = this.Logger;
        }
    }
}
