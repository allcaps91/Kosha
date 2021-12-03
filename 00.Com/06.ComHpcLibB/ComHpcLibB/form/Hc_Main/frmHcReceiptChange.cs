using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcReceiptChange_cs.cs
/// Description     : 검진접수 구분변경 작업
/// Author          : 이상훈
/// Create Date     : 2020-06-11
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm검진접수구분변경.frm(Frm검진접수구분변경)" />

namespace ComHpcLibB
{
    public partial class frmHcReceiptChange : Form
    {
        HicExjongService hicExjongService = null;
        HicJepsuService hicJepsuService = null;
        HicResultService hicResultService = null;
        HicSunapService hicSunapService = null;
        HicCodeService hicCodeService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicSangdamNewService hicSangdamNewService = null;
        HicResBohum1Service hicResBohum1Service = null;
        HicResBohum2Service hicResBohum2Service = null;
        HicResDentalService hicResDentalService = null;
        HicResSpecialService hicResSpecialService = null;
        HicSpcPanjengService hicSpcPanjengService = null;
        ComHpcLibBService comHpcLibBService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        long FnWrtNo;
        long FnPano;
        string FstrJong;
        string FstrJepDate;
        string FstrPtno;

        public frmHcReceiptChange()
        {
            InitializeComponent();
        }

        public frmHcReceiptChange(long nWrtNo)
        {
            InitializeComponent();
            FnWrtNo = nWrtNo;
            SetEvent();
        }

