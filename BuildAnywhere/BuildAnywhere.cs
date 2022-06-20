using BepInEx;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace BuildAnywhere
{
    [BepInPlugin("com.sp00ktober.BuildAnywhere", "BuildAnywhere", "0.0.1")]
    public class BuildAnywhere : BaseUnityPlugin
    {
        private void Awake()
        {
            InitPatches();
        }

        private static void InitPatches()
        {
            Debug.Log("Patching Starsand...");

            try
            {
                Debug.Log("Applying patches from BuildAnywhere 0.0.1");

                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "com.sp00ktober.BuildAnywhere");

                Debug.Log("Patching completed successfully");
            }
            catch (Exception ex)
            {
                Debug.Log("Unhandled exception occurred while patching the game: " + ex);
            }
        }
    }
}
