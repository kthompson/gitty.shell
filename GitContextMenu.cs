using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;
using System.IO;

namespace Gitty.Shell
{

    [Guid("79EC607E-B516-47EC-AD6E-EBE618780D58"), 
     ExtensionFileTypes("*,txtfile,Directory,Drive,Folder,InternetShortcut,lnkfile"),
     ClassInterface(ClassInterfaceType.None)]
    public class GitContextMenu : ContextMenuHandlerBase, _GitContextMenu
    {
        // Methods
        public GitContextMenu()
        {
            var submenu = MenuItems.Add("Git").MenuItems;
            int i = 0;
            do
            {
                submenu.Add("Menu #" + i, CreatePlaylist2);
                i++;
            }
            while (i <= 5);            
        }

        private void CreatePlaylist2(object sender, EventArgs e)
        {
            MessageBox.Show("Hi Pals");
        }

        public override string OnMenuSelected(MenuItem Item)
        {
            return "Select a git command";
        }

        
    }

    // Nested Types
    [ComVisible(true), Guid("8F449365-A47C-41a2-A2FC-96929FC56EFA")]
    public interface _GitContextMenu
    {
        [DispId(1)]
        string OnMenuSelected(MenuItem Item);
    }

}
