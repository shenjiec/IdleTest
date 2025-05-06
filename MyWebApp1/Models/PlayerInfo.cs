using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyWebApp1.Models
{
    public class PlayerInfo
    {
        [DisplayName("帳號")]
        public string PlayerId { get; set; }

        [DisplayName("暱稱")]
        public string PlayerName { get; set; }

        [DisplayName("創建日期")]
        public DateTime CreateDate { get; set; }

        [DisplayName("最高階段")]
        public int BestStage { get; set; }

        [DisplayName("當前階段")]
        public int CurrentStage { get; set; }

        [DisplayName("轉生等級")]
        public int PlayerLevel { get; set; }

        [DisplayName("精華碎片")]
        public int PlayerEssence { get; set; }

        [DisplayName("水晶碎片")]
        public int PlayerCrystal { get; set; }

        [DisplayName("攻擊值")]
        public int PlayerAtk1 { get; set; }

        [DisplayName("攻擊值2")]
        public int PlayerAtk2 { get; set; }

        [DisplayName("攻擊值3")]
        public int PlayerAtk3 { get; set; }

        [DisplayName("命中值")]
        public int PlayerHit1 { get; set; }

        [DisplayName("命中值2")]
        public int PlayerHit2 { get; set; }

        [DisplayName("命中值3")]
        public int PlayerHit3 { get; set; }

        [DisplayName("暴擊值")]
        public int PlayerCrit1 { get; set; }

        [DisplayName("暴擊值2")]
        public int PlayerCrit2 { get; set; }

        [DisplayName("暴擊傷害")]
        public int PlayerCritDamage { get; set; }

        [DisplayName("最終傷害")]
        public int PlayerFinalDamage { get; set; }

        [DisplayName("「精華」掉落數量提升")]
        public int PlayerEssenceAmt { get; set; }

        [DisplayName("轉生時「水晶」數量提升")]
        public int PlayerCrystalAmt { get; set; }

        [DisplayName("階段初始值")]
        public int StageInitialValue { get; set; }

        [DisplayName("精華初始值")]
        public int EssenceInitialValue1 { get; set; }

        [DisplayName("精華初始值2")]
        public int EssenceInitialValue2 { get; set; }
    }
}