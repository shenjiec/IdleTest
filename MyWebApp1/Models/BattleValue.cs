using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyWebApp1.Models
{
    public class BattleValue
    {
        [DisplayName("怪物名稱")]
        public string MonsterName { get; set; }

        [DisplayName("當前階段")]
        public int CurrentStage { get; set; }

        [DisplayName("階段血量")]
        public double StageHP { get; set; }

        [DisplayName("階段迴避")]
        public long StageDefense1 { get; set; }

        [DisplayName("階段抗暴")]
        public long StageDefense2 { get; set; }

        [DisplayName("階段精華")]
        public string StageEssence { get; set; }

        [DisplayName("玩家名稱")]
        public string PlayerName { get; set; }

        [DisplayName("玩家轉生等級")]
        public int PlayerLevel { get; set; }

        [DisplayName("玩家傷害")]
        public long PlayerDamage { get; set; }

        [DisplayName("玩家暴擊傷害")]
        public long PlayerDamageCrit { get; set; }

        [DisplayName("玩家命中")]
        public long PlayerHit { get; set; }

        [DisplayName("玩家暴擊")]
        public long PlayerCrit { get; set; }

        [DisplayName("訊息")]
        public string ShowMessage { get; set; }

        [DisplayName("玩家精華")]
        public int PlayerEssence { get; set; }

        [DisplayName("玩家水晶")]
        public int PlayerCrystal { get; set; }
    }
}