using System;
using System.Data;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using System.Drawing;
using ComBase;
using ComDbB;

namespace ComLibB
{
    public partial class frmSearchRoadWeb : Form
    {
        string currentPage   = "1";                 //현재 페이지
        string countPerPage  = "100";              //1페이지당 출력 갯수
        string confmKey_Test = "TESTJUSOGOKR";      //테스트 Key
        string confmKey      = "U01TX0FVVEgyMDE4MDkyNzEyMDc0MzEwODE5ODc=";  //인증 key
        string keyword       = string.Empty;
        string apiurl        = string.Empty;
        string[] strData     = new string[5000];

        private string GstrValue = "";

        public delegate void SetGstrValue(string GstrValue);
        public event SetGstrValue rSetGstrValue;
        
        public frmSearchRoadWeb()
        {
            InitializeComponent();
            SetEvent();
        }

        void setSpd(FpSpread Spd)
        {
            Spd.ActiveSheet.Columns.Get(-1).Visible = false;

            Spd.ActiveSheet.Columns.Get(0).Visible = true;      //roadAddr
            Spd.ActiveSheet.Columns.Get(3).Visible = true;      //jibunAddr
            Spd.ActiveSheet.Columns.Get(5).Visible = true;      //zipNo
            Spd.ActiveSheet.Columns.Get(9).Visible = true;      //detBdNmList
            Spd.ActiveSheet.Columns.Get(10).Visible = true;     //bdNm

            Spd.ActiveSheet.ColumnHeader.Rows[0].Height = 50;
            Spd.ActiveSheet.Rows[-1].Height = 24;

            Spd.VerticalScrollBarWidth = 16;
            Spd.HorizontalScrollBarHeight = 16;
            
            //2.컬럼 스타일
            Spd.ActiveSheet.Columns.Get(-1).Locked = true;
            TextCellType spdObj = new TextCellType();
            spdObj.WordWrap = true;
            Spd.ActiveSheet.Columns.Get(-1).CellType = spdObj;

            //3.정렬
            Spd.ActiveSheet.Columns[5].HorizontalAlignment = CellHorizontalAlignment.Center;
            Spd.ActiveSheet.Columns[-1].VerticalAlignment = CellVerticalAlignment.Center;
        }

        void setSpdWidth(FpSpread Spd)
        {
            //1.헤더 및 사이즈
            Spd.ActiveSheet.Columns.Get(-1).Font = new Font("굴림체", 9);
            Spd.ActiveSheet.Columns.Get(0).Label = "도로명주소";
            Spd.ActiveSheet.Columns.Get(0).Width = 300;
            Spd.ActiveSheet.Columns.Get(3).Label = "지번주소";
            Spd.ActiveSheet.Columns.Get(3).Width = 300;
            Spd.ActiveSheet.Columns.Get(5).Label = "우편번호";
            Spd.ActiveSheet.Columns.Get(5).Width = 64;
            Spd.ActiveSheet.Columns.Get(9).Label = "건물List";
            Spd.ActiveSheet.Columns.Get(9).Width = 150;
            Spd.ActiveSheet.Columns.Get(10).Label = "건물명";
            Spd.ActiveSheet.Columns.Get(10).Width = 210;
        }
        
        void SetEvent()
        {
            this.Load                   += new EventHandler(eFormLoad);
            this.btnSearch.Click        += new EventHandler(eBtnClick);
            this.btnExit.Click          += new EventHandler(eBtnClick);
            this.btnNext.Click          += new EventHandler(eBtnClick);
            this.btnPrev.Click          += new EventHandler(eBtnClick);
            this.txtSearch.KeyPress     += new KeyPressEventHandler(eKeyPress);
            this.SS1.CellClick          += new CellClickEventHandler(eSpdCellClick);
            this.SS1.CellDoubleClick    += new CellClickEventHandler(eCellDblClick);
            this.SS1.KeyDown            += new KeyEventHandler(eSpdKeyDown);
            
        }

