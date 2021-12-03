using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;

namespace ComEmrBase
{
    public partial class ucEmrChart : UserControl
    {

        private const int EM_GETLINECOUNT = 0xba;
        [DllImport("user32", EntryPoint = "SendMessageA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int SendMessage(int hwnd, int wMsg, int wParam, int lParam);

        /// <summary>
        /// 서식지 컨트롤 정보
        /// </summary>
        private DataTable FormControlDt = null;
        private FormXml[] FormControls = null;
        /// <summary>
        /// 차트 정보
        /// </summary>
        private DataRow ChartInfo;
        /// <summary>
        /// 차트 일련번호
        /// </summary>
        private string EmrNo = string.Empty;
        private string FormNo = string.Empty;
        private string UpdateNo = string.Empty;

        #region 텍스트 내용 드래그 관련
        /// <summary>
        /// 마우스 드래그 체크용도
        /// </summary>
        private bool bClick = false;
        /// <summary>
        /// 마지막 커서 위치
        /// </summary>
        private int lastY = 0;
        #endregion

        private string PtNo = string.Empty;
        private List<Control> ChartControls = new List<Control>();

        public delegate void ChartEventHandler(string emrNo, string formNo, string updateNo);
        public event ChartEventHandler ChartModify;
        //public event ChartEventHandler ChartReg;

        //public delegate void ChartCommentEventHandler(DataTable formControlDt, DataRow chartInfo);
        //public event ChartCommentEventHandler ChartComment;

        public Panel ParentPanel;

        readonly int WhiteSpace = 70;
        readonly int PnlY = 75;
        private int PnlHeight = 0;
        //private Dictionary<int, bool> HeightCheck = new Dictionary<int, bool>();
        private Action<int> TextMouseWheelEvent;
        private Action<Point, int> TextMouseMoveEvent;

        //버튼 관련 
        const string IMPORT_SAVED = "★★★"; //중요차트 등록됨
        const string IMPORT_UNSAVED = "☆중요"; //중요차트 해제됨
        Color IS_IMPORT = Color.Blue;
        Color IS_IMPORT_NOT = SystemColors.ControlText;
        const string REMARK_SAVED = "♣♣♣"; //중요차트 등록됨
        const string REMARK_UNSAVED = "주석(색칠하기)"; //중요차트 해제됨

        //==주석 ==//
        //private bool IsDraw;
        //private Brush b;

        //private Point StartLocation;
        //private Point FirstDownLocation;
        private List<RectangleInfo> RectangleList;
        //private List<RectangleInfo> RectangleHistoryList;
        //private TransparentPanel tranPanel;
        //private CommentPanel tranPanel;
        public CommentPanel tranPanel;
        private bool IsTranPanel;
        //==주석 ==//

        public ucEmrChart()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 기존 방식으로 데이터 받아오기
        /// </summary>
        /// <param name="mFromxmls"></param>
        /// <param name="chartInfo"></param>
        /// <param name="formControlDt"></param>
        public ucEmrChart(FormXml[] mFromxmls, DataRow chartInfo, DataTable formControlDt)
        {
            FormControls = mFromxmls;
            FormControlDt = formControlDt;
            ChartInfo = chartInfo;
            EmrNo = ChartInfo["EMRNO"].ToString();
            
            InitializeComponent();
            PtNo = "06792410";


            Init();
            SetChartData();
            SetImportRemarkChart();

        }

        public ucEmrChart(DataTable formControlDt, DataRow chartInfo, bool isTranPanel)
        {
            FormControlDt = formControlDt;
            IsTranPanel = isTranPanel;

            //  상위 frmEmrBaseContinuView에서 차트 그리기를 완료 한 후 Data를 삭제한다.
            ChartInfo = chartInfo;
            EmrNo = ChartInfo["EMRNO"].ToString();
            FormNo = ChartInfo["FORMNO"].ToString();
            UpdateNo = ChartInfo["UPDATENO"].ToString();

            InitializeComponent();

            PtNo = "06792410";

            Init2();

            SetChartData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formControlDt"></param>
        /// <param name="chartInfo"></param>
        public ucEmrChart(DataTable formControlDt, DataRow chartInfo, Action<int> action = null, Action<Point, int> action2 = null)
        {
            FormControlDt = formControlDt;
            TextMouseWheelEvent = action;
            TextMouseMoveEvent = action2;

            //  상위 frmEmrBaseContinuView에서 차트 그리기를 완료 한 후 Data를 삭제한다.
            ChartInfo = chartInfo;
            EmrNo = ChartInfo["EMRNO"].ToString();
            FormNo = ChartInfo["FORMNO"].ToString();
            UpdateNo = ChartInfo["UPDATENO"].ToString();

            InitializeComponent();

            PtNo = "06792410";

            Init2();

            SetChartData();
        }

        public ucEmrChart(DataTable formControlDt, string emrNo, string formNo, string updateNo)
        {
            FormControlDt = formControlDt;
            EmrNo = emrNo;
            FormNo = formNo;
            UpdateNo = updateNo;

            InitializeComponent();

            PtNo = "06792410";

            Init2();

            SetChartData();
            SetImportRemarkChart();

        }

        private void SetChartData()
        {
            #region 의료정보팀 요청으로 추가함(19-09-19)
            //의료정보팀,심사팀,원무팀 아니면 서버시간 칼럼 삭제
            if (clsType.User.BuseCode.Equals("044201") == false && clsType.User.BuseCode.Equals("078201") == false && clsType.User.BuseCode.Equals("077402") == false)
            {
                tableLayoutPanel1.ColumnCount = 5;
                tableLayoutPanel1.ColumnStyles.RemoveAt(tableLayoutPanel1.GetColumn(lblServerDate));
                tableLayoutPanel1.Controls.Remove(lblServerDate);
                lblServerDate.Dispose();


                //사본발급 완료 없으면 칼럼 삭제함.
                bool prtYN = clsEmrQuery.READ_PRTLOG2(EmrNo);
                if (prtYN == false)
                {
                    //tableLayoutPanel2.ColumnCount = 5;
                    //tableLayoutPanel2.ColumnStyles.RemoveAt(tableLayoutPanel1.GetColumn(lblPrntYn));
                    tableLayoutPanel2.Controls.Remove(lblPrntYn);
                    lblPrntYn.Dispose();
                }

                #region 2021-634 전산업무 의뢰서 처리(입퇴원 요약지 검수완료)
                if (FormNo.Equals("1647") && lblComplete != null && lblComplete.IsDisposed == false)
                {
                    bool ChartComplete = clsEmrFunc.READ_CHART_COMPLETE2(clsDB.DbCon, EmrNo);
                    if (ChartComplete == false)
                    {
                        if (tableLayoutPanel2.Controls.Find("lblComplete", true).Length > 0)
                        {
                            tableLayoutPanel2.Controls.Remove(lblComplete);
                            lblComplete.Dispose();
                        }
                    }
                }
                #endregion

                tableLayoutPanel2.ColumnCount = tableLayoutPanel2.Controls.OfType<Control>().Where(d => d.IsDisposed == false).Count();
            }
            else
            {
                //출력시 불필요한 컨트롤 삭제                
                tableLayoutPanel1.ColumnStyles.Clear();
                tableLayoutPanel1.Controls.Clear();
                btnChartModify.Dispose();
                btnChartReg.Dispose();
                btnCahrtComment.Dispose();
                btnRemarkView.Dispose();
                lblServerDate.Dispose();
                tableLayoutPanel1.Dispose();

                //필요한 데이터 한쪽에 보여줌 출력시 레이아웃 패널 보더가 안보이는 현상 때문.
                using (Font font = new Font("굴림체", 12, FontStyle.Bold))
                {
                    lblChartDate.Font = font;
                }

                lblChartDate.Text = string.Format("작성일시: {0} {1}   작성자: {2}", lblChartDate.Text, lblChartTime.Text, lblUserName.Text);
                lblChartDate.Width = Width;

                tableLayoutPanel2.ColumnCount = 1;
                tableLayoutPanel2.SetColumnSpan(lblChartDate, 5);
                lblChartTime.Dispose();
                lblUserName.Dispose();
                lblDeptName.Dispose();
                lblPrntYn.Dispose();

                Control[] controls = pnlChart.Controls.Find("ta1", true);
                if(controls.Length > 0)
                {
                    ((TextBox)controls[0]).BorderStyle = BorderStyle.None;
                }
                
                tableLayoutPanel2.Top = 5;
                panLine.Top = 5;
                panLine.Height = tableLayoutPanel2.Height;
                panLine.Visible = true;
                panLine.Width = tableLayoutPanel2.Width;
                pnlChart.Top = 40;

                Height -= 30;
            }

            #endregion

            //if (pForm == null)
            //{
            //    return;
            //}
            if (ChartInfo["OLDGB"].ToString().Equals("1"))
            {
                //경과이미지 처리
                if(ChartInfo["FORMNO"].ToString().Equals("1232"))
                {
                    clsOldChart.LoadImageChart(this, ChartInfo["EMRNO"].ToString(), false, false, null, null);
                }
                else if (ChartInfo["FORMNO"].ToString().Equals("1963")) //정형외과 SOAP기록지
                {
                    #region
                    clsOldChart.LoadDataXMLOldChart1963(this, ChartInfo["EMRNO"].ToString());
                    #endregion
                }
                else
                {
                    clsOldChart.LoadDataXMLOldChart(this, ChartInfo["EMRNO"].ToString(), false, false, null, null);
                }
            }
            else
            {
                if (ChartInfo["FORMNO"].ToString().Equals("1963")) //정형외과 SOAP기록지
                {
                    #region
                    clsXML.LoadDataChartRow1963(clsDB.DbCon, this, ChartInfo["EMRNO"].ToString());
                    #endregion
                }
                else if (ChartInfo["FORMNO"].ToString().Equals("963")) //경과 기록지 
                {
                    #region
                    clsXML.LoadDataChartRow963(clsDB.DbCon, this, ChartInfo["EMRNO"].ToString());
                    #endregion
                }
                else
                {
                    clsXML.LoadDataChartRow(clsDB.DbCon, this, ChartInfo["EMRNO"].ToString(), false, true, null, null);
                }
            }
        }

        private void SetImportRemarkChart()
        {
            SetImportChart();

            if(!IsTranPanel)
            {
                SetRemarkChart();
            }
        }

        private void SetImportChart()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT EMRNO FROM " + ComNum.DB_EMR + "AEMRCHARTIMPORT ";
            SQL = SQL + ComNum.VBLF + "WHERE EMRNO         = '" + ChartInfo["EMRNO"] + "'";
            SQL = SQL + ComNum.VBLF + "     AND IDNUMBER = '" + clsType.User.IdNumber + "'";

            try
            {
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    btnChartReg.Text = IMPORT_UNSAVED;
                    btnChartReg.ForeColor = IS_IMPORT_NOT;

                    dt.Dispose();
                    dt = null;
                    return;
                }

                btnChartReg.Text = IMPORT_SAVED;
                btnChartReg.ForeColor = IS_IMPORT;

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

            }
        }


        bool IsRemark = false;
        public void SetRemarkChart()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if(IsRemark)
            {
                //ViewComment();
                //tranPanel.Refresh();
                //DrawHighighter();
                //foreach(Control ctrl in tranPanel.Controls)
                //{
                //    (ctrl as CommentPanel).BorderStyle = BorderStyle.FixedSingle;
                //    (ctrl as CommentPanel).BringToFront();
                //}
                this.Refresh();

                //tranPanel.BringToFront();

                return;
            }

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT EMRNO, EMRNOHIS, SEQ";
            SQL = SQL + ComNum.VBLF + "     , STARTY, STARTX, ENDY, ENDX";
            SQL = SQL + ComNum.VBLF + "     , HEIGHT, BACKCOLOR";
            SQL = SQL + ComNum.VBLF + "     , COMMENTDATE, COMMENTTIME, IDNUMBER";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRCHARTCOMMENT";
            SQL = SQL + ComNum.VBLF + " WHERE EMRNO         = '" + ChartInfo["EMRNO"] + "'";
            SQL = SQL + ComNum.VBLF + "   AND EMRNOHIS      = '" + ChartInfo["EMRNOHIS"] + "'";
            SQL = SQL + ComNum.VBLF + "   AND IDNUMBER = '" + clsType.User.IdNumber + "'";
            SQL = SQL + ComNum.VBLF + "ORDER BY SEQ";

            try
            {
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;

                    btnCahrtComment.Text = REMARK_UNSAVED;
                    btnCahrtComment.ForeColor = IS_IMPORT_NOT;
                    btnRemarkView.Visible = false;
                    if (tranPanel != null)
                    {
                        tranPanel.Visible = false;
                    }

                    //RectangleList = null;
                    //tranPanel = null;

                    btnRemarkView.Text = "보기";
                    this.Refresh();
                    return;
                }

                
                btnCahrtComment.Text = REMARK_SAVED;
                btnCahrtComment.ForeColor = IS_IMPORT;

                ////추후 적용
                ViewComment();
                RectangleList = new List<RectangleInfo>();
                foreach (DataRow row in dt.Rows)
                {
                    Color color = Color.FromName(row["BACKCOLOR"].ToString());
                    RectangleInfo item = new RectangleInfo
                    {
                        StartLocation = new Point(int.Parse(row["STARTX"].ToString()), int.Parse(row["STARTY"].ToString())),
                        EndLocation = new Point(int.Parse(row["ENDX"].ToString()), int.Parse(row["ENDY"].ToString())),
                        BackColor = color,
                        BackBrush = new SolidBrush(Color.FromArgb(255 * 50 / 100, color)),
                        Height = int.Parse(row["HEIGHT"].ToString())
                    };

                    RectangleList.Add(item);
                }

                btnRemarkView.Text = "해제";
                btnRemarkView.Visible = true;
                if (btnRemarkView.Text.Equals("해제"))
                {
                    //  주석 그리기
                    DrawHighighter();
                    tranPanel.BringToFront();
                }

                btnRemarkView.Visible = false;
                if (tranPanel != null)
                {
                    //tranPanel.Visible = false;
                }
                //RectangleList = null;
                //tranPanel = null;
                //tranPanel.Paint += TranPanel_Paint;
                //tranPanel.Refresh();
                tranPanel.Invalidate();
                dt.Dispose();
                dt = null;
               
            }
            catch (Exception ex)
            {
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

            }

