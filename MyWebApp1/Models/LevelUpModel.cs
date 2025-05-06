using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyWebApp1.Models
{
    public class LevelUpModel
    {
        [DisplayName("玩家名稱")]
        public string PlayerName { get; set; }

        [DisplayName("玩家轉生等級")]
        public int PlayerLevel { get; set; }

        [DisplayName("當前階段")]
        public int CurrentStage { get; set; }

        [DisplayName("階段初始值")]
        public int StageInitialValue { get; set; }

        [DisplayName("玩家水晶")]
        public int PlayerCrystal { get; set; }

        [DisplayName("水晶變動")]
        public int CrystalVariate { get; set; }

        [DisplayName("水晶變動參數1")]
        public int CrystalVariableParam1 { get; set; }

        [DisplayName("水晶變動參數2")]
        public int CrystalVariableParam2 { get; set; }

        [DisplayName("精華初始值1")]
        public int EssenceInitialValue1 { get; set; }

        [DisplayName("精華初始值2")]
        public int EssenceInitialValue2 { get; set; }
    }
}