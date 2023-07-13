using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ekrankilit_denemesi
{
    internal class DatabasePath
    {
        public static string form_bacgroundımage_path = Application.StartupPath+ "\\FormBackgrounImage\\forms1.png";
        public static string users_path = System.IO.File.ReadAllText(Application.StartupPath+"\\USDBP.txt");
    }
}
