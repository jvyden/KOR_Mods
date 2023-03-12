using BepInEx;
using HarmonyLib;
using SkinLoader.Patches;

namespace SkinLoader
{
    [BepInPlugin("KOR_Mods.SkinLoader", "Skin Loader", "1.0.0")]
    public class SkinLoaderPlugin : BaseUnityPlugin
    {
        private Harmony _harmony;
        
        private void Awake()
        {
            
        }

        private void Start()
        {
            _harmony = new Harmony("KOR_Mods.SkinLoader");
            _harmony.PatchAll(typeof(CosmeticManagerPatches));
        }
    }
}
