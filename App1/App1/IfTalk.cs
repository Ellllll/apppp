using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App1
{
    public class IfTalk
    {
        public IfTalk(bool If = true) { }
        public static bool If;
        public bool GetIf()
        {
            return If;
        }
        public void SetIf(bool if_)
        {
            If = if_;
        }
    }
}
