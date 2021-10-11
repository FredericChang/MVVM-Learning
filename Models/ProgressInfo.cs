using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Models
{
    public class ProgressInfo
        
    {
        public int ProgressValue { get; set; }
        public string ProgressText { get; set; }

        public ProgressInfo(int value, string text)
        {
            ProgressValue = value;
            ProgressText = text;
        }
    }
}