        void SetEvent()
        {
            hicExjongService = new HicExjongService();
            hicJepsuService = new HicJepsuService();
            hicResultService = new HicResultService();
            hicSunapService = new HicSunapService();
            hicCodeService = new HicCodeService();
            hicSunapdtlService = new HicSunapdtlService();
            hicSangdamNewService = new HicSangdamNewService();
            hicResBohum1Service = new HicResBohum1Service();
            hicResBohum2Service = new HicResBohum2Service();
            hicResDentalService = new HicResDentalService();
            hicResSpecialService = new HicResSpecialService();
            hicSpcPanjengService = new HicSpcPanjengService();
            comHpcLibBService = new ComHpcLibBService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnView.Click += new EventHandler(eBtnClick);
            this.btnOK.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.cboJong.KeyPress += new KeyPressEventHandler(eComboKeyPress);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            lblMsg.Text = "";
            btnView.Enabled = true;

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;
            txtDate.Text = "30";

            cboJong.Items.Clear();
            cboJong.Items.Add("11.사업장1차");
            cboJong.Items.Add("12.공무원1차");
            cboJong.Items.Add("13.성인병1차");
            cboJong.Items.Add("14.사업장1차(회사부담)");
            cboJong.Items.Add("16.사업장2차");
            cboJong.Items.Add("17.공무원2차");
            cboJong.Items.Add("18.성인병2차");
            cboJong.Items.Add("19.사업장2차(회사부담)");
            cboJong.Items.Add("41.생애(사업장1차)");
            cboJong.Items.Add("42.생애(공무원1차)");
            cboJong.Items.Add("43.생애(성인병1차)");
            cboJong.Items.Add("44.생애(사업장1차)");
            cboJong.Items.Add("45.생애(공무원1차)");
            cboJong.Items.Add("46.생애(성인병1차)");
            cboJong.SelectedIndex = 0;
            btnOK.Enabled = false;

            txtWrtNo.Text = "";
            if (FnWrtNo > 0)
            {
                txtWrtNo.Text = FnWrtNo.ToString();
                eBtnClick(btnView, new EventArgs());
                eBtnClick(btnSearch, new EventArgs());
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            if (sender == btnOK)
            {
                int nREAD = 0;
                string strJong = "";
                bool bOK = false;
                long nWrtNo = 0;
                int nOLD_Chasu = 0;
                int nNEW_Chasu = 0;
                string strJepDate = "";
                string strSname = "";
                string strData = "";
                string strOLD_Group = "";
                string strOLD_ExCode = "";
                string strNew_Group = "";
                string strNew_ExCode = "";
                string strGjChasu = "";
                long nSeq = 0;
                long nOLD_Seq = 0;
                int nDate = 0;
                int result = 0;

                strJong = VB.Left(cboJong.Text, 2);
                strSname = ssPatInfo.ActiveSheet.Cells[0, 1].Text;
                strJepDate = ssPatInfo.ActiveSheet.Cells[0, 4].Text;
                nDate = int.Parse(txtDate.Text);

                if (nDate <= 0)
                {
                    MessageBox.Show("작업기간 일수가 작습니다. 하루이상으로 세팅요망.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtDate.Focus();
                    return;
                }

                ComFunc.ReadSysDate(clsDB.DbCon);

                DateTime date1 = new DateTime();
                DateTime date2 = new DateTime();
                date1 = Convert.ToDateTime(strJepDate);
                date2 = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(nDate * -1);
                if (date1 > date2)
                {
                    MessageBox.Show(nDate + " 일이 경과된 자료는 구분변경이 불가능함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtDate.Focus();
                    return;
                }

                switch (strJong)
                {
                    case "11":
                    case "12":
                    case "13":
                    case "14":
                    case "16":
                    case "17":
                    case "18":
                    case "19":
                        bOK = true;
                        break;
                    case "41":
                    case "42":
                    case "43":
                    case "44":
                    case "45":
                    case "46":
                        bOK = false;
                        break;
                    default:
                        break;
                }

                if (bOK == false)
                {
                    MessageBox.Show("변경 건진종류가 오류입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                switch (FstrJong)
                {
                    case "11":
                    case "12":
                    case "13":
                    case "14":
                    case "41":
                    case "42":
                    case "43":
                        nOLD_Chasu = 1;
                        break;
                    default:
                        nOLD_Chasu = 2;
                        break;
                }

                switch (strJong)
                {
                    case "11":
                    case "12":
                    case "13":
                    case "14":
                    case "41":
                    case "42":
                    case "43":
                        nNEW_Chasu = 1;
                        break;
                    default:
                        nNEW_Chasu = 2;
                        break;
                }

                if (nOLD_Chasu != nNEW_Chasu)
                {
                    if (MessageBox.Show("변경 건진종류의 1,2차 구분이 틀립니다." + "\r\n" + "작업을 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                //변경할 건진종류의 Chasu를 읽음
                strGjChasu = hicExjongService.GetChasubyCode(strJong);

                btnOK.Enabled = false;

                //신규 건진접수 번호를 부여 받음
                nWrtNo = hb.Read_New_JepsuNo();

                clsDB.setBeginTran(clsDB.DbCon);

                result = hicJepsuService.InsertAllbySelect(FnWrtNo, strJong, strGjChasu);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("건진 접수마스타 신규생성 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //기존 접수마스타를 삭제처리
                HIC_JEPSU item = new HIC_JEPSU();

                item.JOBSABUN = long.Parse(clsType.User.IdNumber);
                item.GBSTS = "D";
                item.WRTNO = FnWrtNo;

                result = hicJepsuService.UpdateDelDatebyFnWrtNo(item);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("접수마스타에 취소일자 UPDATE시 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //건진결과를 복사함
                result = hicResultService.InsertSelectbyWrtNo(nWrtNo, FnWrtNo);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("건진결과를 복사 시 오류 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //신규번호 최종수납 시퀀스넘버 획득
                nSeq = hicSunapService.GetMaxSeqbyWrtNo(nWrtNo);

                //변경할 접수번호의 마지막 SeqNo를 찾음
                nOLD_Seq = hicSunapService.GetMaxSeqbyWrtNo(FnWrtNo);

                //수납Data 정리(기존 자료 오늘날짜로 +처리)
                result = hicSunapService.InsertSelectbyWrtNOSeqNo(nWrtNo, nSeq, long.Parse(clsType.User.IdNumber), FnWrtNo, nOLD_Seq, clsPublic.GstrSysDate);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("건진 수납 취소 오류 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //수납 상세내역 접수번호 변경
                List<HIC_CODE> list = hicCodeService.GetCodeNamebyGubun("35");

                nREAD = list.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    strData = list[i].NAME;
                    strOLD_Group = "";
                    strOLD_ExCode = "";
                    strNew_Group = "";
                    strNew_ExCode = "";
                    if (VB.Pstr(strData, ";", 1) == FstrJong && VB.Pstr(strData, ";", 2) == strJong)
                    {
                        strOLD_Group = VB.Pstr(strData, ";", 3);
                        strOLD_ExCode = VB.Pstr(strData, ";", 4);
                        strNew_Group = VB.Pstr(strData, ";", 5);
                        strNew_ExCode = VB.Pstr(strData, ";", 6);
                    }
                    else if (VB.Pstr(strData, ";", 2) == FstrJong && VB.Pstr(strData, ";", 1) == strJong)
                    {
                        strOLD_Group = VB.Pstr(strData, ";", 5);
                        strOLD_ExCode = VB.Pstr(strData, ";", 6);
                        strNew_Group = VB.Pstr(strData, ";", 3);
                        strNew_ExCode = VB.Pstr(strData, ";", 4);
                    }

                    if (strOLD_Group != "" && strOLD_ExCode != "")
                    {
                        result = hicSunapdtlService.UpdateCodebyWrtNoCode(strNew_Group, nWrtNo, strOLD_Group);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("HIC_RESULT 검사코드 변경 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }

                //상담 접수번호 변경
                result = hicSangdamNewService.UpdateWrtNoGjJongbyWrtNo(nWrtNo, strJong, FnWrtNo);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담 접수번호 변경 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //1차판정 접수번호 변경
                result = hicResBohum1Service.UpdateWrtNobyFWrtNo(nWrtNo, FnWrtNo);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("1차판정건진 접수번호 변경 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //2차판정 접수번호 변경
                result = hicResBohum2Service.UpdateWrtNobyFWrtNo(nWrtNo, FnWrtNo);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("2차판정건진 접수번호 변경 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //구강판정 접수번호 변경
                result = hicResDentalService.UpdateWrtNobyFWrtNo(nWrtNo, FnWrtNo);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("구강판정건진 접수번호 변경 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //특수판정 접수번호 변경
                result = hicResSpecialService.UpdateWrtNobyFWrtNo(nWrtNo, FnWrtNo);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("구강판정건진 접수번호 변경 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //특수판정(NEW) 접수번호 변경
                result = hicSpcPanjengService.UpdateWrtNobyFWrtNo(nWrtNo, FnWrtNo);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("구강판정건진 접수번호 변경 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //HIC_RESULT의 GroupCode,ExCode 값 변경
                List<HIC_CODE> list2 = hicCodeService.GetCodeNamebyGubun("35");

                nREAD = list2.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    strData = list2[i].NAME;
                    strOLD_Group = "";
                    strNew_Group = "";
                    strNew_ExCode = "";

                    if (VB.Pstr(strData, ";", 1) == FstrJong && VB.Pstr(strData, ";", 2) == strJong)
                    {
                        strOLD_Group = VB.Pstr(strData, ";", 3);
                        strOLD_ExCode = VB.Pstr(strData, ";", 4);
                        strNew_Group = VB.Pstr(strData, ";", 5);
                        strNew_ExCode = VB.Pstr(strData, ";", 6);
                    }
                    else if (VB.Pstr(strData, ";", 2) == FstrJong && VB.Pstr(strData, ";", 1) == strJong)
                    {
                        strOLD_Group = VB.Pstr(strData, ";", 5);
                        strOLD_ExCode = VB.Pstr(strData, ";", 6);
                        strNew_Group = VB.Pstr(strData, ";", 3);
                        strNew_ExCode = VB.Pstr(strData, ";", 4);
                    }

                    if (strOLD_Group != "" && strOLD_ExCode != "")
                    {
                        //그룹코드가 동일한것 모두 변경
                        if (strOLD_ExCode == "*")
                        {
                            result = hicResultService.UpdateGroupCodebyWrtNoCode(strNew_Group, "", nWrtNo, strOLD_Group, "");
                        }
                        else
                        {
                            result = hicResultService.UpdateGroupCodebyWrtNoCode(strNew_Group, strNew_ExCode, nWrtNo, strOLD_Group, strOLD_ExCode);
                        }

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("HIC_RESULT 검사코드 변경 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                }

                //신용카드 결재내역의 WRTNO를 변경
                result = comHpcLibBService.UpdateCARD_APPROV_CENTERHWrtNobyPanoHwrtNo(nWrtNo, FstrPtno, FnWrtNo);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("CARD_APPROV_CENTER 변경 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //구분변경 History에 등록           
                COMHPC item1 = new COMHPC();

                item1.JEPDATE = strJepDate;
                item1.SNAME = strSname;
                item1.OLD_WRTNO = FnWrtNo;
                item1.GJJONG = FstrJong;
                item1.NEW_WRTNO = nWrtNo;
                item1.OLD_GJJONG = strJong;
                item1.JOBSABUN = long.Parse(clsType.User.IdNumber);

                result = comHpcLibBService.InsertHicJongChange(item1);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("구분변경 History에 등록 오류", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                MessageBox.Show("접수화면에서 확인 후 저장을 하십시오.", "작업완료", MessageBoxButtons.OK, MessageBoxIcon.Information);

                clsPublic.GstrRetValue = FstrJepDate + "^^" + FnPano + "^^" + strJong + "^^" + nWrtNo;

                this.Close();
            }
            else if (sender == btnSearch)   //검색
            {
                string strFrDate = "";
                string strToDate = "";
                int nRead = 0;

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text + " 23:59";

                btnSearch.Enabled = false;
                btnPrint.Enabled = false;

                List<COMHPC> list = comHpcLibBService.GetJongChangebyJobTime(strFrDate, strToDate);

                nRead = list.Count;
                SS1.ActiveSheet.RowCount = nRead;
                for (int i = 0; i < nRead; i++)
                {
                    SS1.ActiveSheet.Cells[i, 0].Text = list[i].JOBTIME;
                    SS1.ActiveSheet.Cells[i, 1].Text = list[i].OLD_WRTNO.ToString();
                    SS1.ActiveSheet.Cells[i, 2].Text = list[i].SNAME;
                    SS1.ActiveSheet.Cells[i, 3].Text = list[i].JEPDATE;
                    SS1.ActiveSheet.Cells[i, 4].Text = list[i].OLD_GJJONG;
                    SS1.ActiveSheet.Cells[i, 5].Text = list[i].NEW_GJJONG;
                    SS1.ActiveSheet.Cells[i, 6].Text = list[i].JOBSABUN.ToString();
                    SS1.ActiveSheet.Cells[i, 7].Text = list[i].NEW_WRTNO.ToString();
                }

                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
            }
            else if (sender == btnView)
            {
                bool bOK = false;
                string strSex = "";

                btnOK.Enabled = false;
                FnWrtNo = long.Parse(txtWrtNo.Text);
                if (FnWrtNo == 0)
                {
                    return;
                }

                //인적사항을 Display
                HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(FnWrtNo);

                if (list.IsNullOrEmpty())
                {
                    MessageBox.Show("접수번호 : " + FnWrtNo + " 번 자료가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                FstrJepDate = list.JEPDATE;
                FstrJong = list.GJJONG;
                FnPano = list.PANO;
                strSex = list.SEX;
                FstrPtno = list.PTNO;
                lblMsg.Text = list.GJJONG + "." + hb.READ_GjJong_Name(list.GJJONG);

                ssPatInfo.ActiveSheet.Cells[0, 0].Text = list.PANO.ToString();
                ssPatInfo.ActiveSheet.Cells[0, 1].Text = list.SNAME.ToString();
                ssPatInfo.ActiveSheet.Cells[0, 2].Text = list.AGE.ToString() + "/" + strSex;
                ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list.LTDCODE.ToString());
                ssPatInfo.ActiveSheet.Cells[0, 4].Text = list.JEPDATE;
                ssPatInfo.ActiveSheet.Cells[0, 5].Text = list.GJJONG + "." + hb.READ_GjJong_Name(list.GJJONG);

                //삭제된것 체크
                if (hb.READ_JepsuSTS(FnWrtNo) == "D")
                {
                    MessageBox.Show(FnWrtNo + " 접수번호는 삭제된것 입니다. 확인하십시오", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                switch (FstrJong)
                {
                    case "11":
                    case "12":
                    case "13":
                    case "14":
                    case "16":
                    case "17":
                    case "18":
                    case "19":
                        bOK = true;
                        break;
                    case "41":
                    case "42":
                    case "43":
                    case "44":
                    case "45":
                    case "46":                        
                        bOK = true;
                        break;
                    default:
                        bOK = false;
                        break;
                }

                if (bOK == false)
                {
                    MessageBox.Show("구분변경이 가능한 건진종류가 아닙니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                btnOK.Enabled = true;
            }
            else if (sender == btnPrint)
            {
                ///TODO : 이상훈 2020.06.16 AS_IS에 출력 코딩이 없음
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (txtWrtNo.Text != "")
                {
                    eBtnClick(btnView, new EventArgs());
                }
            }
        }

        void eComboKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }
    }
}
