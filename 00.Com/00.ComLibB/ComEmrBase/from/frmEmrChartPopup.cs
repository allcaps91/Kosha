using ComBase;
using mtsPanel15;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrChartPopup : Form
    {
        private ucEmrChart ucEmrChart = null;
        private DataRow ChartInfo;

        private bool IsDraw;
        private Brush b;

        private Point StartLocation;
        private Point FirstDownLocation;
        private List<RectangleInfo> RectangleList;
        private List<RectangleInfo> RectangleHistoryList;
        private List<RectangleInfo> DBRectangleList;

        private CommentPanel tranPanel;

        public Color GetBackColor
        {
            get
            {
                foreach (Control ctrl in pnlTool.Controls)
                {
                    if (!(ctrl is RadioButton))
                    {
                        continue;
                    }

                    if((ctrl as RadioButton).Checked)
                    {
                        return (ctrl as RadioButton).BackColor;
                    }
                }

                return Color.Transparent;
            }
        }

        public frmEmrChartPopup(DataTable formControlDt, DataRow chartInfo)
        {
            ChartInfo = chartInfo;

            InitializeComponent();

            //  기본브로시 설정
            b = new SolidBrush(Color.FromArgb(255 * 50 / 100, rdoRed.BackColor));
            rdoRed.Checked = true;
            rdoRed.PerformClick();


            this.SuspendLayout();
            //  차트를 Dock 하지 않는다.
            ucEmrChart = new ucEmrChart(formControlDt, chartInfo, true);
            this.Controls.Add(ucEmrChart);
            //ucEmrChart.Height = ucEmrChart.Height - 50;

            //  기록지 정보 부분 수정
            ucEmrChart.tableLayoutPanel1.GetControlFromPosition(0, 0).Dispose();
            ucEmrChart.tableLayoutPanel1.GetControlFromPosition(1, 0).Dispose();
            ucEmrChart.tableLayoutPanel1.GetControlFromPosition(2, 0).Dispose();

            ucEmrChart.tableLayoutPanel1.SetCellPosition(ucEmrChart.lblFormName, new TableLayoutPanelCellPosition(0, 0));
            ucEmrChart.tableLayoutPanel1.SetColumnSpan(ucEmrChart.lblFormName, 4);


            ucEmrChart.Location = new Point(0, 40);
            ucEmrChart.BringToFront();

            RectangleList = new List<RectangleInfo>();
            DBRectangleList = new List<RectangleInfo>();
            RectangleHistoryList = new List<RectangleInfo>();

            //  툴바 상단에 표시
            pnlTool.SetBounds(0, 0, this.Width, 40);
            pnlTool.BringToFront();

            this.ResumeLayout(false);

            this.MouseWheel += FrmEmrChartPopup_MouseWheel;
            this.DoubleBuffered = true;

        }

        /// <summary>
        /// 폼로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmEmrChartPopup_Load(object sender, EventArgs e)
        {
            clsApi.SuspendDrawing(this);
            //  투명패널 위치 지정
            tranPanel = new CommentPanel();
            tranPanel.Height = ucEmrChart.Height;
            tranPanel.Width = ucEmrChart.Width;
            tranPanel.Location = new Point(0, 105);
            tranPanel.Opacity = 0;
            tranPanel.BackColor = Color.FromArgb(0, Color.Transparent);

            this.Controls.Add(tranPanel);
            tranPanel.BringToFront();
            tranPanel.BorderStyle = BorderStyle.FixedSingle;

            clsApi.ResumeDrawing(this);
            //this.ResumeLayout(false);

            //  투명패널 이벤트 설정
            tranPanel.MouseDown += TranPanel_MouseDown;
            tranPanel.MouseUp += TranPanel_MouseUp;
            tranPanel.MouseMove += TranPanel_MouseMove;
        }

        /// <summary>
        /// 폼이 표시 될떄
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmEmrChartPopup_Shown(object sender, EventArgs e)
        {
            GetComment();
        }

        /// <summary>
        /// 주석데이터 조회
        /// </summary>
        private void GetComment()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT EMRNO, EMRNOHIS, SEQ";
            SQL = SQL + ComNum.VBLF + "     , STARTY, STARTX, ENDY, ENDX";
            SQL = SQL + ComNum.VBLF + "     , HEIGHT, BACKCOLOR";
            SQL = SQL + ComNum.VBLF + "     , COMMENTDATE, COMMENTTIME, IDNUMBER";
            SQL = SQL + ComNum.VBLF + "  FROM ADMIN.AEMRCHARTCOMMENT";
            SQL = SQL + ComNum.VBLF + " WHERE EMRNO         = '" + ChartInfo["EMRNO"] + "'";
            SQL = SQL + ComNum.VBLF + "   AND EMRNOHIS      = '" + ChartInfo["EMRNOHIS"] + "'";
            SQL = SQL + ComNum.VBLF + "   AND IDNUMBER = '" + clsType.User.IdNumber + "'";
            SQL = SQL + ComNum.VBLF + "ORDER BY SEQ";

            try
            {
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                foreach(DataRow row in dt.Rows)
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

                    DBRectangleList.Add(item);
                }

                DBDrawHighighter();
                //  주석 그리기
                //DrawHighighter();
            }
            catch(Exception ex)
            {
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

            }
        }


        /// <summary>
        /// 마우스 휠 이벤트
        /// 화면 스크롤 후 FillRectangle 안나오는 형상 발생으로 새로 그려준다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmEmrChartPopup_MouseWheel(object sender, MouseEventArgs e)
        {
            SetPnlToolLocation();
        }

        /// <summary>
        /// 폼 스크롤 이벤트
        /// 화면 스크롤 후 FillRectangle 안나오는 형상 발생으로 새로 그려준다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmEmrChartPopup_Scroll(object sender, ScrollEventArgs e)
        {
            SetPnlToolLocation();
        }

        /// <summary>
        /// 툴상자 상단에 고정
        /// 드로잉 다시 그리기
        /// </summary>
        private void SetPnlToolLocation()
        {
            pnlTool.Location = new Point(0, 0);
            pnlTool.BringToFront();

            tranPanel.BringToFront();
            DrawHighighter();
        }
        
        /// <summary>
        /// 마우스 다운 이벤트 
        /// 하이라이ㄷ트 시작
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TranPanel_MouseDown(object sender, MouseEventArgs e)
        {
            IsDraw = true;
            StartLocation = e.Location;

            FirstDownLocation = e.Location;
        }

        /// <summary>
        /// 마우스 업
        /// 하이라이트 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TranPanel_MouseUp(object sender, MouseEventArgs e)
        {
            IsDraw = false;

            RectangleInfo rect = new RectangleInfo
            {
                StartLocation = FirstDownLocation,
                EndLocation = e.Location,
                BackBrush = b,
                BackColor = GetBackColor
            };

            RectangleList.Add(rect);
        }

        /// <summary>
        /// 마우스 무브
        /// 실제 하이라이트를 색칠한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TranPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDraw)
            {
                Graphics g = tranPanel.CreateGraphics();

                int w = e.Location.X - StartLocation.X;
                if (e.Location.X > StartLocation.X)
                {

                    g.FillRectangle(b, new Rectangle(StartLocation.X, StartLocation.Y, w, 10));
                }
                else
                {
                    w = StartLocation.X - e.Location.X;
                    g.FillRectangle(b, new Rectangle(e.Location.X, StartLocation.Y, w, 10));
                }
                
                StartLocation.X = e.Location.X;

                GraphicsState state = g.Save();
                g.Restore(state);
            }
        }

        //bool IsDBDrawData = false;
        /// <summary>
        /// 드로잉 화면 그리기
        /// </summary>
        private void DrawHighighter()
        {
            //if (IsDBDrawData)
            {
                this.Refresh();
            }

            //clsApi.SuspendDrawing(tranPanel);
            foreach (RectangleInfo item in RectangleList)
            {
                Graphics g = tranPanel.CreateGraphics();

                int w = item.EndLocation.X - item.StartLocation.X;
                if (item.EndLocation.X < item.StartLocation.X)
                {
                    w = item.StartLocation.X - item.EndLocation.X;
                }

                g.FillRectangle(item.BackBrush, new Rectangle(item.StartLocation.X, item.StartLocation.Y, w, item.Height));
            }
            //clsApi.ResumeDrawing(tranPanel);
            //tranPanel.Refresh();

            //IsDBDrawData = true;
        }

        private void DBDrawHighighter()
        {
            clsApi.SuspendDrawing(tranPanel);
            foreach (RectangleInfo item in DBRectangleList)
            {
                //Graphics g = tranPanel.CreateGraphics();

                int w = item.EndLocation.X - item.StartLocation.X;
                if (item.EndLocation.X < item.StartLocation.X)
                {
                    w = item.StartLocation.X - item.EndLocation.X;
                }

                CommentPanel p = new CommentPanel();
                p.Size = new Size(w, item.Height);
                p.Location = new Point(item.StartLocation.X, item.StartLocation.Y);
                tranPanel.Controls.Add(p);

                p.Opacity = 45;
                p.BackColor = item.BackColor;

                p.BringToFront();

                //g.FillRectangle(item.BackBrush, new Rectangle(item.StartLocation.X, item.StartLocation.Y, w, item.Height));
            }
            clsApi.ResumeDrawing(tranPanel);
        }

        /// <summary>
        /// Undo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (RectangleList == null || RectangleList.Count == 0)
            {
                return;
            }

            RectangleInfo item = RectangleList[RectangleList.Count - 1];
            RectangleHistoryList.Add(item);
            RectangleList.Remove(item);

            this.Refresh();

            DrawHighighter();

        }

        /// <summary>
        /// Redo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRedo_Click(object sender, EventArgs e)
        {
            if (RectangleHistoryList == null || RectangleHistoryList.Count == 0)
            {
                return;
            }

            RectangleInfo item = RectangleHistoryList[RectangleHistoryList.Count - 1];
            RectangleList.Add(item);
            RectangleHistoryList.Remove(item);

            this.Refresh();

            DrawHighighter();
        }

        /// <summary>
        /// 색상설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RdoColor_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in pnlTool.Controls)
            {
                if(!(ctrl is RadioButton))
                {
                    continue;
                }

                RadioButton rdo = ctrl as RadioButton;

                rdo.FlatAppearance.BorderSize = rdo.Checked ? 1 : 5;
                if (rdo.Checked)
                {
                    b = new SolidBrush(Color.FromArgb(255 * 50 / 100, rdo.BackColor));
                    rdo.Location = new Point(rdo.Location.X + 3, 8);
                    rdo.Tag = "CHANGE";
                }
                else
                {
                    if(rdo.Tag != null)
                    {
                        rdo.Tag = null;
                        rdo.Location = new Point(rdo.Location.X - 3, 3);
                    }
                }
            }
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveData() == true)
            {
                this.Close();
            }
        }

        private bool SaveData()
        {
            bool rtnVal = false;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int RowAffected = 0;

            //  DB에서 불러온 저장 리스트와 새로 그려진 리스트를 합친다.
            var allList = DBRectangleList.Concat(RectangleList).ToList();

            if(allList.Count == 0)
            {
                ComFunc.MsgBoxEx(this, "저장 할 내용이 없습니다.");
                return false;
            }

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = SQL + ComNum.VBLF + "DELETE FROM ADMIN.AEMRCHARTCOMMENT";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO         = '" + ChartInfo["EMRNO"] + "'";
                SQL = SQL + ComNum.VBLF + "   AND EMRNOHIS      = '" + ChartInfo["EMRNOHIS"] + "'";
                SQL = SQL + ComNum.VBLF + "   AND IDNUMBER = '" + clsType.User.IdNumber + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                for (int i = 0; i < allList.Count; i++)
                {
                    RectangleInfo item = allList[i];

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO ADMIN.AEMRCHARTCOMMENT(";
                    SQL = SQL + ComNum.VBLF + "    EMRNO, EMRNOHIS, SEQ";
                    SQL = SQL + ComNum.VBLF + "  , STARTY, STARTX, ENDY, ENDX";
                    SQL = SQL + ComNum.VBLF + "  , HEIGHT, BACKCOLOR";
                    SQL = SQL + ComNum.VBLF + "  , COMMENTDATE, COMMENTTIME, IDNUMBER";
                    SQL = SQL + ComNum.VBLF + ") VALUES (";
                    SQL = SQL + ComNum.VBLF + "    " + ChartInfo["EMRNO"];
                    SQL = SQL + ComNum.VBLF + "  , " + ChartInfo["EMRNOHIS"];
                    SQL = SQL + ComNum.VBLF + "  , " + (i + 1);
                    SQL = SQL + ComNum.VBLF + "  , " + item.StartLocation.Y;
                    SQL = SQL + ComNum.VBLF + "  , " + item.StartLocation.X;
                    SQL = SQL + ComNum.VBLF + "  , " + item.EndLocation.Y;
                    SQL = SQL + ComNum.VBLF + "  , " + item.EndLocation.X;
                    SQL = SQL + ComNum.VBLF + "  , 10";
                    SQL = SQL + ComNum.VBLF + "  , '" + item.BackColor.Name + "'";
                    SQL = SQL + ComNum.VBLF + "  , TO_CHAR(SYSDATE, 'YYYYMMDD')";
                    SQL = SQL + ComNum.VBLF + "  , TO_CHAR(SYSDATE, 'HH24MISS')";
                    SQL = SQL + ComNum.VBLF + "  , '" + clsType.User.IdNumber + "'";
                    SQL = SQL + ComNum.VBLF + ")";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "저장하였습니다.");
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            tranPanel.BringToFront();
            //tranPanel.Controls.Clear();
            //IsDBDrawData = false;

            //DrawHighighter();
            //tranPanel.Refresh();
            //return;
            if (ComFunc.MsgBoxQEx(this, "삭제 하시겠습니까?", "알림", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
            if (DeleteData() == true)
            {
                this.Close();
            }
        }

        private bool DeleteData()
        {
            bool rtnVal = false;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int RowAffected = 0;

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                SQL = SQL + ComNum.VBLF + "DELETE FROM ADMIN.AEMRCHARTCOMMENT";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO         = '" + ChartInfo["EMRNO"] + "'";
                SQL = SQL + ComNum.VBLF + "   AND EMRNOHIS      = '" + ChartInfo["EMRNOHIS"] + "'";
                SQL = SQL + ComNum.VBLF + "   AND IDNUMBER = '" + clsType.User.IdNumber + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref RowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "삭제하였습니다.");
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    /// <summary>
    /// 하이라이트 정보
    /// </summary>
    public class RectangleInfo
    {
        public Color BackColor;
        public Point StartLocation;
        public Point EndLocation;
        public Brush BackBrush;
        public int Height = 10;
    }
}
