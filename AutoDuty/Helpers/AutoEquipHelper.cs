using ECommons.DalamudServices;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;

namespace AutoDuty.Helpers
{
    internal unsafe class AutoEquipHelper
    {
        internal static bool AutoEquipRunning = false;

        internal static void Invoke()
        {
            return;
            if (!AutoEquipRunning)
            {
                Svc.Log.Info("AutoEquip - Started");
                AutoEquipRunning = true;
                AutoDuty.Plugin.States |= State.Other;
                RecommendEquipModule.Instance()->SetupForClassJob((byte)Svc.ClientState.LocalPlayer!.ClassJob.Id);
                SchedulerHelper.ScheduleAction("AutoEquip_EquipRecommendedGear", () => RecommendEquipModule.Instance()->EquipRecommendedGear(), () => !RecommendEquipModule.Instance()->IsUpdating);
                Svc.Log.Info($"AutoEquip - Equipped Recommended Gear");
                SchedulerHelper.ScheduleAction("AutoEquip_UpdateGearset", () => RaptureGearsetModule.Instance()->UpdateGearset(RaptureGearsetModule.Instance()->CurrentGearsetIndex), 500);
                SchedulerHelper.ScheduleAction("AutoEquip_FinishedLog", () => Svc.Log.Info($"AutoEquip - Finished"), 1000);
                SchedulerHelper.ScheduleAction("AutoEquip_SetRunningFalse", () => AutoEquipRunning = false, 1000);
                SchedulerHelper.ScheduleAction("AutoEquip_SetPreviousStage", () => AutoDuty.Plugin.States &= ~State.Other, 1000);
                SchedulerHelper.ScheduleAction("AutoEquip_SetActionBlank", () => AutoDuty.Plugin.Action = "", 1000);
            }
        }
    }
}
