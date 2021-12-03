using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray
    /// File Name       : frmComSupXraySET17.cs
    /// Description     : 영상의학과 판독프로그램에 사용되는 상용결과보기
    /// Author          : 윤조연
    /// Create Date     : 2018-05-10
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "신규폼 frmComSupXraySET17.cs 폼이름 재정의" />
    public partial class frmComSupXraySET17 : Form
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic(); //공용함수
        conPatInfo pinfo = new conPatInfo(); //공통정보
        clsComSup sup = new clsComSup();
        clsComSupXray cxray = new clsComSupXray();
        clsComSupXraySpd cxraySpd = new clsComSupXraySpd();
        clsComSupXraySQL cxraySql = new clsComSupXraySQL();
        clsComSupXrayRead cRead = new clsComSupXrayRead();
        clsComSupSpd supSpd = new clsComSupSpd();
        clsMethod method = new clsMethod();
        clsQuery Query = new clsQuery();

        clsComSupXrayRead.cXray_ResultSet cXray_ResultSet = null;

        clsComSupXrayRead.cXray_Read_Delegate cXray_Read_Delegate = null; //델리게이트

        public delegate void SendMsg(clsComSupXrayRead.cXray_Read_Delegate argCls);
        public event SendMsg rSendMsg;

        string gROWID = "";
        string gXJong = "";
        string gPart = "";//XRAY, HIC 건진판정 XRAY_ResultSet_HIC
        long gSabun = 0;

        DataTable dt_ResultSet = null;         //상용결과
        DataTable dt_ResultSet_Clone = null;   //상용결과 조건검색용


        #endregion

        public frmComSupXraySET17(string argPart, string argXJong, long argSabun)
        {
            InitializeComponent();
            gPart = argPart;
            gXJong = argXJong;
            gSabun = argSabun;

            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {
            //txtPano.Text = gstrPano;
            //lblSName.Text = gstrSName;

            setCombo();

            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            //lblDrName.Text = sup.read_bas_user(clsDB.DbCon, gSabun.ToString());

            //dt_ResultSet = cRead.sel_Xray_ResultSet(clsDB.DbCon, gSabun, gPart, gXJong, txtSearch.Text.Trim());


        }

        //권한체크
        void setAuth()
        {

            if (gPart =="XRAY")
            {
                panTitleSub0.Visible = false;
                btnExit.Visible = false;
                panel9.Visible = false;
            }


        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSet1.Click += new EventHandler(eBtnClick);
            this.btnSet2.Click += new EventHandler(eBtnClick);
            this.btnSet3.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);

            this.btnSearch.Click += new EventHandler(eBtnSearch);

            this.btnSave_OK.Click += new EventHandler(eBtnSave);
            this.btnSave_new.Click += new EventHandler(eBtnSave);
            this.btnDelete.Click += new EventHandler(eBtnSave);

            this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);

            this.txtSearch.KeyDown += new KeyEventHandler(eTxtEvent);
            //this.txtResult.KeyUp += new KeyEventHandler(eTxtKeyUp);
            //this.txtJepCode.KeyDown += new KeyEventHandler(eTxtEvent);

            //this.cboJong.SelectedIndexChanged += new EventHandler(eCboSelChanged);

            this.exSpliter1.Click += new EventHandler(eSpliterClick);


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
                cxraySpd.sSpd_XrayReadSet(ssList, cxraySpd.sSpdXrayReadSet, cxraySpd.nSpdXrayReadSet, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                
                screen_clear();

                setCtrlData();

                setAuth();

                //
                screen_display();
                //search_data(ssList,"");
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            string sPos = "";
            if (optPos1.Checked == true)
            {
                sPos = "01";
            }
            else if (optPos2.Checked == true)
            {
                sPos = "02";
            }
            else if (optPos3.Checked == true)
            {
                sPos = "03";
            }

            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == this.btnCancel)
            {
                screen_clear("CAN");
            }
            else if (sender == this.btnSet1)
            {
                setDelegate("01", sPos, txtResult.Text);
            }
            else if (sender == this.btnSet2)
            {
                setDelegate("02", sPos, txtResult.Text);
            }
            else if (sender == this.btnSet3)
            {
                if (txtResult.SelectedText.Trim() != "")
                {
                    setDelegate("03", sPos, txtResult.SelectedText);
                }
                else
                {
                    ComFunc.MsgBox("필요한 문구를 선택후 작업하세요!!");
                    txtResult.Focus();
                }
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (sender == this.btnSearch)
            {
                screen_display();
                screen_clear("VIEW");
            }
        }

        void eBtnSave(object sender, EventArgs e)
        {
            if (sender == this.btnSave_OK)
            {
                //
                eSave(clsDB.DbCon, "저장");
            }
            else if (sender == this.btnSave_new)
            {
                screen_clear("NEW");
            }
            else if (sender == this.btnDelete)
            {
                //
                eSave(clsDB.DbCon, "삭제");
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

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;

            gROWID = "";

            if (e.Row < 0 || e.ColumnHeader == true) return;
            if (e.Button != MouseButtons.Left) return;

            if (sender == this.ssList)
            {
                for (int i = 0; i < o.ActiveSheet.RowCount; i++)
                {
                    o.ActiveSheet.Rows.Get(i).BackColor = System.Drawing.Color.White;
                }

                o.ActiveSheet.Rows.Get(e.Row).BackColor = System.Drawing.Color.LightGray;

                txtSetName.Text = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayReadSet.SetName].Text.Trim();
                txtXName.Text = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayReadSet.ExName].Text.Trim();
                txtResult.Text = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayReadSet.Result].Text.Trim() + o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayReadSet.Result1].Text.Trim();
                gROWID = o.ActiveSheet.Cells[e.Row, (int)clsComSupXraySpd.enmXrayReadSet.ROWID].Text.Trim();

                screen_clear("CLICK");

            }

        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            FpSpread o = (FpSpread)sender;


            if (e.Row < 0 || e.Column < 0) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }
            if (sender == this.ssList)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(o, e.Column);
                    return;
                }
                if (e.RowHeader == true)
                {
                    return;
                }

            }


        }

        void eTxtEvent(object sender, KeyEventArgs e)
        {
            if (sender == this.txtSearch)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    screen_display();
                }
            }

        }

        void eSpliterClick(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.ExpandableSplitter ex = (DevComponents.DotNetBar.ExpandableSplitter)sender;

            //if (ex.Expanded == true)
            //{
            //    panel3.Size = new System.Drawing.Size(882, 20);
            //}
            //else
            //{
            //    panel3.Size = new System.Drawing.Size(882, 280);
            //}
        }

        void eTxtKeyUp(object sender, KeyEventArgs e)
        {
            if (sender == this.txtResult)
            {
                #region //기능키 체크


                string s = string.Empty;

                if (e.KeyCode == Keys.F1)
                {
                    s = "F1";
                }
                else if (e.KeyCode == Keys.F2)
                {
                    s = "F2";
                }
                else if (e.KeyCode == Keys.F3)
                {
                    s = "F3";
                }
                else if (e.KeyCode == Keys.F4)
                {
                    s = "F4";
                }
                else if (e.KeyCode == Keys.F5)
                {
                    s = "F5";
                }
                else if (e.KeyCode == Keys.F6)
                {
                    s = "F6";
                }
                else if (e.KeyCode == Keys.F7)
                {
                    s = "F7";
                }
                else if (e.KeyCode == Keys.F8)
                {
                    s = "F8";
                }
                else if (e.KeyCode == Keys.F9)
                {
                    s = "F9";
                }
                else if (e.KeyCode == Keys.F10)
                {
                    s = "F10";
                }
                #endregion

                if (s != "")
                {
                    DataTable dt = sup.sel_Resultward(clsDB.DbCon, Convert.ToInt32(clsType.User.IdNumber), s);
                    if (ComFunc.isDataTableNull(dt) == false)
                    {
                        txtResult.Paste(dt.Rows[0]["WardName"].ToString().Trim()); //현재커서로 붙여넣기                  
                    }
                }
            }

        }

        void eSave(PsmhDb pDbCon, string Job)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            string strResult = "";
            string strResult1 = "";
            string strSetName = txtSetName.Text.Trim().ToUpper();

            if (strSetName == "") return;
            if (txtXName.Text.Trim() == "") return;


            //확인 메시지
            if (Job == "삭제")
            {
                if (ComFunc.MsgBoxQ("해당코드를 정말로 삭제하시겠습니까??", "삭제확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }
            else if (Job == "저장")
            {
                if (gXJong == "")
                {
                    ComFunc.MsgBox("종류를 선택후 저장하십시오!!");
                    return;
                }
            }

            #region 저장 및 갱신에 사용될 배열초기화 및 변수세팅



            int nMaxLen = 2000; //결과값을 2000자 분리 처리(한글 byte 처리됨)

            string s = ComFunc.QuotConv(VB.RTrim((txtResult.Text)));

            int intLenTot = (int)ComFunc.GetWordByByte(s);

            if (intLenTot > 4000)
            {
                ComFunc.MsgBox("판독결과가 4000Byte이상입니다..!!");
                return;
            }

            if (intLenTot <= nMaxLen)
            {
                strResult = s;
            }
            else if (intLenTot > nMaxLen)
            {
                strResult = ComFunc.GetMidStr(s, 0, nMaxLen);
                strResult1 = ComFunc.GetMidStr(s, nMaxLen, intLenTot - nMaxLen);
            }


            cXray_ResultSet = new clsComSupXrayRead.cXray_ResultSet();
            cXray_ResultSet.Sabun = gSabun;
            cXray_ResultSet.Part = gPart;
            cXray_ResultSet.XJong = gXJong;
            cXray_ResultSet.SetName = txtSetName.Text.Trim();
            cXray_ResultSet.XName = txtXName.Text.Trim();
            cXray_ResultSet.Result1 = strResult;
            cXray_ResultSet.Result2 = strResult1;
            cXray_ResultSet.ROWID = gROWID;




            #endregion

            // clsTrans DT = new clsTrans();
            clsDB.setBeginTran(pDbCon);

            try
            {
                if (Job == "저장")
                {
                    if (cXray_ResultSet.ROWID != "")
                    {
                        SqlErr = cRead.up_Xray_ResultSet(pDbCon, cXray_ResultSet, ref intRowAffected);
                    }
                    else
                    {
                        SqlErr = cRead.ins_Xray_ResultSet(pDbCon, cXray_ResultSet, ref intRowAffected);
                    }
                }
                else if (Job == "삭제")
                {
                    SqlErr = cRead.del_Xray_ResultSet(pDbCon, cXray_ResultSet, ref intRowAffected);
                }

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                    return;
                }
                else
                {
                    clsDB.setCommitTran(pDbCon);

                    screen_clear();
                    screen_display();
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장    
            }

        }

        void eCboSelChanged(object sender, EventArgs e)
        {
            ComboBox o = (ComboBox)sender;
            //조회
            try
            {
                if (o.SelectedItem.ToString() != null)
                {
                    gXJong = setJong(o.Text.Trim());
                    if (gXJong == "*")
                    {
                        gXJong = "";
                    }
                    screen_display();
                }

            }
            catch
            {

            }

        }

        void setDelegate(string argJob, string argPos, string argRemark)
        {
            if (rSendMsg == null)
            {
                return;
            }

            //델리게이트용 변수
            cXray_Read_Delegate = new clsComSupXrayRead.cXray_Read_Delegate();
            cXray_Read_Delegate.Job = argJob;
            cXray_Read_Delegate.sPos = argPos; //01.현재커서 02.처음 03.마지막               
            cXray_Read_Delegate.Sogen = argRemark;

            //델리게이트
            rSendMsg(cXray_Read_Delegate);

            //this.Hide();
        }

        string setJong(string argXJong)
        {
            string s = string.Empty;

            s = VB.Left(argXJong, 1);
            if (s == "*")
            {
                s = "";
            }

            return s;
        }

        void setCombo()
        {
            ////검사종류
            //method.setCombo_View(this.cboJong, Query.Get_BasBcode(clsDB.DbCon, "C#_XRAY_접수종류", "", " Code|| '.' || Name Names ", " AND CODE IN ('1','2','3','4','5','6','7','8','9') "), clsParam.enmComParamComboType.ALL);

            //if (gXJong != "")
            //{
            //    DataTable dt = Query.Get_BasBcode(clsDB.DbCon, "C#_XRAY_접수종류", gXJong, " Code|| '.' || Name Names ");
            //    if (ComFunc.isDataTableNull(dt) == false)
            //    {
            //        cboJong.Text = dt.Rows[0]["Names"].ToString().Trim();
            //    }

            //}
        }

        void search_ResultSet_data(FpSpread Spd, string argSearch)
        {

            string s = string.Empty;

            dt_ResultSet_Clone = null;
            dt_ResultSet_Clone = dt_ResultSet.Clone();

            if (argSearch != "")
            {
                s += "   AND (                                                        \r\n";
                s += "     UPPER(SetName) LIKE '%" + argSearch.ToUpper() + "%'        \r\n";
                s += "     OR  UPPER(XName) LIKE '%" + argSearch.ToUpper() + "%'      \r\n";
                s += "     OR  UPPER(Result1) LIKE '%" + argSearch.ToUpper() + "%'    \r\n";
                s += "      )                                                         \r\n";
            }

            string strWhere = s;
            string strOrderby = "XJong,SetName";

            foreach (DataRow dr in dt_ResultSet.Select(strWhere, strOrderby))
            {
                dt_ResultSet_Clone.ImportRow(dr);
            }

            Spd.ActiveSheet.RowCount = 0;
            Spd.ActiveSheet.RowCount = dt_ResultSet_Clone.Rows.Count;

            if (dt_ResultSet_Clone.Rows.Count > 0)
            {
                for (int i = 0; i < dt_ResultSet_Clone.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayReadSet.XJong].Text = dt_ResultSet_Clone.Rows[i]["XJong"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayReadSet.XJongName].Text = dt_ResultSet_Clone.Rows[i]["FC_XJong"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayReadSet.SetName].Text = dt_ResultSet_Clone.Rows[i]["SetName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayReadSet.ExName].Text = dt_ResultSet_Clone.Rows[i]["Xname"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayReadSet.Result].Text = dt_ResultSet_Clone.Rows[i]["Result1"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayReadSet.Result1].Text = dt_ResultSet_Clone.Rows[i]["Result2"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayReadSet.ROWID].Text = dt_ResultSet_Clone.Rows[i]["ROWID"].ToString().Trim();
                }

                // 화면상의 정렬표시 Clear
                Spd.ActiveSheet.ColumnHeader.Cells[0, 0, 0, Spd.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            }


            dt_ResultSet_Clone = null;
        }

        void screen_clear(string argJob = "")
        {
            //
            read_sysdate();

            btnSave_new.Enabled = false;
            btnSave_OK.Enabled = false;
            btnDelete.Enabled = false;

            if (argJob == "")
            {
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
                        ((CheckBox)ctl).Checked = false;
                    }
                    else if (ctl is RadioButton)
                    {
                        //((RadioButton)ctl).Checked = false;
                    }

                }


                gROWID = "";

                btnSave_new.Enabled = true;

                txtSearch.Select();
            }
            else if (argJob == "A1")
            {
                txtSetName.Select();
            }
            else if (argJob == "A2")
            {
                txtSearch.Select();
            }
            else if (argJob == "NEW")
            {
                txtSetName.Select();
                btnSave_OK.Enabled = true;
            }
            else if (argJob == "VIEW" || argJob == "CAN")
            {
                btnSave_new.Enabled = true;
                txtSearch.Select();
            }
            else if (argJob == "CLICK")
            {
                btnSave_OK.Enabled = true;
                btnDelete.Enabled = true;
                txtSearch.Select();
            }

        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void screen_display()
        {
            //if (gXJong == "")
            //{
            //    gXJong = setJong(cboJong.Text.Trim());
            //}
            GetData(clsDB.DbCon, ssList, gXJong);
        }

        void GetData(PsmhDb pDbCon, FpSpread Spd, string argXJong)
        {
            int i = 0;
            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            dt = cRead.sel_Xray_ResultSet(pDbCon, gSabun, gPart, argXJong, txtSearch.Text.Trim());

            #region //데이터셋 읽어 자료 표시

            if (dt != null && dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayReadSet.XJong].Text = dt.Rows[i]["XJong"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayReadSet.XJongName].Text = dt.Rows[i]["FC_XJong"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayReadSet.SetName].Text = dt.Rows[i]["SetName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayReadSet.ExName].Text = dt.Rows[i]["Xname"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayReadSet.Result].Text = dt.Rows[i]["Result1"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayReadSet.Result1].Text = dt.Rows[i]["Result2"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupXraySpd.enmXrayReadSet.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                }
                // 화면상의 정렬표시 Clear
                Spd.ActiveSheet.ColumnHeader.Cells[0, 0, 0, Spd.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            }

            #endregion

            Cursor.Current = Cursors.Default;

        }

    }
}
