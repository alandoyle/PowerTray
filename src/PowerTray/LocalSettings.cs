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

using Microsoft.Win32;
using System;

namespace PowerTray
{
    public class LocalSettings
    {
        public LocalSettings()
        {
            try
            {
                using (RegistryKey keyHKCU = Registry.CurrentUser.OpenSubKey(@"Software\PowerTray", true))
                {
                    keyHKCU.DeleteValue("PowerPlan");
                    keyHKCU.DeleteValue("GUID");
                    keyHKCU.Close();
                }
            }
            catch { }
        }

        public void LoadAC(ref PowerManager powerManager)
        {
            try
            {
                using (RegistryKey keyHKCU = Registry.CurrentUser.OpenSubKey(@"Software\PowerTray\AC"))
                {
                    string strGUID = (String)keyHKCU.GetValue("GUID");
                    if (strGUID.Length > 0)
                    {
                        PowerPlan plan = powerManager.GetPlans().Find(p => (p.Guid == Guid.Parse(strGUID)));
                        powerManager.SetACPlan(plan);
                    }
                    keyHKCU.Close();
                }
            }
            catch
            {
                powerManager.SetACPlan(powerManager.GetCurrentPlan());
            }
        }

        public void LoadBattery(ref PowerManager powerManager)
        {
            try
            {
                using (RegistryKey keyHKCU = Registry.CurrentUser.OpenSubKey(@"Software\PowerTray\Battery"))
                {
                    string strGUID = (String)keyHKCU.GetValue("GUID");
                    if (strGUID.Length > 0)
                    {
                        PowerPlan plan = powerManager.GetPlans().Find(p => (p.Guid == Guid.Parse(strGUID)));
                        powerManager.SetBatteryPlan(plan);
                    }
                    keyHKCU.Close();
                }
            }
            catch
            {
                powerManager.SetBatteryPlan(powerManager.GetPlanByName("Power saver"));
                SaveBattery(powerManager.GetBatteryPlan());
            }
        }

        public bool ShowBalloonTips()
        {
            bool fShowBalloonTips = false;
            try
            {
                using (RegistryKey keyHKCU = Registry.CurrentUser.OpenSubKey(@"Software\PowerTray", true))
                {
                    string strShowBalloonTips = (String)keyHKCU.GetValue("ShowBalloonTips");
                    if (strShowBalloonTips.ToLower().CompareTo("true") == 0) { fShowBalloonTips = true; }
                    if (strShowBalloonTips.ToLower().CompareTo("yes") == 0)  { fShowBalloonTips = true; }
                    if (strShowBalloonTips.ToLower().CompareTo("1") == 0)    { fShowBalloonTips = true; }
                }
            }
            catch
            {
                SaveBalloonTips(fShowBalloonTips);
            }
            return (fShowBalloonTips);
        }


        public bool EnableAutoSwitch()
        {
            bool fAutoSwitch = true;
            try
            {
                using (RegistryKey keyHKCU = Registry.CurrentUser.OpenSubKey(@"Software\PowerTray", true))
                {
                    string strAutoSwitch = (String)keyHKCU.GetValue("AutoSwitch");
                    if (strAutoSwitch.ToLower().CompareTo("false") == 0) { fAutoSwitch = false; }
                    if (strAutoSwitch.ToLower().CompareTo("no") == 0)    { fAutoSwitch = false; }
                    if (strAutoSwitch.ToLower().CompareTo("0") == 0)     { fAutoSwitch = false; }
                }
            }
            catch
            {
                SaveAutoSwitch(fAutoSwitch);
            }
            return (fAutoSwitch);
        }

        public void SaveAC(PowerPlan plan)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\PowerTray\AC", true))
                {
                    key.SetValue("PowerPlan", plan.Name);
                    key.SetValue("GUID",      plan.Guid);
                    key.Close();
                }
            }
            catch { }
        }

        public void SaveBattery(PowerPlan plan)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\PowerTray\Battery", true))
                {
                    key.SetValue("PowerPlan", plan.Name);
                    key.SetValue("GUID",      plan.Guid);
                    key.Close();
                }
            }
            catch { }
        }

        internal void SaveBalloonTips(bool showBalloonTips)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\PowerTray", true))
                {
                    key.SetValue("ShowBalloonTips", showBalloonTips ? "True" : "False");
                    key.Close();
                }
            }
            catch { }
        }

        internal void SaveAutoSwitch(bool autoSwitch)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\PowerTray", true))
                {
                    key.SetValue("AutoSwitch", autoSwitch ? "True" : "False");
                    key.Close();
                }
            }
            catch { }
        }
    }
}
