//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.VisualBasic;



/// <summary>
/// Description : 신용카드승인(다우데이터)
/// Author : 박병규
/// Create Date : 2017.09.19
/// </summary>
/// <history>
/// ComPmpaLibB 프로젝트의 clsCardApprov 클래스 사용으로 사용안함 주석처리 2018-02-09  KMC
/// </history>
/// <seealso cref="CardApprov_Daou.bas"/> 

//namespace ComBase
//{
//    public class clsCard_Daou
//    {
//        public string FstrHostIP = "127.0.0.1"; //병원서버CARD IP
//        public string FstrPort = "6578"; //병원서버CARD PORT


//        [System.Runtime.InteropServices.DllImport("SvkPos.dll")]
//        public static extern int svk_POS(byte[] input_msg, byte[] output_msg);

//        [System.Runtime.InteropServices.DllImport("SvkPos.dll")]
//        public static extern int svk_PAD(byte[] output_msg, int timeout);

//        [System.Runtime.InteropServices.DllImport("SvkPos.dll")]
//        public static extern int svk_GET_SIGNPORT();

//        [System.Runtime.InteropServices.DllImport("SvkPos.dll")]
//        public static extern int svk_SET_SIGNPORT(int sign_port);

//        [System.Runtime.InteropServices.DllImport("SvkPos.dll")]
//        public static extern int svk_KIOSK_FIX(int Mode, byte[] input_msg, int bitmap_raw_len, string bitmap_raw_data, int passwd_raw_len, string passwd_raw_data, byte[] output_msg);

//        public string FstrMdate; //승인일시
//        public string FstrApprovalNo; //승인번호
//        public string FstrApprovalRemark; //승인메세지



//        //신용카드승인 및 싸인패드 싸인요청
//        public int SvkPos(byte[] input_msg, byte[] output_msg)
//        {
//            int rc;
//            byte[] IsReq = new byte[2049];
//            byte[] IsRep = new byte[2049];

//            IsReq = input_msg;

//            rc = svk_POS(IsReq, IsRep);

//            output_msg = IsRep;

//            return rc;
//        }

//        //신용카드승인 - 테블릿용
//        public int SvkPosSign(byte[] input_msg, string sign_data, int sign_len, byte[] output_msg)
//        {
//            int rc;
//            byte[] IsReq = new byte[2049];
//            byte[] IsRep = new byte[2049];

//            IsReq = input_msg;
//            IsRep = output_msg;

//            rc = svk_KIOSK_FIX(1, IsReq, sign_len, sign_data, 0, "0", IsRep);

//            output_msg = IsRep;

//            return rc;
//        }

//        public StringBuilder insertLeftJustify(StringBuilder target, string item, int maxLen)
//        {
//            int myLen = maxLen;

//            if (item.Length < maxLen)
//            {
//                target.Append(item);

//                myLen = myLen - item.Length;

//                for (int i = 0; i < myLen; i++)
//                {
//                    target.Append(" ");
//                }

//                return target;
//            }
//            else if (item.Length == maxLen)
//            {
//                target.Append(item);

//                return target;
//            }
//            else
//            {
//                for (int i = 0; i < myLen; i++)
//                {
//                    target.Append(item[i]);
//                }

//                return target;
//            }
//        }


//        public StringBuilder insertLeftZero(StringBuilder target, string item, int maxLen)
//        {
//            int myLen = maxLen;

//            if (item.Length < maxLen)
//            {
//                myLen = myLen - item.Length;
//                for (int i = 0; i < myLen; i++)
//                {
//                    target.Append("0");
//                }

//                target.Append(item);

//                return target;
//            }
//            else if (item.Length == maxLen)
//            {
//                target.Append(item);

//                return target;
//            }
//            else
//            {
//                for (int i = 0; i < myLen; i++)
//                {
//                    target.Append(item[i]);
//                }

//                return target;
//            }
//        }


//        private void AddResultGridView(System.Windows.Forms.DataGridView RGV, string itemName, byte[] item)
//        {
//            FstrMdate = "";
//            FstrApprovalNo = "";
//            FstrApprovalRemark = "";

