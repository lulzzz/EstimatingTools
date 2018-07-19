﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary.Interfaces
{
    public interface ICost: ITECObject
    {
        string Name { get; }
        string Description { get; }
        double Cost { get; }
        double Labor { get; }
        CostType Type { get; }
    }
}
