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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PowerTray
{
    public partial class FrmAbout : Form
    {
        private readonly PowerTrayAppContext powerTrayAppContext;

        public FrmAbout(PowerTrayAppContext appContext)
        {
            powerTrayAppContext = appContext;
            powerTrayAppContext.AboutOpened = true;
            InitializeComponent();
        }

        private void FrmAbout_Load(object sender, EventArgs e)
        {
            this.Text              = $"{Version.Name} v{Version.Short} build {Version.Build}";
            Label_Name.Text        = Version.Name;
            Label_Description.Text = Version.FriendlyName;
            LinkLabel_URL.Text     = Version.PublisherURL;

            // Place "About..." dialog on top of all other windows
            //
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Button_Exit_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(
                "Are you sure you want to exit?",
                $"{Version.Name} v{Version.Short} build {Version.Build}",
                MessageBoxButtons.YesNo);
            if(dialogResult == DialogResult.Yes)
            {
                powerTrayAppContext.ExitThread();
            }
        }

        private void LinkLabel_URL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start(LinkLabel_URL.Text);

        private void LinkLabel_IconURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start(LinkLabel_IconURL.Text);

        private void FrmAbout_FormClosing(object sender, FormClosingEventArgs e) => powerTrayAppContext.AboutOpened = false;

        #region Win32 API

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        #endregion
    }
}