//            string[] row = { itemName, System.Text.Encoding.Default.GetString(item) };

//            RGV.Rows.Add(row);

//            switch (itemName)
//            {
//                case "거래일시":
//                    FstrMdate = System.Text.Encoding.Default.GetString(item);
//                    break;
//                case "승인번호":
//                    FstrApprovalNo = System.Text.Encoding.Default.GetString(item);
//                    break;

//                case "발급사코드":
//                case "발급사명":
//                case "매입구분":
//                case "매입사코드":
//                case "매입사명":
//                case "가맹점번호":
//                    FstrApprovalRemark += System.Text.Encoding.Default.GetString(item);
//                    break;
//            }


//        }


//        public void SetResultRep(System.Windows.Forms.DataGridView RGV, string rep)
//        {
//            int pos = 0;
//            byte[] tmpStr = System.Text.Encoding.Default.GetBytes(rep);
//            byte[] tmp = new byte[1025];
//            int ibcpartnerlen = 0;

//            #region //신용승인
//            if (rep.Substring(0, 6) == "021010")
//            {
//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전문구분", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 2);
//                AddResultGridView(RGV, "업무구분", tmp);
//                pos += 2;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 8);
//                AddResultGridView(RGV, "단말기번호", tmp);
//                pos += 8;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "응답코드", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전표번호", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래고유번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "거래일시", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래금액", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "발급사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "발급사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "매입구분", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "매입사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "매입사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "가맹점번호", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 48);
//                AddResultGridView(RGV, "에러메시지", tmp);
//                pos += 48;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "출력제어코드", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;
//            }
//            #endregion

//            #region //신용취소
//            else if (rep.Substring(0, 6) == "043010")
//            {
//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전문구분", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 2);
//                AddResultGridView(RGV, "업무구분", tmp);
//                pos += 2;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 8);
//                AddResultGridView(RGV, "단말기번호", tmp);
//                pos += 8;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "응답코드", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전표번호", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래고유번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "거래일시", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래금액", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "발급사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "발급사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "매입구분", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "매입사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "매입사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "가맹점번호", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 48);
//                AddResultGridView(RGV, "에러메시지", tmp);
//                pos += 48;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "출력제어코드", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;
//            }
//            #endregion

//            #region //전화승인 등록/등록취소
//            else if ((rep.Substring(0, 6) == "021012") || (rep.Substring(0, 6) == "043012"))
//            {
//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전문구분", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 2);
//                AddResultGridView(RGV, "업무구분", tmp);
//                pos += 2;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 8);
//                AddResultGridView(RGV, "단말기번호", tmp);
//                pos += 8;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "응답코드", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전표번호", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래고유번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "거래일시", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래금액", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "발급사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "발급사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "매입구분", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "매입사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "매입사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "가맹점번호", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 48);
//                AddResultGridView(RGV, "에러메시지", tmp);
//                pos += 48;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "출력제어코드", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;
//            }
//            #endregion

//            #region // B/L 조회
//            else if (rep.Substring(0, 6) == "021016")
//            {
//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전문구분", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 2);
//                AddResultGridView(RGV, "업무구분", tmp);
//                pos += 2;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 8);
//                AddResultGridView(RGV, "단말기번호", tmp);
//                pos += 8;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "응답코드", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전표번호", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래고유번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "거래일시", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래금액", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "발급사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "발급사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "매입구분", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "매입사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "매입사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "가맹점번호", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 48);
//                AddResultGridView(RGV, "에러메시지", tmp);
//                pos += 48;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "출력제어코드", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;
//            }
//            #endregion

//            #region //신용승인+포인트 승인
//            else if (rep.Substring(0, 6) == "021011")
//            {
//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전문구분", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 2);
//                AddResultGridView(RGV, "업무구분", tmp);
//                pos += 2;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 8);
//                AddResultGridView(RGV, "단말기번호", tmp);
//                pos += 8;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "응답코드", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전표번호", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래고유번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "거래일시", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래금액", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "발급사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "발급사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "매입구분", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "매입사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "매입사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "가맹점번호", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "포인트 승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 10);
//                AddResultGridView(RGV, "고객명", tmp);
//                pos += 10;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "발생포인트", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "가용포인트", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "누적포인트", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 48);
//                AddResultGridView(RGV, "에러메시지", tmp);
//                pos += 48;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "출력제어코드", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;
//            }
//            #endregion

