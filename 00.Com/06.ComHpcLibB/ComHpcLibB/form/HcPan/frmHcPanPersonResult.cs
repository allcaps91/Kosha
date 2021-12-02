using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcPanPersonResult.cs
/// Description     : 개인별 History조회
/// Author          : 이상훈
/// Create Date     : 2019-12-26
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmPersonResult.frm(HcPan102)" />

namespace ComHpcLibB
{
    public partial class frmHcPanPersonResult : Form
    {
        HicPatientService hicPatientService = null;
        HicJepsuSunapService hicJepsuSunapService = null;
        HicSunapdtlGroupcodeService hicSunapdtlGroupcodeService = null;
        HicMemoService hicMemoService = null;
        WorkNhicService workNhicService = null;
        HeaExjongService heaExjongService = null;
        HicJepsuService hicJepsuService = null;

        frmHcResultView FrmHcResultView = null;
        frmHaResultView FrmHaResultView = null;
        frmHcNhicView FrmHcNhicView = null;
        frmHcNhicSub FrmHcNhicSub = null;
        frmHcPatientModify FrmHcPatientModify = null;

        public delegate void SetJepsuGstrValue(string strPtNo, string strJepDate);
        public static event SetJepsuGstrValue rSetJepsuGstrValue;

        public delegate void SetHaJepsuGstrValue(long nWRTNO);
        public static event SetHaJepsuGstrValue rSetHaJepsuGstrValue;

        public delegate void SetJepsuBtnRef(string strPtNo);
        public static event SetJepsuBtnRef rSetJepsuBtnRef;

        public delegate void SetHaJepsuBtnRef(string strPtNo);
        public static event SetHaJepsuBtnRef rSetHaJepsuBtnRef;

        public delegate void SetDelegateJepsuForm(object sender, EventArgs e);
        public static event SetDelegateJepsuForm rSetDelegateJep;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        string FSName = "";
        long FnWrtNo = 0;
        long FnPano;
        string FsPtNo;
        string FstrSname;
        string FstrJumin;
        string FstrProgID;
        string FstrPtNo;    //선택된 외래번호
        bool boolSort = false;

        public frmHcPanPersonResult()
        {
            InitializeComponent();
            SetEvent();
        }

        public frmHcPanPersonResult(string strProgID)
        {
            InitializeComponent();

            FstrProgID = strProgID;
            SetEvent();
        }

        public frmHcPanPersonResult(string strProgID, string sPtNo, string sName)
        {
            InitializeComponent();

            FstrProgID = strProgID;
            FSName = sName;
            FsPtNo = sPtNo;

            SetEvent();
        }

