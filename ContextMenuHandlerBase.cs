using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Gitty.Shell.Com;
using Microsoft.Win32;


namespace Gitty.Shell
{

    [ComVisible(true), Guid("44bfc30d-c42e-4277-a31c-af2b17cbb6d6")]
    public interface IContextMenuHandler
    {
        [DispId(1)]
        string OnMenuSelected(MenuItem Item);
    }

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

        [EditorBrowsable(EditorBrowsableState.Never), DllImport("User32", EntryPoint = "InsertMenuA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern long InsertMenu(IntPtr hMenu, int nPosition, MenuFlags wFlags, int wIDNewItem, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpNewItem);
    

        [EditorBrowsable(EditorBrowsableState.Never), DllImport("User32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr CreatePopupMenu();



        private void AddItems(Menu.MenuItemCollection items, IntPtr hMenu, int index, ref int cmdId)
        {
            foreach (MenuItem menu in items)
            {
                string text = menu.Text;
                if (menu.IsParent)
                {
                    IntPtr popMenu = CreatePopupMenu();
                    InsertMenu(hMenu, index, MenuFlags.MF_BYPOSITION | MenuFlags.MF_POPUP, popMenu.ToInt32(), ref text);
                    AddItems(menu.MenuItems, popMenu, 0, ref cmdId);
                }
                else
                {
                    InsertMenu(hMenu, index, MenuFlags.MF_BYPOSITION, cmdId, ref text);
                    cmdId++;
                }
                index++;
            }
        }

        private MenuItem FindMenu(Menu.MenuItemCollection items, int id, ref int cmdId)
        {
            foreach (MenuItem menu in items)
            {
                if (menu.IsParent)
                {
                    MenuItem ret = FindMenu(menu.MenuItems, id, ref cmdId);
                    if (ret != null)
                        return ret;
                }
                else
                {
                    if (cmdId == id)
                        return menu;
                    cmdId++;
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

        int IContextMenu.QueryContextMenu(IntPtr hmenu, int indexMenu, int idCmdFirst, int idCmdLast, QueryContextMenuFlags uFlags)
        {
            int firstID = idCmdFirst;

            // Add the items to the shell context menu
            AddItems(_menu.MenuItems, hmenu, indexMenu, ref idCmdFirst);

            // Return how many items were added
            return idCmdFirst - firstID;
        }

        void IContextMenu.InvokeCommand(ref CMINVOKECOMMANDINFO pici)
        {
            int zeroRef = 0;
            //Find the MenuItem object
            MenuItem item = FindMenu(_menu.MenuItems, pici.lpVerb.ToInt32(), ref zeroRef);

            // Call the Click event
            if(item!=null)
                item.PerformClick();

            // Release the Files collection
            this.Files = null;

            // Release the menus
            _menu.Dispose();
            _menu = null;

        }

        void IContextMenu.GetCommandString(int idcmd, GetCommandStringFlags uflags, int reserved, IntPtr pszName, int cch)
        {
            if ((uflags & GetCommandStringFlags.GCS_HELPTEXT) == GetCommandStringFlags.GCS_HELPTEXT)
            {
                int zeroRef = 0;
                string text = this.OnMenuSelected(FindMenu(_menu.MenuItems, idcmd, ref zeroRef)) + '\0';

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

        void IShellExtInit.Initialize(int pidlFolder, Gitty.Shell.Com.IDataObject lpIDataObject, int hkeyProgID)
        {
            this.Files = new FileNameCollection(lpIDataObject).AsReadOnly();            
        }

        #endregion

    }
}
