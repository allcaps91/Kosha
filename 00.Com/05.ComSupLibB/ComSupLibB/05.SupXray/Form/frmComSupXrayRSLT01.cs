using ComBase;
using ComDbB;
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXrayRSLT01.cs
    /// Description     : 영상의학과 판독프로그램에 사용되는 조직검사결과 확인
    /// Author          : 윤조연
    /// Create Date     : 2018-02-13
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xuread\xupacs\Frm조직검사결과.frm(Frm조직검사결과) >> frmComSupXrayRSLT01.cs 폼이름 재정의" />
    public partial class frmComSupXrayRSLT01 : Form
    {
        #region 클래스 선언 및 etc....

        clsComSup sup = new clsComSup();
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsQuery Query = new clsQuery();
        clsPublic cpublic = new clsPublic(); //공용함수        
        clsComSupSpd supSpd = new clsComSupSpd();
        clsComSupXray cxray = new clsComSupXray();
        clsComSupXrayRead cRead = new clsComSupXrayRead();
        clsComSupXraySpd cxraySpd = new clsComSupXraySpd();
        clsComSupXraySQL cxraySql = new clsComSupXraySQL();
        clsComSupXrayRead cxreadSql = new clsComSupXrayRead();               

        clsComSupXrayRead.cXray_ResultNew cXray_ResultNew = null;


        #endregion

        public frmComSupXrayRSLT01()
        {
            InitializeComponent();
            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;            

            screen_clear();

            cXray_ResultNew = new clsComSupXrayRead.cXray_ResultNew();
            cXray_ResultNew.XDrCode1 = Convert.ToInt32(clsType.User.Sabun);

            dtpFDate.Text = Convert.ToDateTime(cpublic.strSysDate).AddDays(-10).ToShortDateString();
            dtpTDate.Text = cpublic.strSysDate;

            //btnSave2.Enabled = true;

        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnClick);
            
            this.btnSearch.Click += new EventHandler(eBtnSearch);
            this.btnSave.Click += new EventHandler(eBtnSave);
            //this.btnDelete.Click += new EventHandler(eBtnSave);

            //this.txtHan.KeyDown += new KeyEventHandler(eTxtEvent);

            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssList.ButtonClicked += new EditorNotifyEventHandler(eSpreadBtnClicked);

        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {
                //            
                cxraySpd.sSpd_enmXrayRSLT01(ssList, cxraySpd.sSpdenmXrayRSLT01, cxraySpd.nSpdenmXrayRSLT01, 5, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                //ComFunc.SetAllControlClear(this); //컨트롤 초기화

                screen_clear();

                setCtrlData();

                //
                //screen_display();
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (sender == this.btnSearch)
            {
                //
                screen_display();
            }
        }

        void eBtnSave(object sender, EventArgs e)
        {
            if (sender == this.btnSave)
            {
                //
                eSave(clsDB.DbCon, "등록");
            }

        }

        void eBtnPrint(object sender, EventArgs e)
        {
            //if (sender == this.btnPrint)
            //{
            //    ePrint();
            //}
        }

        void eBtnPopUp(object sender, EventArgs e)
        {

        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;
            DataTable dt = null;

            if (e.Row < 0 || e.Column < 0) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }

            if (sender == this.ssList)
            {
                #region //변수세팅
                string strPano = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayRSLT01.Pano].Text.Trim();
                string strSName = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayRSLT01.SName].Text.Trim();
                string strGubun = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayRSLT01.Gubun].Text.Trim();
                string strROWID = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayRSLT01.ROWID].Text.Trim();
                string strResult = "";
                string s = "(" + strPano + " " + strSName + ")";
                #endregion

                txtAnat.Text = "";
                txtXray.Text = "";

                if (strGubun =="XRAY")
                {
                    #region //판독정보
                    cXray_ResultNew.Job = "01";
                    cXray_ResultNew.ROWID = strROWID;
                    dt = cRead.sel_XRAY_RESULTNEW(clsDB.DbCon, cXray_ResultNew,false);
                    if (ComFunc.isDataTableNull(dt) == false)
                    {
                        strResult = dt.Rows[0]["Result"].ToString();
                        if (dt.Rows[0]["Result1"].ToString().Trim() !="")
                        {
                            strResult += dt.Rows[0]["Result1"].ToString();
                        }
                        txtXray.Text = strResult;
                        lblXray.Text = "영상 판독결과" + s;
                    }
                    #endregion

                }
                else if (strGubun == "EXAM")
                {
                    #region //조직검사 정보
                    dt = cxraySql.sel_EXAM_ANATMST(clsDB.DbCon, "00",strROWID,"");
                    if (ComFunc.isDataTableNull(dt) == false)
                    {
                        strResult = dt.Rows[0]["Result1"].ToString();
                        if (dt.Rows[0]["Result2"].ToString().Trim() != "")
                        {
                            strResult += dt.Rows[0]["Result2"].ToString();
                        }
                        txtAnat.Text = strResult;
                        lblAnat.Text = "조직 검사결과" + s ;
                    }
                    #endregion
                }

            }

        }

        void eSpreadBtnClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {     
            if (e.Row < 0 || e.Column < 0) return;

            FpSpread o = (FpSpread)sender;

            if (sender == this.ssList)
            {
                if (e.Column == (int)clsComSupXraySpd.enmXrayRSLT01.chk)
                {
                    if (o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayRSLT01.chk].Text == "True")
                    {
                        o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayRSLT01.Change].Text = "Y";
                    }
                    else
                    {
                        o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayRSLT01.Change].Text = "Y";
                    }
                }           
            
            }

        }

        void eTxtEvent(object sender, KeyEventArgs e)
        {
            //if (sender == this.txtHan)
            //{
            //    if (e.KeyCode == Keys.Enter)
            //    {
            //        btnSave.Enabled = true;
            //    }
            //}

        }

        void eSave(PsmhDb pDbCon, string argJob, int argRow = -1)
        {
            int i = 0;
            int nstartRow = 0;
            int nLastRow = -1;

            string SQL = "";
            string strROWID = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수                        


            if (ComFunc.MsgBoxQ("선택한걸을 확인 처리 하시겠습니까 ?", "등록확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            read_sysdate();

            if (argRow == -1)
            {
                nLastRow = ssList.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1;
            }
            else
            {
                nstartRow = argRow;
                nLastRow = argRow + 1;
            }

            clsDB.setBeginTran(pDbCon);

            try
            {
                for (i = nstartRow; i < nLastRow; i++)
                {
                    if (ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayRSLT01.Gubun].Text.Trim() =="XRAY")
                    {
                        strROWID = ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayRSLT01.ROWID].Text.Trim();
                        if (strROWID !="")
                        {
                            if (ssList.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayRSLT01.chk].Text.Trim() == "True")
                            {
                                SqlErr = cRead.up_XRAY_RESULTNEW(pDbCon, strROWID, "", " GbAnat = 'Y' ", "", ref intRowAffected);
                            }
                            else
                            {
                                SqlErr = cRead.up_XRAY_RESULTNEW(pDbCon, strROWID, "", " GbAnat = 'N' ", "", ref intRowAffected);
                            }
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                return;
                            }
                        }                        
                    }                   
                }

                if (SqlErr == "")
                {
                    clsDB.setCommitTran(pDbCon);
                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
            }
            
            //조회            
            screen_display();
            screen_clear();
        }

        void screen_display()
        {
            GetData(clsDB.DbCon, ssList, dtpFDate.Text.Trim(),dtpTDate.Text.Trim());
        }

        void screen_clear()
        {
            //
            read_sysdate();

            btnSave.Enabled = false;

            //콘트롤 값 clear
            Control[] controls = ComFunc.GetAllControls(this);

            foreach (Control ctl in controls)
            {

                if (ctl is TextBox)
                {
                    ctl.Text = "";
                }
                else if (ctl is CheckBox)
                {
                    //((CheckBox)ctl).Checked = false;
                }
                else if (ctl is RadioButton)
                {
                    ((RadioButton)ctl).Checked = false;
                }
                else if (ctl is DateTimePicker)
                {
                    if (((DateTimePicker)ctl).Name == "dtpDate")
                    {
                        ((DateTimePicker)ctl).Text = "";
                    }

                }

            }

            lblXray.Text = "영상 판독결과";
            lblAnat.Text = "조직 검사결과";

        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void GetData(PsmhDb pDbCon, FpSpread Spd, string argDate1,string argDate2)
        {
            int i = 0;
            int j = 0;
            int nRow = -1;

            DataTable dt = null;
            DataTable dt2 = null;
            bool strOK = false;
            string strBDate = "";
            
            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            dt = cxraySql.sel_EXAM_ANATMST(pDbCon, argDate1, argDate2);

            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    
                    strOK = false;

                    #region //기본체크 및 쿼리
                    strBDate = dt.Rows[i]["BDate"].ToString().Trim();

                    cXray_ResultNew.Job = "04";
                    cXray_ResultNew.Pano = dt.Rows[i]["Ptno"].ToString().Trim();
                    cXray_ResultNew.SDate = Convert.ToDateTime(strBDate).AddDays(-10).ToShortDateString();
                    cXray_ResultNew.TDate = Convert.ToDateTime(strBDate).AddDays(10).ToShortDateString();                    
                    cXray_ResultNew.AnatChk = "";
                    if (chkAnat.Checked == true)
                    {
                        cXray_ResultNew.AnatChk = "Y";
                    }
                    cXray_ResultNew.MyChk = "";                    
                    if (chkMy.Checked == true)
                    {
                        cXray_ResultNew.MyChk = "Y";
                    }

                    dt2 = cRead.sel_XRAY_RESULTNEW(pDbCon, cXray_ResultNew,false);
                    if (ComFunc.isDataTableNull(dt2) == false)
                    {
                        strOK = true;
                    }
                    #endregion

                    if (strOK == true)
                    {
                        nRow++;
                        if (Spd.ActiveSheet.RowCount <= nRow)
                        {
                            Spd.ActiveSheet.RowCount = nRow + 1;
                        }

                        #region //기본정보표시
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayRSLT01.chk].Text = "";
                        sup.setColStyle_Text(Spd, nRow, (int)clsComSupXraySpd.enmXrayRSLT01.chk, false, false, true); //txt재정의            
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayRSLT01.ResultDate].Text = dt.Rows[i]["ResultDate"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayRSLT01.Pano].Text = cXray_ResultNew.Pano;
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayRSLT01.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayRSLT01.GbIO].Text = dt.Rows[i]["GbIO"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayRSLT01.DeptCode].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayRSLT01.OrderDate].Text = dt.Rows[i]["BDate"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayRSLT01.ExName].Text = dt.Rows[i]["OrderName"].ToString().Trim();
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayRSLT01.Gubun].Text = "EXAM";
                        Spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayRSLT01.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        #endregion

                        #region //조직결과 상세표시
                        if (dt2.Rows.Count >0)
                        {
                            for (j = 0; j < dt2.Rows.Count; j++)
                            {
                                nRow++;

                                if (Spd.ActiveSheet.RowCount <= nRow)
                                {
                                    Spd.ActiveSheet.RowCount = nRow+1;
                                }
                                methodSpd.setColStyle(Spd, nRow, (int)clsComSupXraySpd.enmXrayRSLT01.chk, clsSpread.enmSpdType.CheckBox);
                                if (dt2.Rows[j]["GbAnat"].ToString().Trim() =="Y")
                                {
                                    Spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayRSLT01.chk].Text = "1";
                                }
                                Spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayRSLT01.Pano].Text = cXray_ResultNew.Pano;
                                Spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayRSLT01.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                                Spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayRSLT01.DeptCode].Text = "판독";
                                Spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayRSLT01.OrderDate].Text = dt2.Rows[j]["ReadDate"].ToString().Trim();
                                Spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayRSLT01.ExCode].Text = dt2.Rows[j]["XCode"].ToString().Trim();
                                Spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayRSLT01.ExName].Text = dt2.Rows[j]["XName"].ToString().Trim();
                                Spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayRSLT01.Gubun].Text = "XRAY";
                                Spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayRSLT01.Change].Text = "";
                                Spd.ActiveSheet.Cells[nRow, (int)clsComSupXraySpd.enmXrayRSLT01.ROWID].Text = dt2.Rows[j]["ROWID"].ToString().Trim();

                                Spd.ActiveSheet.Rows.Get(nRow).BackColor = System.Drawing.Color.LightGray;

                            }
                        }
                        #endregion

                    }

                }

            }

            Spd.ActiveSheet.RowCount = nRow+1;

            #endregion

            btnSave.Enabled = true;

            Cursor.Current = Cursors.Default;
            
        }


    }
}
