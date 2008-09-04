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
            this.MenuItems.Add("-");
            this.MenuItems.Add("Create &Playlist...", new EventHandler(this.CreatePlaylist));
            Menu.MenuItemCollection submenu = this.MenuItems.Add("Hi Pals").MenuItems;
            this.MenuItems.Add("-");
            int i = 0;
            do
            {
                submenu.Add("Menu #" + i.ToString(), new EventHandler(this.CreatePlaylist2));
                i++;
            }
            while (i <= 5);            
        }

        private void CreatePlaylist(object sender, EventArgs e)
        {
            MessageBox.Show("Create a playlist");
        }

        private void CreatePlaylist2(object sender, EventArgs e)
        {
            MessageBox.Show("Hi Pals");
        }

        public override string OnMenuSelected(MenuItem Item)
        {
            return "Creates a playlist with the selected files";
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