            IsRemark = true;
        }

        //int Count = 0;
        private void TranPanel_Paint(object sender, PaintEventArgs e)
        {
        }

        /// <summary>
        /// 드로잉 화면 그리기
        /// </summary>
        private void DrawHighighter()
        {
            //return;
            ////this.Refresh();

            if(RectangleList == null)
            {
                return;
            }

            clsApi.SuspendDrawing(tranPanel);

            foreach (RectangleInfo item in RectangleList)
            {
                int w = item.EndLocation.X - item.StartLocation.X;
                if (item.EndLocation.X < item.StartLocation.X)
                {
                    w = item.StartLocation.X - item.EndLocation.X;
                }

                CommentPanel p = new CommentPanel();
                p.Size = new Size(w, item.Height);
                p.Location = new Point(item.StartLocation.X, item.StartLocation.Y);
                tranPanel.Controls.Add(p);

                //new SolidBrush(Color.FromArgb(255 * 50 / 100, rdoRed.BackColor));
                p.Opacity = 55;
                p.BackColor = item.BackColor;
                //p.BackColor = Color.FromArgb(Color.FromName(item.BackColor).ToArgb().ToString());
                p.BringToFront();
            }

            clsApi.ResumeDrawing(tranPanel);
        }

        #region 기존 방식

