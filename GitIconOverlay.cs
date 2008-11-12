using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Gitty.Shell
{
    [Guid("babfc129-05eb-4643-a2a9-cd648a5ea13c"), ComVisible(true), ClassInterface(ClassInterfaceType.None)]
    public class GitIconOverlay : IconOverlayBase, _IconOverlayBase
    {
        public GitIconOverlay()
            : base(@"c:\projects\Gitty\Gitty.Shell\icon1.ico")
        {
            
        }

        public override bool OnDisplayOverlayIcon(DisplayOverlayArgs e)
        {
            return true;
        }

        public override int OnPriority()
        {
            return 99;
        }
    }


}
