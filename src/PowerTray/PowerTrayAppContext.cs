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

/* Icons from https://www.iconsdb.com/guacamole-green-icons/guacamole-green-battery-icons.html */

using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PowerTray
{
    public class PowerTrayAppContext : NotifyTrayAppContext, NotifyIconCallback
    {
        private PowerManager  powerManager;
        private LocalSettings Settings;

        public PowerTrayAppContext()
        {
            WindowsMobilityCentre = Path.Combine(Environment.GetEnvironmentVariable("windir"), @"System32\mblctr.exe");

            refreshTimer = new Timer(hiddenComponents);
            refreshTimer.Interval = TimerInterval;
            refreshTimer.Tick += new System.EventHandler(TimerHandler);
            refreshTimer.Enabled = true;

            powerManager = new PowerManager(this);

            SetNotifyDoubleClick(About_Click);
            SetNotifyContextMenuStrip(MainContextMenu());
            SetNotifyContextMenuStripOpening(OnContextMenuStripOpening);
            SetNotifyVisibility(true);

            Settings = new LocalSettings();
            Settings.Load(ref powerManager);

            UpdateIcon(true);
        }

        public void UpdateIcon(bool bUpdateBalloon)
        {
            PowerStatus pwrStatus    = SystemInformation.PowerStatus;
            PowerPlan   currentPlan  = powerManager.GetCurrentPlan();
            bool        isCharging   = powerManager.IsCharging();
            int         percentValue = powerManager.GetChargeValue();
            Icon        icon;

            // Change the icon if the system doesn't have a battery.
            //
            if (pwrStatus.BatteryChargeStatus == BatteryChargeStatus.NoSystemBattery)
            {
                icon = Properties.Resources.plug_multi_size;
                SetNotifyIcon(icon);
                SetNotifyText("Running on AC\n- " + currentPlan.Name, bUpdateBalloon);
                return;
            }

            // Work out % charge on battery
            //
            if (percentValue >= 99)
            {
                icon = Properties.Resources.white_full_battery;
            }
            else if (percentValue >= 95)
            {
                icon = isCharging ? Properties.Resources.green_almost_full : Properties.Resources.white_almost_full;
            }
            else if (percentValue >= 75)
            {
                icon = isCharging ? Properties.Resources.green_75_percent : Properties.Resources.white_75_percent;
            }
            else if (percentValue >= 50)
            {
                icon = isCharging ? Properties.Resources.green_50_percent : Properties.Resources.white_50_percent;
            }
            else if (percentValue >= 25)
            {
                icon = isCharging ? Properties.Resources.green_25_percent : Properties.Resources.red_25_percent;
            }
            else if (percentValue >= 5)
            {
                icon = isCharging ? Properties.Resources.green_almost_empty : Properties.Resources.red_almost_empty;
            }
            else
            {
                icon = isCharging ? Properties.Resources.green_empty_battery : Properties.Resources.red_empty_battery;
            }

            SetNotifyIcon(icon);
            SetNotifyText((isCharging ? (percentValue == 100 ? "Fully Charged " : "Charging ") : "") +
                            "(" + percentValue + "%)\n- " + currentPlan.Name, bUpdateBalloon);
        }
   
        private ContextMenuStrip MainContextMenu()
        {
            ContextMenuStrip ContextMenuApp = new ContextMenuStrip();
            PowerPlan        currentPlan    = powerManager.GetCurrentPlan();
            PowerStatus      pwrStatus      = SystemInformation.PowerStatus;

            foreach (PowerPlan p in powerManager.GetPlans())
            {

                ToolStripMenuItem item = new ToolStripMenuItem(p.Name);
                PowerPlan pp = p;
                item.Click += delegate (object sender, EventArgs args)
                {
                    powerManager.SetActive(pp);
                    Settings.Save(pp);
                };
                item.Checked = (currentPlan == p);
                ContextMenuApp.Items.Add(item);
            }
            ContextMenuApp.Items.Add("-");
            ContextMenuApp.Items.Add("Power Options", null, new EventHandler(PowerOptions_Click));

            if (pwrStatus.BatteryChargeStatus != BatteryChargeStatus.NoSystemBattery)
            {
                if (File.Exists(WindowsMobilityCentre))
                {
                    ContextMenuApp.Items.Add("Windows Mobility Centre", null, new EventHandler(WindowsMobilityCentre_Click));
                }
                ContextMenuApp.Items.Add("Battery Settings", null, new EventHandler(BatterySettings_Click));
            }

            return ContextMenuApp;
        }

        #region Message handlers

        private void About_Click(object sender, EventArgs e)
        {
            if (AboutOpened == false)
            {
                frmAbout AboutBox = new frmAbout(this);
                AboutBox.Show();
            }
        }

        /// <summary>
        /// Opens Battery Settings section of the Windows 10 Settings.
        /// </summary>
        private void BatterySettings_Click(object sender, EventArgs e)
        {
            using (Process batterySettings = new Process())
            {
                batterySettings.StartInfo.FileName = "ms-settings:batterysaver-settings";
                batterySettings.StartInfo.UseShellExecute = true;
                batterySettings.Start();
            }
        }

        /// <summary>
        /// Opens Power Options section of the Control Panel.
        /// </summary>
        private void PowerOptions_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Environment.GetEnvironmentVariable("SystemRoot") + "\\system32\\control.exe", "/name Microsoft.PowerOptions");
            }
            catch { }
        }

        private void WindowsMobilityCentre_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(WindowsMobilityCentre);
            }
            catch { }
        }

        /// <summary>
        /// Handle checkboxes on context menu
        /// </summary>
        private void OnContextMenuStripOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int idx = (powerManager.GetPlans().IndexOf(powerManager.GetCurrentPlan()));

            foreach (ToolStripItem item in GetNotifyIcon().ContextMenuStrip.Items)
            {
                if (item is ToolStripMenuItem)
                {
                    ((ToolStripMenuItem)item).Checked = false;
                }
            }

            ((ToolStripMenuItem)GetNotifyIcon().ContextMenuStrip.Items[idx]).Checked = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (hiddenComponents != null))
            {
                hiddenComponents.Dispose();
            }
        }

        protected override void ExitThreadCore()
        {
            // We must manually tidy up and remove the icon before we exit.
            // Otherwise it will be left behind until the user mouses over.
            SetNotifyVisibility(false);

            base.ExitThreadCore();
        }

        private void TimerHandler(object sender, EventArgs e)
        {
            UpdateIcon(false);
        }

        #endregion

        #region Internal variables

        /// <summary>
        /// The list of hidden components to be disposed on context disposal.
        /// </summary>
        private IContainer hiddenComponents = new Container();

        /// <summary>
        /// 
        /// </summary>
        private Timer refreshTimer;

        /// <summary>
        /// Power state update interval, milliseconds.
        /// </summary>
        private const int TimerInterval = 5 * 1000;

        /// <summary>
        /// Used to prevent multiple "About" dialogs from opening.
        /// </summary>
        internal bool AboutOpened = false;

        /// <summary>
        /// Windows Mobility EXE Path
        /// </summary>
        public string WindowsMobilityCentre { get; private set; }

        #endregion
    }
}