//            #region //신용승인+포인트 취소
//            else if (rep.Substring(0, 6) == "043011")
//            {
//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전문구분", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 2);
//                AddResultGridView(RGV, "업무구분", tmp);
//                pos += 2;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 8);
//                AddResultGridView(RGV, "단말기번호", tmp);
//                pos += 8;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "응답코드", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전표번호", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래고유번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "거래일시", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래금액", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "발급사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "발급사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "매입구분", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "매입사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "매입사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "가맹점번호", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "포인트 승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 10);
//                AddResultGridView(RGV, "고객명", tmp);
//                pos += 10;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "발생포인트", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "가용포인트", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "누적포인트", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 48);
//                AddResultGridView(RGV, "에러메시지", tmp);
//                pos += 48;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "출력제어코드", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;
//            }
//            #endregion

//            #region //단말기 개시거래
//            else if (rep.Substring(0, 6) == "021090")
//            {
//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전문구분", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 2);
//                AddResultGridView(RGV, "업무구분", tmp);
//                pos += 2;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 8);
//                AddResultGridView(RGV, "단말기번호", tmp);
//                pos += 8;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "응답코드", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "거래일시", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 24);
//                AddResultGridView(RGV, "가맹점명", tmp);
//                pos += 24;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 40);
//                AddResultGridView(RGV, "가맹점주소", tmp);
//                pos += 40;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 14);
//                AddResultGridView(RGV, "가맹점전화", tmp);
//                pos += 14;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "대표자명", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 2);
//                AddResultGridView(RGV, "정산방식", tmp);
//                pos += 2;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 40);
//                AddResultGridView(RGV, "관리대리점명", tmp);
//                pos += 40;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 14);
//                AddResultGridView(RGV, "관리대리점 전화번호", tmp);
//                pos += 14;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 66);
//                AddResultGridView(RGV, "거래접속정보", tmp);
//                pos += 66;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 66);
//                AddResultGridView(RGV, "거래접속정보", tmp);
//                pos += 66;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 66);
//                AddResultGridView(RGV, "거래접속정보", tmp);
//                pos += 66;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 66);
//                AddResultGridView(RGV, "거래접속정보", tmp);
//                pos += 66;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 48);
//                AddResultGridView(RGV, "에러메시지", tmp);
//                pos += 48;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "출력제어코드", tmp);
//                pos += 1;
//            }
//            #endregion

//            #region //단말기 매입요청
//            else if (rep.Substring(0, 6) == "021013")
//            {
//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전문구분", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 2);
//                AddResultGridView(RGV, "업무구분", tmp);
//                pos += 2;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 8);
//                AddResultGridView(RGV, "단말기번호", tmp);
//                pos += 8;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "응답코드", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전표번호", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래고유번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "거래일시", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래금액", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "발급사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "발급사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "매입구분", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "매입사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "매입사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "가맹점번호", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 48);
//                AddResultGridView(RGV, "에러메시지", tmp);
//                pos += 48;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "출력제어코드", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;
//            }
//            #endregion

//            #region //은련카드 승인
//            else if (rep.Substring(0, 6) == "021017")
//            {
//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전문구분", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 2);
//                AddResultGridView(RGV, "업무구분", tmp);
//                pos += 2;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 8);
//                AddResultGridView(RGV, "단말기번호", tmp);
//                pos += 8;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "응답코드", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전표번호", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래고유번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "거래일시", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래금액", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "발급사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "발급사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "매입구분", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "매입사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "매입사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "가맹점번호", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 48);
//                AddResultGridView(RGV, "에러메시지", tmp);
//                pos += 48;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "출력제어코드", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;
//            }
//            #endregion

