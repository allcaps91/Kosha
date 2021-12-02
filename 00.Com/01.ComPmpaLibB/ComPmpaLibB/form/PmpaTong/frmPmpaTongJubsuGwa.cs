using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaTongJubsuGwa.cs
    /// Description     : 접수자 과별 인원 현황
    /// Author          : 안정수
    /// Create Date     : 2017-07-06
    /// Update History  : 2017-10-25
    /// 조회 부분 수정
    /// <history>       
    /// 본 소스(VB)에서는 데이터가 안나옴... 사용여부 확인 
    /// d:\psmh\OPD\oviewa\OVIEWA01.FRM(FrmJubsuGwa) => frmPmpaTongJubsuGwa.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oviewa\OVIEWA01.FRM(FrmJubsuGwa)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaTongJubsuGwa : Form
    {
        string[] strGwaName = new string[41];
        string dateFlag = "";

        int[,] nGwaNum = new int[15, 41];
        int[,] nGwaPay = new int[15, 41];

        string[] FstrDept = new string[200];
        string mstrJobPart = "";

        public frmPmpaTongJubsuGwa()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaTongJubsuGwa(string GstrJobPart)
        {
            InitializeComponent();
            setEvent();
            mstrJobPart = GstrJobPart;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
        }
        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            //ssList_Sheet1.Rows.Count = 0;

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            Set_Init();
        }

        void Set_Init()
        {
            int i = 0;
            int j = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssList.Visible = false;
            txtPart.Text = mstrJobPart;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                        ";
            SQL += ComNum.VBLF + "  PrintRanking,DeptNameK                      ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT     ";
            SQL += ComNum.VBLF + "WHERE Printranking < 30                       ";
            
            try
            {

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
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        j = Convert.ToInt32(dt.Rows[i]["PrintRanking"].ToString().Trim());
                        strGwaName[j - 1] = dt.Rows[i]["DeptNameK"].ToString().Trim();
                    }
                }

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }
        }

        void ePrint()
        {
            string strHead1 = "";

            string strFont1 = "";

            if (ssList_Sheet1.Rows.Count == 0)
            {
                return;
            }

            btnView.Enabled = false;

            strFont1 = "/fn\"바탕체\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strHead1 = "/l/f1" + VB.Space(14) + dtpDate.Text + "일 응급실 SLIP CHECK-LIST";


            ssList_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n";

            ssList_Sheet1.PrintInfo.Margin.Top = 50;
            ssList_Sheet1.PrintInfo.Margin.Bottom = 2000;
            ssList_Sheet1.PrintInfo.Margin.Left = 500;
            ssList_Sheet1.PrintInfo.Margin.Right = 0;

            ssList_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssList_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;

            ssList_Sheet1.PrintInfo.ShowBorder = true;
            ssList_Sheet1.PrintInfo.ShowColor = false;
            ssList_Sheet1.PrintInfo.ShowGrid = false;
            ssList_Sheet1.PrintInfo.ShowShadows = false;
            ssList_Sheet1.PrintInfo.UseMax = false;
            ssList_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssList_Sheet1.PrintInfo.Preview = true;
            ssList.PrintSheet(0);

            btnView.Enabled = true;
        }

        void eGetData()
        {
            if (String.Compare(dtpDate.Text, DateTime.Now.ToString("yyyy-MM-dd")) < 0)
            {
                if (txtPart.Text == "" || VB.IsNull(txtPart.Text))
                {
                    lblTitleSub0.Text = "전　체" + "     일자: " + dtpDate.Text;
                }
                else
                {
                    lblTitleSub0.Text = txtPart.Text + "조" + "     일자: " + dtpDate.Text;
                }

                ssList.Visible = true;

                SSJubSuClear();
                SSJubSuBuild();
                //SSReservedBackJubSuBuild();
                SSJubSuMove();
            }
            else
            {
                if(txtPart.Text == "" || VB.IsNull(txtPart.Text))
                {
                    lblTitleSub0.Text = "전　체" + "     일자: " + dtpDate.Text;
                }
                else
                {
                    lblTitleSub0.Text = txtPart.Text + "조" + "     일자: " + dtpDate.Text;
                }

                ssList.Visible = true;
                SSJubSuClear();
                SSJubSuBuild();
               // SSReservedJubSuBuild();
                SSJubSuMove();
            }
        }

        void SSJubSuBuild()
        {
            int i = 0;
            int j = 0;
            int k = 0;

            string strBi = "";
            string strDept = "";

            int nRank = 0;
            int ncho = 0;

            int nCount = 0;
            int nSum = 0;

            string strOldDept = "";
            string strNewDept = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            nRank = 0;
            strOldDept = "";
            strNewDept = "";

            for (i = 0; i < FstrDept.Length; i++)
            {
                FstrDept[i] = "";
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                ";
            SQL += ComNum.VBLF + "  PrintRanking, DeptNameK, Bi,                                                        ";
            SQL += ComNum.VBLF + "  Chojae, COUNT(Pano) CNT, SUM(AMT7) SM";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_CLINICDEPT B       ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                             ";
            SQL += ComNum.VBLF + "      AND ActDate = TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')                      ";
            SQL += ComNum.VBLF + "      AND Reserved = '0'                                                              ";
          
            if(txtPart.Text != "")
            {
                SQL += ComNum.VBLF + "      AND Part = '" + txtPart.Text + "'                                           ";
            }
            SQL += ComNum.VBLF + "      AND A.DeptCode = B.DeptCode                                                     ";
            SQL += ComNum.VBLF + "GROUP BY PrintRanking, DeptNameK, Bi, Chojae                                       ";

            try
            {

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
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        strBi = dt.Rows[i]["Bi"].ToString().Trim();
                        strDept = dt.Rows[i]["DeptNamek"].ToString().Trim();
                        ncho = Convert.ToInt32(dt.Rows[i]["Chojae"].ToString().Trim());
                        nRank = Convert.ToInt32(dt.Rows[i]["PrintRanking"].ToString().Trim());
                        nCount = Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                        nSum = Convert.ToInt32(VB.Val(dt.Rows[i]["SM"].ToString().Trim()));
                        strNewDept = strDept;

                        if(strOldDept.Trim() != strNewDept.Trim())
                        {
                            nRank = nRank;
                            FstrDept[nRank] = strNewDept;
                            strOldDept = strNewDept;
                        }

                        SET_Subscript_ADD(strBi, strDept, nRank, ncho, nCount, nSum);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(i.ToString());
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

                            
            
            dt.Dispose();
            dt = null;
        }

        void SET_Subscript_ADD(string strBi, string strDept, int nRank, int ncho, int nCount, int nSum)
        {
            int nCol = 0;
            int nRow = 0;
            int nBi = 0;

            if(nRank > 19)
            {
                nRank = 20;
            }

            nRow = nRank - 1;

            if(ncho == 1 || ncho == 2 || ncho == 5)
            {
                switch (strBi)
                {
                    case "21":
                    case "23":
                    case "24":
                        nCol = 1;
                        break;

                    case "22":
                        nCol = 2;
                        break;

                    case "31":
                    case "32":
                    case "33":
                        nCol = 3;
                        break;

                    case "52":
                        nCol = 4;
                        break;

                    case "51":
                    case "53":
                    case "54":
                    case "55":
                        nCol = 5;
                        break;

                    default:
                        nCol = 0;
                        break;
                }
            }

            else if(ncho == 3 || ncho == 4 || ncho == 6 || ncho == 0)
            {
                switch (strBi)
                {
                    case "21":
                    case "23":
                    case "24":
                        nCol = 8;
                        break;

                    case "22":
                        nCol = 9;
                        break;

                    case "31":
                    case "32":
                    case "33":
                        nCol = 10;
                        break;

                    case "52":
                        nCol = 11;
                        break;

                    case "51":
                    case "53":
                    case "54":
                    case "55":
                        nCol = 12;
                        break;

                    default:
                        nCol = 7;
                        break;
                }
            }

            if(ncho == 1 || ncho == 2 || ncho == 3 || ncho == 4 || ncho == 5 || ncho == 6 || ncho == 0)
            {
                //인원수
                strGwaName[nRow] = strDept;
                nGwaNum[nCol, nRow] += nCount;
                nGwaNum[nCol, 20] += nCount;

                if(ncho == 1 || ncho == 2 || ncho == 5)
                {
                    nGwaNum[6, nRow] += nCount;
                    nGwaNum[6, 20] += nCount;
                }
                else if(ncho == 3 || ncho == 4 || ncho == 6 || ncho == 0)
                {
                    nGwaNum[13, nRow] += nCount;
                    nGwaNum[13, 20] += nCount;
                }

                nGwaNum[14, nRow] += nCount;
                nGwaNum[14, 20] += nCount;

                //금액
                nGwaPay[nCol, nRow] += nSum;
                nGwaPay[nCol, 20] += nSum;

                if(ncho == 1 || ncho == 2 || ncho == 5)
                {
                    nGwaPay[6, nRow] += nSum;
                    nGwaPay[6, 20] += nSum;
                }
                else if(ncho == 3 || ncho == 4 || ncho == 6 || ncho == 0)
                {
                    nGwaPay[13, nRow] += nSum;
                    nGwaPay[13, 20] += nSum;
                }

                nGwaPay[14, nRow] += nSum;
                nGwaPay[14, 20] += nSum;
            }

        }

        void SSJubSuClear()
        {
            int i = 0;
            int j = 0;

            //ssList_Sheet1.Rows.Count = 0;

            for(i = 0; i <= 14; i++)
            {
                for(j = 0; j <= 40; j++)
                {
                    strGwaName[j] = "";
                    nGwaNum[i, j] = 0;
                    nGwaPay[i, j] = 0;
                }
            }
        }

        void SSJubSuMove()
        {
            int i = 0;
            int j = 0;

            for (i = 0; i <= 18; i++)
            {
                ssList_Sheet1.Cells[i + 2, 1].Text = strGwaName[i];
                ssList_Sheet1.Cells[i + 23, 1].Text = strGwaName[i];
            }

            for (i = 0; i <= 14; i++)
            {
                for (j = 0; j <= 20; j++)
                {                    
                    ssList_Sheet1.Cells[j + 2, i + 2].Text = String.Format("{0:#,##0}", nGwaNum[i, j]);
                }
            }

            for (i = 0; i <= 14; i++)
            {
                for (j = 0; j <= 20; j++)
                {                    
                    ssList_Sheet1.Cells[j + 23, i + 2].Text = String.Format("{0:#,##0}", nGwaPay[i, j]);
                }

            }

            dtpDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        void SSReservedBackJubSuBuild()
        {
            int i = 0;
            int j = 0;
            int k = 0;

            string strBi = "";
            string strDept = "";

            int nRank = 0;
            int ncho = 0;

            int nCount = 0;
            int nSum = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                        ";
            SQL += ComNum.VBLF + "  PrintRanking, DeptNameK, Bi,                                                                ";
            SQL += ComNum.VBLF + "  Chojae, COUNT(Pano) CNT, SUM(AMT7) SM                                                       ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED A, " + ComNum.DB_PMPA + "BAS_CLINICDEPT B       ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                     ";
            SQL += ComNum.VBLF + "      AND Date1 = TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')                                ";
            if(txtPart.Text.Trim() != "")
            {
                SQL += ComNum.VBLF + "      AND Part = '" + txtPart.Text + "'                                                   ";
            }
            SQL += ComNum.VBLF + "      AND A.DeptCode = B.DeptCode                                                             "; 
            SQL += ComNum.VBLF + "GROUP BY PrintRanking, DeptNameK, Bi, Chojae                                                  ";
            
            try
            {

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
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        strBi = dt.Rows[i]["Bi"].ToString().Trim();
                        strDept = dt.Rows[i]["DeptNamek"].ToString().Trim();
                        ncho = Convert.ToInt32(dt.Rows[i]["Chojae"].ToString().Trim());
                        nCount = Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                        nSum = Convert.ToInt32(dt.Rows[i]["SM"].ToString().Trim());

                        for(j = 0; j <= 50; j++)
                        {
                            if(strDept == FstrDept[j])
                            {
                                nRank = j;
                                break;
                            }
                        }
                        SET_Subscript_ADD(strBi, strDept, nRank, ncho, nCount, nSum);
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
        }

        void SSReservedJubSuBuild()
        {
            int i = 0;
            int j = 0;
            int k = 0;

            string strBi = "";
            string strDept = "";

            int nRank = 0;
            int ncho = 0;

            int nCount = 0;
            int nSum = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                        ";
            SQL += ComNum.VBLF + "  PrintRanking, DeptNameK, Bi,                                                                ";
            SQL += ComNum.VBLF + "  Chojae, COUNT(Pano) CNT, SUM(AMT7) SM                                                       ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_RESERVED A, " + ComNum.DB_PMPA + "BAS_CLINICDEPT B       ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                     ";
            SQL += ComNum.VBLF + "      AND Date1 = TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')                                ";
            if (txtPart.Text.Trim() != "")
            {
                SQL += ComNum.VBLF + "      AND Part = '" + txtPart.Text + "'                                                   ";
            }
            SQL += ComNum.VBLF + "      AND A.DeptCode = B.DeptCode                                                             ";
            SQL += ComNum.VBLF + "GROUP BY PrintRanking, DeptNameK, Bi, Chojae                                                  ";

            try
            {

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
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strBi = dt.Rows[i]["Bi"].ToString().Trim();
                        strDept = dt.Rows[i]["DeptNamek"].ToString().Trim();
                        nRank = Convert.ToInt32(dt.Rows[i]["PrintRanking"].ToString().Trim());
                        ncho = Convert.ToInt32(dt.Rows[i]["Chojae"].ToString().Trim());
                        nCount = Convert.ToInt32(dt.Rows[i]["CNT"].ToString().Trim());
                        nSum = Convert.ToInt32(dt.Rows[i]["SM"].ToString().Trim());
                        
                        SET_Subscript_ADD(strBi, strDept, nRank, ncho, nCount, nSum);
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
        }
    }
}
