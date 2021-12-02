using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComBase;
using ComDbB;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;

namespace ComMirLibB.MirTong
{
    public class clsComMirTong
    {
        /// <summary>
        /// 진료과코드 셋팅하는 함수
        /// </summary>
        /// <param name="argCombo">콤보박스</param>
        /// <param name="strAll">1: **.전체</param>
        /// <param name="intType">1: 코드 + "." + 명칭 2: 코드 3: 명칭</param>
        /// <param name="lstNotIn">List(string) 타입으로 해당 코드들 제외하여 셋팅 </param>
        public void SetComboDept(PsmhDb pDbCon, ComboBox argCombo, string strAll = "", int intType = 0, List<string> lstNotIn = null)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int i = 0;

            if (intType != 2 && intType != 3)
                intType = 1;

            if (strAll == "")
                strAll = "1";

            argCombo.Items.Clear();

            try
            {
                SQL = "";
                SQL = "SELECT DEPTCODE, DEPTNAMEK FROM KOSMOS_PMPA.BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + " WHERE 1=1  ";
                if (lstNotIn != null)
                {                    
                    foreach (string DeptCode in lstNotIn)
                    {
                        SQL = SQL + ComNum.VBLF + " AND DeptCode NOT IN ('" + DeptCode + "') ";
                    }                    
                }
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {

                    if (strAll == "1")
                    {
                        switch (intType)
                        {

                            case 1:
                                argCombo.Items.Add("**.전체");
                                break;

                            case 2:
                                argCombo.Items.Add("**");
                                break;

                            case 3:
                                argCombo.Items.Add("전체");
                                break;
                        }

                    }

                    for (i = 0; i < dt.Rows.Count; i++)
                    {

                        switch (intType)
                        {

                            case 1:
                                argCombo.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                                break;

                            case 2:
                                argCombo.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim());
                                break;

                            case 3:
                                argCombo.Items.Add(dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                                break;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                argCombo.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                MessageBox.Show(ex.Message);
            }
        }

        public void SetComboEdiHang(ComboBox argCombo)
        {
            argCombo.Items.Add("**.전체");
            argCombo.Items.Add("01.진찰료");
            argCombo.Items.Add("02.입원료");
            argCombo.Items.Add("03.투약료");
            argCombo.Items.Add("04.주사료");
            argCombo.Items.Add("05.마취료");
            argCombo.Items.Add("06.이학요법료");
            argCombo.Items.Add("07.정신요법료");
            argCombo.Items.Add("08.처치 및 수술료");
            argCombo.Items.Add("09.검사료");
            argCombo.Items.Add("10.영상진단및 방사선치료료");
            argCombo.Items.Add("L .장기요양");
            argCombo.Items.Add("S .특수장비");
            argCombo.Items.Add("V .100/100");
            argCombo.Items.Add("W .비급여");

            argCombo.SelectedIndex = 0;
        }


    }
}