        void eSpdCellClick(object sender, CellClickEventArgs e)
        {
            clsSpread cSpd = new clsSpread();

            if (sender == SS1)
            {
                if (e.ColumnHeader == true)
                {
                    cSpd.setSpdSort(SS1, e.Column, true);
                    return;
                }
            }
        }

        void eSpdKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (SS1_Sheet1.RowCount == 0)
                {
                    return;
                }

                
                string strBuildNo = SS1.ActiveSheet.Cells[SS1_Sheet1.ActiveRowIndex, 8].Text.Trim();

                CHECK_BAS_ZIPS_ROAD(clsDB.DbCon, strBuildNo, SS1_Sheet1.ActiveRowIndex);

                SS1_Sheet1.Cells[0, 0, SS1_Sheet1.RowCount - 1, SS1_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
                SS1_Sheet1.Cells[SS1_Sheet1.ActiveRowIndex, 0, SS1_Sheet1.ActiveRowIndex, SS1_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

                GstrValue = strData[SS1_Sheet1.ActiveRowIndex];

                rSetGstrValue(GstrValue);
                this.Close();
            }
        }

        void CHECK_BAS_ZIPS_ROAD(PsmhDb pDbCon, string ArgBuildNo, int Inx)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            string strJiCode = string.Empty;
            string strDong = string.Empty;
            string strDo = string.Empty;
            string strSi = string.Empty;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT ROWID ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ZIPS_ROAD ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND BUILDNO = '" + ArgBuildNo + "' ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (Dt.Rows.Count == 0)
                {
                    strDong = SS1.ActiveSheet.Cells[Inx, 14].Text.Trim();
                    strDo   = SS1.ActiveSheet.Cells[Inx, 12].Text.Trim();
                    strSi   = VB.Pstr(SS1.ActiveSheet.Cells[Inx, 13].Text.Trim(), " ", 1).Trim();

                    strJiCode = Read_Bas_Area(pDbCon, strDong, strDo, strSi);

                    //원내 DB INSERT
                    clsDB.setBeginTran(pDbCon);
                    
                    SQL = "";
                    SQL += " INSERT INTO " + ComNum.DB_PMPA + "BAS_ZIPS_ROAD (                  \r\n";
                    SQL += "        ZIPCODE,ZIPNAME1,ZIPNAME2,ZIPNAME3,ROADCODE,ROADNAME,       \r\n";
                    SQL += "        BUN1,BUN2,BUILDNO,BUILDNAME,DONGCODE,                       \r\n";
                    SQL += "        DONGNAME,RINAME,SAN,JIBUN1,DONGSEQNO,                       \r\n";
                    SQL += "        JIBUN2,MAILJIYEK )                                          \r\n";
                    SQL += " VALUES                                                             \r\n";
                    SQL += "       ('" + SS1.ActiveSheet.Cells[Inx, 5].Text.Trim() + "'         \r\n";      //ZIPCODE
                    SQL += "       ,'" + SS1.ActiveSheet.Cells[Inx, 12].Text.Trim() + "'        \r\n";      //ZIPNAME1
                    SQL += "       ,'" + SS1.ActiveSheet.Cells[Inx, 13].Text.Trim() + "'        \r\n";      //ZIPNAME2
                    SQL += "       ,'" + SS1.ActiveSheet.Cells[Inx, 15].Text.Trim() + "'        \r\n";      //ZIPNAME3
                    SQL += "       ,'" + SS1.ActiveSheet.Cells[Inx, 7].Text.Trim() + "'         \r\n";      //ROADCODE
                    SQL += "       ,'" + SS1.ActiveSheet.Cells[Inx, 16].Text.Trim() + "'        \r\n";      //ROADNAME
                    SQL += "       ,'" + SS1.ActiveSheet.Cells[Inx, 18].Text.Trim() + "'        \r\n";      //BUN1
                    SQL += "       ,'" + SS1.ActiveSheet.Cells[Inx, 19].Text.Trim() + "'        \r\n";      //BUN2
                    SQL += "       ,'" + SS1.ActiveSheet.Cells[Inx, 8].Text.Trim() + "'         \r\n";      //BUILDNO
                    SQL += "       ,'" + SS1.ActiveSheet.Cells[Inx, 10].Text.Trim() + "'        \r\n";      //BUILDNAME
                    SQL += "       ,'" + SS1.ActiveSheet.Cells[Inx, 6].Text.Trim() + "'         \r\n";      //DONGCODE
                    SQL += "       ,'" + SS1.ActiveSheet.Cells[Inx, 14].Text.Trim() + "'        \r\n";      //DONGNAME
                    SQL += "       ,'" + SS1.ActiveSheet.Cells[Inx, 15].Text.Trim() + "'        \r\n";      //RINAME
                    SQL += "       ,'" + SS1.ActiveSheet.Cells[Inx, 20].Text.Trim() + "'        \r\n";      //SAN
                    SQL += "       ,'" + SS1.ActiveSheet.Cells[Inx, 21].Text.Trim() + "'        \r\n";      //JIBUN1
                    SQL += "       ,'" + SS1.ActiveSheet.Cells[Inx, 23].Text.Trim() + "'        \r\n";      //DONGSEQNO
                    SQL += "       ,'" + SS1.ActiveSheet.Cells[Inx, 22].Text.Trim() + "'        \r\n";      //JIBUN2
                    SQL += "       ,'" + strJiCode + "')                                        \r\n";      //MAILJIYEK

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(pDbCon);
                }

