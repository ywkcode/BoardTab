using BoardTab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardTab.Utils
{
    public static class ConfigurationCache
    {

        public static IServiceProvider RootServiceProvider { get; set; }
        public static List<TimeSetTemp>  TimeSetlist { get; set; }

        public static List<TimeSetTemp> TimeSetInCallist { get; set; }
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public static string CurrentDay { get; set; } 

        public static string NextDay { get; set; }

      

        public static DateTime FirstDay { get; set; }

        public static DateTime LastDay { get; set; }

        public static Int64 Version { get; set; }

        public static Int64 MaxVersion { get; set; }

        public static bool IsInNext { get; set; } = false;
    }
}
