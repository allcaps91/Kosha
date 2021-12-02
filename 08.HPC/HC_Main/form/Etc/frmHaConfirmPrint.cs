using ComBase;
using ComBase.Controls;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Windows.Forms;
/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHaConfirmPrint.cs
/// Description     : 종검 수검확인서 출력
/// Author          : 김민철
/// Create Date     : 2020-03-28
/// Update History  : 
/// </summary>
/// <seealso cref= "Frm수검자확인(Frm수검자확인서.frm)" />
namespace HC_Main
{
    public partial class frmHaConfirmPrint : Form
    {
        HeaJepsuPatientSunapService heaJepsuPatientSunapService = null;
        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread cSpd = null;
        FpSpread SS = null;

        string FstrGbn = string.Empty;
        long FnWRTNO = 0;

        public frmHaConfirmPrint()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHaConfirmPrint(long argWRTNO, string argGubun)
        {
            InitializeComponent();
            SetEvent();
            SetControl();
            FstrGbn = argGubun;
            FnWRTNO = argWRTNO;
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            SS = new FpSpread();

            heaJepsuPatientSunapService = new HeaJepsuPatientSunapService();

        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            Search_Data(FstrGbn);
        }

        private void Search_Data(string argGbn)
        {
            HEA_JEPSU_PATIENT_SUNAP item = heaJepsuPatientSunapService.GetItembyWrtno(FnWRTNO, argGbn);

            if (!item.IsNullOrEmpty())
            {
                string strJumin = clsAES.DeAES(item.JUMIN2).Trim();

                //경상북도 교육청 교직원
                if (argGbn == "1")
                {
                    SS = SS2;

                    SS2.ActiveSheet.Cells[3,  2].Text = "소    속 : " + item.SOSOK.Trim();
                    SS2.ActiveSheet.Cells[4,  2].Text = "성    명 : " + item.SNAME.Trim();
                    SS2.ActiveSheet.Cells[5,  2].Text = "주민번호 : " + strJumin.Substring(0, 6) + "-" + strJumin.Substring(6, 1) + "******";
                    SS2.ActiveSheet.Cells[6,  2].Text = "검진일시 : " + item.SDATE.Replace("-", ". ");
                    //SS2.ActiveSheet.Cells[7,  2].Text = "검진금액 : ￦" + VB.Format(item.TOTAMT, "#,##0") + "원";
                    SS2.ActiveSheet.Cells[7, 2].Text = "검진금액 : ￦" + VB.Format(item.BONINAMT, "#,##0") + "원";
                    SS2.ActiveSheet.Cells[11, 1].Text = "발 행 일 : " + DateTime.Now.ToShortDateString().Replace("-", ". ");
                }
                //포항시청 산하기관
                else if (argGbn == "2")
                {
                    SS = SS1;

                    SS1.ActiveSheet.Cells[6,  4].Text = " " + item.SNAME.Trim(); 
                    SS1.ActiveSheet.Cells[7,  4].Text = " " + item.SOSOK.Trim();
                    SS1.ActiveSheet.Cells[8,  4].Text = " " + strJumin.Substring(0, 6) + "-" + strJumin.Substring(6, 1) + "******";
                    SS1.ActiveSheet.Cells[9,  4].Text = "포항성모병원 (054-260-8291,2)";
                    SS1.ActiveSheet.Cells[10, 4].Text = item.SDATE.Replace("-", ". ");

                    //SS1.ActiveSheet.Cells[15, 5].Text = "￦" + VB.Format(item.TOTAMT, "#,##0") + "원";
                    //SS1.ActiveSheet.Cells[20, 5].Text = "￦" + VB.Format(item.TOTAMT, "#,##0") + "원";
                    SS1.ActiveSheet.Cells[15, 5].Text = "￦" + VB.Format(item.BONINAMT, "#,##0") + "원";
                    SS1.ActiveSheet.Cells[20, 5].Text = "￦" + VB.Format(item.BONINAMT, "#,##0") + "원";

                    SS1.ActiveSheet.Cells[26, 4].Text = "담당자 성명 : " + clsType.User.JobName + " (확인)";
                    SS1.ActiveSheet.Cells[28, 4].Text = DateTime.Now.ToShortDateString().Replace("-", ". ");
                }
                else if (argGbn == "3")
                {
                    SS = SS3;

                    if(!item.LTDNAME.IsNullOrEmpty())
                    {
                        SS3.ActiveSheet.Cells[3, 2].Text = "소    속 : " + item.LTDNAME.Trim();
                    }
                    else
                    {
                        SS3.ActiveSheet.Cells[3, 2].Text = "소    속 : ";
                    }


                    SS3.ActiveSheet.Cells[4,  2].Text = "성    명 : " + item.SNAME.Trim();
                    SS3.ActiveSheet.Cells[5,  2].Text = "주민번호 : " + strJumin.Substring(0, 6) + "-" + strJumin.Substring(6, 1) + "******";
                    SS3.ActiveSheet.Cells[6,  2].Text = "검진일시 : " + item.SDATE.Replace("-", ". ");
                    if (item.LTDCODE == 951)
                    {
                        SS3.ActiveSheet.Cells[7, 2].Text = "";
                    }
                    else
                    {
                        //SS3.ActiveSheet.Cells[7, 2].Text = "검진금액 : ￦" + VB.Format(item.TOTAMT, "#,##0") + "원";
                        SS3.ActiveSheet.Cells[7, 2].Text = "검진금액 : ￦" + VB.Format(item.BONINAMT, "#,##0") + "원";
                    }
                    SS3.ActiveSheet.Cells[11, 1].Text = "발 행 일 : " + DateTime.Now.ToShortDateString().Replace("-", ". ");
                }

                setMargin = new clsSpread.SpdPrint_Margin(0, 0, 20, 10, 40, 0);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, true, false, false);

                cSpd.setSpdPrint(SS, false, setMargin, setOption, "", "");

                ComFunc.Delay(1500);

                SS.Dispose();
                SS = null;

                this.Close();
            }
            else
            {
                MessageBox.Show("검진확인서를 인쇄할 접수내역이 없습니다.", "오류");
                this.Close();
                return;
            }
        }
    }
}
