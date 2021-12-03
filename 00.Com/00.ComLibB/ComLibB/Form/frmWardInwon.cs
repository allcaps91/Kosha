using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmWardInwon.cs
    /// Description     : 통합간호간병 과별 입원 인원 통계 조회하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06-14
    /// Update History  : 
    /// <history>       
    /// D:\타병원\PSMHH\IPD\iviewa\IVIEWI.frm(FrmWardInwon) => frmOcsDoctor.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\IPD\iviewa\IVIEWI.frm(FrmWardInwon)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\IPD\iviewa\iviewa.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmWardInwon : Form
    {
        ComFunc CF = new ComFunc();
        public frmWardInwon()
        {
            InitializeComponent();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string strFont2 = "";

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb1 /fi0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb1 /fi0 /fk0 /fs2";

            strHead1 = "/f1/l" + VB.Space(34) +" *** 통합간호간병 병동 과별 인원 통계 ***" + "/n";
            strHead2 = "/f2/l" + "작업년도 : " + cboYear.SelectedItem.ToString();
            strHead2 += VB.Space(130) + "단위 : 명 ";

            ssList_Sheet1.PrintInfo.Header = strHead1 + strFont1 + "/n" + strFont2 + strHead2;

            ssList_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;

            ssList_Sheet1.PrintInfo.Margin.Top = 50;
            ssList_Sheet1.PrintInfo.Margin.Bottom = 2000;
            ssList_Sheet1.PrintInfo.Margin.Left = 50;
            ssList_Sheet1.PrintInfo.Margin.Right = 0;

            ssList_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssList_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;

            ssList_Sheet1.PrintInfo.ShowBorder = true;
            ssList_Sheet1.PrintInfo.ShowColor = false;
            ssList_Sheet1.PrintInfo.ShowGrid = true;
            ssList_Sheet1.PrintInfo.ShowShadows = true;
            ssList_Sheet1.PrintInfo.UseMax = false;

            ssList_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssList_Sheet1.PrintInfo.Preview = true;
            ssList.PrintSheet(0);

            //TODO
            // adoodbc.bas SQL_LOG 함수 구현필요
            // SQL_LOG("", ssList_Sheet1.PrintInfo.Header);
        }

        void frmWardInwon_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            SetCombo();
        }

        void SetCombo()
        {
            int i = 0;
            int nYY = 0;
            int nMM = 0;                 

            cboYear.Items.Clear();

            nYY = Convert.ToInt16(VB.Left(DateTime.Now.ToString("yyyy-MM-dd"), 4));
            nMM = Convert.ToInt16(ComFunc.SetAutoZero(VB.Mid(DateTime.Now.ToString("yyyy-MM-dd"), 6, 2), 2));

            for(i = 0; i < 36; i++)
            {
                cboYear.Items.Add(nYY + "-" + ComFunc.vbFormat(Convert.ToString(nMM), "00"));
                nMM -= 1;

                if(nMM == 0)
                {
                    nMM = 12;
                    nYY -= 1;
                }
            }
            cboYear.SelectedIndex = 0;

            btnPrint.Enabled = false;
        
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnView_Click(object sender, EventArgs e)
        {
            int i, j, k, nDay, nREAD;
            int nCNT = 0;
            int nRow = 0;
            int nRow2 = 0;

            string strFDate = "";
            string strTDate = "";
            string strOldData = "";
            string strNewData = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssList_Sheet1.RowCount = 0;

            btnView.Enabled = false;
            btnPrint.Enabled = false;

            SS_Clear();

            strFDate = VB.Left(cboYear.SelectedItem.ToString(), 7) + "-01";
            //strTDate = CF.READ_LASTDAY(strFDate);
            strTDate = DateTime.Parse(strFDate).AddDays(-1).ToString("yyyy-MM-dd");


            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  DECODE(a.Bi,'11','1','12','1','13','1','21','2','22','2','3') AS aBi, ";
            SQL += ComNum.VBLF + "  TO_CHAR(a.JobDate,'YYYY-MM-DD') JobDate, ";
            SQL += ComNum.VBLF + "  a.PaClass,COUNT(a.Pano) CPano";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "TONG_Patient a, " + ComNum.DB_PMPA + "BAS_CLINICDEPT b";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "  AND a.JobDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "  AND a.JobDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  AND a.T_CARE = 'Y' ";
            SQL += ComNum.VBLF + "  AND a.PaClass IN ('2','3','4','5','6') ";
            SQL += ComNum.VBLF + "  AND a.DeptCode=b.DeptCode";
            SQL += ComNum.VBLF + "GROUP BY DECODE(a.Bi,'11','1','12','1','13','1','21','2','22','2','3'), a.JobDate, a.PaClass ";
            SQL += ComNum.VBLF + "ORDER BY a.JobDate, 1, a.PaClass";
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

            nRow = 0;
            nRow2 = 0;
            strOldData = "";
            nREAD = dt.Rows.Count;
            SqlErr = "";

            ssList_Sheet1.RowCount = Convert.ToInt16(VB.Right(strTDate, 2)) * 4;


            for(i = 0; i < nREAD; i++)
            {
                strNewData = dt.Rows[i]["JobDate"].ToString().Trim();

                if(strOldData != strNewData)
                {
                    if(strOldData != "")
                    {
                        for(j = 2; j < 7; j++)
                        {
                            nDay = 0;
                            
                            for (k = nRow; k < nRow + 3; k++)
                            {
                                nDay = nDay + Convert.ToInt16(ssList_Sheet1.Cells[k, j].Text);                                
                            }

                            ssList_Sheet1.Cells[nRow, j].Text = Convert.ToString(nDay);
                        }
                    }

                    nRow = nCNT * 4 ;

                    if (nRow > ssList_Sheet1.RowCount)
                    {
                        ssList_Sheet1.RowCount = nRow;

                    }

                    ssList_Sheet1.Cells[nRow, 0].Text = strNewData;
                    strOldData = strNewData;

                    nRow2 = 0;

                    for (j = 0; j < 4; j++)
                    {
                        nRow2 += 1;

                        if (nRow + nRow2 - 1 > ssList_Sheet1.RowCount)
                        {
                            ssList_Sheet1.RowCount = nRow + nRow2 - 1;
                        }

                        switch (j)
                        {
                            case 0:
                                ssList_Sheet1.Cells[nRow + nRow2 - 1, 1].Text = "합　　계";
                                break;
                            case 1:
                                ssList_Sheet1.Cells[nRow + nRow2 - 1, 1].Text = "건강보험";
                                break;
                            case 2:
                                ssList_Sheet1.Cells[nRow + nRow2 - 1, 1].Text = "의료급여";
                                break;
                            case 3:
                                ssList_Sheet1.Cells[nRow + nRow2 - 1, 1].Text = "기　　타";
                                break;
                            default:
                                break;
                        }

                        for(k = 2; k < 7; k++)
                        {
                            //0으로 초기화한다
                            ssList_Sheet1.Cells[nRow + nRow2 -1, k].Text = "0";
                        }
                        
                    }

                    nCNT += 1;

                    int n = 0;
                    for (n = 0; n < Convert.ToInt16((VB.Right(strTDate, 2))); n++)
                    {
                        ssList_Sheet1.AddSpanCell(n * 4, 0, 4, 1);
                    }
                }

                int z = 0; // Row
                int x = 0; // Col
                
                switch (dt.Rows[i]["aBi"].ToString().Trim())
                {
                    case "1":
                        z = nRow + 1; //건강보험
                        break;
                    case "2":
                        z = nRow + 2; //의료급여
                        break;
                    case "3":
                        z = nRow + 3; //기타
                        break;
                }

                switch (dt.Rows[i]["PaClass"].ToString().Trim())
                {
                    case "2":
                        x = 3;          //입원자
                        break;
                    case "3":
                        x = 2;          //재원자
                        break;
                    case "4":
                        x = 4;          //퇴원자
                        break;
                    case "5":
                        x = 4;          //전출
                        break;
                    case "6":          
                        x = 3;          //전입
                        break;

                }
                ssList_Sheet1.Cells[z, x].Text = (Convert.ToUInt32( ssList_Sheet1.Cells[z, x].Text ) + 
                                                    Convert.ToUInt32(dt.Rows[i]["CPano"].ToString().Trim())).ToString();
            }

            dt.Dispose();
            dt = null;
            SqlErr = "";

            for(j = 2; j < 6; j++)
            {
                nDay = 0;

                // SS.Col = j
                for (k = nRow; k < nRow + 3; k++)
                {
                    //SS.Row = K: nDay = nDay + Val(SS.Text)
                    nDay = nDay + Convert.ToInt16(ssList_Sheet1.Cells[k, j].Text);
                }
                ssList_Sheet1.Cells[nRow, j].Text = Convert.ToString(nDay);
            }

            for( i = 0; i < ssList_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
            {
                nDay = 0;
                //SS.Row = i
                for ( j= 2; j < 6; j++)
                {
                    //SS.Col = j
                    nDay = nDay + Convert.ToInt16(ssList_Sheet1.Cells[i, j].Text);
                }
                ssList_Sheet1.Cells[i, 6].Text = String.Format("{0:###,###,##0}", nDay);
            }


            //당일입퇴원자 명수 구함

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  DECODE(a.Bi,'11','1','12','1','13','1','21','2','22','2','3') AS aBi, ";
            SQL += ComNum.VBLF + "  TO_CHAR(a.JobDate,'YYYY-MM-DD') JobDate,";
            SQL += ComNum.VBLF + "  a.PaClass,COUNT(a.Pano) CPano";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "TONG_Patient a, " + ComNum.DB_PMPA + "BAS_CLINICDEPT b";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "  AND a.JobDate >= TO_DATE('" + strFDate + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "  AND a.JobDate <= TO_DATE('" + strTDate + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "  AND a.T_CARE = 'Y'";
            SQL += ComNum.VBLF + "  AND a.INDATE=a.OUTDATE ";
            SQL += ComNum.VBLF + "  AND a.DeptCode=b.DeptCode ";
            SQL += ComNum.VBLF + "GROUP BY DECODE(a.Bi,'11','1','12','1','13','1','21','2','22','2','3'), a.JobDate, a.PaClass";
            SQL += ComNum.VBLF + "ORDER BY a.JobDate, 1, a.PaClass ";
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

            nREAD = 0;
            nREAD = dt.Rows.Count;

            if(nREAD == 0)
            {
                btnView.Enabled = true;
                btnPrint.Enabled = true;
                dt.Dispose();
                dt = null;
                return;
            }

            for(i = 0; i < nREAD; i++)
            {
                for (j = 0; j < ssList_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); j++)
                {
                    //ss.Col = 1, ss.Row = j 
                    // Row -> w
                    if (ssList_Sheet1.Cells[j, 0].Text == dt.Rows[i]["JOBDATE"].ToString().Trim())
                    {
                        int w = 0;
                        switch (dt.Rows[i]["aBi"].ToString().Trim())
                        {
                            case "1":
                                w = j + 1;              //건강보험
                                break;
                            case "2":
                                w = j + 2;              //의료보험
                                break;
                            case "3":
                                w = j + 3;              //기타
                                break;
                        }

                        //당일입퇴원 합계
                        ssList_Sheet1.Cells[w, 5].Text = dt.Rows[i]["CPano"].ToString().Trim();

                        //누계를 위함
                        ssList_Sheet1.Cells[j, 5].Text = Convert.ToString(Convert.ToInt16(ssList_Sheet1.Cells[j, 5].Text) 
                                                    + Convert.ToInt16(ssList_Sheet1.Cells[w, 5].Text));}
                }
            }

            dt.Dispose();
            dt = null;

            btnView.Enabled = true;
            btnPrint.Enabled = true;
        }

        void SS_Clear()
        {
            for(int i = 0; i < ssList_Sheet1.RowCount; i++)
            {
                for(int j = 0; j < ssList_Sheet1.ColumnCount; j++)
                {
                    ssList_Sheet1.Cells[i, j].Text = "";
                }
            }
        }

      
    }
}
