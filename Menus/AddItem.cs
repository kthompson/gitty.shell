using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gitty.Shell.Menus
{
    public class AddItem : GitMenu
    {

        public AddItem(GitContextMenu mainMenu)
            : base(mainMenu)
        {
            
        }

        public override Type Parent
        {
            get { throw new NotImplementedException(); }
        }

        public override string Text
        {
            get { throw new NotImplementedException(); }
        }
    }
}
