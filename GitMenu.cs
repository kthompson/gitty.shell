using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gitty.Shell
{
    public abstract class GitMenu
    {
        public abstract Type Parent { get; }
        public abstract string Text { get; }

        public MenuItem GetMenu()
        {
            var menu = new MenuItem(Text, OnClick);

            return menu;
        }

        public virtual void OnClick(object sender, EventArgs e)
        {
            
        }

        public virtual bool CanPerform(FileSystemInfo file)
        {
            return true;
        }
    }
}
