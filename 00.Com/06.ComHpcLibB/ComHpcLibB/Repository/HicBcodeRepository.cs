namespace ComHpcLibB.Repository
{
    using ComBase.Mvc;
    using ComHpcLibB.Dto;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class HicBcodeRepository :BaseRepository
    {

        /// <summary>
        /// 
        /// </summary>
        public HicBcodeRepository()
        {
        }

        public List<HIC_BCODE> BarCodeAutoPrintSet(string IPADDRESS)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_BCODE           ");
            parameter.AppendSql(" WHERE GUBUN = 'EXAM_바코드인쇄요청PC' ");
            parameter.AppendSql("   AND NAME = :IPADDRESS               ");

            parameter.Add("IPADDRESS", IPADDRESS);

            return ExecuteReader<HIC_BCODE>(parameter);
        }

        public string Order_Insert(long SABUN)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT DRCODE                  ");
            parameter.AppendSql("  FROM KOSMOS_OCS.OCS_DOCTOR   ");
            parameter.AppendSql(" WHERE SABUN = :SABUN          ");

            parameter.Add("SABUN", SABUN, Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            return ExecuteReaderSingle<string>(parameter);
        }

        public List<HIC_BCODE> Read_Code_All(string argGubun, string argCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GUBUN, CODE, NAME       ");
            parameter.AppendSql("     , JDATE, SORT, DELDATE    ");
            parameter.AppendSql("     , ENTSABUN, ENTDATE       ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_BCODE   ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN          ");
            if (argCode != "")
            {
                parameter.AppendSql("   AND CODE  = :CODE       ");
            }

            parameter.Add("GUBUN", argGubun);
            if (argCode != "")
            {
                parameter.Add("CODE", argCode);
            }

            return ExecuteReader<HIC_BCODE>(parameter);
        }

        public int InsertCode(string strGubun, int nMaxCode, string strName)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_BCODE  ");
            parameter.AppendSql("      (GUBUN, CODE, NAME)          ");
            parameter.AppendSql("VALUES                             ");
            parameter.AppendSql("      (:GUBUN, :CODE, :NAME)       ");

            parameter.Add("GUBUN", strGubun);
            parameter.Add("CODE", nMaxCode);
            parameter.Add("NAME", strName);

            return ExecuteNonQuery(parameter);
        }
        public int UpdateCode(string strGubun, int nMaxCode, string strName, string strSort)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("UPDATE KOSMOS_PMPA.HIC_BCODE SET       ");
            parameter.AppendSql("       NAME   = :NAME,                 ");
            parameter.AppendSql("       SORT   = :SORT                  ");
            parameter.AppendSql(" WHERE GUBUN  = :GUBUN                 ");
            parameter.AppendSql(" AND CODE     = :CODE                  ");


            parameter.Add("GUBUN", strGubun);
            parameter.Add("CODE", nMaxCode);
            parameter.Add("NAME", strName);
            parameter.Add("SORT", strSort);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_BCODE> GetItembyGubun(string strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME, SORT, ROWID RID     ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_BCODE           ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                  ");
            parameter.AppendSql(" ORDER BY NVL(SORT, 1)                 ");

            parameter.Add("GUBUN", strGubun);

            return ExecuteReader<HIC_BCODE>(parameter);
        }

        public string GetHeaRowIdbySabun(long gnJobSabun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_BCODE               ");
            parameter.AppendSql(" WHERE GUBUN = 'HEA_종합건진관리자사번'    ");
            parameter.AppendSql("   AND CODE  = :CODE                       ");

            parameter.Add("CODE", gnJobSabun);

            return ExecuteScalar<string>(parameter);
        }

        public int SaveBcode(HIC_BCODE item)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("INSERT INTO KOSMOS_PMPA.HIC_BCODE      ");
            parameter.AppendSql("      (GUBUN, CODE, NAME, SORT)        ");
            parameter.AppendSql("VALUES                                 ");
            parameter.AppendSql("      (:GUBUN, :CODE, :NAME, :SORT)    ");

            parameter.Add("GUBUN", item.GUBUN);
            parameter.Add("CODE", item.CODE);
            parameter.Add("NAME", item.NAME);
            parameter.Add("SORT", item.SORT);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_BCODE> GetRowid(string sabun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID                               ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_BCODE               ");
            parameter.AppendSql(" WHERE GUBUN = 'HIC_전자계산서발급책임자'      ");
            parameter.AppendSql("   AND CODE  = :CODE                       ");

            parameter.Add("CODE", sabun);

            return ExecuteReader<HIC_BCODE>(parameter);
        }

        public int GetMenuSetAuthoritybyIdNumber(string strGubun, string idNumber)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') cnt                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_BCODE               ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                      ");
            parameter.AppendSql("   AND CODE  = :CODE                       ");

            parameter.Add("GUBUN", strGubun);
            parameter.Add("CODE", idNumber);

            return ExecuteScalar<int>(parameter);
        }

        public int GetCountbyGubunCodeName(string strGubun, string strCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') cnt                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_BCODE               ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                      ");
            parameter.AppendSql("   AND CODE  = :CODE                       ");
            parameter.AppendSql("   AND NAME  = 'Y'                         ");

            parameter.Add("GUBUN", strGubun);
            parameter.Add("CODE", strCode);

            return ExecuteScalar<int>(parameter);
        }

        public int GetHeaResultAutoSendPCbyGubun(string strGubun, string strName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') cnt                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_BCODE               ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                      ");
            parameter.AppendSql("   AND NAME  = :NAME                       ");

            parameter.Add("GUBUN", strGubun);
            parameter.Add("NAME", strName);

            return ExecuteScalar<int>(parameter);
        }

        public int DeletebyGubun(string argGubun)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE KOSMOS_PMPA.HIC_BCODE       ");            
            parameter.AppendSql(" WHERE GUBUN = :GUBUN              ");

            parameter.Add("GUBUN", argGubun);

            return ExecuteNonQuery(parameter);
        }

        public int GetCountbyGuybunName(string strGubun, string gstrCOMIP)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_BCODE               ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                      ");
            parameter.AppendSql("   AND NAME  = :NAME                       ");

            parameter.Add("GUBUN", strGubun);
            parameter.Add("NAME", gstrCOMIP); 

            return ExecuteScalar<int>(parameter);
        }

        public int GetCountbyGubunCode(string strGubun, string strCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_BCODE           ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                  ");
            parameter.AppendSql("   AND CODE  = :CODE                   ");

            parameter.Add("GUBUN", strGubun);
            parameter.Add("CODE", strCode);

            return ExecuteScalar<int>(parameter);
        }

        public HIC_BCODE Read_Hic_BCode(string strGubun, string strCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_BCODE           ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                  ");
            parameter.AppendSql("   AND CODE  = :CODE                   ");

            parameter.Add("GUBUN", strGubun);
            parameter.Add("CODE", strCode);

            return ExecuteReaderSingle<HIC_BCODE>(parameter);
        }

        public List<HIC_BCODE> GetCodebyGubun(string strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME                      ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_BCODE           ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                  ");

            parameter.Add("GUBUN", strGubun);

            return ExecuteReader<HIC_BCODE>(parameter);
        }

        public string GetCodeNamebyGubunCode(string strGubun, string strCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NAME                            ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_BCODE           ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                  ");
            parameter.AppendSql("   AND CODE  = :CODE                   ");

            parameter.Add("GUBUN", strGubun);
            parameter.Add("CODE", strCode);

            return ExecuteScalar<string>(parameter);
        }

        public int DeletebyCode(string strGubun, string strCode)
        {
            MParameter parameter = CreateParameter();

            parameter.AppendSql("DELETE FROM  KOSMOS_PMPA.HIC_BCODE     ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                  ");
            parameter.AppendSql("   AND CODE  = :CODE                   ");

            parameter.Add("GUBUN", strGubun);
            parameter.Add("CODE", strCode);

            return ExecuteNonQuery(parameter);
        }

        public List<HIC_BCODE> GetCodeNamebyGubun(string strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE, NAME, SORT                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_BCODE           ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                  ");
            parameter.AppendSql("   AND NAME IS NOT NULL                ");
            parameter.AppendSql(" ORDER By SORT, CODE                   ");

            parameter.Add("GUBUN", strGubun);

            return ExecuteReader<HIC_BCODE>(parameter);
        }

        public int GetMaxCodebyGubun(string strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT MAX(CODE) + 1 CODE                              ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_BCODE                           ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                                  ");

            parameter.Add("GUBUN", strGubun);

            return ExecuteScalar<int>(parameter);
        }

        public long GetCountbyName(string strName, string strGubun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT COUNT('X') CNT                                  ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_BCODE                           ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                                  ");
            parameter.AppendSql("   AND NAME  = :NAME                                   ");

            parameter.Add("NAME", strName);
            parameter.Add("GUBUN", strGubun);

            return ExecuteScalar<long>(parameter);
        }

        public List<HIC_BCODE> GetCodeNamebyBcode(string strGubun, string strName = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GUBUN, CODE, NAME, JDATE, SORT, DELDATE         ");
            parameter.AppendSql("     , ENTSABUN, ENTDATE, ROWID RID                    ");
            parameter.AppendSql("  FROM HIC_BCODE                           ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                                  ");
            if (strName != "")
            {
                parameter.AppendSql("   AND NAME  = :NAME                               ");
            }
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE >= TRUNC(SYSDATE))  ");

            parameter.Add("GUBUN", strGubun);
            if (strName != "")
            {
                parameter.Add("NAME", strName);
            }

            return ExecuteReader<HIC_BCODE>(parameter);
        }

        public List<HIC_BCODE> GetCodeNamebyBcode1(string strGubun, string strCode = "")
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT GUBUN, CODE, NAME, JDATE, SORT, DELDATE         ");
            parameter.AppendSql("     , ENTSABUN, ENTDATE, ROWID RID                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_BCODE                           ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN                                  ");
            if (strCode != "")
            {
                parameter.AppendSql("   AND CODE  = :CODE                               ");
            }
            parameter.AppendSql("   AND (DELDATE IS NULL OR DELDATE >= TRUNC(SYSDATE))  ");

            parameter.Add("GUBUN", strGubun);
            if (strCode != "")
            {
                parameter.Add("CODE", strCode);
            }

            return ExecuteReader<HIC_BCODE>(parameter);
        }

        public string GetRowIdbySabun(long gnJobSabun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_BCODE               ");
            parameter.AppendSql(" WHERE GUBUN = 'HIC_일반건진관리자사번'      ");
            parameter.AppendSql("   AND CODE  = :CODE                       ");

            parameter.Add("CODE", gnJobSabun);

            return ExecuteScalar<string>(parameter);
        }

        public string GetRowIdbySabunJupsu(long gnJobSabun)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT ROWID RID                           ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_BCODE               ");
            parameter.AppendSql(" WHERE GUBUN = 'HIC_접수변경권한'           ");
            parameter.AppendSql("   AND CODE  = :CODE                       ");

            parameter.Add("CODE", gnJobSabun);

            return ExecuteScalar<string>(parameter);
        }

        public string Read_Code_One(string argGubun, string argCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT Name                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_BCODE   ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN          ");
            if (argCode != "")
            {
                parameter.AppendSql("   AND CODE  = :CODE       ");
            }

            parameter.Add("GUBUN", argGubun);
            if (argCode != "")
            {
                parameter.Add("CODE", argCode);
            }

            return ExecuteScalar<string>(parameter);
        }

        /// <summary>
        /// 건진 자료사전 코드명칭 읽기
        /// </summary>
        /// <param name="argGubun"></param>
        /// <param name="argName"></param>
        /// <returns></returns>
        public string Read_Code(string argGubun, string argName)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_BCODE   ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN          ");
            parameter.AppendSql("   AND NAME  = :NAME           ");

            parameter.Add("GUBUN", argGubun);
            parameter.Add("NAME", argName);

            return ExecuteScalar<string>(parameter);
        }

        public string Read_Code2(string argGubun, string argCode)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT NAME                    ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.BAS_BCODE   ");
            parameter.AppendSql(" WHERE GUBUN = :GUBUN          ");
            parameter.AppendSql("   AND CODE  = :CODE           ");

            parameter.Add("GUBUN", argGubun);
            parameter.Add("CODE", argCode);

            return ExecuteScalar<string>(parameter);
        }

        public string Read_Check_Center_Buse(string CODE)
        {
            MParameter parameter = CreateParameter();
            parameter.AppendSql("SELECT CODE                                ");
            parameter.AppendSql("  FROM KOSMOS_PMPA.HIC_BCODE               ");
            parameter.AppendSql(" WHERE GUBUN = 'BAS_건강증진센타부서코드'  ");
            parameter.AppendSql("   AND CODE  = :CODE                       ");

            parameter.Add("CODE", CODE);

            return ExecuteScalar<string>(parameter);
        }
    }
}
