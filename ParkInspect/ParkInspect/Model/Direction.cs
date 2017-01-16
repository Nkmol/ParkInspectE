using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkInspect.Model
{
    public class Direction
    {
        private string _Name;
        public List<String> direction_items = new List<string>();
        public int index = 0;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                }
            }
        }
    }
}
