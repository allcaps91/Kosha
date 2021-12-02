using ComLibB;
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.DataAccess.Client;  //이 참조는 필요없음

namespace ComSupLibB.SupPthl
{

    /// <summary>
    /// Class Name      : ComSupLibB.SupLbEx
    /// File Name       : frmComSupPthlTAT01.cs
    /// Description     : MIC검사리스트
    /// Author          : 김홍록
    /// Create Date     : 2017-06-09
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref="d:\psmh\exam\exanat\Frm병리TAT.frm"/>
    public partial class frmComSupPthlTAT01 : Form, MainFormMessage
    {

        #region //MainFormMessage
        public MainFormMessage mCallForm = null;
        public void MsgActivedForm(Form frm)
        {

        }
        public void MsgUnloadForm(Form frm)
        {

        }
        public void MsgFormClear()
        {

        }
        public void MsgSendPara(string strPara)
        {

        }
        #endregion //MainFormMessage

        public frmComSupPthlTAT01(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;

            setEvent();
        }

        public frmComSupPthlTAT01()
        {
            InitializeComponent();
            setEvent();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
            }
            else
            {
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "N", "Y"); //폼 기본값 세팅 등
                setCtrl();
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        DateTime sysdate;
        clsPthlSQL pthlSQL = new clsPthlSQL();
        clsComSQL comSql = new clsComSQL();
        clsMethod method = new clsMethod();
        clsSpread spread = new clsSpread();

        Thread thread;

        clsPthlSQL.enmANATMST_TYPE gEnmPthlType;
        string[] gArrCombo;

        enum gEnmIn {OVER,OK,ALL};
        DataSet ds = null;

        void setEvent()
        {
            this.Load += new EventHandler(eFormLoad);            
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnSearch.Click += new EventHandler(eBtnSearch);
            this.btnSave.Click += new EventHandler(eBtnSave);
            this.btnPrint.Click += new EventHandler(eBtnPrint);

            this.btnClear.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);

            this.rdoSearchAll.Click += new EventHandler(eRdoClick);
            this.rdoSearchDeilay.Click += new EventHandler(eRdoClick);
            this.rdoSearchIn.Click += new EventHandler(eRdoClick);
            this.rdo_A.Click += new EventHandler(eRdoClick);
            this.rdo_T.Click += new EventHandler(eRdoClick);



            this.ssMain.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            this.ssMain.EditChange += new EditorNotifyEventHandler(eSpreadEditChange);


        }

        void setCtrl()
        {
            setCtrlCombo();
            setCtrlTitle();
            setCtrlDate();
            setCtrlSpread();

            gEnmPthlType = clsPthlSQL.enmANATMST_TYPE.A;
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            setCtrlSpread();
        }

        void eBtnSave(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            setSave();
        }

        void eBtnPrint(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return; //권한 확인
            }

