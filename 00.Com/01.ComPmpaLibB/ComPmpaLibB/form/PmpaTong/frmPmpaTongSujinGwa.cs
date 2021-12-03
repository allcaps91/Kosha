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
    /// File Name       : frmPmpaTongSujinGwa.cs
    /// Description     : 수진자 과별 인원 현황
    /// Author          : 안정수
    /// Create Date     : 2017-08-19
    /// Update History  :  
    /// <history>       
    /// 본 소스(VB)에서는 데이터가 안나옴... 사용여부 확인 필요 함
    /// d:\psmh\OPD\oviewa\OVIEWA02_new.FRM(FrmSuJinGwa_new) => frmPmpaTongSujinGwa.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oviewa\OVIEWA02_new.FRM(FrmSuJinGwa_new)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaTongSujinGwa : Form
    {
        string[] strGwaName = new string[40];
        string dateFlag = "";

        int[,] nGwaNum = new int[15, 40];
        int[,] nGwaPay = new int[15, 40];

        string mstrJobPart = "";

        public frmPmpaTongSujinGwa()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaTongSujinGwa(string GstrJobPart)
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
            dtpDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

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
                    for (i = 0; i < dt.Rows.Count; i++)
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
                SSJubSuMove();
            }
            else
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
                SSJubSuMove();
            }
        }

        void SSJubSuBuild()
        {
            int i = 0;           

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

            
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                ";
            SQL += ComNum.VBLF + "  PrintRanking, DeptNameK, Bi,                                                        ";
            SQL += ComNum.VBLF + "  Chojae, COUNT(Pano) CNT, nvl(SUM(AMT7),0) SM                                               ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, " + ComNum.DB_PMPA + "BAS_CLINICDEPT B       ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                             ";
            SQL += ComNum.VBLF + "      AND ActDate = TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')                      ";            
            SQL += ComNum.VBLF + "      AND Jin NOT IN ('D')                                                         ";
            if (txtPart.Text != "")
            {
                SQL += ComNum.VBLF + "      AND Part = '" + txtPart.Text + "'                                           ";
            }
            SQL += ComNum.VBLF + "      AND A.DeptCode = B.DeptCode                                                     ";
            SQL += ComNum.VBLF + "GROUP BY PrintRanking, DeptNameK, Bi, Chojae order by PrintRanking                                          ";

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
                        strNewDept = strDept;
                        if (strOldDept.Trim() != strNewDept.Trim())
                        {
                            nRank = nRank;
                            strOldDept = strNewDept;
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

        void SET_Subscript_ADD(string strBi, string strDept, int nRank, int ncho, int nCount, int nSum)
        {
            int nCol = 0;
            int nRow = 0;
            int nBi = 0;

            if (nRank > 27)
            {
                nRank = 28;
            }

            nRow = nRank - 1;

            if (ncho == 1 || ncho == 2 || ncho == 5)
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

            else if (ncho == 3 || ncho == 4 || ncho == 6 || ncho == 0)
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

            if (ncho == 1 || ncho == 2 || ncho == 3 || ncho == 4 || ncho == 5 || ncho == 6 || ncho == 0)
            {
                strGwaName[nRow] = strDept;
                
                //인원수
                nGwaNum[nCol, nRow] += nCount;
                nGwaNum[nCol, 28] += nCount;

                if (ncho == 1 || ncho == 2 || ncho == 5)
                {
                    nGwaNum[6, nRow] += nCount;
                    nGwaNum[6, 28] += nCount;
                }
                else if (ncho == 3 || ncho == 4 || ncho == 6 || ncho == 0)
                {
                    nGwaNum[13, nRow] += nCount;
                    nGwaNum[13, 28] += nCount;
                }

                nGwaNum[14, nRow] += nCount;
                nGwaNum[14, 28] += nCount;

                //금액
                nGwaPay[nCol, nRow] += nSum;
                nGwaPay[nCol, 28] += nSum;

                if (ncho == 1 || ncho == 2 || ncho == 5)
                {
                    nGwaPay[6, nRow] += nSum;
                    nGwaPay[6, 28] += nSum;
                }
                else if (ncho == 3 || ncho == 4 || ncho == 6 || ncho == 0)
                {
                    nGwaPay[13, nRow] += nSum;
                    nGwaPay[13, 28] += nSum;
                }

                nGwaPay[14, nRow] += nSum;
                nGwaPay[14, 28] += nSum;
            }

        }

        void SSJubSuClear()
        {
            int i = 0;
            int j = 0;

            //ssList_Sheet1.Rows.Count = 0;

            for (i = 0; i <= 14; i++)
            {
                for (j = 0; j <= 39; j++)
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

            for (i = 0; i <= 28; i++)
            {
                ssList_Sheet1.Cells[i + 2, 1].Text = strGwaName[i];
                if(i == 28)
                {
                    ssList_Sheet1.Cells[i + 2, 1].Text = "합계";
                }

                ssList_Sheet1.Cells[i + 31, 1].Text = strGwaName[i];
                if (i == 28)
                {
                    ssList_Sheet1.Cells[i + 31, 1].Text = "합계";
                }
            }

            for (i = 0; i <= 14; i++)
            {
                for (j = 0; j <= 28; j++)
                {
                    ssList_Sheet1.Cells[j + 2, i + 2].Text = String.Format("{0:#,##0}", nGwaNum[i, j]);
                }
            }

            for (i = 0; i <= 14; i++)
            {
                for (j = 0; j <= 28; j++)
                {
                    ssList_Sheet1.Cells[j + 31, i + 2].Text = String.Format("{0:#,##0}", nGwaPay[i, j]);
                }

            }

            dtpDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        void txtPart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                txtPart.Text = txtPart.Text.ToUpper();                    
            }
        }
    }
}

