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
using ComNurLibB;

namespace ComNurLibB
{
    public partial class frmNurCode : Form
    {
        enum eNurCode { check01,Gubun,Code,CodeName,Seq01,Jik,JikName,Remark,Change,ROWID  };

        public frmNurCode()
        {
            InitializeComponent();
        }

        void frmNurCode_Load(object sender, EventArgs e)
        {
            string str = "";
            
            SetCombo("NUR_간호기초항목");

            SetInit();

            AUTO_SPREAD_SET_NurCode(ssCode_Sheet1,1,0);

            if(cboCode.SelectedItem.ToString()!="")  str = VB.Left(cboCode.SelectedItem.ToString(), 1);

            SetSpreadHeaderVisble(str, ssCode_Sheet1);

        }

        void SetInit()
        {

            //clsNurse.GstrHelpCode = "";

            cboCode.Items.Add("1.직책코드");
            cboCode.Items.Add("2.병동코드");
            cboCode.Items.Add("3.주사분류");
            cboCode.Items.Add("4.근무형태");
            cboCode.Items.Add("5.외래부서");
            cboCode.Items.Add("6.특수검사");
            cboCode.Items.Add("7.가동병상수");
            cboCode.Items.Add("Z.특수,주사(파트별");

            cboCode.Items.Add("A.주간당직");
            cboCode.Items.Add("B.저녁당직");
            cboCode.Items.Add("P.아르바이트등록");

            //TODO : 안정수 BAS_BCODE 콤보체크 > Call Combo_BCode_SET(ComboJong, "NUR_검사항목", False, True)

            cboCode.SelectedIndex = 0;

            btnSearch.Enabled = true;
            btnRegist.Enabled = false;
            btnCancel.Enabled = false;
            //ssCode.Enabled = false;

        }

        void SetCombo(string ArgGbn)
        {
            int i = 0;
 
            DataTable dt = null;
            string strSql = "";
            
            cboCode.Items.Clear();
            
            try
            {
                strSql = "";
                strSql = strSql + "SELECT  ";
                strSql = strSql + ComNum.VBLF + "  Code,Name  ";
                strSql = strSql + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                strSql = strSql + ComNum.VBLF + "   WHERE 1=1 ";
                strSql = strSql + ComNum.VBLF + "    AND GUBUN='" + ArgGbn + "' ";
                strSql = strSql + ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate ='') ";                
                strSql = strSql + ComNum.VBLF + "  ORDER BY Code ";

                dt = clsDB.GetDataTable(strSql);
              
                //cboCode.Items.Add("**.전체");
                                
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboCode.Items.Add(dt.Rows[i]["Code"].ToString().Trim() + "." + dt.Rows[i]["Name"].ToString().Trim());
                    }

                    cboCode.SelectedIndex = 0;
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                //TODO :  쿼리오류 추가
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            string str = VB.Left(cboCode.SelectedItem.ToString(), 1);

            GetData(ssCode_Sheet1,str);

            if(str=="A" || str =="B") GetData2(ssList_Sheet1, txtSearch.Text.Trim());
            
            
        }

