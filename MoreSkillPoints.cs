using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;



namespace MoreSkillPoints
{
    [BepInPlugin(PLUGIN_GUID , PLUGIN_NAME, PLUGIN_VERSION)]
    public class MoreSkillPoints : BaseUnityPlugin
    {
        public static ManualLogSource Log;
            
        private const string PLUGIN_GUID = "juan514.poe.moreskillpoints";
        private const string PLUGIN_NAME = "More Skill Points";
        private const string PLUGIN_VERSION = "0.0.1";

        private readonly Harmony harmony = new Harmony(PLUGIN_GUID);

        private void Awake()
        {
            // Plugin startup logic
            Log = Logger;
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_NAME} is loaded!");
            harmony.PatchAll();
        }
    }
    
    [HarmonyPatch(typeof(UICharacterCreationManager), "Show")] 
    class ShowPatch
    {
        internal static ILGenerator generator;
        
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {

            CodeMatcher codeMatcher = new CodeMatcher(instructions, generator);
            return codeMatcher.MatchForward(false, new CodeMatch[1] {new CodeMatch(new OpCode?(OpCodes.Ldc_I4_6), (object)null, (string)null) }).RemoveInstructions(1).Insert(new CodeInstruction[1] { new CodeInstruction(OpCodes.Ldc_I4, (object)16) }).InstructionEnumeration();
            
        }
    }
}