        /// <summary>
        /// 초기설정
        /// </summary>
        private void Init()
        {
            lblFormName.Text = ChartInfo["FORMNAME"].ToString();
            lblChartDate.Text = ComFunc.FormatStrToDate(ChartInfo["CHARTDATE"].ToString(), "D");
            lblChartTime.Text = ComFunc.FormatStrToDate(ChartInfo["CHARTTIME"].ToString(), "M");
            lblUserName.Text = ChartInfo["NAME"].ToString();
            lblDeptName.Text = ChartInfo["DEPTNAMEK"].ToString();

            string maxX = FormControlDt.AsEnumerable()
                .Max(row => row["LOCATIONX"]).ToString();
            string maxControlWidth = FormControlDt.AsEnumerable()
                .Where(r => r["LOCATIONX"].ToString().Equals(maxX))
                .Max(r => r["SIZEWIDTH"]).ToString();
            string maxY = FormControlDt.AsEnumerable()
                .Max(row => row["LOCATIONY"]).ToString();
            string maxControlHeight = FormControlDt.AsEnumerable()
                .Where(r => r["LOCATIONY"].ToString().Equals(maxY))
                .Max(r => r["SIZEHEIGHT"]).ToString();


            int width = Convert.ToInt32(maxX) + Convert.ToInt32(maxControlWidth);
            int height = Convert.ToInt32(maxY) + Convert.ToInt32(maxControlHeight);

            this.BorderStyle = BorderStyle.FixedSingle;
            this.Width = width + WhiteSpace;
            this.Height = height + WhiteSpace + 75;

            this.tableLayoutPanel1.Width = width;
            this.tableLayoutPanel1.Location = new Point(0, 5);
            this.tableLayoutPanel2.Width = width;
            this.tableLayoutPanel2.Location = new Point(0, 35);

            this.pnlChart.Location = new Point(0, 75);
            this.pnlChart.Width = width;
            this.pnlChart.Height = height;

            if (FormControls != null)
            {
                for (int i = 0; i < FormControls.Length; i++)
                {
                    if (FormControls[i].strCONTROLPARENT == "Form1")
                    {
                        FormControls[i].strCONTROLPARENT = "pnlChart";
                    }

                    //if (FormControls[i].strCONTROTYPE == "System.Windows.Forms.Panel")
                    //{
                    //    FormControls[i].strCONTROTYPE = "mtsPanel15.mPanel";
                    //}
                }

                FormLoadControl.LoadControl(this, FormControls, "pnlChart");
            }
            return;

            var list = FormControlDt.AsEnumerable()
                .Where(r => r["CONTROLPARENT"].Equals("Form1"))
                .OrderByDescending(r => r["CHILDINDEX"]);

            foreach (DataRow r in list)
            {
                Control ctrl = CreateControl(r);
                this.pnlChart.Controls.Add(ctrl);

                ChartControls.Add(ctrl);
                ctrl.BringToFront();

                CreateChilds(ctrl);
            }

            //SetJindan();

            this.pnlChart.Location = new Point(0, 75);
            this.pnlChart.Width = width;
            this.pnlChart.Height = height + WhiteSpace;
        }

