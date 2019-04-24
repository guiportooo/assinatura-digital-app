using System;
using System.Collections.Generic;
using System.Text;

namespace AssinaturaDigital.Models
{
    public class Steps
    {
        public bool Done { get; private set; }
        public Steps(bool done)
        {
            Done = done;
        }
    }
}
