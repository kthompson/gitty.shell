using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Gitty.Shell
{
    [ComVisible(false)]
    public abstract class IconOverlayBase : IShellIconOverlayIdentifier
    {
        private const int S_OK = 0;
        private const int S_FALSE = 1;
        private const int E_FAIL = 0;
        public string IconFile { get; set; }
        public int IconIndex { get; set; }

        public class DisplayOverlayArgs : EventArgs
        {
            public DisplayOverlayArgs(string path, int attributes)
            {
                Path = path;
                FileAttributes = attributes;
            }

            public string Path { get; private set; }
            public int FileAttributes { get; private set; }
        }

        protected IconOverlayBase(string file)
            : this(file, 0)
        {
            
        }
        protected IconOverlayBase(string file, int index)
        {
            IconFile = file;
            IconIndex = index;
        }

        public virtual int OnPriority()
        {
            return 0;
        }

        public virtual bool OnDisplayOverlayIcon(DisplayOverlayArgs e)
        {
            return false;
        }

        #region Registry

        [ComRegisterFunction]
        public static void Register(Type t)
        {
            RegistryKey rk =
            Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\ShellIconOverlayIdentifiers\_" + t.Name);
            rk.SetValue(string.Empty, t.GUID.ToString("B").ToUpper());
            rk.Close();
            Helper.FileAssociationsChanged();
        }
        [ComUnregisterFunction]
        public static void Unregister(Type t)
        {
            Registry.LocalMachine.DeleteSubKeyTree(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\ShellIconOverlayIdentifiers\_" + t.Name);
            Helper.FileAssociationsChanged();
        }

        #endregion


        #region IShellIconOverlayIdentifier Members

        int IShellIconOverlayIdentifier.IsMemberOf(string path, int attributes)
        {
            return OnDisplayOverlayIcon(new DisplayOverlayArgs(path,attributes)) ? S_OK : S_FALSE;
        }

        void IShellIconOverlayIdentifier.GetOverlayInfo(IntPtr iconFileBuffer, int iconFileBufferSize, out int iconIndex, out ISIOI flags)
        {
            flags = ISIOI.ISIOI_ICONFILE | ISIOI.ISIOI_ICONINDEX;
            iconIndex = IconIndex;
            WriteToIntPtr(IconFile, iconFileBuffer, iconFileBufferSize * 2);
        }

        void IShellIconOverlayIdentifier.GetPriority(out int priority)
        {
            priority = OnPriority(); // 0-100 (0 is highest priority)
        }

        #endregion

        internal static void WriteToIntPtr(string value, IntPtr destination, int bufferSize)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(value);
            int length = bytes.Length;
            if ((length + 2) > bufferSize)
            {
                length = bufferSize - 2;
            }
            Marshal.Copy(bytes, 0, destination, length);
            Marshal.WriteByte(destination, length, 0);
            Marshal.WriteByte(destination, length + 1, 0);
        }
    }

    [ComVisible(true), Guid("A901546F-10A8-4ECE-BB25-414343D0C005")]
    public interface _IconOverlayBase
    {
        [DispId(1)]
        int OnPriority();
        [DispId(2)]
        bool OnDisplayOverlayIcon(IconOverlayBase.DisplayOverlayArgs e);

    }
}
