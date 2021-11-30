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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PowerTray
{
    public interface NotifyIconCallback
    {
        void UpdateIcon();
    }

    public class NotifyTrayAppContext : ApplicationContext
    {
        private NotifyIcon notifyIcon = new NotifyIcon();

        /// <summary>
        /// Returns pointer to SysTray notification icon.
        /// </summary>
        /// <returns></returns>
        public NotifyIcon GetNotifyIcon()
        {
            return (notifyIcon);
        }

        /// <summary>
        /// Sets the icon to use in the SysTray
        /// </summary>
        /// <param name="icon"></param>
        public void SetNotifyIcon(Icon icon)
        {
            notifyIcon.Icon = icon;
        }

        /// <summary>
        /// Sets the tooltip for the SysTray icon
        /// </summary>
        /// <param name="text"></param>
        public void SetNotifyText(string text)
        {
            notifyIcon.Text = text;
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(30000, Version.FriendlyName, text, ToolTipIcon.Info);
        }

        /// <summary>
        /// Sets the Right Click menu for the SysTray icon.
        /// </summary>
        /// <param name="contextMenuStrip"></param>
        public void SetNotifyContextMenuStrip(ContextMenuStrip contextMenuStrip)
        {
            notifyIcon.ContextMenuStrip = contextMenuStrip;
        }

        /// <summary>
        /// Set SysTray icon visibility.
        /// </summary>
        /// <param name="visibility"></param>
        public void SetNotifyVisibility(bool visibility)
        {
            notifyIcon.Visible = visibility;
        }

        /// <summary>
        /// Set the double-click event handler
        /// </summary>
        /// <param name="doubleclickEvent"></param>
        public void SetNotifyDoubleClick(Action<object, EventArgs> doubleclickEvent)
        {
            notifyIcon.DoubleClick += new EventHandler(doubleclickEvent);
        }

        /// <summary>
        /// Set the MouseUp event handler for the SysTray icon
        /// </summary>
        /// <param name="onNotifyIconMouseUp"></param>
        public void SetNotifyMouseUp(MouseEventHandler onNotifyIconMouseUp)
        {
            notifyIcon.MouseUp += onNotifyIconMouseUp;
        }

        /// <summary>
        /// Set the ContextMenuStripOpening event handler.
        /// </summary>
        /// <param name="onContextMenuStripOpening"></param>
        public void SetNotifyContextMenuStripOpening(CancelEventHandler onContextMenuStripOpening)
        {
            notifyIcon.ContextMenuStrip.Opening += onContextMenuStripOpening;
        }
    }
}