        void GetData(FarPoint.Win.Spread.SheetView Spd,string strGbn)
        {
            int i = 0;
            DataTable dt = null;
            string strSql = string.Empty;
            Spd.RowCount = 0;
            ssCode.Enabled = true;

            btnRegist.Enabled = true;
            btnCancel.Enabled = true;            

            try
            {
                    strSql = "";
                    strSql = strSql + ComNum.VBLF + "SELECT";
                    strSql = strSql + ComNum.VBLF + "    Code, Name, PrintRanking, ROWID, Gubun,Jik ";
                    strSql = strSql + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_CODE";
                    strSql = strSql + ComNum.VBLF + "WHERE Gubun = '" + SetGubun(VB.Left(cboCode.SelectedItem.ToString(), 1)) + "'";

                    if (strGbn != "A" && strGbn != "B")
                    {
                        strSql = strSql + ComNum.VBLF + "ORDER BY PrintRanking, Code";
                    }
                    else
                    {
                        strSql = strSql + ComNum.VBLF + "ORDER BY PrintRanking,Jik,Name";
                    }
       

                dt = clsDB.GetDataTable(strSql);

                if (dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                Spd.RowCount = dt.Rows.Count + 10;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    
                        Spd.Cells[i, (int)eNurCode.check01].Text = "";
                        Spd.Cells[i, (int)eNurCode.Gubun].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                        Spd.Cells[i, (int)eNurCode.Code].Text = dt.Rows[i]["Code"].ToString().Trim();
                        Spd.Cells[i, (int)eNurCode.CodeName].Text = dt.Rows[i]["Name"].ToString().Trim();
                        Spd.Cells[i, (int)eNurCode.Seq01].Text = dt.Rows[i]["PrintRanking"].ToString().Trim();

                        Spd.Cells[i, (int)eNurCode.Jik].Text = dt.Rows[i]["Jik"].ToString().Trim();
                        Spd.Cells[i, (int)eNurCode.JikName].Text = clsNurse.READ_JikName(dt.Rows[i]["Jik"].ToString().Trim());

                        Spd.Cells[i, (int)eNurCode.Change].Text = "";

                        Spd.Cells[i, (int)eNurCode.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
               

                }
            } 
            catch (Exception ex)
            {
                //TODO : 예외처리 
            }
            
            dt.Dispose();
            dt = null;
            
        }
                
        void GetData2(FarPoint.Win.Spread.SheetView Spd,string ArgSearch)
        {
            int i = 0;
            DataTable dt = null;
            string strSql = string.Empty;
            Spd.RowCount = 0;
                      
            try
            {
                strSql = "";
                strSql = strSql + ComNum.VBLF + "SELECT";
                strSql = strSql + ComNum.VBLF + "   Sabun,KorName,Jik ";
                strSql = strSql + ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST";
                strSql = strSql + ComNum.VBLF + "WHERE 1=1 ";
                strSql = strSql + ComNum.VBLF + " AND JIK IN ('04','31','32','33') ";
                strSql = strSql + ComNum.VBLF + " AND Buse IN ( SELECT MATCH_CODE FROM " + ComNum.DB_PMPA + "NUR_CODE WHERE GUBUN = '2' ) ";
                if (ArgSearch!="")
                {
                    strSql = strSql + ComNum.VBLF + " AND ( Sabun = '" + ArgSearch + "'  ";
                    strSql = strSql + ComNum.VBLF + "      OR  KorName LIKE '%" + ArgSearch + "%'  ";
                    strSql = strSql + ComNum.VBLF + "     )";
                }
                strSql = strSql + ComNum.VBLF + " AND ToiDay is NULL ";
                strSql = strSql + ComNum.VBLF + "ORDER BY KorName ";
               

                dt = clsDB.GetDataTable(strSql);

                if (dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                Spd.RowCount = dt.Rows.Count ;

                for (i = 0; i < dt.Rows.Count; i++)
                { 
                    Spd.Cells[i, 0].Text = dt.Rows[i]["Sabun"].ToString().Trim();
                    Spd.Cells[i, 1].Text = dt.Rows[i]["KorName"].ToString().Trim();
                    Spd.Cells[i, 2].Text = dt.Rows[i]["Jik"].ToString().Trim();
                    Spd.Cells[i, 3].Text = clsNurse.READ_JikName( dt.Rows[i]["Jik"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                //TODO : 예외처리 
            }

            dt.Dispose();
            dt = null;

        }
      
        void btnCancel_Click(object sender, EventArgs e)
        {
            SetClear();
        }

        void SetClear()
        {            
            btnSearch.Enabled = true;
            btnRegist.Enabled = false;
            btnCancel.Enabled = false;
            btnExit.Enabled = true;
                        
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void AUTO_SPREAD_SET_NurCode(FarPoint.Win.Spread.SheetView Spd, int RowCnt, int ColCnt)
        {
            FarPoint.Win.Spread.CellType.CheckBoxCellType TypeCheckBox = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType TypeText = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType TypeText500 = new FarPoint.Win.Spread.CellType.TextCellType();            
            FarPoint.Win.Spread.CellType.TextCellType TypeMultiText = new FarPoint.Win.Spread.CellType.TextCellType();
            TypeText.MaxLength = 1000;
            TypeText500.MaxLength = 500;

            Spd.RowCount = RowCnt;
            Spd.ColumnCount = ColCnt;
            if (ColCnt == 0) Spd.ColumnCount = Enum.GetValues(typeof(eNurCode)).Length;

            Spd.ColumnHeader.Cells.Get(0, 0, 0, Spd.ColumnCount - 1).BackColor = Color.LightGray;

            Spd.ColumnHeader.Rows[0].Height = 35;

           //enum eNurCode { check01, Gubun, Code, CodeName, Seq01, Jik, JikName, Remark, Change, Rowid };

            //0
            Spd.ColumnHeader.Cells[0, (int)eNurCode.check01].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
            Spd.ColumnHeader.Cells[0, (int)eNurCode.check01].Text = "삭제";
            Spd.Columns[(int)eNurCode.check01].CellType = TypeCheckBox;
            Spd.Columns[(int)eNurCode.check01].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.Columns[(int)eNurCode.check01].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.Columns[(int)eNurCode.check01].Width = 18;
            Spd.Columns[(int)eNurCode.check01].Locked = false;
            Spd.Columns[(int)eNurCode.check01].Visible = true;

            Spd.ColumnHeader.Cells[0, (int)eNurCode.Gubun].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
            Spd.ColumnHeader.Cells[0, (int)eNurCode.Gubun].Text = "구분";
            Spd.Columns[(int)eNurCode.Gubun].CellType = TypeText;
            Spd.Columns[(int)eNurCode.Gubun].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.Columns[(int)eNurCode.Gubun].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.Columns[(int)eNurCode.Gubun].Width = 50;
            Spd.Columns[(int)eNurCode.Gubun].Locked = true;
            Spd.Columns[(int)eNurCode.Gubun].Visible = false;

            Spd.ColumnHeader.Cells[0, (int)eNurCode.Code].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
            Spd.ColumnHeader.Cells[0, (int)eNurCode.Code].Text = "코드";
            Spd.Columns[(int)eNurCode.Code].CellType = TypeText;
            Spd.Columns[(int)eNurCode.Code].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.Columns[(int)eNurCode.Code].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.Columns[(int)eNurCode.Code].Width = 50;
            Spd.Columns[(int)eNurCode.Code].Locked = false;
            Spd.Columns[(int)eNurCode.Code].Visible = true;

            Spd.ColumnHeader.Cells[0, (int)eNurCode.CodeName].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
            Spd.ColumnHeader.Cells[0, (int)eNurCode.CodeName].Text = "코드명칭";
            Spd.Columns[(int)eNurCode.CodeName].CellType = TypeText500; // 텍스트길이 500 자
            Spd.Columns[(int)eNurCode.CodeName].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.Columns[(int)eNurCode.CodeName].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            Spd.Columns[(int)eNurCode.CodeName].Width = 200;            
            Spd.Columns[(int)eNurCode.CodeName].Locked = false;
            Spd.Columns[(int)eNurCode.CodeName].Visible = true;

            Spd.ColumnHeader.Cells[0, (int)eNurCode.Seq01].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
            Spd.ColumnHeader.Cells[0, (int)eNurCode.Seq01].Text = "순번";
            Spd.Columns[(int)eNurCode.Seq01].CellType = TypeText;
            Spd.Columns[(int)eNurCode.Seq01].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.Columns[(int)eNurCode.Seq01].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.Columns[(int)eNurCode.Seq01].Width = 50;
            Spd.Columns[(int)eNurCode.Seq01].Locked = false;
            Spd.Columns[(int)eNurCode.Seq01].Visible = true;
            
            Spd.ColumnHeader.Cells[0, (int)eNurCode.Jik].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
            Spd.ColumnHeader.Cells[0, (int)eNurCode.Jik].Text = "직책";
            Spd.Columns[(int)eNurCode.Jik].CellType = TypeText;
            Spd.Columns[(int)eNurCode.Jik].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.Columns[(int)eNurCode.Jik].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.Columns[(int)eNurCode.Jik].Width = 35;
            Spd.Columns[(int)eNurCode.Jik].Locked = false;
            Spd.Columns[(int)eNurCode.Jik].Visible = true;

            Spd.ColumnHeader.Cells[0, (int)eNurCode.JikName].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
            Spd.ColumnHeader.Cells[0, (int)eNurCode.JikName].Text = "직책명";
            Spd.Columns[(int)eNurCode.JikName].CellType = TypeText;
            Spd.Columns[(int)eNurCode.JikName].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.Columns[(int)eNurCode.JikName].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            Spd.Columns[(int)eNurCode.JikName].Width = 215;
            Spd.Columns[(int)eNurCode.JikName].Locked = false;
            Spd.Columns[(int)eNurCode.JikName].Visible = true;

            Spd.ColumnHeader.Cells[0, (int)eNurCode.Remark].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
            Spd.ColumnHeader.Cells[0, (int)eNurCode.Remark].Text = "참고사항";
            Spd.Columns[(int)eNurCode.Remark].CellType = TypeText;
            Spd.Columns[(int)eNurCode.Remark].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.Columns[(int)eNurCode.Remark].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            Spd.Columns[(int)eNurCode.Remark].Width = 200;
            Spd.Columns[(int)eNurCode.Remark].Locked = true;
            Spd.Columns[(int)eNurCode.Remark].Visible = true;

            Spd.ColumnHeader.Cells[0, (int)eNurCode.Change].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
            Spd.ColumnHeader.Cells[0, (int)eNurCode.Change].Text = "수정";
            Spd.Columns[(int)eNurCode.Change].CellType = TypeText;
            Spd.Columns[(int)eNurCode.Change].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.Columns[(int)eNurCode.Change].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.Columns[(int)eNurCode.Change].Width = 50;
            Spd.Columns[(int)eNurCode.Change].Locked = true;
            Spd.Columns[(int)eNurCode.Change].Visible = true;

            Spd.ColumnHeader.Cells[0, (int)eNurCode.ROWID].Font = new Font("굴림체", 9, System.Drawing.FontStyle.Bold);
            Spd.ColumnHeader.Cells[0, (int)eNurCode.ROWID].Text = "ROWID";
            Spd.Columns[(int)eNurCode.ROWID].CellType = TypeText;
            Spd.Columns[(int)eNurCode.ROWID].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            Spd.Columns[(int)eNurCode.ROWID].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            Spd.Columns[(int)eNurCode.ROWID].Width = 50;
            Spd.Columns[(int)eNurCode.ROWID].Locked = true;
            Spd.Columns[(int)eNurCode.ROWID].Visible = false;
        }

        void btnRegist_Click(object sender, EventArgs e)
        {
            string str = VB.Left(cboCode.SelectedItem.ToString(), 1);

            if (str != "P")
            {
                SaveData(ssCode_Sheet1);
            }
            else
                SaveData(ssCode_Sheet1, str);

            GetData(ssCode_Sheet1, str);

            SetClear();
        }

        string SetGubun(string argGubun)
        {
            string str = argGubun.Trim();

            if(str=="P")
            {
                str = "8";                
            }

            return str;
        }

        void SaveData(FarPoint.Win.Spread.SheetView Spd, string str = "")
        {
            int i, j = 0;
            string strCode, strName = "";
            string strRowid = "";
            string strChange = "";
            string strJik = "";
            
            string strDel = "";
            int nSeq = 0;

            string strGubun = SetGubun(VB.Left(cboCode.Text, 1));

            //TODO : 안정수 저장 일시 막음
            //MessageBox.Show("저장일시 막음");
            //return;

            DataTable dt = null;

            string strSql = string.Empty;

            //for (i = 0; i < Spd.RowCount; i++)
            for (i = 0; i < Spd.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data + 1); i++)
            {
                strDel = Spd.Cells[i, (int)eNurCode.check01].Text == "True" ? "1" : "0";
                strCode = Spd.Cells[i, (int)eNurCode.Code].Text.Trim();
                strName = Spd.Cells[i, (int)eNurCode.CodeName].Text.Trim();
                //nSeq = 0;
                if (Spd.Cells[i, (int)eNurCode.Seq01].Text != "") nSeq = Convert.ToInt16(Spd.Cells[i, (int)eNurCode.Seq01].Text);

               
                //strJik = "";
                if(Spd.Cells[i, (int)eNurCode.Jik].Text.Trim() !="") strJik = ComFunc.SetAutoZero(Spd.Cells[i, (int)eNurCode.Jik].Text,2);

                //ssCode_Sheet1.Cells[e.Row, (int)eNurCode.Change].TextSpd.Cells[i, (int)eNurCode.Change].Text = "Y";

                
                strChange = Spd.Cells[i, (int)eNurCode.Change].Text;

                strRowid = Spd.Cells[i, (int)eNurCode.ROWID].Text;

                if (strCode.Trim() == "0191A")
                {
                    i = i;
                }

                // 기존데이터 삭제
                if (strDel == "1" && strRowid != "")
                {
                    clsDB.setBeginTran();

                    try
                    {
                        strSql = "";
                        strSql = strSql + ComNum.VBLF + " DELETE FROM ";
                        strSql = strSql + ComNum.VBLF + " " + ComNum.DB_PMPA + "NUR_CODE ";
                        strSql = strSql + ComNum.VBLF + "WHERE ROWID = '" + strRowid + "' ";
                        clsDB.ExecuteNonQuery(strSql);

                        clsDB.setCommitTran();
                        ComFunc.MsgBox("삭제하였습니다.");
                    }
                    catch (Exception e)
                    {
                        clsDB.setRollbackTran();
                        ComFunc.MsgBox("");
                    }

                }

                // 기존데이터 수정 or 신규등록
                else if (strDel != "1" && strCode != "")
                {
                    if (strChange =="Y")
                    {
                        if (strRowid == "")
                        {
                            clsDB.setBeginTran();
                            try
                            {
                                strSql = "";
                                strSql = strSql + ComNum.VBLF + "INSERT INTO ";
                                strSql = strSql + ComNum.VBLF + " " + ComNum.DB_PMPA + "NUR_CODE ";                                                                
                                strSql = strSql + ComNum.VBLF + " (Gubun, Code, Name, Jik, PrintRanking) ";
                                strSql = strSql + ComNum.VBLF + " VALUES ('" + strGubun + "','" + strCode + "', ";
                                strSql = strSql + ComNum.VBLF + "  '" + strName + "', '" + strJik + "', " + nSeq + " )";
                             
                               
                                clsDB.ExecuteNonQuery(strSql);

                                clsDB.setCommitTran();
                                ComFunc.MsgBox("등록하였습니다.");
                            }
                            catch (Exception e)
                            {
                                clsDB.setRollbackTran();
                                ComFunc.MsgBox(e.Message);
                            }
                        }

                        if (strRowid != "")
                        {
                            clsDB.setBeginTran();
                            try
                            {
                                strSql = "";
                                strSql = strSql + ComNum.VBLF + "UPDATE ";
                                strSql = strSql + ComNum.VBLF + " " + ComNum.DB_PMPA + "NUR_CODE ";                                
                                strSql = strSql + ComNum.VBLF + " SET Code = '" + strCode + "', ";
                                strSql = strSql + ComNum.VBLF + " Jik = '" + strJik + "', ";
                                strSql = strSql + ComNum.VBLF + " Name = '" + strName + "', PrintRanking = " + nSeq + " ";                                
                                strSql = strSql + ComNum.VBLF + " WHERE ROWID = '" + strRowid + "' ";
                                clsDB.ExecuteNonQuery(strSql);

                                clsDB.setCommitTran();
                                ComFunc.MsgBox("수정하였습니다.");
                            }
                            catch (Exception e)
                            {
                                clsDB.setRollbackTran();
                                ComFunc.MsgBox(e.Message);
                            }
                        }
                    }
                }

            }

        }

        //셀에서 데이터를 수정할 경우 이벤트 발생
       
        void ssCode_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0) return;

            ssCode_Sheet1.Cells[e.Row, (int)eNurCode.Change].Text = "Y";
        }

        void SetSpreadHeaderVisble(string strGbn , FarPoint.Win.Spread.SheetView Spd)
        {
            
            if(strGbn !="A" && strGbn != "B" && strGbn != "P")
            {
                Spd.ColumnHeader.Cells[0, (int)eNurCode.Code].Text = "코드";
                Spd.ColumnHeader.Cells[0, (int)eNurCode.CodeName].Text = "코드명칭";


                Spd.Columns[(int)eNurCode.Seq01].Visible = true;
                Spd.Columns[(int)eNurCode.Jik].Visible = false;
                Spd.Columns[(int)eNurCode.JikName].Visible = false;
                Spd.Columns[(int)eNurCode.Remark].Visible = true;


                this.Width = 593;

            }
            else
            {
                Spd.ColumnHeader.Cells[0, (int)eNurCode.Code].Text = "사번";
                Spd.ColumnHeader.Cells[0, (int)eNurCode.CodeName].Text = "성명";

                Spd.Columns[(int)eNurCode.Seq01].Visible = false;
                Spd.Columns[(int)eNurCode.Jik].Visible = true;
                Spd.Columns[(int)eNurCode.JikName].Visible = true;
                Spd.Columns[(int)eNurCode.Remark].Visible = false;

                // 폼 넓이 조절
                if (strGbn != "P")
                {
                    this.Width = 839;
                }
                else
                    this.Width = 593;

            }




        }

        void cboCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = VB.Left(cboCode.SelectedItem.ToString(),1);

            SetSpreadHeaderVisble(str, ssCode_Sheet1);
        }

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int nRow = 0;

            if (e.Row < 0 || e.Column < 0) return;

            nRow = ssCode_Sheet1.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1;

            ssCode_Sheet1.Cells[nRow, (int)eNurCode.Code].Text = ssList_Sheet1.Cells[e.Row, 0].Text;
            ssCode_Sheet1.Cells[nRow, (int)eNurCode.CodeName].Text = ssList_Sheet1.Cells[e.Row, 1].Text;
            ssCode_Sheet1.Cells[nRow, (int)eNurCode.Jik].Text = ssList_Sheet1.Cells[e.Row, 2].Text;
            ssCode_Sheet1.Cells[nRow, (int)eNurCode.JikName].Text = ssList_Sheet1.Cells[e.Row, 3].Text;

            ssCode_Sheet1.Cells[nRow, (int)eNurCode.Change].Text = "Y"; //변경표시


            ssCode_Sheet1.RowCount = ssCode_Sheet1.RowCount + 1;
            

        }

        void btnView_Click(object sender, EventArgs e)
        {
            GetData2(ssList_Sheet1, txtSearch.Text.Trim());
        }
    }
}
