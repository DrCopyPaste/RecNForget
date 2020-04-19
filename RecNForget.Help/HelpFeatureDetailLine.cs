using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Help
{
    public class HelpFeatureDetailLine
    {
        /// <summary>
        /// if true contents of this line will be evaluated according to application uri (resource) rules OR accorting to github link structure for online md files
        /// otherwise contents will be interpreted as plain simple text
        /// </summary>
        public bool IsImage { get; set; } = false;

        public string Content { get; set; }
    }
}
