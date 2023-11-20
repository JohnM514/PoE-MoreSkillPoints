using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using BepInEx.Configuration;


namespace MoreSkillPoints
{
    [BepInPlugin(PLUGIN_GUID , PLUGIN_NAME, PLUGIN_VERSION)]
    public class MoreSkillPoints : BaseUnityPlugin
    {

        private const string PLUGIN_GUID = "juan514.poe.moreskillpoints";
        private const string PLUGIN_NAME = "More Skill Points";
        private const string PLUGIN_VERSION = "0.0.2";

        private readonly Harmony harmony = new Harmony(PLUGIN_GUID);

        internal static ConfigEntry<int> configSkillPointsPerLevel;

        private void Awake()
        {
            // Plugin startup logic
            configSkillPointsPerLevel = Config.Bind("Level Up", "SkillPointsPerLevel", 6,
                "Number of skill points to spend per level");
            
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
            return codeMatcher.MatchForward(false, new CodeMatch(new OpCode? (OpCodes.Ldc_I4_6))).RemoveInstruction()
                .Insert(new CodeInstruction(OpCodes.Ldc_I4, MoreSkillPoints.configSkillPointsPerLevel.Value))
                .InstructionEnumeration();
        }
    }
    
    [HarmonyPatch(typeof(UICharacterCreationManager), "HandleLevelLooping")] 
    class HandleLevelLoopingPatch
    {
        internal static ILGenerator generator;
        
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {

            CodeMatcher codeMatcher = new CodeMatcher(instructions, generator);
            return codeMatcher.MatchForward(false, new CodeMatch(new OpCode?(OpCodes.Ldc_I4_6))).RemoveInstruction()
                .Insert(new CodeInstruction(OpCodes.Ldc_I4, MoreSkillPoints.configSkillPointsPerLevel.Value))
                .InstructionEnumeration();
        }
    }
    
}