using HarmonyLib;
using UltimateSurvival.Building;

namespace BuildAnywhere.Patches.Dynamic
{
    [HarmonyPatch(typeof(Socket))]
    public class Socket_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Socket), nameof(Socket.SupportsPiece))]
        public static void SupportsPiece_Postfix(Socket __instance, ref bool __result, BuildingPiece piece)
        {
            if(piece.NeededSpace == BuildingSpace.Roof || piece.NeededSpace == BuildingSpace.Floor)
            {
                __result = true;
            }
        }
    }
}
