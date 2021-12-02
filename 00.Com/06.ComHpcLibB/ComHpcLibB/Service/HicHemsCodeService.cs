using System.Collections.Generic;
using ComHpcLibB.Repository;
using ComHpcLibB.Dto;
using System;
using ComBase;
using ComHpcLibB.Model;

namespace ComHpcLibB.Service
{
    /// <summary>
    /// 주석을 입력해주세요
    /// </summary>
    public class HicHemsCodeService
    {
        private HicHemsCodeRepository hicHemsCodeRepository;
        /// <summary>
        /// 생성자 
        /// </summary>
        public HicHemsCodeService()
        {
            hicHemsCodeRepository = new HicHemsCodeRepository();
        }

        public List<HIC_HEMS_CODE> GetItembyGubunName(string strJong, string strName)
        {
            return hicHemsCodeRepository.GetItembyGubunName(strJong, strName);
        }

        public int Insert(HIC_HEMS_CODE item)
        {
            return hicHemsCodeRepository.Insert(item);
        }

        public int Delete(HIC_HEMS_CODE item)
        {
            return hicHemsCodeRepository.Delete(item);
        }

        public int Update(HIC_HEMS_CODE item)
        {
            return hicHemsCodeRepository.Update(item);
        }

        public List<HIC_HEMS_CODE> GetCode2byGubunCode1(string strGubun, string argCode)
        {
            return hicHemsCodeRepository.GetCode2byGubunCode1(strGubun, argCode);
        }
    }
}
