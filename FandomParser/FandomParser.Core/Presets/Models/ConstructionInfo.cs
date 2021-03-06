﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FandomParser.Core.Presets.Models
{

    [DataContract]
    public class ConstructionInfo
    {
        [DataMember(Order = 0)]
        public double Value { get; set; }

        [DataMember(Order = 1)]
        public CostUnit Unit { get; set; }
    }
}
