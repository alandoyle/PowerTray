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

/* Icons from https://www.iconsdb.com/ */

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PowerTray
{
    /// <summary>
    /// Main class implementing the Application specific System Tray icon and contex menu.
    /// </summary>
    public class PowerTrayAppContext : NotifyTrayAppContext, INotifyIconCallback
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PowerTrayAppContext()
        {
            isDarkMode = ShouldSystemUseDarkMode();

            refreshTimer = new Timer(hiddenComponents)
            {
                Interval = TimerInterval
            };
            refreshTimer.Tick    += new System.EventHandler(TimerHandler);
            refreshTimer.Enabled  = true;

            Settings        = new LocalSettings();
            showBalloonTips = Settings.ShowBalloonTips();
            autoSwitch      = Settings.EnableAutoSwitch();

            powerManager = new PowerManager(this, showBalloonTips);

            Settings.LoadAC(ref powerManager);
            Settings.LoadBattery(ref powerManager);

            SetNotifyDoubleClick(About_Click);
            SetNotifyContextMenuStrip(new ContextMenuStrip());
            SetNotifyContextMenuStripOpening(OnContextMenuStripOpening);
            SetNotifyVisibility(true);
            GenerateContextMenu();

            // Set initial power plan
            //
            if (autoSwitch)
            { 
                powerManager.SetActive(powerManager.IsOnBattery() ?
                                       powerManager.GetBatteryPlan() :
                                       powerManager.GetACPlan());
            }

            // Add System event handler
            //
            SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
        }

        /// <summary>
        /// Update the System tray icon.
        /// </summary>
        public void UpdateIcon(bool bUpdateBalloon)
        {
            PowerStatus pwrStatus    = SystemInformation.PowerStatus;
            PowerPlan   currentPlan  = powerManager.GetCurrentPlan();
            bool        isCharging   = powerManager.IsCharging();
            int         percentValue = powerManager.GetChargeValue();
            string      iconText     = String.Empty;
            Icon        icon         = (isDarkMode ? Properties.Resources.white_plug :
                                                     Properties.Resources.black_plug);

            // Change the icon if the system doesn't have a battery.
            //
            if (pwrStatus.BatteryChargeStatus == BatteryChargeStatus.NoSystemBattery)
            {
                iconText = $"Running on AC\n- {currentPlan.Name}";
            }
            else
            {
                // Work out icon for mobile device with battery.
                //
                IconList[] iconList = new IconList[] { 
	                new IconList(99, Properties.Resources.white_full_battery, Properties.Resources.black_full_battery, null),
	                new IconList(95, Properties.Resources.white_almost_full,  Properties.Resources.black_almost_full,  Properties.Resources.green_almost_full ),
	                new IconList(75, Properties.Resources.white_75_percent,   Properties.Resources.black_75_percent,   Properties.Resources.green_75_percent ),
	                new IconList(50, Properties.Resources.white_50_percent,   Properties.Resources.black_50_percent,   Properties.Resources.green_50_percent ),
	                new IconList(25, Properties.Resources.red_25_percent,     Properties.Resources.red_25_percent,     Properties.Resources.green_25_percent ),
	                new IconList( 5, Properties.Resources.red_almost_empty,   Properties.Resources.red_almost_empty,   Properties.Resources.green_almost_empty ),
	                new IconList( 0, Properties.Resources.red_empty_battery,  Properties.Resources.red_empty_battery,  Properties.Resources.green_empty_battery ),
                };

                foreach (IconList item in iconList)
                {
                    if (percentValue < item.Percent) { continue; }
                    if (isCharging && item.Charging != null) { icon = item.Charging; break; }
                    else { icon = isDarkMode ? item.DarkMode : item.LightMode; break; }
                }
                iconText = String.Format((isCharging ? 
                    (percentValue == 100 ? "Fully Charged " : "Charging ") : "") +
                    "(" + percentValue + "%)\n- " + currentPlan.Name);
            }

            // Set the icon and the text.
            //
            SetNotifyIcon(icon);
            SetNotifyText(iconText, bUpdateBalloon);
        }
   
        /// <summary>
        /// Generate the main context menu.
        /// </summary>
        private void GenerateContextMenu()
        {
            try
            {
                ContextMenuStrip ContextMenuApp = GetNotifyIcon().ContextMenuStrip;
                PowerStatus      pwrStatus      = SystemInformation.PowerStatus;

                // Clear out old context menu
                //
                ContextMenuApp.Items.Clear();
                powerManager.GeneratePowerPlanList();

                PowerPlan currentPlan = powerManager.GetCurrentPlan();
                PowerPlan acPlan      = powerManager.GetACPlan();
                PowerPlan batteryPlan = powerManager.GetBatteryPlan();

                // Add About... menu item
                //
                ContextMenuApp.Items.Add($"{Version.Name} v{Version.Short} build {Version.Build}",
                                         null, new EventHandler(About_Click));
                ContextMenuApp.Items.Add("-");

                // Add Power plans to context menu
                //
                foreach (PowerPlan p in powerManager.GetPlans())
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(p.Name);
                    PowerPlan pp = p;
                    item.Click += delegate (object sender, EventArgs args)
                    {
                        powerManager.SetActive(pp);
                    };
                    item.Checked = (currentPlan.Guid == p.Guid);
                    ContextMenuApp.Items.Add(item);
                }
                ContextMenuApp.Items.Add("-");

                // Add Power Control Panel item
                //
                ContextMenuApp.Items.Add("Power Options...", null, new EventHandler(PowerOptions_Click));

                // Add battery options
                //
                if (pwrStatus.BatteryChargeStatus != BatteryChargeStatus.NoSystemBattery)
                {
                    WindowsMobilityCentre = Path.Combine(
                        Environment.GetEnvironmentVariable("windir"),
                        @"System32\mblctr.exe");
                    if (File.Exists(WindowsMobilityCentre))
                    {
                        ContextMenuApp.Items.Add("Windows Mobility Centre...",
                            null, new EventHandler(WindowsMobilityCentre_Click));
                    }
                    ContextMenuApp.Items.Add("Battery Settings...",
                        null, new EventHandler(BatterySettings_Click));
                }
                ContextMenuApp.Items.Add("-");
                
                // Add Configuration menu
                //
                ToolStripMenuItem configmenu = new ToolStripMenuItem("Configuration");
                ToolStripMenuItem submenu = new ToolStripMenuItem("Default AC Power Plan");
                foreach (PowerPlan p in powerManager.GetPlans())
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(p.Name);
                    PowerPlan pp = p;
                    item.Click += delegate (object sender, EventArgs args)
                    {
                        Settings.SaveAC(pp);
                        powerManager.SetACPlan(pp);
                    };
                    item.Checked = (acPlan.Guid == p.Guid);
                    submenu.DropDownItems.Add(item);
                }
                configmenu.DropDownItems.Add(submenu);

                if (pwrStatus.BatteryChargeStatus != BatteryChargeStatus.NoSystemBattery)
                {
                    submenu = new ToolStripMenuItem("Default Battery Power Plan");
                    foreach (PowerPlan p in powerManager.GetPlans())
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem(p.Name);
                        PowerPlan pp = p;
                        item.Click += delegate (object sender, EventArgs args)
                        {
                            Settings.SaveBattery(pp);
                            powerManager.SetBatteryPlan(pp);
                        };
                        item.Checked = (batteryPlan.Guid == p.Guid);
                        submenu.DropDownItems.Add(item);
                    }
                    configmenu.DropDownItems.Add(submenu);
                }
                configmenu.DropDownItems.Add(new ToolStripSeparator());
                
                // Auto switch plans menu item
                //
                submenu = new ToolStripMenuItem("Auto-Switch Plans");
                submenu.Click += delegate (object sender, EventArgs args)
                {
                    autoSwitch = !autoSwitch;
                    Settings.SaveAutoSwitch(autoSwitch);
                    // Set default power plan (if Auto-Switch is enabled)
                    //
                    if (autoSwitch)
                    { 
                        powerManager.SetActive(powerManager.IsOnBattery() ?
                                               powerManager.GetBatteryPlan() :
                                               powerManager.GetACPlan());
                    }
                };
                submenu.Checked = autoSwitch;
                configmenu.DropDownItems.Add(submenu);

                // Show Balloon Tips menu item
                //
                submenu = new ToolStripMenuItem("Show Balloon Tips");
                submenu.Click += delegate (object sender, EventArgs args)
                {
                    showBalloonTips = !showBalloonTips;
                    Settings.SaveBalloonTips(showBalloonTips);
                    powerManager.SetBalloonTips(showBalloonTips);
                };
                submenu.Checked = showBalloonTips;
                configmenu.DropDownItems.Add(submenu);
                configmenu.DropDownItems.Add(new ToolStripSeparator());
         
                // Show About... menu item
                //
                submenu = new ToolStripMenuItem("About...");
                submenu.Click += About_Click;
                configmenu.DropDownItems.Add(submenu);      

                // Attach to main context menu
                //
                ContextMenuApp.Items.Add(configmenu);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception creating context menu. [{ex.Message}]",
                     $"{Version.Name} v{Version.Short} build {Version.Build}",
                     MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Message Handlers

        /// <summary>
        /// Power state handler for devices with batteries.
        /// </summary>
        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            // Check if Auto-switching is enabled.
            //
            if (!autoSwitch) {  return; }

            //Get the current Status
            //
            switch (e.Mode)
            {
                case PowerModes.StatusChange:
                    powerManager.SetActive(
                        powerManager.IsOnBattery() ?
                        powerManager.GetBatteryPlan() :
                        powerManager.GetACPlan());
                    break;
            }
        }

        /// <summary>
        /// "About..." menu item handler.
        /// </summary>
        private void About_Click(object sender, EventArgs e)
        {
            if (AboutOpened == false)
            {
                FrmAbout AboutBox = new FrmAbout(this);
                AboutBox.Show();
            }
        }

        /// <summary>
        /// Opens Battery Settings section of the Windows 10/11 Settings.
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

        /// <summary>
        /// Opens the "Windows Mobility Centre" on mobile devices.
        /// </summary>
        private void WindowsMobilityCentre_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(WindowsMobilityCentre);
            }
            catch { }
        }

        /// <summary>
        /// Cleanup on shutdown.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (!disposing || hiddenComponents == null)
            {
                return;
            }
            hiddenComponents.Dispose();
        }

        /// <summary>
        /// Remove the System Tray icon and exit the application.
        /// </summary>
        protected override void ExitThreadCore()
        {
            // We must manually tidy up and remove the icon before we exit.
            // Otherwise it will be left behind until the user mouses over.
            SetNotifyVisibility(false);

            base.ExitThreadCore();
        }

        /// <summary>
        /// Handle checkboxes on context menu
        /// </summary>
        private void OnContextMenuStripOpening(object sender, System.ComponentModel.CancelEventArgs e) => GenerateContextMenu();

        /// <summary>
        /// Icon update Timer.
        /// </summary>
        private void TimerHandler(object sender, EventArgs e) => UpdateIcon(false);

        #endregion

        #region Internal variables

        private readonly PowerManager  powerManager;
        private readonly LocalSettings Settings;
        private readonly bool          isDarkMode;
        private bool                   showBalloonTips;
        private bool                   autoSwitch;
        private readonly IContainer    hiddenComponents = new Container();
        private readonly Timer         refreshTimer;
        private const int              TimerInterval = 5 * 1000;
        public bool                    AboutOpened = false;

        public string WindowsMobilityCentre { get; private set; }

        #endregion

        #region Win32 API

        /// <summary>
        /// Win32 API to detect Light/Dark Mode. Only available in Windows 10
        /// v1909 or higher.
        /// </summary>
        [DllImport("UXTheme.dll", SetLastError = true, EntryPoint = "#138")] 
        public static extern bool ShouldSystemUseDarkMode();

        #endregion
    }
}