//            #region // 은련카드 취소
//            else if (rep.Substring(0, 6) == "043017")
//            {
//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전문구분", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 2);
//                AddResultGridView(RGV, "업무구분", tmp);
//                pos += 2;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 8);
//                AddResultGridView(RGV, "단말기번호", tmp);
//                pos += 8;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "응답코드", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전표번호", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래고유번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "거래일시", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래금액", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "발급사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "발급사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "매입구분", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "매입사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "매입사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "가맹점번호", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 48);
//                AddResultGridView(RGV, "에러메시지", tmp);
//                pos += 48;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "출력제어코드", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;
//            }
//            #endregion

//            #region // 수표조회
//            else if (rep.Substring(0, 6) == "021015")
//            {
//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전문구분", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 2);
//                AddResultGridView(RGV, "업무구분", tmp);
//                pos += 2;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 8);
//                AddResultGridView(RGV, "단말기번호", tmp);
//                pos += 8;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "응답코드", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래고유번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "거래일시", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 48);
//                AddResultGridView(RGV, "에러메시지", tmp);
//                pos += 48;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "출력제어코드", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;
//            }
//            #endregion

//            #region //포인트
//            else if ((rep.Substring(0, 6) == "021030") ||
//                 (rep.Substring(0, 6) == "021031") ||
//                 (rep.Substring(0, 6) == "043031") ||
//                 (rep.Substring(0, 6) == "021032") ||
//                 (rep.Substring(0, 6) == "043032"))
//            {
//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전문구분", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 2);
//                AddResultGridView(RGV, "업무구분", tmp);
//                pos += 2;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 8);
//                AddResultGridView(RGV, "단말기번호", tmp);
//                pos += 8;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "응답코드", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전표번호", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래고유번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "거래일시", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래금액", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "발급사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "발급사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "가맹점번호", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "포인트 승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 10);
//                AddResultGridView(RGV, "고객명", tmp);
//                pos += 10;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "발생포인트", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "가용포인트", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "누적포인트", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 48);
//                AddResultGridView(RGV, "에러메시지", tmp);
//                pos += 48;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "출력제어코드", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;
//            }
//            #endregion

//            #region //현금영수증 판매등록
//            else if (rep.Substring(0, 6) == "021040")
//            {
//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전문구분", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 2);
//                AddResultGridView(RGV, "업무구분", tmp);
//                pos += 2;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 8);
//                AddResultGridView(RGV, "단말기번호", tmp);
//                pos += 8;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "응답코드", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전표번호", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래고유번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "거래일시", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래금액", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 48);
//                AddResultGridView(RGV, "에러메시지", tmp);
//                pos += 48;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "출력제어코드", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;
//            }
//            #endregion

//            #region //현금영수증 판매등록 취소
//            else if (rep.Substring(0, 6) == "043040")
//            {
//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전문구분", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 2);
//                AddResultGridView(RGV, "업무구분", tmp);
//                pos += 2;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 8);
//                AddResultGridView(RGV, "단말기번호", tmp);
//                pos += 8;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "응답코드", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전표번호", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래고유번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "거래일시", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래금액", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 48);
//                AddResultGridView(RGV, "에러메시지", tmp);
//                pos += 48;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "출력제어코드", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;
//            }
//            #endregion

//            #region //현금영수증+포인트 승인
//            else if (rep.Substring(0, 6) == "021041")
//            {
//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전문구분", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 2);
//                AddResultGridView(RGV, "업무구분", tmp);
//                pos += 2;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 8);
//                AddResultGridView(RGV, "단말기번호", tmp);
//                pos += 8;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "응답코드", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전표번호", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래고유번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "거래일시", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래금액", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "포인트 승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 10);
//                AddResultGridView(RGV, "고객명", tmp);
//                pos += 10;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "발생포인트", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "가용포인트", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "누적포인트", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 48);
//                AddResultGridView(RGV, "에러메시지", tmp);
//                pos += 48;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "출력제어코드", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;
//            }
//            #endregion

