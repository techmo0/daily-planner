using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer
{
    public class Event
    {
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{Type} on {Date.ToShortDateString()} at {Date.ToShortTimeString()} for {Duration} minutes: {Description}";
        }
    }

}
