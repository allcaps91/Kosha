using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ComBase;
using ComDbB;
using FarPoint.Win;
using FarPoint.Win.Chart;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread.Chart;
using FarPoint.Win.Spread.Model;

namespace ComEmrBase
{
    public partial class frmAnForm : Form, EmrChartForm
    {        
        /// <summary>
        /// 항목
        /// </summary>
        private List<MedcationInfo> Medcations = null;
        /// <summary>
        /// 그래프 차트
        /// </summary>
        private SpreadChart SpChart = null;
        /// <summary>
        /// SBP
        /// </summary>
        private PointSeries SBPSeries;
        /// <summary>
        /// DBP
        /// </summary>
        private PointSeries DBPSeries;
        /// <summary>
        /// 맥박
        /// </summary>
        private PointSeries PulseSeries;
        /// <summary>
        /// 호흡
        /// </summary>
        private PointSeries BreathSeries;
        /// <summary>
        /// 체온
        /// </summary>
        //private PointSeries TempSeries;
        /// <summary>
        /// 차트 Y축
        /// </summary>
        private YPlotArea YPlotArea;

        /// <summary>
        /// 그래프 위치
        /// </summary>
        private PointF YPlotLocation;
        /// <summary>
        /// 그래프 사이즈
        /// </summary>
        private SizeF YPlotSize;
        /// <summary>
        /// 그래프 그래픽 사이즈
        /// </summary>
        private Rectangle ChartRect;

        private readonly string ValueEquals = "───";

        //private string EMRNO = string.Empty;
        private int MedcationRowIndex = -1;
        private int JepRowIndex = -1;

        /// <summary>
        /// 자동완성 선택 스프레드
        /// </summary>
        private FpSpread CurrentFpSpread;
        private int CurrentSpreadRowIndex = -1;

        //EmrPatient AcpEmr = null;
        private int ViewItemCount = 0;

        List<MedcationInfo> BasicMedcationInfo = null;

        /// <summary>
        /// 수술 스케줄 불러오기
        /// </summary>
        frmAnFormMapping frmAnFormMappingX;
        /// <summary>
        /// 검사결과 불러오기
        /// </summary>
        frmAnFormExam frmAnFormExamX;
        /// <summary>
        /// Tourniquest 입력폼
        /// </summary>
        frmAnFormGrapeTurniquest frmAnFormGrapeTurniquestX;
        /// <summary>
        /// 스프레드 상용구
        /// </summary>
        frmAnFormSpdMacro frmAnFormSpdMacroX;

        #region // 상용구 관련 변수 선언
        Control mControl = null;    //일반 텍스트
        frmEmrMacrowordProg frmMacrowordProgEvent;

        //Control mCalControl = null; //달력 띄우기
        //frmEmrCaledar frmEmrCaledarEvent;

        FarPoint.Win.Spread.FpSpread ssMacroWord;
        FarPoint.Win.Spread.SheetView ssMacroWord_Sheet1;

        /// <summary>
        /// 상용구 폼.
        /// </summary>
        //frmEmrBaseSympOld fEmrMacro = null;

        /// <summary>
        /// 상용구 클릭시 커서 이동 때문에 필요함.
        /// </summary>
        int lastScrollValue = 0;

        #endregion // 상용구 관련 모듈

        #region //EmrChartForm
        public void SetChartHisMsg(string strEmrNo, string strOldGb)
        {
            return;
        }
        public double SaveDataMsg(string strFlag)
        {
            double dblEmrNo = 0;
            //dblEmrNo = pSaveData(strFlag);
            return dblEmrNo;
        }

        public bool DelDataMsg()
        {
            bool rtnVal = false;

            //return pDelData();

            return rtnVal;
        }

        public void ClearFormMsg()
        {
            //mstrEmrNo = "0";
            //pClearForm();
        }
        public void SetUserFormMsg(double dblMACRONO)
        {
            //TODO
            //pSetUserForm(dblMACRONO);
        }

        public bool SaveUserFormMsg(double dblMACRONO)
        {
            bool rtnVal = false;

            return rtnVal;
        }

        public int PrintFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;

            if (strPRINTFLAG == "N")
            {
                frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption();
                frmEmrPrintOptionX.ShowDialog();
            }

            if (clsFormPrint.mstrPRINTFLAG == "-1")
            {
                return rtnVal;
            }

            pPrintForm();
            //if (clsEmrQuery.SaveEmrXmlPrnYnForm(clsDB.DbCon, mstrEmrNo, "0") == false)
            //{
            //    return rtnVal;
            //}

            //rtnVal = clsFormPrint.PrintFormLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");

            return rtnVal;
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;

            //int rtnVal = clsFormPrint.PrintToTifFileLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            //return rtnVal;

            return rtnVal;
        }
        #endregion //EmrChartForm

