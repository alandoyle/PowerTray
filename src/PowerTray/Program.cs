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
using System.Windows.Forms;

namespace PowerTray
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool fixonly = false;
            bool isQuiet = true;

            // Check the arguments first
            //
            if ((args.Length == 1) && (args[0].StartsWith("-fix-powerplans")))
            {
                fixonly = true;
                isQuiet = args[0].CompareTo("-fix-powerplans-quiet") == 0;
            }

            // Repair the standard power plans (if necessary)
            //
            PowerPlans powerplans = new PowerPlans(isQuiet, fixonly);    
            powerplans.Fix();

            // Check if we only want to fix the power plans.
            //
            if (fixonly) { return; }

            // Run only a single instance
            //
            if (SingleInstance.Start())
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new PowerTrayAppContext());
                }
                catch
                {
                    SingleInstance.Stop();
                }
            }
            else
            {
                MessageBox.Show("\t" + Version.Name + " is already running!\t\n",
                                $"{Version.Name} v{Version.Short} build {Version.Build}",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}


