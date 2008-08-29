using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Gitty.Shell
{
    internal class FileNameCollection : List<string>, IDisposable
    {

        #region " API Declarations "


        [DllImport("shell32.dll")]
        private static extern int DragQueryFile(IntPtr hDrop, int iFile, [Out] StringBuilder lpszFile, int cch);

        [DllImport("ole32.dll")]
        private static extern int ReleaseStgMedium([In] ref STGMEDIUM pmedium);

        #endregion

        #region " Constructors "

        public FileNameCollection(IDataObject data)
            : base()
        {
            FORMATETC fmt = new FORMATETC();
            STGMEDIUM stm = new STGMEDIUM();

            fmt.cfFormat = 15;//CF_HDROP
            fmt.tymed = TYMED.TYMED_HGLOBAL;
            fmt.dwAspect = DVASPECT.DVASPECT_CONTENT;

            try
            {
                int max;
                StringBuilder file;

                data.GetData(ref fmt, out stm);

                max = DragQueryFile(stm.unionmember, -1, null, 0);

                for (int i = 0; i < max; i++)
                {
                    file = new StringBuilder(260);
                    DragQueryFile(stm.unionmember, i, file, file.Capacity);

                    this.Add(file.ToString());
                }

            }
            finally
            {
                ReleaseStgMedium(ref stm);
            }


        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            this.Clear();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
