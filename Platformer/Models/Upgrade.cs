using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Models;

public class Upgrade
{
        public string Icon { get; set; }
        public string UpgradeName { get; set; }
        public double DefaultCost { get; set; }
        public double ExtraCostPerUpgrade { get; set; }
        public double ExtraJumpsPerUpgrade { get; set; }
}
