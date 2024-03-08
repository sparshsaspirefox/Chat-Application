using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Helpers
{
    public static class AppConstants
    {
        public static Uri baseAddress = new Uri("https://7mwfwf0g-7074.inc1.devtunnels.ms/api");
        public static Uri staticsFiles = new Uri("https://7mwfwf0g-7074.inc1.devtunnels.ms/");
        public static Uri hubAddress = new Uri("https://7mwfwf0g-7074.inc1.devtunnels.ms");


        public static string ActiveTab = "chats";

        // coverting relative url to absolute
        public static string GetUrl(string url)
        {
            if (url != null)
            {
                return AppConstants.staticsFiles.ToString() + url;
            }
            // for image placeholder 
            return ".\\imagePlace.jpg";
        }
    }
}
