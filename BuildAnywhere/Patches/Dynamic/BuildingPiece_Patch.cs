using HarmonyLib;
using UltimateSurvival.Building;

namespace BuildAnywhere.Patches.Dynamic
{
    [HarmonyPatch(typeof(BuildingPiece))]
    public class BuildingPiece_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(BuildingPiece), "get_IsLast")]
        public static void get_IsLast_Postfix(BuildingPiece __instance, ref bool __result)
        {
            string name = (string)AccessTools.Field(typeof(BuildingPiece), "m_PieceName").GetValue(__instance);
            if(name.IndexOf("Roof") != -1)
            {
                __result = false;
            }
        }
    }
}
