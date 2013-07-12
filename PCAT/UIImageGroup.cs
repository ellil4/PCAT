using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace FiveElementsIntTest
{
    public class UIImageGroup
    {
        public List<CompImage> mImages;
        public static int SELCOUNT = 8;
        private OnNextFunc mfOnNext;
        private OnSaveFunc mfOnSave;

        public delegate void OnNextFunc();
        public delegate void OnSaveFunc(int id);

        public UIImageGroup(OnSaveFunc func2, OnNextFunc func = null)
        {
            mImages = new List<CompImage>();
            for (int i = 0; i < SELCOUNT; i++)
            {
                CompImage inst = new CompImage(i);
                inst.MouseEnter += new System.Windows.Input.MouseEventHandler(inst_MouseEnter);
                inst.MouseLeave += new System.Windows.Input.MouseEventHandler(inst_MouseLeave);
                inst.MouseDown += new System.Windows.Input.MouseButtonEventHandler(inst_MouseDown);
                mImages.Add(inst);

                //inst.BorderVisiable(false);
            }

            mfOnNext = func;
            mfOnSave = func2;
        }

        public void ClearSelection()
        {
            for (int i = 0; i < SELCOUNT; i++)
            {
                mImages[i].BorderVisiable(false);
                mImages[i].isSelected = false;
            }
        }

        private void inst_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mfOnSave(((CompImage)sender).mId);
            
            if(mfOnNext != null)
                mfOnNext();

            ClearSelection();

            ((CompImage)sender).isSelected = true;
            ((CompImage)sender).BorderVisiable(true);
        }

        private void inst_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!((CompImage)sender).isSelected)
                ((CompImage)sender).BorderVisiable(false);
        }

        private void inst_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ((CompImage)sender).BorderVisiable(true);
        }
    }
}