                Dt.Dispose();
                Dt = null;
                
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        void eCellDblClick(object sender, CellClickEventArgs e)
        {
            if (SS1_Sheet1.RowCount == 0)
            {
                return;
            }
            
            string strBuildNo = SS1.ActiveSheet.Cells[e.Row, 8].Text.Trim();

            CHECK_BAS_ZIPS_ROAD(clsDB.DbCon, strBuildNo, e.Row);

            GstrValue = strData[e.Row];
            rSetGstrValue(GstrValue);
            this.Close();
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                Screen_Clear();
                currentPage = "1";
                keyword = txtSearch.Text.Trim();
                Search_Data(keyword);
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Clear();
                currentPage = "1";
                keyword = txtSearch.Text.Trim();
                Search_Data(keyword);
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnNext)
            {
                currentPage = (Convert.ToInt32(currentPage) + 1).ToString();
                Search_Data(keyword);
            }
            else if (sender == btnPrev)
            {
                currentPage = (Convert.ToInt32(currentPage) - 1).ToString();
                Search_Data(keyword);
            }

        }

        void Search_Data(string keyword)
        {
            int nCNT = 0;
            int nTotal = 0;
            int nPPCnt = 0;
            int nResult = 0;
            int i = 0;

            string strZipName = string.Empty;
            string strShow = string.Empty;
            string strJiCode = string.Empty;
            string strDong = string.Empty;
            string strDo = string.Empty;
            string strSi = string.Empty;

            btnPrev.Enabled = false;
            btnNext.Enabled = false;

            SS1.ActiveSheet.RowCount = 0;

            Array.Clear(strData, 0, strData.Length);

            try
            {
                Control[] controls = ComFunc.GetAllControls(this);

                foreach (Control ctl in controls)
                {
                    if (ctl is RadioButton)
                    {
                        if (VB.Left(((RadioButton)ctl).Name, 10) == "rdoZipName")
                        {
                            if (((RadioButton)ctl).Checked == true)
                            {
                                strZipName = ((RadioButton)ctl).Text;
                                break;
                            }
                        }
                    }
                }

                if (strZipName == "전체")
                {
                    strZipName = "";
                }

                keyword = strZipName + " " + keyword;

                apiurl = "http://www.juso.go.kr/addrlink/addrLinkApi.do?currentPage=" + currentPage + "&countPerPage=" + countPerPage + "&keyword=" + keyword.Trim() + "&confmKey=" + confmKey;

                WebClient wc    = new WebClient();
                XmlReader read  = new XmlTextReader(wc.OpenRead(apiurl));
                DataSet ds      = new DataSet();
                DataSet dsTemp  = new DataSet();

                ds.ReadXml(read);

                DataRow[] rows = ds.Tables[0].Select();

                lblTotalCount.Text = rows[0]["totalCount"].ToString() + " 건";

                if (rows[0]["totalCount"].ToString() != "0")
                {
                    #region 건수가 있을 경우
                    
                    SS1.DataSource = ds.Tables[1];
                    
                    nTotal = Convert.ToInt32(rows[0]["totalCount"].ToString());
                    nCNT = Convert.ToInt32(rows[0]["currentPage"].ToString());
                    nPPCnt = Convert.ToInt32(rows[0]["countPerPage"].ToString());
                    nResult = nTotal / 100 + 1;

                    if (rows[0]["currentPage"].ToString() == "1")
                    {
                        if (nCNT < nResult)
                        {
                            btnNext.Enabled = true;
                        }
                    }
                    else
                    {
                        if ((nCNT * nPPCnt) > nTotal)
                        {
                            btnPrev.Enabled = true;
                        }
                        else
                        {
                            btnPrev.Enabled = true;
                            btnNext.Enabled = true;
                        }

                    }

                    lblPage.Text = rows[0]["currentPage"].ToString() + "/" + nResult.ToString() + " Page";
                    #endregion
                }
                else
                {
                    #region 건수가 없을 경우
                    SS1.DataSource = null;

                    if (rows[0]["errorMessage"].ToString() != "정상")
                    {
                        MessageBox.Show(rows[0]["errorMessage"].ToString());
                    }
                    else
                    {
                        if (rows[0]["totalCount"].ToString() == "0")
                        {
                            MessageBox.Show("검색된 자료가 없습니다.");
                        }
                    }
                    #endregion
                }

                setSpdWidth(SS1);

                for (i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    strDong = SS1.ActiveSheet.Cells[i, 14].Text.Trim();
                    strDo = SS1.ActiveSheet.Cells[i, 12].Text.Trim();
                    strSi = VB.Pstr(SS1.ActiveSheet.Cells[i, 13].Text.Trim(), " ", 1).Trim();

                    strJiCode = Read_Bas_Area(clsDB.DbCon, strDong, strDo, strSi);

                    strShow = "";
                    strShow = SS1.ActiveSheet.Cells[i, 5].Text.Trim() + "|";                //1 우편번호
                    strShow = strShow + SS1.ActiveSheet.Cells[i, 1].Text.Trim() + " ";      //2 도로명주소
                    strShow = strShow + SS1.ActiveSheet.Cells[i, 10].Text.Trim() + "|";     //2 건물명
                    strShow = strShow + strJiCode + "|";                                    //3 지역코드
                    strShow = strShow + SS1.ActiveSheet.Cells[i, 7].Text.Trim() + "|";      //4 도로명코드
                    strShow = strShow + SS1.ActiveSheet.Cells[i, 8].Text.Trim();            //5 건물번호

                    strData[i] = strShow;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        string Read_Bas_Area(PsmhDb pDbCon, string ArgDong, string ArgDo, string ArgSi)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string rtnVal = "";

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT JICODE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_AREA ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND (JINAME LIKE '%" + ArgDong + "%' OR JINAME LIKE '%" + ArgDo + "%' OR JINAME LIKE '%" + ArgSi + "%')";
                SQL += ComNum.VBLF + "  ORDER BY JICODE ";
                SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "82";
                }

                if (Dt.Rows.Count > 0)
                {
                    rtnVal = Dt.Rows[0]["JICODE"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return "82";
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            Screen_Clear();

            setSpd(SS1);

            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("인터넷 연결에 실패하였습니다.", "도로명주소 검색불가");
                this.Close();

                frmSearchRoadAdd f = new frmSearchRoadAdd();
                f.ShowDialog();
                return;
            }

            txtSearch.Select();
        }
        
        void Screen_Clear()
        {
            lblTotalCount.Text = "0 건";
            lblPage.Text = "";
            btnPrev.Enabled = false;
            btnNext.Enabled = false;

            SS1.ActiveSheet.RowCount = 0;

        }
        
    }
}
