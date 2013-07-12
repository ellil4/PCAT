using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiveElementsIntTest.CtSpan
{
    public class SpaceDict
    {
        public Dictionary<int, CSPoint> mDic;
        public int mWidth, mHeight;

        private void initDict(int width, int height)
        {
            mDic.Clear();
            for (short i = 0; i < height; i++)
            {
                for (short j = 0; j < width; j++)
                {
                    mDic.Add(GetIndex(j, i), new CSPoint(j, i));
                }
            }
        }

        public SpaceDict(short width, short height)
        {
            mDic = new Dictionary<int, CSPoint>();
            mWidth = width;
            mHeight = height;
            initDict(width, height);
        }

        public void TakeRoundSpace(CSPoint pinPoint, short radius)
        {
            short horMax = (short)(pinPoint.x + (radius - 1));
            short horMin = (short)(pinPoint.x - (radius - 1));
            short verMax = (short)(pinPoint.y + (radius - 1));
            short verMin = (short)(pinPoint.y - (radius - 1));

            for(short ver = verMin; ver <= verMax; ver++)
            {
                for (short hor = horMin; hor <= horMax; hor++)
                {
                    if (isInside(pinPoint, radius, hor, ver))
                    {
                        int key = GetIndex(hor, ver);

                        if(mDic.ContainsKey(key))
                            mDic.Remove(key);
                    }
                }
            }
        }

        public int GetIndex(short x, short y)
        {
            return x + y * mWidth;
        }

        private bool isInside(CSPoint pin, short radius, short x, short y)
        {
            bool retval = false;

            if (Math.Pow((double)(pin.x - x), 2) + Math.Pow((double)(pin.y - y), 2) <
                Math.Pow(radius, 2))
            {
                retval = true;
            }

            return retval;
        }

        public void Clear()
        {
            initDict(mWidth, mHeight);
        }
    }
}
