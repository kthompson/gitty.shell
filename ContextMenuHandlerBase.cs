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
    public abstract class ContextMenuHandlerBase : IContextMenu, IShellExtInit
    {
        // Fields
        [EditorBrowsable(EditorBrowsableState.Never)]
        private ContextMenu m_CtxMenu = new ContextMenu();
        [EditorBrowsable(EditorBrowsableState.Never)]
        private FileNameCollection m_Files;

        // Methods
        protected ContextMenuHandlerBase()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        private void AddItems(Menu.MenuItemCollection items, IntPtr hMenu, int index, ref int cmdId)
        {
            foreach (MenuItem menu in items)
            {
                string menuText = menu.Text;
                if (menu.IsParent)
                {
                    IntPtr popMenu = CreatePopupMenu();
                    InsertMenu(hMenu, index, MenuFlags.MF_BYPOSITION | MenuFlags.MF_POPUP, popMenu.ToInt32(), ref menuText);
                    this.AddItems(menu.MenuItems, popMenu, 0, ref cmdId);
                }
                else
                {
                    if (menuText == "-")
                        InsertMenu(hMenu, index, MenuFlags.MF_BYPOSITION | MenuFlags.MF_SEPARATOR, cmdId, ref menuText);
                    else
                        InsertMenu(hMenu, index, MenuFlags.MF_BYPOSITION, cmdId, ref menuText);
                    
                    cmdId++;
                }
                menu.Text = menuText;
                index++;
            }

        }

        [EditorBrowsable(EditorBrowsableState.Never), ComRegisterFunction]
        private static void ComReg(Type t)
        {
            Helper.RegisterExtension(t, @"ContextMenuHandlers\" + t.Name);
        }

        [ComUnregisterFunction, EditorBrowsable(EditorBrowsableState.Never)]
        private static void ComUnreg(Type t)
        {
            Helper.UnregisterExtension(t, @"ContextMenuHandlers\" + t.Name);
        }

        [EditorBrowsable(EditorBrowsableState.Never), DllImport("User32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr CreatePopupMenu();
        [EditorBrowsable(EditorBrowsableState.Never)]
        private MenuItem FindMenu(Menu.MenuItemCollection items, int id, ref int cmdId)
        {
            MenuItem findMenu = null;
            foreach (MenuItem menu in items)
            {
                if (menu.IsParent)
                {
                    findMenu = this.FindMenu(menu.MenuItems, id, ref cmdId);
                    if (findMenu != null)
                        return findMenu;
                }
                else
                {
                    if (cmdId == id)
                        return menu;
                    cmdId++;
                }
            }
            return findMenu;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        void IContextMenu.GetCommandString(int idCmd, GetCommandStringFlags uType, int pwReserved, IntPtr pszName, int cchMax)
        {
            if ((uType & GetCommandStringFlags.GCS_HELPTEXT) == GetCommandStringFlags.GCS_HELPTEXT)
            {
                int S0 = 0;
                string text = this.OnMenuSelected(this.FindMenu(this.m_CtxMenu.MenuItems, idCmd, ref S0)) + "\0";
                if (text.Length < cchMax)
                {
                    cchMax = text.Length;
                }
                if ((uType & GetCommandStringFlags.GCS_UNICODE) == GetCommandStringFlags.GCS_UNICODE)
                {
                    Marshal.Copy(text.ToCharArray(), 0, pszName, cchMax);
                }
                else
                {
                    Marshal.Copy(Encoding.ASCII.GetBytes(text), 0, pszName, cchMax);
                }
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        void IShellExtInit.Initialize(int pidlFolder, Gitty.Shell.Com.IDataObject lpIDataObject, int hkeyProgID)
        {
            this.m_Files = new FileNameCollection(lpIDataObject);
        }

        [EditorBrowsable(EditorBrowsableState.Never), DllImport("User32", EntryPoint = "InsertMenuA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern long InsertMenu(IntPtr hMenu, int nPosition, MenuFlags wFlags, int wIDNewItem, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpNewItem);
        [EditorBrowsable(EditorBrowsableState.Never)]
        void IContextMenu.InvokeCommand(ref CMINVOKECOMMANDINFO lpici)
        {
            int S0 = 0;
            this.FindMenu(this.m_CtxMenu.MenuItems, lpici.lpVerb.ToInt32(), ref S0).PerformClick();
            this.m_Files = null;
            this.m_CtxMenu.Dispose();
            this.m_CtxMenu = null;
        }

        public virtual string OnMenuSelected(MenuItem item)
        {
            return item.Text;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        int IContextMenu.QueryContextMenu(IntPtr hMenu, int indexMenu, int idCmdFirst, int idCmdLast, QueryContextMenuFlags uFlags)
        {
            int firstID = idCmdFirst;
            this.AddItems(this.m_CtxMenu.MenuItems, hMenu, indexMenu, ref idCmdFirst);
            return (idCmdFirst - firstID);
        }

        // Properties
        public ReadOnlyCollection<string> Files
        {
            get
            {
                return this.m_Files.AsReadOnly();
            }
        }

        public Menu.MenuItemCollection MenuItems
        {
            get
            {
                return this.m_CtxMenu.MenuItems;
            }
        }

        // Nested Types
        [Flags, EditorBrowsable(EditorBrowsableState.Never)]
        private enum MenuFlags
        {
            MF_BYPOSITION = 0x400,
            MF_OWNERDRAW = 0x100,
            MF_POPUP = 0x10,
            MF_SEPARATOR = 0x800
        }

    }
}
