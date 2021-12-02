using ComBase;
using ComDbB;
using ComSupLibB.Com;
using ComSupLibB.SupXray;
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
 
namespace ComSupLibB.SupEnds
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupEnds
    /// File Name       : frmComSupEndsCLE01.cs
    /// Description     : 내시경 오더 취소작업
    /// Author          : 윤조연
    /// Create Date     : 2017-12-13
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\xray\xuagfa\Frm내시경취소작업.frm(Frm내시경취소작업) >> frmComSupEndsCLE01.cs 폼이름 재정의" />
    public partial class frmComSupEndsCLE01 : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsPublic cpublic = new clsPublic();
        clsQuery Query = new clsQuery();
        clsSpread methodSpd = new clsSpread();
        ComFunc fun = new ComFunc();
        clsComSup sup = new clsComSup();
        clsComSupEndsSQL cendsSql = new clsComSupEndsSQL();
        clsComSupEndsSpd cendsSpd = new clsComSupEndsSpd();
        clsComSupXraySQL cxraySql = new clsComSupXraySQL();
        clsComSupXray cxray = new clsComSupXray();

        clsComSupEndsSQL.cEndoJupmst cEndoJupmst = null;


        #endregion

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
        public frmComSupEndsCLE01()
        {
            InitializeComponent();
            setEvent();
        }

        public frmComSupEndsCLE01(MainFormMessage pform)
        {
            InitializeComponent();

            this.mCallForm = pform;

            setEvent();
        }

        //기본값 세팅
        void setCtrlData()
        {

        }

        void setCtrlInit()
        {

        }

        /// <summary>
        /// 이벤트세팅
        /// </summary>
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Resize += new EventHandler(eFormResize);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);

            this.btnSearch.Click += new EventHandler(eBtnSearch);

            this.btnDelete.Click += new EventHandler(eBtnSave);
            

            //명단 더블클릭 이벤트
            //this.ssList.CellClick += new CellClickEventHandler(eSpreadClick);
            //this.ssList.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);
            //this.ssList.TextTipFetch += new TextTipFetchEventHandler(eTxtTipFetch);
            //this.ssList.SelectionChanged += new SelectionChangedEventHandler(eSpreadSelChanged);


            //this.dtpDate.ValueChanged += new EventHandler(eDtpValueChanged);

            this.txtPano.KeyDown += new KeyEventHandler(eTxtKeyDown);


        }

        void setTxtTip()
        {
            //툴팁
            ssList.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Fixed;
            ssList.TextTipDelay = 500;
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                //            
                cendsSpd.sSpd_enmComSupEndsCLE01A(ssList, cendsSpd.sSpdenmComSupEndsCLE01A, cendsSpd.nSpdenmComSupEndsCLE01A, 10, 0);

                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등            

                //툴팁
                setTxtTip();


                //setForm("0");

                screen_clear("ALL");

                setCtrlData();

                //설정정보 체크
                setCtrlInit();

                //screen_display();

            }

        }

        void eFormResize(object sender, EventArgs e)
        {
            setCtrlProgress();
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

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }
        }

        void eBtnSearch(object sender, EventArgs e)
        {
            if (sender == this.btnSearch)
            {
                screen_display();
            }
        }

        void eBtnSave(object sender, EventArgs e)
        {
            if (sender == this.btnDelete)
            {
                eSave(clsDB.DbCon);
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
            FpSpread s = (FpSpread)sender;

            if (e.Row < 0 || e.ColumnHeader == true) return;
            if (e.Button != MouseButtons.Left) return;

            //
            //read_cinfo(s, e.Row);
            //txtMemo.Text = cinfo.strMemo;


            //TODO 윤조연 환자공통 정보
            //if (cinfo.strPano != "") conPatInfo1.SetDisPlay(cinfo.strEmrNo, cinfo.strIO, cinfo.strBDate, cinfo.strPano, cinfo.strDept);

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

            }

        }

        void eSpreadSelChanged(object sender, SelectionChangedEventArgs e)
        {
            FpSpread o = (FpSpread)sender;


        }

        void eTxtTipFetch(object sender, FarPoint.Win.Spread.TextTipFetchEventArgs e)
        {
            FpSpread s = (FpSpread)sender;

            if (s.ActiveSheet.RowCount <= 0)
            {
                return;
            }

            if (e.RowHeader == true || e.Column < 1)
            {
                return;
            }

            e.TipText = ssList.ActiveSheet.Cells[e.Row, e.Column].Text.Trim();
            e.ShowTip = true;
        }

        void eCboSelChanged(object sender, EventArgs e)
        {
            ComboBox o = (ComboBox)sender;

            //조회
            try
            {
                if (o.SelectedItem.ToString() != null)
                {
                    //screen_display();
                    ssList.ActiveSheet.RowCount = 0;
                }

            }
            catch
            {

            }

        }

        void eOptClick(object sender, EventArgs e)
        {
            //조회
            screen_display();
        }

        void eDtpValueChanged(object sender, EventArgs e)
        {
            //조회
            //screen_display();
        }

        void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == this.txtPano)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string strPano = txtPano.Text.Trim();
                    if (strPano != "") read_pano_info(clsDB.DbCon, ComFunc.SetAutoZero(strPano, ComNum.LENPTNO));

                    btnSearch.Select();
                }
            }
        }

        void eSave(PsmhDb pDbCon, int argRow = -1)
        {
            string s = "입원환자일경우 내시경 오더를 DC 하십시오" + ComNum.VBLF ;
            s += "선택된 내시경 오더 취소를 하면 ," + ComNum.VBLF ;
            s += "해당오더가 미접수 상태로 변경이 됩니다." + ComNum.VBLF;
            s += "정말로 내시경을 취소를 하시겠습니까?" ;
            if (ComFunc.MsgBoxQ(s, "작업확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            int i = 0;
            int nstartRow = 0;
            int nLastRow = -1;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strUpCols = "";
            string strWheres = "";

            //string strROWID = "";


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
                    if (ssList_Sheet1.Cells[i, (int)clsComSupEndsSpd.enmComSupEndsCLE01A.chk].Text.Trim() == "True" || argRow != -1)
                    {
                        #region //한건 자료 갱신 작업
                        string strROWID = ssList.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmComSupEndsCLE01A.ROWID].Text.Trim() ;
                        long nSeqno = 0;
                        if (ssList.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmComSupEndsCLE01A.ROWID].Text.Trim() !="")
                        {
                            nSeqno =Convert.ToInt32( ssList.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmComSupEndsCLE01A.Seqno].Text.Trim());
                        }

                        //
                        strUpCols = " GbSunap = '*',PacsSend='*' ";
                        strWheres = "";
                        SqlErr = cendsSql.up_ENDO_JUPMST(pDbCon, strROWID, "", strUpCols, strWheres, ref intRowAffected);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                            return;
                        }
                        if (SqlErr =="")
                        {
                            //
                            strUpCols = " GbBoth = '0' ";
                            strWheres = " AND GbBoth = '1' ";
                            SqlErr = cendsSql.up_ENDO_JUSAMST(pDbCon, "", nSeqno, strUpCols, strWheres, ref intRowAffected);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(pDbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                            
                                return;
                            }
                        }
                        #endregion
                    }
                }

                if (SqlErr == "")
                {
                    clsDB.setCommitTran(pDbCon);
                    ComFunc.MsgBox("작업완료");
                    screen_display();
                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
            }


        }

        void ePrint()
        {
            //clsSpread SPR = new clsSpread();
            //clsSpread.SpdPrint_Margin setMargin;
            //clsSpread.SpdPrint_Option setOption;

            //string strTitle = "";
            //string strHeader = "";
            //string strFooter = "";
            //bool PrePrint = true;


            //#region //시트 히든

            ////ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;


            //#endregion

            //strTitle = "내시경 결과입력 LIST " + "(" + dtpDate.Text.Trim() + ")";

            //strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

            //strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            //strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            //setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            //setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            //SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

            //#region //시트 히든 복원

            ////ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;


            //#endregion

        }

        void setCombo()
        {

        }

        void setCtrlProgress()
        {
            //Point p = new Point();

            //p.X = (this.panheader2.Size.Width / 2) - 90;
            //if (p.X < 0)
            //{
            //    p.X = 0;
            //}
            //p.Y = this.Progress.Location.Y;

            //this.Progress.Location = p;

        }

        void read_pano_info(PsmhDb pDbCon, string argPano)
        {

            txtPano.Text = argPano;
            txtSName.Text = fun.Read_Patient(pDbCon, argPano, "2");

        }

        void screen_clear(string Job = "")
        {
            read_sysdate();

            btnDelete.Enabled = false;
            
            if (Job == "ALL")
            {
                txtPano.Text = "";
            }

            //setSize(true);



        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void screen_display()
        {
            //if (txtPano.Text.Trim() == "")
            //{
            //    ComFunc.MsgBox("등록번호를 넣고 조회하십시오!!");
            //    txtPano.Focus();
            //    return;
            //}

            GetData(clsDB.DbCon, ssList, txtPano.Text.Trim(),dtpFDate.Text.Trim(),dtpTDate.Text.Trim());

            screen_clear();

            btnDelete.Enabled = true;
         
        }

        void GetData(PsmhDb pDbCon, FarPoint.Win.Spread.FpSpread Spd, string argPano, string argDate1,string argDate2)
        {

            int i = 0;
            string xName = string.Empty;
            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            #region //클래스 생성 변수 설정
            cEndoJupmst = new clsComSupEndsSQL.cEndoJupmst();
            cEndoJupmst.STS = "10";
            cEndoJupmst.Date1 = argDate1;
            cEndoJupmst.Date2 = argDate2;
            cEndoJupmst.Ptno = argPano;
            cEndoJupmst.Job = "2"; //위내시경만
            #endregion
            dt = cendsSql.sel_ENDO_JUPMST(pDbCon, cEndoJupmst,true);

            #region //데이터셋 읽어 자료 표시
            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);


                for (i = 0; i < dt.Rows.Count; i++)
                {

                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmComSupEndsCLE01A.chk].Text = "";
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmComSupEndsCLE01A.Pano].Text = dt.Rows[i]["Ptno"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmComSupEndsCLE01A.SName].Text = dt.Rows[i]["SName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmComSupEndsCLE01A.JepDate].Text = dt.Rows[i]["JDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmComSupEndsCLE01A.RDate].Text = dt.Rows[i]["RDate"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmComSupEndsCLE01A.WardCode].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmComSupEndsCLE01A.RoomCode].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmComSupEndsCLE01A.DeptCode].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmComSupEndsCLE01A.DrCode].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmComSupEndsCLE01A.PacsNo].Text = dt.Rows[i]["PACSNO"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmComSupEndsCLE01A.PacsUid].Text = dt.Rows[i]["PACSUID"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmComSupEndsCLE01A.Seqno].Text = dt.Rows[i]["Seqno"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmComSupEndsCLE01A.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                }
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            #endregion


        }

    }
}
