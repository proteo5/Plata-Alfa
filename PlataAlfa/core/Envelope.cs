using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataAlfa.core
{
    public class Envelope
    {
        public string Result { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public object Data { get; set; }
    }
}
