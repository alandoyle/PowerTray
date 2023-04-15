/* 
 * This file is part of PowerTray <https://github.com/alandoyle/PowerTray>
 * Copyright (c) 2020-2023 Alan Doyle.
 * 
 * This program is free software: you can redistribute it and/or modify  
 * it under the terms of the GNU General Public License as published by  
 * the Free Software Foundation, version 3.
 *
 * This program is distributed in the hope that it will be useful, but 
 * WITHOUT ANY WARRANTY; without even the implied warranty of 
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU 
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License 
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PowerTray
{
    public class PowerPlan
    {
        public readonly string Name;
        public Guid Guid;

        public PowerPlan(string name, Guid guid)
        {
            Name = name;
            Guid = guid;
        }
    }

    public class PowerManager
    {
        public PowerManager(INotifyIconCallback notifyCallback, bool fShowBalloonTips)
        {
            notifyIconCallback = notifyCallback;
            plans = new List<PowerPlan>();
            showBalloonTips = fShowBalloonTips;
            GeneratePowerPlanList();
        }

        public PowerPlan GetPlanByName(string strPlanName)
        {
             foreach (PowerPlan p in GetPlans())
            {
                if (p.Name == strPlanName) { return (p); }
            }
            return (GetPlanByName("Balanced"));
        }

        public Guid GetActiveGuid()
        {
            Guid ActiveScheme = Guid.Empty;
            if (PowerGetActiveScheme((IntPtr)null, out IntPtr ptr) == 0)
            {
                ActiveScheme = (Guid)Marshal.PtrToStructure(ptr, typeof(Guid));
                if (ptr != null)
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
            return (ActiveScheme);
        }

        public void GeneratePowerPlanList()
        {
            plans.Clear();
            foreach (Guid guidPlan in GetAll())
            {
                PowerPlan plan = new PowerPlan(ReadFriendlyName(guidPlan), guidPlan);
                plans.Add(plan);
            }
            plans = plans.OrderBy(i => i.Name).ToList();
        }

        public string ReadFriendlyName(Guid schemeGuid)
        {
            uint sizeName = 1024;
            IntPtr pSizeName = Marshal.AllocHGlobal((int)sizeName);

            string friendlyName;
            try
            {
                PowerReadFriendlyName(IntPtr.Zero, ref schemeGuid, IntPtr.Zero, IntPtr.Zero, pSizeName, ref sizeName);
                friendlyName = Marshal.PtrToStringUni(pSizeName);
            }
            finally
            {
                Marshal.FreeHGlobal(pSizeName);
            }
            return (friendlyName);
        }

        public int GetChargeValue()
        {
            PowerStatus pwrStatus = SystemInformation.PowerStatus;
            return (int)(pwrStatus.BatteryLifePercent * 100);
        }

        public bool IsCharging()
        {
            PowerStatus pwrStatus = SystemInformation.PowerStatus;
            return ((pwrStatus.PowerLineStatus == PowerLineStatus.Online) &&
                    (pwrStatus.BatteryChargeStatus != BatteryChargeStatus.NoSystemBattery));
        }

        internal bool IsOnBattery()
        {
            PowerStatus pwrStatus = SystemInformation.PowerStatus;
            return ((pwrStatus.PowerLineStatus != PowerLineStatus.Online) &&
                    (pwrStatus.BatteryChargeStatus != BatteryChargeStatus.NoSystemBattery));
        }

        public void SetActive(PowerPlan plan)
        {
            PowerSetActiveScheme(IntPtr.Zero, ref plan.Guid);
            notifyIconCallback.UpdateIcon(showBalloonTips);
        }

        public IEnumerable<Guid> GetAll()
        {
            Guid schemeGuid = Guid.Empty;
            uint sizeSchemeGuid = (uint)Marshal.SizeOf(typeof(Guid));
            uint schemeIndex = 0;

            while (PowerEnumerate(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 
                ACCESS_SCHEME, schemeIndex, 
                ref schemeGuid, ref sizeSchemeGuid) == 0)
            {
                yield return schemeGuid;
                schemeIndex++;
            }
        }

        public PowerPlan GetCurrentPlan() => GetPlans().Find(p => (p.Guid == GetActiveGuid()));

        public PowerPlan GetACPlan() => ACPlan;

        public PowerPlan GetBatteryPlan() => BatteryPlan;

        public void SetACPlan(PowerPlan plan) => ACPlan = plan;

        public void SetBatteryPlan(PowerPlan plan) => BatteryPlan = plan;

        public List<PowerPlan> GetPlans() => (plans);

        internal void SetBalloonTips(bool fShowBalloonTips) => showBalloonTips = fShowBalloonTips;

        #region  Internal Variables

        private readonly INotifyIconCallback notifyIconCallback;
        private bool                         showBalloonTips;
        private List<PowerPlan>              plans;
        private const uint                   ACCESS_SCHEME = 16;
        private PowerPlan                    ACPlan;
        private PowerPlan                    BatteryPlan;

        #endregion

        #region Win32 Power API

        [DllImport("PowrProf.dll")]
        public static extern UInt32 PowerEnumerate(IntPtr RootPowerKey, IntPtr SchemeGuid, IntPtr SubGroupOfPowerSettingGuid, UInt32 AcessFlags, UInt32 Index, ref Guid Buffer, ref UInt32 BufferSize);

        [DllImport("PowrProf.dll")]
        public static extern UInt32 PowerReadFriendlyName(IntPtr RootPowerKey, ref Guid SchemeGuid, IntPtr SubGroupOfPowerSettingGuid, IntPtr PowerSettingGuid, IntPtr Buffer, ref UInt32 BufferSize);

        [DllImportAttribute("powrprof.dll", EntryPoint = "PowerGetActiveScheme")]
        public static extern uint PowerGetActiveScheme(IntPtr UserPowerKey, out IntPtr ActivePolicyGuid);

        [DllImportAttribute("powrprof.dll", EntryPoint = "PowerSetActiveScheme")]
        public static extern uint PowerSetActiveScheme(IntPtr UserPowerKey, ref Guid ActivePolicyGuid);

        #endregion
    }
}