//            #region //현금영수증+포인트 취소
//            else if (rep.Substring(0, 6) == "043041")
//            {
//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전문구분", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 2);
//                AddResultGridView(RGV, "업무구분", tmp);
//                pos += 2;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 8);
//                AddResultGridView(RGV, "단말기번호", tmp);
//                pos += 8;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "응답코드", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전표번호", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래고유번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "거래일시", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래금액", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "포인트 승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 10);
//                AddResultGridView(RGV, "고객명", tmp);
//                pos += 10;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "발생포인트", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "가용포인트", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "누적포인트", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 48);
//                AddResultGridView(RGV, "에러메시지", tmp);
//                pos += 48;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "출력제어코드", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;
//            }
//            #endregion

//            #region //멤버쉽
//            else if ((rep.Substring(0, 6) == "021033") ||
//                (rep.Substring(0, 6) == "021034") ||
//                (rep.Substring(0, 6) == "043034"))
//            {
//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전문구분", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 2);
//                AddResultGridView(RGV, "업무구분", tmp);
//                pos += 2;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 8);
//                AddResultGridView(RGV, "단말기번호", tmp);
//                pos += 8;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "응답코드", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전표번호", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래고유번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "거래일시", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래금액", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "발급사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "발급사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "가맹점번호", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "멤버쉽 승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 10);
//                AddResultGridView(RGV, "고객명", tmp);
//                pos += 10;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 48);
//                AddResultGridView(RGV, "에러메시지", tmp);
//                pos += 48;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "출력제어코드", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;
//            }
//            #endregion

//            #region // 비씨파트너스 신용카드 승인
//            else if ((rep.Substring(0, 6) == "0210B0") ||
//                (rep.Substring(0, 6) == "0210B1"))
//            {
//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전문구분", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 2);
//                AddResultGridView(RGV, "업무구분", tmp);
//                pos += 2;


//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 8);
//                AddResultGridView(RGV, "단말기번호", tmp);
//                pos += 8;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "응답코드", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전표번호", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래고유번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "거래일시", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래금액", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "발급사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "발급사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "매입구분", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "매입사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "매입사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "가맹점번호", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 48);
//                AddResultGridView(RGV, "에러메시지", tmp);
//                pos += 48;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "출력제어코드", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "파트너스 정보 길이", tmp);
//                pos += 4;

//                if (System.Text.Encoding.Default.GetString(tmp).Trim().Replace("\0", "").Length > 0)
//                {
//                    ibcpartnerlen = Int16.Parse(System.Text.Encoding.Default.GetString(tmp).Trim());

//                    Array.Clear(tmp, 0, tmp.Length);
//                    Array.Copy(tmpStr, pos, tmp, 0, ibcpartnerlen);
//                    AddResultGridView(RGV, "파트너스 정보", tmp);
//                    pos += ibcpartnerlen;
//                }
//            }
//            #endregion

//            #region // 비씨파트너스 신용카드 승인 취소
//            else if ((rep.Substring(0, 6) == "0430B0") ||
//                (rep.Substring(0, 6) == "0430B1"))
//            {
//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전문구분", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 2);
//                AddResultGridView(RGV, "업무구분", tmp);
//                pos += 2;


//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 8);
//                AddResultGridView(RGV, "단말기번호", tmp);
//                pos += 8;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "응답코드", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전표번호", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래고유번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "거래일시", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래금액", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "발급사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "발급사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "매입구분", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "매입사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "매입사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "가맹점번호", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 48);
//                AddResultGridView(RGV, "에러메시지", tmp);
//                pos += 48;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "출력제어코드", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "파트너스 정보 길이", tmp);
//                pos += 4;

//                if (System.Text.Encoding.Default.GetString(tmp).Trim().Replace("\0", "").Length > 0)
//                {
//                    ibcpartnerlen = Int16.Parse(System.Text.Encoding.Default.GetString(tmp).Trim());

//                    Array.Clear(tmp, 0, tmp.Length);
//                    Array.Copy(tmpStr, pos, tmp, 0, ibcpartnerlen);
//                    AddResultGridView(RGV, "파트너스 정보", tmp);
//                    pos += ibcpartnerlen;
//                }
//            }
//            #endregion

