using ComBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrAnItem : Form
    {
        private bool IsStart;
        private int RowIndex = 0;
        private List<MedcationInfo> MedcationInfos;
        public bool IsSave;

        public frmEmrAnItem()
        {
            InitializeComponent();
        }

        public frmEmrAnItem(List<MedcationInfo> list, bool start)
        {
            InitializeComponent();
            MedcationInfos = list;
            IsStart = start;
        }

        /// <summary>
        /// 폼로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmEmrAnItem_Load(object sender, EventArgs e)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            string strItemName = "";
            SetInit();

            if (IsStart == true)
            {
                try
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT BASCD, BASNAME, BASEXNAME, 'A' DISPSTR, DISSEQNO  ";
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.AEMRBASCD a                            ";
                    SQL = SQL + ComNum.VBLF + " WHERE BSNSCLS = '기록지관리'                            ";
                    SQL = SQL + ComNum.VBLF + "   AND UNITCLS = '마취기록지기본항목'                    ";
                    SQL = SQL + ComNum.VBLF + "UNION ALL                                                ";
                    SQL = SQL + ComNum.VBLF + "SELECT BASCD, BASNAME, BASEXNAME, 'B' DISPSTR, DISSEQNO  ";
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.AEMRBASCD a                            ";
                    SQL = SQL + ComNum.VBLF + " WHERE BSNSCLS = '기록지관리'                            ";
                    SQL = SQL + ComNum.VBLF + "   AND UNITCLS = '마취기록지항목'                        ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY DISPSTR, DISSEQNO                               ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            strItemName = dt.Rows[i]["BASNAME"].ToString();

                            for (int j = 0; j < ssItemList.ActiveSheet.RowCount; j++)
                            {
                                if (ssItemList.ActiveSheet.Cells[j, 1].Text.Trim() == strItemName.Trim())
                                {
                                    ssItemList.ActiveSheet.Cells[j, 0].Locked = true;
                                }
                            }
                        }
                    }

                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBoxEx(this, ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
            }

        }

        /// <summary>
        /// 나가기 폼닫기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 엔터키
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetData();
            }
        }

        /// <summary>
        /// 조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            for(int i=0; i<ssItem.ActiveSheet.RowCount; i++)
            {
                if(Convert.ToBoolean(ssItem.ActiveSheet.Cells[i, 0].Value))
                {
                    ssItem.ActiveSheet.Cells[i, 0].Value = false;

                    string itemName = ssItem.ActiveSheet.Cells[i, 1].Text;
                    string itemUnit = ssItem.ActiveSheet.Cells[i, 2].Text;
                    string itemCode = ssItem.ActiveSheet.Cells[i, 3].Text;

                    //  중복체크
                    if (!IsContains(itemName))
                    {
                        ssItemList.ActiveSheet.Cells[RowIndex, 1].Text = itemName;
                        ssItemList.ActiveSheet.Cells[RowIndex, 2].Text = itemUnit;
                        ssItemList.ActiveSheet.Cells[RowIndex, 3].Text = itemCode;
                        ssItemList.ActiveSheet.Cells[RowIndex, 4].Text = "-1";
                        ssItemList.ActiveSheet.Cells[RowIndex, 5].Text = "true";
                        ssItemList.ActiveSheet.Cells[RowIndex, 6].Text = "true";

                        ssItemList.ActiveSheet.Cells[RowIndex, 1].Locked = true;
                        ssItemList.ActiveSheet.Cells[RowIndex, 2].Locked = true;
                        RowIndex++;
                    }
                }
            }
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            List<AnItem> list = new List<AnItem>();
            for (int i = 0; i < ssItemList.ActiveSheet.RowCount; i++)
            {
                if (i < 7)
                {
                    ssItemList.ActiveSheet.Cells[i, 0].Value = false;
                }

                if (!Convert.ToBoolean(ssItemList.ActiveSheet.Cells[i, 0].Value) && 
                    !string.IsNullOrWhiteSpace(ssItemList.ActiveSheet.Cells[i, 1].Text))
                {
                    AnItem item = new AnItem
                    {
                        ItemName = ssItemList.ActiveSheet.Cells[i, 1].Text,
                        ItemUnit = ssItemList.ActiveSheet.Cells[i, 2].Text,
                        ItemCode = ssItemList.ActiveSheet.Cells[i, 3].Text,
                        row = i < 7 ? ssItemList.ActiveSheet.Cells[i, 4].Text : "-1",
                        IsWrite = i < 7 ? ssItemList.ActiveSheet.Cells[i, 5].Text : "true",
                        IsView = i < 7 ? ssItemList.ActiveSheet.Cells[i, 6].Text : "true"
                    };

                    list.Add(item);
                }

                ssItemList.ActiveSheet.Cells[i, 0].Value = false;
                ssItemList.ActiveSheet.Cells[i, 1].Text = string.Empty;
                ssItemList.ActiveSheet.Cells[i, 2].Text = string.Empty;
                ssItemList.ActiveSheet.Cells[i, 3].Text = string.Empty;
                ssItemList.ActiveSheet.Cells[i, 4].Text = string.Empty;
                ssItemList.ActiveSheet.Cells[i, 5].Text = string.Empty;
                ssItemList.ActiveSheet.Cells[i, 6].Text = string.Empty;

                ssItemList.ActiveSheet.Cells[i, 1].Locked = false;
                ssItemList.ActiveSheet.Cells[i, 2].Locked = false;
            }

            
            for (int i = 0; i < list.Count; i++)
            {
                ssItemList.ActiveSheet.Cells[i, 1].Text = list[i].ItemName;
                ssItemList.ActiveSheet.Cells[i, 2].Text = list[i].ItemUnit;
                ssItemList.ActiveSheet.Cells[i, 3].Text = list[i].ItemCode;
                ssItemList.ActiveSheet.Cells[i, 4].Text = list[i].row;
                ssItemList.ActiveSheet.Cells[i, 5].Text = list[i].IsWrite;
                ssItemList.ActiveSheet.Cells[i, 6].Text = list[i].IsView;
                
                ssItemList.ActiveSheet.Cells[i, 1].Locked = true;
                ssItemList.ActiveSheet.Cells[i, 2].Locked = true;
            }
            
            RowIndex = list.Count;
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            IsSave = true;
            btnExit.PerformClick();
        }

        /// <summary>
        /// 위로
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUp_Click(object sender, EventArgs e)
        {
            if (!IsOneCheck())
            {
                ComFunc.MsgBoxEx(this, "체크는 한 항목만 할수 있습니다.");
                return;
            }

            for (int i = 0; i < ssItemList.ActiveSheet.RowCount; i++)
            {
                if (Convert.ToBoolean(ssItemList.ActiveSheet.Cells[i, 0].Value))
                {
                    if (i - 1 < 0)
                    {
                        return;
                    }

                    ssItemList.ActiveSheet.SwapRange(i, 0, i - 1, 0, 1, 3, true);
                    return;
                }
            }
        }

        /// <summary>
        /// 아래로
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDown_Click(object sender, EventArgs e)
        {
            if (!IsOneCheck())
            {
                ComFunc.MsgBoxEx(this, "체크는 한 항목만 할수 있습니다.");
                return;
            }

            for (int i = 0; i < ssItemList.ActiveSheet.RowCount; i++)
            {
                if (Convert.ToBoolean(ssItemList.ActiveSheet.Cells[i, 0].Value))
                {
                    if (i + 1 >= ssItemList.ActiveSheet.RowCount)
                    {
                        return;
                    }

                    string nextItemName = ssItemList.ActiveSheet.Cells[i + 1, 1].Text;
                    if (string.IsNullOrWhiteSpace(nextItemName))
                    {
                        return;
                    }

                    ssItemList.ActiveSheet.SwapRange(i, 0, i + 1, 0, 1, 3, true);
                    return;
                }
            }
        }

        /// <summary>
        /// 초기설정
        /// </summary>
        public void SetInit()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            if (MedcationInfos == null || MedcationInfos.Count == 0)
            {
                MedcationInfos = new List<MedcationInfo>
                {
                    new MedcationInfo{Name = "SBP", Uint = "mmHg", IsView = false, Row = 100, ItemNo = "I0000002018"},
                    new MedcationInfo{Name = "DBP", Uint = "mmHg", IsView = false, Row = 101, ItemNo = "I0000001765"},
                    new MedcationInfo{Name = "맥박", Uint = "", IsView = false, Row = 102, ItemNo = "I0000001178"},
                    new MedcationInfo{Name = "호흡", Uint = "", IsView = false, Row = 103, ItemNo = "I0000000574"},
                    
                    new MedcationInfo{Name = "마취", Uint = "", IsView = true, IsWrite = false, ItemNo = "I0000001164"},
                    new MedcationInfo{Name = "수술", Uint = "", IsView = true, IsWrite = false, ItemNo = "I0000001422"},
                    new MedcationInfo{Name = "삽관", Uint = "", IsView = true, IsWrite = false, ItemNo = "I0000012402"},
                
                    //new MedcationInfo{Name = "N2O", Uint = "ℓ/min", ItemNo = "I0000015889"},
                    //new MedcationInfo{Name = "O2", Uint = "ℓ/min", ItemNo = "I0000022307"},
                    //new MedcationInfo{Name = "Sevo", Uint = "%", ItemNo = "I0000034070"},
                    //new MedcationInfo{Name = "Propofol", Uint = "㎖/hrs", ItemNo = "I0000010880"},
                    //new MedcationInfo{Name = "SpO2 (%)", Uint = "%", ItemNo = "I0000008708"},
                    //new MedcationInfo{Name = "ETCO2", Uint = "mmHg", ItemNo = "I0000031627"},
                    //new MedcationInfo{Name = "Temp", Uint = "℃", ItemNo = "I0000022536"},
                    //new MedcationInfo{Name = "EKG monitoring", Uint = "", ItemNo = "I0000030240"},
                    //new MedcationInfo{Name = "U/O", Uint = "㎖/hrs", ItemNo = "I0000034071"},
                    //new MedcationInfo{Name = "Warm blanket", Uint = "", ItemNo = "I0000034072"},
                    //new MedcationInfo{Name = "Tourniquet", Uint = "", ItemNo = "I0000030758"},
                };




                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT BASCD, BASNAME, BASEXNAME, DISSEQNO   ";
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.AEMRBASCD a                ";
                    SQL = SQL + ComNum.VBLF + " WHERE BSNSCLS = '기록지관리'                ";
                    SQL = SQL + ComNum.VBLF + "   AND UNITCLS = '마취기록지항목'            ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY DISSEQNO                            ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            MedcationInfos.Add(new MedcationInfo { Name = dt.Rows[i]["BASNAME"].ToString(), Uint = dt.Rows[i]["BASEXNAME"].ToString(), ItemNo = dt.Rows[i]["BASCD"].ToString() });                            
                        }
                    }

                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    ComFunc.MsgBoxEx(this, ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                
            }

            for(int i=0; i<MedcationInfos.Count; i++)
            {                
                ssItemList.ActiveSheet.Cells[i, 1].Text = MedcationInfos[i].Name;
                ssItemList.ActiveSheet.Cells[i, 2].Text = MedcationInfos[i].Uint;

                ssItemList.ActiveSheet.Cells[i, 3].Text = MedcationInfos[i].ItemNo;                 //  ItemNo
                ssItemList.ActiveSheet.Cells[i, 4].Value = MedcationInfos[i].Row;                   //  Row
                ssItemList.ActiveSheet.Cells[i, 5].Text = MedcationInfos[i].IsWrite.ToString();     //  IsWrite
                ssItemList.ActiveSheet.Cells[i, 6].Text = MedcationInfos[i].IsView.ToString();      //  IsView
            }

            RowIndex = ssItemList.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data) + 1;
        }

        /// <summary>
        /// 중복확인
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool IsContains(string name)
        {
            for (int i = 0; i < ssItemList.ActiveSheet.RowCount; i++)
            {
                string itemName = ssItemList.ActiveSheet.Cells[i, 1].Text;
                if (itemName.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 순서정렬 하나만 체크 확인
        /// </summary>
        /// <returns></returns>
        private bool IsOneCheck()
        {
            int count = 0;
            for (int i = 0; i < ssItemList.ActiveSheet.RowCount; i++)
            {
                if (Convert.ToBoolean(ssItemList.ActiveSheet.Cells[i, 0].Value))
                {
                    count++;
                }
            }

            return count == 1;
        }

        /// <summary>
        /// 데이터 조회
        /// </summary>
        private void GetData()
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ITEMNO, ITEMNAME, ITEMUNIT";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRITEM";
                SQL = SQL + ComNum.VBLF + " WHERE 1 = 1";
                SQL = SQL + ComNum.VBLF + "   AND (UPPER(ITEMNAME) LIKE '%" + txtWord.Text.ToUpper() + "%' OR UPPER(ITEMINDEXNM) LIKE '%" + txtWord.Text.ToUpper() + "%')";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }

                ssItem.ActiveSheet.Rows.Count = dt.Rows.Count;
                for(int i=0; i<dt.Rows.Count; i++)
                {
                    ssItem.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["ITEMNAME"].ToString();
                    ssItem.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["ITEMUNIT"].ToString();
                    ssItem.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["ITEMNO"].ToString();
                }

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssItemList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row < 7)
            {
                ssItemList.ActiveSheet.Cells[e.Row, 0].Value = false;
            }
        }
    }

    public class AnItem
    {
        public string ItemName;
        public string ItemUnit;
        public string ItemCode;
        public string row;
        public string IsWrite;
        public string IsView;
    }
}
