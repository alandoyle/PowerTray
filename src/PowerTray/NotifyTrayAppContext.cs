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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PowerTray
{
    public interface INotifyIconCallback
    {
        void UpdateIcon(bool bUpdateBalloon);
    }

    public class NotifyTrayAppContext : ApplicationContext
    {
         /// <summary>
        /// Sets the tooltip for the SysTray icon
        /// </summary>
        public void SetNotifyText(string text, bool bUpdateBalloon)
        {
            notifyIcon.Text    = text;
            notifyIcon.Visible = true;

            if (bUpdateBalloon)
            {
                notifyIcon.ShowBalloonTip(30000, Version.FriendlyName, text, ToolTipIcon.Info);
            }
        }

        /// <summary>
        /// Returns pointer to SysTray notification icon.
        /// </summary>
        public NotifyIcon GetNotifyIcon() => (notifyIcon);

        /// <summary>
        /// Sets the icon to use in the SysTray
        /// </summary>
        public void SetNotifyIcon(Icon icon) => notifyIcon.Icon = icon;

        /// <summary>
        /// Sets the Right Click menu for the SysTray icon.
        /// </summary>
        public void SetNotifyContextMenuStrip(ContextMenuStrip contextMenuStrip) => notifyIcon.ContextMenuStrip = contextMenuStrip;

        /// <summary>
        /// Set SysTray icon visibility.
        /// </summary>
        public void SetNotifyVisibility(bool visibility) => notifyIcon.Visible = visibility;

        /// <summary>
        /// Set the double-click event handler
        /// </summary>
        public void SetNotifyDoubleClick(Action<object, EventArgs> doubleclickEvent) => notifyIcon.DoubleClick += new EventHandler(doubleclickEvent);

        /// <summary>
        /// Set the MouseUp event handler for the SysTray icon
        /// </summary>
        public void SetNotifyMouseUp(MouseEventHandler onNotifyIconMouseUp) => notifyIcon.MouseUp += onNotifyIconMouseUp;

        /// <summary>
        /// Set the ContextMenuStripOpening event handler.
        /// </summary>
        public void SetNotifyContextMenuStripOpening(CancelEventHandler onContextMenuStripOpening) => notifyIcon.ContextMenuStrip.Opening += onContextMenuStripOpening;

        #region Internal Variables

        private readonly NotifyIcon notifyIcon = new NotifyIcon();

        #endregion
    }
}

