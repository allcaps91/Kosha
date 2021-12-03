using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Enums;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcScheduleEntry.cs
/// Description     : 일반검진 출장 예약 등록
/// Author          : 김민철
/// Create Date     : 2020-05-29
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmScheduleEntry(HcMain47.frm)" />
namespace ComHpcLibB
{
    public partial class frmHcScheduleEntry :BaseForm
    {
        HicChulresvService hicChulresvService = null;
        HicLtdService hicLtdService = null;
        HIC_LTD LtdHelpItem = null;
        EtcSmsService etcSmsService = null;
        clsSpread cSpd = null;

        string FstrDate = string.Empty;
        string FstrView = string.Empty;

        public frmHcScheduleEntry()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcScheduleEntry(string argDate, string argGubun)
        {
            InitializeComponent();
            SetEvent();
            SetControl();
            FstrDate = argDate;
            FstrView = argGubun;
        }

        private void SetControl()
        {
            hicChulresvService = new HicChulresvService();
            hicLtdService = new HicLtdService();
            LtdHelpItem = new HIC_LTD();
            etcSmsService = new EtcSmsService();
            cSpd = new clsSpread();

            SSList.Initialize(new SpreadOption { RowHeight = 22, IsRowSelectColor = true });
            SSList.AddColumnDateTime("예약일자", nameof(HIC_CHULRESV.RDATE), 84, IsReadOnly.Y, DateTimeType.YYYY_MM_DD, new SpreadCellTypeOption { IsEditble = false, IsShowCalendarButton = false });
            SSList.AddColumn("검진시간", nameof(HIC_CHULRESV.RTIME), 84, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("회사명", nameof(HIC_CHULRESV.LTDNAME), 160, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("회사코드", nameof(HIC_CHULRESV.LTDCODE), 68, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("인원", nameof(HIC_CHULRESV.INWON), 47, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("검진장소", nameof(HIC_CHULRESV.PLACE), 47, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("출발시간", nameof(HIC_CHULRESV.STARTTIME), 47, new SpreadCellTypeOption { IsEditble = false });
            SSList.AddColumn("검진종류", nameof(HIC_CHULRESV.SPECIAL), 47, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SSList.AddColumn("참고사항", nameof(HIC_CHULRESV.REMARK), 47, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });
            SSList.AddColumn("ROWID", nameof(HIC_CHULRESV.RID), 47, new SpreadCellTypeOption { IsEditble = false, IsVisivle = false });

            SS10.Initialize(new SpreadOption { RowHeight = 22, IsRowSelectColor = true });
            SS10.AddColumnDateTime("전송일자", nameof(ETC_SMS.JOBDATE), 84, IsReadOnly.Y, DateTimeType.YYYY_MM_DD_HH_MM, new SpreadCellTypeOption { IsEditble = false, IsShowCalendarButton = false });
            SS10.AddColumn("회사코드", nameof(ETC_SMS.BIGO), 68, new SpreadCellTypeOption { IsEditble = false });
            SS10.AddColumn("전송내용", nameof(ETC_SMS.SENDMSG), 220, new SpreadCellTypeOption { IsEditble = false });
            SS10.AddColumn("전송여부", nameof(ETC_SMS.ISSEND), 47, new SpreadCellTypeOption { IsEditble = false });
            SS10.AddColumn("RID", nameof(ETC_SMS.RID), 47, new SpreadCellTypeOption { IsEditble = false });
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSearch_SMS.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);
            this.btnSmsNew.Click += new EventHandler(eBtnClick);
            this.btnSmsSelect.Click += new EventHandler(eBtnClick);
            this.btnSmsSend.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnVLtdHelp.Click += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.txtLtdName.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtVLtdName.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtMsgBox.TextChanged += new EventHandler(eTxtChanged);

            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
        }

        private void eTxtChanged(object sender, EventArgs e)
        {
            if (sender == txtMsgBox)
            {
                lblTxtLength.Text = Encoding.Default.GetByteCount(txtMsgBox.Text).To<string>() + "/80";
            }
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true)
            {
                return;
            }

            if (sender == SSList)
            {
                HIC_CHULRESV item = SSList.GetRowData(e.Row) as HIC_CHULRESV;
                panChul.SetData(item);

                //출장불가 사유
                txtChulNotSayu.Text = hicLtdService.GetChulNotSayuByLtdCode(item.LTDCODE.To<long>());

                if (e.Column == 2)
                {
                    txtHtel.Text = hicLtdService.GetHTelByLtdCode(item.LTDCODE.To<long>());

                    string strRDate = Convert.ToDateTime(item.RDATE).ToShortDateString();
                    string strLtdCode = item.LTDCODE.To<string>();

                    if (!txtSmsLtdcode.Text.IsNullOrEmpty() && txtSmsLtdcode.Text.Trim() != strLtdCode)
                    {
                        MessageBox.Show("선택한 예약일자의 회사코드가 다릅니다." + ComNum.VBLF + "회사변경시 신규작성 선택후 작성하시기 바랍니다.", "확인");
                        return;
                    }
                    else
                    {
                        txtSmsLtdcode.Text = strLtdCode;
                        Screen_Display_SMS();
                    }

                    if (txtMsgBox.Text.Trim() == "")
                    {
                        txtMsgBox.Text = strRDate;
                    }
                    else
                    {
                        txtMsgBox.Text += "," + strRDate;
                    }
                }
            }
        }

        private void Screen_Display_SMS()
        {
            SS10.DataSource = etcSmsService.GetListByBigoGubun(txtSmsLtdcode.Text.Trim(), "41");
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdName && e.KeyChar == (char)13)
            {
                if (!txtLtdName.Text.Trim().IsNullOrEmpty()) { Ltd_Code_Help(txtLtdName); }
            }
            else if (sender == txtVLtdName && e.KeyChar == (char)13)
            {
                if (!txtVLtdName.Text.Trim().IsNullOrEmpty()) { Ltd_Code_Help(txtVLtdName); }
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Display();
            }
            else if (sender == btnDelete)
            {
                Delete_Date(txtRowid.Text);
            }
            else if (sender == btnSearch_SMS)
            {
                Screen_Display_SMS();
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnVLtdHelp)
            {
                Ltd_Code_Help(txtVLtdName);
            }
            else if (sender == btnLtdHelp)
            {
                Ltd_Code_Help(txtLtdName);
            }
            else if (sender == btnCancel)
            {
                panChul.Initialize();
                grpBox1.Initialize();
                panSMS.Initialize();
            }
            else if (sender == btnSmsNew)
            {
                grpBox1.Initialize();
                cSpd.Spread_Clear_Simple(SS10);
            }
            else if (sender == btnSmsSelect)
            {
                if (MessageBox.Show("SMS를 전송하시겠습니까?", "SMS전송", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                txtMsgBox.Text = "출장검진이 " + txtMsgBox.Text + " 확정되었습니다.-- 포항성모병원";

                Send_SMS_Message();
            }
            else if (sender == btnSmsSend)
            {
                Send_SMS_Message();
            }
            else if (sender == btnSave)
            {
                Data_Save();
            }
        }

        private void Data_Save()
        {
            HIC_CHULRESV item = panChul.GetData<HIC_CHULRESV>();

            if (!item.IsNullOrEmpty())
            {
                //Data Error Check
                if (item.INWON == 0) { txtInwon.Text = "0"; }
                if (item.STARTTIME == "") { txtStartTime.Text = "00:00"; }
                if (txtChulNotSayu.Text.Trim() != "")
                {
                    MessageBox.Show("출장검진이 불가능한 회사입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (item.STARTTIME.Length != 5)
                {
                    MessageBox.Show("출발시간 오류.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!panChul.RequiredValidate())
                {
                    MessageBox.Show("필수 입력항목이 누락되었습니다.");
                    return;
                }

                clsPublic.GstrMsgList = "정말로 변경 하시겠습니까? 스케쥴이 변경됩니다.";
                if (!item.RID.IsNullOrEmpty())
                { 
                   if (MessageBox.Show(clsPublic.GstrMsgList, "자료변경", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                }

                item.RDATE = dtpRDate.Value;
                item.LTDCODE = VB.Pstr(txtLtdName.Text, ".", 1);
                item.LTDNAME = VB.Pstr(txtLtdName.Text, ".", 2);

                string strView = rdoView1.Checked == true ? "1" : "2";
                item.GUBUN = strView;
                item.JOBSABUN = clsType.User.IdNumber.To<long>();

                if (hicChulresvService.Save(item))
                {
                    MessageBox.Show("저장하였습니다");
                }
                else
                {
                    MessageBox.Show("오류가 발생하였습니다.");
                }

                panChul.Initialize();
                panSMS.Initialize();
                Screen_Display();
            }
        }

        private void Delete_Date(string argRowid)
        {
            if (argRowid.Trim() == "")
            {
                return;
            }

            if (MessageBox.Show("삭제 하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            int result = hicChulresvService.Delete(argRowid);
            if (result < 0)
            {
                MessageBox.Show("삭제시 오류발생.", "오류");
                return;
            }

            panChul.Initialize();
            panSMS.Initialize();
            Screen_Display();
        }

        private void Send_SMS_Message()
        {
            if (Encoding.Default.GetByteCount(txtMsgBox.Text) > 80)
            {
                MessageBox.Show("메세지는 80자까지만 가능합니다.", "확인");
                return;
            }

            ETC_SMS eSMS = new ETC_SMS
            {
                JOBDATE = DateTime.Now,
                BIGO = txtSmsLtdcode.Text.Trim(),
                HPHONE =  hicLtdService.GetHTelByLtdCode(txtSmsLtdcode.Text.Trim().To<long>()),
                RETTEL = txtRetTel.Text.Trim(),
                GUBUN = "41",
                SENDMSG = txtMsgBox.Text.Trim(),
                ENTSABUN = clsType.User.IdNumber.To<long>()
            };

            int result = etcSmsService.Insert(eSMS);
            if (result < 0)
            {
                MessageBox.Show("SMS 전송오류.", "전송오류");
                return;
            }
            else
            {
                MessageBox.Show("전송완료", "확인");
                grpBox1.Initialize();
                cSpd.Spread_Clear_Simple(SS10);
                Screen_Display_SMS();
            }
        }

        /// <summary>
        /// 사업장 코드 검색창 연동
        /// </summary>
        private void Ltd_Code_Help(TextBox tx)
        {
            string strFind = "";

            if (tx.Text.Contains("."))
            {
                strFind = VB.Pstr(tx.Text, ".", 2).Trim();
            }
            else
            {
                strFind = tx.Text.Trim();
            }

            frmHcLtdHelp frm = new frmHcLtdHelp(strFind);
            frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(ePost_value_LTD);
            frm.ShowDialog();

            if (LtdHelpItem.CODE > 0 && !LtdHelpItem.IsNullOrEmpty())
            {
                tx.Text = LtdHelpItem.CODE.To<string>();
                tx.Text += "." + LtdHelpItem.SANGHO;
            }
            else
            {
                tx.Text = "";
            }
        }

        private void ePost_value_LTD(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        private void Screen_Display()
        {
            string strFDate = dtpFDate.Text;
            string strTDate = dtpTDate.Text;
            string strView = rdoView1.Checked == true ? "1" : "2";
            long nLtdCode = VB.Pstr(txtVLtdName.Text, ".", 1).To<long>();

            IList<HIC_CHULRESV> List = hicChulresvService.GetListByDateGubun(strFDate, strTDate, strView, nLtdCode, "");
            SSList.DataSource = List;

        }

        private void eFormLoad(object sender, EventArgs e)
        {
            int nLastDD = 0;

            if (!FstrDate.IsNullOrEmpty())
            {
                nLastDD = DateTime.DaysInMonth(VB.Left(FstrDate, 4).To<int>(), VB.Mid(FstrDate, 6, 2).To<int>());

                dtpFDate.Text = FstrDate;
                dtpTDate.Text = VB.Mid(dtpFDate.Text, 1, 8) + VB.Format(nLastDD, "00");
            }
            else
            {
                nLastDD = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

                dtpFDate.Text = DateTime.Now.ToShortDateString();
                dtpTDate.Text = VB.Mid(dtpFDate.Text, 1, 8) + VB.Format(nLastDD, "00");
            }

            panChul.Initialize();
            grpBox1.Initialize();

            panChul.AddRequiredControl(dtpRDate);

            if (FstrView == "1")
            {
                rdoView1.Checked = true;
            }
            else if (FstrView == "2")
            {
                rdoView2.Checked = true;
            }

            Screen_Display();
        }
    }
}
