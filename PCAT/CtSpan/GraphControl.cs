using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace FiveElementsIntTest.CtSpan
{
    public class GraphControl 
    {
        public PageCtSpan mPage;
        public int mTargetID = 0;
        public int mInterCircleID = 0;
        public int mInterTriangleID = 0;

        public SpaceDict mSpaceDict;

        public List<CtReacTapeSeg> mCountingTape;

        List<GraphicToken> mTarElem;
        List<GraphicToken> mInterElem;

        public static int SUBAREA_BEG_X =
            ((int)System.Windows.SystemParameters.PrimaryScreenWidth) / 2
            - 640 / 2;

        public static int SUBAREA_BEG_Y =
            ((int)System.Windows.SystemParameters.PrimaryScreenHeight) / 2
            - 480 / 2 - 50;

        private Random mRDM;

        public GraphControl(PageCtSpan page)
        {
            mPage = page;

            mCountingTape = new List<CtReacTapeSeg>();
            mRDM = new Random();
            mSpaceDict = new SpaceDict(640, 480);
            mTarElem = new List<GraphicToken>();
            mInterElem = new List<GraphicToken>();
        }

        public void DotOnTape(int X, int Y, TokenType type, long spotOfTime)
        {
            mCountingTape.Add(new CtReacTapeSeg(X, Y, type, spotOfTime));
        }

        private GraphicToken draw(int x, int y, TokenType type)
        {
            GraphicToken gt = new GraphicToken(type, this);
            mPage.amBaseCanvas.Children.Add(gt);
            Canvas.SetTop(gt, SUBAREA_BEG_Y + y - gt.HALF_HEIGHT);
            Canvas.SetLeft(gt, SUBAREA_BEG_X + x - gt.HALF_WIDHT);

            return gt;
        }

        public void DrawTarget(int x, int y)
        {
            GraphicToken gt = draw(x, y, TokenType.DARKCIRCLE);
            mTarElem.Add(gt);
            gt.SetPos(x, y);
            mTargetID++;
        }

        public void DrawInterCircle(int x, int y)
        {
            GraphicToken gt = draw(x, y, TokenType.LIGHTCIRCLE);
            mInterElem.Add(gt);
            gt.SetPos(x, y);
            mInterCircleID++;
        }

        public void DrawInterTriangle(int x, int y)
        {
            GraphicToken gt = draw(x, y, TokenType.TRIANGLE);
            mInterElem.Add(gt);
            gt.SetPos(x, y);
            mInterTriangleID++;
        }

        private CSPoint getPoint(CSPointType type)
        {
            CSPoint retval = new CSPoint();
            int limitxMin = 0, limitxMax = 0, limityMin = 0, limityMax = 0;

            switch (type)
            {
                case CSPointType.FirstQuad:
                    limitxMin = 320;
                    limitxMax = 640;
                    limityMin = 0;
                    limityMax = 240;
                    break;
                case CSPointType.SecondQuad:
                    limitxMin = 0;
                    limitxMax = 320;
                    limityMin = 0;
                    limityMax = 240;
                    break;
                case CSPointType.ThirdQuad:
                    limitxMin = 0;
                    limitxMax = 320;
                    limityMin = 240;
                    limityMax = 480;
                    break;
                case CSPointType.AllQuad:
                    limitxMin = 0;
                    limitxMax = 640;
                    limityMin = 0;
                    limityMax = 480;
                    break;
            }

            retval.x = (short)mRDM.Next(limitxMin, limitxMax);
            retval.y = (short)mRDM.Next(limityMin, limityMax);

            return retval;
        }

        /*private double getDistance(CSPoint point1, CSPoint point2)
        {
            return Math.Sqrt(
                Math.Pow((point1.x - point2.x), 2) + 
                Math.Pow((point1.y - point2.y), 2));
        }*/

        /*private bool within(CSPoint point, List<CSPoint> pointGroup, double distance)
        {
            bool retval = false;

            for (int i = 0; i < pointGroup.Count; i++)
            {
                if (getDistance(point, pointGroup[i]) < distance)
                {
                    retval = true;
                    break;
                }
            }

            return retval;
        }*/

        public bool GetCountingCorrectness()
        {
            bool retval = true;
            
            for (int i = 0; i < mTarElem.Count; i++)
            {
                if (mTarElem[i].mClickCount != 1)
                {
                    retval = false;
                    break;
                }
            }

            for (int i = 0; i < mInterElem.Count; i++)
            {
                if (mInterElem[i].mClickCount > 0)
                {
                    retval = false;
                    break;
                }
            }

            return retval;
        }

        public void DrawScene(StGraphItem item)
        {
            DrawScene(item.TarCount, item.InterCircleCount, item.InterTriCount,
                item.DistanceTar, item.DistanceComm);
        }

        public void DrawScene(int tarCount, int interCirCount, int interTriCount, 
            short disTar, short disComm)
        {
            while (true)
            {
                //assert
                if (tarCount < 3 || interCirCount < 0 ||
                    interTriCount < 0 || disTar < 0.0 || disComm < 0.0)
                {
                    return;
                }

                Clear();

                //first 3 target
                CSPoint pTemp = getOutRangePoint(CSPointType.FirstQuad, disTar);
                if (pTemp == null)
                    continue;

                DrawTarget(pTemp.x, pTemp.y);

                pTemp = getOutRangePoint(CSPointType.SecondQuad, disTar);
                if (pTemp == null)
                    continue;

                DrawTarget(pTemp.x, pTemp.y);

                pTemp = getOutRangePoint(CSPointType.ThirdQuad, disTar);
                if (pTemp == null)
                    continue;

                DrawTarget(pTemp.x, pTemp.y);

                //other targets
                for (int i = 0; i < tarCount - 3; i++)
                {
                    pTemp = getOutRangePoint(CSPointType.AllQuad, disTar);
                    if (pTemp == null)
                        break;
                    
                    DrawTarget(pTemp.x, pTemp.y);
                }
                if (pTemp == null)
                    continue;

                //inter triangle
                for (int i = 0; i < interTriCount; i++)
                {
                    pTemp = getOutRangePoint(CSPointType.AllQuad, disComm);
                    if (pTemp == null)
                        break;

                    DrawInterTriangle(pTemp.x, pTemp.y);
                }
                if (pTemp == null)
                    continue;

                //inter circle
                for (int i = 0; i < interCirCount; i++)
                {
                    pTemp = getOutRangePoint(CSPointType.AllQuad, disComm);
                    if (pTemp == null)
                        break;

                    DrawInterCircle(pTemp.x, pTemp.y);
                }
                if (pTemp == null)
                    continue;

                break;
            }
        }

        private CSPoint getOutRangePoint(CSPointType type, short radius)
        {
            CSPoint pTemp = null;
            int dicMax = mSpaceDict.mDic.Count;
            int randomPointPos = -1;

            if (mSpaceDict.mDic.Count != 0)//when no space left, redo in the DrawScene()
            {
                if (type != CSPointType.AllQuad)
                {
                    while (pTemp == null ||
                        !mSpaceDict.mDic.ContainsKey(mSpaceDict.GetIndex(pTemp.x, pTemp.y)))
                    {
                        pTemp = getPoint(type);
                    }
                }
                else
                {
                    randomPointPos = mRDM.Next(0, dicMax);
                    pTemp = mSpaceDict.mDic.ElementAt(randomPointPos).Value;
                }

                mSpaceDict.TakeRoundSpace(pTemp, radius);
            }

            if (pTemp == null)
                Console.WriteLine("Rearrange");

            return pTemp;
        }

        public String GetTokenTypeString(TokenType type)
        {
            String retval = "";
            switch (type)
            {
                case TokenType.DARKCIRCLE:
                    retval = "Tgt";
                    break;
                case TokenType.LIGHTCIRCLE:
                    retval = "Shp";
                    break;
                case TokenType.TRIANGLE:
                    retval = "Clr";
                    break;
            }

            return retval;
        }

        public String GetTapeTypecall()
        {
            String retval = "";

            for (int i = 0; i < mCountingTape.Count; i++)
            {
                retval += GetTokenTypeString(mCountingTape[i].Type) + ",";
            }

            return retval;
        }

        public String GetTapePositioncall()
        {
            String retval = "()";

            for (int i = 0; i < mCountingTape.Count; i++)
            {
                retval += "(" + mCountingTape[i].X + "," + mCountingTape[i].Y + ")";
            }

            return retval;
        }

        public void Clear()
        {
            mPage.ClearAll();
            mSpaceDict.Clear();
            mTargetID = 0;
            mInterCircleID = 0;
            mInterTriangleID = 0;
            mCountingTape.Clear();
            mTarElem.Clear();
            mInterElem.Clear();
        }
    }
}
