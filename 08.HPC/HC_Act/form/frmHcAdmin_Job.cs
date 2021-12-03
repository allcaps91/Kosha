using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_Act
{
    public partial class frmHcAdmin_Job :Form
    {
        long FnWRTNO = 0;
        string FstrJepDate = "";
        string FstrGjJong = "";
        string FstrGjChasu = "";

        HicJepsuPatientService hicJepsuPatientService = null;
        HicDoctorService hicDoctorService = null;
        HicSangdamNewService hicSangdamNewService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicJepsuService hicJepsuService = null;
        HicResBohum1Service hicResBohum1Service = null;
        HicResSpecialService hicResSpecialService = null;
        HicXMunjinService hicXMunjinService = null;
        HicResultService hicResultService = null;
        HicResBohum2Service hicResBohum2Service = null;
        HicSpcPanjengService hicSpcPanjengService = null;
        HicResDentalService hicResDentalService = null;
        HicCancerNewService hicCancerNewService = null;

        clsHaBase hb = null;

        public frmHcAdmin_Job()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        public frmHcAdmin_Job(long argWRTNO)
        {
            InitializeComponent();
            SetControl();
            SetEvent();

            FnWRTNO = argWRTNO;
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);

            this.btnSave11.Click += new EventHandler(eBtnSave1);
            this.btnSave12.Click += new EventHandler(eBtnSave1);

            this.btnSave21.Click += new EventHandler(eBtnSave2);
            this.btnSave22.Click += new EventHandler(eBtnSave2);

            this.btnSave31.Click += new EventHandler(eBtnSave3);
            this.btnSave32.Click += new EventHandler(eBtnSave3);

            this.btnSave41.Click += new EventHandler(eBtnSave4);
            this.btnSave42.Click += new EventHandler(eBtnSave4);

            this.btnSave51.Click += new EventHandler(eBtnSave5);
            this.btnSave52.Click += new EventHandler(eBtnSave5);

            this.btnWebUpDate.Click += new EventHandler(eBtnWebDelete);
        }

        private void eBtnWebDelete(object sender, EventArgs e)
        {
            string strMsg = "";

            if (FnWRTNO == 0) { return; }

            if (VB.Left(dtpDate6.Text, 10) == DateTime.Now.ToShortDateString())
            {
                strMsg = "웹결과지 및 통보일자를 정리 하시겠습니까?";

                if (MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    Screen_Display();
                    return;
                }

                try
                {
                    clsDB.setBeginTran(clsDB.DbCon);

                    if (!hicJepsuService.UpdateWebPrtSendbyWrtno(FnWRTNO))
                    {
                        MessageBox.Show("HIC_JEPSU 웹결과지 출력일자 변경시 오류 발생", "오류");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);

                    MessageBox.Show("작업완료!");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }
            }
        }

        private void eBtnSave1(object sender, EventArgs e)
        {
            string strMsg = "";

            string strDate = "";
            string strDoct = "";
            long nDrNO = 0;
            long nSabun = 0;

            if (FnWRTNO == 0) { return; }

            strDate = dtpDate1.Text;
            nDrNO = VB.STRCUT(cboDoct1.Text, "(", ",").To<long>(0);
            nSabun = VB.STRCUT(cboDoct1.Text, ",", ")").To<long>(0);

            if (sender == btnSave12)
            {
                strDate = "";
                strDoct = "";
                nDrNO = 0;
                nSabun = 0;
            }

            if (strDate.IsNullOrEmpty())
            {
                strMsg = "미상담으로 변경을 하시겠습니까?";
            }
            else
            {
                strMsg = "상담일자를 " + strDate + "로 변경하시겠습니까?";
            }

            if (MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                Screen_Display();
                return;
            }

            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                //HIC_JEPSU 상담일자 날짜 변경
                if (!hicJepsuService.UpdateSangdamDrnobyWrtno(strDate, nSabun, FnWRTNO))
                {
                    MessageBox.Show("HIC_JEPSU 상담일자 변경시 오류 발생", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                //1차판정 상담일자 변경 MunjinEntDate, MunjinDrno
                if (hicResBohum1Service.UpdateMunjinDrNobyWrtNo(nSabun, FnWRTNO, strDate) < 0)
                {
                    MessageBox.Show("HIC_RES_BOHUM1 상담일자 변경시 오류 발생", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                //HIC_SANGDAM_NEW 상담일자 날짜 변경
                if (!hicSangdamNewService.UpDateSangdamDrno(nDrNO, FnWRTNO))
                {
                    MessageBox.Show("HIC_SANGDAM_NEW 상담의사 변경시 오류 발생", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                //HIC_RES_SPECIAL 상담일자 날짜 변경
                if (!hicResSpecialService.UpDaterJinDrnoByWrtno(nDrNO, FnWRTNO))
                {
                    MessageBox.Show("HIC_RES_SPECIAL 상담의사 변경시 오류 발생", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                //HIC_X_MUNJIN 상담일자 날짜 변경
                if (!hicXMunjinService.UpDateMunDrnoByWrtno(nDrNO, FnWRTNO))
                {
                    MessageBox.Show("HIC_X_MUNJIN 상담의사 변경시 오류 발생", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                //미상담으로 액팅코드 변경
                if (strDate.IsNullOrEmpty())
                {
                    if (!hicResultService.Update_Reset_Acting(FnWRTNO))
                    {
                        MessageBox.Show("HIC_RESULT 미상담으로 변경시 오류 발생", "오류");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                if (strDate.IsNullOrEmpty())
                {
                    hb.INSERT_JOB_LOG(clsDB.DbCon, "HcAdmin", FnWRTNO, "미상담으로 변경");
                }
                else
                {
                    hb.INSERT_JOB_LOG(clsDB.DbCon, "HcAdmin", FnWRTNO, "상담일을 " + strDate + "로 변경");
                }

                MessageBox.Show("작업완료!");

                Screen_Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }
        }

        private void eBtnSave2(object sender, EventArgs e)
        {
            string strMsg = "";

            string strDate = "";
            string strDoct = "";
            long nDrNO = 0;
            long nSabun = 0;

            if (FnWRTNO == 0) { return; }

            strDate = dtpDate2.Text;
            nDrNO = VB.STRCUT(cboDoct2.Text, "(", ",").To<long>(0);
            nSabun = VB.STRCUT(cboDoct2.Text, ",", ")").To<long>(0);

            if (sender == btnSave22)
            {
                strDate = "";
                strDoct = "";
                nDrNO = 0;
                nSabun = 0;
            }

            if (strDoct.IsNullOrEmpty())
            {
                strMsg = "미판정으로 변경을 하시겠습니까?";
            }
            else
            {
                strMsg = "판정일자를 " + strDate + "로 변경하시겠습니까?";
            }

            if (MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                Screen_Display();
                return;
            }

            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                if (FstrGjChasu == "1")
                {
                    if (strDate.IsNullOrEmpty())
                    {
                        //미판정으로 전환
                        if (!hicResBohum1Service.UpdateNotPanjengbyWrtNo(FnWRTNO))
                        {
                            MessageBox.Show("HIC_RES_BOHUM1 미판정 변경시 오류 발생", "오류");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }

                        if (!hicJepsuService.UpdateNotPanjengbyWrtno(FnWRTNO))
                        {
                            MessageBox.Show("HIC_JEPSU 미판정 변경시 오류 발생", "오류");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }

                        //PDF파일 삭제
                        if (!comHpcLibBService.DeletePdfSendByWrtno(FnWRTNO))
                        {
                            MessageBox.Show("HIC_PDF_SEND 삭제시 오류 발생", "오류");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }
                    else
                    {
                        if (!hicResBohum1Service.UpdatePanjengInfobyWrtNo(FnWRTNO, strDate, nDrNO))
                        {
                            MessageBox.Show("HIC_RES_BOHUM1 판정정보 변경시 오류 발생", "오류");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }

                        if (!hicJepsuService.UpdatePanjengInfobyWrtno(FnWRTNO, strDate, nDrNO))
                        {
                            MessageBox.Show("HIC_JEPSU 판정정보 변경시 오류 발생", "오류");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }
                }
                else
                {
                    if (strDate.IsNullOrEmpty())
                    {
                        if (!hicJepsuService.UpdateNotPanjengbyWrtno(FnWRTNO))
                        {
                            MessageBox.Show("HIC_JEPSU 미판정 변경시 오류 발생", "오류");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }

                        //PDF파일 삭제
                        if (!comHpcLibBService.DeletePdfSendByWrtno(FnWRTNO))
                        {
                            MessageBox.Show("HIC_PDF_SEND 삭제시 오류 발생", "오류");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }
                    else
                    {
                        if (!hicResBohum2Service.UpdatePanjengInfobyWrtNo(FnWRTNO, strDate, nDrNO))
                        {
                            MessageBox.Show("HIC_RES_BOHUM2 판정 변경시 오류 발생", "오류");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }

                        if (!hicJepsuService.UpdatePanjengInfobyWrtno(FnWRTNO, strDate, nDrNO))
                        {
                            MessageBox.Show("HIC_JEPSU 미판정 변경시 오류 발생", "오류");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                if (strDate.IsNullOrEmpty())
                {
                    hb.INSERT_JOB_LOG(clsDB.DbCon, "HcAdmin", FnWRTNO, "미판정으로 변경");
                }
                else
                {
                    hb.INSERT_JOB_LOG(clsDB.DbCon, "HcAdmin", FnWRTNO, "판정일자 및 의사를 변경함");
                }

                MessageBox.Show("작업완료!");
                Screen_Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }
        }

        private void eBtnSave3(object sender, EventArgs e)
        {
            string strMsg = "";

            string strDate = "";
            string strDoct = "";
            long nDrNO = 0;
            long nSabun = 0;

            if (FnWRTNO == 0) { return; }

            strDate = dtpDate3.Text;
            nDrNO = VB.STRCUT(cboDoct3.Text, "(", ",").To<long>(0);
            nSabun = VB.STRCUT(cboDoct3.Text, ",", ")").To<long>(0);

            if (sender == btnSave32)
            {
                strDate = "";
                strDoct = "";
                nDrNO = 0;
                nSabun = 0;
            }

            if (strDoct.IsNullOrEmpty())
            {
                strMsg = "미판정으로 변경을 하시겠습니까?";
            }
            else
            {
                strMsg = "판정일자를 " + strDate + "로 변경하시겠습니까?";
            }

            if (MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                Screen_Display();
                return;
            }

            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                if (!hicResDentalService.UpdatePanjengInfobyWrtNo(FnWRTNO, strDate, nDrNO))
                {
                    MessageBox.Show("HIC_RES_DENTAL 판정 변경시 오류 발생", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                if (!hicJepsuService.UpdatePanjengInfobyWrtno(FnWRTNO, strDate, nDrNO))
                {
                    MessageBox.Show("HIC_JEPSU 미판정 변경시 오류 발생", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                if (strDate.IsNullOrEmpty())
                {
                    hb.INSERT_JOB_LOG(clsDB.DbCon, "HcAdmin", FnWRTNO, "구강 미판정으로 변경");
                }
                else
                {
                    hb.INSERT_JOB_LOG(clsDB.DbCon, "HcAdmin", FnWRTNO, "구강 판정일자 및 의사를 변경함");
                }

                MessageBox.Show("작업완료!");
                Screen_Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }
        }

        private void eBtnSave4(object sender, EventArgs e)
        {
            string strMsg = "";

            string strDate = "";
            string strGbn = "";

            if (FnWRTNO == 0) { return; }

            strDate = dtpDate4.Text;
            strGbn = VB.Left(cboGbn.Text, 1);

            if (sender == btnSave42)
            {
                strDate = "";
                strGbn = "";
            }

            if (strDate.IsNullOrEmpty())
            {
                strMsg = "미통보로 변경을 하시겠습니까?";
            }
            else
            {
                strMsg = "통보일자를 " + strDate + "로 변경하시겠습니까?";
            }

            if (MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                Screen_Display();
                return;
            }

            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                //인쇄완료 취소 또는 날짜 변경
                if (!hicResBohum1Service.UpdateTongBoInfobyWrtNo(FnWRTNO, strDate, strGbn, clsType.User.IdNumber))
                {
                    MessageBox.Show("HIC_RES_BOHUM1 통보일 변경시 오류 발생", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                if (!hicResBohum2Service.UpdateTongBoInfobyWrtNo(FnWRTNO, strDate, strGbn, clsType.User.IdNumber))
                {
                    MessageBox.Show("HIC_RES_BOHUM2 통보일 변경시 오류 발생", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                if (!hicJepsuService.UpdateTongBoInfobyWrtNo(FnWRTNO, strDate))
                {
                    MessageBox.Show("HIC_JEPSU 통보일 변경시 오류 발생", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                if (!hicResDentalService.UpdateTongBoInfobyWrtNo(FnWRTNO, strDate, strGbn))
                {
                    MessageBox.Show("HIC_RES_DENTAL 통보일 변경시 오류 발생", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                if (!hicCancerNewService.UpdateTongBoInfobyWrtNo(FnWRTNO, strDate, strGbn))
                {
                    MessageBox.Show("HIC_CANCER_NEW 통보일 변경시 오류 발생", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                //PDF파일 삭제
                if (!comHpcLibBService.DeletePdfSendByWrtno(FnWRTNO))
                {
                    MessageBox.Show("HIC_PDF_SEND 삭제시 오류 발생", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                MessageBox.Show("작업완료!");
                Screen_Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }
        }

        private void eBtnSave5(object sender, EventArgs e)
        {
            string strMsg = "";

            string strDate = "";
            string strDoct = "";
            long nDrNO = 0;
            long nSabun = 0;

            if (FnWRTNO == 0) { return; }

            strDate = dtpDate5.Text;
            nDrNO = VB.STRCUT(cboDoct5.Text, "(", ",").To<long>(0);
            nSabun = VB.STRCUT(cboDoct5.Text, ",", ")").To<long>(0);

            if (sender == btnSave52)
            {
                strDate = "";
                strDoct = "";
                nDrNO = 0;
                nSabun = 0;
            }

            if (strDoct.IsNullOrEmpty())
            {
                strMsg = "미판정으로 변경을 하시겠습니까?";
            }
            else
            {
                strMsg = "판정일자를 " + strDate + "로 변경하시겠습니까?";
            }

            if (MessageBox.Show(strMsg, "확인", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                Screen_Display();
                return;
            }

            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                if (!hicJepsuService.UpdatePanjengInfobyWrtno(FnWRTNO, strDate, nDrNO))
                {
                    MessageBox.Show("HIC_JEPSU 판정정보 변경시 오류 발생", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                if (!hicResBohum1Service.UpdatePanjengInfobyWrtNo(FnWRTNO, strDate, nDrNO))
                {
                    MessageBox.Show("HIC_RES_BOHUM1 판정정보 변경시 오류 발생", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                if (!hicResSpecialService.UpdatePanjengInfobyWrtNo(FnWRTNO, strDate, nDrNO))
                {
                    MessageBox.Show("HIC_RES_SPECAIL 판정 변경시 오류 발생", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                if (!hicSpcPanjengService.UpdatePanjengInfobyWrtNo(FnWRTNO, strDate, nDrNO))
                {
                    MessageBox.Show("HIC_SPC_PANJENG 판정 변경시 오류 발생", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                if (!hicResBohum2Service.UpdatePanjengInfobyWrtNo(FnWRTNO, strDate, nDrNO))
                {
                    MessageBox.Show("HIC_RES_BOHUM2 판정 변경시 오류 발생", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                if (strDate.IsNullOrEmpty())
                {
                    hb.INSERT_JOB_LOG(clsDB.DbCon, "HcAdmin", FnWRTNO, "미판정으로 변경");
                }
                else
                {
                    hb.INSERT_JOB_LOG(clsDB.DbCon, "HcAdmin", FnWRTNO, "판정일자 및 의사를 변경함");
                }

                MessageBox.Show("작업완료!");
                Screen_Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }
        }

        private void SetControl()
        {
            hicJepsuPatientService = new HicJepsuPatientService();
            hicDoctorService = new HicDoctorService();
            hicSangdamNewService = new HicSangdamNewService();
            comHpcLibBService = new ComHpcLibBService();
            hicJepsuService = new HicJepsuService();
            hicResBohum1Service = new HicResBohum1Service();
            hicResSpecialService = new HicResSpecialService();
            hicXMunjinService = new HicXMunjinService();
            hicResultService = new HicResultService();
            hicResBohum2Service = new HicResBohum2Service();
            hicSpcPanjengService = new HicSpcPanjengService();
            hicResDentalService = new HicResDentalService();
            hicCancerNewService = new HicCancerNewService();

            hb = new clsHaBase();

            cboGbn.Items.Clear();
            cboGbn.Items.Add("");
            cboGbn.Items.Add("1.사업장");
            cboGbn.Items.Add("2.주소지");
            cboGbn.Items.Add("3.내원");
        }

        private void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (!txtWrtNo.Text.IsNullOrEmpty())
                {
                    Screen_Display();
                }
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            Screen_Clear();

            if (FnWRTNO > 0)
            {
                txtWrtNo.Text = FnWRTNO.To<string>("");
                Screen_Display();
            }

            this.ActiveControl = txtWrtNo;
        }

        private void Screen_Clear()
        {
            txtWrtNo.Text = "";
            dtpDate1.Checked = false;
            dtpDate2.Checked = false;
            dtpDate3.Checked = false;
            dtpDate4.Checked = false;
            dtpDate5.Checked = false;
            dtpDate6.Checked = false;

            cboDoct1.Text = "";
            cboDoct2.Text = "";
            cboDoct3.Text = "";
            cboDoct5.Text = "";

            cboGbn.SelectedIndex = 0;

            btnSave11.Enabled = false;
            btnSave12.Enabled = false;
            btnSave21.Enabled = false;
            btnSave22.Enabled = false;
            btnSave31.Enabled = false;
            btnSave32.Enabled = false;
            btnSave41.Enabled = false;
            btnSave42.Enabled = false;
            btnSave51.Enabled = false;
            btnSave52.Enabled = false;

            for (int i = 0; i < 6; i++)
            {
                ssPatInfo.ActiveSheet.Cells[0, i].Text = "";
            }
        }

        private void Screen_Display()
        {
            FnWRTNO = txtWrtNo.Text.To<long>();

            Screen_Clear();

            txtWrtNo.Text = FnWRTNO.To<string>("");

            string strDoct = string.Empty;
            string strTable = string.Empty;
            long nSangDamDrno = 0;

            cboDoct1.Items.Clear();
            cboDoct2.Items.Clear();
            cboDoct3.Items.Clear();
            cboDoct5.Items.Clear();

            HIC_JEPSU_PATIENT lstPat = hicJepsuPatientService.GetItembyWrtNo(FnWRTNO);

            if (lstPat.IsNullOrEmpty())
            {
                MessageBox.Show(txtWrtNo.Text.Trim() + " 접수번호가 등록 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ssPatInfo.ActiveSheet.RowCount = 1;

            ssPatInfo.ActiveSheet.Cells[0, 0].Text = lstPat.PTNO;
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = lstPat.SNAME;
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = lstPat.AGE + "/" + lstPat.SEX;
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(lstPat.LTDCODE.To<string>());
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = lstPat.JEPDATE.To<string>();
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(lstPat.GJJONG);

            FstrJepDate = lstPat.JEPDATE.To<string>();
            FstrGjJong = lstPat.GJJONG;
            FstrGjChasu = lstPat.GJCHASU;


            //접수일자를 기준으로 근무한 의사를 읽음
            List<HIC_DOCTOR> lstDoct = hicDoctorService.GetListbyReday(FstrJepDate);

            if (lstDoct.Count > 0)
            {
                for (int i = 0; i < lstDoct.Count; i++)
                {
                    strDoct = lstDoct[i].DRNAME + "(";
                    strDoct += lstDoct[i].LICENCE + ",";
                    strDoct += VB.Format(lstDoct[i].SABUN, "#00000") + ")";

                    if (lstDoct[i].GBDENT != "1")
                    {
                        cboDoct1.Items.Add(strDoct);
                        cboDoct2.Items.Add(strDoct);
                        cboDoct5.Items.Add(strDoct);
                    }
                    else
                    {
                        cboDoct3.Items.Add(strDoct);
                    }
                }
            }

            //상담일자
            if (!lstPat.SANGDAMDATE.IsNullOrEmpty())
            {   
                btnSave11.Enabled = true;
                btnSave12.Enabled = true;

                dtpDate1.Checked = true; dtpDate1.Text = lstPat.SANGDAMDATE;
                dtpDate6.Checked = true; dtpDate6.Text = lstPat.WEBPRINTSEND;

                if (lstPat.SANGDAMDRNO > 0)
                {
                    strDoct = VB.Format(lstPat.SANGDAMDRNO, "#00000");
                }
                else
                {
                    nSangDamDrno = hicSangdamNewService.GetSangdamDrNobyWrtNo(FnWRTNO);
                }

                for (int i = 0; i < cboDoct1.Items.Count; i++)
                {
                    if (VB.InStr(cboDoct1.Items[i].ToString(), strDoct) > 0)
                    {
                        cboDoct1.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                dtpDate1.Checked = true;
                btnSave11.Enabled = true;
                btnSave12.Enabled = true;
            }

            //판정일자
            if (FstrGjJong == "31")     //암검진
            {
                //암판정은 수정못함
            }
            else
            {
                dtpDate2.Checked = true;
                cboDoct2.Text = "";
                
                if (FstrGjChasu == "1")
                {
                    strTable = "HIC_RES_BOHUM1";
                }
                else
                {
                    strTable = "HIC_RES_BOHUM2";
                }

                COMHPC cHPC = comHpcLibBService.GetPanjengInfoByTableWrtno(strTable, FnWRTNO);
                if (!cHPC.IsNullOrEmpty())
                {
                    if (cHPC.PANJENGDATE.To<string>("") != "")
                    {
                        btnSave21.Enabled = true;
                        btnSave22.Enabled = true;
                        dtpDate2.Text = cHPC.PANJENGDATE.To<string>("");
                        strDoct = VB.Format(cHPC.PANJENGDRNO, "#00000");

                        for (int i = 0; i < cboDoct2.Items.Count; i++)
                        {
                            if (VB.InStr(cboDoct2.Items[i].ToString(), strDoct) > 0)
                            {
                                cboDoct2.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }


                dtpDate5.Checked = true;
                cboDoct5.Text = "";
                btnSave51.Enabled = true;
                btnSave52.Enabled = true;
                strTable = "HIC_SPC_PANJENG";

                COMHPC cHPC2 = comHpcLibBService.GetPanjengInfoByTableWrtno(strTable, FnWRTNO);
                if (!cHPC2.IsNullOrEmpty())
                {
                    if (cHPC2.PANJENGDATE.To<string>("") != "")
                    {
                        dtpDate5.Text = cHPC2.PANJENGDATE.To<string>("");
                        strDoct = VB.Format(cHPC2.PANJENGDRNO, "#00000");

                        for (int i = 0; i < cboDoct5.Items.Count; i++)
                        {
                            if (VB.InStr(cboDoct5.Items[i].ToString(), strDoct) > 0)
                            {
                                cboDoct5.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }

            //구강 판정일자
            strTable = "HIC_RES_DENTAL";
            COMHPC cHPC3 = comHpcLibBService.GetPanjengInfoByTableWrtno(strTable, FnWRTNO);
            if (!cHPC3.IsNullOrEmpty())
            {
                if (cHPC3.PANJENGDATE.To<string>("") != "")
                {
                    dtpDate3.Checked = true;
                    btnSave31.Enabled = true;
                    btnSave32.Enabled = true;
                    dtpDate3.Text = cHPC3.PANJENGDATE.To<string>("");
                    strDoct = VB.Format(cHPC3.PANJENGDRNO, "#00000");

                    for (int i = 0; i < cboDoct3.Items.Count; i++)
                    {
                        if (VB.InStr(cboDoct3.Items[i].ToString(), strDoct) > 0)
                        {
                            cboDoct3.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }

            //통보일자,통보방법
            if (FstrGjJong == "31")
            {
                //암판정은 변경 못함
            }
            else
            {
                if (FstrGjChasu == "1")
                {
                    strTable = "HIC_RES_BOHUM1";
                }
                else
                {
                    strTable = "HIC_RES_BOHUM2";
                }
                COMHPC cHPC4 = comHpcLibBService.GetPanjengInfoByTableWrtno(strTable, FnWRTNO);
                if (!cHPC4.IsNullOrEmpty())
                {
                    if (cHPC4.TONGBODATE.To<string>("") != "")
                    {
                        dtpDate4.Checked = true;
                        btnSave41.Enabled = true;
                        btnSave42.Enabled = true;
                        dtpDate4.Text = cHPC4.TONGBODATE.To<string>("");
                        switch (cHPC4.TONGBOGBN)
                        {
                            case "1": cboGbn.SelectedIndex = 1; break;
                            case "2": cboGbn.SelectedIndex = 2; break;
                            case "3": cboGbn.SelectedIndex = 3; break;
                            default:
                                break;
                        }
                    }

                    //int result = 0;
                    //result = hicJepsuService.UpdateTongbodatePrtsabunbyWrtNo(FnWRTNO, clsType.User.IdNumber.To<long>());

                }
            }
        }
    }
}