        void SetEvent()
        {
            hicPatientService = new HicPatientService();
            hicJepsuSunapService = new HicJepsuSunapService();
            hicSunapdtlGroupcodeService = new HicSunapdtlGroupcodeService();
            hicMemoService = new HicMemoService();
            workNhicService = new WorkNhicService();
            heaExjongService = new HeaExjongService();
            hicJepsuService = new HicJepsuService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnNhic.Click += new EventHandler(eBtnClick);
            this.btnMemoSave.Click += new EventHandler(eBtnClick);
            this.btnRef1.Click += new EventHandler(eBtnClick);
            this.btnRef2.Click += new EventHandler(eBtnClick);
            this.btnInfo.Click += new EventHandler(eBtnClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSList.CellClick += new CellClickEventHandler(eSpdClick);
            this.SS2.CellClick += new CellClickEventHandler(eSpdClick);
            this.rdoJob0.Click += new EventHandler(eRdoClick);
            this.rdoJob1.Click += new EventHandler(eRdoClick);
            this.rdoJob2.Click += new EventHandler(eRdoClick);
            this.rdoJob3.Click += new EventHandler(eRdoClick);
            this.rdoJob4.Click += new EventHandler(eRdoClick);
            this.rdoJob5.Click += new EventHandler(eRdoClick);
            this.rdoJob6.Click += new EventHandler(eRdoClick);
            this.txtSName.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtSName.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtSName.LostFocus += new EventHandler(eTxtLostFocus);
        }


        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            if (FstrProgID == "frmHcJepMain" || FstrProgID == "frmHaJepMain")
            {
                btnRef1.Visible = true; btnRef2.Visible = true;
                SS2_Sheet1.Columns.Get(9).Visible = true;
            }
            else
            {
                btnRef1.Visible = false; btnRef2.Visible = false;
                SS2_Sheet1.Columns.Get(9).Visible = false;
            }

            sp.Spread_All_Clear(SS_ETC);
            rdoJob2.Checked = true;
            this.SS2_Sheet1.Rows.Get(-1).Height = 24F;
            //SS2_Sheet1.Columns.Get(8).Visible = false;

            if (!FsPtNo.IsNullOrEmpty())
            {
                rdoJob5.Checked = true;
                lblViewTitle.Text = rdoJob5.Text;
                eRdoClick(rdoJob5, new EventArgs());
                txtSName.Text = FsPtNo;
            }
            else
            {
                rdoJob0.Checked = true;
                lblViewTitle.Text = rdoJob0.Text;
                eRdoClick(rdoJob0, new EventArgs());
                txtSName.Text = FSName;
            }

            if (!txtSName.Text.IsNullOrEmpty())
            {
                eBtnClick(btnSearch, new EventArgs());
                eSpdDClick(SSList, new CellClickEventArgs(new SpreadView(), 0, 0, 0, 0, new MouseButtons(), false, false));
            }
        }

