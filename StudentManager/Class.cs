using System;
using System.Collections.Generic;

namespace StudentManager
{
    public class Class : Identity
    {
        [Identity.ID]
        public string Name { get; set; }

        public string Teacher { get; set; }
    }
}

