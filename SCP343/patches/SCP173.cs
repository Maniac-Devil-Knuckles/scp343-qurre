/*#pragma warning disable SA1118
#pragma warning disable SA1313
using Qurre.API;
namespace SCP343.Patches
{

    using HarmonyLib;

    using NorthwoodLib.Pools;

    using UnityEngine;

    using static HarmonyLib.AccessTools;

    using System.Collections.Generic;
    using System.Reflection.Emit;
    [HarmonyPatch(typeof(Scp173PlayerScript), nameof(Scp173PlayerScript.FixedUpdate))]
    internal static class Scp173BeingLooked
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            var newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            // Search for the last "br.s".
            var index = newInstructions.FindLastIndex(instruction => instruction.opcode == OpCodes.Br_S) + 1;

            // Declare Player, to be able to store its instance with "stloc.2".
            generator.DeclareLocal(typeof(Player));

            // Get the start label and remove it.
            var startLabel = newInstructions[index].labels[0];
            newInstructions[index].labels.Clear();

            // Define the continue label and add it.
            var continueLabel = generator.DefineLabel();
            newInstructions[index].labels.Add(continueLabel);
           // OpCodes.Ldc_I4_1
            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Ldloca_S, 0),
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(List<GameObject>.Enumerator), nameof(List<GameObject>.Enumerator.Current))),
                new CodeInstruction(OpCodes.Call, Method(typeof(Player), nameof(Player.Get), new[] { typeof(GameObject) })),
                new CodeInstruction(OpCodes.Dup),
                new CodeInstruction(OpCodes.Stloc_2),
                new CodeInstruction(OpCodes.Brtrue_S, newInstructions[index - 1].operand),
                new CodeInstruction(OpCodes.Ldloc_2),
                new CodeInstruction(OpCodes.Call, Method(typeof(ExtentionMethods), nameof(ExtentionMethods.IsSCP343),new[]{typeof(Player) })),
                new CodeInstruction(OpCodes.Ceq),
                new CodeInstruction(OpCodes.Brfalse, continueLabel),
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(scp343), nameof(scp343.Instance))),
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(Config), nameof(Config.scp343_canblock173))),
                new CodeInstruction(OpCodes.Brtrue_S, newInstructions[index - 1].operand),
            });

            // Add the start label.
            newInstructions[index].WithLabels(startLabel);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}*/
