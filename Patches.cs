using Random = UnityEngine.Random;
using HarmonyLib;
using UnityEngine;
using static HearthyFood.HearthyFoodPlugin;

namespace HearthyFood;

[HarmonyPatch(typeof(Player), nameof(Player.EatFood))]
static class Player__Patch
{
    static void Postfix(Player __instance, ItemDrop.ItemData item)
    {
        float randval = Random.value;
        if (randval <= HearthChance.Value / 100f)
        {
            HearthyFoodLogger.LogDebug(
                $"You have teleported! CHANCE WAS: {HearthChance.Value} and your random value was {randval}");
            if (NoFoodBenefit.Value == Toggle.On)
            {
                HearthyFoodLogger.LogDebug(
                    $"Receiving no benefit from the food, consuming {Localization.instance.Localize(item.m_shared.m_name)}");
                Player.m_localPlayer.ClearFood();
            }

            Player.m_localPlayer.TeleportTo(
                new Vector3(Random.Range(0, 10000), Random.Range(0.5f, 50), Random.Range(0, 10000)),
                Quaternion.identity, true);
            Player.m_localPlayer.m_lastGroundTouch = 0f;
        }
        else
        {
            HearthyFoodLogger.LogDebug(
                $"Lucky, you didn't teleport! CHANCE WAS: {HearthChance.Value} and your random value was {randval}");
        }
    }
}