        #endregion

        /// <summary>
        /// 초기설정
        /// </summary>
        private void Init2()
        {

            lblFormName.Text = ChartInfo["FORMNAME"].ToString();

            lblChartDate.Text = ComFunc.FormatStrToDate(ChartInfo["CHARTDATE"].ToString(), "D");
            lblChartTime.Text = ComFunc.FormatStrToDate(ChartInfo["CHARTTIME"].ToString(), "M");

            lblUserName.Text = ChartInfo["NAME"].ToString();
            lblDeptName.Text = ChartInfo["DEPTNAMEK"].ToString();


            string maxX = FormControlDt.AsEnumerable()
                .Max(row => row["LOCATIONX"]).ToString();
            string maxControlWidth = FormControlDt.AsEnumerable()
                .Where(r => r["LOCATIONX"].ToString().Equals(maxX))
                .Max(r => r["SIZEWIDTH"]).ToString();
            string maxY = FormControlDt.AsEnumerable()
                .Max(row => row["LOCATIONY"]).ToString();
            string maxControlHeight = FormControlDt.AsEnumerable()
                .Where(r => r["LOCATIONY"].ToString().Equals(maxY))
                .Max(r => r["SIZEHEIGHT"]).ToString();

            int width = Convert.ToInt32(maxX) + Convert.ToInt32(maxControlWidth);
            int height = Convert.ToInt32(maxY) + Convert.ToInt32(maxControlHeight);

            if(FormNo.Equals("963"))
            {
                height = 50;
            }

            #region 2021-634 전산업무 의뢰서 처리 검수완료 표시(입퇴원 요약지)
            if (FormNo.Equals("1647") && ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>() < "2021-07-01 06:00:00".To<DateTime>() || 
                FormNo.Equals("1647") == false)
            {
                tableLayoutPanel2.Controls.Remove(lblComplete);
                lblComplete.Dispose();
                tableLayoutPanel2.ColumnCount = tableLayoutPanel2.Controls.OfType<Control>().Where(d => d.IsDisposed == false).Count();
            }
            #endregion

            this.BorderStyle = BorderStyle.FixedSingle;
            this.Width = width + WhiteSpace;
            this.Height = height + WhiteSpace + 75;

            this.tableLayoutPanel1.Width = width;
            this.tableLayoutPanel1.Location = new Point(0, 5);
            this.tableLayoutPanel2.Width = width;
            this.tableLayoutPanel2.Location = new Point(0, 35);

            this.pnlChart.Location = new Point(0, 75);
            this.pnlChart.Width = width;
            this.pnlChart.Height = height;

            if(FormNo.Equals("1963") == false)
            {
                var list = FormControlDt.AsEnumerable()
                    .Where(r => r["CONTROLPARENT"].Equals("Form1"))
                    .OrderByDescending(r => r["CHILDINDEX"]);

                foreach (DataRow r in list)
                {
                    this.SuspendLayout();
                    Control ctrl = CreateControl(r);
                    this.pnlChart.Controls.Add(ctrl);

                    ctrl.BringToFront();

                    CreateChilds(ctrl);
                    //  높이 설정
                    //if (ctrl.Visible)
                    {
                        PnlHeight += ctrl.Height;
                    }
                    
                    this.ResumeLayout(false);
                }
            }
            else
            {
                #region 리치텍스박스 하드코딩
                RichTextBox control = new RichTextBox();
                control.Name = "Content";
                control.Text = string.Empty;
                control.Dock = DockStyle.Left;
                control.Width = 600;
                control.AutoSize = false;
                control.Multiline = true;
                control.ScrollBars = RichTextBoxScrollBars.None;
                control.MouseWheel += TextBox_MouseWheel;
                control.ReadOnly = true;
                control.BackColor = Color.White;
                control.Height = 50;

                this.pnlChart.Controls.Add(control);
                control.Parent = pnlChart;

                ChartControls.Add(control);
                control.BringToFront();
                //  높이 설정
                PnlHeight += control.Height;

                (control as RichTextBox).GotFocus += (o, s) =>
                {
                    (control as RichTextBox).SelectionStart = 0;
                    (control as RichTextBox).SelectionLength = 0;
                    (control as RichTextBox).SelectedText = string.Empty;
                };

                (control as RichTextBox).LostFocus += (o, s) =>
                {
                    (control as RichTextBox).SelectionLength = 0;
                    (control as RichTextBox).SelectionStart = 0;
                    (control as RichTextBox).SelectedText = string.Empty;
                };


                control.Font = new Font("돋움체", 11);
                control.AutoSize = true;
                //if ((control as RichTextBox).Multiline)
                //{
                (control as RichTextBox).ContentsResized += UcEmrChart_ContentsResized;
                //}

                control.BringToFront();
                Point location = new Point(0, 0);
                //  넓이 재설정
                if (this.pnlChart.Width < (width + location.X))
                {
                    this.Width = width + location.X + WhiteSpace;
                    this.pnlChart.Width = width + location.X;
                    this.tableLayoutPanel1.Width = width + location.X;
                    this.tableLayoutPanel2.Width = width + location.X;
                }


                #endregion
            }

            //SetJindan();

            this.pnlChart.Height = PnlHeight + WhiteSpace;
            if (FormNo.Equals("963"))
            {
                this.Height = PnlHeight + 30;
            }
            else
            {
                this.Height = PnlHeight + 100;
            }
            

            btnChartReg.Text = IMPORT_UNSAVED;
            btnChartReg.ForeColor = IS_IMPORT_NOT;

            btnCahrtComment.Text = REMARK_UNSAVED;
            btnCahrtComment.ForeColor = IS_IMPORT_NOT;
            btnRemarkView.Text = "보기";

        }

 
        private void UcEmrChart_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            ((RichTextBox)sender).Height = e.NewRectangle.Height + 5;
            ((RichTextBox)sender).Parent.Height = e.NewRectangle.Height + 75;
            this.Height = pnlChart.Height + 30;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
        }
        private void ViewComment()
        {
            if (tranPanel != null)
            {
                tranPanel.Dispose();
                tranPanel = null;
            }

            this.SuspendLayout();
            //clsApi.SuspendDrawing(this);
            //tranPanel = new TransparentPanel();
            tranPanel = new CommentPanel();
            tranPanel.Height = this.Height - WhiteSpace - PnlY;
            tranPanel.Width = this.Width - WhiteSpace;
            tranPanel.Location = new Point(0, 65);  //75
            tranPanel.BackColor = Color.FromArgb(0, Color.Transparent);
            tranPanel.Opacity = 0;
            //tranPanel.BorderStyle = BorderStyle.FixedSingle;

            this.Controls.Add(tranPanel);
            tranPanel.BorderStyle = BorderStyle.FixedSingle;
            tranPanel.BringToFront();
            //clsApi.ResumeDrawing(this);
            this.ResumeLayout(false);
        }

        private void UnViewComment()
        {
            tranPanel = null;
        }

        #region 진단

        private string GetJindanQuery
        {
            get
            {
                string SQL = "";    //Query문

                SQL = SQL + ComNum.VBLF + "SELECT A.ILLCODE, B.ILLNAMEK, B.ILLNAMEE";
                SQL = SQL + ComNum.VBLF + "  FROM {0} A";
                SQL = SQL + ComNum.VBLF + "  INNER JOIN {1}BAS_ILLS B";
                SQL = SQL + ComNum.VBLF + "          ON A.ILLCODE = B.ILLCODE";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + PtNo + "'";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + ChartInfo["CHARTDATE"].ToString() + "', 'YYYY-MM-DD')";

                if (ChartInfo["MEDDEPTCD"].ToString().Equals("ER"))
                {
                    SQL = string.Format(SQL, "KOSMOS_OCS.OCS_EILLS", ComNum.DB_PMPA);
                }
                else if (ChartInfo["INOUTCLS"].ToString().Equals("O"))
                {
                    SQL = string.Format(SQL, "KOSMOS_OCS.OCS_OILLS", ComNum.DB_PMPA);
                }
                else
                {
                    SQL = string.Format(SQL, "KOSMOS_OCS.OCS_IILLS", ComNum.DB_PMPA);
                }

                return SQL;
            }
        }
        /// <summary>
        /// 진단가져오기
        /// </summary>
        private void SetJindan()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = GetJindanQuery;
            //  입원
            if (ChartInfo["INOUTCLS"].ToString().Equals("I"))
            {
                SQL = string.Empty;
                SQL = SQL + ComNum.VBLF + "SELECT INDATE, OUTDATE";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE TO_DATE('" + ChartInfo["CHARTDATE"].ToString() + "', 'YYYY-MM-DD') BETWEEN INDATE AND OUTDATE";
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + PtNo + "'";
                SQL = SQL + ComNum.VBLF + "UNION ALL";
                SQL = SQL + ComNum.VBLF + "SELECT INDATE, OUTDATE";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE TO_DATE(INDATE, 'YYYY-MM-DD') <= TO_DATE('" + ChartInfo["CHARTDATE"].ToString() + "', 'YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND OUTDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + PtNo + "' ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if(dt == null || dt.Rows.Count == 0)
                {
                    //txtJindan.Text = string.Empty;
                    return;
                }

                DateTime? frDate = null;
                DateTime? endDate = null;

                frDate = (DateTime)dt.Rows[0]["INDATE"];
                if(dt.Rows[0]["OUTDATE"] != null && !string.IsNullOrWhiteSpace(dt.Rows[0]["OUTDATE"].ToString()))
                {
                    endDate = (DateTime)dt.Rows[0]["OUTDATE"];
                }

                SQL = string.Empty;
                SQL = SQL + ComNum.VBLF + "SELECT A.ILLCODE, B.ILLNAMEK, B.ILLNAMEE";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_IILLS A";
                SQL = SQL + ComNum.VBLF + "  INNER JOIN KOSMOS_PMPA.BAS_ILLS B";
                SQL = SQL + ComNum.VBLF + "          ON A.ILLCODE = B.ILLCODE";
                SQL = SQL + ComNum.VBLF + "WHERE PTNO = '" + PtNo + "'";
                SQL = SQL + ComNum.VBLF + "   AND BDATE >= TO_DATE('" + frDate.Value.ToString("yyyyMMdd") + "', 'YYYY-MM-DD')";

                if (endDate != null)
                {
                    SQL = SQL + ComNum.VBLF + "   AND BDATE <= TO_DATE('" + endDate.Value.ToString("yyyyMMdd") + "', 'YYYY-MM-DD')";
                }
            }

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            //txtJindan.Text = string.Empty;
            foreach(DataRow row in dt.Rows)
            {
                //txtJindan.Text += row["ILLNAMEE"].ToString() + ComNum.VBLF;
            }
        }

        #endregion

        #region 컨트롤 생성

        /// <summary>
        /// 자식 컨트롤 생성
        /// </summary>
        /// <param name="parent"></param>
        private void CreateChilds(Control parent)
        {
            var list = FormControlDt.AsEnumerable()
                .Where(r => r["CONTROLPARENT"].Equals(parent.Name))
                .OrderByDescending(r => r["CHILDINDEX"]);

            foreach (DataRow r in list)
            {
                Control ctrl = CreateControl(r);
                this.pnlChart.Controls.Add(ctrl);

                ctrl.Parent = parent;
                ctrl.BringToFront();
                ChartControls.Add(ctrl);

                CreateChilds(ctrl);
            }
        }


        /// <summary>
        /// 컨트롤 생성
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private Control CreateControl(DataRow row)
        {
            DockStyle dock = (DockStyle)Enum.Parse(typeof(DockStyle), row["DOCK"].ToString(), true);
            int width = Convert.ToInt32(row["SIZEWIDTH"].ToString());
            int height = Convert.ToInt32(row["SIZEHEIGHT"].ToString());
            Point location = new Point(Convert.ToInt32(row["LOCATIONX"].ToString()), Math.Abs(Convert.ToInt32(row["LOCATIONY"].ToString())));
            string name = row["CONTROLNAME"].ToString();
            string parent = row["CONTROLPARENT"].ToString();

            string text = string.Empty;
            if (row["TEXT"] != null)
            {
                text = row["TEXT"].ToString();
            }

            Control control = null;
            if (row["CONTROTYPE"].Equals("System.Windows.Forms.Panel"))
            {
                Panel panel = new Panel();
                panel.BorderStyle = FormFunc.GetBorderStyle(row["BOARDSTYLE"].ToString());
                control = panel;
            }
            else if (row["CONTROTYPE"].Equals("System.Windows.Forms.RadioButton"))
            {
                RadioButton rdo = new RadioButton();
                rdo.Text = text;
                rdo.TextAlign = FormFunc.GetContentAlign(row["TEXTALIGN"].ToString());
                rdo.AutoSize = FormFunc.GetTrueFalse(row["AUTOSIZE"].ToString());
                rdo.Checked = Convert.ToBoolean(row["CHECKED"]);
                rdo.Appearance = FormFunc.GetAppearance(row["APPEARANCS"].ToString());

                if (rdo.Appearance == Appearance.Button)
                {
                    rdo.FlatAppearance.BorderSize = (int)VB.Val(row["FLATBORDERSIZE"].ToString());
                }
                rdo.FlatStyle = FormFunc.GetFlatStyle(row["FLATSTYLE"].ToString());

                rdo.AutoCheck = false;

                control = rdo;
            }
            else if (row["CONTROTYPE"].Equals("System.Windows.Forms.CheckBox"))
            {
                CheckBox chk = new CheckBox();
                chk.Text = text;
                chk.TextAlign = FormFunc.GetContentAlign(row["TEXTALIGN"].ToString());
                chk.AutoSize = FormFunc.GetTrueFalse(row["AUTOSIZE"].ToString());
                chk.Appearance = FormFunc.GetAppearance(row["APPEARANCS"].ToString());

                if (chk.Appearance == Appearance.Button)
                {
                    chk.FlatAppearance.BorderSize = (int)VB.Val(row["FLATBORDERSIZE"].ToString());
                }
                chk.FlatStyle = FormFunc.GetFlatStyle(row["FLATSTYLE"].ToString());

                chk.Checked = Convert.ToBoolean(row["CHECKED"]);

                chk.AutoCheck = false;

                control = chk;
            }
            else if (row["CONTROTYPE"].Equals("System.Windows.Forms.TextBox"))
            {
                TextBox textBox = new TextBox();
                textBox.Text = text;
                textBox.AutoSize = false;
                textBox.Multiline = Convert.ToBoolean(row["MULTILINE"]);
                textBox.TextAlign = FormFunc.GetControlAlign(row["TEXTALIGN"].ToString());
                textBox.BorderStyle = FormFunc.GetBorderStyle(row["BOARDSTYLE"].ToString());
                textBox.ScrollBars = ScrollBars.None;

                textBox.MouseWheel += TextBox_MouseWheel;
                textBox.MouseDown += TextBox_MouseDown;
                textBox.MouseUp += TextBox_MouseUp;
                textBox.MouseMove += TextBox_MouseMove;

                textBox.ReadOnly = true;
                textBox.BackColor = Color.White;

                control = textBox;
            }
            else if (row["CONTROTYPE"].Equals("System.Windows.Forms.Label"))
            {
                Label label = new Label();

                label.Text = text;
                label.Dock = dock;
                label.AutoSize = FormFunc.GetTrueFalse(row["AUTOSIZE"].ToString());
                label.TextAlign = FormFunc.GetContentAlign(row["TEXTALIGN"].ToString());
                label.BorderStyle = FormFunc.GetBorderStyle(row["BOARDSTYLE"].ToString());

                control = label;
            }
            else if (row["CONTROTYPE"].Equals("System.Windows.Forms.Button"))
            {
                Button button = new Button();
                button.Text = text;
                button.TextAlign = FormFunc.GetContentAlign(row["TEXTALIGN"].ToString());

                control = button;

            }
            else if (row["CONTROTYPE"].Equals("System.Windows.Forms.PictureBox"))
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.Image = ComFunc.StringToImage(row["IMAGE"].ToString().Trim());
                pictureBox.BorderStyle = FormFunc.GetBorderStyle(row["BOARDSTYLE"].ToString());

                control = pictureBox;
            }

            if (control != null)
            {
                Color myBackColor = ColorTranslator.FromHtml(row["BACKCOLOR"].ToString());
                Color myForeColorColor = ColorTranslator.FromHtml(row["FORECOLOR"].ToString());

                control.BackColor = myBackColor;
                control.ForeColor = myForeColorColor;

                control.Width = width;
                control.Height = height;
                control.Location = location;
                control.Dock = dock;
                control.Name = name;
                control.Tag = row["USERFUNC"].ToString();

                //  경과 기록지 
                //  사이즈 글 내용만큼만 표시 하기 위해서 높이 줄임
                if (FormNo.Equals("963"))
                {
                    control.Height = 50;
                }

                if (row["CONTROTYPE"].Equals("System.Windows.Forms.TextBox"))
                {
                    (control as TextBox).GotFocus += (o, s) =>
                    {
                        (control as TextBox).SelectionStart = 0;
                        (control as TextBox).SelectionLength = 0;
                        (control as TextBox).SelectedText = string.Empty;
                    };

                    (control as TextBox).LostFocus += (o, s) =>
                    {
                        (control as TextBox).SelectionLength = 0;
                        (control as TextBox).SelectionStart = 0;
                        (control as TextBox).SelectedText = string.Empty;
                    };

                    if ((control as TextBox).Multiline)
                    {
                        control.Tag = FormNo.Equals("963") ? "True" : row["AUTOHEIGH"].ToString();
                        (control as TextBox).TextChanged += pTextBox_TextChanged;
                    }

                }

                control.BringToFront();

                control.Enabled = FormFunc.GetTrueFalse(row["ENABLED"].ToString());
                control.Visible = FormFunc.GetTrueFalse(row["VISIBLED"].ToString());

                //  넓이 재설정
                if(this.pnlChart.Width < (width + location.X))
                {
                    this.Width = width + location.X + WhiteSpace;
                    this.pnlChart.Width = width + location.X;
                    this.tableLayoutPanel1.Width = width + location.X;
                    this.tableLayoutPanel2.Width = width + location.X;
                }

                if (row["FONTS"] != null)
                {
                    if (FormNo.Equals("963"))
                    {
                        control.Font = new Font("돋움체", 11, FontStyle.Regular);
                    }
                    else
                    {
                        control.Font = FormFunc.StringToFont(row["FONTS"].ToString());
                    }
                }
            }

            return control;
        }


        #region 텍스트박스 내용 드래그 체크 이벤트
        private void TextBox_MouseMove(object sender, MouseEventArgs e)
        {
            if(TextMouseMoveEvent != null && bClick)
            {
                TextBox textBox = ((TextBox)sender);
                if (lastY == e.Y || textBox.Lines.Length == 0)
                    return;

                int LineHeight = textBox.ClientSize.Height / textBox.Lines.Length;
                int CheckHeight = LineHeight * 4;

                if (e.Y >= textBox.ClientSize.Height + CheckHeight)
                {
                    lastY += CheckHeight;
                    return;
                }
                else if (lastY >= CheckHeight && e.Y <= CheckHeight)
                {
                    lastY = 0;
                    return;
                }

                if (e.Y > lastY && Math.Abs(e.Y - lastY) >= CheckHeight ||
                    lastY > e.Y && Math.Abs(lastY - e.Y) >= CheckHeight
                    )
                {
                    //Console.WriteLine("아래4 last Y: {0} e.Y: {1}", lastY, e.Y);
                    TextMouseMoveEvent(e.Location, lastY);
                    lastY = e.Y;
                    return;
                }

            }   
        }

        private void TextBox_MouseDown(object sender, MouseEventArgs e)
        {
            TextBox textBox = ((TextBox)sender);
            if (e.Button != MouseButtons.Left || e.Y == 0 || textBox.Lines.Length <= 6)
            {
                bClick = false;
                return;
            }

            bClick = true;
            lastY = e.Y;
        }

        private void TextBox_MouseUp(object sender, MouseEventArgs e)
        {
            lastY = 0;
            bClick = false;
        }
        #endregion

        /// <summary>
        /// 텍스트 박스 마우스 스크롤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if(TextMouseWheelEvent != null)
            {
                TextMouseWheelEvent(e.Delta);
            }
        }

        #endregion

        private void UcEmrChart_Load(object sender, EventArgs e)
        {
            //pnlChart.AutoScrollPosition = new Point(0, -32000);
            //int d = this.pnlChart.AutoScrollPosition.Y;

            //SetRemarkChart();
            SetImportRemarkChart();
        }

        private void pTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tx = (TextBox) sender;
            if (tx.Tag == null) return;
            if (tx.Tag.ToString().Trim() == "" || tx.Tag.ToString().Trim().ToUpper() == "FALSE") return;

            ChengeHeight(tx);
        }

        /// <summary>
        /// TextBox Height 조절
        /// 컨트롤은 Dock FIll 이 되어 있어야함
        /// </summary>
        /// <param name="cTextBox"></param>
        private void ChengeHeight(TextBox cTextBox)
        {
            Control pCon = null;
            Control MaxParentCon = null;

            int beforeTextBoxHeight = cTextBox.Height;

            pCon = cTextBox.Parent;

            if (pCon.Parent.Name == "pnlChart")
            {
                MaxParentCon = pCon;
            }
            else
            {
                MaxParentCon = FindMaxParentCon(pCon);
            }

            int afterTextBoxHeight = CalcHeght(cTextBox);
            int PContHeight = pCon.Height;

            if (afterTextBoxHeight < 40)
            {
                afterTextBoxHeight = 40;
                if (afterTextBoxHeight > beforeTextBoxHeight)
                {
                    if (MaxParentCon != pCon)
                    {
                        if (pCon.Dock == DockStyle.Top)
                        {
                            pCon.Height = pCon.Height + (afterTextBoxHeight - beforeTextBoxHeight);
                        }
                    }
                    MaxParentCon.Height = MaxParentCon.Height + (afterTextBoxHeight - beforeTextBoxHeight);

                    this.Height = this.Height + (afterTextBoxHeight - beforeTextBoxHeight);
                    this.pnlChart.Height = this.pnlChart.Height + (afterTextBoxHeight - beforeTextBoxHeight);
                }
            }
            else
            {
                if (afterTextBoxHeight > beforeTextBoxHeight)
                {
                    if (MaxParentCon != pCon)
                    {
                        if (pCon.Dock == DockStyle.Top)
                        {
                            pCon.Height = pCon.Height + (afterTextBoxHeight - beforeTextBoxHeight);
                        }
                    }
                    MaxParentCon.Height = MaxParentCon.Height + (afterTextBoxHeight - beforeTextBoxHeight);
                    this.Height = this.Height + (afterTextBoxHeight - beforeTextBoxHeight);
                    this.pnlChart.Height = this.pnlChart.Height + (afterTextBoxHeight - beforeTextBoxHeight);
                }
                else if (afterTextBoxHeight < beforeTextBoxHeight)
                {
                    if (MaxParentCon != pCon)
                    {
                        if (pCon.Dock == DockStyle.Top)
                        {
                            pCon.Height = pCon.Height - (beforeTextBoxHeight - afterTextBoxHeight);
                        }
                    }
                    MaxParentCon.Height = MaxParentCon.Height - (beforeTextBoxHeight - afterTextBoxHeight);
                    this.Height = this.Height - (afterTextBoxHeight - beforeTextBoxHeight);
                    this.pnlChart.Height = this.pnlChart.Height - (afterTextBoxHeight - beforeTextBoxHeight);
                }
            }
        }

        private Control FindMaxParentCon(Control cCon)
        {
            Control pCon = null;

            pCon = cCon.Parent;
            if(pCon.Parent == null)
            {
                return null;
            }

            if (pCon.Parent.Name != "pnlChart")
            {
                pCon = FindMaxParentCon(pCon);
            }
            return pCon;
        }

        /// <summary>
        /// TextBox 사이즈 측정 
        /// 버거 땜시 하드코딩해서 사이즈 마춤
        /// </summary>
        /// <param name="cTextBox"></param>
        /// <returns></returns>
        private int CalcHeght(TextBox cTextBox)
        {
            var numberOfLines = SendMessage(cTextBox.Handle.ToInt32(), EM_GETLINECOUNT, 0, 0);
            //float intHeight = GetSize(cTextBox);

            int FontHeight = cTextBox.Font.Height;
            double fHeight = 0;

            if (numberOfLines <= 4)
            {
                fHeight = cTextBox.Font.Height;
            }
            else if (numberOfLines > 4 && numberOfLines <= 10)
            {
                fHeight = FontHeight - 1;
            }
            else if (numberOfLines > 10 && numberOfLines <= 20)
            {
                fHeight = FontHeight - 1.3;
            }
            else if (numberOfLines > 20 && numberOfLines <= 24)
            {
                fHeight = FontHeight - 1.5;
            }
            else if (numberOfLines > 24 && numberOfLines <= 30)
            {
                fHeight = FontHeight - 1.6;
            }
            else if (numberOfLines > 30 && numberOfLines <= 50)
            {
                fHeight = FontHeight - 1.8;
            }
            else if (numberOfLines > 50 && numberOfLines <= 180)  //80
            {
                fHeight = FontHeight - 1.9;
            }
            else if (numberOfLines > 180)  //80
            {
                fHeight = FontHeight - 2;
            }

            int cHeight = (int)(fHeight * numberOfLines) + 2;
            return cHeight;
        }

        /// <summary>
        /// 수정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnChartModify_Click(object sender, EventArgs e)
        {
           
            ChartModify(EmrNo, FormNo, UpdateNo);
        }

        /// <summary>
        /// 등록
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnChartReg_Click(object sender, EventArgs e)
        {
            if (btnChartReg.Text.Trim() == IMPORT_UNSAVED)
            {
                if (SaveImport(EmrNo) == true)
                {
                    btnChartReg.Text = IMPORT_SAVED;
                    btnChartReg.ForeColor = IS_IMPORT;
                }
            }
            else
            {
                if (DeleteImport(EmrNo) == true)
                {
                    btnChartReg.Text = IMPORT_UNSAVED;
                    btnChartReg.ForeColor = IS_IMPORT_NOT;
                }
            }
            //ChartReg(EmrNo, FormNo, UpdateNo);
        }

        /// <summary>
        /// 주석
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCahrtComment_Click(object sender, EventArgs e)
        {
            ChartComment(FormControlDt, ChartInfo);
        }

        private void ChartComment(DataTable formControlDt, DataRow chartInfo)
        {
            using (frmEmrChartPopup popup = new frmEmrChartPopup(formControlDt, chartInfo))
            {
                popup.StartPosition = FormStartPosition.CenterParent;
                popup.ShowDialog(this);
            }

            SetRemarkChart();

            return;
        }

        private void ucEmrChart_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            
        }

        private bool SaveImport(string strEMRNO)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT EMRNO FROM " + ComNum.DB_EMR + "AEMRCHARTIMPORT ";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO         = '" + ChartInfo["EMRNO"] + "'";
                SQL = SQL + ComNum.VBLF + "     AND IDNUMBER = '" + clsType.User.IdNumber + "'";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count == 0)
                {
                    //string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTIMPORT ";
                    SQL = SQL + ComNum.VBLF + "     (";
                    SQL = SQL + ComNum.VBLF + "     IDNUMBER, EMRNO, INPDATE, INPTIME";
                    SQL = SQL + ComNum.VBLF + "     )";
                    SQL = SQL + ComNum.VBLF + "VALUES (";
                    SQL = SQL + ComNum.VBLF + "     '" + clsType.User.IdNumber + "',";
                    SQL = SQL + ComNum.VBLF + "     " + VB.Val(strEMRNO) + ",";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(SYSDATE, 'YYYYMMDD'),";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(SYSDATE, 'HH24MISS')";
                    SQL = SQL + ComNum.VBLF + "     )";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }
                dt.Dispose();
                dt = null;

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private bool DeleteImport(string strEMRNO)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");

                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM " + ComNum.DB_EMR + "AEMRCHARTIMPORT ";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO         = '" + ChartInfo["EMRNO"] + "'";
                SQL = SQL + ComNum.VBLF + "     AND IDNUMBER = '" + clsType.User.IdNumber + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        private void btnRemarkView_Click(object sender, EventArgs e)
        {
            //if(btnRemarkView.Text.Equals("보기"))
            //{
            //    tranPanel.Visible = true;
            //    tranPanel.BringToFront();
            //    DrawHighighter();
            //    btnRemarkView.Text = "해제";
            //}
            //else
            //{
            //    tranPanel.Visible = false;
            //    btnRemarkView.Text = "보기";
            //    this.Refresh();
            //}
        }
    }

    public static class ControlExtensions
    {
        public static void InvokeUiRequired(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(action);
            }
            else
            {
                action.Invoke();
            }
        }
    }
}
