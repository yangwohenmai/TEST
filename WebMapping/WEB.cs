using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WebMapping
{
    public class WEB
    {
        [DllImport("mpr.dll", EntryPoint = "WNetAddConnection2")]
        public static extern uint WNetAddConnection2(
            [In] NETRESOURCE lpNetResource,
            string lpPassword,
            string lpUsername,
            uint dwFlags);

        [DllImport("Mpr.dll")]
        public static extern uint WNetCancelConnection2(
            string lpName,
            uint dwFlags,
            bool fForce);

        [StructLayout(LayoutKind.Sequential)]
        public class NETRESOURCE
        {
            public int dwScope;
            public int dwType;
            public int dwDisplayType;
            public int dwUsage;
            public string LocalName;
            public string RemoteName;
            public string Comment;
            public string Provider;
        }

        // remoteNetworkPath format:  @"\\192.168.1.48\sharefolder"
        // localDriveName format:     @"E:"
        public static bool CreateMap(string userName, string password, string remoteNetworkPath, string localDriveName)
        {
            NETRESOURCE myNetResource = new NETRESOURCE();
            myNetResource.dwScope = 2;       //2:RESOURCE_GLOBALNET
            myNetResource.dwType = 1;        //1:RESOURCETYPE_ANY
            myNetResource.dwDisplayType = 3; //3:RESOURCEDISPLAYTYPE_GENERIC
            myNetResource.dwUsage = 1;       //1: RESOURCEUSAGE_CONNECTABLE
            myNetResource.LocalName = localDriveName;
            myNetResource.RemoteName = remoteNetworkPath;
            myNetResource.Provider = null;

            uint nret = WNetAddConnection2(myNetResource, password, userName, 0);

            if (nret == 0)
                return true;
            else
                return false;
        }

        // localDriveName format:     @"E:"
        public static bool DeleteMap(string localDriveName)
        {
            uint nret = WNetCancelConnection2(localDriveName, 1, true);

            if (nret == 0)
                return true;
            else
                return false;
        }

    }
}
