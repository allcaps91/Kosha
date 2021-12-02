using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HC_Print
{
    public partial class frmHcPrint_Talk_ReSend : Form
    {


        clsHaBase cHB = new clsHaBase();
        clsAlimTalk cATK = new clsAlimTalk();
        clsHaBase hb = new clsHaBase();
        clsSpread sp = new clsSpread();


        HicJepsuPatientService hicJepsuPatientService = null;
        HeaJepsuPatientService heaJepsuPatientService = null;
        EtcAlimTalkService etcAlimTalkService = null;


        public frmHcPrint_Talk_ReSend()
        {
            InitializeComponent();
            SetControl();
            SetEvents();
        }

        private void SetControl()
        {
            hicJepsuPatientService = new HicJepsuPatientService();
            heaJepsuPatientService = new HeaJepsuPatientService();
            etcAlimTalkService = new EtcAlimTalkService();
        }

        private void SetEvents()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSend.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);

        }

        void eFormLoad(object sender, EventArgs e)
        {

            ComFunc.ReadSysDate(clsDB.DbCon);
            dtpFDate.Text = DateTime.Now.AddDays(-1).ToShortDateString();
            dtpTDate.Text = DateTime.Now.ToShortDateString();

            SSList.ActiveSheet.Columns[8].Visible = false;
            SSList.ActiveSheet.Columns[9].Visible = false;
            SSList.ActiveSheet.Columns[10].Visible = false;
            SSList.ActiveSheet.Columns[11].Visible = false;
            SSList.ActiveSheet.Columns[12].Visible = false;

        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Display(SSList);
            }
            else if (sender == btnSend)
            {
                Talk_Send();
            }

            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        private void Screen_Display(FpSpread Spd)
        {

            string strSname = "";

            strSname = txtSname.Text;

            if (rdoJob1.Checked = true)
            {
                List<HIC_JEPSU_PATIENT> list = hicJepsuPatientService.GetItembyJepDateSname(dtpFDate.Text, dtpTDate.Text, strSname);
                for (int i = 0; i < list.Count; i++)
                {
                    SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME.Trim();
                    SSList.ActiveSheet.Cells[i, 2].Text = list[i].JEPDATE.Trim();
                    //SSList.ActiveSheet.Cells[i, 3].Text = list[i].PTNO.Trim();
                    SSList.ActiveSheet.Cells[i, 3].Text = VB.Format(VB.Val(list[i].PTNO), "00000000");



                    SSList.ActiveSheet.Cells[i, 4].Text = hb.READ_GjJong_Name(list[i].GJJONG.Trim());
                    SSList.ActiveSheet.Cells[i, 5].Text = list[i].GJJONG.Trim();
                    SSList.ActiveSheet.Cells[i, 6].Text = list[i].WRTNO.Trim();
                    SSList.ActiveSheet.Cells[i, 7].Text = list[i].HPHONE.Trim();
                    SSList.ActiveSheet.Cells[i, 8].Text = list[i].SEX.Trim();
                    SSList.ActiveSheet.Cells[i, 9].Text = list[i].AGE.Trim();
                    SSList.ActiveSheet.Cells[i, 10].Text = list[i].JUMIN.Trim();
                    SSList.ActiveSheet.Cells[i, 11].Text = list[i].JUMIN2.Trim();
                    SSList.ActiveSheet.Cells[i, 12].Text = "일반검진";
                }
            }
            else
            {
                List<HEA_JEPSU_PATIENT> list = heaJepsuPatientService.GetItembySdateSname(dtpFDate.Text, dtpTDate.Text, strSname);
                for (int i = 0; i < list.Count; i++)
                {
                    SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME.Trim();
                    SSList.ActiveSheet.Cells[i, 2].Text = list[i].JEPDATE.Trim();
                    SSList.ActiveSheet.Cells[i, 3].Text = list[i].PTNO.Trim();
                    SSList.ActiveSheet.Cells[i, 4].Text = "종합검진";
                    SSList.ActiveSheet.Cells[i, 5].Text = list[i].GJJONG.Trim();
                    SSList.ActiveSheet.Cells[i, 6].Text = list[i].WRTNO.Trim();
                    SSList.ActiveSheet.Cells[i, 7].Text = list[i].HPHONE.Trim();
                    SSList.ActiveSheet.Cells[i, 8].Text = list[i].SEX.Trim();
                    SSList.ActiveSheet.Cells[i, 9].Text = list[i].AGE.Trim();
                    SSList.ActiveSheet.Cells[i, 10].Text = list[i].JUMIN.Trim();
                    SSList.ActiveSheet.Cells[i, 11].Text = list[i].JUMIN2.Trim();
                    SSList.ActiveSheet.Cells[i, 12].Text = "종합검진";
                }
            }
        }

        private void Talk_Send()
        {

            bool bJOB = false;
            int nREAD = 0;
            string strFDate = "";
            string strTDate = "";
            string strPANO = "";
            string strName = "";
            string strTime = "";
            string strSTIME = "";
            string strTel = "";
            string strLtdName = "";
            string strGjjong = "";
            string strSex = "";
            string strLINK = "";
            string strLINKJUSO = "";
            string strBIRTHDAY = "";
            string strJepDate = "";
            string strJumin = "";
            string strJumin2 = "";
            string strGubun = "";
            string strJong = "";
            string strMinRTime = "";
            string strAmPm2 = "";
            string strTempCD = "";
            string strDeptCode = "";
            long nCount = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            strTempCD = "C_MJ_001_02_13891";
            strLINKJUSO = "https://pohangsmh.co.kr/result/view.php?param=";
            nCount = 0;


            for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
            {
                bJOB = true;
                strName = "";
                strTel = "";
                strJong = "";
                strDeptCode = "";
                //파일생성시간


                if ( SSList.ActiveSheet.Cells[i,0].Text == "True")
                {

                    strName = SSList.ActiveSheet.Cells[i, 1].Text;
                    strJepDate = SSList.ActiveSheet.Cells[i, 2].Text;
                    strPANO = SSList.ActiveSheet.Cells[i, 3].Text;
                    strJong = SSList.ActiveSheet.Cells[i, 5].Text;
                    strTel = SSList.ActiveSheet.Cells[i, 7].Text;


                    if ( SSList.ActiveSheet.Cells[i, 8].Text == "M")
                    {
                        strSex = "1";
                    }
                    else if (SSList.ActiveSheet.Cells[i, 8].Text == "F")
                    {
                        strSex = "2";
                    }

                    //strAge = SSList.ActiveSheet.Cells[i, 9].Text;
                    strJumin = VB.Left(SSList.ActiveSheet.Cells[i, 10].Text, 6);
                    strJumin2 = VB.Mid(SSList.ActiveSheet.Cells[i, 11].Text, 7,1);

                    if (strJumin2 == "3" || strJumin2 == "4"|| strJumin2 == "7" || strJumin2 == "8")
                    {
                        strBIRTHDAY = "20" + strJumin;
                    }
                    else
                    {
                        strBIRTHDAY = "19" + strJumin;
                    }


                    if(SSList.ActiveSheet.Cells[i, 12].Text == "일반검진")
                    {
                        strDeptCode = "HR";
                        strGjjong = hb.READ_GjJong_Name(strGjjong);
                        if (strJong == "11") { strGjjong = "일반"; }
                        if (strJong == "31") { strGjjong = "암"; }
                    }
                    else
                    {
                        strDeptCode = "TO";
                        strGjjong = "종합";
                        strJong = "83";
                    }
                    

                    //이름^생년월일^성별(남자1 또는 여자2)^검진일자^환자번호
                    strLINK = strLINKJUSO + clsAES.Base64Encode(strName + "^" + strBIRTHDAY + "^" + strSex + "^" + strJepDate + "^" + strPANO + "^" + strJong);

                    //TEST
                    //strTel = "010-9328-4620";

                    //SMS 자료에 INSERT
                    cATK.Clear_ATK_Varient();
                    //------------( 자료를 DB에 INSERT )---------------------
                    clsHcType.ATK.RDate = strJepDate;
                    clsHcType.ATK.SendUID = strTel + clsPublic.GstrSysDate + DateTime.Now.ToString("HH:mm:ss");
                    clsHcType.ATK.SendUID = clsHcType.ATK.SendUID.Replace(":", "").Replace("-", "").Replace(" ", "").Replace(".", "");
                    clsHcType.ATK.Pano = strPANO;
                    clsHcType.ATK.sName = strName;
                    clsHcType.ATK.HPhone = strTel;
                    clsHcType.ATK.RetTel = "054-260-8188";
                    clsHcType.ATK.SendType = "L";
                    clsHcType.ATK.TempCD = strTempCD;
                    clsHcType.ATK.DrName = "";
                    clsHcType.ATK.LtdName = "";
                    clsHcType.ATK.JobSabun = long.Parse(clsType.User.IdNumber);
                    clsHcType.ATK.GJNAME = strGjjong;
                    clsHcType.ATK.LINK = strLINK;

                    clsHcType.ATK.ATMsg = cATK.READ_TEMPLATE_MESSAGE(strTempCD);
                    clsHcType.ATK.SmsMsg = cATK.READ_TEMPLATE_SMS_MESSAGE(strTempCD);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{수검자명}", clsHcType.ATK.sName);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{검진종류}", clsHcType.ATK.GJNAME);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{사업장명}", clsHcType.ATK.LtdName);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{YYYY}", VB.Left(clsHcType.ATK.RDate, 4));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{MM}", VB.Mid(clsHcType.ATK.RDate, 6, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{DD}", VB.Mid(clsHcType.ATK.RDate, 9, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{링크주소}", clsHcType.ATK.LINK);

                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{수검자명}", clsHcType.ATK.sName);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{검진종류}", clsHcType.ATK.GJNAME);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{사업장명}", clsHcType.ATK.LtdName);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{YYYY}", VB.Left(clsHcType.ATK.RDate, 4));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{MM}", VB.Mid(clsHcType.ATK.RDate, 6, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{DD}", VB.Mid(clsHcType.ATK.RDate, 9, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{링크주소}", clsHcType.ATK.LINK);

                    if (cATK.INSERT_ALIMTALK_MESSAGE() == false)
                    {
                        return;
                    }

                    cATK.MYSQL_ALIMTALK_INSERT();

                }
            }
        }
    }
}
