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

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PowerTray
{
    public partial class frmAbout : Form
    {
        private PowerTrayAppContext powerTrayAppContext;

        public frmAbout(PowerTrayAppContext appContext)
        {
            powerTrayAppContext = appContext;
            powerTrayAppContext.AboutOpened = true;
            InitializeComponent();
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            this.Text = Version.Name + " v" + Version.Full;
            lblName.Text = Version.Name;
            lblDescription.Text = Version.FriendlyName;
            llURL.Text = Version.PublisherURL;

            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            powerTrayAppContext.AboutOpened = false;
            Close();
        }

        private void llURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(llURL.Text);
        }

        private void llIconURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(llIconURL.Text);

        }

        private void lblName_DoubleClick(object sender, EventArgs e)
        {
            powerTrayAppContext.ExitThread();
        }

        #region Win32 Details

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
