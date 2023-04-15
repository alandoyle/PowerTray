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
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Windows.Forms;

namespace PowerTray
{
    internal class PowerPlans
    {
        public PowerPlans(bool fIsQuiet, bool fFixOnly)
        {
            IsAdministrator();
            isQuiet       = fIsQuiet;
            isFixOnly     = fFixOnly;
            messages      = new List<String>();
            errormessages = new List<String>();
        }

        /// <summary>
        /// Fix the Power Plans
        /// </summary>
        /// <returns></returns>
        internal void Fix()
        {
            if (isAdmin == false) 
            {
                if (isQuiet == true) { return; }
                MessageBox.Show(
                    "Fixing Power Plans can only be performed by an Administrator.",
                    $"{Version.Name} v{Version.Short} build {Version.Build}",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AddPowerPlan("Power saver",
                         "a1841308-3541-4fab-bc81-f71556f20b4a");
            AddPowerPlan("Balanced",
                         "381b4222-f694-41f0-9685-ff5bb260df2e");
            AddPowerPlan("High performance",
                         "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c");
            AddPowerPlan("Ultimate Performance",
                         "e9a42b02-d5df-448d-aa00-03f14749eb61");

            if (errormessages.Count > 0 && !isQuiet)
            {
                string strMessage = String.Concat(errormessages);
                foreach(String message in errormessages)
                {
                    strMessage.Concat(message);
                }
                MessageBox.Show($"Power Plan fixup failed.\n{strMessage}",
                    $"{Version.Name} v{Version.Short} build {Version.Build}",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);                
            }
            if (messages.Count > 0 && !isQuiet)
            {
                string strMessage = String.Concat(messages);
                MessageBox.Show($"Power Plan fixup complete.\n{strMessage}",
                    $"{Version.Name} v{Version.Short} build {Version.Build}",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);                
            }
        }

        #region Internal Variables

        private readonly bool          isQuiet;
        private readonly bool          isFixOnly;
        private          bool          isAdmin;
        private readonly List<String>  messages;
        private readonly List<String>  errormessages;

        #endregion

        #region Internal Methods

        private void IsAdministrator()
        {
            var claims = new WindowsPrincipal(WindowsIdentity.GetCurrent()).Claims;
            var adminClaimID = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null).Value;
            isAdmin = claims.Any(c => c.Value == adminClaimID);
        }

        private bool FindPlan(string strPlanName)
        {
            PowerManager powerManager = new PowerManager(null, false);
            foreach (PowerPlan p in powerManager.GetPlans())
            {
                if (p.Name == strPlanName) { return (true); }
            }
            return (false);
        }

        private bool AddPowerPlan(string strPowerPlan, string strGuid)
        {
            bool fRetVal = true;
            try
            {
                // Check if we need to add the Plan
                //
                if (FindPlan(strPowerPlan) == true)
                {
                    if (isQuiet) { return (fRetVal); }
                    if (isFixOnly) { messages.Add($"- '{strPowerPlan}' Power Plan already exists.\n"); }
                    return (fRetVal);
                }

                // Use ProcessStartInfo class
                //
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    CreateNoWindow  = true,
                    UseShellExecute = false,
                    WindowStyle     = ProcessWindowStyle.Hidden,
                    FileName        = "powercfg.exe",
                    Arguments       = $"-duplicatescheme {strGuid}"
                };

                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                //
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }

                // Check it succeeded
                //
                if (FindPlan(strPowerPlan))
                {
                    if (isQuiet) { return (fRetVal); }
                    messages.Add($"- '{strPowerPlan}' Power Plan successfully created.\n");
                }
                else
                {
                    errormessages.Add($"- Failed creating '{strPowerPlan}' Power Plan.\n");
                    fRetVal = false;
                }
            }
            catch (Exception ex)
            {
                // Show exception details.
                //
                errormessages.Add($"- Exception creating '{strPowerPlan}' Power Plan. [{ex.Message}]\n");
                fRetVal = false;
            }
            
            return (fRetVal);
        }

        #endregion
    }
}
