using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Gitty.Shell.Com
{

    #region IContextMenu
    [ComImport(),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
     Guid("000214e4-0000-0000-c000-000000000046")]
    internal interface IContextMenu
    {
        [PreserveSig()]
        int QueryContextMenu(IntPtr hmenu, int iMenu, int idCmdFirst, int idCmdLast, QueryContextMenuFlags uFlags);

        [PreserveSig()]
        void InvokeCommand(IntPtr pici);

        [PreserveSig()]
        void GetCommandString(int idcmd, GetCommandStringFlags uflags, int reserved, IntPtr pszName, int cch);
    }



    [Flags()]
    internal enum QueryContextMenuFlags : uint
    {
        CMF_NORMAL = 0x0,
        CMF_DEFAULTONLY = 0x1,
        CMF_VERBSONLY = 0x2,
        CMF_EXPLORE = 0x4,
        CMF_NOVERBS = 0x8,
        CMF_CANRENAME = 0x10,
        CMF_NODEFAULT = 0x20,
        CMF_INCLUDESTATIC = 0x40,
        CMF_RESERVED = 0xFFFF0000,
    }


    [Flags()]
    internal enum GetCommandStringFlags
    {
        GCS_VERB = 0x0,
        GCS_HELPTEXT = 0x1,
        GCS_VALIDATE = 0x2,
        GCS_UNICODE = 0x4,
    }


    [Flags()]
    internal enum InvokeCommandMask
    {
        CMIC_MASK_HOTKEY = 0x20,
        CMIC_MASK_ICON = 0x10,
        CMIC_MASK_FLAG_NO_UI = 0x400,
        CMIC_MASK_UNICODE = 0x4000,
        CMIC_MASK_NO_CONSOLE = 0x8000,
        CMIC_MASK_ASYNCOK = 0x100000,
        CMIC_MASK_PTINVOKE = 0x20000000,
        CMIC_MASK_SHIFT_DOWN = 0x10000000,
        CMIC_MASK_CONTROL_DOWN = 0x20000000,
    }


    internal struct CMINVOKECOMMANDINFO
    {
        public int cbSize;
        public InvokeCommandMask fMask;
        public IntPtr hwnd;
        public IntPtr lpVerb;
        public IntPtr lpParameters;
        public IntPtr lpDirectory;
        public int nShow;
        public int dwHotKey;
        public IntPtr hIcon;
    }

    #endregion



    [ComImport(),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
    Guid("000214e8-0000-0000-c000-000000000046")]
    public interface IShellExtInit
    {
        [PreserveSig()]
        int Initialize(int pidlFolder, System.Runtime.InteropServices.ComTypes.IDataObject lpdobj, int hKeyProgID);
    }

}
