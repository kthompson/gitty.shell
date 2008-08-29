using System;
using System.Collections.Generic;
using System.Text;

namespace Gitty.Shell
{
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class ExtensionFileTypesAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236

        public ExtensionFileTypesAttribute(string fileTypes)
        {
            this.FileTypesString = fileTypes;

        }

        public string FileTypesString { get; private set; }

        public string[] FileTypes
        {
            get
            {
                return this.FileTypesString.Split(',');
            }
        }
    }

}
