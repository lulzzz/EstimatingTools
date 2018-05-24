﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary.Interfaces
{
    interface IControllerConnection
    {
        TECController ParentController { get; }
        IOCollection HardwiredIO { get; }
    }
}
