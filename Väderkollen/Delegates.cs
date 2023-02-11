using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Väderkollen
{
    public class Delegates
    {

        static string Fukt(string kod)
        {
            return kod;
        }

        public static void Fuktmethod()
        {
            Func<string> Get = Fukt(@".(?<=\.\d,\d +).").ToString;

        }
        

        
        
      


    }
}