        #region // 상용구 이벤트
        private void pAddEventToText(Control objParent)
        {
            Control[] controls = FormFunc.GetAllControls(this);

            foreach (Control control in controls)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).Click += new System.EventHandler(pTextBox_Click);
                    ((TextBox)control).DoubleClick += new System.EventHandler(pTextBox_DoubleClick);
                }
            }
        }

        private void pTextBox_Click(object sender, EventArgs e)
        {
            if (ssMacroWord.Visible)
            {
                ssMacroWord.Visible = false;
                return;
            }

            if (clsType.User.AuAMANAGE.Equals("1"))
                return;

            lastScrollValue = panel36.AutoScrollPosition.Y < 0 ? panel36.AutoScrollPosition.Y * -1 : panel36.AutoScrollPosition.Y;

            if (((TextBox)sender).Focused == false)
            {
                return;
            }

            mControl = (TextBox)sender;

            clsEmrFunc.DspControl(clsDB.DbCon, this, panel36, mstrFormNo, ssMacroWord, mControl);
            lastScrollValue = panel36.AutoScrollPosition.Y < 0 ? panel36.AutoScrollPosition.Y * -1 : panel36.AutoScrollPosition.Y;
        }

        private void pTextBox_DoubleClick(object sender, EventArgs e)
        {
            ssMacroWord.Visible = false;
            
            if (clsType.User.AuAMANAGE.Equals("1"))
                return;

            mControl = (TextBox)sender;

            pLoadMacro(mControl);
            return;
        }

        private void pAddMacroSpd()
        {
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color333635194368298125000", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text397635194368298125000", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();

            ssMacroWord = new FarPoint.Win.Spread.FpSpread();
            ssMacroWord_Sheet1 = new FarPoint.Win.Spread.SheetView();

            // ssMacroWord
            this.ssMacroWord.AccessibleDescription = string.Empty;
            this.ssMacroWord.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssMacroWord.Location = new System.Drawing.Point(3, 6);
            this.ssMacroWord.Name = "ssMacroWord";
            namedStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            textCellType1.MaxLength = 32000;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            this.ssMacroWord.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2});
            this.ssMacroWord.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssMacroWord_Sheet1});
            this.ssMacroWord.Size = new System.Drawing.Size(62, 20);
            this.ssMacroWord.TabIndex = 105;
            this.ssMacroWord.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssMacroWord.TextTipAppearance = tipAppearance1;
            this.ssMacroWord.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssMacroWord.Visible = false;
            this.ssMacroWord.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssMacroWord_CellClick);
            this.ssMacroWord.Leave += new System.EventHandler(this.ssMacroWord_Leave);
            // 
            // ssMacroWord_Sheet1
            // 
            this.ssMacroWord_Sheet1.Reset();
            this.ssMacroWord_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssMacroWord_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            ssMacroWord_Sheet1.ColumnCount = 2;
            ssMacroWord_Sheet1.RowCount = 10;
            this.ssMacroWord_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMacroWord_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMacroWord_Sheet1.ColumnHeader.Visible = false;
            this.ssMacroWord_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMacroWord_Sheet1.DefaultStyleName = "Text397635194368298125000";
            this.ssMacroWord_Sheet1.GrayAreaBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssMacroWord_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssMacroWord_Sheet1.RowHeader.Visible = false;
            this.ssMacroWord_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;

            Controls.Add(this.ssMacroWord);
        }

        private void pLoadMacro(Control mControl)
        {
            string strConIndex = string.Empty;
            strConIndex = clsXML.IsArryCon(mControl);

            if (frmMacrowordProgEvent == null)
            {
                string strUserGb = string.Empty;
                switch (clsEmrPublic.gstrMcrAllFlag)
                {
                    //전체
                    case "1":
                        strUserGb = "A";
                        break;
                    //과별
                    case "2":
                        strUserGb = "D";
                        break;
                    //유저
                    case "3":
                        strUserGb = "U";
                        break;
                }

                frmMacrowordProgEvent = new frmEmrMacrowordProg(mControl.Name, strConIndex, mstrFormNo, "200", mstrFormText, strUserGb);
                frmMacrowordProgEvent.rSetMacro += new frmEmrMacrowordProg.SetMacro(frmMacrowordProgEvent_SetMacro);
                frmMacrowordProgEvent.rEventClosed += new frmEmrMacrowordProg.EventClosed(frmMacrowordProgEvent_EventClosed);
            }
            frmMacrowordProgEvent.ShowDialog();
        }

        private void frmMacrowordProgEvent_SetMacro(string strCtlName, string strMacrono, string strMacro, string strCtlNameIdx)
        {
            string strConIndex = string.Empty;
            strConIndex = clsXML.IsArryCon(mControl);

            frmMacrowordProgEvent.Close();
            frmMacrowordProgEvent = null;

            if (clsEmrPublic.gstrMcrAddFlag == "1")
            {
                clsEmrFunc.MacroSpace(clsDB.DbCon, mControl, strMacro);
            }
            else
            {
                mControl.Text = strMacro;
            }

            if (mstrFormNo == "963")
                return;


            //if (panChart.VerticalScroll.Value == lastScrollValue)
            //    return;
            //
            //panChart.VerticalScroll.Value = lastScrollValue;
        }

        private void frmMacrowordProgEvent_EventClosed()
        {
            frmMacrowordProgEvent.Close();
            frmMacrowordProgEvent = null;
        }


        private void ssMacroWord_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssMacroWord.Visible = false;
            string strMacro = ssMacroWord_Sheet1.Cells[e.Row, 1].Text;
            strMacro = VB.Replace(VB.Replace(strMacro, "<pre>", ""), "</pre>", "");

            //clsApi.SuspendDrawing(panChart);
            if (clsEmrPublic.gstrMcrAddFlag == "1")
            {
                clsEmrFunc.MacroSpace(clsDB.DbCon, mControl, strMacro);
            }
            else
            {
                mControl.Text = strMacro;
            }

            //panChart.AutoScroll = false;

            //panChart.BringToFront();
            //if (panChart.VerticalScroll.Value != lastScrollValue)
            //{
            //    panChart.VerticalScroll.Value = lastScrollValue;
            //    //mControl.Focus();
            //}

            //panChart.AutoScroll = true;


            //panChart.Refresh();

            //clsApi.ResumeDrawing(panChart);
        }

        private void ssMacroWord_Leave(object sender, EventArgs e)
        {
            ssMacroWord.Visible = false;
        }
        #endregion


        public frmAnForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strPtNo"></param>
        /// <param name="strInoutCls"></param>
        /// <param name="strMedFrDate"></param>
        /// <param name="strDeptCd"></param>
        public frmAnForm(string strPtNo, string strInoutCls, string strMedFrDate, string strDeptCd)
        {
            InitializeComponent();
            
            strInoutCls = "I";
            try
            {
                //? asdfsadfasasdfas
                //!? asdflkjasdflkjasldfjl
                //x asdfasdfasdfasdfasdfasdasdf
                //! asdfasdfasdfasfasfasfasf
                //TODO : task
                EmrPatient AcpEmr = clsEmrChart.ClearPatient();
                AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, strPtNo, strInoutCls, strMedFrDate, strDeptCd);
                if (AcpEmr == null)
                {
                    ComFunc.MsgBoxEx(this, "접수내역을 찾을 수 없습니다.");
                    return;
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, "SetEmrPatInfoIpd 접수정보 전달", clsDB.DbCon); //에러로그 저장
            }
        }

        public frmAnForm(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            AcpEmr = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
        }

        public frmAnForm(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            AcpEmr = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
        }

        /// <summary>
        /// 폼로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAnForm_Load(object sender, EventArgs e)
        {
            Type1 = new ComboBoxCellType();
            Type2 = new CheckBoxCellType();

            SetInit();
            pAddMacroSpd();
            pAddEventToText(this);

            // 항목자동세팅
            //btnItem.Visible = false;
            SetBasicMedcationInfo();
            InitAnItem();

            SetFormulaIntake();
            SetFormulaOutput();

            SetEnterKey(panTop);
        }

        /// <summary>
        /// 약속 처방 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CboYak_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cboYak.SelectedIndex < 0)
            {
                return;
            }

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string wrtNo = string.Empty;
            Cursor.Current = Cursors.WaitCursor;

            clsSpread CS = new clsSpread();

            CS.Spread_Clear_Range(ssMedication, 0, 0, ssMedication.ActiveSheet.RowCount, ssMedication.ActiveSheet.ColumnCount);
            CS.Spread_Clear_Range(ssJep, 0, 0, ssJep.ActiveSheet.RowCount, ssJep.ActiveSheet.ColumnCount);

            ssMedication.ActiveSheet.Cells[0, 0, ssMedication.ActiveSheet.RowCount - 1, ssMedication.ActiveSheet.ColumnCount - 1].Locked = false;
            ssJep.ActiveSheet.Cells[0, 0, ssJep.ActiveSheet.RowCount - 1, ssJep.ActiveSheet.ColumnCount - 1].Locked = false;

            MedcationRowIndex = -1;
            JepRowIndex = -1;

            try
            {
                string[] split = cboYak.Items[cboYak.SelectedIndex].ToString().Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                wrtNo = split[0];

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT A.JEPCODE, A.CODEGBN";
                SQL = SQL + ComNum.VBLF + "     , C.UNIT, C.SUNAMEE, C.SUNAMEK";
                SQL = SQL + ComNum.VBLF + "     , D.JEPNAME, D.BUSE_UNIT, D.GBEXCHANGE ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.OPR_GROUPDTL A";
                SQL = SQL + ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_PMPA.BAS_SUT B";
                SQL = SQL + ComNum.VBLF + "               ON A.JEPCODE = B.SUNEXT";
                SQL = SQL + ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_PMPA.BAS_SUN C";
                SQL = SQL + ComNum.VBLF + "               ON B.SUNEXT = C.SUNEXT";
                SQL = SQL + ComNum.VBLF + "  LEFT OUTER JOIN KOSMOS_ADM.ORD_JEP D";
                SQL = SQL + ComNum.VBLF + "               ON A.JEPCODE = D.JEPCODE";
                SQL = SQL + ComNum.VBLF + " WHERE A.WRTNO = '" + wrtNo + "'";
                SQL = SQL + ComNum.VBLF + "ORDER BY A.SEQNO ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                foreach(DataRow row in dt.Rows)
                {
                    //? 수가정보
                    if("1".Equals(row["CODEGBN"].ToString()))
                    {
                        MedcationRowIndex++;
                        ssMedication.ActiveSheet.Cells[MedcationRowIndex, 0].Text = row["SUNAMEK"].ToString();
                        ssMedication.ActiveSheet.Cells[MedcationRowIndex, 2].Text = row["UNIT"].ToString();
                        ssMedication.ActiveSheet.Cells[MedcationRowIndex, 3].Text = row["JEPCODE"].ToString();

                        //ssMedication.ActiveSheet.Cells[MedcationRowIndex, 0].Locked = true;
                    }
                    //? 물품정보
                    if("2".Equals(row["CODEGBN"].ToString()))
                    {
                        JepRowIndex++;
                        ssJep.ActiveSheet.Cells[JepRowIndex, 0].Text = row["JEPNAME"].ToString();
                        ssJep.ActiveSheet.Cells[JepRowIndex, 2].Text = row["BUSE_UNIT"].ToString();

                        //ssJep.ActiveSheet.Cells[JepRowIndex, 0].Locked = true;
                    }
                }

                Cursor.Current = Cursors.Default;
            }
            catch(Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        /// <summary>
        /// 수술시작
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkOpStart_Click(object sender, EventArgs e)
        {
            if (chkOpStart.Checked == false)
            {
                if (mstrEmrNo != "0")
                {
                    ComFunc.MsgBoxEx(this, "저장된 차트는 초기화 할 수 없습니다.");
                    chkOpStart.Checked = true;
                    return;
                }

                if (ComFunc.MsgBoxQ("VITAL GRAPH를 초기화 하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    chkOpStart.Checked = true;
                    return;
                }
            }
            
            if (chkOpStart.Checked)
            {
                if(Medcations == null || Medcations.Count == 0)
                {
                    ComFunc.MsgBoxEx(this, "설정된 항목이 없습니다.");
                    chkOpStart.Checked = false;
                    return;
                }

                Timer timer = new Timer();
                //  최초 3분으로 설정 후 
                //  처음 이벤트를 타면 5분으로 변경한다.
                timer.Interval = 60 * 3 * 1000;
                timer.Tick += (s, evt) =>
                {
                    timer.Interval = 60 * 5 * 1000;
                    int comboIndex = cboTime.SelectedIndex;
                    if(comboIndex + 1 < cboTime.Items.Count)
                    {
                        cboTime.SelectedIndex = comboIndex + 1;
                    }
                };
                //timer.Start();

                ssView.ActiveSheet.Columns.Count = 2;
                ssView.ActiveSheet.Rows.Count = 2;

                //  스프레드 컬럼/로우 설정
                //ssView.ActiveSheet.Columns.Count = 36;
                ssView.ActiveSheet.Columns.Count = 29;
                ssView.ActiveSheet.Rows.Count = 200;
                ssView.ActiveSheet.SetRowHeight(-1, 18);
                
                ssView.ActiveSheet.Rows[0].MergePolicy = MergePolicy.Always;

                //  스프레드 상단 시간설정 및 콤보박스 아이템 설정
                CreateDate = dtpNow.Value;
                SetSpreadTime(2);
                //  콤보박스 첫번쨰로 설정
                cboTime.SelectedIndex = 0;

                ssView.ActiveSheet.Rows.Get(0).Border = new ComplexBorder(new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.MediumLine), new ComplexBorderSide(ComplexBorderSideStyle.None), false, false);
                ssView.ActiveSheet.Rows.Get(1).Border = new ComplexBorder(new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.MediumLine), new ComplexBorderSide(ComplexBorderSideStyle.None), false, false);

                //  컬럼고정
                ssView.ActiveSheet.FrozenColumnCount = 2;

                //  View Spread 항목설정
                List<MedcationInfo> list = Medcations.ToList().FindAll(r => r.IsView);
                ViewItemCount = list.Count;

                for (int i = 0; i < list.Count; i++)
                {
                    ssView.ActiveSheet.Cells[i + 2, 0].Text = list[i].Name;                    
                    ssView.ActiveSheet.Cells[i + 2, 1].Text = list[i].Uint;

                    ssView.ActiveSheet.Cells[i + 2, 0].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssView.ActiveSheet.Cells[i + 2, 1].HorizontalAlignment = CellHorizontalAlignment.Center;

                    ssView.ActiveSheet.Cells[i + 2, 0].Locked = true;
                    ssView.ActiveSheet.Cells[i + 2, 1].Locked = true;                    
                }

                #region 그래프 설정

                //  그래프 설정
                XYPointSeries xyPointSeries = new XYPointSeries();
                
                SpChart = new SpreadChart(xyPointSeries, "SpreadChart1");

                ssView.ActiveSheet.DrawingContainer.ContainedObjects.Add(SpChart);

                SpChart.IgnoreUpdateShapeLocation = false;
                SpChart.IsGrayscale = false;

                //LegendArea legendArea1 = new LegendArea();
                //SpChart.Model.LegendAreas.AddRange(new LegendArea[] { legendArea1 });

                YPlotArea = new YPlotArea
                {                    
                    Location = new PointF(0.083F, 0.05F),
                    Size = new SizeF(0.85F, 0.8F)
                };

                SpChart.Model.PlotAreas.AddRange(new PlotArea[] { YPlotArea });

                string lastAlphabet = GetExcelColumnName(ssView.ActiveSheet.Columns.Count);

                SBPSeries = new PointSeries
                {
                    SeriesName = "SBP",
                    LabelVisible = true,
                    PointBorder = new NoLine(),
                    PointFill = Properties.Resources.Chart_SBP
                };
                SBPSeries.Values.DataSource = new SeriesDataField(
                    ssView, 
                    "SBP", 
                    string.Concat("Sheet1!$C$101:$", lastAlphabet, "$101"), 
                    SegmentDataType.AutoIndex, 
                    new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps }
                );

                DBPSeries = new PointSeries
                {
                    SeriesName = "DBP",
                    LabelVisible = true,
                    PointBorder = new NoLine(),
                    PointFill = Properties.Resources.Chart_DBP
                };
                DBPSeries.Values.DataSource = new SeriesDataField(
                    ssView, 
                    "DBP",
                    string.Concat("Sheet1!$C$102:$", lastAlphabet, "$102"),
                    SegmentDataType.AutoIndex, 
                    new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps }
                );

                PulseSeries = new PointSeries
                {
                    SeriesName = "맥박",
                    LabelVisible = true,
                    PointBorder = new NoLine(),
                    PointFill = Properties.Resources.Chart_Pulse
                };
                PulseSeries.Values.DataSource = new SeriesDataField(
                    ssView, 
                    "맥박",
                    string.Concat("Sheet1!$C$103:$", lastAlphabet, "$103"),
                    SegmentDataType.AutoIndex, 
                    new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps }
                );

                BreathSeries = new PointSeries
                {
                    SeriesName = "호흡",
                    LabelVisible = true,
                    PointBorder = new NoLine(),
                    PointFill = Properties.Resources.Chart_Breath
                };
                BreathSeries.Values.DataSource = new SeriesDataField(
                    ssView, 
                    "호흡",
                    string.Concat("Sheet1!$C$104:$", lastAlphabet, "$104"),
                    SegmentDataType.AutoIndex, 
                    new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps }
                );

                YPlotArea.Series.AddRange(new Series[] {
                    SBPSeries,
                    DBPSeries,
                    PulseSeries,
                    BreathSeries,
                });

                IndexAxis indexAxis1 = new IndexAxis { LabelTextDirection = TextDirection.Rotate90Degree };
                ValueAxis valueAxis1 = new ValueAxis();
                valueAxis1.Minimum = 0;
                valueAxis1.Maximum = 200;

                YPlotArea.XAxis = indexAxis1;
                YPlotArea.YAxes.Clear();
                YPlotArea.YAxes.AddRange(new ValueAxis[] { valueAxis1 });
                
                int y = (ViewItemCount * 18) + 50;
                SpChart.Rectangle = new Rectangle(50, y, 800, 600);
                SpChart.SheetName = "fpSpread1_Sheet1";

                

                YPlotLocation = YPlotArea.Location;
                YPlotSize = YPlotArea.Size;
                ChartRect = SpChart.Rectangle;

                #endregion

                //  그래프 마커 설정
                SetChartPointMarkers(0);

                //  입력 스프레드 포커스 설정
                ssWrite.ActiveSheet.ActiveColumnIndex = 1;

                //chkOpStart.Enabled = false;
            }
            else
            {

            }
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {            
            if (cboTime.SelectedIndex < 0)
            {
                return;
            }

            string[] split = cboTime.Text.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            string hh = split[0];
            string mm = split[1];

            int col = -1;
            for(int i=2; i<ssView.ActiveSheet.ColumnCount; i++)
            {
                if(ssView.ActiveSheet.Cells[0, i].Text.Equals(hh))
                {
                    if(ssView.ActiveSheet.Cells[1, i].Text.Equals(mm))
                    {
                        col = i;
                        break;
                    }
                }
            }


            if (dtpOpDateEnd.Value.ToString("yyyy-MM-dd") != ssView.ActiveSheet.Cells[0, col].Tag.ToString())
            {
                dtpOpDateEnd.Value = Convert.ToDateTime(ssView.ActiveSheet.Cells[0, col].Tag.ToString());
            }

            //  좌측 결과 표시 부분에 RowIndex 설정
            //  SBP, DBP, 맥박, 호흡, Temp 는 항목에서 삭제한다.
            //  ssView 스프레드의 실제 RowIndex 구하기 위한 작업
            int itemMinus = 0;
            foreach(MedcationInfo item in Medcations)
            {
                if( item.Name.Equals("SBP") || item.Name.Equals("DBP") ||
                    item.Name.Equals("맥박") || item.Name.Equals("호흡"))                    
                {
                    itemMinus++;
                }
            }

            //  마취 시작/종료
            if (chkAnesthesia.Checked)
            {
                MedcationInfo item = Medcations.ToList().Find(r => r.Name.Equals("마취"));
                ssView.ActiveSheet.Cells[item.Row - itemMinus, col].Text = "X";
                chkAnesthesia.Checked = false;
            }
            else
            {
                MedcationInfo item = Medcations.ToList().Find(r => r.Name.Equals("마취"));
                ssView.ActiveSheet.Cells[item.Row - itemMinus, col].Text = "";
                chkAnesthesia.Checked = false;
            }

            //  수술 시작/종료
            if (chkOp.Checked)
            {
                MedcationInfo item = Medcations.ToList().Find(r => r.Name.Equals("수술"));
                ssView.ActiveSheet.Cells[item.Row - itemMinus, col].Text = "⊙";
                chkOp.Checked = false;
            }
            else
            {
                MedcationInfo item = Medcations.ToList().Find(r => r.Name.Equals("수술"));
                ssView.ActiveSheet.Cells[item.Row - itemMinus, col].Text = "";
                chkOp.Checked = false;
            }

            //  삽관 시작/종료
            //  5분단위 아닌 실제 시간을 표시하기로 원함
            if (chkIntubation.Checked)
            {
                MedcationInfo item = Medcations.ToList().Find(r => r.Name.Equals("삽관"));
                ssView.ActiveSheet.Cells[item.Row - itemMinus, col].Text = "T";
                chkIntubation.Checked = false;
            }
            else
            {
                MedcationInfo item = Medcations.ToList().Find(r => r.Name.Equals("삽관"));
                ssView.ActiveSheet.Cells[item.Row - itemMinus, col].Text = "";
                chkIntubation.Checked = false;
            }

            for(int i=0; i<ssWrite.ActiveSheet.Rows.Count; i++)
            {
                if(ssWrite.ActiveSheet.Rows[i].Tag != null)
                {
                    MedcationInfo medcationInfo = ssWrite.ActiveSheet.Rows[i].Tag as MedcationInfo;
                    string value = ssWrite.ActiveSheet.Cells[i, 1].Text;
                    string unit = ssWrite.ActiveSheet.Cells[i, 2].Text;
                    object tag = ssWrite.ActiveSheet.Cells[i, 2].Tag;

                    string itemName = ssWrite.ActiveSheet.Cells[i, 0].Text;
                    int row = medcationInfo.Row - itemMinus;
                    if (itemName.Equals("SBP") || itemName.Equals("DBP") || itemName.Equals("맥박") || itemName.Equals("호흡"))
                    {
                        row = medcationInfo.Row;
                    }

                    if (medcationInfo.IsView)
                    {
                        if (col > 2)
                        {                            
                            string display = ssView.ActiveSheet.Cells[row, col - 1].Text;
                            if(display.Equals(ValueEquals))
                            {
                                for(int j=col - 1; j>=0; j--)
                                {
                                    display = ssView.ActiveSheet.Cells[row, j].Text;

                                    // Temp 15분 간격으로 숫자입력
                                    if (itemName.Equals("Temp") && j == col - 3)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        if (!display.Equals(ValueEquals))
                                        {
                                            if (value.Equals(display))
                                            {
                                                value = ValueEquals;
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                            else if (display.Equals(value) && !string.IsNullOrWhiteSpace(display))
                            {
                                value = ValueEquals;
                            }                            
                        }

                        if (itemName == "EKG monitoring" || itemName == "Warm blanket")
                        {
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                value = ValueEquals;
                            }
                        }
                    }


                    if (ssWrite.ActiveSheet.Cells[i, 3].CellType == Type2)
                    {
                        if (string.IsNullOrEmpty(value) == false && Convert.ToBoolean(ssWrite.ActiveSheet.Cells[i, 3].Value) == true)
                        {
                            if (value.Equals(ValueEquals) == true)
                            {
                                value = "─/";
                            }
                            else
                            {
                                value = value + "/";
                            }
                        }
                    }

                    ssView.ActiveSheet.Cells[row, col].Text = value;

                    // 호흡은 3가지 방식이 있어서 Unit을 선택하여 저장함.
                    if (itemName.Equals("호흡"))
                    {
                        //ssView.ActiveSheet.Cells[row + 1, col].Text = string.IsNullOrEmpty(value) == true ? "" : unit;

                        //2020-05-12 마취과 이송이 선생님 고정값으로 변경요청 
                        ssView.ActiveSheet.Cells[row + 1, col].Text = string.IsNullOrEmpty(value) == true ? "" : "RR";
                    }

                    // 입력내용을 Tag로 저장
                    if (itemName.Equals("Tourniquet"))
                    {
                        ssView.ActiveSheet.Cells[row, col].Tag = tag;

                        // 초기화
                        ssWrite.ActiveSheet.Cells[i, 1].Text = "";
                        ssWrite.ActiveSheet.Cells[i, 2].Tag = null;
                    }

                    if (ssWrite.ActiveSheet.Cells[i, 3].CellType == Type2)
                    {
                        if (Convert.ToBoolean(ssWrite.ActiveSheet.Cells[i, 3].Value) == true)
                        {
                            // 초기화
                            ssWrite.ActiveSheet.Cells[i, 1].Text = "";
                            if (itemName.Equals("Tourniquet"))
                            {
                                ssWrite.ActiveSheet.Cells[i, 2].Tag = null;
                            }
                            ssWrite.ActiveSheet.Cells[i, 3].Value = false;
                        }
                    }
                }
            }

            // LabelArea
            List<LabelArea> labelAreas1 = new List<LabelArea>();
            for (int column = 2; column < ssView.ActiveSheet.ColumnCount; column++)
            {
                if (ssView.ActiveSheet.Cells[103, column].Text == "") continue;

                float value = (float)VB.Val(ssView.ActiveSheet.Cells[103, column].Text);
                LabelArea la = new LabelArea();
                la.Text = VB.Left(ssView.ActiveSheet.Cells[104, column].Text, 1);
                la.TextFont = new Font("굴림", 8F);

                float width = ((float)column / (float)ssView.ActiveSheet.ColumnCount) * 0.94f;
                la.Location = new PointF(0.034f + width, 0.82f);
                labelAreas1.Add(la);
            }
            SpChart.Model.LabelAreas.Clear();
            SpChart.Model.LabelAreas.AddRange(labelAreas1.ToArray());

            // 저장 후 다음타임으로 콤보박스 변경
            if (cboTime.SelectedIndex < cboTime.Items.Count - 1)
            {
                cboTime.SelectedIndex = cboTime.SelectedIndex + 1;
            }

            btnAllChk.Text = "Chk";

            ssWrite.Focus();
            ssWrite.ActiveSheet.SetActiveCell(0, 1);            
            ssWrite.SetViewportTopRow(0, ssWrite.ActiveSheet.Rows.Count);
            ssWrite.SetViewportLeftColumn(0, 0);
        }

    /// <summary>
    /// 스프레드 단축키 저장
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SsWrite_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                btnSave.PerformClick();
            }
        }

        /// <summary>
        /// 항목설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnItem_Click(object sender, EventArgs e)
        {
            frmEmrAnItem frmEmrAnItem = new frmEmrAnItem(Medcations, chkOpStart.Checked);
            frmEmrAnItem.ShowDialog(this);

            //  저장버튼 체크
            if(!frmEmrAnItem.IsSave)
            {
                return;
            }

            Medcations = new List<MedcationInfo>();
            for (int i=0; i < frmEmrAnItem.ssItemList.ActiveSheet.RowCount; i++)
            {
                string itemNo = frmEmrAnItem.ssItemList.ActiveSheet.Cells[i, 3].Text;
                if (string.IsNullOrWhiteSpace(itemNo))
                {
                    continue;
                }

                string itemName = frmEmrAnItem.ssItemList.ActiveSheet.Cells[i, 1].Text;
                string itemUnit = frmEmrAnItem.ssItemList.ActiveSheet.Cells[i, 2].Text;
                int row = Convert.ToInt32(frmEmrAnItem.ssItemList.ActiveSheet.Cells[i, 4].Value.ToString());
                bool isWrite = Convert.ToBoolean(frmEmrAnItem.ssItemList.ActiveSheet.Cells[i, 5].Text);
                bool isView = Convert.ToBoolean(frmEmrAnItem.ssItemList.ActiveSheet.Cells[i, 6].Text);

                Medcations.Add(new MedcationInfo
                {
                    Name = itemName,
                    Uint = itemUnit,
                    Row = row == -1 ? i + 2 : row,
                    IsWrite = isWrite,
                    IsView = isView,
                    ItemNo = itemNo
                });
            }


            //  Write Spread 항목설정            
            ListBox list1 = new ListBox();
            list1.Items.Add("SR");
            list1.Items.Add("CR");
            list1.Items.Add("AR");

            Type1.Clear();
            Type1.ListControl = list1;
            Type1.Editable = false;

            //  Write Spread 항목설정
            List<MedcationInfo> list = Medcations.ToList().FindAll(r => r.IsWrite);
            ssWrite.ActiveSheet.Rows.Count = list.Count;
            for (int i = 0; i < list.Count; i++)
            {
                ssWrite.ActiveSheet.Rows[i].Tag = list[i];
                ssWrite.ActiveSheet.Cells[i, 0].Text = list[i].Name;
                ssWrite.ActiveSheet.Cells[i, 2].Text = list[i].Uint;

                // 호흡                    
                if (list[i].ItemNo == "I0000000574")
                {
                    //ssWrite.ActiveSheet.Cells[i, 2].CellType = Type1;
                    //ssWrite.ActiveSheet.Cells[i, 2].Text = "SR";
                    //ssWrite.ActiveSheet.Cells[i, 2].Locked = false;

                    //2020-05-12 마취과 이송이 선생님 고정값으로 변경요청 
                    ssWrite.ActiveSheet.Cells[i, 2].Text = "RR";                    
                }
                else
                {
                    ssWrite.ActiveSheet.Cells[i, 2].Text = list[i].Uint;
                }

                // Tourniquest
                if (list[i].ItemNo == "I0000030758")
                {
                    ssWrite.ActiveSheet.Cells[i, 1].Locked = true;
                    ssWrite.ActiveSheet.Cells[i, 2].Locked = true;
                }

                if (list[i].Name == "SBP" || list[i].Name == "DBP" || list[i].Name == "맥박" || list[i].Name == "호흡")
                {
                    ssWrite.ActiveSheet.Cells[i, 3].CellType = null;
                }
                else
                {
                    ssWrite.ActiveSheet.Cells[i, 3].CellType = Type2;
                }
            }

            // 수술시작 이후 항목변경시
            if (chkOpStart.Checked == true)
            {
                int i = 0;
                //  View Spread 항목설정
                List<MedcationInfo> vlist = Medcations.ToList().FindAll(r => r.IsView);
                ViewItemCount = vlist.Count;

                for (i = 0; i < vlist.Count; i++)
                {
                    ssView.ActiveSheet.Cells[i + 2, 0].Text = vlist[i].Name;
                    ssView.ActiveSheet.Cells[i + 2, 1].Text = vlist[i].Uint;

                    ssView.ActiveSheet.Cells[i + 2, 0].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssView.ActiveSheet.Cells[i + 2, 1].HorizontalAlignment = CellHorizontalAlignment.Center;

                    ssView.ActiveSheet.Cells[i + 2, 0].Locked = true;
                    ssView.ActiveSheet.Cells[i + 2, 1].Locked = true;
                }

                if (i + 2 <= 18)
                {
                    ssView.ActiveSheet.Cells[i + 2, 0, 18, ssView.ActiveSheet.ColumnCount - 1].Text = "";
                }

                // 그래프 위치변경
                int y = (ViewItemCount * 18) + 50;
                SpChart.Rectangle = new Rectangle(50, y, 800, 600);
                SpChart.SheetName = "fpSpread1_Sheet1";
            }
        }

        /// <summary>
        /// 시간 추가 1시간
        /// dtpTime 시간이 스프레드 표시시간을 넘어갈경우 자동추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            if (chkOpStart.Checked == false && mstrEmrNo == "0") return;

            string hhmm = dtpNow.Value.ToString("HHmm");
            string spHHmm = string.Concat(ssView.ActiveSheet.Cells[0, ssView.ActiveSheet.Columns.Count -1].Text, ssView.ActiveSheet.Cells[1, ssView.ActiveSheet.Columns.Count - 1].Text);

            //if(Convert.ToInt32(hhmm) > Convert.ToInt32(spHHmm))
            {
                //  컬럼 추가할 시작 번호
                int count = ssView.ActiveSheet.Columns.Count;
                //  컴럼 한시간 추가(5분간격)
                ssView.ActiveSheet.Columns.Count += 12;

                //  스프레드 상단 시간설정 및 콤보박스 아이템 설정
                SetSpreadTime(count, spHHmm);

                //  그래프 마크설정
                SetChartPointMarkers(count);

                string start = GetExcelColumnName(3);
                string end = GetExcelColumnName(ssView.ActiveSheet.ColumnCount);

                SBPSeries.Values.DataSource = new SeriesDataField(
                    ssView, 
                    "SBP", 
                    "Sheet1!$" + start + "$101:$" + end + "$101", 
                    SegmentDataType.AutoIndex, 
                    new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps }
                );
                DBPSeries.Values.DataSource = new SeriesDataField(ssView, 
                    "DBP", 
                    "Sheet1!$" + start + "$102:$" + end + "$102", 
                    SegmentDataType.AutoIndex, 
                    new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps }
                );
                PulseSeries.Values.DataSource = new SeriesDataField(
                    ssView, 
                    "맥박", 
                    "Sheet1!$" + start + "$103:$" + end + "$103", 
                    SegmentDataType.AutoIndex, 
                    new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps }
                );
                BreathSeries.Values.DataSource = new SeriesDataField(
                    ssView, 
                    "호흡", 
                    "Sheet1!$" + start + "$104:$" + end + "$104", 
                    SegmentDataType.AutoIndex, 
                    new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps }
                );

                Rectangle rectangle = SpChart.Rectangle;

                //  그래프 X축 기준점 정의
                //  변경전 X축(변경전 넓이 * 변경전 비율) / 변경후 넓이
                float x = (ChartRect.Width * YPlotLocation.X) / rectangle.Width;
                YPlotArea.Location = new PointF(x, 0.05F);
                
                //  ((변경전 넓이 * 변경전 비율) + (변경후 넓이 - 변경전 넓이)) / 변경후 넓이
                float w = ((ChartRect.Width * YPlotSize.Width) + (rectangle.Width - ChartRect.Width)) / rectangle.Width;
                YPlotArea.Size = new SizeF(w, 0.8F);

                ChartRect = rectangle;
                YPlotLocation = YPlotArea.Location;
                YPlotSize = YPlotArea.Size;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            
        }

        private void BtnChartSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            // 작성자, 수정자 체크
            if (mstrEmrNo != "0")
            {
                if (mstrCHARTUSEID != clsType.User.IdNumber)
                {
                    ComFunc.MsgBoxEx(this, "기존 작성자와 다릅니다." + ComNum.VBLF + "변경할 수 없습니다.");
                    return;
                }
            }

            if (EmrMstSave(sender.Equals(btnChartSave2)) == true)
            {
                ComFunc.MsgBox("저장 하였습니다.");
                btnBlood.Visible = mstrMode.Equals("W") && FormPatInfoFunc.Set_FormPatInfo_IsBlood(clsDB.DbCon, AcpEmr.ptNo, dtpOpDateFr.Value.ToString("yyyy-MM-dd"));
            }

            Cursor.Current = Cursors.Default;
        }

        private void BtnChartSearch_Click(object sender, EventArgs e)
        {
            //mstrEmrNo = "1072";
            //SetData();

            if (frmAnFormMappingX != null)
            {
                frmAnFormMappingX.Dispose();
                frmAnFormMappingX = null;
            }
            frmAnFormMappingX = new frmAnFormMapping(AcpEmr.ptNo, AcpEmr.medFrDate);
            frmAnFormMappingX.rGetPatientInfo += frmAnFormMappingX_rGetPatientInfo;
            frmAnFormMappingX.rEventClosed += frmAnFormMappingX_rEventClosed;
            frmAnFormMappingX.ShowDialog();
        }

        private void frmAnFormMappingX_rGetPatientInfo(OpSchedule opInfo)
        {
            if (frmAnFormMappingX != null)
            {
                frmAnFormMappingX.Dispose();
                frmAnFormMappingX = null;
            }

            if (opInfo == null) return;

            // 수술일
            if (VB.IsDate(opInfo.OpDate) == true)
            {
                dtpOpDateFr.Value = Convert.ToDateTime(opInfo.OpDate);
                dtpOpDateEnd.Value = Convert.ToDateTime(opInfo.OpDate);
            }

            //ASA
            switch (opInfo.AsaAdd)
            {
                case "1":
                    rdoASA_1.Checked = true;
                    break;
                case "2":
                    rdoASA_2.Checked = true;
                    break;
                case "3":
                    rdoASA_3.Checked = true;
                    break;
                case "4":
                    rdoASA_4.Checked = true;
                    break;
                case "5":
                    rdoASA_5.Checked = true;
                    break;
                case "6":
                    rdoASA_6.Checked = true;
                    break;
            }

            //NPO
            rdoNPO_Y.Checked = opInfo.NpoY == "true" ? true : false;
            rdoNPO_N.Checked = opInfo.NpoN == "true" ? true : false;

            // 혈액형
            txtBlod.Text = opInfo.ABO;

            // 수술실 룸번호
            txtRoomNo.Text = opInfo.OpRoom;

            // 상병
            txtPreopDx.Text = opInfo.PreOpDx;
            txtPostopDx.Text = opInfo.PostOpDx;

            // 수술명
            if (opInfo.PostOpTitle == "")
            {
                txtOperation.Text = opInfo.PreOpTitle;
            }
            else
            {
                txtOperation.Text = opInfo.PostOpTitle;
            }

            // 의사, 간호사
            RemoveLabelControl(pnlAnesDr);
            PnlAddLbl(pnlAnesDr, opInfo.AnesDr);
            RemoveLabelControl(pnlSurgeon);
            PnlAddLbl(pnlSurgeon, opInfo.Surgeon);
            RemoveLabelControl(pnlAssist);
            PnlAddLbl(pnlAssist, opInfo.Assist);
            RemoveLabelControl(pnlAnesNr);
            PnlAddLbl(pnlAnesNr, opInfo.AnesNr);
            RemoveLabelControl(pnlScrubNr);
            PnlAddLbl(pnlScrubNr, opInfo.ScrubNr);
            RemoveLabelControl(pnlCirNr);
            PnlAddLbl(pnlCirNr, opInfo.CirNr);

            // 검사결과
            txtSBP.Text = opInfo.Sbp;
            txtDBP.Text = opInfo.Dbp;
            txtPR.Text = opInfo.Pr;
            txtBW.Text = opInfo.Bw;
            txtBT2.Text = opInfo.Bt;

            txtHb.Text = opInfo.ExamHb;
            txtHct.Text = opInfo.ExamHct;
            txtOT.Text = opInfo.ExamGOT;
            txtPT.Text = opInfo.ExamGPT;
            txtNa.Text = opInfo.ExamNa;
            txtK.Text = opInfo.ExamK;
            txtPT2.Text = opInfo.ExamPT;
            txtPTT.Text = opInfo.ExamPTT;
            txtPLT.Text = opInfo.ExamPLT;
            txtWBC.Text = opInfo.ExamWBC;

            // 과거력
            chkHTN.Checked = opInfo.PastHxHTM.Equals("true") ? true : false;
            chkDM.Checked = opInfo.PastHxDM.Equals("true") ? true : false;
            chkTC.Checked = opInfo.PastHxTC.Equals("true") ? true : false;
            chkCC.Checked = opInfo.PastHxCC.Equals("true") ? true : false;
            chkLC.Checked = opInfo.PastHxLC.Equals("true") ? true : false;
            chkCD.Checked = opInfo.PastHxCD.Equals("true") ? true : false;
            chkOther.Checked = opInfo.PastHxOTHER.Equals("true") ? true : false;
            txtPastHx.Text = opInfo.PastHxOTHERTXT;


            // 수술실 입구 도착시간
            if (opInfo.Leave.Replace(":", "") != "")
            {
                txtArrive.Text = opInfo.Leave;
            }
            if (opInfo.Arrive.Replace(":", "") != "")
            {
                txtArrive.Text = opInfo.Arrive;
            }

            txtWRTNO.Text = opInfo.WrtNo;
        }

        private void PnlAddLbl(Panel panel, string strValue)
        {
            if (string.IsNullOrWhiteSpace(strValue)) return;

            strValue = strValue.Replace("RN", "").Trim();
            
            string userId = strValue.Split('/')[0];
            string userName = strValue.Split('/')[1];

            if (userName == "") return;

            Label label = new Label();
            label.Text = userName;
            label.Tag = userId;
            label.Width = LabelWidth;
            label.TextAlign = ContentAlignment.MiddleLeft;
            panel.Controls.Add(label);
            label.Dock = DockStyle.Left;

            label.DoubleClick += (s, e) => {
                Label lbl = (s as Label);
                lbl.Parent.Controls.Remove(lbl);
            };
        }

        private void frmAnFormMappingX_rEventClosed()
        {
            if (frmAnFormMappingX != null)
            {
                frmAnFormMappingX.Dispose();
                frmAnFormMappingX = null;
            }
        }

        /// <summary>
        /// 환자 조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUserSearch_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string gbn = btn.Tag.ToString();
            Panel panel = null;

            if(btn.Equals(btnAnesDr))
            {
                panel = pnlAnesDr;
            }
            else if(btn.Equals(btnSurgeon))
            {
                panel = pnlSurgeon;
            }
            else if (btn.Equals(btnAssist))
            {
                panel = pnlAssist;
            }
            else if (btn.Equals(btnAnesNr))
            {
                panel = pnlAnesNr;
            }
            else if (btn.Equals(btnScrubNr))
            {
                panel = pnlScrubNr;
            }
            else if (btn.Equals(btnCirNr))
            {
                panel = pnlCirNr;
            }

            FindUserSearch(gbn, panel);
        }


        private void Spread_KeyDown(object sender, KeyEventArgs e)
        {

        }

        /// <summary>
        /// 약품정보 수정 완료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Spread_EditModeOff(object sender, EventArgs e)
        {
            CurrentFpSpread = (sender as FpSpread);

            int currentCol = CurrentFpSpread.ActiveSheet.ActiveColumnIndex;
            if (currentCol != 0)
            {
                return;
            }

            Rectangle rt = CurrentFpSpread.GetCellRectangle(0, 0, CurrentFpSpread.ActiveSheet.ActiveRowIndex, CurrentFpSpread.ActiveSheet.ActiveColumnIndex);
            CurrentSpreadRowIndex = CurrentFpSpread.ActiveSheet.ActiveRowIndex;

            ssAuto.Left = CurrentFpSpread.Left + rt.X;
            ssAuto.Top = tabControl1.Top + panTop.Top + tableLayoutPanel17.Top + CurrentFpSpread.Top + 50 + rt.Y;// + ssMedication.Top + rt.Y + rt.Height ;

            ssAuto.Enabled = true;
        }

        /// <summary>
        /// 약품정보 키업 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Spread_KeyUp(object sender, KeyEventArgs e)
        {
            CurrentFpSpread = (sender as FpSpread);

            if (!ssAuto.Enabled)
            {
                return;
            }

            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                //ssAuto.Enabled = true;
                ssAuto.Visible = true;
                ssAuto.BringToFront();

                CurrentFpSpread.ActiveSheet.ActiveRowIndex = CurrentSpreadRowIndex;
                SetAutoData();
            }
        }

        /// <summary>
        /// 약품정보 자동완성
        /// </summary>
        private void SetAutoData()
        {
            DataTable dt = null;
            try
            {
                dt = GetAutoData();
                ssAuto.ActiveSheet.RowCount = 0;
                ssAuto.ActiveSheet.RowCount = dt.Rows.Count;

                if(dt != null && dt.Rows.Count > 0)
                {
                    ssAuto.Focus();
                    ssAuto.ActiveSheet.SetActiveCell(0, 0);
                }
                else
                {
                    SsAuto_KeyUp(null, new KeyEventArgs(Keys.Escape));
                }

                for (int i=0; i<ssAuto.ActiveSheet.RowCount; i++)
                {
                    //? 0 : 약품명
                    //? 2 : 단위
                    //? 3 : 코드
                    ssAuto.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["CODE"].ToString();
                    ssAuto.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["NAME"].ToString();
                    ssAuto.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["UNIT"].ToString();
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 엔터
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SsAuto_KeyUp(object sender, KeyEventArgs e)
        {
            //CurrentFpSpread = (sender as FpSpread);

            if (e.KeyCode == Keys.Escape)
            {
                ssAuto.Enabled = false;
                ssAuto.Visible = false;
                ssAuto.ActiveSheet.Rows.Clear();
                CurrentFpSpread.Focus();
            }
            else if(e.KeyCode == Keys.Enter)
            {
                //? 0 : 약품명
                //? 2 : 단위
                //? 3 : 코드
                int autoRowIndex = ssAuto.ActiveSheet.ActiveRowIndex;
                int currentRowIndex = CurrentFpSpread.ActiveSheet.ActiveRowIndex;
                CurrentFpSpread.ActiveSheet.Cells[currentRowIndex, 0].Text = ssAuto.ActiveSheet.Cells[autoRowIndex, 1].Text;
                CurrentFpSpread.ActiveSheet.Cells[currentRowIndex, 2].Text = ssAuto.ActiveSheet.Cells[autoRowIndex, 2].Text;
                CurrentFpSpread.ActiveSheet.Cells[currentRowIndex, 3].Text = ssAuto.ActiveSheet.Cells[autoRowIndex, 0].Text;

                ssAuto.Enabled = false;
                ssAuto.Visible = false;
                ssAuto.ActiveSheet.Rows.Clear();

                CurrentFpSpread.Focus();
            }
        }

        /// <summary>
        /// 더블클릭으로 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Spread_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            CurrentFpSpread = (sender as FpSpread);
            int rowIndex = CurrentFpSpread.ActiveSheet.ActiveRowIndex;

            if(rowIndex < 0)
            {
                return;
            }

            CurrentFpSpread.ActiveSheet.RemoveRows(rowIndex, 1);
            CurrentFpSpread.ActiveSheet.AddRows(CurrentFpSpread.ActiveSheet.RowCount, 1);
        }

        /// <summary>
        /// 스프레드 입력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Spread_EnterCell(object sender, EnterCellEventArgs e)
        {
            if (ssAuto.Visible)
            {
                ssAuto.Enabled = false;
                ssAuto.Visible = false;
            }
        }

        private void frmAnForm_Resize(object sender, EventArgs e)
        {
            panTopSub.Top = 0;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // 작성자, 수정자 체크
            if (mstrEmrNo != "0")
            {
                if (mstrCHARTUSEID != clsType.User.IdNumber)
                {
                    ComFunc.MsgBoxEx(this, "기존 작성자와 다릅니다." + ComNum.VBLF + "변경할 수 없습니다.");
                    return;
                }
            }

            if (EmrMstDelete() == true)
            {
                ComFunc.MsgBox("삭제 하였습니다.");
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            //PrintAnForm("V");
            PrintAnForm();

            Cursor.Current = Cursors.Default;
        }


        private int pPrintForm()
        {
            return PrintAnForm();
        }

        private int PrintAnForm(string strPrintType = "P")
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int rtnVal = 0;
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

            if (mstrEmrNo == "0") return 0;

            int Page = 0;
            int ColCount = ssView.ActiveSheet.ColumnCount;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT                               ";
                SQL = SQL + ComNum.VBLF + "    CEIL(COUNT(*) / 27) PAGE         ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRANCHARTGRAPE   ";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + mstrEmrNo;
                SQL = SQL + ComNum.VBLF + "   AND EMRNOHIS = " + mstrEmrHisNo;
                SQL = SQL + ComNum.VBLF + "   AND ITEMCODE = 'I0000001164'      ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                Page = (int)VB.Val(dt.Rows[0]["PAGE"].ToString().Trim());
                ssView.ActiveSheet.ColumnCount = (Page * 27) + 2;
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                ComFunc.MsgBoxEx(this, ex.Message);
                return rtnVal;
            }

            for (int p = 0; p <= Page + 1; p++)
            {
                if (p == 0)
                {
                    //FpSpread ssPrint1 = clsEmrAnChartPrint.CreatePrintSpreadPage1(this);

                    //SetPrintDataPage1(ref ssPrint1, p, Page);

                    //rtnVal = clsSpreadPrint.PrintSpdAnForm(clsDB.DbCon, mstrFormNo, mstrUpdateNo, AcpEmr, true, ref ssPrint1, strPrintType, 30, 20, 10, 0, false,
                    //            FarPoint.Win.Spread.PrintOrientation.Portrait, strCurDateTime, p + 1, Page + 1, 0.8f);
                    //ssPrint1.PrintSheet(0);

                    FpSpread ssPrint1 = clsEmrAnChartPrint.CreatePrintSpreadNewPage1(this);

                    SetPrintDataPage1(ref ssPrint1);

                    rtnVal = clsSpreadPrint.PrintSpdAnForm(clsDB.DbCon, mstrFormNo, mstrUpdateNo, AcpEmr, true, ref ssPrint1, strPrintType, 30, 20, 10, 0, false,
                                FarPoint.Win.Spread.PrintOrientation.Portrait, strCurDateTime, 1, Page + 2, 0.8f);
                    ssPrint1.PrintSheet(0);
                }
                else if (p == 1)
                {
                    FpSpread ssPrint2 = clsEmrAnChartPrint.CreatePrintSpreadNewPage2(this);

                    SetPrintDataNewPage2(ref ssPrint2);

                    rtnVal = clsSpreadPrint.PrintSpdAnForm(clsDB.DbCon, mstrFormNo, mstrUpdateNo, AcpEmr, true, ref ssPrint2, strPrintType, 30, 20, 10, 0, false,
                                FarPoint.Win.Spread.PrintOrientation.Portrait, strCurDateTime, 2, Page + 2, 0.8f);
                    ssPrint2.PrintSheet(0);
                }
                else
                {
                    FpSpread ssPrint2 = clsEmrAnChartPrint.CreatePrintSpreadPage2(this);

                    SetPrintDataPage2(ref ssPrint2, p - 1);

                    rtnVal = clsSpreadPrint.PrintSpdAnForm(clsDB.DbCon, mstrFormNo, mstrUpdateNo, AcpEmr, true, ref ssPrint2, strPrintType, 30, 20, 10, 0, false,
                                FarPoint.Win.Spread.PrintOrientation.Portrait, strCurDateTime, p + 1, Page + 2, 0.75f);
                    ssPrint2.PrintSheet(0);
                }

                ComFunc.Delay(1000);
            }

            ssView.ActiveSheet.ColumnCount = ColCount;

            return rtnVal;
        }

        #region // 출력물 1페이지 데이터 세팅
        /// <summary>
        /// 출력물 1페이지 데이터 세팅
        /// </summary>
        /// <param name="ssPrint"></param>
        /// <param name="page"></param>
        /// <param name="TotalPage"></param>
        private void SetPrintDataPage1(ref FpSpread ssPrint)
        {
            #region // 수술일자 ~ 마취간호사
            ssPrint.ActiveSheet.Cells[1, 2].Text = dtpOpDateFr.Value.ToString("yyyy.MM.dd") + " ~ " + dtpOpDateEnd.Value.ToString("yyyy.MM.dd");
            ssPrint.ActiveSheet.Cells[1, 11].Text = txtBlod.Text;
            ssPrint.ActiveSheet.Cells[1, 15].Value = rdoNPO_Y.Checked;
            ssPrint.ActiveSheet.Cells[1, 16].Value = rdoNPO_N.Checked;
            ssPrint.ActiveSheet.Cells[1, 18].Text = txtNPO_Etc.Text;

            ssPrint.ActiveSheet.Cells[2, 2].Value = rdoASA_1.Checked;
            ssPrint.ActiveSheet.Cells[2, 4].Value = rdoASA_2.Checked;
            ssPrint.ActiveSheet.Cells[2, 6].Value = rdoASA_3.Checked;
            ssPrint.ActiveSheet.Cells[2, 8].Value = rdoASA_4.Checked;
            ssPrint.ActiveSheet.Cells[2, 10].Value = rdoASA_5.Checked;
            ssPrint.ActiveSheet.Cells[2, 12].Value = rdoASA_6.Checked;
            ssPrint.ActiveSheet.Cells[2, 14].Value = chkEmergency.Checked;
            ssPrint.ActiveSheet.Cells[2, 18].Text = txtRoomNo.Text;

            // 수술전 진단명
            ssPrint.ActiveSheet.Cells[3, 3].Text = txtPreopDx.Text;
            // 수술후 진단명
            ssPrint.ActiveSheet.Cells[4, 3].Text = txtPostopDx.Text;
            // 수술명
            ssPrint.ActiveSheet.Cells[5, 3].Text = txtOperation.Text;
            
            // 마취의사
            ssPrint.ActiveSheet.Cells[6, 2].Text = ReadAnesName(pnlAnesDr);
            ssPrint.ActiveSheet.Cells[6, 14].Text = ReadAnesName(pnlSurgeon);
            ssPrint.ActiveSheet.Cells[6, 23].Text = ReadAnesName(pnlAssist);
            // 마취간호사
            ssPrint.ActiveSheet.Cells[7, 2].Text = ReadAnesName(pnlAnesNr);
            ssPrint.ActiveSheet.Cells[7, 14].Text = ReadAnesName(pnlScrubNr);
            ssPrint.ActiveSheet.Cells[7, 23].Text = ReadAnesName(pnlCirNr);
            #endregion

            #region // 검사결과
            ssPrint.ActiveSheet.Cells[8, 4].Text = txtHb.Text;
            ssPrint.ActiveSheet.Cells[8, 6].Text = txtHct.Text;
            ssPrint.ActiveSheet.Cells[8, 9].Text = txtOT.Text;
            ssPrint.ActiveSheet.Cells[8, 11].Text = txtPT.Text;
            ssPrint.ActiveSheet.Cells[8, 14].Text = txtNa.Text;
            ssPrint.ActiveSheet.Cells[8, 16].Text = txtK.Text;
            ssPrint.ActiveSheet.Cells[8, 19].Text = txtPT2.Text;
            ssPrint.ActiveSheet.Cells[8, 21].Text = txtPTT.Text;
            ssPrint.ActiveSheet.Cells[8, 26].Text = txtPLT.Text;
            ssPrint.ActiveSheet.Cells[8, 29].Text = txtWBC.Text;
            #endregion

            #region // 바이탈            
            ssPrint.ActiveSheet.Cells[9, 3].Text = string.Concat(txtSBP.Text, (txtSBP.Text.Trim() != "" ? "mmHg" : ""));
            ssPrint.ActiveSheet.Cells[9, 6].Text = string.Concat(txtDBP.Text, (txtDBP.Text.Trim() != "" ? "mmHg" : ""));
            ssPrint.ActiveSheet.Cells[9, 9].Text = string.Concat(txtPR.Text, (txtPR.Text.Trim() != "" ? "회/m" : ""));
            ssPrint.ActiveSheet.Cells[9, 13].Text = string.Concat(txtBW.Text, (txtBW.Text.Trim() != "" ? "Kg" : ""));
            ssPrint.ActiveSheet.Cells[9, 16].Text = string.Concat(txtBT2.Text, (txtBT2.Text.Trim() != "" ? "℃" : ""));
            ssPrint.ActiveSheet.Cells[9, 20].Text = txtEKG.Text;
            #endregion

            #region // 과거력
            ssPrint.ActiveSheet.Cells[10, 2].Value = chkHTN.Checked;
            ssPrint.ActiveSheet.Cells[10, 5].Value = chkDM.Checked;
            ssPrint.ActiveSheet.Cells[10, 8].Value = chkTC.Checked;
            ssPrint.ActiveSheet.Cells[10, 11].Value = chkCC.Checked;
            ssPrint.ActiveSheet.Cells[10, 14].Value = chkLC.Checked;
            ssPrint.ActiveSheet.Cells[10, 17].Value = chkCD.Checked;
            ssPrint.ActiveSheet.Cells[11, 2].Value = chkOther.Checked;
            ssPrint.ActiveSheet.Cells[11, 4].Text = txtPastHx.Text;
            #endregion

            #region // Consult,  Mentality, Remark
            ssPrint.ActiveSheet.Cells[12, 2].Text = txtConsult.Text;
            
            ssPrint.ActiveSheet.Cells[13, 2].Value = chkMentalityAlert.Checked;
            ssPrint.ActiveSheet.Cells[13, 5].Value = chkMentalityDrowsy.Checked;
            ssPrint.ActiveSheet.Cells[13, 8].Value = chkMentalityStupor.Checked;
            ssPrint.ActiveSheet.Cells[13, 11].Value = chkMentalitySemicoma.Checked;
            ssPrint.ActiveSheet.Cells[13, 14].Value = chkMentalityComa.Checked;
            ssPrint.ActiveSheet.Cells[13, 17].Value = txtMentalityRemark.Text;
            
            ssPrint.ActiveSheet.Cells[14, 2].Text = txtRemark.Text;
            #endregion
            
            #region // Pre induction assessment
            ssPrint.ActiveSheet.Cells[17, 1].Text = txtPreSBP.Text;
            ssPrint.ActiveSheet.Cells[17, 3].Text = txtPreDBP.Text;
            ssPrint.ActiveSheet.Cells[17, 5].Text = txtPrePR.Text;
            ssPrint.ActiveSheet.Cells[18, 1].Text = txtPreSPO2.Text;
            ssPrint.ActiveSheet.Cells[18, 5].Text = txtPreBT.Text;
            ssPrint.ActiveSheet.Cells[19, 1].Text = txtPreEKG.Text;
            ssPrint.ActiveSheet.Cells[20, 2].Value = rdoPreNPO_Y.Checked;
            ssPrint.ActiveSheet.Cells[20, 4].Value = rdoPreNPO_N.Checked;
            ssPrint.ActiveSheet.Cells[21, 2].Value = rdoAttention_Y.Checked;
            ssPrint.ActiveSheet.Cells[21, 4].Value = rdoAttention_N.Checked;
            ssPrint.ActiveSheet.Cells[22, 0].Text = txtPreEtc.Text;
            #endregion

            #region // OP POSITION
            ssPrint.ActiveSheet.Cells[27, 0].Value = chkSupine.Checked;
            ssPrint.ActiveSheet.Cells[27, 3].Value = chkProne.Checked;
            ssPrint.ActiveSheet.Cells[28, 0].Value = chkLithotomy.Checked;
            ssPrint.ActiveSheet.Cells[29, 0].Value = chkJack.Checked;
            ssPrint.ActiveSheet.Cells[30, 0].Value = chkLateral.Checked;
            ssPrint.ActiveSheet.Cells[30, 3].Value = chkLateral_R.Checked;
            ssPrint.ActiveSheet.Cells[30, 5].Value = chkLateral_L.Checked;
            ssPrint.ActiveSheet.Cells[31, 0].Value = chkSemi.Checked;
            #endregion

            #region // 전신마취
            ssPrint.ActiveSheet.Cells[17, 7].Value = chkGeneral.Checked;
            ssPrint.ActiveSheet.Cells[17, 10].Value = chkDifficult.Checked;

            ssPrint.ActiveSheet.Cells[18, 8].Text = txtETT.Text;
            ssPrint.ActiveSheet.Cells[18, 12].Value = chkCuff.Checked;

            ssPrint.ActiveSheet.Cells[19, 7].Value = chkOral.Checked;
            ssPrint.ActiveSheet.Cells[19, 8].Value = chkNasal.Checked;
            ssPrint.ActiveSheet.Cells[19, 10].Value = chkTrac.Checked;
            ssPrint.ActiveSheet.Cells[19, 12].Value = chkWire.Checked;

            ssPrint.ActiveSheet.Cells[20, 7].Value = chkMask.Checked;
            ssPrint.ActiveSheet.Cells[20, 10].Value = chkMac.Checked;

            ssPrint.ActiveSheet.Cells[21, 7].Value = chkLMA.Checked;
            ssPrint.ActiveSheet.Cells[21, 10].Text = txtLMA.Text;
            ssPrint.ActiveSheet.Cells[21, 12].Text = txtLMARemark.Text;

            ssPrint.ActiveSheet.Cells[22, 7].Value = chkOnelung.Checked;
            ssPrint.ActiveSheet.Cells[22, 10].Value = chkOnelungR.Checked;
            ssPrint.ActiveSheet.Cells[22, 11].Value = chkOnelungL.Checked;
            ssPrint.ActiveSheet.Cells[22, 12].Text = txtOnelung.Text;
            #endregion

            #region // 부위마취
            ssPrint.ActiveSheet.Cells[24, 7].Value = chkSpinal.Checked;
            ssPrint.ActiveSheet.Cells[24, 10].Text = txtSpinal.Text;
            ssPrint.ActiveSheet.Cells[25, 7].Text = txtSpinalProduct.Text;
            ssPrint.ActiveSheet.Cells[26, 7].Value = chkEpidural.Checked;
            ssPrint.ActiveSheet.Cells[26, 10].Text = txtEpidural.Text;
            ssPrint.ActiveSheet.Cells[27, 7].Text = txtEpiduralProduct.Text;
            ssPrint.ActiveSheet.Cells[28, 7].Value = chkCaudal.Checked;
            ssPrint.ActiveSheet.Cells[28, 10].Text = txtCaudal.Text;
            ssPrint.ActiveSheet.Cells[29, 7].Text = txtCaudalProduct.Text;
            
            ssPrint.ActiveSheet.Cells[30, 7].Value = chkNB.Checked;
            ssPrint.ActiveSheet.Cells[30, 9].Value = chkSonoguide.Checked;

            ssPrint.ActiveSheet.Cells[31, 7].Value = chkFNB.Checked;
            ssPrint.ActiveSheet.Cells[31, 9].Value = chkSNB.Checked;
            ssPrint.ActiveSheet.Cells[31, 11].Value = chkSNB_P.Checked;
            ssPrint.ActiveSheet.Cells[31, 13].Value = chkSNB_G.Checked;

            ssPrint.ActiveSheet.Cells[32, 7].Value = chkACB.Checked;
            ssPrint.ActiveSheet.Cells[32, 9].Value = chkBPB.Checked;
            ssPrint.ActiveSheet.Cells[32, 11].Value = chkBPB_A.Checked;
            ssPrint.ActiveSheet.Cells[32, 13].Value = chkBPB_I.Checked;

            ssPrint.ActiveSheet.Cells[33, 7].Value = chkETC.Checked;
            ssPrint.ActiveSheet.Cells[34, 7].Text = txtEtc.Text;
            #endregion


            #region // PCA
            ssPrint.ActiveSheet.Cells[17, 15].Value = rdoPCAIV.Checked;
            ssPrint.ActiveSheet.Cells[17, 16].Value = rdoPCAEP.Checked;

            if (ssPCA.ActiveSheet.RowCount > 15)
            {
                for (int i = 0; i < 15; i++)
                {
                    ssPrint.ActiveSheet.Cells[18 + i, 15].Text = ssPCA.ActiveSheet.Cells[i, 0].Text + " " + ssPCA.ActiveSheet.Cells[i, 1].Text + " " + ssPCA.ActiveSheet.Cells[i, 2].Text;
                }
            }
            else
            {
                for (int i = 0; i < ssPCA.ActiveSheet.RowCount; i++)
                {
                    ssPrint.ActiveSheet.Cells[18 + i, 15].Text = ssPCA.ActiveSheet.Cells[i, 0].Text + " " + ssPCA.ActiveSheet.Cells[i, 1].Text + " " + ssPCA.ActiveSheet.Cells[i, 2].Text;
                }
            }
            #endregion

            #region // Intake
            for (int i = 0; i < ssIn.ActiveSheet.RowCount - 1; i++)
            {
                ssPrint.ActiveSheet.Cells[17 + i, 19].Text = ssIn.ActiveSheet.Cells[i, 0].Text;
                ssPrint.ActiveSheet.Cells[17 + i, 22].Text = ssIn.ActiveSheet.Cells[i, 1].Text;
                ssPrint.ActiveSheet.Cells[17 + i, 24].Text = ssIn.ActiveSheet.Cells[i, 2].Text;
            }            
            ssPrint.ActiveSheet.Cells[35, 22].Text = ssIn.ActiveSheet.Cells[ssIn.ActiveSheet.RowCount - 1, 2].Text;            
            #endregion

            #region // Output
            for (int i = 0; i < ssOut.ActiveSheet.RowCount - 1; i++)
            {
                ssPrint.ActiveSheet.Cells[17 + i, 26].Text = ssOut.ActiveSheet.Cells[i, 0].Text;
                ssPrint.ActiveSheet.Cells[17 + i, 29].Text = ssOut.ActiveSheet.Cells[i, 1].Text;
            }
            ssPrint.ActiveSheet.Cells[31, 29].Text = ssOut.ActiveSheet.Cells[ssOut.ActiveSheet.RowCount - 1, 1].Text;
            #endregion
            

            Control[] controls = null;
            controls = ComFunc.GetAllControls(pnlAnesDr);

            foreach (Control objControl in controls)
            {
                if (objControl is Label)
                {
                    ssPrint.ActiveSheet.Cells[34, 26].HorizontalAlignment = CellHorizontalAlignment.Center;
                    ssPrint.ActiveSheet.Cells[34, 26].Text = ((Label)objControl).Text;
                    SetDrSign(ssPrint, 35, 26, ((Label)objControl).Tag.ToString());
                    break;
                }
            }
        }
        #endregion


        private void SetPrintDataNewPage2(ref FpSpread ssPrint)
        {

            #region // Medication
            for (int i = 0; i < ssMedication.ActiveSheet.RowCount; i++)
            {
                if (i == 58) break;
                if (i < 29)
                {
                    ssPrint.ActiveSheet.Cells[3 + i, 0].Text = ssMedication.ActiveSheet.Cells[i, 0].Text;
                    ssPrint.ActiveSheet.Cells[3 + i, 1].Text = ssMedication.ActiveSheet.Cells[i, 1].Text;
                    ssPrint.ActiveSheet.Cells[3 + i, 2].Text = ssMedication.ActiveSheet.Cells[i, 2].Text;
                    ssPrint.ActiveSheet.Cells[3 + i, 3].Text = ssMedication.ActiveSheet.Cells[i, 4].Text;                    
                }
                else
                {
                    ssPrint.ActiveSheet.Cells[3 + (i - 29), 4].Text = ssMedication.ActiveSheet.Cells[i, 0].Text;
                    ssPrint.ActiveSheet.Cells[3 + (i - 29), 5].Text = ssMedication.ActiveSheet.Cells[i, 1].Text;
                    ssPrint.ActiveSheet.Cells[3 + (i - 29), 6].Text = ssMedication.ActiveSheet.Cells[i, 2].Text;
                    ssPrint.ActiveSheet.Cells[3 + (i - 29), 7].Text = ssMedication.ActiveSheet.Cells[i, 4].Text;
                }                
            }            
            #endregion


            #region // 물품
            for (int i = 0; i < ssJep.ActiveSheet.RowCount; i++)
            {
                if (i == 17) break;
                ssPrint.ActiveSheet.Cells[3 + i, 8].Text = ssJep.ActiveSheet.Cells[i, 0].Text;
                ssPrint.ActiveSheet.Cells[3 + i, 9].Text = ssJep.ActiveSheet.Cells[i, 1].Text;
                ssPrint.ActiveSheet.Cells[3 + i, 10].Text = ssJep.ActiveSheet.Cells[i, 2].Text;
            }            
            #endregion


            #region // 회복실
            for (int i = 0; i < ssRecovery.ActiveSheet.RowCount; i++)
            {
                if (i == 10) break;
                ssPrint.ActiveSheet.Cells[22 + i, 8].Text = ssRecovery.ActiveSheet.Cells[i, 0].Text;
                ssPrint.ActiveSheet.Cells[22 + i, 9].Text = ssRecovery.ActiveSheet.Cells[i, 1].Text;
                ssPrint.ActiveSheet.Cells[22 + i, 10].Text = ssRecovery.ActiveSheet.Cells[i, 2].Text;
                ssPrint.ActiveSheet.Cells[22 + i, 11].Text = ssRecovery.ActiveSheet.Cells[i, 4].Text;
            }            
            #endregion

        }


        #region // 출력물 2페이지 데이터 세팅
        /// <summary>
        /// 출력물 2페이지 데이터 세팅
        /// </summary>
        /// <param name="ssPrint"></param>
        /// <param name="page"></param>
        /// <param name="TotalPage"></param>
        private void SetPrintDataPage2(ref FpSpread ssPrint, int page)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            SpreadChart PrintChart = null;

            // 수술장 입구 도착            
            ssPrint.ActiveSheet.Cells[1, 0].Text = txtArrive.Text;

            #region // ITEM 항목
            //  View Spread 항목설정            
            List<MedcationInfo> list = Medcations.ToList().FindAll(r => r.IsView);            
            for (int i = 0; i < list.Count; i++)
            {
                CheckBoxCellType checkBoxCell = new CheckBoxCellType();
                checkBoxCell.Caption = list[i].Name;
                
                ssPrint.ActiveSheet.Cells[i + 2, 0].CellType = checkBoxCell;
                ssPrint.ActiveSheet.Cells[i + 2, 0].Value = true;                
                ssPrint.ActiveSheet.Cells[i + 2, 9].Text = list[i].Uint;

                ssPrint.ActiveSheet.Cells[i + 2, 0].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssPrint.ActiveSheet.Cells[i + 2, 9].HorizontalAlignment = CellHorizontalAlignment.Center;
            }

            //foreach (MedcationInfo item in Medcations)
            //{
            //    switch (item.ItemNo)
            //    {
            //        case "I0000001164":     //마취
            //            ssPrint.ActiveSheet.Cells[2, 0].Value = true;
            //            break;
            //        case "I0000001422":     //수술
            //            ssPrint.ActiveSheet.Cells[3, 0].Value = true;
            //            break;
            //        case "I0000012402":     //삽관
            //            ssPrint.ActiveSheet.Cells[4, 0].Value = true;
            //            break;
            //        case "I0000015889":     //N2O
            //            ssPrint.ActiveSheet.Cells[5, 0].Value = true;
            //            break;
            //        case "I0000022307":     //O2
            //            ssPrint.ActiveSheet.Cells[6, 0].Value = true;
            //            break;
            //        case "I0000034070":     //Sevo
            //            ssPrint.ActiveSheet.Cells[7, 0].Value = true;
            //            break;
            //        case "I0000010880":     //Propofol
            //            ssPrint.ActiveSheet.Cells[8, 0].Value = true;
            //            break;
            //        case "I0000008708":     //SpO2 (%)
            //            ssPrint.ActiveSheet.Cells[9, 0].Value = true;
            //            break;
            //        case "I0000031627":     //ETCO2
            //            ssPrint.ActiveSheet.Cells[10, 0].Value = true;
            //            break;
            //        case "I0000022536":     //Temp
            //            ssPrint.ActiveSheet.Cells[11, 0].Value = true;
            //            break;
            //        case "I0000030240":     //EKG monitoring
            //            ssPrint.ActiveSheet.Cells[12, 0].Value = true;
            //            break;
            //        case "I0000034071":     //U/O
            //            ssPrint.ActiveSheet.Cells[13, 0].Value = true;
            //            break;
            //        case "I0000034072":     //Warm blanket
            //            ssPrint.ActiveSheet.Cells[14, 0].Value = true;
            //            break;
            //        case "I0000030758":     //Tourniquet
            //            ssPrint.ActiveSheet.Cells[15, 0].Value = true;
            //            break;
            //    }
            //}
            #endregion

            #region // Tourniquest List
            try
            {
                string strHH = string.Empty;
                string strMM = string.Empty;
                int TourniquestRow = 20;

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT                                   ";
                SQL = SQL + ComNum.VBLF + "    HOUR, MINUTE, VALUE, UNIT            ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.AEMRANCHARTGRAPE a      ";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO    = " + mstrEmrNo    ;
                SQL = SQL + ComNum.VBLF + "  AND EMRNOHIS = " + mstrEmrHisNo;
                SQL = SQL + ComNum.VBLF + "  AND ITEMCODE = 'I0000030758'           ";
                SQL = SQL + ComNum.VBLF + "  AND VALUE IS NOT NULL                  ";
                SQL = SQL + ComNum.VBLF + "ORDER BY COLINDEX                        ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                foreach (DataRow row in dt.Rows)
                {
                    strHH = row["HOUR"].ToString();
                    strMM = row["MINUTE"].ToString();

                    string[] split = row["UNIT"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in split)
                    {
                        string[] item = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                        if (item.Length > 0)
                        {
                            ssPrint.ActiveSheet.Cells[TourniquestRow, 0].Text = string.Concat(strHH, ":", strMM);
                            ssPrint.ActiveSheet.Cells[TourniquestRow, 3].Text = item[0];
                            ssPrint.ActiveSheet.Cells[TourniquestRow, 6].Text = item[1];
                            ssPrint.ActiveSheet.Cells[TourniquestRow, 9].Text = item[2];

                            TourniquestRow = TourniquestRow + 1;

                            if (TourniquestRow > 35) return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
            #endregion

            #region // BST 
            for (int i = 0; i < ssBST.ActiveSheet.RowCount; i++)
            {
                if (i <= 4)
                {
                    // 좌
                    ssPrint.ActiveSheet.Cells[37 + i, 0].Text = ssBST.ActiveSheet.Cells[i, 0].Text;
                    ssPrint.ActiveSheet.Cells[37 + i, 3].Text = ssBST.ActiveSheet.Cells[i, 1].Text;                    
                }
                else
                {
                    // 우
                    ssPrint.ActiveSheet.Cells[37 + (i - 5), 6].Text = ssBST.ActiveSheet.Cells[i, 0].Text;
                    ssPrint.ActiveSheet.Cells[37 + (i - 5), 9].Text = ssBST.ActiveSheet.Cells[i, 1].Text;
                }
            }
            #endregion
            
            #region // 그래프 상단 시간
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT                               ";
                SQL = SQL + ComNum.VBLF + "      HOUR                           ";
                SQL = SQL + ComNum.VBLF + "    , MINUTE                         ";
                SQL = SQL + ComNum.VBLF + "    , COLINDEX + 11 AS COLINDEX      ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRANCHARTGRAPE   ";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO    = " + mstrEmrNo   ;
                SQL = SQL + ComNum.VBLF + "   AND EMRNOHIS = " + mstrEmrHisNo;
                SQL = SQL + ComNum.VBLF + "   AND ITEMCODE = 'I0000001164'      ";
                SQL = SQL + ComNum.VBLF + "   AND COLINDEX >= " + (((27 * page) - 27) + 2);
                SQL = SQL + ComNum.VBLF + "   AND COLINDEX < " + ((27 * page) + 2);
                SQL = SQL + ComNum.VBLF + " ORDER BY COLINDEX                   ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                foreach (DataRow row in dt.Rows)
                {

                    ssPrint.ActiveSheet.Cells[0, (int)VB.Val(row["COLINDEX"].ToString()) - (27 * (page - 1))].Text = row["HOUR"].ToString();
                    ssPrint.ActiveSheet.Cells[1, (int)VB.Val(row["COLINDEX"].ToString()) - (27 * (page - 1))].Text = row["MINUTE"].ToString();

                }

                //ssPrint.ActiveSheet.SetRowMerge(10, MergePolicy.Always);
                //ssPrint.ActiveSheet.SetRowMerge(11, MergePolicy.Always);
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
            #endregion

            #region // 그래프 상단 ITEM
            try
            {
                SQL = "";
                //SQL = SQL + ComNum.VBLF + "SELECT ITEM                                                              ";
                //SQL = SQL + ComNum.VBLF + "     , ITEMCODE                                                          ";
                //SQL = SQL + ComNum.VBLF + "     , VALUE                                                            ";
                //SQL = SQL + ComNum.VBLF + "     , UNIT,                                                            ";
                //SQL = SQL + ComNum.VBLF + "     CASE                                                                ";
                //SQL = SQL + ComNum.VBLF + "        WHEN ITEMCODE = 'I0000001164' THEN '2'  --마취                    ";
                //SQL = SQL + ComNum.VBLF + "        WHEN ITEMCODE = 'I0000001422' THEN '3'  --수술                    ";
                //SQL = SQL + ComNum.VBLF + "        WHEN ITEMCODE = 'I0000012402' THEN '4'  --삽관                    ";
                //SQL = SQL + ComNum.VBLF + "        WHEN ITEMCODE = 'I0000015889' THEN '5'  --N2O                    ";
                //SQL = SQL + ComNum.VBLF + "        WHEN ITEMCODE = 'I0000022307' THEN '6'  --O2                     ";
                //SQL = SQL + ComNum.VBLF + "        WHEN ITEMCODE = 'I0000034070' THEN '7'  --Sevo                   ";
                //SQL = SQL + ComNum.VBLF + "        WHEN ITEMCODE = 'I0000010880' THEN '8'  --Propofol               ";
                //SQL = SQL + ComNum.VBLF + "        WHEN ITEMCODE = 'I0000008708' THEN '9'  --SpO2                   ";
                //SQL = SQL + ComNum.VBLF + "        WHEN ITEMCODE = 'I0000031627' THEN '10'  --ETCO2                 ";
                //SQL = SQL + ComNum.VBLF + "        WHEN ITEMCODE = 'I0000022536' THEN '11'  --Temp                  ";
                //SQL = SQL + ComNum.VBLF + "        WHEN ITEMCODE = 'I0000030240' THEN '12'  --EKG monitoring        ";
                //SQL = SQL + ComNum.VBLF + "        WHEN ITEMCODE = 'I0000034071' THEN '13'  --U / O                 ";
                //SQL = SQL + ComNum.VBLF + "        WHEN ITEMCODE = 'I0000034072' THEN '14'  --Warm blanket          ";
                //SQL = SQL + ComNum.VBLF + "        WHEN ITEMCODE = 'I0000030758' THEN '15'  --Tourniquet            ";
                //SQL = SQL + ComNum.VBLF + "     END AS ROWINDEX                                                     ";
                //SQL = SQL + ComNum.VBLF + "     , (COLINDEX + 11) AS COLINDEX                                       ";
                //SQL = SQL + ComNum.VBLF + "     , COLINDEX AS COLINDEXOLD                                           ";
                //SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRANCHARTGRAPE                                       ";
                //SQL = SQL + ComNum.VBLF + " WHERE ITEMCODE IN('I0000015889', 'I0000022307','I0000034070', 'I0000010880','I0000008708', 'I0000031627'    ";
                //SQL = SQL + ComNum.VBLF + " 	             ,'I0000022536', 'I0000030240','I0000034071', 'I0000034072','I0000030758'                   ";
                //SQL = SQL + ComNum.VBLF + " 	             ,'I0000001164', 'I0000001422','I0000012402','I0000000574') ";
                SQL = SQL + ComNum.VBLF + "SELECT                                                                               ";
                SQL = SQL + ComNum.VBLF + "      ITEM                                                                           ";
                SQL = SQL + ComNum.VBLF + "    , ITEMCODE                                                                       ";
                SQL = SQL + ComNum.VBLF + "    , VALUE                                                                          ";
                SQL = SQL + ComNum.VBLF + "    , UNIT                                                                           ";
                SQL = SQL + ComNum.VBLF + "    , ROWINDEX                                                                       ";
                SQL = SQL + ComNum.VBLF + "    , (COLINDEX + 11) AS COLINDEX                                                    ";
                SQL = SQL + ComNum.VBLF + "    , COLINDEX AS COLINDEXOLD                                                        ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.AEMRANCHARTGRAPE                                                    ";
                SQL = SQL + ComNum.VBLF + "WHERE ITEMCODE NOT IN('I0000002018', 'I0000001765', 'I0000001178')    ";
                SQL = SQL + ComNum.VBLF + "   AND COLINDEX >= " + (((27 * page) - 27) + 2);
                SQL = SQL + ComNum.VBLF + "   AND COLINDEX < " + ((27 * page) + 2);
                SQL = SQL + ComNum.VBLF + "   AND EMRNO    = " + mstrEmrNo    ;
                SQL = SQL + ComNum.VBLF + "   AND EMRNOHIS = " + mstrEmrHisNo;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                foreach (DataRow row in dt.Rows)
                {
                    if (row["ITEMCODE"].ToString() == "I0000000574")
                    {
                        ssPrint.ActiveSheet.Cells[46, (int)VB.Val(row["COLINDEX"].ToString()) - (27 * (page - 1))].Text = row["UNIT"].ToString();
                    }
                    else
                    {
                        ssPrint.ActiveSheet.Cells[(int)VB.Val(row["ROWINDEX"].ToString()), (int)VB.Val(row["COLINDEX"].ToString()) - (27 * (page - 1))].Text = row["VALUE"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
            #endregion

            #region 그래프 설정

            //  그래프 설정
            XYPointSeries xyPointSeries = new XYPointSeries();
            PrintChart = new SpreadChart(xyPointSeries, "SpreadChart1");
            ssPrint.ActiveSheet.DrawingContainer.ContainedObjects.Add(PrintChart);

            PrintChart.IgnoreUpdateShapeLocation = false;
            PrintChart.IsGrayscale = false;



            //LegendArea legendArea1 = new LegendArea();
            //spreadChart.Model.LegendAreas.AddRange(new LegendArea[] { legendArea1 });

            YPlotArea = new YPlotArea
            {
                Location = new PointF(0.07F, 0.01F),
                Size = new SizeF(0.93F, 0.98F)

                //Location = new PointF(0F, 0F),
                ////Size = new SizeF(0.86F, 0.8F)
                //Size = new SizeF(1F, 0.99F)                
            };

            PrintChart.Model.PlotAreas.AddRange(new PlotArea[] { YPlotArea });

            string firstAlphabet = GetExcelColumnName(((27 * page) - 27) + 3);
            //string lastAlphabet = GetExcelColumnName(page < TotalPage ? (27 * page) + 2 : ssView.ActiveSheet.Columns.Count);
            string lastAlphabet = GetExcelColumnName((27 * page) + 2);

            SBPSeries = new PointSeries
            {
                SeriesName = "SBP",
                LabelVisible = true,
                PointBorder = new NoLine(),
                PointFill = Properties.Resources.Chart_SBP
            };
            SBPSeries.Values.DataSource = new SeriesDataField(
                ssView,
                "SBP",
                string.Concat("Sheet1!$", firstAlphabet, "$101:$", lastAlphabet, "$101"),
                SegmentDataType.AutoIndex,
                new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps }
            );

            DBPSeries = new PointSeries
            {
                SeriesName = "DBP",
                LabelVisible = true,
                PointBorder = new NoLine(),
                PointFill = Properties.Resources.Chart_DBP
            };
            DBPSeries.Values.DataSource = new SeriesDataField(
                ssView,
                "DBP",
                string.Concat("Sheet1!$", firstAlphabet, "$102:$", lastAlphabet, "$102"),
                SegmentDataType.AutoIndex,
                new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps }
            );

            PulseSeries = new PointSeries
            {
                SeriesName = "맥박",
                LabelVisible = true,
                PointBorder = new NoLine(),
                PointFill = Properties.Resources.Chart_Pulse
            };
            PulseSeries.Values.DataSource = new SeriesDataField(
                ssView,
                "맥박",
                string.Concat("Sheet1!$", firstAlphabet, "$103:$", lastAlphabet, "$103"),
                SegmentDataType.AutoIndex,
                new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps }
            );

            BreathSeries = new PointSeries
            {
                SeriesName = "호흡",
                LabelVisible = true,
                PointBorder = new NoLine(),
                PointFill = Properties.Resources.Chart_Breath
            };
            BreathSeries.Values.DataSource = new SeriesDataField(
                ssView,
                "호흡",
                string.Concat("Sheet1!$", firstAlphabet, "$104:$", lastAlphabet, "$104"),
                SegmentDataType.AutoIndex,
                new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps }
            );

            YPlotArea.Series.AddRange(new Series[] {
                    SBPSeries,
                    DBPSeries,
                    PulseSeries,
                    BreathSeries,
                });

            YPlotArea.YAxes.Clear();
            IndexAxis indexAxis1 = new IndexAxis { LabelTextDirection = TextDirection.Rotate90Degree };
            ValueAxis valueAxis1 = new ValueAxis();
            valueAxis1.LabelTextFont = new Font("굴림", 9, FontStyle.Regular);
            YPlotArea.XAxis = indexAxis1;
            YPlotArea.YAxes.Clear();
            YPlotArea.YAxes.AddRange(new ValueAxis[] { valueAxis1 });
            //int y = (ViewItemCount * 18) + 50;
            PrintChart.Rectangle = new Rectangle(245, 520, 775, 710);
            PrintChart.SheetName = "fpSpread1_Sheet1";
            
            SetChartPointMarkers(0);
            #endregion

        }
        #endregion


        public static void SetDrSign(FarPoint.Win.Spread.FpSpread spd, int row, int Col, string sabun)
        {
            Image ImageX = GetDrSign(clsDB.DbCon, sabun, "");
            FarPoint.Win.Spread.CellType.TextCellType cellType = new FarPoint.Win.Spread.CellType.TextCellType();
            cellType.BackgroundImage = new FarPoint.Win.Picture(ImageX, FarPoint.Win.RenderStyle.Stretch);
            spd.ActiveSheet.Cells[row, Col].CellType = cellType;

            ImageX = null;
            cellType = null;
        }

        public static Image GetDrSign(PsmhDb pDbCon, string strSabun, string strgubun)
        {
            Image rtnVAL = null;

            if (string.IsNullOrEmpty(strSabun)) return rtnVAL;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "SIGNATURE ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_DOCTOR ";
                if (strgubun == "1")
                {
                    SQL = SQL + ComNum.VBLF + "WHERE TRIM(drcode) = '" + strSabun + "'";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE TRIM(SABUN) = '" + strSabun + "'";
                }


                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return rtnVAL;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVAL;
                }

                if (dt.Rows[0]["SIGNATURE"] == DBNull.Value)
                {
                    ComFunc.MsgBox("현재 의사는 서명이 없습니다 확인해주세요.");
                    return rtnVAL;
                }

                using (MemoryStream memStream = new MemoryStream((byte[])dt.Rows[0]["SIGNATURE"]))
                {
                    rtnVAL = Image.FromStream(memStream);
                }

                return rtnVAL;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVAL;
            }
        }

        private string ReadAnesName(Panel obj)
        {
            string rtnVal = "";
            Control[] controls = null;            
            controls = ComFunc.GetAllControls(obj);
            foreach (Control objControl in controls)
            {
                if (objControl is Label)
                {
                    rtnVal += " " + ((Label)objControl).Text + ",";
                }                
            }

            return VB.Left(rtnVal, rtnVal.Length - 1);
        }

        private void ssView_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true) return;

            if (e.Column > 1)
            {
                string hh = ssView.ActiveSheet.Cells[0, e.Column].Text;
                string mm = ssView.ActiveSheet.Cells[1, e.Column].Text;

                cboTime.SelectedIndex = cboTime.FindStringExact(string.Concat(hh, ":", mm));

                if (e.Row == 0 || e.Row == 1)
                {

                }


                if (e.Button == MouseButtons.Right)
                {
                    CellRange cr = ssView.ActiveSheet.GetSelection(0);
                    string strItem = ssView.ActiveSheet.Cells[cr.Row, 0].Text;

                    if (string.IsNullOrEmpty(strItem)) return;

                    if (BasicMedcationInfo.FindIndex(r => r.Name.Equals(strItem)) >= 0)
                    {
                        return;
                    }
                    
                    string strValue = VB.InputBox("투약용량 입력", "입력");

                    if (!string.IsNullOrEmpty(strValue))
                    {
                        for (int c = 0; c < cr.ColumnCount; c++)
                        {
                            if (c == 0)
                            {
                                ssView.ActiveSheet.Cells[cr.Row, cr.Column + c].Text = strValue;
                            }
                            else
                            {
                                ssView.ActiveSheet.Cells[cr.Row, cr.Column + c].Text = ValueEquals;
                            }                                
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 프로그램 로드시 아이템항목 세팅
        /// </summary>
        private void InitAnItem()
        {
            frmEmrAnItem frmEmrAnItem = new frmEmrAnItem(Medcations, chkOpStart.Checked);
            frmEmrAnItem.SetInit();
            frmEmrAnItem.IsSave = true;

            //  저장버튼 체크
            if (!frmEmrAnItem.IsSave)
            {
                return;
            }

            Medcations = new List<MedcationInfo>();
            for (int i = 0; i < frmEmrAnItem.ssItemList.ActiveSheet.RowCount; i++)
            {
                string itemNo = frmEmrAnItem.ssItemList.ActiveSheet.Cells[i, 3].Text;
                if (string.IsNullOrWhiteSpace(itemNo))
                {
                    continue;
                }

                string itemName = frmEmrAnItem.ssItemList.ActiveSheet.Cells[i, 1].Text;
                string itemUnit = frmEmrAnItem.ssItemList.ActiveSheet.Cells[i, 2].Text;
                int row = Convert.ToInt32(frmEmrAnItem.ssItemList.ActiveSheet.Cells[i, 4].Value.ToString());
                bool isWrite = Convert.ToBoolean(frmEmrAnItem.ssItemList.ActiveSheet.Cells[i, 5].Text);
                bool isView = Convert.ToBoolean(frmEmrAnItem.ssItemList.ActiveSheet.Cells[i, 6].Text);

                Medcations.Add(new MedcationInfo
                {
                    Name = itemName,
                    Uint = itemUnit,
                    Row = row == -1 ? i + 2 : row,
                    IsWrite = isWrite,
                    IsView = isView,
                    ItemNo = itemNo
                });
            }

            //  Write Spread 항목설정            
            ListBox list1 = new ListBox();
            list1.Items.Add("SR");
            list1.Items.Add("CR");
            list1.Items.Add("AR");

            Type1.Clear();            
            Type1.ListControl = list1;
            Type1.Editable = false;

            List<MedcationInfo> list = Medcations.ToList().FindAll(r => r.IsWrite);
            ssWrite.ActiveSheet.Rows.Count = list.Count;
            for (int i = 0; i < list.Count; i++)
            {
                ssWrite.ActiveSheet.Rows[i].Tag = list[i];
                ssWrite.ActiveSheet.Cells[i, 0].Text = list[i].Name;

                // 호흡                    
                if (list[i].ItemNo == "I0000000574")
                {
                    //ssWrite.ActiveSheet.Cells[i, 2].CellType = Type1;
                    //ssWrite.ActiveSheet.Cells[i, 2].Text = "SR";
                    //ssWrite.ActiveSheet.Cells[i, 2].Locked = false;

                    //2020-05-12 마취과 이송이 선생님 고정값으로 변경요청
                    ssWrite.ActiveSheet.Cells[i, 2].Text = "RR";                    
                }
                else
                {
                    ssWrite.ActiveSheet.Cells[i, 2].Text = list[i].Uint;
                }

                // Tourniquest
                if (list[i].ItemNo == "I0000030758")
                {
                    ssWrite.ActiveSheet.Cells[i, 1].Locked = true;
                    ssWrite.ActiveSheet.Cells[i, 2].Locked = true;
                }

                if (list[i].Name == "SBP" || list[i].Name == "DBP" || list[i].Name == "맥박" || list[i].Name == "호흡")
                {
                    ssWrite.ActiveSheet.Cells[i, 3].CellType = null;                    
                }
                else
                {
                    ssWrite.ActiveSheet.Cells[i, 3].CellType = Type2;
                }
            }
        }

        private void btnAddIntake_Click(object sender, EventArgs e)
        {
            if (ssIn.ActiveSheet.RowCount == 19)
            {
                ComFunc.MsgBox("최대 입력 수량은 18개 입니다.");
                return;
            }

            ssIn.ActiveSheet.Rows.Add(ssIn.ActiveSheet.RowCount - 1, 1);

            SetFormulaIntake();            
        }

        private void btnAddOutput_Click(object sender, EventArgs e)
        {
            if (ssOut.ActiveSheet.RowCount == 15)
            {
                ComFunc.MsgBox("최대 입력 수량은 14개 입니다.");
                return;
            }

            ssOut.ActiveSheet.Rows.Add(ssOut.ActiveSheet.RowCount - 1, 1);

            SetFormulaOutput();
        }

        private void ssWrite_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true) return;

            if (ssWrite.ActiveSheet.Cells[e.Row, 0].Text == "Tourniquet" && e.Column > 0)
            {
                if (frmAnFormGrapeTurniquestX != null)
                {
                    frmAnFormGrapeTurniquestX.Dispose();
                    frmAnFormGrapeTurniquestX = null;
                }

                if (ssWrite.ActiveSheet.Cells[e.Row, 1].Text == "")
                {
                    frmAnFormGrapeTurniquestX = new frmAnFormGrapeTurniquest();
                }
                else
                {
                    string strTag = ssWrite.ActiveSheet.Cells[e.Row, 2].Tag.ToString();
                    frmAnFormGrapeTurniquestX = new frmAnFormGrapeTurniquest(strTag);
                }
                frmAnFormGrapeTurniquestX.rGetInfo += frmAnFormGrapeTurniquestX_rGetInfo;
                frmAnFormGrapeTurniquestX.rEventClosed += frmAnFormGrapeTurniquestX_rEventClosed;
                frmAnFormGrapeTurniquestX.ShowDialog();
            }
        }

        private void frmAnFormGrapeTurniquestX_rEventClosed()
        {
            if (frmAnFormGrapeTurniquestX != null)
            {
                frmAnFormGrapeTurniquestX.Dispose();
                frmAnFormGrapeTurniquestX = null;
            }
        }

        private void frmAnFormGrapeTurniquestX_rGetInfo(string Info)
        {
            if (frmAnFormGrapeTurniquestX != null)
            {
                frmAnFormGrapeTurniquestX.Dispose();
                frmAnFormGrapeTurniquestX = null;
            }

            bool bolUp = false;
            bool bolDown = false;

            if (Info.IndexOf("Up") != -1) bolUp = true;
            if (Info.IndexOf("Down") != -1) bolDown = true;


            if (bolUp == true && bolDown == true)
            {
                ssWrite.ActiveSheet.Cells[ssWrite.ActiveSheet.ActiveRowIndex, 1].Text = "↑↓";
                ssWrite.ActiveSheet.Cells[ssWrite.ActiveSheet.ActiveRowIndex, 2].Tag = Info;
            }
            else if (bolUp == true && bolDown == false)
            {
                ssWrite.ActiveSheet.Cells[ssWrite.ActiveSheet.ActiveRowIndex, 1].Text = "↑";
                ssWrite.ActiveSheet.Cells[ssWrite.ActiveSheet.ActiveRowIndex, 2].Tag = Info;
            }
            else if (bolUp == false && bolDown == true)
            {
                ssWrite.ActiveSheet.Cells[ssWrite.ActiveSheet.ActiveRowIndex, 1].Text = "↓";
                ssWrite.ActiveSheet.Cells[ssWrite.ActiveSheet.ActiveRowIndex, 2].Tag = Info;
            }
            else
            {
                ssWrite.ActiveSheet.Cells[ssWrite.ActiveSheet.ActiveRowIndex, 1].Text = "";
                ssWrite.ActiveSheet.Cells[ssWrite.ActiveSheet.ActiveRowIndex, 2].Tag = null;
            }
            
        }

        private void ssView_TextTipFetch(object sender, TextTipFetchEventArgs e)
        {
            if (e.ColumnHeader == true || e.RowHeader == true) return;

            e.ShowTip = false;
            if (ssView.ActiveSheet.Cells[e.Row, 0].Text == "Tourniquet" && e.Column > 1)
            {
                if (ssView.ActiveSheet.Cells[e.Row, e.Column].Text != "")
                {
                    string strTag = ssView.ActiveSheet.Cells[e.Row, e.Column].Tag.ToString();
                    string strToolTip = "";
                    string[] split = strTag.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in split)
                    {
                        string[] item = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                        if (item.Length > 0)
                        {
                            strToolTip += item[0] + "/" + item[1] + "/" + item[2] + "\r\n";
                        }
                    }

                    e.TipText = strToolTip;
                    e.ShowTip = true;
                }
            }
        }

        private void IOSpread_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //CurrentFpSpread = (sender as FpSpread);
            //int rowIndex = CurrentFpSpread.ActiveSheet.ActiveRowIndex;

            //if (rowIndex < 0)
            //{
            //    return;
            //}

            //if (rowIndex == CurrentFpSpread.ActiveSheet.RowCount - 1)
            //{
            //    return;
            //}

            //CurrentFpSpread.ActiveSheet.RemoveRows(rowIndex, 1);
            //CurrentFpSpread.ActiveSheet.AddRows(CurrentFpSpread.ActiveSheet.RowCount, 1);
        }

        private void btnSpdMacro_M_Click(object sender, EventArgs e)
        {
            //약품
            if (frmAnFormSpdMacroX != null)
            {
                frmAnFormSpdMacroX.Dispose();
                frmAnFormSpdMacroX = null;
            }

            frmAnFormSpdMacroX = new frmAnFormSpdMacro(0);
            frmAnFormSpdMacroX.rGetInfo += frmAnFormSpdMacroX_rGetInfo0;
            //frmAnFormSpdMacroX.rEventClosed += frmAnFormSpdMacroX_rEventClosed;
            frmAnFormSpdMacroX.StartPosition = FormStartPosition.CenterParent;
            frmAnFormSpdMacroX.ShowDialog();
        }

        private void btnSpdMacro_G_Click(object sender, EventArgs e)
        {
            //물품
            if (frmAnFormSpdMacroX != null)
            {
                frmAnFormSpdMacroX.Dispose();
                frmAnFormSpdMacroX = null;
            }

            frmAnFormSpdMacroX = new frmAnFormSpdMacro(1);
            frmAnFormSpdMacroX.rGetInfo += frmAnFormSpdMacroX_rGetInfo1;
            //frmAnFormSpdMacroX.rEventClosed += frmAnFormSpdMacroX_rEventClosed;
            frmAnFormSpdMacroX.StartPosition = FormStartPosition.CenterParent;
            frmAnFormSpdMacroX.ShowDialog();
        }

        private void btnSpdMacro_P_Click(object sender, EventArgs e)
        {
            //PCA
            if (frmAnFormSpdMacroX != null)
            {
                frmAnFormSpdMacroX.Dispose();
                frmAnFormSpdMacroX = null;
            }

            frmAnFormSpdMacroX = new frmAnFormSpdMacro(2);
            frmAnFormSpdMacroX.rGetInfo += frmAnFormSpdMacroX_rGetInfo2;
            //frmAnFormSpdMacroX.rEventClosed += frmAnFormSpdMacroX_rEventClosed;
            frmAnFormSpdMacroX.StartPosition = FormStartPosition.CenterParent;
            frmAnFormSpdMacroX.ShowDialog();
        }

        private void btnSpdMacro_R_Click(object sender, EventArgs e)
        {
            //회복실
            if (frmAnFormSpdMacroX != null)
            {
                frmAnFormSpdMacroX.Dispose();
                frmAnFormSpdMacroX = null;
            }

            frmAnFormSpdMacroX = new frmAnFormSpdMacro(3);
            frmAnFormSpdMacroX.rGetInfo += frmAnFormSpdMacroX_rGetInfo3;
            //frmAnFormSpdMacroX.rEventClosed += frmAnFormSpdMacroX_rEventClosed;
            frmAnFormSpdMacroX.StartPosition = FormStartPosition.CenterParent;
            frmAnFormSpdMacroX.ShowDialog();
        }

        private void frmAnFormSpdMacroX_rEventClosed()
        {
            if (frmAnFormSpdMacroX != null)
            {
                frmAnFormSpdMacroX.Dispose();
                frmAnFormSpdMacroX = null;
            }
        }

        private void frmAnFormSpdMacroX_rGetInfo0(string Name, string Unit)
        {
            //if (frmAnFormSpdMacroX != null)
            //{
            //    frmAnFormSpdMacroX.Dispose();
            //    frmAnFormSpdMacroX = null;
            //}
            int rowIndex = ssMedication.ActiveSheet.GetLastNonEmptyRow(NonEmptyItemFlag.Data) + 1;

            ssMedication.ActiveSheet.Cells[rowIndex, 0].Text = Name;
            ssMedication.ActiveSheet.Cells[rowIndex, 2].Text = Unit;
        }

        private void frmAnFormSpdMacroX_rGetInfo1(string Name, string Unit)
        {
            //if (frmAnFormSpdMacroX != null)
            //{
            //    frmAnFormSpdMacroX.Dispose();
            //    frmAnFormSpdMacroX = null;
            //}
            int rowIndex = ssJep.ActiveSheet.GetLastNonEmptyRow(NonEmptyItemFlag.Data) + 1;

            ssJep.ActiveSheet.Cells[rowIndex, 0].Text = Name;
            ssJep.ActiveSheet.Cells[rowIndex, 1].Text = "1";
            ssJep.ActiveSheet.Cells[rowIndex, 2].Text = Unit;
        }

        private void frmAnFormSpdMacroX_rGetInfo2(string Name, string Unit)
        {
            //if (frmAnFormSpdMacroX != null)
            //{
            //    frmAnFormSpdMacroX.Dispose();
            //    frmAnFormSpdMacroX = null;
            //}
            int rowIndex = ssPCA.ActiveSheet.GetLastNonEmptyRow(NonEmptyItemFlag.Data) + 1;

            ssPCA.ActiveSheet.Cells[rowIndex, 0].Text = Name;
            ssPCA.ActiveSheet.Cells[rowIndex, 2].Text = Unit;
        }

        private void frmAnFormSpdMacroX_rGetInfo3(string Name, string Unit)
        {
            //if (frmAnFormSpdMacroX != null)
            //{
            //    frmAnFormSpdMacroX.Dispose();
            //    frmAnFormSpdMacroX = null;
            //}
            int rowIndex = ssRecovery.ActiveSheet.GetLastNonEmptyRow(NonEmptyItemFlag.Data) + 1;

            ssRecovery.ActiveSheet.Cells[rowIndex, 0].Text = Name;
            ssRecovery.ActiveSheet.Cells[rowIndex, 2].Text = Unit;
        }

        private void btnPreDx_Click(object sender, EventArgs e)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (txtWRTNO.Text == "") return;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT                                ";
                SQL = SQL + ComNum.VBLF + "     PREDIAGNOSIS AS PreOpDx          ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_OCS.OCS_OPSCHE           ";                
                SQL = SQL + ComNum.VBLF + "WHERE WRTNO = '" + txtWRTNO.Text + "' ";
                
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    txtPreopDx.Text = dt.Rows[0]["PreOpDx"].ToString();                    
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPostDx_Click(object sender, EventArgs e)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (txtWRTNO.Text == "") return;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT                                ";
                SQL = SQL + ComNum.VBLF + "    DIAGNOSIS AS PostOpDx            ";                
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.ORAN_MASTER         ";                
                SQL = SQL + ComNum.VBLF + "WHERE WRTNO = '" + txtWRTNO.Text + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    txtPostopDx.Text = dt.Rows[0]["PostOpDx"].ToString();
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnOperation_Click(object sender, EventArgs e)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (txtWRTNO.Text == "") return;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT                                   ";                
                SQL = SQL + ComNum.VBLF + "    OPTITLE AS PostOpTitle              ";                
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.ORAN_MASTER            ";                
                SQL = SQL + ComNum.VBLF + "WHERE WRTNO = '" + txtWRTNO.Text + "'    ";                

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {                    
                    txtOperation.Text = dt.Rows[0]["PostOpTitle"].ToString();
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void dtpOpDate_ValueChanged(object sender, EventArgs e)
        {
            if (mstrEmrNo == "0")
            {
                dtpNow.Value = dtpOpDateFr.Value;
            }
        }

        private void btnTopVisible_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text == "▲")
            {
                panTop.Visible = false;
                ((Button)sender).Text = "▼";
            }
            else
            {
                panTop.Visible = true;
                ((Button)sender).Text = "▲";
            }
        }

        private void RemoveLabelControl(Panel panel)
        {
            Control[] controls = null;
            controls = ComFunc.GetAllControls(panel);

            foreach (Control objControl in controls)
            {
                if (objControl is Label)
                {
                    ((Label)objControl).Parent.Controls.Remove(((Label)objControl));
                }
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {            
            chkAnesthesia.Checked = false;
            chkOp.Checked = false;
            chkIntubation.Checked = false;

            for (int i = 0; i < ssWrite.ActiveSheet.RowCount; i++)
            {
                ssWrite.ActiveSheet.Cells[i, 1].Text = "";

                if (ssWrite.ActiveSheet.Cells[i, 0].Text == "Tourniquet")
                {
                    ssWrite.ActiveSheet.Cells[i, 2].Tag = null;
                }

                if (ssWrite.ActiveSheet.Cells[i, 3].CellType == Type2)
                {
                    ssWrite.ActiveSheet.Cells[i, 3].Value = false;
                }
            }            
        }

        private void SetFormulaIntake()
        {
            int row = ssIn.ActiveSheet.RowCount - 1;
            ssIn.ActiveSheet.Cells[row, 1].Formula = "SUM(B1:B" + Convert.ToString(row) + ")";
            ssIn.ActiveSheet.Cells[row, 2].Formula = "SUM(C1:C" + Convert.ToString(row) + ")";
        }

        private void SetFormulaOutput()
        {
            int row = ssOut.ActiveSheet.RowCount - 1;
            ssOut.ActiveSheet.Cells[row, 1].Formula = "SUM(B1:B" + Convert.ToString(row) + ")";
        }

        private void btnAllChk_Click(object sender, EventArgs e)
        {
            if (btnAllChk.Text == "Chk")
            {
                chkAnesthesia.Checked = true;
                chkOp.Checked = true;
                chkIntubation.Checked = true;

                for (int i = 0; i < ssWrite.ActiveSheet.RowCount; i++)
                {
                    if (ssWrite.ActiveSheet.Cells[i, 3].CellType == Type2)
                    {
                        ssWrite.ActiveSheet.Cells[i, 3].Value = true;
                    }
                }

                btnAllChk.Text = "UnChk";
            }
            else
            {
                chkAnesthesia.Checked = false;
                chkOp.Checked = false;
                chkIntubation.Checked = false;

                for (int i = 0; i < ssWrite.ActiveSheet.RowCount; i++)
                {
                    if (ssWrite.ActiveSheet.Cells[i, 3].CellType == Type2)
                    {
                        ssWrite.ActiveSheet.Cells[i, 3].Value = false;
                    }
                }

                btnAllChk.Text = "Chk";
            }
        }

        private void btnSpdSort_M_Click(object sender, EventArgs e)
        {
            SpdSort(ssMedication, ssMedication.ActiveSheet.RowCount);
        }

        private void btnSpdSort_G_Click(object sender, EventArgs e)
        {
            SpdSort(ssJep, ssJep.ActiveSheet.RowCount);
        }

        private void btnSpdSort_P_Click(object sender, EventArgs e)
        {
            SpdSort(ssPCA, ssPCA.ActiveSheet.RowCount);
        }

        private void btnSpdSort_R_Click(object sender, EventArgs e)
        {
            SpdSort(ssRecovery, ssRecovery.ActiveSheet.RowCount);
        }

        private void SpdSort(FpSpread CurrentFpSpread, int rowCount)
        {
            for (int i = CurrentFpSpread.ActiveSheet.RowCount - 1; i >= 0; i--)
            {
                if (string.IsNullOrEmpty(CurrentFpSpread.ActiveSheet.Cells[i, 0].Text))
                {
                    CurrentFpSpread.ActiveSheet.Rows[i].Remove();
                }
            }

            CurrentFpSpread.ActiveSheet.RowCount = rowCount;
        }

        private void ssView_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader == true || e.ColumnHeader == true) return;

            if (e.Column > 1)
            {
                string hh = ssView.ActiveSheet.Cells[0, e.Column].Text;
                string mm = ssView.ActiveSheet.Cells[1, e.Column].Text;

                cboTime.SelectedIndex = cboTime.FindStringExact(string.Concat(hh, ":", mm));

                //if (e.Row == 0 || e.Row == 1)
                if (e.Row >= 2)
                {
                    LoadPreTimeEmr(e.Column);
                }
            }
        }

        private void LoadPreTimeEmr(int column)
        {
            string Item = string.Empty;
            string Value = string.Empty;

            btnClearAll.PerformClick();

            for (int i = 2; i < 50; i++)
            {
                Item = ssView.ActiveSheet.Cells[i, 0].Text;
                Value = ssView.ActiveSheet.Cells[i, column].Text;

                switch (Item)
                {
                    case "마취":
                        if (!string.IsNullOrEmpty(Value))
                        {
                            chkAnesthesia.Checked = true;
                        }
                        else
                        {
                            chkAnesthesia.Checked = false;
                        }
                        break;
                    case "수술":
                        if (!string.IsNullOrEmpty(Value))
                        {
                            chkOp.Checked = true;
                        }
                        else
                        {
                            chkOp.Checked = false;
                        }
                        break;
                    case "삽관":
                        if (!string.IsNullOrEmpty(Value))
                        {
                            chkIntubation.Checked = true;
                        }
                        else
                        {
                            chkIntubation.Checked = false;
                        }
                        break;
                    default:
                        LoadPreTimeEmrSpd(i, column, Item, Value);
                        break;
                }
            }

            LoadPreTimeEmrSpd(100, column, "SBP", ssView.ActiveSheet.Cells[100, column].Text);
            LoadPreTimeEmrSpd(101, column, "DBP", ssView.ActiveSheet.Cells[101, column].Text);
            LoadPreTimeEmrSpd(102, column, "맥박", ssView.ActiveSheet.Cells[102, column].Text);
            LoadPreTimeEmrSpd(103, column, "호흡", ssView.ActiveSheet.Cells[103, column].Text);
        }

        private void LoadPreTimeEmrSpd(int row, int col, string Item, string Value)
        {
            int rowIndex = 0;
            string display = "";

            for (int i = 0; i < ssWrite.ActiveSheet.RowCount; i++)
            {
                if (ssWrite.ActiveSheet.Cells[i, 0].Text.Trim() == Item)
                {
                    rowIndex = i;
                    break;
                }
            }

            if (Item == "SBP" || Item == "DBP" || Item == "맥박" || Item == "호흡")
            {
                ssWrite.ActiveSheet.Cells[rowIndex, 1].Text = Value;

                if (Item == "호흡")
                {
                    //ssWrite.ActiveSheet.Cells[rowIndex, 2].Text = ssView.ActiveSheet.Cells[row + 1, col].Text;
                    
                    //2020-05-12 마취과 이송이 선생님 고정값으로 변경요청 
                    ssWrite.ActiveSheet.Cells[rowIndex, 2].Text = "RR";
                }
            }
            else if (Item == "Tourniquet")
            {
                ssWrite.ActiveSheet.Cells[rowIndex, 1].Text = Value;
                ssWrite.ActiveSheet.Cells[rowIndex, 2].Tag = ssView.ActiveSheet.Cells[row, col].Tag;
            }
            else if (Item == "EKG monitoring" || Item == "Warm blanket")
            {
                if (Value.Equals(ValueEquals))
                {
                    ssWrite.ActiveSheet.Cells[rowIndex, 1].Text = "1";
                }       
                else if (Value.Equals("─/"))
                {
                    ssWrite.ActiveSheet.Cells[rowIndex, 1].Text = "1";
                    ssWrite.ActiveSheet.Cells[rowIndex, 3].Value = true;
                }
            }
            else
            {
                if (!Value.Equals(ValueEquals) && !Value.Equals("─/"))
                {
                    ssWrite.ActiveSheet.Cells[rowIndex, 1].Text = Value;
                }
                else
                {
                    for (int j = col - 1; j >= 0; j--)
                    {
                        display = ssView.ActiveSheet.Cells[row, j].Text;
                        if (!display.Equals(ValueEquals))
                        {
                            ssWrite.ActiveSheet.Cells[rowIndex, 1].Text = display;
                            break;
                        }
                    }

                    if (Value.Equals("─/"))
                    {
                        ssWrite.ActiveSheet.Cells[rowIndex, 3].Value = true;
                    }
                }
            }
        }

        public void Spread_Button_Down(FarPoint.Win.Spread.FpSpread SpdNm)
        {            
            int intActRow = (int)SpdNm.ActiveSheet.ActiveRowIndex;

            if (intActRow != (int)SpdNm.ActiveSheet.RowCount - 1)
            {
                if (intActRow >= (int)SpdNm.ActiveSheet.RowCount - 1) return;
                SpdNm.ActiveSheet.MoveRow(intActRow, intActRow + 1, true);
                SpdNm.ActiveSheet.ActiveRowIndex = SpdNm.ActiveSheet.ActiveRowIndex + 1;
                SpdNm.ActiveSheet.SetActiveCell(SpdNm.ActiveSheet.ActiveRowIndex, 0);
                SpdNm.ShowRow(0, SpdNm.ActiveSheet.ActiveRowIndex, FarPoint.Win.Spread.VerticalPosition.Nearest);
            }            
        }

        public void Spread_Button_Up(FarPoint.Win.Spread.FpSpread SpdNm)
        {            
            int intActRow = (int)SpdNm.ActiveSheet.ActiveRowIndex;

            if (intActRow != 0)
            {
                if (intActRow <= 0) return;
                SpdNm.ActiveSheet.MoveRow(intActRow, intActRow - 1, true);
                SpdNm.ActiveSheet.ActiveRowIndex = SpdNm.ActiveSheet.ActiveRowIndex - 1;
                SpdNm.ActiveSheet.SetActiveCell(SpdNm.ActiveSheet.ActiveRowIndex, 0);
                SpdNm.ShowRow(0, SpdNm.ActiveSheet.ActiveRowIndex, FarPoint.Win.Spread.VerticalPosition.Nearest);
            }                        
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            string SpdName = ((Button)sender).Tag.ToString();

            switch (SpdName)
            {
                case "ssMedication":
                    Spread_Button_Up(ssMedication);
                    break;
                case "ssJep":
                    Spread_Button_Up(ssJep);
                    break;
                case "ssPCA":
                    Spread_Button_Up(ssPCA);
                    break;
                case "ssRecovery":
                    Spread_Button_Up(ssRecovery);
                    break;
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            string SpdName = ((Button)sender).Tag.ToString();

            switch (SpdName)
            {
                case "ssMedication":
                    Spread_Button_Down(ssMedication);
                    break;
                case "ssJep":
                    Spread_Button_Down(ssJep);
                    break;
                case "ssPCA":
                    Spread_Button_Down(ssPCA);
                    break;
                case "ssRecovery":
                    Spread_Button_Down(ssRecovery);
                    break;
            }
        }

        private void btnExam_Click(object sender, EventArgs e)
        {
            if (frmAnFormExamX != null)
            {
                frmAnFormExamX.Dispose();
                frmAnFormExamX = null;
            }
            frmAnFormExamX = new frmAnFormExam(AcpEmr.ptNo, txtHb.Text, txtHct.Text, txtPLT.Text, txtWBC.Text, txtNa.Text, txtK.Text, txtOT.Text, txtPT.Text, txtPT2.Text, txtPTT.Text);
            frmAnFormExamX.rGetPatientInfo += frmAnFormExamX_rGetPatientInfo;
            frmAnFormExamX.rEventClosed += frmAnFormExamX_rEventClosed;
            frmAnFormExamX.ShowDialog();
        }

        private void frmAnFormExamX_rEventClosed()
        {
            if (frmAnFormExamX != null)
            {
                frmAnFormExamX.Dispose();
                frmAnFormExamX = null;
            }
        }

        private void frmAnFormExamX_rGetPatientInfo(string Hb, string Hct, string Plt, string Wbc, string Na, string K, string GOT, string GPT, string PT, string PTT)
        {
            if (frmAnFormExamX != null)
            {
                frmAnFormExamX.Dispose();
                frmAnFormExamX = null;
            }

            txtHb.Text = Hb;
            txtHct.Text = Hct;
            txtNa.Text = Na;
            txtK.Text = K;
            txtOT.Text = GOT;
            txtPT.Text = GPT;
            txtPT2.Text = PT;
            txtPTT.Text = PTT;
            txtPLT.Text = Plt;
            txtWBC.Text = Wbc;
        }

        private bool CheckSaveException()
        {
            bool rtnVal = true;
            string strMsg = "";
            strMsg = strMsg + ComNum.VBLF + "-------------------";
            strMsg = strMsg + ComNum.VBLF + " ※ 항목누락 체크  ";
            strMsg = strMsg + ComNum.VBLF + "-------------------";

            // 수술시작 전 차트저장시 막음.
            if (chkOpStart.Checked == false)
            {
                ComFunc.MsgBox("차트저장은 수술시작 이후부터 가능합니다.");
                return false;
            }

            // ASA 점수 Validation
            if (rdoASA_1.Checked == false && rdoASA_2.Checked == false && rdoASA_3.Checked == false &&
                rdoASA_4.Checked == false && rdoASA_5.Checked == false && rdoASA_6.Checked == false)
            {
                strMsg = strMsg + ComNum.VBLF + "  ASA점수 누락";
                rtnVal = false;
            }

            // NPO Validation            
            if (rdoPreNPO_Y.Checked == false && rdoPreNPO_N.Checked == false)
            {
                strMsg = strMsg + ComNum.VBLF + "  NPO 누락";
                rtnVal = false;
            }

            // PCA Validation
            if (ssPCA.ActiveSheet.NonEmptyRowCount > 0)
            {
                if (rdoPCAIV.Checked == false && rdoPCAEP.Checked == false)
                {
                    strMsg = strMsg + ComNum.VBLF + "  PCA 누락";
                    rtnVal = false;
                }
            }

            if (rtnVal == false)
            {
                ComFunc.MsgBox(strMsg, "오류");
            }
            return rtnVal;
        }

        private void ssOut_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {                
                ssOutMacro.Location = new Point(tableLayoutPanel17.Width - 100, tableLayoutPanel17.Location.Y + 80 + e.Y);
                ssOutMacro.Visible = true;
                ssOutMacro.ActiveSheet.SetActiveCell(0, 0);
                ssOutMacro.Focus();
            }
        }

        private void ssOutMacro_Leave(object sender, EventArgs e)
        {
            ssOutMacro.Visible = false;
        }

        private void ssOutMacro_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            int row = ssOut.ActiveSheet.ActiveRowIndex;
            int col = ssOut.ActiveSheet.ActiveColumnIndex;

            ssOut.ActiveSheet.Cells[row, 1].Text = ssOutMacro.ActiveSheet.Cells[e.Row, 0].Text;
            ssOutMacro.Visible = false;
        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void ChkGeneralAnes_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkGeneralAnes.Checked == true)
            {
                PanGeneralAnes.Visible = true;
            }
            else
            {
                PanGeneralAnes.Visible = false;
            }
        }

        private void ChkSiteAnes_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkSiteAnes.Checked == true)
            {
                PanSiteAnes.Visible = true;
            }
            else
            {
                PanSiteAnes.Visible = false;
            }
        }

        private void rdoAttention_Y_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoAttention_Y.Checked == true)
            {
                PanAttention.Visible = true;
            }
            else
            {
                PanAttention.Visible = false;
            }
        }

        private void btnDelIntake_Click(object sender, EventArgs e)
        {
            if (ssIn.ActiveSheet.RowCount == 1) return;
            if (ssIn.ActiveSheet.ActiveRowIndex == ssIn.ActiveSheet.RowCount - 1) return;

            ssIn.ActiveSheet.Rows.Remove(ssIn.ActiveSheet.ActiveRowIndex, 1);
        }

        private void SetBasicMedcationInfo()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            BasicMedcationInfo = new List<MedcationInfo>();

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT BASCD, BASNAME, BASEXNAME, 'A' DISPSTR, DISSEQNO  ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRBASCD a                            ";
                SQL = SQL + ComNum.VBLF + " WHERE BSNSCLS = '기록지관리'                            ";
                SQL = SQL + ComNum.VBLF + "   AND UNITCLS = '마취기록지기본항목'                    ";
                SQL = SQL + ComNum.VBLF + "UNION ALL                                                ";
                SQL = SQL + ComNum.VBLF + "SELECT BASCD, BASNAME, BASEXNAME, 'B' DISPSTR, DISSEQNO  ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRBASCD a                            ";
                SQL = SQL + ComNum.VBLF + " WHERE BSNSCLS = '기록지관리'                            ";
                SQL = SQL + ComNum.VBLF + "   AND UNITCLS = '마취기록지항목'                        ";
                SQL = SQL + ComNum.VBLF + "ORDER BY DISPSTR, DISSEQNO                               ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        BasicMedcationInfo.Add(new MedcationInfo { ItemNo = dt.Rows[i]["BASCD"].ToString().Trim(), Name = dt.Rows[i]["BASNAME"].ToString().Trim() });
                    }                    
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnSpdMacro_I_Click(object sender, EventArgs e)
        {
            //Intake
            if (frmAnFormSpdMacroX != null)
            {
                frmAnFormSpdMacroX.Dispose();
                frmAnFormSpdMacroX = null;
            }

            frmAnFormSpdMacroX = new frmAnFormSpdMacro(4);
            frmAnFormSpdMacroX.rGetInfo += frmAnFormSpdMacroX_rGetInfo4;
            //frmAnFormSpdMacroX.rEventClosed += frmAnFormSpdMacroX_rEventClosed;
            frmAnFormSpdMacroX.StartPosition = FormStartPosition.CenterParent;
            frmAnFormSpdMacroX.ShowDialog();
        }
        private void frmAnFormSpdMacroX_rGetInfo4(string Name, string Unit)
        {
            int rowIndex = -1;

            for (int i = 0; i < ssIn.ActiveSheet.RowCount; i++)
            {
                if (ssIn.ActiveSheet.Cells[i, 0].Text == "")
                {
                    rowIndex = i;
                    break;
                }
            }

            if (rowIndex >= 0)
            {
                ssIn.ActiveSheet.Cells[rowIndex, 0].Text = Name;
            }
        }

        private void btnBlood_Click(object sender, EventArgs e)
        {
            string strEmrNo = "0";
            string strFormNo = "1965";
            double dUpdateNo = clsEmrQuery.GetMaxUpdateNo(clsDB.DbCon, VB.Val(strFormNo));
            //ActiveFormWrite = new frmEmrChartFlowOld(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, strEmrNo, "W", this);
            using (frmEmrBloodInfo fEmrChartFlowOld = new frmEmrBloodInfo(strFormNo, dUpdateNo.ToString(), AcpEmr, strEmrNo, "W"))
            {
                fEmrChartFlowOld.StartPosition = FormStartPosition.CenterScreen;
                fEmrChartFlowOld.ShowDialog(this);
            }
        }
    }
}