//            #region // 비씨파트너스 쿠폰사용/쿠폰조회/원거래조회
//            else if ((rep.Substring(0, 6) == "0210B2") ||
//                (rep.Substring(0, 6) == "0210B3") ||
//                (rep.Substring(0, 6) == "0210B4"))
//            {
//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전문구분", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 2);
//                AddResultGridView(RGV, "업무구분", tmp);
//                pos += 2;


//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 8);
//                AddResultGridView(RGV, "단말기번호", tmp);
//                pos += 8;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "응답코드", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전표번호", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래고유번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "거래일시", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래금액", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "발급사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "발급사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "매입구분", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "매입사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "매입사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "가맹점번호", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 48);
//                AddResultGridView(RGV, "에러메시지", tmp);
//                pos += 48;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "출력제어코드", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "파트너스 정보 길이", tmp);
//                pos += 4;

//                if (System.Text.Encoding.Default.GetString(tmp).Trim().Replace("\0", "").Length > 0)
//                {
//                    ibcpartnerlen = Int16.Parse(System.Text.Encoding.Default.GetString(tmp).Trim());

//                    Array.Clear(tmp, 0, tmp.Length);
//                    Array.Copy(tmpStr, pos, tmp, 0, ibcpartnerlen);
//                    AddResultGridView(RGV, "파트너스 정보", tmp);
//                    pos += ibcpartnerlen;
//                }
//            }
//            #endregion

//            #region // 비씨파트너스 롤파일 다운로드/로드결과
//            else if ((rep.Substring(0, 6) == "0210B5") ||
//                (rep.Substring(0, 6) == "0210B6"))
//            {
//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전문구분", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 2);
//                AddResultGridView(RGV, "업무구분", tmp);
//                pos += 2;


//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 8);
//                AddResultGridView(RGV, "단말기번호", tmp);
//                pos += 8;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "응답코드", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "전표번호", tmp);
//                pos += 4;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래고유번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "거래일시", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "승인번호", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 12);
//                AddResultGridView(RGV, "거래금액", tmp);
//                pos += 12;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "발급사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "발급사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "매입구분", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 3);
//                AddResultGridView(RGV, "매입사코드", tmp);
//                pos += 3;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 16);
//                AddResultGridView(RGV, "매입사명", tmp);
//                pos += 16;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 15);
//                AddResultGridView(RGV, "가맹점번호", tmp);
//                pos += 15;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 48);
//                AddResultGridView(RGV, "에러메시지", tmp);
//                pos += 48;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 1);
//                AddResultGridView(RGV, "출력제어코드", tmp);
//                pos += 1;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 32);
//                AddResultGridView(RGV, "출력메시지", tmp);
//                pos += 32;

//                Array.Clear(tmp, 0, tmp.Length);
//                Array.Copy(tmpStr, pos, tmp, 0, 4);
//                AddResultGridView(RGV, "파트너스 정보 길이", tmp);
//                pos += 4;

//                if (System.Text.Encoding.Default.GetString(tmp).Trim().Replace("\0", "").Length > 0)
//                {
//                    ibcpartnerlen = Int16.Parse(System.Text.Encoding.Default.GetString(tmp).Trim());

//                    Array.Clear(tmp, 0, tmp.Length);
//                    Array.Copy(tmpStr, pos, tmp, 0, ibcpartnerlen);
//                    AddResultGridView(RGV, "파트너스 정보", tmp);
//                    pos += ibcpartnerlen;
//                }
//            }
//            #endregion
//        }


//        /// <summary>
//        /// Description : 한글을 2byte로 계산하기 위해 아래와같이 변경하여 위치의 byte들을 가져온다.
//        /// Author : 박병규
//        /// Create Date : 2017.09.19
//        /// </summary>
//        static string getWordByByte(string sDat, int Start_len, int Last_len)
//        {
//            //ks_c_5601-1987 변경하면 한글은 2바이트로 인식
//            System.Text.Encoding myEncoding = System.Text.Encoding.GetEncoding("ks_c_5601-1987");

//            byte[] buf = myEncoding.GetBytes(sDat);

//            return myEncoding.GetString(buf, Start_len, Last_len);
//        }



//    }
//}
