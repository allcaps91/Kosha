using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.ManagedDataAccess.Client;

namespace ComLibB
{
    public partial class frmBuppatOP : Form
    {
        public frmBuppatOP()
        {
            InitializeComponent();
        }

        public void Print1(string argPano, string argSex, string argAge, string argSname, string argGubun)
        {
            FarPoint.Win.Spread.FpSpread ssSpread = null;
            switch (argGubun)
            {
                case "1":
                    ssSpread = ssOP1;
                    break;
                case "2":
                    ssSpread = ssOP2;
                    break;
                case "3":
                    ssSpread = ssOP3;
                    break;
                case "4":
                    ssSpread = ssOP4;
                    break;
                case "5":
                    ssSpread = ssOP5;
                    break;
            }

            ssSpread.ActiveSheet.Cells[0, 5].Text = argPano;
            ssSpread.ActiveSheet.Cells[1, 5].Text = argSex + "/" + argAge;
            ssSpread.ActiveSheet.Cells[2, 5].Text = argSname;

            ssSpread.ActiveSheet.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssSpread.ActiveSheet.PrintInfo.Margin.Top = 60;
            ssSpread.ActiveSheet.PrintInfo.Margin.Bottom = 20;
            ssSpread.ActiveSheet.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssSpread.ActiveSheet.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssSpread.ActiveSheet.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssSpread.ActiveSheet.PrintInfo.ShowBorder = false;
            ssSpread.ActiveSheet.PrintInfo.ShowColor = false;
            ssSpread.ActiveSheet.PrintInfo.ShowGrid = true;
            ssSpread.ActiveSheet.PrintInfo.ShowShadows = false;
            ssSpread.ActiveSheet.PrintInfo.UseMax = false;
            ssSpread.ActiveSheet.PrintInfo.UseSmartPrint = false;
            ssSpread.ActiveSheet.PrintInfo.ShowPrintDialog = false;
            ssSpread.ActiveSheet.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssSpread.ActiveSheet.PrintInfo.Preview = false;
            ssSpread.PrintSheet(0);

            //ssSpread = null;
            
            this.Close();

        }

    }
}
