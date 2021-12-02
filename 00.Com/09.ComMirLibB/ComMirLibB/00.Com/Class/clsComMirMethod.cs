using ComBase; //기본 클래스
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ComMirLibB.Com
{

    /// <summary>
    /// Class Name      : ComMirLibB.Com
    /// File Name       : clsComMirMethod.cs
    /// Description     : 청구 공통 함수들 모음
    /// Author          : 전종윤
    /// Create Date     : 2017-12-05
    /// Update History  : 
    /// </summary>
    /// <history>       
    /// </history>
    /// <seealso cref= "신규" />
    public class clsComMirMethod : clsComMirParam
    {
        /// <summary>
        /// DtNO -> DeptName 리턴
        /// cFunc.GetBCODENameCode(clsDB.DbCon, "MIR_청구상세진료과", 1,Val(TID.DTno)); 함수 사용할것 (DB조회방식)
        /// </summary>
        /// <param name="Dtno">DtNo</param>
        /// <returns></returns>
        public string getDtName_Dtno(string Dtno)
        {
            string rtnString = "";

            switch (Dtno)
            {
                case "11": rtnString = "내과"; break;
                case "12": rtnString = "인공신장"; break;
                case "13": rtnString = "신경정신과"; break;
                case "14": rtnString = "신경과"; break;
                case "15": rtnString = "영상의학과"; break;
                case "16": rtnString = "가정의학과"; break;

                case "21": rtnString = "일반외과"; break;
                case "22": rtnString = "흉부외과"; break;
                case "23": rtnString = "신경외과"; break;
                case "24": rtnString = "정형외과"; break;
                //case "25": rtnString = "성형외과"; break;
                case "25": rtnString = "산업의학과"; break;
                case "26": rtnString = "마취과(AN)"; break;
                case "27": rtnString = "응급실"; break;
                case "28": rtnString = "재활의학"; break;
                case "29": rtnString = "마취과(PC)"; break;

                case "31": rtnString = "산부인과"; break;
                case "32": rtnString = "소아과"; break;
                case "41": rtnString = "안과"; break;
                case "42": rtnString = "이비인후과"; break;
                case "51": rtnString = "피부과"; break;
                case "52": rtnString = "비뇨기과"; break;
                case "61": rtnString = "치과"; break;
            }

            return rtnString;
        }

        /// <summary>구분자가 있는 문자의 코드 갖고 오기
        /// </summary>
        /// <param name="s">대상 문자</param>
        /// <param name="gubun">구분짓는 문자</param>
        /// <returns>12345.생화학 -> 12345를 반환</returns>
        public string getGubunText(string s, string gubun) // 12345.생화학 -> 12345를 반환
        {
            string strReturn = "";
            strReturn = s;

            if (strReturn != null && strReturn.Length > 0 && strReturn.IndexOf(gubun) > 0)
            {
                strReturn = strReturn.Substring(0, strReturn.IndexOf(gubun));
                if (strReturn.ToUpper() == "NULL" || strReturn.IndexOf('*') == 0)
                {
                    strReturn = "*";
                }
            }

            return strReturn;
        }

        /// <summary>진료지원콤보박스설정</summary>
        /// <param name="cbo">콤버박스</param>
        /// <param name="str">설정명</param>
        /// <param name="e">타입</param>
        public void setCombo_View(ComboBox cbo, string[] str, enmComParamComboType e)
        {
            if (str != null)
            {

                string s = getGubunText(str[0], ".");
                string s2 = string.Empty;


                cbo.Text = null;
                cbo.Items.Clear();
                if (e == enmComParamComboType.ALL)
                {
                    for (int i = 0; i < s.Length; i++)
                    {
                        s2 += "*";
                    }
                    cbo.Items.Add(s2 + ".전체");
                }
                else if (e == enmComParamComboType.NULL)
                {
                    cbo.Items.Add("");
                }

                for (int i = 0; i < str.Length; i++)
                {
                    cbo.Items.Add(str[i].ToString());
                }

                if (cbo.Items.Count > 0) cbo.SelectedIndex = 0;
            }
            else
            {
                cbo.Text = null;
                cbo.Items.Clear();
                cbo.Items.Add("");
            }
        }

        /// <summary>진료지원콤보박스설정</summary>
        /// <param name="cbo">콤버박스</param>
        /// <param name="dt">조회된 값으로 반드시 컬럼은 1개이며 코드.코드명으로 나와야 함</param>
        /// <param name="e">타입</param>
        public void setCombo_View(ComboBox cbo, DataTable dt, enmComParamComboType e)
        {
            if (ComFunc.isDataTableNull(dt) == false)
            {
                cbo.Text = null;
                cbo.Items.Clear();

                string s = getGubunText(dt.Rows[0][0].ToString(), ".");
                string s2 = string.Empty;

                if (e == enmComParamComboType.ALL)
                {
                    for (int i = 0; i < s.Length; i++)
                    {
                        s2 += "*";
                    }
                    cbo.Items.Add(s2 + ".전체");

                }
                else if (e == enmComParamComboType.NULL)
                {
                    cbo.Items.Add("");
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cbo.Items.Add(dt.Rows[i][0].ToString().Trim());
                }

                if (cbo.Items.Count > 0) cbo.SelectedIndex = 0;

            }
            else
            {
                cbo.Text = null;
                cbo.Items.Clear();
                cbo.Items.Add("");
            }
        }

        /// <summary>진료지원콤보박스설정</summary>
        /// <param name="cbo">콤버박스</param>
        /// <param name="lstStr">조회된 값</param>
        /// <param name="e">타입</param>
        public void setCombo_View(ComboBox cbo, List<string> lstStr, enmComParamComboType e, string strTEXT = "")
        {
            try
            {
                if (lstStr.Count > 0)
                {
                    //cbo.Text = null;
                    cbo.Items.Clear();
                    string s = getGubunText(lstStr[0], ".");
                    string s2 = string.Empty;

                    if (e == enmComParamComboType.ALL)
                    {
                        for (int i = 0; i < s.Length; i++)
                        {
                            s2 += "*";
                        }
                        cbo.Items.Add(s2 + ".전체");
                    }
                    else if (e == enmComParamComboType.NULL)
                    {
                        cbo.Items.Add("");
                    }

                    for (int i = 0; i < lstStr.Count; i++)
                    {
                        cbo.Items.Add(lstStr[i].ToString().Trim());
                    }

                    if (string.IsNullOrEmpty(strTEXT) == true)
                    {
                        if (cbo.Items.Count > 0) cbo.SelectedIndex = 0;
                    }
                    else
                    {
                        cbo.Text = strTEXT;
                    }

                }
                else
                {
                    cbo.Text = null;
                    cbo.Items.Clear();
                    cbo.Items.Add("");
                }

            }
            catch (Exception)
            {
                cbo.Text = null;
                cbo.Items.Clear();
                cbo.Items.Add("");


            }

        }

        /// <summary>청구 ListView설정</summary>
        /// <param name="cbo">ListView</param>
        /// <param name="dt">조회된 값으로 반드시 컬럼은 1개이며 코드.코드명으로 나와야 함</param>
        /// <param name="e">타입</param>
        public void setListView_View(ListView listview, DataTable dt, enmComParamComboType e)
        {
            ListViewItem item;
            if (ComFunc.isDataTableNull(dt) == false)
            {
                //listview.Text = null;
                listview.Items.Clear();

                //ListViewItem item = new ListViewItem(dt.Rows[i]["Sucode"].ToString().Trim());
                //item.SubItems.Add(dt.Rows[i]["Hcode"].ToString().Trim());
                //item.SubItems.Add(strSuname);
                //List1.Items.Add(item);



                string s = getGubunText(dt.Rows[0][0].ToString(), ".");
                string s2 = string.Empty;

                if (e == enmComParamComboType.ALL)
                {
                    for (int i = 0; i < s.Length; i++)
                    {
                        s2 += "*";
                    }
                    listview.Items.Add(s2 + ".전체");

                }
                else if (e == enmComParamComboType.NULL)
                {
                    listview.Items.Add("");
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    item = new ListViewItem(dt.Rows[i][0].ToString().Trim());
                    listview.Items.Add(item);
                }

                //if (listview.Items.Count > 0) listview.SelectedIndex = 0;
            }
            else
            {
                //cbo.Text = null;
                listview.Items.Clear();
                listview.Items.Add("");
            }
        }

        /// <summary>
        /// nCol: list의 컬럼갯수
        /// </summary>
        /// <param name="listview"></param>
        /// <param name="dt"></param>
        /// <param name="e"></param>
        /// <param name="nCol"></param>
        public void setListView_View(ListView listview, DataTable dt, enmComParamComboType e, int nCol )
        {
            ListViewItem item;
            if (ComFunc.isDataTableNull(dt) == false)
            { 
                //listview.Text = null;
                listview.Items.Clear();

                //ListViewItem item = new ListViewItem(dt.Rows[i]["Sucode"].ToString().Trim());
                //item.SubItems.Add(dt.Rows[i]["Hcode"].ToString().Trim());
                //item.SubItems.Add(strSuname);
                //List1.Items.Add(item);



                string s = getGubunText(dt.Rows[0][0].ToString(), ".");
                string s2 = string.Empty;

                if (e == enmComParamComboType.ALL)
                {
                    for (int i = 0; i < s.Length; i++)
                    {
                        s2 += "*";
                    }
                    listview.Items.Add(s2 + ".전체");

                }
                else if (e == enmComParamComboType.NULL)
                {
                    listview.Items.Add("");
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    item = new ListViewItem(dt.Rows[i][0].ToString().Trim());

                    for (int j = 1; j < nCol; j++)
                    {
                        item.SubItems.Add(dt.Rows[i][j].ToString().Trim());
                    }
                    listview.Items.Add(item);
                }

                //if (listview.Items.Count > 0) listview.SelectedIndex = 0;
            }
            else
            {
                //cbo.Text = null;
                listview.Items.Clear();
                listview.Items.Add("");
            }
        }

        //Mid
        /// <summary>
        /// 문자열 원본의 지정한 위치에서 부터 추출할 갯수 만큼 문자열을 가져옵니다.
        /// </summary>
        /// <param name="sString">문자열 원본</param>
        /// <param name="nStart">추출을 시작할 위치</param>
        /// <param name="nLength">추출할 갯수</param>
        /// <returns>추출된 문자열</returns>
        public string Mid(string sString, int nStart, int nLength)
        {
            string sReturn;

            //VB에서 문자열의 시작은 0이 아니므로 같은 처리를 하려면 
            //스타트 위치를 인덱스로 바꿔야 하므로 -1을 하여
            //1부터 시작하면 0부터 시작하도록 변경하여 준다.
            --nStart;

            //시작위치가 데이터의 범위를 안넘겼는지?
            if (nStart <= sString.Length)
            {
                //안넘겼다.

                //필요한 부분이 데이터를 넘겼는지?
                if ((nStart + nLength) <= sString.Length)
                {
                    //안넘겼다.
                    sReturn = sString.Substring(nStart, nLength);
                }
                else
                {
                    //넘겼다.

                    //데이터 끝까지 출력
                    sReturn = sString.Substring(nStart);
                }

            }
            else
            {
                //넘겼다.

                //그렇다는 것은 데이터가 없음을 의미한다.
                sReturn = string.Empty;
            }

            return sReturn;
        }
        //Left
        /// <summary>
        /// 문자열 원본에서 왼쪽에서 부터 추출한 갯수만큼 문자열을 가져옵니다.
        /// </summary>
        /// <param name="sString">문자열 원본</param>
        /// <param name="nLength">추출할 갯수</param>
        /// <returns>추출된 문자열</returns>
        public string Left(string sString, int nLength)
        {
            string sReturn;

            //추출할 갯수가 문자열 길이보다 긴지?
            if (nLength > sString.Length)
            {
                //길다!

                //길다면 원본의 길이만큼 리턴해 준다.
                nLength = sString.Length;
            }

            //문자열 추출
            sReturn = sString.Substring(0, nLength);

            return sReturn;
        }

        //Right
        /// <summary>
        /// 문자열 원본에서 오른쪽에서 부터 추출한 갯수만큼 문자열을 가져옵니다.
        /// </summary>
        /// <param name="sString">문자열 원본</param>
        /// <param name="nLength">추출할 갯수</param>
        /// <returns>추출된 문자열</returns>
        public string Right(string sString, int nLength)
        {
            string sReturn;

            //추출할 갯수가 문자열 길이보다 긴지?
            if (nLength > sString.Length)
            {
                //길다!

                //길다면 원본의 길이만큼 리턴해 준다.
                nLength = sString.Length;
            }

            //문자열 추출
            sReturn = sString.Substring(sString.Length - nLength, nLength);

            return sReturn;
        }

        /// <summary>
        /// 문자열 Byte로 계산하여 공백 수 만큼 Left, Right 정렬 
        /// </summary>
        /// <param name="argString">사용할 문자열</param>
        /// <param name="argNum">공백 수</param>
        /// <param name="argWay">'L' - 왼쪽 정렬, 'R' - 오른쪽 정렬</param>
        /// <returns></returns>
        public string ByteStr(string argString, int argNum, char argWay)
        {
            byte[] byte1;
            byte[] byte2;

            string rtnVal = "";

            if (argWay == 'L')
            {
                byte1 = Encoding.Default.GetBytes(argString + VB.Space(argNum));
                byte2 = new byte[argNum];

                Array.Copy(byte1, byte2, argNum);

                rtnVal = Encoding.Default.GetString(byte2);
            }
            else if (argWay == 'R')
            {
                byte1 = Encoding.Default.GetBytes(VB.Space(argNum) + argString);
                byte2 = new byte[argNum];

                Array.Copy(byte1, byte2, argNum);

                rtnVal = Encoding.Default.GetString(byte2);
            }

            return rtnVal;
        }

        /// <summary>
        /// CRLF 값을 " " 공백 한칸으로 바꾸는 함수
        /// </summary>
        /// <param name="ArgData">해당 문자열</param>
        /// <returns></returns>
        public string CRLF_Remove(string ArgData)
        {
            string rtnData = string.Empty;

            try
            {
                rtnData = "";
                foreach (char argChar in ArgData)
                {
                    switch (argChar)
                    {
                        case '\r': rtnData = rtnData + VB.Space(1); break;
                        case '\n':
                        default: rtnData = rtnData + argChar; break;
                    }
                }

                return rtnData.Trim();
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 폼을 UI쓰레드를 따로 두어 생성한다. (1개의 유일한 폼) 
        /// </summary>
        /// <param name="nameSpace">폼이 속한 네임스페이스명</param>
        /// <param name="frmName">폼명(문자열)</param>
        /// <param name="argFrm">사용할 폼(폼)</param>
        /// <param name="argAb">부르기전 Assembly</param>
        public void ThreadUIForm(string nameSpace, string frmName, Form argFrm, Assembly argAb)
        {
            //폼이 존재하는지 검사 (기존 폼 포커싱)
            Assembly Ab = argAb;
            Form frm = (Form)Ab.CreateInstance(string.Format("{0}.{1}", nameSpace, frmName));

            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == frm.Name)
                {
                    form.Invoke(new Action(() =>
                    {
                        form.Activate();
                        form.BringToFront();
                    }));
                    return;
                }
            }

            //폼을 처음 연다면

            Thread thread = new Thread(() =>
            {
                argFrm.ShowDialog();
            });
            thread.Start();

        }
    }
}
