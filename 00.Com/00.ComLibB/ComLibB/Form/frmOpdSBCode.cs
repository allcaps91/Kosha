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
    /// File Name       : frmOpdSBCode.cs
    /// Description     : 외래환자 상황 별 조회하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-05
    /// Update History  : try-catch문 수정
    /// <history>       
    /// D:\타병원\PSMHH\mid\midout\Frm외래상병별환자조회.frm(Frm외래상병별환자조회) => frmOpdSBCode.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\mid\midout\Frm외래상병별환자조회.frm(Frm외래상병별환자조회)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\mid\midout\midout.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmOpdSBCode : Form
    {
        ComFunc CF = new ComFunc();

        public frmOpdSBCode()
        {
            InitializeComponent();          
        }         
               
        void frmOpdSBCode_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtDept.Text = "";
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnClear_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < ssSBList_Sheet1.RowCount; i++)
            {
                for(int j = 0; j < ssSBList_Sheet1.ColumnCount; j++)
                {
                    ssSBList_Sheet1.Cells[i, j].Text = "";
                }
            }
        }

        void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strFont3 = "";

            strFont1 = "/c/fb0/fn\"바탕체\" /fz\"20\" /fu1";
            strFont2 = "/r";
            strFont3 = "/n/fn\"System\" /fb0/fu0/fz\" 15";
                     
            ssOPD_Sheet1.PrintInfo.AbortMessage = "의무기록 자료를 인쇄중 입니다";
            ssOPD_Sheet1.PrintInfo.Header = strFont1 + "[ 의 무 기 록  자 료 ]" + strFont3;
            ssOPD_Sheet1.PrintInfo.Header += strFont2;
            ssOPD_Sheet1.PrintInfo.Header += "출력일자 : " + DateTime.Now.ToString("yyyy-MM-dd") + DateTime.Now.ToString("hh:mm:ss") + "        ";

            ssOPD_Sheet1.PrintInfo.JobName = "인쇄 : (의무기록 자료)";

            ssOPD_Sheet1.PrintInfo.Margin.Top = 50;
            ssOPD_Sheet1.PrintInfo.Margin.Bottom = 2000;
            ssOPD_Sheet1.PrintInfo.Margin.Left = 0;
            ssOPD_Sheet1.PrintInfo.Margin.Right = 0;

            ssOPD_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssOPD_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;

            ssOPD_Sheet1.PrintInfo.ShowBorder = true;
            ssOPD_Sheet1.PrintInfo.ShowColor = false;
            ssOPD_Sheet1.PrintInfo.ShowGrid = true;
            ssOPD_Sheet1.PrintInfo.ShowShadows = true;
            ssOPD_Sheet1.PrintInfo.UseMax = false;

            ssOPD_Sheet1.PrintInfo.PageEnd = 4;
            ssOPD_Sheet1.PrintInfo.PageStart = 2;

            ssOPD_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssOPD_Sheet1.PrintInfo.Preview = true;
            ssOPD.PrintSheet(0);
        }

        void btnView_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인            

            GetData();
        }
        void GetData()
        {
            int i = 0;

            string strPano = "";
            string strOutDate = "";
            string strNewData = "";
            string strOldData = "";
            string strIllCode = "";
            string SQL = "";
            string SqlErr = "";

            int nCnt1 = 0;

            DataTable dt = null;
            DataTable dt2 = null;

            SS_Clear(ssOPD_Sheet1);

            for (i = 0; i < ssSBList_Sheet1.RowCount; i++)
            {
                if (ssSBList_Sheet1.Cells[i, 0].Text != "")
                {
                    strIllCode = strIllCode + VB.IIf(strIllCode == "", "'" + ssSBList_Sheet1.Cells[i, 0].Text + "'", ",'" + ssSBList_Sheet1.Cells[i, 0].Text + "'");
                }
            }

            if (strIllCode == "")
            {
                if (MessageBox.Show("상병코드가 없습니다...전체 조회시 시간이 많이 걸립니다.." + ComNum.VBLF + ComNum.VBLF + "상병코드없이 조회하시겠습니까??", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT a.PTNO,a.DEPTCODE,a.BDATE,a.ILLCODE,b.SName,b.Sex,b.Age,b.Bi,b.JiCode";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OILLS a, " + ComNum.DB_PMPA + "OPD_MASTER b";
                SQL += ComNum.VBLF + "WHERE a.Ptno=b.Pano";
                SQL += ComNum.VBLF + "  AND a.DeptCode=b.DeptCode";
                SQL += ComNum.VBLF + "  AND a.BDate =b.BDate";
                SQL += ComNum.VBLF + "  AND a.BDATE >=TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  AND a.BDATE <=TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD')";
                if (strIllCode != "")
                {
                    SQL += ComNum.VBLF + "  AND a.ILLCODE IN ( " + strIllCode + " ) ";
                }
                if (txtDept.Text.Trim() != " AND a.DeptCode  ='" + txtDept.Text + "' ")
                {
                    SQL += ComNum.VBLF + "";
                }
                SQL += ComNum.VBLF + "UNION ALL";
                SQL += ComNum.VBLF + "  SELECT a.PTNO,a.DEPTCODE,a.BDATE,a.ILLCODE,b.SName,b.Sex,b.Age,b.Bi,b.JiCode ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_EILLS a, " + ComNum.DB_PMPA + "OPD_MASTER b";
                SQL += ComNum.VBLF + "  WHERE a.Ptno=b.Pano ";
                SQL += ComNum.VBLF + "      AND a.DeptCode=b.DeptCode ";
                SQL += ComNum.VBLF + "      AND a.BDate =b.BDate ";
                SQL += ComNum.VBLF + "      AND a.BDATE >=TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND a.BDATE <=TO_DATE('" + dtpTdate.Text + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "      AND a.DeptCode  ='ER' ";
                if (strIllCode != "")
                {
                    SQL += ComNum.VBLF + "      AND a.ILLCODE IN ( " + strIllCode + " ) ";
                }

                SQL += ComNum.VBLF + "ORDER BY BDATE,Ptno";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                SqlErr = "";
                ssOPD_Sheet1.RowCount = dt.Rows.Count;
                for (i = 0; i < ssOPD_Sheet1.RowCount; i++)
                {
                    strNewData = dt.Rows[i]["Ptno"].ToString().Trim() + dt.Rows[i]["DeptCode"].ToString().Trim() + dt.Rows[i]["BDATE"].ToString().Trim();

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT B.ZIPCODE1|| '-' || B.ZIPCODE2 ZIPCODE, TEL,HPHONE,B.JUSO || ' ' || B.JUSO2 JUSO,Jumin1 ||'-' || Jumin2 Jumin ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT A, " + ComNum.DB_PMPA + "VIEW_PATIENT_JUSO B";
                    SQL += ComNum.VBLF + "  WHERE A.PANO ='" + dt.Rows[i]["Ptno"].ToString().Trim() + "' ";
                    SQL += ComNum.VBLF + "      AND A.PANO = B.PANO";
                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (strNewData != strOldData)
                    {
                        ssOPD_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Ptno"].ToString().Trim();
                        ssOPD_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BDATE"].ToString().Trim();

                        ssOPD_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssOPD_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssOPD_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssOPD_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Age"].ToString().Trim();
                        ssOPD_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Bi"].ToString().Trim();
                        ssOPD_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ILLCODE"].ToString().Trim();
                        ssOPD_Sheet1.Cells[i, 9].Text = dt.Rows[i]["JiCode"].ToString().Trim();

                        ssOPD_Sheet1.Cells[i, 8].Text = dt2.Rows[0]["ZipCode"].ToString().Trim();
                        ssOPD_Sheet1.Cells[i, 10].Text = dt2.Rows[0]["Tel"].ToString().Trim();
                        ssOPD_Sheet1.Cells[i, 11].Text = dt2.Rows[0]["Hphone"].ToString().Trim();
                        
                        ssOPD_Sheet1.Cells[i, 12].Text = CF.READ_BAS_Mail(clsDB.DbCon, dt2.Rows[0]["ZIPCODE"].ToString().Trim()) + " " + dt2.Rows[0]["JUSO"].ToString().Trim();
                        ssOPD_Sheet1.Cells[i, 13].Text = dt2.Rows[0]["Jumin"].ToString().Trim();
                    }

                    else
                    {
                        ssOPD_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssOPD_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssOPD_Sheet1.Cells[i, 5].Text = dt.Rows[i]["Age"].ToString().Trim();
                        ssOPD_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Bi"].ToString().Trim();
                        ssOPD_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ILLCODE"].ToString().Trim();

                    }

                    dt2.Dispose();
                    dt2 = null;

                    strOldData = dt.Rows[i]["Ptno"].ToString().Trim() + dt.Rows[i]["DeptCode"].ToString().Trim() + dt.Rows[i]["BDATE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        void SS_Clear(FarPoint.Win.Spread.SheetView Spd)
        {
            for(int i = 0; i < Spd.RowCount;i++)
            {
                for(int j = 0; j < Spd.ColumnCount; j++)
                {
                    Spd.Cells[i, j].Text = "";
                }
            }
        }

        void txtDept_Leave(object sender, EventArgs e)
        {
            txtDept.Text = txtDept.Text.Trim().ToUpper();
        }

        void txtDept_Click(object sender, EventArgs e)
        {
            txtDept.ImeMode = ImeMode.Alpha;
        }
    }
}
