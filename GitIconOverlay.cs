using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Gitty.Shell
{
    [Guid("babfc129-05eb-4643-a2a9-cd648a5ea13c"), ComVisible(true), ClassInterface(ClassInterfaceType.AutoDispatch)]
    public class GitIconOverlay : IconOverlayBase
    {
        protected override bool OnDisplayOverlayIcon(DisplayOverlayArgs e)
        {
            return Path.GetExtension(e.Path) == ".dll";
        }

        protected override void OnSelectOverlay(SelectOverlayArgs e)
        {
            e.IconFile = @"C:\projects\Gitty\Gitty.Shell\icon1.ico";
        }

        protected override int OnPriority()
        {
            return 0;
        }
    }
}
