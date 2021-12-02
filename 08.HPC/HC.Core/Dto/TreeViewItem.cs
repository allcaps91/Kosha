using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_Core.Dto
{
    public class TreeViewItem
    {
        public long ID { get; set; }
        public long ParentID { get; set; }
        public string Text { get; set; }
        public object Model { get; set; }

        public Color Color { get; set; }
}
}
