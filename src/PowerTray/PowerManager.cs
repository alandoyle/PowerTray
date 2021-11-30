﻿/* 
 * This file is part of PowerTray <https://github.com/alandoyle/PowerTray>
 * Copyright (c) 2020-2021 Alan Doyle.
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
//@@@ https://www.top-password.com/blog/fix-battery-icon-missing-from-windows-10-taskbar/
using Microsoft.Win32;
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
        NotifyIconCallback notifyIconCallback;
        List<PowerPlan>    plans;

        public enum AccessFlags : uint
        {
            ACCESS_SCHEME = 16,
            ACCESS_SUBGROUP = 17,
            ACCESS_INDIVIDUAL_SETTING = 18
        }

        public PowerManager(NotifyIconCallback notifyCallback)
        {
            notifyIconCallback = notifyCallback;

            plans = new List<PowerPlan>();
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

            return friendlyName;
        }

        public int GetChargeValue()
        {
            PowerStatus pwrStatus = SystemInformation.PowerStatus;
            return (int)(pwrStatus.BatteryLifePercent * 100);
        }

        public bool IsCharging()
        {
            PowerStatus pwrStatus = SystemInformation.PowerStatus;
            return pwrStatus.PowerLineStatus == PowerLineStatus.Online;
        }

        public PowerPlan GetCurrentPlan()
        {
            return GetPlans().Find(p => (p.Guid == GetActiveGuid()));
        }

        public Guid GetActiveGuid()
        {
            Guid ActiveScheme = Guid.Empty;
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));

            if (PowerGetActiveScheme((IntPtr)null, out ptr) == 0)
            {
                ActiveScheme = (Guid)Marshal.PtrToStructure(ptr, typeof(Guid));
                if (ptr != null)
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
            return ActiveScheme;
        }

        public void SetActive(PowerPlan plan)
        {
            PowerSetActiveScheme(IntPtr.Zero, ref plan.Guid);
            notifyIconCallback.UpdateIcon();

            RegistryKey key;
            key = Registry.CurrentUser.CreateSubKey(@"Software\PowerTray");
            key.SetValue("PowerPlan", plan.Name);
            key.SetValue("GUID", plan.Guid);
            key.Close();
        }

        public IEnumerable<Guid> GetAll()
        {
            var schemeGuid = Guid.Empty;

            uint sizeSchemeGuid = (uint)Marshal.SizeOf(typeof(Guid));
            uint schemeIndex = 0;

            while (PowerEnumerate(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, (uint)AccessFlags.ACCESS_SCHEME, schemeIndex, ref schemeGuid, ref sizeSchemeGuid) == 0)
            {
                yield return schemeGuid;
                schemeIndex++;
            }
        }

        public List<string> GetAllNames()
        {
            List<string> PowerPlansNames = new List<string>();
            var guidPlans = GetAll();

            foreach (Guid guidPlan in guidPlans)
            {
                PowerPlansNames.Add(ReadFriendlyName(guidPlan));
            }

            PowerPlansNames.Sort();

            return (PowerPlansNames);
        }

        public List<PowerPlan> GetPlans()
        {
            return (plans);
        }

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