/* 
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

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTray
{
    public class LocalSettings
    {
        public LocalSettings() { }

        public void Load(ref PowerManager powerManager)
        {
            try
            {
                RegistryKey keyHKCU;
                keyHKCU = Registry.CurrentUser.OpenSubKey(@"Software\PowerTray");
                if (keyHKCU != null)
                {
                    string strGUID = (String)keyHKCU.GetValue("GUID");
                    if (strGUID.Length > 0)
                    {
                        PowerPlan plan = powerManager.GetPlans().Find(p => (p.Guid == Guid.Parse(strGUID)));
                        powerManager.SetActive(plan);
                    }
                }
                keyHKCU.Close();
            }
            catch { }
        }

        public void Save(PowerPlan plan)
        {
            try
            {
                RegistryKey key;
                key = Registry.CurrentUser.CreateSubKey(@"Software\PowerTray");
                key.SetValue("PowerPlan", plan.Name);
                key.SetValue("GUID", plan.Guid);
                key.Close();
            }
            catch { }
        }
    }
}
