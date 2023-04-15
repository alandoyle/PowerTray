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

using System.Drawing;

namespace PowerTray
{
    internal class IconList
    {
        int  _percent;
        Icon _charging;
        Icon _darkmode;
        Icon _lightmode;

        public int Percent    { get { return _percent; } }
        public Icon Charging  { get { return _charging; } }
        public Icon DarkMode  { get { return _darkmode; } }
        public Icon LightMode { get { return _lightmode; } }

        internal IconList(int percent, Icon darkmode, Icon lightmode, Icon charging)
        {
            _percent   = percent;
            _charging  = charging;
            _darkmode  = darkmode;
            _lightmode = lightmode;
        }
    }
}
