using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.DataAccess.Client;

namespace ComLibB
{
    public partial class frmBuppatEtc : Form
    {
        public frmBuppatEtc()
        {
            InitializeComponent();
        }

        public void PrintGub(string argGubun)
        {
            
            FarPoint.Win.Spread.FpSpread spd = new FarPoint.Win.Spread.FpSpread();

            if (argGubun == "1")
            {
                spd = ssList1;
            }
            else if (argGubun == "2")
            {
                spd = ssList2;
            }
            else if (argGubun == "3")
            {
                spd = ssList3;
            }

            spd.ActiveSheet.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            spd.ActiveSheet.PrintInfo.Margin.Top = 60;
            spd.ActiveSheet.PrintInfo.Margin.Bottom = 20;
            spd.ActiveSheet.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            spd.ActiveSheet.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            spd.ActiveSheet.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            spd.ActiveSheet.PrintInfo.ShowBorder = false;
            spd.ActiveSheet.PrintInfo.ShowColor = false;
            spd.ActiveSheet.PrintInfo.ShowGrid = true;
            spd.ActiveSheet.PrintInfo.ShowShadows = false;
            spd.ActiveSheet.PrintInfo.UseMax = false;
            spd.ActiveSheet.PrintInfo.UseSmartPrint = false;
            spd.ActiveSheet.PrintInfo.ShowPrintDialog = false;
            spd.ActiveSheet.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            spd.ActiveSheet.PrintInfo.Preview = false;
            spd.PrintSheet(0);

            //ssSpread = null;

            this.Close();

        }
        public void PrintBohum(string argGubun)
        {
            FarPoint.Win.Spread.FpSpread spd = new FarPoint.Win.Spread.FpSpread();

            if (argGubun == "1")
            {
                spd = ssList4;
            }
            else if (argGubun == "2")
            {
                spd = ssList5;
            }
            else if (argGubun == "3")
            {
                spd = ssList6;
            }
            else if (argGubun == "5")
            {
                spd = ssList7;
            }


            spd.ActiveSheet.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;

            if (spd == ssList4)
            {
                spd.ActiveSheet.PrintInfo.Margin.Top = 40;
            }
            else
            {
                spd.ActiveSheet.PrintInfo.Margin.Top = 60;
            }

            spd.ActiveSheet.PrintInfo.Margin.Bottom = 20;
            spd.ActiveSheet.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            spd.ActiveSheet.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            spd.ActiveSheet.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            spd.ActiveSheet.PrintInfo.ShowBorder = false;
            spd.ActiveSheet.PrintInfo.ShowColor = false;
            spd.ActiveSheet.PrintInfo.ShowGrid = true;
            spd.ActiveSheet.PrintInfo.ShowShadows = false;
            spd.ActiveSheet.PrintInfo.UseMax = false;
            spd.ActiveSheet.PrintInfo.UseSmartPrint = false;
            spd.ActiveSheet.PrintInfo.ShowPrintDialog = false;
            spd.ActiveSheet.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            spd.ActiveSheet.PrintInfo.Preview = false;
            spd.PrintSheet(0);

            //ssSpread = null;

            this.Close();

        }
    }
}
