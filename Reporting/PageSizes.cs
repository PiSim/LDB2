using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Reporting
{
    public static class PageSizes
    {
        public static Size A4 => new Size(8.27 * 96.0, 11.69 * 96.0);
        public static Size A4Landscape => new Size(11.69 * 96.0, 8.27 * 96.0);
    }
}
