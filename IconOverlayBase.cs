using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace Gitty.Shell
{
    [ComVisible(false)]
    public abstract class IconOverlayBase : IShellIconOverlayIdentifier
    {
        public class SelectOverlayArgs : CancelEventArgs
        {
            public SelectOverlayArgs()
            {
            }

            public SelectOverlayArgs(string file, int index)
            {
                IconFile = file;
                IconIndex = index;
            }

            public string IconFile { get; set; }
            public int IconIndex { get; set; }
        }

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

        protected virtual void OnSelectOverlay(SelectOverlayArgs e)
        {
        }

        protected virtual int OnPriority()
        {
            return 0;
        }

        protected virtual bool OnDisplayOverlayIcon(DisplayOverlayArgs e)
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
            return OnDisplayOverlayIcon(new DisplayOverlayArgs(path,attributes)) ? 0 : 1;
        }

        int IShellIconOverlayIdentifier.GetOverlayInfo(out string iconFileBuffer, int iconFileBufferSize, out int iconIndex, out ISIOI flags)
        {
            var e = new SelectOverlayArgs();

            OnSelectOverlay(e);

            flags = ISIOI.ISIOI_ICONFILE | ISIOI.ISIOI_ICONINDEX;
            iconIndex = e.IconIndex;
            iconFileBuffer = e.IconFile;
            
            return e.Cancel ? 1 : 0;
        }

        int IShellIconOverlayIdentifier.GetPriority(out int priority)
        {
            priority = OnPriority(); // 0-100 (0 is highest priority)
            return 0; // S_OK
        }

        #endregion
    }
    
 
}