        /// <summary>
        /// 수검자 공단 자격조회 Main Rutine
        /// </summary>
        /// <param name="argGbn"></param>
        /// <param name="argSName"></param>
        /// <param name="argJuminNo"></param>
        /// <param name="argPtno"></param>
        /// <param name="argYear"></param>
        void fn_Hic_Chk_Nhic(string argGbn, string argSName, string argJuminNo, string argPtno, string argYear, string arg생애구분)
        {
            //당일 조회된 자격내역이 있는지 점검
            WORK_NHIC item = workNhicService.GetNhicInfo_Am(argGbn, clsAES.AES(argJuminNo), argSName, argYear, "1", arg생애구분);

            if (item.IsNullOrEmpty())
            {
                //신규자격조회
                FrmHcNhicSub = new frmHcNhicSub(argSName, argJuminNo, argYear, "H", argPtno);
                FrmHcNhicSub.ShowDialog();
            }

            //자격조회 정보 Spread Display
            FrmHcNhicView = new frmHcNhicView();
            FrmHcNhicView.SetDisPlay(item);
            FrmHcNhicView.Show();


            //자격조회 정보 변수 대입
            hm.Display_Nhic_Info(item);

            if (clsHcType.THNV.hJaGubun.IsNullOrEmpty())
            {
                MessageBox.Show("자격이 없습니다.", "확인요망");
                return;
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnNhic)
            {
                ComFunc.ReadSysDate(clsDB.DbCon);

                fn_Hic_Chk_Nhic("H", txtSName.Text, FstrJumin, FnPano.ToString(), VB.Left(clsPublic.GstrSysDate, 4), "");
            }
            else if (sender == btnRef1)
            {
                if (!FstrPtNo.IsNullOrEmpty())
                {
                    if (!rSetJepsuBtnRef.IsNullOrEmpty())
                    {
                        rSetDelegateJep(sender, e);
                        rSetJepsuBtnRef(FstrPtNo);
                        this.Close();
                    }
                }
            }
            else if (sender == btnRef2)
            {
                if (!FstrPtNo.IsNullOrEmpty())
                {
                    if (!rSetHaJepsuBtnRef.IsNullOrEmpty())
                    {
                        rSetDelegateJep(sender, e);
                        rSetHaJepsuBtnRef(FstrPtNo);
                        this.Close();
                    }
                }
            }
            else if (sender == btnInfo)
            {
                if (!FstrPtNo.IsNullOrEmpty())
                {
                    FrmHcPatientModify = new frmHcPatientModify(FstrPtNo);
                    FrmHcPatientModify.ShowDialog();
                }
            }


            else if (sender == btnMemoSave)
            {
                fn_Hic_Memo_Save();
                fn_Hic_Memo_Screen();
            }
            else if (sender == btnSearch)
            {
                string strSName = "";
                string strJob = "";
                string strGubun = "";
                List<string> strNotPatient = new List<string>();

                sp.Spread_All_Clear(SSList);
                fn_Screen_Clear();

                if (!txtSName.Text.IsNullOrEmpty())
                {
                    strSName = txtSName.Text.Trim();
                }
                else
                {
                    return;
                }

                if (rdoJob3.Checked == true)    //검진번호
                {
                    strJob = "3";
                }
                else if (rdoJob0.Checked == true)   //성명
                {
                    strJob = "0";
                }
                else if (rdoJob1.Checked == true)   //주민번호
                {
                    strJob = "1";
                }
                else if (rdoJob2.Checked == true)   //회사코드
                {
                    strJob = "2";
                }
                else if (rdoJob4.Checked == true)   //접수번호
                {
                    strJob = "4";
                }
                else if (rdoJob5.Checked == true)   //등록번호
                {
                    strJob = "5";
                }
                else if (rdoJob6.Checked == true)   //휴대폰
                {
                    strJob = "6";
                }

                if (rdoGubun1.Checked == true)
                {
                    strGubun = "1";
                }
                else if (rdoGubun2.Checked == true)
                {
                    strGubun = "2";
                }
                else if (rdoGubun3.Checked == true)
                {
                    strGubun = "3";
                }

                if (!strSName.IsNullOrEmpty())
                {
                    if (strJob == "1")
                    {
                        FSName = clsAES.AES(strSName);
                    }
                    else
                    {
                        FSName = strSName;
                    }
                }

                List<HIC_PATIENT> list = hicPatientService.GetItembySomeone(strJob, FSName, clsHcVariable.B04_NOT_PATIENT, strGubun);                

                SSList.ActiveSheet.RowCount = list.Count;
                SSList_Sheet1.Rows[-1].Height = 24;
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        SSList.ActiveSheet.Cells[i, 0].Text = list[i].PTNO;
                        SSList.ActiveSheet.Cells[i, 1].Text = list[i].SNAME.Trim();
                        SSList.ActiveSheet.Cells[i, 2].Text = VB.Left(clsAES.DeAES(list[i].JUMIN2), 7) + "******";
                        SSList.ActiveSheet.Cells[i, 3].Text = list[i].PANO.ToString();
                        SSList.ActiveSheet.Cells[i, 4].Text = clsAES.DeAES(list[i].JUMIN2);
                        SSList.ActiveSheet.Cells[i, 5].Text = list[i].LTDNAME;
                    }
                }
            }
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS2)
            {
                string strJepDate = "";
                string strPtno = "";
                long nWRTNO = 0;

                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(SS2, e.Column, ref boolSort, true);
                    return;
                }

                if (FstrProgID == "frmHcJepMain" || FstrProgID == "frmHaJepMain")
                {
                    if (e.Column == 9)
                    {
                        if (SS2.ActiveSheet.Cells[e.Row, 0].Text.Trim() != "종검")
                        {
                            strJepDate = SS2.ActiveSheet.Cells[e.Row, 2].Text;

                            if (!rSetJepsuGstrValue.IsNullOrEmpty())
                            {
                                rSetJepsuGstrValue(FstrPtNo, strJepDate);
                            }
                            
                            this.Close();
                            return;
                        }
                        else 
                        {
                            nWRTNO = SS2.ActiveSheet.Cells[e.Row, 1].Text.To<long>();

                            if (!rSetHaJepsuGstrValue.IsNullOrEmpty())
                            {
                                rSetHaJepsuGstrValue(nWRTNO);
                            }
                            
                            this.Close();
                            return;
                        }
                    }
                }
            }
            else if (sender == SSList)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(SSList, e.Column, ref boolSort, true);
                    return;
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS2)
            {
                string strTemp = "";
                string strJong = "";
                string strGubun = "";

                if (!SS2.ActiveSheet.Cells[e.Row, 1].Text.Trim().IsNullOrEmpty())
                {
                    clsHcVariable.GnWRTNO = SS2.ActiveSheet.Cells[e.Row, 1].Text.To<long>();
                }

                strTemp = SS2.ActiveSheet.Cells[e.Row, 7].Text.Trim();

                //검진년도,반기,접수일자,검진번호,검진종류
                strJong = VB.Pstr(strTemp, "@@", 5);

                if (SS2.ActiveSheet.Cells[e.Row, 0].Text.Trim() == "종검")
                {
                    strGubun = "HEA";
                }
                else
                {
                    strGubun = "HIC";
                }

                if (strGubun == "HIC")
                {
                    FrmHcResultView = new frmHcResultView(strGubun, clsHcVariable.GnWRTNO);
                    FrmHcResultView.StartPosition = FormStartPosition.CenterParent;
                    FrmHcResultView.ShowDialog(this);
                }
                else if(strGubun == "HEA")
                {
                    FrmHaResultView = new frmHaResultView(clsHcVariable.GnWRTNO);
                    FrmHaResultView.StartPosition = FormStartPosition.CenterParent;
                    FrmHaResultView.ShowDialog(this);
                }
                clsHcVariable.GnWRTNO = 0;
            }
            else if (sender == SSList)
            {
                long nPano = 0;
                string strSname = "";
                string strJumin = "";
                string strAddExam = "";
                int nRow = 0;
                int nREAD = 0;
                long nWRTNO = 0;

                fn_Screen_Clear();
                FstrPtNo = "";
                FstrPtNo = SSList.ActiveSheet.Cells[e.Row, 0].Text;
                nPano = SSList.ActiveSheet.Cells[e.Row, 3].Text.To<long>();
                FnPano = nPano;
                strSname = SSList.ActiveSheet.Cells[e.Row, 1].Text;
                FstrSname = strSname;
                strJumin = SSList.ActiveSheet.Cells[e.Row, 4].Text;
                FstrJumin = strJumin; 

                //일반건진자 기초자료
                HIC_PATIENT list = hicPatientService.GetJusobyPano(nPano, "");

                if (!list.IsNullOrEmpty())
                {
                    SS1.ActiveSheet.Cells[0, 1].Text = nPano.ToString();                //종검번호
                    SS1.ActiveSheet.Cells[0, 3].Text = strSname;                        //수진자명
                    if (VB.Mid(strJumin, 7, 1) == "1" || VB.Mid(strJumin, 7, 1) == "3") //성별
                    {
                        SS1.ActiveSheet.Cells[1, 1].Text = "남";
                    }
                    else if (VB.Mid(strJumin, 7, 1) == "2" || VB.Mid(strJumin, 7, 1) == "4")
                    {
                        SS1.ActiveSheet.Cells[1, 1].Text = "여";
                    }
                    SS1.ActiveSheet.Cells[1, 3].Text = VB.Left(strJumin, 2) + "년" + VB.Mid(strJumin, 3, 2) + "월" + VB.Mid(strJumin, 5, 2) + "일";
                    SS1.ActiveSheet.Cells[2, 1].Text = VB.Left(strJumin, 6) + "-" + VB.Right(strJumin, 7);
                    SS1.ActiveSheet.Cells[2, 3].Text = list.HPHONE;
                    SS1.ActiveSheet.Cells[3, 1].Text = list.JUSO1 + " " + list.JUSO2;
                    SS1.ActiveSheet.Cells[4, 1].Text = hb.READ_Ltd_Name(list.LTDCODE.ToString());
                }

                //건진실시 자료
                //List<HIC_JEPSU_SUNAP> list2 = hicJepsuSunapService.GetItembyPaNo(nPano);
                List<HIC_JEPSU_SUNAP> list2 = hicJepsuSunapService.GetUnionItembyPaNo(nPano);

                nREAD = list2.Count;
                SS2.ActiveSheet.RowCount = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    nWRTNO = list2[i].WRTNO;
                    SS2.ActiveSheet.Cells[i, 0].Text = list2[i].GUBUN;
                    if (list2[i].GUBUN == "종검")
                    {
                        SS2.ActiveSheet.Cells[i, 0].BackColor = Color.LightPink;
                        SS2.ActiveSheet.Cells[i, 3].Text = heaExjongService.Read_ExJong_Name(list2[i].GJJONG);
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[i, 0].BackColor = Color.White;
                        SS2.ActiveSheet.Cells[i, 3].Text = hb.READ_GjJong_Name(list2[i].GJJONG);

                        if (!hicJepsuService.GetUcodesbyWrtNo(nWRTNO).IsNullOrEmpty() && list2[i].GJJONG =="11")
                        {
                            SS2.ActiveSheet.Cells[i, 3].Text = "일반+특수";
                        }

                    }
                    SS2.ActiveSheet.Cells[i, 1].Text = nWRTNO.To<string>();
                    SS2.ActiveSheet.Cells[i, 2].Text = list2[i].JEPDATE;
                    SS2.ActiveSheet.Cells[i, 4].Text = string.Format("{0:###,###,###}", list2[i].TOTAMT);
                    SS2.ActiveSheet.Cells[i, 6].Text = list2[i].SECOND_SAYU;
                    SS2.ActiveSheet.Cells[i, 7].Text = list2[i].GJYEAR + "@@" + list2[i].GJBANGI + "@@" + list2[i].JEPDATE + "@@" + nPano + "@@" + list2[i].GJJONG;
                    SS2.ActiveSheet.Cells[i, 8].Text = list2[i].JUSO1 + " " + list2[i].JUSO2; ;

                    //추가검사가 있는지 Check
                    List<HIC_SUNAPDTL_GROUPCODE> list3 = hicSunapdtlGroupcodeService.GetCodeNamebyWrtNo(nWRTNO);

                    if (list3.Count > 0)
                    {
                        strAddExam = "";
                        for (int j = 0; j < list3.Count; j++)
                        {
                            strAddExam += list3[j].NAME.Trim() + ",";
                        }
                        strAddExam = VB.Left(strAddExam, strAddExam.Length - 1);
                        SS2.ActiveSheet.Cells[i, 5].Text = strAddExam;
                    }
                }

                fn_Hic_Memo_Screen();

                nPano = 0;
                strSname = "";
                strAddExam = "";
                nWRTNO = 0;
                strJumin = "";
            }
        }

        void eRdoClick(object sender, EventArgs e)
        {
            if (sender == rdoJob0)
            {
                lblViewTitle.Text = "수진자명";
            }
            else if (sender == rdoJob1)
            {
                lblViewTitle.Text = "주민번호";
            }
            else if (sender == rdoJob2)
            {
                lblViewTitle.Text = "회사명";
            }
            else if (sender == rdoJob3)
            {
                lblViewTitle.Text = "검진번호";
            }
            else if (sender == rdoJob4)
            {
                lblViewTitle.Text = "접수번호";
            }
            else if (sender == rdoJob5)
            {
                lblViewTitle.Text = "등록번호";
            }
            else if (sender == rdoJob6)
            {
                lblViewTitle.Text = "휴대폰";
            }
            txtSName.Text = "";
            txtSName.Focus();
        }

        void eTxtGotFocus(object sender, EventArgs e)
        {
            if (sender == txtSName)
            {
                txtSName.ImeMode = ImeMode.Hangul;
            }
        }
        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtSName)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                    eBtnClick(btnSearch, new EventArgs());
                }
            }
        }
            
        void eTxtLostFocus(object sender, EventArgs e)
        {
            if (sender == txtSName)
            {
                if (txtSName.Text.Trim() == "")
                {
                    return;
                }

                if (rdoJob4.Checked == true)
                {
                    txtSName.Text = string.Format("{0:00000000}", txtSName.Text);
                }
            }
        }

        void fn_Screen_Clear()
        {
            //lblViewTitle.Text = "수진자명";
            for (int i = 0; i <= 2; i++)
            {
                SS1.ActiveSheet.Cells[i, 1].Text = "";
                SS1.ActiveSheet.Cells[i, 3].Text = "";
            }

            for (int i = 3; i < 4; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    SS1.ActiveSheet.Cells[i, j].Text = "";
                }
            }
            sp.Spread_All_Clear(SS2);
        }

        void fn_Hic_Memo_Save()
        {
            long nPano = 0;
            string strCODE = "";
            string strMemo = "";
            string strROWID = "";
            string strOK = "";
            string strTime = "";
            int result = 0;

            if (SS1.ActiveSheet.Cells[0, 1].Text.IsNullOrEmpty())
            {
                return;
            }

            nPano = SS1.ActiveSheet.Cells[0, 1].Text.To<long>();
            if (FstrPtNo.IsNullOrEmpty())
            {
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            for (int i = 0; i < SS_ETC.ActiveSheet.NonEmptyRowCount; i++)
            {
                strOK = SS_ETC.ActiveSheet.Cells[i, 0].Text.Trim();
                strTime = SS_ETC.ActiveSheet.Cells[i, 2].Text.Trim();
                strMemo = SS_ETC.ActiveSheet.Cells[i, 3].Text.Trim();
                strROWID = SS_ETC.ActiveSheet.Cells[i, 5].Text.Trim();

                if (strROWID != "")
                {
                    if (strOK == "True")
                    {
                        result = hicMemoService.UpdatebyRowId(strROWID);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("수검자 메모 저장 오류!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                else if (strTime == "" && strMemo != "")    //신규작성일경우
                {
                    HIC_MEMO item = new HIC_MEMO();

                    item.PANO = nPano;
                    item.MEMO = strMemo;
                    item.PTNO = FstrPtNo;
                    item.JOBSABUN = clsType.User.IdNumber.To<long>();

                    result = hicMemoService.InsertPanoMemoJobsabun(item);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("수검자 메모 저장 오류!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            clsDB.setCommitTran(clsDB.DbCon);
        }

        void fn_Hic_Memo_Screen()
        {
            long nPano = 0;
            int nRead = 0;

            sp.Spread_All_Clear(SS_ETC);
            SS_ETC.ActiveSheet.RowCount = 5;

            if (SS1.ActiveSheet.Cells[0, 1].Text.IsNullOrEmpty())
            {
                return;
            }

            nPano = long.Parse(SS1.ActiveSheet.Cells[0, 1].Text);
            if (nPano == 0)
            {
                return;
            }

            //참고사항 Display
            //List<HIC_MEMO> list = hicMemoService.GetItembyPaNo(nPano);
            List<HIC_MEMO> list = hicMemoService.GetItembyPaNo(FstrPtNo, "");

            nRead = list.Count;
            SS_ETC.ActiveSheet.RowCount = nRead + 5;
            for (int i = 0; i < nRead; i++)
            {
                //SS_ETC_Sheet1.Rows.Get(i).Height = 24F;

                SS_ETC.ActiveSheet.Cells[i, 1].Text = list[i].JOBGBN.ToString();
                SS_ETC.ActiveSheet.Cells[i, 2].Text = list[i].ENTTIME.ToString();
                SS_ETC.ActiveSheet.Cells[i, 3].Text = list[i].MEMO.Trim();
                SS_ETC.ActiveSheet.Cells[i, 4].Text = list[i].JOBNAME;
                SS_ETC.ActiveSheet.Cells[i, 5].Text = list[i].RID;

                //Row 높이 설정 2021-01-27
                FarPoint.Win.Spread.Row row;
                row = SS_ETC.ActiveSheet.Rows[i];
                float rowSize = row.GetPreferredHeight();
                SS_ETC_Sheet1.Rows.Get(i).Height = rowSize;
            }
        }
    }
}
