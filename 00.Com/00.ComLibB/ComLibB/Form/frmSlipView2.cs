using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmSlipView2.cs
    /// Description     : 구매과, 공급실 OCS전달 판넬 조회하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-05
    /// Update History  : try-catch문 수정
    ///                 : GstrBuseGbn과 GnJobSabun을 받아오는 생성자 추가
    ///                 : ssSlipView2_Change 이벤트 안 쿼리에 테이블 2개(OCS_SUBCODE, OCS_ORDERCODE) 존재하지 않는다고 오류 발생
    /// <history>       
    /// D:\타병원\PSMHH\gume\guocs\GuOcs03.frm(FrmSlipView) => frmSlipView2.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\gume\guocs\GuOcs03.frm(FrmSlipView)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\gume\guocs\guocs.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmSlipView2 : Form
    {       
        string GstrSysDate = DateTime.Now.ToString("yyyy-MM-dd");
        string GstrSysTime = DateTime.Now.ToString("HH:mm:ss");

        string mstrBuseGbn = "";
        string mJobSabun = "";
        public frmSlipView2()
        {
            InitializeComponent();
        }

        public frmSlipView2(string GstrBuseGbn, string GnJobSabun)
        {
            InitializeComponent();
            mstrBuseGbn = GstrBuseGbn;
            mJobSabun = GnJobSabun;
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void frmSlipView2_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            //5번째 칼럼 이외 수정 불가 설정
            int i = 0;
            for (i = 0; i < ssSlipView2_Sheet1.ColumnCount; i++)
            {
                if (i == 5)
                {
                    i++;
                }
                ssSlipView2_Sheet1.Columns[i].Locked = true;
            }

            GetData();
        }

        void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            int j = 0;          

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt2 = null;

            ssSlipView2_Sheet1.RowCount = 0;      
            ssSlipView2_Sheet1.Columns[7].Visible = false;
            ssSlipView2_Sheet1.Columns[8].Visible = false;

            if(mJobSabun == "4349")
            {
                this.Text += "(전체)";
            }
            else
            {
                if(mstrBuseGbn == "1")
                {
                    this.Text += "(관리과)";
                }
                else if(mstrBuseGbn == "2")
                {
                    this.Text += "(공급실)";
                }
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    SLIPNO, ORDERCODE,GbGume,GbInfo,SUCODE, ORDERNAME, ItemCD,ROWID";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ORDERCODE";                
                if(mJobSabun == "4349")
                {
                    SQL = SQL + ComNum.VBLF + "WHERE GBGUME IN ('1','2') ";
                }
                else if(mstrBuseGbn == "1") // 관리과
                {
                    SQL = SQL + ComNum.VBLF + "WHERE GbGume = '1' ";
                }              
                SQL = SQL + ComNum.VBLF + "WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + " AND GbGume = '2' ";
                SQL = SQL + ComNum.VBLF + " AND SEQNO <>0    ";
                SQL = SQL + ComNum.VBLF + " AND SendDept <> 'N' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SLIPNO, ORDERCODE,GBGUME ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssSlipView2_Sheet1.RowCount = dt.Rows.Count;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssSlipView2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SLIPNO"].ToString().Trim();
                        ssSlipView2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                        ssSlipView2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssSlipView2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                        if (dt.Rows[i]["GbGume"].ToString().Trim() == "1")
                        {
                            ssSlipView2_Sheet1.Cells[i, 4].Text = "관리";
                        }
                        else
                        {
                            ssSlipView2_Sheet1.Cells[i, 4].Text = "공급";
                        }
                        ssSlipView2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["ItemCd"].ToString().Trim();
                        ssSlipView2_Sheet1.Cells[i, 6].Text = READ_GumeName(dt.Rows[i]["ItemCd"].ToString().Trim());
                        ssSlipView2_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssSlipView2_Sheet1.Cells[i, 8].Text = "0";

                        SqlErr = "";
                        if (dt.Rows[i]["GbInfo"].ToString().Trim() == "1")
                        {

                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "SELECT";
                            SQL = SQL + ComNum.VBLF + "    SubName,SuCode,ItemCd,ROWID";
                            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_SUBCODE";
                            SQL = SQL + ComNum.VBLF + "WHERE OrderCode = '" + dt.Rows[i]["OrderCode"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "ORDER BY SeqNo ";
                            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }



                            for (j = 0; j < dt2.Rows.Count; j++)
                            {
                                ssSlipView2_Sheet1.Cells[j, 2].Text = dt2.Rows[j]["SUCODE"].ToString().Trim();
                                ssSlipView2_Sheet1.Cells[j, 3].Text = " ▶" + dt2.Rows[j]["SubName"].ToString().Trim();
                                if (dt.Rows[i]["GbGume"].ToString().Trim() == "1")
                                {
                                    ssSlipView2_Sheet1.Cells[j, 4].Text = "관리";
                                }
                                else
                                {
                                    ssSlipView2_Sheet1.Cells[j, 4].Text = "공급";
                                }
                                ssSlipView2_Sheet1.Cells[j, 5].Text = dt2.Rows[j]["ItemCd"].ToString().Trim();
                                ssSlipView2_Sheet1.Cells[j, 6].Text = READ_GumeName(dt2.Rows[j]["ItemCd"].ToString().Trim());
                                ssSlipView2_Sheet1.Cells[j, 7].Text = dt2.Rows[j]["ROWID"].ToString().Trim();
                                ssSlipView2_Sheet1.Cells[j, 8].Text = "S";
                            }
                            dt2.Dispose();
                            dt2 = null;
                        }
                    }
                }
                dt.Dispose();
                dt = null;
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

        }

        public string READ_GumeName(string str)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            string rtnVal = "";

            if(str == "")
            {
                return "";
            }

            //명칭을 READ
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    CsrName,JepName,Unit";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "ORD_JEP";
                SQL = SQL + ComNum.VBLF + "WHERE JepCode='" + str + "'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                rtnVal = dt.Rows[0]["CsrName"].ToString().Trim();

                if (dt.Rows.Count > 0)
                {
                    if (rtnVal != "")
                    {
                        return rtnVal;
                    }
                    else
                    {
                        rtnVal = dt.Rows[0]["JepName"].ToString().Trim() + " " + dt.Rows[0]["Unit"].ToString().Trim();
                        return rtnVal;
                    }
                }
                else
                    return rtnVal;

                dt.Dispose();
                dt = null;
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return "";
            }

            
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            string strHead1 = "";
            string strHead3 = "";
            string strFont1 = "";
            string strFont2 = "";

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs1";
            strHead1 = "/n/f1" + VB.Space(2) + " OCS전달 판넬" + "/n/n";

            strFont2 = "/fn\"굴림체\" /fz\"11\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead3 = "/n/f2" + VB.Space(5) + " 출력시간 : " + GstrSysDate + GstrSysTime + VB.Space(10) + "Page : " + "/p";


            ssSlipView2_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead3;
            ssSlipView2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssSlipView2_Sheet1.PrintInfo.Margin.Top = 400;
            ssSlipView2_Sheet1.PrintInfo.Margin.Bottom = 200;
            ssSlipView2_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssSlipView2_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssSlipView2_Sheet1.PrintInfo.ShowBorder = true;
            ssSlipView2_Sheet1.PrintInfo.ShowColor = true;
            ssSlipView2_Sheet1.PrintInfo.ShowGrid = false;
            ssSlipView2_Sheet1.PrintInfo.ShowShadows = true;
            ssSlipView2_Sheet1.PrintInfo.UseMax = false;
            ssSlipView2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssSlipView2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssSlipView2_Sheet1.PrintInfo.Preview = true;
            ssSlipView2.PrintSheet(0);
        }

        void ssSlipView2_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            if(e.Column != 5)
            {
                return;
            }

            string strJepCode = "";
            string strTable = "";
            string strGbn = "";
            string strROWID = "";

            string SQL = string.Empty;
            DataTable dt = null;

            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            strJepCode = ssSlipView2_Sheet1.Cells[e.Row, 5].Text;
            try
            {
                if (strJepCode != "")
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT";
                    SQL = SQL + ComNum.VBLF + "    CsrName,JepName,Unit";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "ORD_JEP";
                    SQL = SQL + ComNum.VBLF + "WHERE JepCode='" + strJepCode + "'";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        ComFunc.MsgBox(strJepCode + "관리과 물품코드에 등록 않됨");
                        return;
                    }

                    if (dt.Rows[0]["CsrName"].ToString().Trim() != "")
                    {
                        ssSlipView2_Sheet1.Cells[e.Row, 6].Text = dt.Rows[0]["CsrName"].ToString().Trim();
                    }
                    else
                    {
                        ssSlipView2_Sheet1.Cells[e.Row, 6].Text = dt.Rows[0]["JepName"].ToString().Trim();
                        ssSlipView2_Sheet1.Cells[e.Row, 6].Text += " " + dt.Rows[0]["Unit"].ToString().Trim();
                    }
                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    ssSlipView2_Sheet1.Cells[e.Row, 6].Text = "";
                }

                strROWID = ssSlipView2_Sheet1.Cells[e.Row, 7].Text;
                strGbn = ssSlipView2_Sheet1.Cells[e.Row, 8].Text;
                strTable = "OCS_ORDERCODE";

                if (strGbn == "S")
                {
                    strTable = "OCS_SUBCODE";
                }

                SqlErr = "";

                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE " + strTable + " SET ITEMCD = '" + strJepCode + "' ";
                SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count != 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("OCS판넬에 자료를 등록중 오류가 발생함");
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("수정하였습니다.");
                Cursor.Current = Cursors.Default;
            }

            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
