using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace CheatTools.Patches;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class MovementScriptPatches
{
    private static readonly FieldInfo _movementInfo = AccessTools.Field(typeof(MovementScript), "movement");
    private static readonly FieldInfo _cosmeticSwitcherInfo = AccessTools.Field(typeof(MovementScript), "cosmeticSwitcher");
    private static readonly FieldInfo _rbInfo = AccessTools.Field(typeof(MovementScript), "rb");

    [HarmonyPatch(typeof(MovementScript), "Awake")]
    [HarmonyPostfix]
    public static void AfterAwake(MovementScript __instance)
    {
        if (!CheatToolsPlugin.ConfigBouncy.Value) return;
        
        Rigidbody2D rb = (Rigidbody2D)_rbInfo.GetValue(__instance);
        
        rb.constraints = RigidbodyConstraints2D.None;
        
        PhysicsMaterial2D sharedMaterial = rb.sharedMaterial;
        sharedMaterial.bounciness = 0.5f;
        sharedMaterial.friction = 0.1f;
        rb.sharedMaterial = sharedMaterial;

        // 5 wipes, 5hp at start, because this shit is difficult
        GameManager.Instance.wipes = 5;
        GameManager.Instance.HealthSystem.AddHP(2);
    }
    
    [HarmonyPatch(typeof(MovementScript), nameof(MovementScript.MovementFunction))]
    [HarmonyPrefix]
    public static bool BeforeMovementFunction(MovementScript __instance, float moveHorizontal)
    {
        if (!CheatToolsPlugin.ConfigBouncy.Value) return true;
        if (GameManager.Instance.GameOver) return false;

        CosmeticSwitcher cosmeticSwitcher = (CosmeticSwitcher)_cosmeticSwitcherInfo.GetValue(__instance);
        Vector3 movement = (Vector3)_movementInfo.GetValue(__instance);
        Rigidbody2D rb = (Rigidbody2D)_rbInfo.GetValue(__instance);
        
        _movementInfo.SetValue(__instance, movement * __instance.speed * Time.deltaTime);
        
        // OriginalMovementLogic(__instance, moveHorizontal, movement, rb);
        BouncyMovementLogic(__instance, moveHorizontal, rb);

        if (moveHorizontal * Mathf.Sign(moveHorizontal) > 0.2f && cosmeticSwitcher.cosmeticItem.flippable)
            __instance.transform.localScale = new Vector2(Mathf.Sign(moveHorizontal), 1f);
        
        __instance.transform.position += movement;

        return false;
    }

    private static void OriginalMovementLogic(MovementScript __instance, float moveHorizontal, Rigidbody2D rb)
    {
        rb.velocity = new Vector2(moveHorizontal * (__instance.speed * GameManager.Instance.gameSpeed), rb.velocity.y);
    }

    private static void BouncyMovementLogic(MovementScript __instance, float moveHorizontal, Rigidbody2D rb)
    {
        float force = moveHorizontal * __instance.speed * (GameManager.Instance.gameSpeed / 2);

        rb.AddTorque(-force / 6, ForceMode2D.Impulse);
        rb.AddForce(new Vector2(force / 4, 0), ForceMode2D.Impulse);
    }
}