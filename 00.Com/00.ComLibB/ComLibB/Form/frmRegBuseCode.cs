using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// Class Name      : ComLibB.dll
    /// File Name       : frmRegBuseCode.cs
    /// Description     : 부서코드등록 페이지
    /// Author          : 박성완
    /// Create Date     : 2017-06-08
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// VB\PSMHH\basic\bucode\Frm부서코드_등록.frm => frmRegBuseCode.cs 으로 변경함
    /// 2017-06-30 트리뷰 닷넷바 advtree로 수정 미비한 함수 수정
    /// </history>
    /// <vbp>
    /// default : VB\PSMHH\basic\bucode\Frm부서코드_등록.frm
    /// </vbp>

    public partial class frmRegBuseCode : Form
    {

        public frmRegBuseCode()
        {
            InitializeComponent();

            SetEvent();
        }

        #region 메소드

        void Read_Tree()
        {
            Image imgClose = imageList1.Images[0];
            Image imgOpen = imageList1.Images[1];

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string pName = ""; //부모 노드의 name
            DataTable dt = null;

            DevComponents.AdvTree.Node root = new DevComponents.AdvTree.Node();

            root.Expanded = true;
            root.Name = "NODE10";
            root.Text = "포항성모병원";
            root.Image = imgClose;
            root.ImageExpanded = imgOpen;
            trvBuse.Nodes.Clear();
            trvBuse.Nodes.Add(root);

            Cursor.Current = Cursors.WaitCursor;

            SQL = " SELECT DEPT_LEVEL, BUCODE, DEPT_ID, NAME, DEPT_ID_UP, DELDATE, GBABC, BUSE1 ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_BUSE  ";
            SQL = SQL + ComNum.VBLF + " ORDER BY DEPT_LEVEL, DEPT_SORT ";

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


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DevComponents.AdvTree.Node cNode = new DevComponents.AdvTree.Node();

                cNode.Name = "NODE" + dt.Rows[i]["DEPT_ID"].ToString().Trim();
                cNode.Image = imgClose;
                cNode.ImageExpanded = imgOpen;

                if (string.IsNullOrEmpty(dt.Rows[i]["DEPT_ID_UP"].ToString().Trim()))
                {
                    pName = "NODE10";
                }
                else
                {
                    pName = "NODE" + dt.Rows[i]["DEPT_ID_UP"].ToString().Trim();
                }

                //ABC 원가대상 부서 여부(Y/N)
                if (dt.Rows[i]["GBABC"].ToString().Trim() != "Y")
                {
                    //부서 대표코드(뒷자리 2자리가 00 인것)
                    if (dt.Rows[i]["BUSE1"].ToString().Trim() == "*")
                    {
                        cNode.Text = "tx" + dt.Rows[i]["Name"].ToString().Trim();
                        trvBuse.FindNodeByName(pName).Nodes.Add(cNode);
                    }
                    else
                    {
                        cNode.Text = "x" + dt.Rows[i]["Name"].ToString().Trim();
                        trvBuse.FindNodeByName(pName).Nodes.Add(cNode);
                    }
                }
                else
                {
                    cNode.Text = dt.Rows[i]["Name"].ToString().Trim();
                    trvBuse.FindNodeByName(pName).Nodes.Add(cNode);
                }

                cNode.TagString = dt.Rows[i]["BuCode"].ToString().Trim();
                //삭제된 부서 색상 변경
                if (string.IsNullOrEmpty(dt.Rows[i]["DelDate"].ToString().Trim()) == false)
                {
                    cNode.Style = DevComponents.AdvTree.NodeStyles.Red;
                    cNode.StyleSelected = DevComponents.AdvTree.NodeStyles.Red;
                }

                cNode = null;
            }
            Cursor.Current = Cursors.Default;

        }

        void Screen_Clear()
        {
            txtName.Text = "";
            txtSort.Text = "";
            txtYName.Text = "";
            panel.Text = "";
            txtSort.Text = "";
            txtDeptCode.Text = "";
            txtWard.Text = "";
            cboBuseBun.Text = "";
            txtRemark.Text = "";
            txtABCBuCode.Text = "";
            txtABCYName.Text = "";
            txtDeptSort.Text = "";
            dtpDelDate.Text = "";
            lblMsg.Text = "";
            lblABCMsg.Text = "아래 부서코드 칸을 더블클릭하면 등록이 됩니다.";

            ss1_Sheet1.ClearRange(0, 0, 1, ss1_Sheet1.Columns.Count, true);

            panMain.Enabled = false;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            panCode.Enabled = true;
            btnView.Enabled = true;

        }

        bool DeleteData()
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return false; //권한 확인

            long strBuCode = 0;
            long nCnt = 0;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            if (MessageBox.Show("정말 삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                strBuCode = long.Parse(txtBuCode.Text.Trim());
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                //급여 마스타 건수를 읽음
                SQL = "SELECT COUNT(*) CNT FROM " + ComNum.DB_ERP + "PAY_MST ";
                SQL = SQL + ComNum.VBLF + "WHERE Buse='" + strBuCode + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }


                nCnt = 0;
                if (dt.Rows.Count > 0) { nCnt = long.Parse(dt.Rows[0]["CNT"].ToString()); }

                dt.Dispose();
                dt = null;

                if (nCnt > 0)
                {
                    MessageBox.Show("급여마스타에 " + nCnt + "명이 등록되어 있습니다", "오류");
                    return false;
                }

                //인사 마스타 건수를 읽음
                SQL = "SELECT COUNT(*) CNT FROM " + ComNum.DB_ERP + "INSA_MST ";
                SQL = SQL + ComNum.VBLF + "WHERE Buse='" + strBuCode + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }


                nCnt = 0;
                if (dt.Rows.Count > 0) { nCnt = long.Parse(dt.Rows[0]["CNT"].ToString()); }

                dt.Dispose();
                dt = null;

                if (nCnt > 0)
                {
                    MessageBox.Show("인사마스타에 " + nCnt + "명이 등록되어 있습니다", "오류");
                    return false;
                }

                //관리과 입출고내역
                SQL = "SELECT COUNT(*) CNT FROM " + ComNum.DB_ERP + "ORD_SUBUL ";
                SQL = SQL + ComNum.VBLF + "WHERE BuseCode = '" + strBuCode + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }


                nCnt = 0;
                if (dt.Rows.Count > 0) { nCnt = long.Parse(dt.Rows[0]["CNT"].ToString()); }

                dt.Dispose();
                dt = null;

                if (nCnt > 0)
                {
                    MessageBox.Show("관리과 입출고 내역에 " + nCnt + "명이 등록되어 있습니다", "오류");
                    return false;
                }

                //약제 입출고내역
                SQL = "SELECT COUNT(*) CNT FROM " + ComNum.DB_ERP + "DRUG_SUBUL ";
                SQL = SQL + ComNum.VBLF + "WHERE BuseCode = '" + strBuCode + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }


                nCnt = 0;
                if (dt.Rows.Count > 0) { nCnt = long.Parse(dt.Rows[0]["CNT"].ToString()); }

                dt.Dispose();
                dt = null;

                if (nCnt > 0)
                {
                    MessageBox.Show("약제과 입출고 내역에 " + nCnt + "명이 등록되어 있습니다", "오류");
                    return false;
                }

                SQL = " DELETE BAS_BUSE ";
                SQL = SQL + ComNum.VBLF + "WHERE Bucode = '" + strBuCode + "' ";

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

                Screen_Clear();
                txtBuCode.Focus();
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        bool SaveData()
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인

            string strROWID,
                   strTree,
                   strBuCode,
                   strName,
                   strSName,
                   strDeptCode,
                   strDeldate,
                   strACC,
                   strPAY,
                   strINSA,
                   strJAS,
                   strCSR,
                   strBuseBun,
                   strBun,
                   strGbPayTime,
                   strYName,
                   strGbABC,
                   strORDFlag,
                   strGbDRUG,
                   strWardCode,
                   strSORT,
                   strGbJego,
                   strBuse1,
                   strBuse2,
                   strOpdBuCode,
                   strBuse3 = "";
            int nTREE_Depth = 0;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수
            DataTable dt = null;

            strBuCode = txtBuCode.Text.Trim();
            if (strBuCode.Length != 6) { MessageBox.Show("부서코드를 6자리로 입력하세요!!", "확인"); return false; }
            strName = txtName.Text.Trim();
            if (strName.Length > 30) { MessageBox.Show("부서명칭이 30자를 초과함!!", "오류"); return false; }
            strSName = txtSort.Text.Trim();
            if (strSName.Length > 10) { MessageBox.Show("부서 약식명칭이 10자를 초과함!!", "오류"); return false; }
            strYName = txtYName.Text.Trim();
            if (strYName.Length > 6) { MessageBox.Show("부서약어가 6자를 초과함!!", "오류"); return false; }
            strTree = txtTree.Text.Trim();
            //조건삭제
            strSORT = txtSort.Text.Trim();
            if (strSORT != "")
                if (strSORT.Length != 4) { MessageBox.Show("부서 분류순위를 4자리로 입력하세요!!", "오류"); return false; }
            strDeldate = dtpDelDate.Text.Trim();
            if (strDeldate != "")
            {
                if (strDeldate.Length != 10) { MessageBox.Show("삭제일자를 YYYY-MM-DD 형식으로 입력하세요", "오류"); return false; }
                if (VB.IsDate(strDeldate) == false) { MessageBox.Show("삭제일자를 날짜형식으로 정확하게 입력하세요", "오류"); return false; }
            }
            strBuseBun = cboBuseBun.Text.Substring(0, 1).Trim();
            if (strBuseBun == "") { MessageBox.Show("부서분류가 공란입니다!!", "오류"); return false; }
            strDeptCode = txtDeptCode.Text.Trim();
            if (strDeptCode != "")
            {
                if (strDeptCode.Length > 4) { MessageBox.Show("진료과 코드를 정확하게 입력하세요!!", "오류"); return false; }
            }
            strWardCode = txtWard.Text.Trim();
            if (strWardCode != "")
            {
                if (strWardCode.Length > 4) { MessageBox.Show("병동코드를 정확하게 입력하세요!!", "오류"); return false; }
            }

            if (txtRemark.Text.Length > 100) { MessageBox.Show("참고사항은 100자리 까지 가능합니다", "오류"); return false; }

            //업무별 플래그 설정
            strACC = ss1_Sheet1.Cells[0, 0].Text == "True" ? "*" : " ";
            strGbABC = ss1_Sheet1.Cells[0, 1].Text == "True" ? "Y" : "N";
            strINSA = ss1_Sheet1.Cells[0, 2].Text == "True" ? "*" : " ";
            strPAY = ss1_Sheet1.Cells[0, 3].Text == "True" ? "*" : " ";
            strORDFlag = ss1_Sheet1.Cells[0, 4].Text == "True" ? "Y" : "N";
            strGbDRUG = ss1_Sheet1.Cells[0, 5].Text == "True" ? "Y" : "N";
            strCSR = ss1_Sheet1.Cells[0, 6].Text == "True" ? "*" : " ";
            strJAS = ss1_Sheet1.Cells[0, 7].Text == "True" ? "*" : " ";
            strGbPayTime = ss1_Sheet1.Cells[0, 8].Text == "True" ? "N" : "Y";
            strGbJego = ss1_Sheet1.Cells[0, 9].Text == "True" ? "Y" : "N";
            strBuse1 = ss1_Sheet1.Cells[0, 10].Text == "True" ? "*" : " ";
            strBuse2 = ss1_Sheet1.Cells[0, 11].Text == "True" ? "*" : " ";
            strBuse3 = ss1_Sheet1.Cells[0, 12].Text == "True" ? "*" : " ";

            //트리 깊이
            if (VB.Right(strTree, 6) == "000000")
            {
                nTREE_Depth = 1;
            }
            else if (VB.Right(strTree, 4) == "0000")
            {
                nTREE_Depth = 2;
            }
            else if (VB.Right(strTree, 2) == "00")
            {
                nTREE_Depth = 3;
            }
            else
            {
                nTREE_Depth = 4;
            }
            strBun = strTree.Substring(0, 2);

            //이미 등록이 되었는지 점검
            SQL = "SELECT ROWID FROM " + ComNum.DB_PMPA + "BAS_BUSE ";
            SQL = SQL + ComNum.VBLF + " WHERE BuCode='" + strBuCode + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return false;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                return false;
            }
            strROWID = "";
            if (dt.Rows.Count > 0) { strROWID = dt.Rows[0]["ROWID"].ToString().Trim(); }
            dt.Dispose();
            dt = null;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            
            try
            {
                //신규등록
                if (strROWID == "")
                {
                    SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_BUSE (";
                    SQL = SQL + ComNum.VBLF + " BUCODE,NAME,SNAME,YNAME,TREE,TREE_DEPTH,RANKING,BUSEBUN,BUN,DEPTCODE,";
                    SQL = SQL + ComNum.VBLF + " WARDCODE,ACC,GBABC,INSA,PAY,ORDFLAG,GBDRUG,CSR,JAS,GBPAYTIME,GBJEGO,";
                    SQL = SQL + ComNum.VBLF + " Buse1,Buse2,DELDATE, ABCBUCODE, DEPT_ID, DEPT_ID_UP, DEPT_SORT, DEPT_LEVEL, ABCYNAME , BUSE3)  ";
                    SQL = SQL + ComNum.VBLF + " VALUES ('" + strBuCode + "','" + strName + "','" + strSName + "','";
                    SQL = SQL + ComNum.VBLF + strYName + "','" + strTree + "'," + nTREE_Depth + ",'" + strSORT + "','";
                    SQL = SQL + ComNum.VBLF + strBuseBun + "','" + strBun + "','" + strDeptCode + "','";
                    SQL = SQL + ComNum.VBLF + strWardCode + "','" + strACC + "','" + strGbABC + "','" + strINSA + "','";
                    SQL = SQL + ComNum.VBLF + strPAY + "','" + strORDFlag + "','" + strGbDRUG + "','" + strCSR + "','";
                    SQL = SQL + ComNum.VBLF + strJAS + "','" + strGbPayTime + "','" + strGbJego + "','";
                    SQL = SQL + ComNum.VBLF + strBuse1 + "','" + strBuse2 + "', ";
                    SQL = SQL + ComNum.VBLF + "TO_DATE('" + strDeldate + "','YYYY-MM-DD'), '" + txtABCBuCode.Text + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strBuCode + "', '','10', '1' ,'" + txtABCYName.Text + "' , '" + strBuse3 + "' ";
                    SQL = SQL + ComNum.VBLF + ") ";
                }
                else
                {
                    strOpdBuCode = "";
                    if (strBun == "02")
                    {
                        SQL = " SELECT ABCBUCODE FROM BAS_BUSE";
                        SQL = SQL + ComNum.VBLF + " WHERE DEPT_ID_UP IN (SELECT DEPT_ID FROM BAS_BUSE";
                        SQL = SQL + ComNum.VBLF + "                       WHERE ROWID = '" + strROWID + "'";
                        SQL = SQL + ComNum.VBLF + "                         AND ABCYNAME IS NOT NULL) ";
                        SQL = SQL + ComNum.VBLF + "                         AND DEPT_LEVEL = '4' ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY DEPT_SORT ";
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return false;
                        }
                        if (dt.Rows.Count == 0)
                        {
                            dt.Dispose();
                            dt = null;
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            return false;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            strOpdBuCode = dt.Rows[0]["ABCBUCODE"].ToString().Trim();
                        }
                        dt.Dispose();
                        dt = null;
                    }
                    SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_BUSE SET ";
                    SQL = SQL + ComNum.VBLF + " Name='" + strName + "',";
                    SQL = SQL + ComNum.VBLF + " YName='" + strYName + "',";
                    SQL = SQL + ComNum.VBLF + " SName='" + strSName + "',";
                    SQL = SQL + ComNum.VBLF + " Tree='" + strTree + "',";
                    SQL = SQL + ComNum.VBLF + " Tree_Depth=" + nTREE_Depth + ",";
                    SQL = SQL + ComNum.VBLF + " Ranking='" + strSORT + "',";
                    SQL = SQL + ComNum.VBLF + " BuseBun='" + strBuseBun + "',";
                    SQL = SQL + ComNum.VBLF + " Bun='" + strBun + "',";
                    SQL = SQL + ComNum.VBLF + " DeptCode='" + strDeptCode + "',";
                    SQL = SQL + ComNum.VBLF + " WardCode='" + strWardCode + "',";
                    SQL = SQL + ComNum.VBLF + " Acc='" + strACC + "',";
                    SQL = SQL + ComNum.VBLF + " GbAbc='" + strGbABC + "',";
                    SQL = SQL + ComNum.VBLF + " Insa='" + strINSA + "',";
                    SQL = SQL + ComNum.VBLF + " Pay='" + strPAY + "',";
                    SQL = SQL + ComNum.VBLF + " ORDFlag='" + strORDFlag + "',";
                    SQL = SQL + ComNum.VBLF + " GbDrug='" + strGbDRUG + "',";
                    SQL = SQL + ComNum.VBLF + " CSR='" + strCSR + "',";
                    SQL = SQL + ComNum.VBLF + " JAS='" + strJAS + "',";
                    SQL = SQL + ComNum.VBLF + " GbPayTime='" + strGbPayTime + "',";
                    SQL = SQL + ComNum.VBLF + " GbJego='" + strGbJego + "',";
                    SQL = SQL + ComNum.VBLF + " Buse1='" + strBuse1 + "',";
                    SQL = SQL + ComNum.VBLF + " Buse2='" + strBuse2 + "',";
                    SQL = SQL + ComNum.VBLF + " Buse3='" + strBuse3 + "',";
                    SQL = SQL + ComNum.VBLF + " DelDate=TO_DATE('" + strDeldate + "','YYYY-MM-DD'),";
                    SQL = SQL + ComNum.VBLF + " ABCBUCODE = '" + txtABCBuCode.Text + "',";
                    SQL = SQL + ComNum.VBLF + " ABCYNAME = '" + txtABCYName.Text.Trim() + "' ,";
                    SQL = SQL + ComNum.VBLF + " OPDBUCODE = '" + strOpdBuCode + "', ";
                    SQL = SQL + ComNum.VBLF + " DEPT_SORT = '" + txtDeptSort.Text + "', ";
                    SQL = SQL + ComNum.VBLF + " REMARK = '" + VB.Replace(txtRemark.Text, "'", "`") + "'";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID='" + strROWID + "' ";
                }

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
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                Screen_Clear();
                txtBuCode.Focus();
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        bool SearchData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인

            int nRow = 0;

            string strBun = "";
            string strViewName = "";
            string strTree = "";
            string strName = "";
            bool bDisplayOK = false;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            ss2_Sheet1.Rows.Count = 30;
            ss2_Sheet1.ClearRange(0, 0, ss2_Sheet1.Rows.Count, ss2_Sheet1.Columns.Count, true);

            strBun = cboBuse.Text.Substring(0, 2);
            strViewName = txtViewName.Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = " SELECT TREE,BuCode,Name,SName,YName,BUSEBUN,Bun,DEPTCODE, WARDCODE,  ";
                SQL = SQL + ComNum.VBLF + " ACC,GbABC,INSA,PAY,OrdFlag,GbDrug,JAS,CSR,GbPayTime,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(DelDate,'YYYY-MM-DD') DelDate,BuseBun,";
                SQL = SQL + ComNum.VBLF + " GbJego,Buse1,Buse2,ABCBUCODE, ROWID ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_BUSE ";
                if (chkDel.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " WHERE DELDATE IS NULL ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " WHERE DELDATE IS NOT NULL ";
                }
                if (strBun != "**") { SQL = SQL + ComNum.VBLF + " AND Bun='" + strBun + "' "; }
                if (strViewName != "") { SQL = SQL + ComNum.VBLF + " AND NAME LIKE '%" + strViewName + "%' "; }
                //대표부서 제외
                if (chkDaepyo.Checked == true) { SQL = SQL + ComNum.VBLF + " AND Buse1 IS NULL "; }

                if (optSort0.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY TREE,BuCode,NAME ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY BuCode,Name ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }

                ss2_Sheet1.Rows.Count = dt.Rows.Count;
                ss2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bDisplayOK = true;
                    if (chkName.Checked == true && optSort0.Checked == true)
                    {
                        if (VB.Right(dt.Rows[i]["BuCode"].ToString(), 2) != "00" && dt.Rows[i]["GbABC"].ToString() != "Y")
                        {
                            bDisplayOK = false;
                        }
                    }

                    if (bDisplayOK == true)
                    {
                        if (chkName.Checked == false)
                        {
                            strName = " " + dt.Rows[i]["Name"].ToString().Trim();
                        }
                        else
                        {
                            //명칭을 트리형식으로 변환
                            if (optSort0.Checked == true)
                            {
                                strTree = dt.Rows[i]["Tree"].ToString().Trim();
                                if (VB.Right(strTree, 6) == "000000")
                                {
                                    strName = " " + dt.Rows[i]["Name"].ToString().Trim();
                                }
                                else if (VB.Right(strTree, 4) == "0000")
                                {
                                    strName = " . . . " + dt.Rows[i]["Name"].ToString().Trim();
                                }
                                else if (VB.Right(strTree, 2) == "00")
                                {
                                    strName = " . . . . . . " + dt.Rows[i]["Name"].ToString().Trim();
                                }
                                else
                                {
                                    strName = " . . . . . . . . . " + dt.Rows[i]["Name"].ToString().Trim();
                                }
                            }
                            else
                            {
                                strTree = dt.Rows[i]["BuCode"].ToString().Trim();
                                if (int.Parse(strTree.Substring(0, 2)) >= 10)
                                {
                                    strName = " " + dt.Rows[i]["Name"].ToString().Trim();
                                }
                                else
                                {
                                    if (VB.Right(strTree, 2) == "00")
                                    {
                                        strName = " " + dt.Rows[i]["Name"].ToString().Trim();
                                    }
                                    else
                                    {
                                        strName = " . . . " + dt.Rows[i]["Name"].ToString().Trim();
                                    }
                                }
                            }
                        }

                        nRow++;
                        if (nRow > ss2_Sheet1.Rows.Count)
                        {
                            ss2_Sheet1.Rows.Count++;
                        }
                        
                        ss2_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["BuCode"].ToString();
                        ss2_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["Tree"].ToString();
                        ss2_Sheet1.Cells[nRow - 1, 2].Text = strName;
                        ss2_Sheet1.Cells[nRow - 1, 3].Text = " " + dt.Rows[i]["SName"].ToString();
                        ss2_Sheet1.Cells[nRow - 1, 4].Text = " " + dt.Rows[i]["YName"].ToString();
                        ss2_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["Bun"].ToString();
                        ss2_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["ACC"].ToString() == "*" ? "◎" : "";
                        ss2_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["GbAbc"].ToString() == "Y" ? "◎" : "";
                        ss2_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["Insa"].ToString() == "*" ? "◎" : "";
                        ss2_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["Pay"].ToString() == "*" ? "◎" : "";
                        ss2_Sheet1.Cells[nRow - 1, 10].Text = dt.Rows[i]["OrdFlag"].ToString() == "Y" ? "◎" : "";
                        ss2_Sheet1.Cells[nRow - 1, 11].Text = dt.Rows[i]["GbDrug"].ToString() == "Y" ? "◎" : "";
                        ss2_Sheet1.Cells[nRow - 1, 12].Text = dt.Rows[i]["CSR"].ToString() == "*" ? "◎" : "";
                        ss2_Sheet1.Cells[nRow - 1, 13].Text = dt.Rows[i]["JAS"].ToString() == "*" ? "◎" : "";
                        ss2_Sheet1.Cells[nRow - 1, 14].Text = dt.Rows[i]["GbPayTime"].ToString() == "Y" ? "◎" : "";
                        ss2_Sheet1.Cells[nRow - 1, 15].Text = dt.Rows[i]["GbJego"].ToString() == "Y" ? "◎" : "";
                        ss2_Sheet1.Cells[nRow - 1, 16].Text = dt.Rows[i]["Buse1"].ToString() == "*" ? "◎" : "";
                        ss2_Sheet1.Cells[nRow - 1, 17].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ss2_Sheet1.Cells[nRow - 1, 18].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                        ss2_Sheet1.Cells[nRow - 1, 19].Text = dt.Rows[i]["BuseBun"].ToString().Trim();
                        ss2_Sheet1.Cells[nRow - 1, 20].Text = dt.Rows[i]["DelDate"].ToString().Trim();
                        ss2_Sheet1.Cells[nRow - 1, 21].Text = dt.Rows[i]["ABCBUCODE"].ToString().Trim();
                        if (dt.Rows[i]["ABCBUCODE"].ToString() != "")
                        {
                            ss2_Sheet1.Cells[nRow - 1, 22].Text = ReadBasBuse(dt.Rows[i]["ABCBUCODE"].ToString());
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                ss2_Sheet1.Rows.Count = nRow;
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        bool ViewData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인

            string strPath = "";
            string strDept_ID_UP = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //부서코드 점검
                txtBuCode.Text = txtBuCode.Text.Trim();
                if (txtBuCode.Text.Length != 6) { MessageBox.Show("부서코드를 6자리로 입력하세요!!", "오류"); return false; }

                Screen_Clear();

                SQL = "SELECT TREE,BuCode,Name,SName,YName,BUSEBUN,Bun,DEPTCODE, WARDCODE,  ";
                SQL = SQL + ComNum.VBLF + " ACC,GbABC,INSA,PAY,OrdFlag,GbDrug,JAS,CSR,GbPayTime,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(DelDate,'YYYY-MM-DD') DelDate,BuseBun,Ranking,";
                SQL = SQL + ComNum.VBLF + " GbJego,Buse1,Buse2, Buse3, ABCBUCODE, ROWID, DEPT_ID_UP, DEPT_SORT, Remark, abcyname ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_BUSE ";
                SQL = SQL + ComNum.VBLF + "WHERE BuCode='" + txtBuCode.Text + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return false;
                }

                panCode.Enabled = false;
                btnView.Enabled = false;

                if (dt.Rows.Count > 0)
                {
                    txtName.Text = dt.Rows[0]["Name"].ToString().Trim();
                    txtSName.Text = dt.Rows[0]["SName"].ToString().Trim();
                    txtYName.Text = dt.Rows[0]["YName"].ToString().Trim();
                    txtTree.Text = dt.Rows[0]["Tree"].ToString().Trim();
                    txtSort.Text = dt.Rows[0]["Ranking"].ToString().Trim();
                    txtDeptCode.Text = dt.Rows[0]["DeptCode"].ToString().Trim();
                    txtWard.Text = dt.Rows[0]["WardCode"].ToString().Trim();
                    dtpDelDate.Text = dt.Rows[0]["DelDate"].ToString().Trim();
                    txtABCBuCode.Text = dt.Rows[0]["ABCBUCODE"].ToString().Trim();
                    if (txtABCBuCode.Text != "") { lblABCMsg.Text = ReadBasBuse(txtABCBuCode.Text); }
                    txtABCYName.Text = dt.Rows[0]["ABCYNAME"].ToString().Trim();
                    switch (dt.Rows[0]["BuseBun"].ToString().Trim())
                    {
                        case "1": cboBuseBun.SelectedIndex = 0; break;
                        case "2": cboBuseBun.SelectedIndex = 1; break;
                        case "3": cboBuseBun.SelectedIndex = 2; break;
                        case "4": cboBuseBun.SelectedIndex = 3; break;
                        default: cboBuseBun.SelectedIndex = 4; break;
                    }

                    ss1_Sheet1.Cells[0, 0].Text = dt.Rows[0]["ACC"].ToString() == "*" ? "◎" : "";
                    ss1_Sheet1.Cells[0, 1].Text = dt.Rows[0]["GbAbc"].ToString() == "Y" ? "◎" : "";
                    ss1_Sheet1.Cells[0, 2].Text = dt.Rows[0]["Insa"].ToString() == "*" ? "◎" : "";
                    ss1_Sheet1.Cells[0, 3].Text = dt.Rows[0]["Pay"].ToString() == "*" ? "◎" : "";
                    ss1_Sheet1.Cells[0, 4].Text = dt.Rows[0]["OrdFlag"].ToString() == "Y" ? "◎" : "";
                    ss1_Sheet1.Cells[0, 5].Text = dt.Rows[0]["GbDrug"].ToString() == "Y" ? "◎" : "";
                    ss1_Sheet1.Cells[0, 6].Text = dt.Rows[0]["CSR"].ToString() == "*" ? "◎" : "";
                    ss1_Sheet1.Cells[0, 7].Text = dt.Rows[0]["JAS"].ToString() == "*" ? "◎" : "";
                    ss1_Sheet1.Cells[0, 8].Text = dt.Rows[0]["GbPayTime"].ToString() == "Y" ? "◎" : "";
                    ss1_Sheet1.Cells[0, 9].Text = dt.Rows[0]["GbJego"].ToString() == "Y" ? "◎" : "";
                    ss1_Sheet1.Cells[0, 10].Text = dt.Rows[0]["Buse1"].ToString() == "*" ? "◎" : "";
                    ss1_Sheet1.Cells[0, 11].Text = dt.Rows[0]["Buse2"].ToString() == "*" ? "◎" : "";
                    ss1_Sheet1.Cells[0, 12].Text = dt.Rows[0]["Buse3"].ToString() == "*" ? "◎" : "";
                    btnDelete.Enabled = true;

                    txtDeptSort.Text = dt.Rows[0]["DEPT_SORT"].ToString();
                    txtRemark.Text = dt.Rows[0]["Remark"].ToString();
                    strDept_ID_UP = dt.Rows[0]["DEPT_ID_UP"].ToString();
                    strPath = dt.Rows[0]["NAME"].ToString();

                    dt.Dispose();
                    dt = null;

                    for (int i = 1; i <= 10; i++)
                    {
                        SQL = "SELECT DEPT_ID_UP, NAME FROM BAS_BUSE ";
                        SQL = SQL + ComNum.VBLF + " WHERE DEPT_ID = '" + strDept_ID_UP + "'";
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                        if (dt.Rows.Count == 0)
                        {
                            break;
                        }

                        strPath = dt.Rows[0]["NAME"].ToString() + "/" + strPath;
                        strDept_ID_UP = dt.Rows[0]["DEPT_ID_UP"].ToString();
                        dt.Dispose();
                        dt = null;
                    }
                }

                lblPath.Text = strPath;

                panMain.Enabled = true;
                btnCancel.Enabled = true;

                switch (clsPublic.GnJobSabun)
                {
                    case 13850:
                    case 16092:
                    case 4349:
                        chkTree.Enabled = true;
                        btnSave.Enabled = true;
                        break;
                    default:
                        MessageBox.Show("등록권한이 없습니다 " + "\n" + "부서코드 관리는 기획 행정과 김재관 계장이 담당입니다.", "확인");
                        btnSave.Enabled = false;
                        chkTree.Enabled = false;
                        break;
                }
                Cursor.Current = Cursors.Default;
                return true;
            }

            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        //TODO:READ_BAS_BUSE 사용 VbFunction BuCode1
        string ReadBasBuse(string strBucode)
        {
            DataTable dt = null;
            string strVal = "";
            string SQL = "";
            string SqlErr = "";

            if (strBucode.Trim() == "")
            {
                strVal = "";
                return strVal;
            }


            SQL = "";
            SQL = SQL + "SELECT Name,SName FROM ADMIN.BAS_BUSE ";
            SQL = SQL + ComNum.VBLF + " WHERE BuCode='" + strBucode + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return strVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                return strVal;
            }

            if (dt == null)
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                Cursor.Current = Cursors.Default;
                return strVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return strVal;
            }

            if (dt.Rows[0]["SName"].ToString().Trim() != "")
            {
                strVal = dt.Rows[0]["SName"].ToString().Trim();
            }
            else
            {
                strVal = dt.Rows[0]["Name"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            return strVal;
        }

        #endregion

        #region 이벤트

        void SetEvent()
        {
            this.Load += (sender, e) =>
            {
                if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                string strData = "";

                string SQL = "";
                string SqlErr = "";
                DataTable dt = null;

                txtViewName.Text = "";
                txtBuCode.Text = "";

                cboBuse.Items.Clear();
                cboBuse.Items.Add("**.전체");
                //부서 대분류 읽어온다
                SQL = "SELECT Code,Name FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + "WHERE Gubun='BAS_부서대분류' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY Code ";

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

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strData = String.Format("{0:00}", dt.Rows[i]["Code"].ToString()) + ".";
                    strData += dt.Rows[i]["Name"].ToString().Trim();
                    cboBuse.Items.Add(strData);
                }

                dt.Dispose();
                dt = null;
                cboBuse.SelectedIndex = 0;

                cboBuseBun.Items.Clear();
                cboBuseBun.Items.Add("1.행정/관리부서");
                cboBuseBun.Items.Add("2.진료부서");
                cboBuseBun.Items.Add("3.진료지원부서");
                cboBuseBun.Items.Add("4.간호부");
                cboBuseBun.Items.Add("5.기타");

                Screen_Clear();
                Read_Tree();
                ss2_Sheet1.Columns[1].Visible = false;
            };

            #region Click

            menuExit.Click   += (sender, e) => { this.Close(); };

            menuEtc01.Click  += (sender, e) =>
            {
                string strBuCode = "";
                string strROWID = "";

                int intRowAffected = 0;
                string SQL = "";
                string SqlErr = "";
                DataTable dt = null;
                DataTable dt1 = null;

                Cursor.Current = Cursors.WaitCursor;

                clsDB.setBeginTran(clsDB.DbCon);
                

                try
                {
                    SQL = "SELECT * FROM " + ComNum.DB_PMPA + "BAS_BUCODE ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY TREE,Name ";
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

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        strBuCode = dt.Rows[i]["BuCode"].ToString().Trim();

                        //자료 있는지 점검
                        SQL = "SELECT ROWID FROM " + ComNum.DB_PMPA + "BAS_BUSE ";
                        SQL = SQL + ComNum.VBLF + " WHERE BuCode='" + strBuCode + "' ";
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

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
                        strROWID = "";
                        if (dt1.Rows.Count > 0) { strROWID = dt1.Rows[0]["ROWID"].ToString().Trim(); }
                        dt1.Dispose();
                        dt1 = null;

                        SQL = "";
                        if (strROWID == "")
                        {
                            SQL = "INSERT INTO " + ComNum.DB_PMPA + "BAS_BUSE (BuCode,Tree,Name,SName,";
                            SQL = SQL + ComNum.VBLF + " YName,Bun,Acc,GbABC,Insa,Pay,ORDFlag,GbDrug,Jas,";
                            SQL = SQL + ComNum.VBLF + " CSR,GbPayTime,DelDate,DeptCode,WardCode,BuseBun) ";
                            SQL = SQL + ComNum.VBLF + " VALUES ('";
                            SQL = SQL + ComNum.VBLF + dt.Rows[i]["BuCode"].ToString().Trim() + "','";
                            SQL = SQL + ComNum.VBLF + dt.Rows[i]["Tree"].ToString().Trim() + "','";
                            SQL = SQL + ComNum.VBLF + dt.Rows[i]["Name"].ToString().Trim() + "','','','";
                            SQL = SQL + ComNum.VBLF + dt.Rows[i]["Tree"].ToString().Substring(0, 2) + "',";
                            SQL = SQL + ComNum.VBLF + "'','Y','','','','','Y','','Y','','','','') ";
                        }
                        else
                        {
                            SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_BUSE SET ";
                            SQL = SQL + ComNum.VBLF + " Tree='" + dt.Rows[i]["Tree"].ToString().Trim() + "',";
                            SQL = SQL + ComNum.VBLF + " Bun='" + dt.Rows[i]["Tree"].ToString().Substring(0, 2) + "',";
                            SQL = SQL + ComNum.VBLF + " GbABC='" + dt.Rows[i]["GbABC"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + " WHERE BuCode='" + dt.Rows[i]["BuCode"].ToString().Trim() + "' ";
                        }
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    clsDB.setCommitTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("!! 작업 완료 !!", "확인");
                    return;
                }
                catch (Exception ex)
                {
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show(ex.Message);
                    Cursor.Current = Cursors.Default;
                    return;
                }
            };
            menuEtc02.Click  += (sender, e) => 
            {   //TODO:텍스트파일 -> SQL문의 변수로 가는 로직 확실한지 확인 필요 17-06-08 박성완
                string fileName = "";
                string fileFullName = "";
                string filePath = "";

                string strTree = "";
                string strBuCode = "";
                string strBuName = "";
                string strGbABC = "";

                string SQL = "";    //Query문
                int intRowAffected = 0; //변경된 Row 받는 변수

                Cursor.Current = Cursors.WaitCursor;


                DialogResult dr = openFileDialog1.ShowDialog();

                try
                {
                    if (dr == DialogResult.OK)
                    {
                        fileName = openFileDialog1.SafeFileName.ToString();
                        fileFullName = openFileDialog1.FileName.ToString();
                        filePath = fileFullName.Replace(fileName, "");
                    }
                    else if (dr == DialogResult.Cancel)
                    {
                        return;
                    }

                    StreamReader sr = new StreamReader(fileFullName);

                    while (sr.Peek() >= 0)
                    {
                        string[] separator = { "\t" };
                        string[] result = sr.ReadLine().ToString().Split(separator, StringSplitOptions.None);
                        strTree = result[7].Trim();
                        strBuCode = result[0].Trim();
                        strBuName = result[2].Trim();
                        strGbABC = result[8].Trim() != "" ? "Y" : "N";

                        //부서코드에 업데이트
                        SQL = "UPDATE " + ComNum.DB_PMPA + "BAS_BUSE SET ";
                        SQL = SQL + ComNum.VBLF + " Tree='" + strTree + "',";
                        SQL = SQL + ComNum.VBLF + " GbAbc='" + strGbABC + "' ";
                        SQL = SQL + ComNum.VBLF + " WHERE BuCode = '" + strBuCode + "' ";

                        clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    }
                    sr.Close();
                    MessageBox.Show("!! 작업 완료 !!", "확인");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            };

            menuView01.Click += (sender, e) => { }; //TODO: frmTreeDept.show 폼 만들어졌을때 적용
            menuView02.Click += (sender, e) => { }; //TODO:frm부서코드_조직도1.show 폼 만들어졌을때 적용 확인

            btnPrint.Click   += (sender, e) =>
            {
                clsSpread SPR = new clsSpread();
                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                strTitle = "부서코드(신규)";

                strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);

                strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter    += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString(), new Font("굴림체", 12), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, true, true, true, false, false, false);

                SPR.setSpdPrint(ss2, PrePrint, setMargin, setOption, strHeader, strFooter);
            };
            btnSearch.Click  += (sender, e) => { if (SearchData() == false) return; };
            btnView.Click    += (sender, e) => { if (ViewData() == false) return; };
            btnSave.Click    += (sender, e) => { if (SaveData() == false) return; };
            btnDelete.Click  += (sender, e) => { if (DeleteData() == false) return; };
            btnCancel.Click  += (sender, e) => { Screen_Clear(); };
            #endregion

        }

        private void ss2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row == -1) { return; }

            if (txtName.Text.Trim() != "" && e.Column == 0)
            {
                txtABCBuCode.Text = ss2_Sheet1.Cells[e.Row, 0].Text;
                lblABCMsg.Text = ss2_Sheet1.Cells[e.Row, 2].Text;
                return;
            }

            if (e.Column <= 5)
            {
                Screen_Clear();
                txtBuCode.Text = ss2_Sheet1.Cells[e.Row, 0].Text.Trim();
                btnView.PerformClick();
                return;
            }
            if(e.Column > 16)
            {
                return;
            }        
        }

        private void Textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{Tab}");
            }
        }

        private void trvBuse_MouseMove(object sender, MouseEventArgs e)
        {
            if (chkTree.Checked == false) { return; }

            if (e.Button == MouseButtons.Left)
            {
                //루트는 drag 안됨
                if (trvBuse.SelectedNode.Name == "NODE10") { return; }               
            }
        }

        private void trvBuse_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            if (e.Node.TagString == "") { Screen_Clear(); return; }
            txtBuCode.Text = e.Node.TagString;
            btnView.PerformClick();
        }

        private void trvBuse_BeforeNodeDrop(object sender, DevComponents.AdvTree.TreeDragDropEventArgs e)
        {
            int nLevel = 0;
            int intRowAffected = 0;
            string strOldDest = "";
            string strNewDest = "";
            string SQL = "";
            string sqlError = "";

            if (chkTree.Checked == false) { return; }
            
            if (e.NewParentNode.FullPath.Contains(trvBuse.SelectedNode.FullPath) == true)
            {
                ComFunc.MsgBox("해당부서는 같은 하위부서로 이동이 않됩니다.", "확인");
                return;
            }

            if (trvBuse.SelectedIndex == e.NewParentNode.Index) { return; }

            if (trvBuse.SelectedNode.Name == e.NewParentNode.Name) { return; }

            strOldDest = trvBuse.SelectedNode.Name;
            strNewDest = e.NewParentNode.Name;

            nLevel = 0;
            foreach (char c in e.NewParentNode.FullPath)
                if (c == ';') nLevel++;
            nLevel = nLevel + 1;

            clsDB.setBeginTran(clsDB.DbCon);
            
            try
            {
                SQL = " UPDATE ADMIN.BAS_BUSE SET ";
                SQL = SQL + ComNum.VBLF + " DEPT_ID_UP = '" + VB.Val(strNewDest.Replace("NODE", "")) + "', ";
                SQL = SQL + ComNum.VBLF + " DEPT_LEVEL = '" + nLevel + "' ";
                SQL = SQL + ComNum.VBLF + "  WHERE DEPT_ID = '" + VB.Val(strOldDest.Replace("NODE", "")) + "'";

                sqlError = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (sqlError != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(sqlError);
                    clsDB.SaveSqlErrLog(sqlError, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("변경 되었습니다.");
                Cursor.Current = Cursors.Default;

                return;
            }

            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void menuEtc02_Click(object sender, EventArgs e)
        {

        }
    }
        #endregion
}
