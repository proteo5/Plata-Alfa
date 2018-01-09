using PlataAlfa.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataAlfa.api.V1_0
{
    public class Auth : Entity
    {
        public Envelope Login(object data)
        {
            return new Envelope() { Result="ok", Data = data };
        }

        
    }
}
