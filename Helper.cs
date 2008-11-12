using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Gitty.Shell
{
    internal sealed class Helper
    {
        public static void RegisterExtension(Type t, string ExtensionKey)
        {

            try
            {


                // Get the type ClassId
                string ClassId = GetClassId(t);

                // Unregister the class as an approved extensoin
                var key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", true);
                key.SetValue(ClassId, t.Name, RegistryValueKind.String);

                key.Close();


                object[] attributes = t.GetCustomAttributes(typeof (ExtensionFileTypesAttribute), true);
                if (attributes.Length == 0)
                    return;

                // Get the ExtensionFileTypes attribute
                var fileTypes = (ExtensionFileTypesAttribute) (attributes[0]);
                key = Registry.ClassesRoot;
                foreach (string type in fileTypes.FileTypes)
                {
                    RegistryKey key2 = key.CreateSubKey(type + @"\shellex\" + ExtensionKey);
                    key2.SetValue(null, ClassId);
                    key2.Close();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(string.Format("Error while registering {0}: {1}\n\n{2}", t.FullName, e, t.GUID));
                throw;
            }
        }

        public static void UnregisterExtension(Type t, string ExtensionKey)
        {
            try
            {
                // Get the type ClassId
                string ClassId = GetClassId(t);

                // Unregister the class as an approved extensoin
                try
                {
                    RegistryKey key =
                        Registry.LocalMachine.OpenSubKey(
                            @"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved", true);
                    key.DeleteValue(ClassId);
                    key.Close();
                }
                catch (ArgumentException)
                {
                }

                object[] attributes = t.GetCustomAttributes(typeof(ExtensionFileTypesAttribute), true);
                if (attributes.Length == 0)
                    return;

                // Get the ExtensionFileTypes attribute
                var fileTypes = (ExtensionFileTypesAttribute)(attributes[0]);

                foreach (string type in fileTypes.FileTypes)
                {
                    Registry.ClassesRoot.DeleteSubKeyTree(type + @"\shellex\" + ExtensionKey);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("Error while unregistering {0}: {1}", t.FullName, e));
                throw;
            }


        }

        private static string GetClassId(Type t)
        {
            Object[] attributes = t.GetCustomAttributes(typeof(GuidAttribute), true);

            if (attributes.Length > 0)
                return "{" + ((GuidAttribute)attributes[0]).Value + "}";

            throw new Exception("Cannot get class id");
        }

        [DllImport("shell32.dll")]
        private static extern void SHChangeNotify(int eventID,uint flags,IntPtr item1,IntPtr item2);

        public static void FileAssociationsChanged()
        {
            SHChangeNotify(0x08000000, 0, IntPtr.Zero, IntPtr.Zero);
        }

    }
}
