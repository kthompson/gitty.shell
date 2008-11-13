using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;
using System.IO;

namespace Gitty.Shell
{

    [ComVisible(true), Guid("79EC607E-B516-47EC-AD6E-EBE618780D58"), ExtensionFileTypes("*,txtfile,Directory,Drive,Folder,InternetShortcut,lnkfile")]
    public class GitContextMenu : ContextMenuHandlerBase//, _GitContextMenu
    {
        public GitContext Context { get; private set; }

        public GitContextMenu()
        {
            MenuItems.Add("-", OnClick);
            MenuItems.Add("Git", OnClick);
        }

        protected override void OnInitialize()
        {
            //setup context
            //Context = Git.Open().GetContext();
        }

        protected void OnClick(object sender, EventArgs e)
        {
            MessageBox.Show("Click");
        }
     
    }

    //// Nested Types
    //[ComVisible(true), Guid("8F449365-A47C-41a2-A2FC-96929FC56EFA")]
    //public interface _GitContextMenu
    //{
    //    [DispId(1)]
    //    string OnMenuSelected(MenuItem Item);
    //}

}
