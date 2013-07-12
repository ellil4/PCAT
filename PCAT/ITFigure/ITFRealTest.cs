using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveElementsIntTest.ITFigure
{
    class ITFRealTest : ITFProc
    {
        public long mRTTar = 500;
        public long mStepLen = 20;

        public bool mLastLeft;

        public bool mLastCorrect;
        public int m30Count = 0;

        private bool mFirstTime = true;

        public int mDelta = 0;
        public long mCeiling = -1, mFloor = -1;

        public List<long> mTrackedRTs;

        public ITFRealTest(PageITFigure page)
            : base(page)
        {
            mItems = new List<ITFigureItem>();
            mTrackedRTs = new List<long>();
        }

        private void resetRTTar(long trackPoint)
        {
            if (mTrackedRTs.Count % 2 == 0)
            {
                mRTTar = trackPoint + 60;
            }
            else
            {
                mRTTar = trackPoint - 60;
            }
        }

        private void finishOneTrack()
        {
            long trackPoint = (mCeiling + mFloor) / 2;
            Console.Out.WriteLine("<<" + trackPoint + ">>");
            //save
            mTrackedRTs.Add(trackPoint);
            //reset mRTTar
            resetRTTar(trackPoint);
            mStepLen = 20;
        }

        private void saveItemOps(ref ITFigureItem item)
        {
            item.ShowingTimeSpan = mRTTar;
            item.ItemDirection = true;

            mItems.Add(item);
        }

        //approaching steps
        public override void NextItem()
        {
            if (!mFirstTime)
            {
                if (mLastCorrect)
                {
                    if (mDelta == 1)//direction changed
                    {
                        mCeiling = mRTTar;
                        mFloor = mRTTar - mStepLen;

                        if (mStepLen != 5)
                        {
                            mStepLen /= 2;
                        }
                        else
                        {
                            finishOneTrack();
                        }
                    }

                    mRTTar -= mStepLen;
                    mDelta = -1;
                }
                else
                {
                    if (mDelta == -1)//direction changed
                    {
                        mCeiling = mRTTar + mStepLen;
                        mFloor = mRTTar;

                        if (mStepLen != 5)
                        {
                            mStepLen /= 2;
                        }
                        else
                        {
                            finishOneTrack();
                        }
                    }

                    mRTTar += mStepLen;
                    mDelta = 1;
                }
            }

            Console.Out.WriteLine(mRTTar);

            if (mRTTar < 5)
                mRTTar = 5;

            ITFigureItem item = new ITFigureItem();

            if (RandomBool())
            {
                mPage.ShowLeft(mRTTar);
                mLastLeft = true;

                saveItemOps(ref item);
            }
            else
            {
                mPage.ShowRight(mRTTar);
                mLastLeft = false;

                saveItemOps(ref item);
            }

            mFirstTime = false;

            m30Count++;


        }

        public long GetRT()
        {
            long retval = 0;
            for (int i = 0; i < mTrackedRTs.Count; i++)
            {
                retval += mTrackedRTs[i];
            }

            retval /= mTrackedRTs.Count;

            return retval;
        }
    }
}
