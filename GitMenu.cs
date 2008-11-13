using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public GitContextMenu MainMenu { get; protected set; }

        protected GitMenu(GitContextMenu mainMenu)
        {
            if (mainMenu == null) throw new ArgumentNullException("mainMenu");
            MainMenu = mainMenu;
        }

        public MenuItem GetMenu()
        {
            if(ShouldDisplay(MainMenu.Context, MainMenu.Files))
                return new MenuItem(Text, OnClick);

            return null;
        }

        public virtual bool ShouldDisplay(GitContext context, ReadOnlyCollection<string> files)
        {
            foreach (var file in files)
                if (ShouldDisplay(context, file))
                    return true;

            return false;
        }

        public virtual bool ShouldDisplay(GitContext context, string file)
        {
            return false;
        }

        public virtual void PerformAction(GitContext context, ReadOnlyCollection<string> files)
        {
            foreach (var file in files)
                PerformAction(context, file);
        }

        public virtual void PerformAction(GitContext context, string file)
        {
        }

        public void OnClick(object sender, EventArgs e)
        {
            PerformAction(MainMenu.Context, MainMenu.Files);
        }

        public virtual bool CanPerform(FileSystemInfo file)
        {
            return true;
        }
    }
}
