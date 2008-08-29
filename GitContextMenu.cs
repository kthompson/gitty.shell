using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Gitty.Shell
{
    [ExtensionFileTypes("*,Folder"),     
    Guid("79EC607E-B516-47EC-AD6E-EBE618780D58"),
    ClassInterface(ClassInterfaceType.None),
    ComVisible(true)]
    public class GitContextMenu : ContextMenuHandlerBase, IContextMenuHandler
    {
        public GitContextMenu()
        {
            this.MenuItems.Add("Git Project", this.GitProject_Click);
        }

        public override string OnMenuSelected(System.Windows.Forms.MenuItem item)
        {
            return base.OnMenuSelected(item);
        }

        public void GitProject_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hi Pals");
        }
    }
}
