using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;

using Microsoft.Win32;

using Gitty.Shell.Com;
using System.Runtime.InteropServices.ComTypes;


namespace Gitty.Shell
{
    public abstract class ContextMenuHandlerBase : IContextMenu, IShellExtInit
    {
        ContextMenu _menu =  new ContextMenu();

        public ContextMenuHandlerBase()
        {

        }

        public ReadOnlyCollection<string> Files { get; private set; }
        public MenuItem.MenuItemCollection MenuItems { get; private set; }

        public virtual string OnMenuSelected(MenuItem item)
        {
            return item.Text;
        }

        [Flags()]
        enum MenuFlags : uint
        {
            MF_BYPOSITION = 0x400,
            MF_SEPARATOR = 0x800,
            MF_OWNERDRAW = 0x100,
            MF_POPUP = 0x10,
        }

        [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool InsertMenu(IntPtr hmenu, int position, MenuFlags flags, int item_id, [MarshalAs(UnmanagedType.LPTStr)]string item_text);

        [DllImport("user32.dll")]
        private static extern IntPtr CreatePopupMenu();


        private void AddItems(MenuItem.MenuItemCollection items, IntPtr hMenu, int index, int cmd)
        {
            foreach (MenuItem menu in items)
            {
                if (menu.IsParent)
                {
                    IntPtr popMenu = CreatePopupMenu();

                    InsertMenu(hMenu, index, MenuFlags.MF_BYPOSITION | MenuFlags.MF_POPUP, popMenu.ToInt32(), menu.Text);
                    AddItems(menu.MenuItems, popMenu, 0, cmd);
                }
                else
                {
                    InsertMenu(hMenu, index, MenuFlags.MF_BYPOSITION, cmd, menu.Text);
                    cmd++;
                }
                index++;
            }
        }

        private MenuItem FindMenu(MenuItem.MenuItemCollection items, int id, int cmd)
        {
            foreach (MenuItem menu in items)
            {
                if (menu.IsParent)
                {
                    MenuItem ret = FindMenu(menu.MenuItems, id, cmd);
                    if (ret != null)
                        return ret;
                }
                else
                {
                    if (cmd == id)
                        return menu;
                    cmd++;
                }
            }
            return null;
        }


        #region " COM Registration "


        [ComRegisterFunction()]
        private static void Register(Type t)
        {
            Helper.RegisterExtension(t, "ContextMenuHandlers\\" + t.Name);
        }

        [ComUnregisterFunction()]
        private static void Unregister(Type t)
        {
            Helper.UnregisterExtension(t, "ContextMenuHandlers\\" + t.Name);
        }

        #endregion

        #region IContextMenu Members

        int IContextMenu.QueryContextMenu(IntPtr hmenu, int iMenu, int idCmdFirst, int idCmdLast, QueryContextMenuFlags uFlags)
        {
            MessageBox.Show("IContextMenu.QueryContextMenu");
            int firstID = idCmdFirst;

            // Add the items to the shell context menu
            AddItems(_menu.MenuItems, hmenu, iMenu, idCmdFirst);

            // Return how many items were added
            return idCmdFirst - firstID;
        }

        void IContextMenu.InvokeCommand(IntPtr ipici)
        {
            CMINVOKECOMMANDINFO pici = (CMINVOKECOMMANDINFO)Marshal.PtrToStructure(ipici, typeof(CMINVOKECOMMANDINFO));
            MessageBox.Show("IContextMenu.InvokeCommand");
            //Find the MenuItem object
            MenuItem item = FindMenu(_menu.MenuItems, pici.lpVerb.ToInt32(), 0);

            // Call the Click event
            item.PerformClick();

            // Release the Files collection
            this.Files = null;

            // Release the menus
            _menu.Dispose();
            _menu = null;

        }

        void IContextMenu.GetCommandString(int idcmd, GetCommandStringFlags uflags, int reserved, IntPtr pszName, int cch)
        {
            MessageBox.Show("IContextMenu.GetCommandString");
            if ((uflags & GetCommandStringFlags.GCS_HELPTEXT) == GetCommandStringFlags.GCS_HELPTEXT)
            {

                string text = this.OnMenuSelected(FindMenu(_menu.MenuItems, idcmd, 0)) + '\0';

                if (text.Length < cch)
                    cch = text.Length;


                if ((uflags & GetCommandStringFlags.GCS_UNICODE) == GetCommandStringFlags.GCS_UNICODE)
                    Marshal.Copy(text.ToCharArray(), 0, pszName, cch);
                else
                    Marshal.Copy(Encoding.ASCII.GetBytes(text), 0, pszName, cch);
            }
        }

        #endregion



        #region IShellExtInit Members

        int IShellExtInit.Initialize(int pidlFolder, System.Runtime.InteropServices.ComTypes.IDataObject lpdobj, int hKeyProgID)
        {
            MessageBox.Show("IShellExtInit.Initialize");
            this.Files = new FileNameCollection(lpdobj).AsReadOnly();
            return 0;
        }

        #endregion

    }
}
