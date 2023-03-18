using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CheatTools.Movement;
using HarmonyLib;
using UnityEngine;

namespace CheatTools.Patches;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class MovementScriptPatches
{
    private static readonly FieldInfo _cosmeticSwitcherInfo = AccessTools.Field(typeof(MovementScript), "cosmeticSwitcher");
    private static readonly FieldInfo _colliderInfo = AccessTools.Field(typeof(MovementScript), "_collider");

    [HarmonyPatch(typeof(MovementScript), "Awake")]
    [HarmonyPostfix]
    public static void AfterAwake(MovementScript __instance)
    {
        if (CheatToolsPlugin.ConfigBouncy.Value) __instance.gameObject.AddComponent<BouncyMovement>();
        if (CheatToolsPlugin.ConfigMouseX.Value) __instance.gameObject.AddComponent<MouseXMovement>();

        if (CheatToolsPlugin.ConfigCircle.Value)
        {
            Object.Destroy(__instance.GetComponent<BoxCollider2D>());
            Collider2D collider = __instance.gameObject.AddComponent<CircleCollider2D>();
            
            _colliderInfo.SetValue(__instance, collider);
        }
    }

    [HarmonyPatch(typeof(MovementScript), nameof(MovementScript.MovementFunction))]
    [HarmonyPrefix]
    public static bool BeforeMovementFunction(MovementScript __instance, float moveHorizontal)
    {
        if (!CheatToolsPlugin.ConfigBouncy.Value) return true;
        if (GameManager.Instance.GameOver) return false;

        CosmeticSwitcher cosmeticSwitcher = (CosmeticSwitcher)_cosmeticSwitcherInfo.GetValue(__instance);

        BouncyMovement.Instance.RunLogic(moveHorizontal);

        if (moveHorizontal * Mathf.Sign(moveHorizontal) > 0.2f && cosmeticSwitcher.cosmeticItem.flippable)
            __instance.transform.localScale = new Vector2(Mathf.Sign(moveHorizontal), 1f);

        return false;
    }
}