            setCtrlSpreadPrint();
        }

        void eRdoClick(object sender, EventArgs e)
        {
            //기준내 클릭
            if (sender == this.rdoSearchIn)
            {
                this.grpSearchIn.Enabled = true;
            }
            else
            {
                if (sender == this.rdo_A)
                {
                    gEnmPthlType = clsPthlSQL.enmANATMST_TYPE.A;
                }
                else if (sender == this.rdo_T)
                {
                    gEnmPthlType = clsPthlSQL.enmANATMST_TYPE.T;
                }
                else
                {
                    this.grpSearchIn.Enabled = false;
                }
            }


        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {                
                this.Close();
                return;
            }
            else if (sender == this.btnClear)
            {
                this.ssMain.ActiveSheet.RowCount = 0;
            }            
        } 

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {
            string strAntNo = string.Empty;
            string strPano = string.Empty;
            string strResult1 = string.Empty;
            string strResult2 = string.Empty;

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(this.ssMain, e.Column);
            }
            else
            {
                if (e.Column < (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.GBTATSAU)
                {
                    strAntNo = this.ssMain.ActiveSheet.Cells[e.Row, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.ANATNO].Text;
                    strPano = this.ssMain.ActiveSheet.Cells[e.Row, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.PTNO].Text;
                    strResult1 = this.ssMain.ActiveSheet.Cells[e.Row, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.RESULT1].Text;
                    strResult2 = this.ssMain.ActiveSheet.Cells[e.Row, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.RESULT2].Text;

                    //TODO : 그레이브시티 문의중...DataSource를 통해서 값을 갖고 오면 엔터가 안들어 온다.
                    if (ComFunc.isDataSetNull(this.ds) == false)
                    {
                        DataRow[] dr = this.ds.Tables[0].Select("ANATNO = '" + strAntNo + "'");
                        strResult1 = dr[0][(int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.RESULT1].ToString();
                        strResult2 = dr[0][(int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.RESULT2].ToString();
                    }

                    int n = strResult1.IndexOf((char)13);

                    frmComSupINFO f = new frmComSupINFO("결과내용", "조직번호 : " + strAntNo + " " + "등록번호 : " + strPano, strResult1 + "\r\n" + strResult2);
                    f.ShowDialog();

                }
            }
        }

        void eSpreadEditChange(object sender, EditorNotifyEventArgs e)
        {
            this.ssMain.ActiveSheet.Cells[e.Row, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.CHANGE].Text = "C";
        } 

        void setCtrlCombo()
        {
            string []arrCombo = {
            ""
            ,"01.토.일 휴무"
            ,"02.공휴일 휴무"
            ,"03.금.토.일 휴무"
            ,"04.토.일.월 휴무"
            ,"05.3일 연휴(추석.설날)"
            ,"06.장기휴무(4일이상)"
            ,"07.업무시간 이후 접수"
            ,"08.충분한 고정(1일)"
            ,"09.탈회(2일)"
            ,"10.재박절(1일)"
            ,"11.추가블록(1일)"
            ,"12.특수염색(2일)"
            ,"13.외부자문(7일)"
            ,"14.학회기간"
            ,"15.외부의뢰검사"
            ,"16.결과수정"
            ,"20.기타"
            };

            this.gArrCombo = arrCombo;

        }

        void setCtrlTitle()
        {
            //세포
            if (this.gEnmPthlType == clsPthlSQL.enmANATMST_TYPE.A)
            {
                this.lblTitle.Text = "세포 TAT";
                this.lblTitleSub0.Text = "세포 TAT 리스트";

            }
            //조직
            else if (this.gEnmPthlType == clsPthlSQL.enmANATMST_TYPE.T)
            {
                this.lblTitle.Text = "조직 TAT";
                this.lblTitleSub0.Text = "조직 TAT 리스트";
            }
        }

        void setCtrlDate()
        {
            sysdate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            this.dtpFDate.Value = sysdate.AddDays(-7);
            this.dtpTDate.Value = sysdate;
        }

        void setSave()
        {
            // clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);
            string SqlErr = string.Empty;

            int nRow = 0;
            int chkRow = 0;
            string SQL = string.Empty;


            if (this.ssMain.ActiveSheet.Rows.Count > 0)
            {
                for (int i = 0; i < this.ssMain.ActiveSheet.Rows.Count; i++)
                {
                    if (this.ssMain.ActiveSheet.Cells[i, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.CHANGE].Text == "C"
                         || this.ssMain.ActiveSheet.Cells[i, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.GBTATSAU].Text != ""
                         || this.ssMain.ActiveSheet.Cells[i, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.GBTATBUN].Text != ""
                         || this.ssMain.ActiveSheet.Cells[i, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.GBTATBUN2].Text != ""
                       )
                    {
                        SqlErr = pthlSQL.up_EXAM_ANATMST(
                                                          clsDB.DbCon
                                                        , this.ssMain.ActiveSheet.Cells[i, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.GBTATSAU].Text
                                                        , this.ssMain.ActiveSheet.Cells[i, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.GBTATBUN].Text
                                                        , this.ssMain.ActiveSheet.Cells[i, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.GBTATBUN2].Text
                                                        , this.ssMain.ActiveSheet.Cells[i, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.GBTATBUN3].Text
                                                        , this.ssMain.ActiveSheet.Cells[i, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.ROWID_R].Text
                                                        , ref nRow
                                                        );
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        chkRow += 1;
                    }
                }

                if (chkRow > 0)
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox(chkRow.ToString() + " 건을 저장하였습니다."); 

                    setCtrlSpread();
                }
            }
            else
            {
                ComFunc.MsgBox("저장할 내용이 없습니다.");
            }







        }

        void setCtrlSpread()
        {
            string strFDate = this.dtpFDate.Value.ToString("yyyy-MM-dd");
            string strTDate = this.dtpTDate.Value.AddDays(1).ToString("yyyy-MM-dd");

            clsPthlSQL.enumTAT_SEARCH_TYPE enmType = clsPthlSQL.enumTAT_SEARCH_TYPE.ALL;

            if (this.rdoSearchIn.Checked == true)
            {
                enmType = clsPthlSQL.enumTAT_SEARCH_TYPE.IN;
            }
            else if (this.rdoSearchDeilay.Checked == true)
            {
                enmType = clsPthlSQL.enumTAT_SEARCH_TYPE.DEILY;
            }

            gEnmIn _enmIn = gEnmIn.ALL;

            if (this.rdoLimtOK.Checked == true)
            {
                _enmIn = gEnmIn.OK;
            }
            else if (this.rdoLimtOver.Checked == true)
            {
                _enmIn = gEnmIn.OVER;
            }

            thread = new Thread(() => threadSetCtrlSpread(strFDate,strTDate,enmType,this.chkExcep.Checked,_enmIn));
            thread.Start();           
        }

        void setCtrlSpreadPrint()
        {
            string strHeader = string.Empty;
            string strFoot = string.Empty;

            string s = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-");

            if (this.ssMain.ActiveSheet.Rows.Count > 0)
            {
                string strSearch = "";
                if (this.rdoSearchAll.Checked == true)
                {
                    strSearch = " (전체)";
                }
                else if (this.rdoSearchIn.Checked == true)
                {
                    strSearch = " (기준내)";
                }
                else if (this.rdoSearchDeilay.Checked == true)
                {
                    strSearch = " (지연)";
                }
                 
                if (this.gEnmPthlType == clsPthlSQL.enmANATMST_TYPE.T)
                {                    
                    strHeader = spread.setSpdPrint_String("세포 TAT 현황" + strSearch, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                }
                else if (this.gEnmPthlType == clsPthlSQL.enmANATMST_TYPE.A)
                {
                    strHeader = spread.setSpdPrint_String("조직 TAT 현황" + strSearch, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                }

                ssMain.ActiveSheet.Columns[(int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.GBTATBUN].Visible = false;
                ssMain.ActiveSheet.Columns[(int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.GBTATBUN2].Visible = false;


                strHeader += spread.setSpdPrint_String("조회기간:" + dtpFDate.Value.ToString("yyyy-MM-dd") + "~" + dtpTDate.Value.ToString("yyyy-MM-dd"), new Font("굴림체", 9, FontStyle.Bold), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += spread.setSpdPrint_String("출력시간:" + s + "       PAGE : /p / /pc", new Font("굴림체", 9, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

                strFoot = "/c/f1포항성모병원 병리과";

                clsSpread.SpdPrint_Margin setMargin = new clsSpread.SpdPrint_Margin(1, 1, 1, 1, 1, 1);
                clsSpread.SpdPrint_Option setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, false, false, false, false);

                spread.setSpdPrint(this.ssMain, true, setMargin, setOption, strHeader, strFoot);

                ssMain.ActiveSheet.Columns[(int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.GBTATBUN].Visible = true;
                ssMain.ActiveSheet.Columns[(int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.GBTATBUN2].Visible = true;

            }
        }

        void threadSetCtrlSpread(string strFDate, string strTDate, clsPthlSQL.enumTAT_SEARCH_TYPE enmType, bool isChk, gEnmIn enmIn)
        {

            DataSet ds_tmp;

            this.Invoke(new delegateSetCtrlCircular(setCtrlCircular), true);
            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = false));

            ds_tmp = pthlSQL.sel_EXAM_ANATMST_TAT(clsDB.DbCon, strFDate, strTDate, this.gEnmPthlType, enmType, isChk);
            ds = ds_tmp.Clone();

            if (ComFunc.isDataSetNull(ds_tmp) == false)
            {
                if (enmType == clsPthlSQL.enumTAT_SEARCH_TYPE.IN)
                {
                    if (enmIn == gEnmIn.OVER)
                    {
                        foreach (DataRow item in ds_tmp.Tables[0].Select("ILLGAP > 0"))
                        {
                            ds.Tables[0].ImportRow(item);
                        }
                    }
                    else if (enmIn == gEnmIn.OK)
                    {
                        foreach (DataRow item in ds_tmp.Tables[0].Select("ILLGAP <= 0"))
                        {
                            ds.Tables[0].ImportRow(item);
                        }
                    }
                    else
                    {
                        ds = ds_tmp;
                    }
                }
                else
                {
                    ds = ds_tmp;
                }
            }

            if (this.gEnmPthlType == clsPthlSQL.enmANATMST_TYPE.A)
            {
                this.Invoke(new delegateSetSpdStyle(setSpdStyle), new object[] { this.ssMain, ds, pthlSQL.sSel_EXAM_ANATMST_TAT_A, pthlSQL.nSel_EXAM_ANATMST_TAT });
            }
            else if (this.gEnmPthlType == clsPthlSQL.enmANATMST_TYPE.T)
            {
                this.Invoke(new delegateSetSpdStyle(setSpdStyle), new object[] { this.ssMain, ds, pthlSQL.sSel_EXAM_ANATMST_TAT_T, pthlSQL.nSel_EXAM_ANATMST_TAT });
            }
            
            this.Invoke(new delegateSetCtrlCircular(setCtrlCircular), false);

            this.btnSearch.BeginInvoke(new System.Action(() => this.btnSearch.Enabled = true));

        }

        delegate void delegateSetCtrlCircular(bool b);
        void setCtrlCircular(bool b)
        {
            if (b == true)
            {
                this.ssMain.Enabled = false;
            }
            else
            {
                this.ssMain.Enabled = true;
            }

            this.circProgress.Visible = b;
            this.circProgress.IsRunning = b;
        }

        delegate void delegateSetSpdStyle(FpSpread spd, DataSet ds, string[] colName, int[] size);
        void setSpdStyle(FpSpread spd, DataSet ds, string[] colName, int[] size)
        {

            spd.ActiveSheet.Rows.Count = 0;

            spread.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            spread.setColStyle(spd, -1, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.RESULT1, clsSpread.enmSpdType.Text);

            spd.DataSource = ds;
            spd.ActiveSheet.ColumnCount = colName.Length;

            //1. 스프레드 사이즈 설정
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                spd.ActiveSheet.RowCount = 0;
            }

            //2. 헤더 사이즈
            spread.setHeader(spd, colName, size);

            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            // 3. 컬럼 스타일 설정.
            spread.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);

            spread.setColStyle(spd, -1, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.ILLGAP, clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.RESULT1, clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.RESULT2, clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.ROWID_R, clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.CHANGE, clsSpread.enmSpdType.Hide);

            if (this.gEnmPthlType == clsPthlSQL.enmANATMST_TYPE.A)
            {
                spread.setColStyle(spd, -1, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.GBTATBUN3, clsSpread.enmSpdType.Hide);
            }

            spread.setColStyle(spd, -1, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.GBTATSAU, clsSpread.enmSpdType.Text);
            spread.setColStyle(spd, -1, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.GBTATBUN, clsSpread.enmSpdType.ComboBox, this.gArrCombo);
            spread.setColStyle(spd, -1, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.GBTATBUN2, clsSpread.enmSpdType.ComboBox, this.gArrCombo);
            spread.setColStyle(spd, -1, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.GBTATBUN3, clsSpread.enmSpdType.ComboBox, this.gArrCombo);

            // 4. 정렬
            spread.setColAlign(spd, 0, CellHorizontalAlignment.Left, CellVerticalAlignment.Center);

            spd.ActiveSheet.Columns.Get((int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.ANATNO).BackColor = method.cPaleGreen;
            spd.ActiveSheet.Columns.Get((int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.RDATE).BackColor = method.cPaleGreen;
            spd.ActiveSheet.Columns.Get((int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.RTIME).BackColor = method.cPaleGreen;
            spd.ActiveSheet.Columns.Get((int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.ILLDATE).BackColor = method.cPaleGreen;

            if (ComFunc.isDataSetNull(ds) == false)
            {
                spd.ActiveSheet.RowCount = spd.ActiveSheet.RowCount + 1;
                spd.ActiveSheet.Cells[spd.ActiveSheet.RowCount - 1, ((int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.RDATE)].Text = "일수평균";

                double nDay = 0;
                double nSum = 0;
                double AbrDay = 0;
                string strIllDay;

                for (int i = 0; i < spd.ActiveSheet.RowCount - 1; i++)
                {
                    strIllDay = spd.ActiveSheet.Cells[i, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.ILLDATE].Text;
                    nDay += Convert.ToInt16(strIllDay);
                }

                nSum = spd.ActiveSheet.RowCount - 1;
                AbrDay = nDay / nSum;

                spd.ActiveSheet.Cells[spd.ActiveSheet.RowCount - 1, (int)clsPthlSQL.enmSel_EXAM_ANATMST_TAT.ILLDATE].Text = AbrDay.ToString("##0.0");
            }
        }

    }
}
