using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Gitty.Shell
{
    internal class FileNameCollection : List<string>, IDisposable
    {

        #region " API Declarations "


        [EditorBrowsable(EditorBrowsableState.Never), DllImport("shell32", EntryPoint = "DragQueryFileA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int DragQueryFile(int hDrop, int iFile, StringBuilder lpszFile, int cch);

        [EditorBrowsable(EditorBrowsableState.Never), DllImport("ole32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int ReleaseStgMedium(ref STGMEDIUM Stm);


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

                data.GetData(ref fmt, ref stm);

                max = DragQueryFile(stm.data, -1, null, 0);

                for (int i = 0; i < max; i++)
                {
                    file = new StringBuilder(260);
                    DragQueryFile(stm.data, i, file, file.Capacity);

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
