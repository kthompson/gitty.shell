﻿using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Gitty.Shell.Com
{

    #region IContextMenu
    [ComImport, Guid("000214E4-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IContextMenu
    {
        [PreserveSig]
        int QueryContextMenu(IntPtr hMenu, int indexMenu, int idCmdFirst, int idCmdLast, QueryContextMenuFlags uFlags);
        void InvokeCommand(ref CMINVOKECOMMANDINFO lpici);
        void GetCommandString(int idCmd, GetCommandStringFlags uType, int pwReserved, IntPtr pszName, int cchMax);
    }

    [Flags, EditorBrowsable(EditorBrowsableState.Never)]
    internal enum GetCommandStringFlags
    {
        GCS_HELPTEXT = 1,
        GCS_UNICODE = 4,
        GCS_VALIDATE = 2,
        GCS_VERB = 0
    }

    [Flags, EditorBrowsable(EditorBrowsableState.Never)]
    internal enum QueryContextMenuFlags
    {
        CMF_CANRENAME = 0x10,
        CMF_DEFAULTONLY = 1,
        CMF_EXPLORE = 4,
        CMF_INCLUDESTATIC = 0x40,
        CMF_NODEFAULT = 0x20,
        CMF_NORMAL = 0,
        CMF_NOVERBS = 8,
        CMF_RESERVED = -65536,
        CMF_VERBSONLY = 2
    }

 



    


    [StructLayout(LayoutKind.Sequential), EditorBrowsable(EditorBrowsableState.Never)]
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

    [EditorBrowsable(EditorBrowsableState.Never), Flags]
    internal enum InvokeCommandMask
    {
        CMIC_MASK_ASYNCOK = 0x100000,
        CMIC_MASK_CONTROL_DOWN = 0x20000000,
        CMIC_MASK_FLAG_NO_UI = 0x400,
        CMIC_MASK_HOTKEY = 0x20,
        CMIC_MASK_ICON = 0x10,
        CMIC_MASK_NO_CONSOLE = 0x8000,
        CMIC_MASK_PTINVOKE = 0x20000000,
        CMIC_MASK_SHIFT_DOWN = 0x10000000,
        CMIC_MASK_UNICODE = 0x4000
    }

 


 

    #endregion



    [ComImport, Guid("000214E8-0000-0000-C000-000000000046"), EditorBrowsable(EditorBrowsableState.Never), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IShellExtInit
    {
        void Initialize(int pidlFolder, [MarshalAs(UnmanagedType.Interface)] IDataObject lpIDataObject, int hkeyProgID);
    }



    #region IDataObject
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("0000010e-0000-0000-C000-000000000046"), EditorBrowsable(EditorBrowsableState.Never)]
    internal interface IDataObject
    {
        void GetData(ref FORMATETC pformatetcIn, ref STGMEDIUM pmedium);
        void GetDataHere(ref FORMATETC pformatetc, ref STGMEDIUM pmedium);
        [PreserveSig]
        int QueryGetData(ref FORMATETC pformatetc);
        void GetCanonicalFormatEtc(ref FORMATETC pformatectIn, ref FORMATETC pformatetcOut);
        void SetData(ref FORMATETC pformatetc, ref STGMEDIUM pmedium, [MarshalAs(UnmanagedType.Bool)] bool fRelease);
        [return: MarshalAs(UnmanagedType.Interface)]
        object EnumFormatEtc(DATADIR dwDirection);
        int DAdvise(ref FORMATETC pformatetc, int advf, [MarshalAs(UnmanagedType.Interface)] object pAdvSink);
        void DUnadvise(int dwConnection);
        [return: MarshalAs(UnmanagedType.Interface)]
        object EnumDAdvise();
    }

    [StructLayout(LayoutKind.Sequential), EditorBrowsable(EditorBrowsableState.Never)]
    internal struct FORMATETC
    {
        public short cfFormat;
        public int ptd;
        public DVASPECT dwAspect;
        public int lindex;
        public TYMED tymed;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal enum DVASPECT
    {
        DVASPECT_CONTENT = 1,
        DVASPECT_DOCPRINT = 8,
        DVASPECT_ICON = 4,
        DVASPECT_THUMBNAIL = 2
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal enum TYMED
    {
        TYMED_ENHMF = 0x40,
        TYMED_FILE = 2,
        TYMED_GDI = 0x10,
        TYMED_HGLOBAL = 1,
        TYMED_ISTORAGE = 8,
        TYMED_ISTREAM = 4,
        TYMED_MFPICT = 0x20,
        TYMED_NULL = 0
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal enum DATADIR
    {
        DATADIR_GET = 1,
        DATADIR_SET = 2
    }

    [StructLayout(LayoutKind.Sequential), EditorBrowsable(EditorBrowsableState.Never)]
    internal struct STGMEDIUM
    {
        public TYMED tymed;
        public int data;
        [MarshalAs(UnmanagedType.IUnknown)]
        public object pUnkForRelease;
    }

    #endregion



}
