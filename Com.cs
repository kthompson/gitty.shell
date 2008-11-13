using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Gitty.Shell
{

    [ComImport, Guid("0C6C4200-C589-11D0-999A-00C04FD655E1"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface x68b0aa936d40bad2
    {
        [PreserveSig]
        int x2ace4480301ba06c([MarshalAs(UnmanagedType.LPWStr)] string xa31bc830851248f5, [MarshalAs(UnmanagedType.U4)] int x37122af4efc25ea2);
        [PreserveSig]
        int xbd2501623e4874d2(IntPtr xafe2f3653ee64ebc, int xfcffe10b2208dfca, out int xf9de607638860fdb, out uint x896ecbff45ecd055);
        [PreserveSig]
        int x411547f8fa9c8b64(out int xc0ccccf0a590fcb4);
    }

    [Flags]
    internal enum ISIOI
    {
        ISIOI_ICONFILE = 1,
        ISIOI_ICONINDEX = 2
    }

    

    [ComVisible(false)]
    [ComImport]
    [Guid("0C6C4200-C589-11D0-999A-00C04FD655E1")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IShellIconOverlayIdentifier
    {

        [PreserveSig]
        int IsMemberOf(
            [MarshalAs(UnmanagedType.LPWStr)] string path,
            [MarshalAs(UnmanagedType.U4)] int attributes);

        void GetOverlayInfo(
            IntPtr iconFileBuffer,
            int iconFileBufferSize,
            out int iconIndex,
            [MarshalAs(UnmanagedType.U4)] out ISIOI flags);

        
        void GetPriority(
            out int priority);

    }


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

    [ComImport, Guid("000214E8-0000-0000-C000-000000000046"), EditorBrowsable(EditorBrowsableState.Never), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IShellExtInit
    {
        void Initialize(int pidlFolder, [MarshalAs(UnmanagedType.Interface)] IDataObject lpIDataObject, int hkeyProgID);
    }

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

    public static class ComHelper
    {
        [DllImport("Kernel32.dll")]
        private static extern int FormatMessage(
            [MarshalAs(UnmanagedType.I4)] ref FormatMessageFlags dwFlags, ref object lpSource, ref int dwMessageId,
            ref int dwLanguageId, IntPtr msg, ref int nSize, ref object Arguments);


        /// <summary> 
/// Flags for the FormatMessage kernel32 function. 
/// </summary> 
        [Flags()]
        public enum FormatMessageFlags
        {
            /// <summary>The lpBuffer parameter is a pointer to a PVOID pointer, and that the nSize parameter specifies the minimum number of TCHARs to allocate for an output message buffer. The function allocates a buffer large enough to hold the formatted message, and places a pointer to the allocated buffer at the address specified by lpBuffer. The caller should use the LocalFree function to free the buffer when it is no longer needed.</summary> 
            FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100,
            /// <summary>Insert sequences in the message definition are to be ignored and passed through to the output buffer unchanged. This flag is useful for fetching a message for later formatting. If this flag is set, the Arguments parameter is ignored.</summary> 
            FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200,
            /// <summary>The lpSource parameter is a pointer to a null-terminated message definition. The message definition may contain insert sequences, just as the message text in a message table resource may. Cannot be used with FORMAT_MESSAGE_FROM_HMODULE or FORMAT_MESSAGE_FROM_SYSTEM.</summary> 
            FORMAT_MESSAGE_FROM_STRING = 0x00000400,
            /// <summary>The lpSource parameter is a module handle containing the message-table resource(s) to search. If this lpSource handle is NULL, the current process's application image file will be searched. Cannot be used with FORMAT_MESSAGE_FROM_STRING.</summary> 
            FORMAT_MESSAGE_FROM_HMODULE = 0x00000800,
            /// <summary>The function should search the system message-table resource(s) for the requested message. If this flag is specified with FORMAT_MESSAGE_FROM_HMODULE, the function searches the system message table if the message is not found in the module specified by lpSource. Cannot be used with FORMAT_MESSAGE_FROM_STRING. 
            /// If this flag is specified, an application can pass the result of the GetLastError function to retrieve the message text for a system-defined error.</summary> 
            FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000,
            /// <summary>The Arguments parameter is not a va_list structure, but is a pointer to an array of values that represent the arguments. 
            /// This flag cannot be used with 64-bit argument values. If you are using 64-bit values, you must use the va_list structure.</summary> 
            FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x00002000,
            /// <summary>The low-order byte of dwFlags can specify the maximum width of a formatted output line. Use the FORMAT_MESSAGE_MAX_WIDTH_MASK constant and bitwise Boolean operations to set and retrieve this maximum width value.</summary> 
            FORMAT_MESSAGE_MAX_WIDTH_MASK = 0x000000FF
        }
        [DllImport("kernel32.dll")]
        private extern static int GetLastError();

        public static string LastError
        {
            get
            {
                int max_length = 64000;
                IntPtr ptr = Marshal.AllocHGlobal(max_length);
                try
                {
                    FormatMessageFlags flags = FormatMessageFlags.FORMAT_MESSAGE_FROM_SYSTEM;
                    int e = GetLastError();
                    object source = null;
                    int lang = 0;
                    object args = null;
                    int c = FormatMessage(ref flags, ref source, ref e, ref lang, ptr, ref max_length, ref args);
                    /* 
                        'c', the return value, is always 0. 
                        If GetLastError() is called at this point, it will return 1812, which seems strange since I'm passing in 
                    FORMAT_MESSAGE_FROM_SYSTEM 
                    */
                    StringBuilder sb = new StringBuilder();
                    sb.Append(e);
                    if (c != 0)
                        sb.AppendFormat(": {0}", Marshal.PtrToStringAnsi(ptr));
                    return sb.ToString();
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        } 
    }
}