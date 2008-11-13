using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;


namespace Gitty.Shell
{
    public abstract class ContextMenuHandlerBase : IContextMenu, IShellExtInit
    {
        // Fields
        private ContextMenu m_CtxMenu = new ContextMenu();
        private FileNameCollection m_Files;

        // Methods
        #region static methods
        private static void AddItems(Menu.MenuItemCollection items, IntPtr hMenu, ref int cmdId)
        {
            foreach (MenuItem menu in items)
                InsertMenuItem(hMenu, menu, ref cmdId);
        }

        private static MenuItem FindMenu(Menu.MenuItemCollection items, int id, ref int cmdId)
        {
            Debugger.Launch();
            MenuItem findMenu = null;
            foreach (MenuItem menu in items)
            {
                if (menu.IsParent)
                {
                    findMenu = FindMenu(menu.MenuItems, id, ref cmdId);
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
        #endregion

        #region virtual methods
        protected virtual void OnInitialize()
        {
        }

        public virtual string OnMenuSelected(MenuItem item)
        {
            return item.Text;
        }
        #endregion

        #region com registration
        [ComRegisterFunction]
        private static void ComReg(Type t)
        {
            Helper.RegisterExtension(t, @"ContextMenuHandlers\" + t.Name);
        }

        [ComUnregisterFunction]
        private static void ComUnreg(Type t)
        {
            Helper.UnregisterExtension(t, @"ContextMenuHandlers\" + t.Name);
        }
        #endregion
        
        #region IShellExtInit
        void IShellExtInit.Initialize(int pidlFolder, IDataObject lpIDataObject, int hkeyProgID)
        {
            m_Files = new FileNameCollection(lpIDataObject);
            OnInitialize();
        }
        #endregion

        #region InsertMenuItem

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool InsertMenuItem(IntPtr hMenu, uint uItem, bool fByPosition, [In] ref MENUITEMINFO lpmii);

        private static  bool InsertMenuItem(IntPtr hMenu, uint position, [In] ref MENUITEMINFO lpmii)
        {
            return InsertMenuItem(hMenu, position, true, ref lpmii);
        }

        private static void InsertMenuItem(IntPtr hMenu, MenuItem menu, ref int cmdId)
        {
            var mii = new MENUITEMINFO {cbSize = MENUITEMINFO.sizeOf};
            IntPtr subMenu = IntPtr.Zero;

            if (menu.Checked)
                mii.fMask |= MENUITEMINFO_MASK.MIIM_CHECKMARKS;

            if (menu.IsParent)
            {
                mii.fMask |= MENUITEMINFO_MASK.MIIM_SUBMENU;
                subMenu = mii.hSubMenu = CreatePopupMenu();
            }
            else
            {
                mii.fMask = MENUITEMINFO_MASK.MIIM_ID;
                cmdId++;
                mii.wID = cmdId;
            }

            if (menu.Text == "-")
                mii.fType |= MENUITEMINFO_TYPE.MFT_SEPARATOR;
            else
            {
                mii.fType |= MENUITEMINFO_TYPE.MFT_STRING;
                mii.fMask |= MENUITEMINFO_MASK.MIIM_STRING;
                mii.dwTypeData = menu.Text;
                mii.cch = (uint)menu.Text.Length*2;
            }

            var success = InsertMenuItem(hMenu, (uint)menu.Index, ref mii);
            if (!success)
            {
                Debugger.Launch();
                string err = ComHelper.LastError;
            }

            if (subMenu != IntPtr.Zero)    
                AddItems(menu.MenuItems, subMenu, ref cmdId);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MENUITEMINFO
        {
            public uint cbSize;
            public MENUITEMINFO_MASK fMask;
            public MENUITEMINFO_TYPE fType;
            public MENUITEMINFO_STATE fState;
            public int wID;
            public IntPtr /*HMENU*/ hSubMenu;
            public IntPtr /*HBITMAP*/ hbmpChecked;
            public IntPtr /*HBITMAP*/ hbmpUnchecked;
            public IntPtr /*ULONG_PTR*/ dwItemData;
            [MarshalAs(UnmanagedType.LPStr)]
            public String dwTypeData;
            public uint cch;
            public IntPtr /*HBITMAP*/ hbmpItem;


            internal static uint sizeOf
            {
                get { return (uint)Marshal.SizeOf(typeof(MENUITEMINFO)); }
            }

        }  

        [Flags]
        private enum MENUITEMINFO_MASK : uint
        {
            MIIM_STATE = 0x1,
            MIIM_ID = 0x2,
            MIIM_SUBMENU = 0x4,
            MIIM_CHECKMARKS = 0x8,
            MIIM_TYPE = 0x10,
            MIIM_DATA = 0x20,
            MIIM_STRING = 0x40,
            MIIM_BITMAP = 0x80,
            MIIM_FTYPE = 0x100,
        }

        [Flags]
        private enum MENUITEMINFO_TYPE : uint
        {
            MFT_STRING = 0x0,
            MFT_BITMAP = 0x4,
            MFT_MENUBARBREAK = 0x20,
            MFT_MENUBREAK = 0x40,
            MFT_OWNERDRAW = 0x100,
            MFT_RADIOCHECK = 0x200,
            MFT_SEPARATOR = 0x800,
            MFT_RIGHTORDER = 0x2000,
        }

        [Flags]
        private enum MENUITEMINFO_STATE : uint
        {
            MFS_GRAYED = 0x3,
            MFS_DISABLED = 0x1,
            MFS_CHECKED = 0x8,
            MFS_HILITE = 0x80,
            MFS_DEFAULT = 0x1000,
        }
        #endregion

        #region IContextMenu

        [DllImport("User32")]
        private static extern IntPtr CreatePopupMenu();

        void IContextMenu.GetCommandString(int idCmd, GetCommandStringFlags uType, int pwReserved, IntPtr pszName, int cchMax)
        {
            if ((uType & GetCommandStringFlags.GCS_HELPTEXT) != GetCommandStringFlags.GCS_HELPTEXT) return;

            int S0 = 0;
            string text = OnMenuSelected(FindMenu(m_CtxMenu.MenuItems, idCmd, ref S0)) + "\0";
            if (text.Length < cchMax)
                cchMax = text.Length;

            if ((uType & GetCommandStringFlags.GCS_UNICODE) == GetCommandStringFlags.GCS_UNICODE)
                Marshal.Copy(text.ToCharArray(), 0, pszName, cchMax);
            else
                Marshal.Copy(Encoding.ASCII.GetBytes(text), 0, pszName, cchMax);
        }


        void IContextMenu.InvokeCommand(ref CMINVOKECOMMANDINFO lpici)
        {
            int S0 = 0;
            FindMenu(m_CtxMenu.MenuItems, lpici.lpVerb.ToInt32(), ref S0).PerformClick();
            m_Files = null;
            m_CtxMenu.Dispose();
            m_CtxMenu = null;
        }

        int IContextMenu.QueryContextMenu(IntPtr hMenu, int indexMenu, int idCmdFirst, int idCmdLast, QueryContextMenuFlags uFlags)
        {
            int firstID = idCmdFirst;
            AddItems(m_CtxMenu.MenuItems, hMenu, ref idCmdFirst);
            return (idCmdFirst - firstID);
        }
#endregion
        // Properties
        public ReadOnlyCollection<string> Files
        {
            get
            {
                return m_Files.AsReadOnly();
            }
        }

        public Menu.MenuItemCollection MenuItems
        {
            get
            {
                return m_CtxMenu.MenuItems;
            }
        }

    }
}
