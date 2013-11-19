using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Timers;
using System.Windows.Threading;

namespace FiveElementsIntTest.OpSpan
{
    public class SubPageDetermine
    {
        private CompDualDetermine mDualDeter;
        private PageOpSpan mPage;
        private OrganizerTrailOS mOrg;

        public SubPageDetermine(ref PageOpSpan _page, OrganizerTrailOS org)
        {
            mPage = _page;
            mOrg = org;
            mDualDeter = new CompDualDetermine();
            mDualDeter.mConfirmMethod = confirmReaction;
            mDualDeter.mDenyMethod = denyReaction;
        }

        public void Show()
        {
            mPage.mBaseCanvas.Children.Add(mDualDeter);
            Canvas.SetTop(mDualDeter, FEITStandard.PAGE_BEG_Y + 
                (FEITStandard.PAGE_HEIGHT - CompDualDetermine.OUTHEIGHT) / 2);

            Canvas.SetLeft(mDualDeter, FEITStandard.PAGE_BEG_X + 
                (FEITStandard.PAGE_WIDTH - CompDualDetermine.OUTWIDTH) / 2);

            mPage.mRecorder.choiceShowTime.Add(mPage.mTimer.GetElapsedTime());
        }

        public void setResult(string num)
        {
            mDualDeter.setResult(num);
        }

        public void setCorrectness(bool correct)
        {
            mDualDeter.setCorrectness(correct);
        }

        public void hideCorrectness(bool hide)
        {
            mDualDeter.HideCorrecteness(hide);
        }

        public void confirmReaction(CompDualDetermine self)
        {

            //mDualDeter.HideCorrecteness(!mOrg.practiseMode());
            bool choicezCorrectness = false;

            if (mOrg.currentCorrectness() == true)
            {
                mDualDeter.setCorrectness(true);
                choicezCorrectness = true;
            }
            else 
            {
                mDualDeter.setCorrectness(false);
                choicezCorrectness = false;
            }

            record(true);

            mPage.nextStep();

            recordAfterUserzPress(true.ToString(), choicezCorrectness);
        }

        //record
        private void recordAfterUserzPress(string choice, bool correctness)
        {
            mPage.mRecorder.choiceMadeTime.Add(mPage.mTimer.GetElapsedTime());
            mPage.mRecorder.choice.Add(choice);
            mPage.mRecorder.correctness.Add(correctness);
        }

        public void denyReaction(CompDualDetermine self)
        {
            //mDualDeter.HideCorrecteness(!mOrg.practiseMode());
            bool choiceCorrectness = false;

            if (mOrg.currentCorrectness() == true)
            {
                mDualDeter.setCorrectness(false);
                choiceCorrectness = false;
            }
            else
            {
                mDualDeter.setCorrectness(true);
                choiceCorrectness = true;
            }

            record(false);

            mPage.nextStep();

            recordAfterUserzPress(false.ToString(), choiceCorrectness);
        }

        private void record(bool choice)
        {
            mOrg.mAnswer.Confirm.Add(choice);
        }

        private delegate void timedele();

        private void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            mPage.Dispatcher.Invoke(DispatcherPriority.Normal, new timedele(mPage.nextStep));
        }
    }
}
