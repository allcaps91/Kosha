using System;
using System.Drawing;


namespace ComNurLibB
{
    //간호공용 폼 스프레드 자동설정
    public class clsSpdNr
    {
    
        public void SPREAD_PRINT(FarPoint.Win.Spread.SheetView Spd, FarPoint.Win.Spread.FpSpread FpSpd, string[] argHead, string[] argFont, int argLef, int argTop, int ArgPortrait = 1, bool preview = true)
        {
            FarPoint.Win.Spread.PrintInfo pi = new FarPoint.Win.Spread.PrintInfo();

            //string strHead1 = "";
            //string strHead2 = "";
            //string strFont1 = "";
            //string strFont2 = "";

            //pi.ShowColumnHeaders = true;
            //pi.ShowRowHeaders = true;

            //argFont[0] = "/fn\"바탕체\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            //argHead[0] = "/f1" + VB.Space(35) + " 약품의 재고구분 및 보관";

            //argFont[1] = "/fn\"바탕체\" /fz\"10\" /fb1 /fi0 /fu0 /fk0 /fs2";
            //argHead[1] = "/f2" + "인쇄일자 : " + DateTime.Now.ToString("yyyy-MM-dd");

            //SS1.PrintHeader = strFont1 + strHead1 + "/n/n" + strFont2 + strHead2



            //strHead1 = "/c/f1" + "퇴원예고자 List" + "/f1/n/n";
            //strHead2 = "/n/l/f2" + "퇴원일자 : " + Strings.Format(Conversion.Val(strOrdDate), "####년##월##일") + " /n/l/f2" + "출력시간 : " + strPrintTime.Substring(0, 2) + "시" + strPrintTime.Substring(2, 2) + "분" + " /r/f2" + "출력자 : " + clsType.gUseInfo.strUseName + "     /n";

            ////Print Head 지정
            //strFont1 = "/fn\"바탕체\" /fz\"14\" /fb1 /fi0 /fu0 /fk0 /fs1";
            //strFont2 = "/fn\"바탕체\" /fz\"12\" /fb0 /fi0 /fu0 /fk0 /fs2";


            string str = "";

            if (argFont.Length > 0)
            {
                for (int i = 0; i < argFont.Length; i++)
                {
                    str = str + argFont[i] + argHead[i];
                }

                //str = argFont[0] + argHead[0] +  argFont[1] + argHead[1];

                //pi.HeaderHeight = 50;
                pi.Header = str;


            }

            pi.Margin.Top = argTop;
            pi.Margin.Left = argLef;

            pi.Margin.Bottom = 10;
            pi.Margin.Right = 10;

            pi.ShowGrid = false; ;
            pi.ShowBorder = true;
            pi.ShowShadows = true;
            pi.ShowColor = true;
            //pi.UseMax = true;
            //pi.BestFitCols = true;
            if (ArgPortrait == 1)
            {
                pi.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            }
            else
            {
                pi.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            }
            pi.UseSmartPrint = false;
            pi.Preview = preview;

            Spd.PrintInfo = pi;

            FpSpd.PrintSheet(0);

        }

    }
}
