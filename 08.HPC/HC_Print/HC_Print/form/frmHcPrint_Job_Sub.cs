using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_Print
{
    public partial class frmHcPrint_Job_Sub : Form
    {

        long fnWrtno = 0;

        ComFunc cf = new ComFunc();
        clsHaBase hb = new clsHaBase();
        clsSpread cSpd = new clsSpread();

        HicJepsuPatientService hicJepsuPatientService = null;
        HicResultService hicResultService = null;
        HicResEtcService hicResEtcService = null;
        HicJepsuService hicJepsuService = null;


        public frmHcPrint_Job_Sub()
        {
            InitializeComponent();
        }

        public frmHcPrint_Job_Sub(long argWRTNO)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            fnWrtno = argWRTNO;
        }
        private void SetControl()
        {
            hicResultService = new HicResultService();
            hicJepsuPatientService = new HicJepsuPatientService();
            hicResEtcService = new HicResEtcService();
            hicJepsuService = new HicJepsuService();
        }
        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
        }

        private void eFormload(object sender, EventArgs e)
        {
            this.Hide();

            Result_Print_Sub();
            Result_Print_Update();

            ComFunc.Delay(1500);
            this.Close();

        }

        private void Result_Print_Sub()
        {
            Result_Print_Clear();       //Sheet Clear
            Result_Print();

        }

        private void Result_Print_Clear()
        {
            //인적사항
            SS1.ActiveSheet.Cells[6, 4].Text = "";
            SS1.ActiveSheet.Cells[6, 9].Text = "";
            SS1.ActiveSheet.Cells[7, 4].Text = "";
            SS1.ActiveSheet.Cells[7, 9].Text = "";
            SS1.ActiveSheet.Cells[8, 4].Text = "";

            //검진 및 팔정일
            SS1.ActiveSheet.Cells[12, 4].Text = "";
            SS1.ActiveSheet.Cells[12, 6].Text = "";
            SS1.ActiveSheet.Cells[12, 8].Text = "";
            SS1.ActiveSheet.Cells[12, 10].Text = "";
            SS1.ActiveSheet.Cells[13, 4].Text = "";
            SS1.ActiveSheet.Cells[13, 6].Text = "";
            SS1.ActiveSheet.Cells[13, 8].Text = "";
            SS1.ActiveSheet.Cells[13, 10].Text = "";

            //면허번호, 진단의사, 발급일
            SS1.ActiveSheet.Cells[21, 3].Text = "";
            SS1.ActiveSheet.Cells[21, 8].Text = "";
            SS1.ActiveSheet.Cells[22, 6].Text = "";

            //검진항목 및 결과
            //for (int i = 8; i < 26; i = i + 2)
            //{
            //    SS1.ActiveSheet.Cells[i, 15].Text = "";
            //}

            for (int i = 8; i < 26; i = i + 2)
            {
                for (int j = 18; j < 25; j = j + 2)
                {
                    SS1.ActiveSheet.Cells[i, j].Text = "";
                }
            }

            //비고
            SS1.ActiveSheet.Cells[28, 17].Text = "";
        }

        private void Result_Print()
        {
            HIC_JEPSU_PATIENT item = hicJepsuPatientService.GetItembyWrtNo(fnWrtno);

            SS1.ActiveSheet.Cells[6, 4].Text = VB.Right(item.GJYEAR,2) + "-     "; //일련번호
            SS1.ActiveSheet.Cells[7, 4].Text = item.SNAME; //성명
            SS1.ActiveSheet.Cells[8, 4].Text = item.HPHONE; //연락처

            SS1.ActiveSheet.Cells[6, 9].Text = fnWrtno.ToString(); //접수번호
            SS1.ActiveSheet.Cells[7, 9].Text = VB.Mid(item.JUMIN,1,6) + "-" + VB.Mid(item.JUMIN, 7,7); //주민번호

            SS1.ActiveSheet.Cells[12, 4].Text = item.JEPDATE;
            SS1.ActiveSheet.Cells[13, 4].Text = Convert.ToDateTime(item.PANJENGDATE.ToString()).ToShortDateString();

            List<HIC_RESULT> list = hicResultService.GetItembyWrtNo(fnWrtno);


         
            if (list.Count >0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].EXCODE.Trim() == "A142")
                    {
                        SS1.ActiveSheet.Cells[8,18].Text = hb.READ_ResultName(list[i].RESCODE.Trim(),list[i].RESULT.Trim());
                    }
                    else if (list[i].EXCODE.Trim() == "A303")
                    {
                        if (list[i].RESULT == "No salmonella & shigella,Vibrio spp.isolated")
                        {
                            SS1.ActiveSheet.Cells[10, 18].Text = "정상";
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[10, 18].Text = list[i].RESULT.Trim();
                        }
                    }
                    else if (list[i].EXCODE.Trim() == "TW16")
                    {
                        SS1.ActiveSheet.Cells[12, 18].Text = list[i].RESULT.Trim();
                    }
                }
            }

            SS1.ActiveSheet.Cells[21, 4].Text = item.PANJENGDRNO.ToString();
            SS1.ActiveSheet.Cells[21, 8].Text = hb.READ_License_DrName(item.PANJENGDRNO);

            //발급일
            SS1.ActiveSheet.Cells[23, 7].Text = clsPublic.GstrSysDate;

            HIC_RES_ETC item1 = hicResEtcService.GetItembyWrtNo(fnWrtno,"4");
            if(!item1.IsNullOrEmpty())
            {
                SS1.ActiveSheet.Cells[28, 17].Text = item1.SOGEN;
            }

            //출력
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "";
            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Auto, PrintType.All, 0, 0, false, false, true, true, false, false, true);
            //cSpd.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);

            SS1.ActiveSheet.PrintInfo.Margin.Header = 40;
            SS1.ActiveSheet.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            SS1.ActiveSheet.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            SS1.ActiveSheet.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Both;
            SS1.ActiveSheet.PrintInfo.Orientation = PrintOrientation.Landscape;
            SS1.ActiveSheet.PrintInfo.ZoomFactor = 0.9f;
            SS1.ActiveSheet.PrintInfo.UseMax = false;
            SS1.PrintSheet(SS1.ActiveSheet);

        }
        private void Result_Print_Update()
        {
            string strUpdateOK = "";

            HIC_RES_ETC item = hicResEtcService.GetItembyWrtNo(fnWrtno, "4");
            if (item.IsNullOrEmpty())
            {
                if(item.PRTSABUN == 0)
                {
                    strUpdateOK = "OK";
                }
            }

            if (strUpdateOK == "OK")
            {
                hicResEtcService.UpdateByWrtnoGubun(fnWrtno, clsType.User.IdNumber.To<long>(), clsPublic.GstrSysDate, "4");
            }

            //접수테이블에 통보일자 세팅최초값-갱신안됨
            HIC_JEPSU item1 = hicJepsuService.GetItemByWrtnoGjjong(fnWrtno, "54");
            if (!item1.IsNullOrEmpty())
            {
                if (item1.TONGBODATE.IsNullOrEmpty())
                {
                    hicJepsuService.UpdateTongbodatePrtsabunbyWrtNo(fnWrtno, clsType.User.IdNumber.To<long>());
                }
            }



        }
    }
}
