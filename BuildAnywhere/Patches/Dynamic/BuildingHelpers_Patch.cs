using HarmonyLib;
using UltimateSurvival.Building;
using UnityEngine;

namespace BuildAnywhere.Patches.Dynamic
{
    [HarmonyPatch(typeof(BuildingHelpers))]
    public class BuildingHelpers_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(BuildingHelpers), nameof(BuildingHelpers.IsTouchingGround))]
        public static void IsTouchingGround_Postfix(BuildingHelpers __instance, ref bool __result)
        {
            __result = true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(BuildingHelpers), nameof(BuildingHelpers.HandleSnapPreview))]
        public static bool HandleSnapPreview_Prefix(BuildingHelpers __instance, Collider[] buildingPieces)
        {
            BuildingPiece bp = (BuildingPiece)AccessTools.Field(typeof(BuildingHelpers), "m_CurrentPreviewPiece").GetValue(__instance);
            BuildingPiece pf = (BuildingPiece)AccessTools.Field(typeof(BuildingHelpers), "m_CurrentPrefab").GetValue(__instance);
            Transform tr = (Transform)AccessTools.Field(typeof(BuildingHelpers), "m_Transform").GetValue(__instance);

            int br = (int)AccessTools.Field(typeof(BuildingHelpers), "m_BuildRange").GetValue(__instance);
            Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
            float num = float.PositiveInfinity;
            
            Socket socket = null;
            for(int i = 0; i < buildingPieces.Length; i++)
            {
                BuildingPiece component = buildingPieces[i].GetComponent<BuildingPiece>();
                if(component != null && component.Sockets.Length != 0)
                {
                    for(int j = 0; j < component.Sockets.Length; j++)
                    {
                        Socket socket2 = component.Sockets[j];
                        if ((socket2.transform.position - tr.position).sqrMagnitude < Mathf.Pow((float)br, 2f))
                        {
                            float num2 = Vector3.Angle(ray.direction, socket2.transform.position - ray.origin);
                            if (num2 < num && num2 < 35f)
                            {
                                num = num2;
                                socket = socket2;
                            }
                        }
                    }
                }
                Socket.PieceOffset pieceOffset;
                if (socket != null && socket.GetPieceOffsetByName(pf.Name, out pieceOffset))
                {
                    bp.transform.position = socket.transform.position + socket.transform.TransformVector(pieceOffset.PositionOffset);
                    bp.transform.rotation = socket.transform.rotation * pieceOffset.RotationOffset;
                    AccessTools.Field(typeof(BuildingHelpers), "m_LastValidSocket").SetValue(__instance, socket);
                    AccessTools.Field(typeof(BuildingHelpers), "m_HasSocket").SetValue(__instance, true);
                    return false;
                }
            }

            return true;
        }
    }
}
