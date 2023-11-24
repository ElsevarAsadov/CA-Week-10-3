using Microsoft.Extensions.Hosting;
using System.ComponentModel;
using WebApplication1.Areas.Admin.Models;

namespace WebApplication1.Helpers
{
    public static class Utils
    {


        public static string ConvertAbsToURI(string root, string abs) {
        
            return abs.Replace(root, "").Replace("\\", "/");
        }

        public static long ByteToMb(long length) {

            return length / 1024 * 2;
        }
    }

}
