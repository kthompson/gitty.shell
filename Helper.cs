using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Gitty.Shell
{
    internal sealed class Helper
    {
        public static void RegisterExtension(Type t, string ExtensionKey)
        {


            // Get the type ClassId
            string ClassId = GetClassId(t);

            // Unregister the class as an approved extensoin
            RegistryKey key = Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved");
            key.SetValue(ClassId, t.Name);
            key.Close();


            object[] attributes = t.GetCustomAttributes(typeof(ExtensionFileTypesAttribute), true);
            if (attributes.Length == 0)
                return;

            // Get the ExtensionFileTypes attribute
            ExtensionFileTypesAttribute fileTypes = (ExtensionFileTypesAttribute)(attributes[0]);
            key = Registry.ClassesRoot;
            foreach (string type in fileTypes.FileTypes)
            {
                RegistryKey key2 = key.CreateSubKey(type + @"\shellex\" + ExtensionKey);
                key2.SetValue(null, ClassId);
                key2.Close();
            }
        }

        public static void UnregisterExtension(Type t, string ExtensionKey)
        {

            // Get the type ClassId
            string ClassId = GetClassId(t);

            // Unregister the class as an approved extensoin
            RegistryKey key = Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Shell Extensions\Approved");
            key.DeleteValue(ClassId);
            key.Close();


            object[] attributes = t.GetCustomAttributes(typeof(ExtensionFileTypesAttribute), true);
            if (attributes.Length == 0)
                return;

            // Get the ExtensionFileTypes attribute
            ExtensionFileTypesAttribute fileTypes = (ExtensionFileTypesAttribute)(attributes[0]);

            foreach (string type in fileTypes.FileTypes)
            {
                Registry.ClassesRoot.DeleteSubKeyTree(type + @"\shellex\" + ExtensionKey);
            }


        }

        private static string GetClassId(Type t)
        {
            Object[] attributes = t.GetCustomAttributes(typeof(GuidAttribute), true);

            if (attributes.Length > 0)
                return "{" + ((GuidAttribute)attributes[0]).Value + "}";

            throw new Exception("Cannot get class id");
        }
    }
}
