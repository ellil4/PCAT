using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FiveElementsIntTest.SybSrh
{
    class TypeAndIndex
    {
        public int type;
        public int index;
    }

    public class SybSrhItemGenerator
    {
        private Random mRDM;
        private SybSrhSourceFetcher mFetcher;

        public SybSrhItemGenerator()
        {
            mRDM = new Random();
            mFetcher = new SybSrhSourceFetcher(PageSybSrh.RESPATH);
        }

        private List<SybSrhItem> gen1PosNegByHalf()
        {
            List<SybSrhItem> retval = new List<SybSrhItem>();
            int typeTemp = -1;
            int posTarTemp = -1;
            int posSelectionTemp = -1;
            int picIndexTemp = -1;

            for (int i = 0; i < 12; i++)
            {
                typeTemp = mRDM.Next(0, 10);
                picIndexTemp = mRDM.Next(0, mFetcher.GetTypeElemCount(typeTemp));
                Bitmap bmp =
                    mFetcher.GetPic(typeTemp, picIndexTemp);

                posTarTemp = mRDM.Next(0, 2);
                posSelectionTemp = mRDM.Next(0, 5);

                SybSrhItem item = new SybSrhItem();
                SybSrhVisualElem target = new SybSrhVisualElem();
                target.BMP = bmp;
                target.Type = typeTemp;
                target.Index = picIndexTemp;
                
                //item.Targets[posTarTemp] = bmp;
                //item.Info.TargetsTypes[posTarTemp] = typeTemp;//save info
                //item.Info.TargetsPicIndex[posTarTemp] = picIndexTemp;

                if (i < 6)//one target has a same selection
                {
                    //mark as true
                    target.IfTrue = true;
                    //selections
                    SybSrhVisualElem selection = new SybSrhVisualElem();
                    selection.BMP = bmp;
                    selection.IfTrue = true;
                    selection.Type = typeTemp;
                    selection.Index = picIndexTemp;
                    item.Selection[posSelectionTemp] = selection;
                }

                item.Target[posTarTemp] = target;

                retval.Add(item);
            }

            return retval;
        }

        private SybSrhItem randomPickout(ref List<SybSrhItem> source)
        {
            SybSrhItem retval = null;

            int pos2Get = -1;
            if (source.Count != 0)
            {
                pos2Get = mRDM.Next(0, source.Count);
                retval = source[pos2Get];
                source.RemoveAt(pos2Get);
            }

            return retval;
        }

        private bool targetSameType(SybSrhItem item)
        {
            bool retval = false;

            if (item.Target[0].Type == item.Target[1].Type)
                retval = true;

            return retval;
        }

        public static bool hasTrueTarget(SybSrhItem item)
        {
            bool retval = false;

            if (item.GetTrueTarIdx() != -1)
                retval = true;

            return retval;
        }

        private void randomFillSelectionNoOverwrite(
            ref SybSrhItem item, TypeAndIndex[] t0i)
        {
            int[] expList = new int[1];
            expList[0] = item.GetTrueSelectionIdx();
            int[] order = randomPickElemNoRepeat(t0i.Length, 5, expList);
            for (int i = 0; i < order.Length; i++)
            {
                SybSrhVisualElem elem = new SybSrhVisualElem();
                elem.BMP = mFetcher.GetPic(t0i[i].type, t0i[i].index);
                elem.Type = t0i[i].type;
                elem.Index = t0i[i].index;

                item.Selection[order[i]] = elem;
            }
        }

        private bool containsSameElem(int tar, int[] exception)
        {
            bool retval = false;
            for (int i = 0; i < exception.Length; i++)
            {
                if (tar == exception[i])
                {
                    retval = true;
                    break;
                }
            }
            return retval;
        }

        private int[] randomPickElemNoRepeat(int genCount, int cogSize, int[] exceptList)
        {
            int[] retval = new int[genCount];
            for (int j = 0; j < genCount; j++)
            {
                retval[j] = -1;
            }

            int pickedElemCount = 0;

            while (true)
            {
                int pickedIndex = mRDM.Next(0, cogSize);

                if (!containsSameElem(pickedIndex, exceptList))//check if same as exception
                {
                    for (int i = 0; i < genCount; i++)//check if repeat
                    {
                        if (retval[i] == pickedIndex)
                            break;

                        if (i == genCount - 1)
                        {
                            retval[pickedElemCount] = pickedIndex;
                            pickedElemCount++;
                        }
                    }
                }

                if (pickedElemCount == genCount)
                    break;
            }

            return retval;
        }

        private TypeAndIndex[] Join2ArraysOfT0I(
            TypeAndIndex[] listA, TypeAndIndex[] listB)
        {
            TypeAndIndex[] retval = new TypeAndIndex[listA.Length + listB.Length];

            for (int i = 0; i < listA.Length; i++)
            {
                retval[i] = listA[i];
            }

            for (int j = listA.Length, bi = 0; j < listA.Length + listB.Length; j++, bi++)
            {
                retval[j] = listB[bi];
            }

            return retval;
        }

        private bool coin()
        {
            if (mRDM.Next(0, 2) == 0)
                return false;
            else
                return true;
        }

        private TypeAndIndex[] genElemsOfType(int type, int[] exceptionIndex, int count)
        {
            TypeAndIndex[] retval = new TypeAndIndex[count];

            int[] index = 
                randomPickElemNoRepeat(
                count, mFetcher.GetTypeElemCount(type), exceptionIndex);

            for (int i = 0; i < count; i++)
            {
                TypeAndIndex t0i = new TypeAndIndex();
                t0i.type = type;
                t0i.index = index[i];
                retval[i] = t0i;
            }
            
            return retval;
        }

        private TypeAndIndex[] gen1ElemsFromEachType(int[] exceptionTypes, int count)
        {
            TypeAndIndex[] retval = new TypeAndIndex[count];

            int[] types = randomPickElemNoRepeat(count, 10, exceptionTypes);
            int[] index = new int[count];

            for (int i = 0; i < count; i++)
            {
                index[i] = mRDM.Next(0, mFetcher.GetTypeElemCount(types[i]));
            }

            for (int j = 0; j < count; j++)
            {
                TypeAndIndex t0i = new TypeAndIndex();
                t0i.type = types[j];
                t0i.index = index[j];
                retval[j] = t0i;
            }

            return retval;
        }

        private SybSrhItem make234Types(SybSrhItem item, int typeCount)
        {
            int trueSelIdx = -1;
            if (targetSameType(item))
            {
                //determine how many same type 2 random
                int abCount = -1;
                trueSelIdx = item.GetTrueSelectionIdx();
                if (trueSelIdx != -1)//has true sel
                {
                    switch (typeCount)
                    {
                        case 2:
                            abCount = 3;
                            break;
                        case 3:
                            abCount = mRDM.Next(1, 3);
                            break;
                        case 4:
                            abCount = mRDM.Next(0, 2);
                            break;
                    }
                }
                else//has no true sel
                {
                    switch(typeCount)
                    {
                        case 2:
                            abCount = 4;
                            break;
                        case 3:
                            abCount = mRDM.Next(2, 4);
                            break;
                        case 4:
                            abCount = mRDM.Next(1, 3);
                            break;
                    }
                }

                //build fill in index
                TypeAndIndex[] t0i = null;
                //random same type ones
                int[] exceptionIndex = new int[2];
                exceptionIndex[0] = item.Target[0].Index;
                exceptionIndex[1] = item.Target[1].Index;
                TypeAndIndex[] sameTypeOnes = genElemsOfType(item.Target[0].Type, exceptionIndex, abCount);
                //random diff type ones
                int diffTypeCount = -1;
                switch(typeCount)
                {
                    case 2:
                        diffTypeCount = 1;
                        break;
                    case 3:
                        diffTypeCount = 2;
                        break;
                    case 4:
                        diffTypeCount = 3;
                        break;
                }

                TypeAndIndex[] diffTypeOnes = gen1ElemsFromEachType(new int[1] { item.Target[0].Type }, diffTypeCount);
                //combine and fill
                t0i = Join2ArraysOfT0I(sameTypeOnes, diffTypeOnes);
                randomFillSelectionNoOverwrite(ref item, t0i);

                //one supplementary
                for (int i = 0; i < item.Selection.Length; i++)
                {
                    if (item.Selection[i] == null)
                    {
                        TypeAndIndex[] oneSupp =
                            genElemsOfType(diffTypeOnes[0].type,
                            new int[1] { diffTypeOnes[0].index }, 1);
                        
                        SybSrhVisualElem ssvi = new SybSrhVisualElem();

                        ssvi.Type = oneSupp[0].type;
                        ssvi.Index = oneSupp[0].index;
                        ssvi.BMP = mFetcher.GetPic(oneSupp[0].type, oneSupp[0].index);
                        item.Selection[i] = ssvi;
                    }
                }


            }
            else
            {
                //get count
                int aCount = 0, bCount = 0, randomCount = 0;
                switch (typeCount)
                {
                    case 2:
                        aCount = mRDM.Next(2, 4);
                        bCount = 5 - aCount;
                        break;
                    case 3:
                        aCount = mRDM.Next(2, 4);
                        bCount = mRDM.Next(0, 5 - aCount);
                        randomCount = 5 - aCount - bCount;
                        break;
                    case 4:
                        aCount = mRDM.Next(1, 3);
                        bCount = mRDM.Next(1, 4 - aCount);
                        randomCount = 5 - aCount - bCount;
                        break;
                }
                //get if has true
                int trueTarIdx = item.GetTrueTarIdx();
                if (trueTarIdx != -1)
                {
                    if (trueTarIdx == 0)
                    {
                        aCount--;
                    }
                    else
                    {
                        if (bCount > 0)
                        {
                            bCount--;
                        }
                        else
                        {
                            randomCount--;
                        }
                    }
                }

                //gen, join & fill
                TypeAndIndex[] aTA = genElemsOfType(item.Target[0].Type, 
                    new int[1]{item.Target[0].Index}, aCount);
                TypeAndIndex[] bTA = genElemsOfType(item.Target[1].Type, 
                    new int[1]{item.Target[1].Index}, bCount);
                TypeAndIndex[] rTA = gen1ElemsFromEachType(
                    new int[2]{item.Target[0].Type, item.Target[0].Type}, 
                    randomCount);
                TypeAndIndex[] a0b = Join2ArraysOfT0I(aTA, bTA);
                TypeAndIndex[] all = Join2ArraysOfT0I(a0b, rTA);
                randomFillSelectionNoOverwrite(ref item, all);

                ////undefined
                //for (int i = 0; i < item.Selection.Length; i++)
                //{
                //    if (item.Selection[i] == null)
                //    {
                //        int a = 0;
                //    }
                //}
            }

            return item;
        }

        private List<SybSrhItem> gen2TragetsSmaeTypeByHalf(List<SybSrhItem> source)
        {
            List<SybSrhItem> retval = new List<SybSrhItem>();

            for (int i = 0; i < 12; i++)
            {
                SybSrhItem item = randomPickout(ref source);
                int oriTarType = -1;
                int oriTarIndex = -1;
                int oriTarAt = -1;
                int fillTarAt = -1;
                int typeElemCount = -1;
                int newTarIndex = -1;

                //to get which to fill
                if (item.Target[0] != null)
                {
                    oriTarAt = 0;
                    fillTarAt = 1;
                }
                else
                {
                    oriTarAt = 1;
                    fillTarAt = 0;
                }

                oriTarType = item.Target[oriTarAt].Type;
                oriTarIndex = item.Target[oriTarAt].Index;
                typeElemCount = mFetcher.GetTypeElemCount(oriTarType);

                if (i < 6)//from same type
                {
                    while (newTarIndex == -1 || newTarIndex == oriTarIndex)
                    {
                        newTarIndex = mRDM.Next(0, typeElemCount);
                    }

                    SybSrhVisualElem tar = new SybSrhVisualElem();
                    tar.BMP = mFetcher.GetPic(oriTarType, newTarIndex);
                    tar.Index = newTarIndex;
                    tar.Type = oriTarType;
                    item.Target[fillTarAt] = tar;
                }
                else//form different type
                {
                    int newTarType = -1;
                    while (newTarType == -1 || newTarType == oriTarType)
                    {
                        newTarType = mRDM.Next(0, 10);
                    }

                    newTarIndex = mRDM.Next(0, mFetcher.GetTypeElemCount(newTarType));

                    SybSrhVisualElem tar = new SybSrhVisualElem();
                    tar.BMP = mFetcher.GetPic(newTarType, newTarIndex);
                    tar.Index = newTarIndex;
                    tar.Type = newTarType;
                    item.Target[fillTarAt] = tar;
                }

                retval.Add(item);
            }

            return retval;
        }

        public List<SybSrhItem> Get12Items()
        {
            List<SybSrhItem> List1 = null;
            while (List1 == null)
            {
                //step 1
                List1 = gen1PosNegByHalf();
                List<SybSrhItem> List2 = new List<SybSrhItem>();
                for (int i = 0; i < 12; i++)
                {
                    List2.Add(randomPickout(ref List1));
                }
                //step2
                List2 = gen2TragetsSmaeTypeByHalf(List2);
                List1.Clear();
                for (int i = 0; i < 12; i++)
                {
                    List1.Add(randomPickout(ref List2));
                }
                //step3
                List2.Clear();
                for (int i = 0; i < 12; i++)
                {
                    if (i < 2)
                    {
                        List2.Add(make234Types(List1[i], 2));
                    }
                    else if (i >= 2 && i < 8)
                    {
                        List2.Add(make234Types(List1[i], 3));
                    }
                    else
                    {
                        List2.Add(make234Types(List1[i], 4));
                    }
                }
                List1.Clear();
                for (int i = 0; i < 12; i++)
                {
                    List1.Add(randomPickout(ref List2));
                }

                //String outS = "origin: ";
                //for (int z = 0; z < List1.Count; z++)
                //{
                //    if (hasTrueTarget(List1[z]))
                //    {
                //        outS += "o";
                //    }
                //    else
                //    {
                //        outS += "x";
                //    }
                //}
                //Console.WriteLine(outS);

                List1 = rearrangeSame(List1);
            }

            return List1;
        }

        private int get4sameStartIdx(List<SybSrhItem> items)
        {
            int lastType = -1;//0 has not; 1 has; -1 not given value
            int count = 0;
            int start = -1;
            for (int i = 0; i < items.Count; i++)
            {
                if (hasTrueTarget(items[i]))
                {
                    if (lastType == 1)//same
                    {
                        count++;
                    }
                    else//diff
                    {
                        lastType = 1;
                        count = 0;
                    }
                }
                else
                {
                    if (lastType == 0)
                    {
                        count++;
                    }
                    else
                    {
                        lastType = 0;
                        count = 0;
                    }
                }

                if (count == 2)
                {
                    start = i - 2;
                    break;
                }
            }
            
            return start;
        }

        private List<SybSrhItem> makeHeadTailDiff(List<SybSrhItem> items)
        {
            //head same
            bool noBreak = true;
            int noBreakCount = 0;
            while (noBreak)
            {
                noBreak = false;
                if (hasTrueTarget(items[0]) == hasTrueTarget(items[1]))
                {
                    noBreak = true;
                }
                //tail same
                if (hasTrueTarget(items[10]) == hasTrueTarget(items[11]))
                {
                    noBreak = true;
                }
                //head 2 tail
                if (noBreak)
                {
                    SybSrhItem item = items[0];
                    items.RemoveAt(0);
                    items.Add(item);
                    noBreakCount++;
                    if (noBreakCount >= 6)
                    {
                        noBreak = false;
                    }
                }
            }

            //String outS = "headT: ";
            //for (int z = 0; z < items.Count; z++)
            //{
            //    if (hasTrueTarget(items[z]))
            //    {
            //        outS += "o";
            //    }
            //    else
            //    {
            //        outS += "x";
            //    }
            //}
            //Console.WriteLine(outS);

            return items;
        }

        private List<SybSrhItem> rearrangeSame(List<SybSrhItem> items)
        {
            int startIdx = 0;
            bool arrType;
            int idx2pick = -1;
            int rearrangeCount = 0;
            while ((startIdx = get4sameStartIdx(items)) != -1)
            {
                arrType = hasTrueTarget(items[startIdx]);//get same`s type
                List<SybSrhItem> sameArr = items.GetRange(startIdx, 3);
                items.RemoveRange(startIdx, 3);

                for (int i = 0; i < items.Count; i++)
                {
                    if (hasTrueTarget(items[i]) != arrType)
                    {
                        idx2pick = i;
                        break;
                    }
                }

                sameArr.Insert(1, items[idx2pick]);
                items.RemoveAt(idx2pick);
                items.AddRange(sameArr);

                //String outS = "block: ";
                //for (int z = 0; z < items.Count; z++)
                //{
                //    if(hasTrueTarget(items[z]))
                //    {
                //        outS += "o";
                //    }
                //    else
                //    {
                //        outS += "x";
                //    }
                //}
                //Console.WriteLine(outS);

                items = makeHeadTailDiff(items);
                rearrangeCount++;

                if (rearrangeCount > 6)
                {
                    items = null;
                    break;
                }
            }
            return items;
        }
    }
}
