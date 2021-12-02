namespace ComHpcLibB.Repository
{
    using System;
    using System.Collections.Generic;
    using ComBase.Controls;
    using ComBase.Mvc;
    using ComHpcLibB.Model;


    /// <summary>
    /// 
    /// </summary>
    public class HicJepsuExjongPatientRepository : BaseRepository
    {
        
        /// <summary>
        /// 
        /// </summary>
        public HicJepsuExjongPatientRepository()
        {
        }

        public List<HIC_JEPSU_EXJONG_PATIENT> GetItembyJepDateLtdCode(string strFrDate, string strToDate, long nLtdCode, string strTong, string strJob)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT TO_CHAR(a.JepDate,'YYYY/MM/DD') JepDate,a.GjJong,a.SName,a.Sex,a.Age,a.GbSts    ");
            parameter.AppendSql("     , a.WRTNO,a.LtdCode,a.UCodes,b.Name GjName,c.Tel,c.Jumin2                         ");
            parameter.AppendSql("     , a.SECOND_EXAMS,a.SECOND_Sayu,a.Second_MiSayu,a.JONGGUMYN                        ");
            parameter.AppendSql("     , TO_CHAR(a.SECOND_TONGBO,'YYYY/MM/DD') SECOND_TONGBO                             ");
            parameter.AppendSql("     , TO_CHAR(a.SECOND_DATE,'YYYY/MM/DD')   SECOND_DATE                               ");
            parameter.AppendSql("     , a.MailCode,a.Juso1,a.Juso2                                                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_EXJONG b, KOSMOS_PMPA.HIC_PATIENT c    ");
            parameter.AppendSql(" WHERE a.JEPDATE >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                     ");
            parameter.AppendSql("   AND a.JEPDATE <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                     ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                                               ");
            parameter.AppendSql("   AND a.SECOND_FLAG = 'Y'                                                             "); //2�������

            if (strTong == "2")
            {
                parameter.AppendSql("   AND (a.SECOND_Tongbo IS NOT NULL OR a.SECOND_DATE   IS NOT NULL)                "); //�뺸������ '2��������
            }
            else if (strTong == "3")
            {
                parameter.AppendSql("   AND a.SECOND_Tongbo IS NULL                                                     "); //�뺸������
                parameter.AppendSql("   AND a.SECOND_DATE   IS NULL                                                     "); //2��������
            }

            switch (strJob)
            {
                case "1":
                    parameter.AppendSql("   AND a.GjJong IN('11', '14', '23')                                           "); //�����1��,Ư������
                    break;
                case "2":
                    parameter.AppendSql("   AND a.GjJong = '12'                                                         "); //������
                    break;
                case "3":
                    parameter.AppendSql("   AND a.GjJong = '13'                                                         "); //���κ�
                    break;
                case "4":
                    parameter.AppendSql("   AND a.GjJong IN ('21','22','24','30','49')                                  "); //�����1��,Ư������
                    break;
                case "5":
                    parameter.AppendSql("   AND a.GjJong = '69'                                                         "); //�߰�����
                    break;
                default:
                    break;
            }

            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                                        ");
            }
            parameter.AppendSql("   AND a.GjJong = b.Code(+)                                                            ");
            parameter.AppendSql("   AND a.Pano = c.Pano(+)                                                              ");
            parameter.AppendSql(" ORDER BY a.LtdCode,a.SName,a.JepDate Desc ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_EXJONG_PATIENT>(parameter);
        }

        public List<HIC_JEPSU_EXJONG_PATIENT> GetItembyWrtNoJepDateJongLtdCode(string strFrDate, string strToDate, string strJob, string strJong, long nLtdCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.WRTNO,a.SName,TO_CHAR(a.JepDate,'YY-MM-DD') JepDate,a.GjJong  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_EXJONG b               ");
            parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                     ");
            parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                     ");
            parameter.AppendSql("   AND a.DelDate IS NULL                                               ");
            parameter.AppendSql("   AND a.GjYear >='2009'                                               ");
            if (strJob == "1") //�ű�
            {
                parameter.AppendSql("   AND (a.GbMunjin1 IS NULL OR a.GbMunjin1<>'Y')                   ");
            }
            else
            {
                parameter.AppendSql("   AND a.GbMunjin1 = 'Y'                                           ");
            }
            if (nLtdCode != 0)
            {
                parameter.AppendSql("   AND a.LTDCODE = :LTDCODE                                        ");
            }
            if (strJong != "**" && !strJob.IsNullOrEmpty())
            {
                parameter.AppendSql("   AND GJJONG = :GJJONG                                            ");
            }
            parameter.AppendSql("   AND a.GjJong NOT IN ('31','35')                                     "); //�ϰ��� ����
            parameter.AppendSql("   AND a.GjJong = b.Code(+)                                            ");
            parameter.AppendSql("   AND a.GjYear >= '2009'                                              "); //2009�� ���ͻ��
            parameter.AppendSql("   AND b.GbMunjin IN ('1','3','4')                                     "); //�ǰ�����,�ǰ�����+Ư��
            parameter.AppendSql(" ORDER BY a.SName,a.WRTNO                                              ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }
            if (strJong != "**" && !strJong.IsNullOrEmpty())
            {
                parameter.Add("GJJONG", strJong, Oracle.DataAccess.Client.OracleDbType.Char);
            }

            return ExecuteReader<HIC_JEPSU_EXJONG_PATIENT>(parameter);
        }

        public HIC_JEPSU_EXJONG_PATIENT ReadJepMunjinSatus(long argWRTNO)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("SELECT a.GJJONG,a.GBMUNJIN1,a.GBMUNJIN2,a.GBMUNJIN3    ");
            parameter.AppendSql("      ,a.UCODES,a.GBDENTAL,b.GBMUNJIN                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a                         ");
            parameter.AppendSql("      ,KOSMOS_PMPA.HIC_EXJONG b                        ");
            parameter.AppendSql(" WHERE 1 = 1                                           ");
            parameter.AppendSql("   AND a.WRTNO =:WRTNO                                ");
            parameter.AppendSql("   AND a.GJJONG =b.CODE(+)                             ");

            #region Query ��������
            parameter.Add("WRTNO", argWRTNO);
            #endregion

            return ExecuteReaderSingle<HIC_JEPSU_EXJONG_PATIENT>(parameter);
        }

        public List<HIC_JEPSU_EXJONG_PATIENT> GetItembyWrtNoJepDateGjJongLtdCode(string strFrDate, string strToDate, string strJong, long nLtdCode, List<long> str2ChaWrtno)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT TO_CHAR(a.JepDate, 'YYYY-MM-DD') JepDate, a.GjJong, a.SName, a.Sex, a.Age, a.GbSabun                ");
            parameter.AppendSql("     , a.WRTNO,a.LtdCode,a.UCodes,b.Name GjName,c.Tel,c.Jumin2, a.Pano,a.GjYear,a.GjBangi                  ");
            parameter.AppendSql("     , a.SECOND_EXAMS,a.SECOND_Sayu,a.Second_MiSayu,a.GbSTS,a.BuseName                                     ");
            parameter.AppendSql("     , a.UCodes,a.SExams,a.Jisa,a.PTno,a.Kiho,a.GbChul,a.GbInwon,a.Gkiho,a.GJCHASU                         ");
            parameter.AppendSql("     , a.GJYEAR,a.MAILCODE,a.JUSO1,a.JUSO2,a.BURATE,a.JIKGBN                                               ");
            parameter.AppendSql("     , a.JIKJONG,TO_CHAR(a.IPSADATE,'YYYY-MM-DD')IPSADATE ,a.GBSUCHEP, a.GBDENTAL,a.BOGUNSO,a.LIVER2       ");
            parameter.AppendSql("     , a.YOUNGUPSO,a.MILEAGEAM, a.MURYOAM,a.GUMDAESANG,a.MILEAGEAMGBN,a.MURYOGBN , a.REMARK, a.EMAIL       ");
            parameter.AppendSql("     , c.Hphone, TO_CHAR(a.SECOND_TONGBO,'YYYY-MM-DD') SECOND_TONGBO                                       ");
            parameter.AppendSql("     , TO_CHAR(a.SECOND_DATE,'YYYY-MM-DD') SECOND_DATE                                                     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_EXJONG b, KOSMOS_PMPA.HIC_PATIENT c                        ");
            if (str2ChaWrtno.IsNullOrEmpty())
            {
                parameter.AppendSql(" WHERE a.WRTNO IN (:WRTNO)                                                                             ");
            }
            else
            {
                parameter.AppendSql(" WHERE a.JepDate >= TO_DATE(:FRDATE, 'YYYY-MM-DD')                                                     ");
                parameter.AppendSql("   AND a.JepDate <= TO_DATE(:TODATE, 'YYYY-MM-DD')                                                     ");
                parameter.AppendSql("   AND a.DelDate IS NULL                                                                               ");
                parameter.AppendSql("   AND a.SECOND_FLAG = 'Y'                                                                             "); //2�������
                parameter.AppendSql("   AND a.SECOND_MiSayu IS NULL                                                                         "); //�̽ǽû����� ������
                parameter.AppendSql("   AND a.SECOND_DATE IS NULL                                                                           "); //2������ �̽ǽ� ��
                if (strJong != "00" && !strJong.IsNullOrEmpty())
                {
                    parameter.AppendSql("   AND a.GJJONG = :GJJONG                                                                          ");
                }
                if (strJong != "13" && strJong != "43")
                {
                    parameter.AppendSql("   AND a.SECOND_Tongbo IS NOT NULL                                                                 ");
                }
                if (nLtdCode != 0)
                {
                    parameter.AppendSql(" AND a.LTDCODE = :LTDCODE                                                                          ");
                }
            }

            parameter.AppendSql("   AND a.GjJong = b.Code(+)                                                                                ");
            parameter.AppendSql("   AND a.Pano   = c.Pano(+)                                                                                ");
            parameter.AppendSql(" ORDER BY a.LtdCode,a.SName,a.JepDate                                                                      ");

            parameter.Add("FRDATE", strFrDate);
            parameter.Add("TODATE", strToDate);
            if (str2ChaWrtno.IsNullOrEmpty())
            {
                parameter.AddInStatement("WRTNO", str2ChaWrtno);
            }
            if (strJong != "00" && !strJong.IsNullOrEmpty())
            {
                parameter.Add("GJJONG", strJong);
            }
            if (nLtdCode != 0)
            {
                parameter.Add("LTDCODE", nLtdCode);
            }

            return ExecuteReader<HIC_JEPSU_EXJONG_PATIENT>(parameter);
        }

        public string GetGbMunjinbyWrtNo(long argWrtNo)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GBMUNJIN                                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_JEPSU a, KOSMOS_PMPA.HIC_EXJONG b   ");
            parameter.AppendSql(" WHERE a.GJJONG = b.CODE                                   ");
            parameter.AppendSql("   AND a.WRTNO = :WRTNO                                    ");
            parameter.AppendSql("   AND a.DELDATE IS NULL                                   ");

            parameter.Add("WRTNO", argWrtNo);

            return ExecuteScalar<string>(parameter);
        }
    }
}
