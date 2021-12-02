namespace ComHpcLibB.Service
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using ComBase.Controls;
    using ComBase.Mvc.Spread;
    using ComHpcLibB.Dto;
    using ComHpcLibB.Repository;

    /// <summary>
    /// 
    /// </summary>
    public class HicCodeService
    {
        private HicCodeRepository hicCodeRepository;

        /// <summary>
        /// 
        /// </summary>
        public HicCodeService()
        {
            this.hicCodeRepository = new HicCodeRepository();
        }

        public List<HIC_CODE> FindOne(string v, string SortColumn = "CODE")
        {
            return hicCodeRepository.FindOne(v, SortColumn);
        }

        public List<HIC_CODE> Hic_Part_Jepsu(string v, string SortColumn = "CODE")
        {
            return hicCodeRepository.Hic_Part_Jepsu(v, SortColumn);
        }

        public HIC_CODE Read_Hic_Code(string strGubun, string strCode)
        {
            return hicCodeRepository.Read_Hic_Code(strGubun, strCode);
        }

        public string Read_Hic_Code2(string strGubun, string strCode)
        {
            return hicCodeRepository.Read_Hic_Code2(strGubun, strCode);
        }

        public SpreadComboBoxData GetListSpdComboBoxData(string argGubun)
        {
            SpreadComboBoxData data = new SpreadComboBoxData();
            List<HIC_CODE> list = hicCodeRepository.GetCodeNamebyGubun(argGubun);
            
            foreach (HIC_CODE code in list)
            {
                data.Put(code.CODE.To<string>("").Trim(), code.NAME.To<string>("").Trim());
            }
            return data;
        }

        public string Read_Hic_Code3(string strGubun, string strCode)
        {
            return hicCodeRepository.Read_Hic_Code3(strGubun, strCode);
        }

        public string Read_Hic_Name2(string strGubun, string strCode)
        {
            return hicCodeRepository.Read_Hic_Name2(strGubun, strCode);
        }

        public string Read_Hic_CodeName(string strCode)
        {
            return hicCodeRepository.Read_Hic_CodeName(strCode);
        }

        public List<HIC_CODE> FindCodeIn(string v, string argSql)
        {
            return hicCodeRepository.FindCodeIn(v, argSql);
        }

        public string Read_HicCode2_GCode(string argGubun, string argCode)
        {
            return hicCodeRepository.Read_HicCode2_GCode(argGubun, argCode);
        }

        public List<HIC_CODE> GetItembyGubun(string strGubun, string idNumber)
        {
            return hicCodeRepository.GetItembyGubun(strGubun, idNumber);
        }

        public List<HIC_CODE> GetCode1(string fstrGubun, string TxtData, string gstrRetValue)
        {
            return hicCodeRepository.GetCode1(fstrGubun, TxtData, gstrRetValue);
        }

        public long GetCodebyGubun(string strGubun)
        {
            return hicCodeRepository.GetCodebyGubun(strGubun);
        }

        public List<HIC_CODE> GetCodeNamebyGubun(string gstrRetValue)
        {
            return hicCodeRepository.GetCodeNamebyGubun(gstrRetValue);
        }

        public string Read_HicCode2_GCodeNew1(string argGubun, string argCode1, string argCode2)
        {
            return hicCodeRepository.Read_HicCode2_GCodeNew1(argGubun, argCode1, argCode2);
        }

        public List<HIC_CODE> Read_Combo_HisDoctor()
        {
            return hicCodeRepository.Read_Combo_HisDoctor();
        }

        public string Read_Hic_Jibung(string argGbn, string argPan, string argSogen1, string argSogen2)
        {
            return hicCodeRepository.Read_Hic_Jibung(argGbn, argPan, argSogen1, argSogen2);
        }

        public List<HIC_CODE> GetItembyGubun(string fstrJong)
        {
            return hicCodeRepository.GetItembyGubun(fstrJong);
        }

        public int UpdateGCode2byRowId(string fstrROWID, string strBun)
        {
            return hicCodeRepository.UpdateGCode2byRowId(fstrROWID, strBun);
        }

        public HIC_SCODE Read_SCode2_GCode(string argCode)
        {
            return hicCodeRepository.Read_SCode2_GCode(argCode);
        }

        public List<HIC_CODE> GetItembyGubunName(string strJong, string strName)
        {
            return hicCodeRepository.GetItembyGubunName(strJong, strName);
        }

        public int InsertCode(string strGubun, string strCODE, string strName, string idNumber)
        {
            return hicCodeRepository.InsertCode(strGubun, strCODE, strName, idNumber);
        }

        public string Read_JisaCode(string argJisaName)
        {
            return hicCodeRepository.Read_JisaCode(argJisaName);
        }

        public SpreadComboBoxData GetSpreadComboBoxData(string argGubun)
        {
            SpreadComboBoxData data = new SpreadComboBoxData();
            List<HIC_CODE> list = hicCodeRepository.GetListByGubun(argGubun);

            foreach (HIC_CODE code in list)
            {
                data.Put(code.CODE, code.NAME);
            }
            return data;
        }

        public string GetMaxCodebyGubun(string fstrJong)
        {
            return hicCodeRepository.GetMaxCodebyGubun(fstrJong);
        }

        public List<HIC_CODE> Read_Hic_Code_All(string v, List<string> argSQL)
        {
            return hicCodeRepository.Read_Hic_Code_All(v, argSQL);
        }

        public List<HIC_CODE> GetListByCode(string argGbn, string strCode)
        {
            return hicCodeRepository.GetListByCode(argGbn, strCode);
        }

        public int DeleteCode(string strROWID)
        {
            return hicCodeRepository.DeleteCode(strROWID);
        }

        public List<HIC_CODE> AutoCompleteText(string fstrGubun)
        {
            return hicCodeRepository.AutoCompleteText(fstrGubun);
        }

        public List<HIC_CODE> GetListByCodeName(string strGubun, string strName = "")
        {
            return hicCodeRepository.GetListByCodeName(strGubun, strName);
        }

        public List<HIC_CODE> BogunSearch(string TxtBogun)
        {
            return hicCodeRepository.BogunSearch(TxtBogun);
        }

        public int UpdateCodeName(string strCODE, string strName, string strROWID)
        {
            return hicCodeRepository.UpdateCodeName(strCODE, strName, strROWID);
        }

        public List<HIC_CODE> GetItemByLikeAll(string argCode, string argKeyWard)
        {
            return hicCodeRepository.GetItemByLikeAll(argCode, argKeyWard);
        }

        public int UpdateSortbyRowId(string cODE, string rOWID)
        {
            return hicCodeRepository.UpdateSortbyRowId(cODE, rOWID);
        }

        public string GetNameByGubunCode(string v1, string v2)
        {
            return hicCodeRepository.GetNameByGubunCode(v1, v2);
        }

        public List<HIC_CODE> GetCodeGubunbyGubun(string strGubun)
        {
            return hicCodeRepository.GetCodeGubunbyGubun(strGubun);
        }

        public string GetNameByGubunGcode1(string argGubun, string argGcode1)
        {
            return hicCodeRepository.GetNameByGubunGcode1(argGubun, argGcode1);
        }

        public int Insert(string v1, string v2, string strName, string v3, string strGCode)
        {
            return hicCodeRepository.Insert(v1, v2, strName, v3, strGCode);
        }

        public int Delete(string strROWID)
        {
            return hicCodeRepository.Delete(strROWID);
        }

        public List<HIC_CODE> GetGrpByNameGcode1ByGubun(string argGubun)
        {
            return hicCodeRepository.GetGrpByNameGcode1ByGubun(argGubun);
        }

        public int Update(string strName, string strGCode, string strROWID)
        {
            return hicCodeRepository.Update(strName, strGCode, strROWID);
        }

        public List<HIC_CODE> GetCodeNamebyGubunNameGcode(string strGubun, string strName, string strstrRetValue)
        {
            return hicCodeRepository.GetCodeNamebyGubunNameGcode(strGubun, strName, strstrRetValue);
        }

        public HIC_CODE GetGCode2byGubunCode(string argGbn, string argCode)
        {
            return hicCodeRepository.GetGCode2byGubunCode(argGbn, argCode);
        }

        public HIC_CODE GetItembyGubunCode(string argGbn, List<string> argCode)
        {
            return hicCodeRepository.GetItembyGubunCode(argGbn, argCode);
        }

        public string GetNamebyCode(string strMGubun)
        {
            return hicCodeRepository.GetNamebyCode(strMGubun);
        }

        public List<HIC_CODE> GetCodeGCodebyCode(List<string> strCodes)
        {
            return hicCodeRepository.GetCodeGCodebyCode(strCodes);
        }

        public List<HIC_CODE> GetCodeNamebyCode(string strTempMGubun)
        {
            return hicCodeRepository.GetCodeNamebyCode(strTempMGubun);
        }

        public HIC_CODE GetItembyCode(string strGubun, string strCode)
        {
            return hicCodeRepository.GetItembyCode(strGubun, strCode);
        }

        public List<HIC_CODE> GetCodeNamebyName(string strName)
        {
            return hicCodeRepository.GetCodeNamebyName(strName);
        }

        public HIC_CODE GetItembyGubunCode2(string argGbn, string argCode)
        {
            return hicCodeRepository.GetItembyGubunCode2(argGbn, argCode);
        }

        public HIC_CODE GetItembyGubunGCode(string argGbn, List<string> argCode)
        {
            return hicCodeRepository.GetItembyGubunGCode(argGbn, argCode);
        }

        public List<HIC_CODE> GetListCodebyGubun(string strGubun)
        {
            return hicCodeRepository.GetListCodebyGubun(strGubun);
        }

        public string GetGCode1ByGubunCode(string argGubun, string argCode)
        {
            return hicCodeRepository.GetGCode1ByGubunCode(argGubun, argCode);
        }

        public string GetCodebyName(string argJisa)
        {
            return hicCodeRepository.GetCodebyName(argJisa);
        }

        public string GetGCodebyGubunCode(string argGubun, string argCode)
        {
            return hicCodeRepository.GetGCodebyGubunCode(argGubun, argCode);
        }

        public List<HIC_CODE> GetCodeNameListByGubunCodeIN(string argGubun, List<string> lstCode)
        {
            return hicCodeRepository.GetCodeNameListByGubunCodeIN(argGubun, lstCode);
        }

        public HIC_CODE GetNameGcodebyGubunCode(string argGbn, string argCode)
        {
            return hicCodeRepository.GetNameGcodebyGubunCode(argGbn, argCode);
        }

        public int GetCodeNameGcode(string strOldData)
        {
            return hicCodeRepository.GetCodeNameGcode(strOldData);
        }

        public List<HIC_CODE> GetCodeNameGcode1(string strOldData)
        {
            return hicCodeRepository.GetCodeNameGcode1(strOldData);
        }
    }
}