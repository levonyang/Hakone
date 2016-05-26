using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PanGu;

namespace Honshu.Fetcher.Cube
{
    public class PanguHelper
    {
        public PanguHelper()
        {
            PanGu.Segment.Init();
        }        

        public static string Segment(string input)
        {
            var result = string.Empty;

            
            var segment = new Segment();

            var words = segment.DoSegment(input);
            foreach (var wordInfo in words)
            {
                result += " " + wordInfo.Word;;
            }

            return result;
        }
    }
}
