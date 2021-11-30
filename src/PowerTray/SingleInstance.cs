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
using System.Threading;

namespace PowerTray
{
    /// <summary>
    /// Utilities for restricting application to a single instance.
    /// </summary>
    public static class SingleInstance
    {
        private static Mutex mutex;

        /// <summary>
        /// Returns true if this instance is the only one running.
        /// </summary>
        public static bool Start()
        {
            bool singleInstance = false;

            string assemblyName = Version.Name;

            // Note: using local mutex, so multiple instantiations are still 
            // possible across different sessions.
            string mutexName = String.Format("Local\\{0}", assemblyName);

            mutex = new Mutex(true, mutexName, out singleInstance);
            return singleInstance;
        }

        /// <summary>
        /// Marks the current instance as not running.
        /// </summary>
        static public void Stop()
        {
            if (mutex != null)
            {
                mutex.ReleaseMutex();
            }
        }
    }
}
