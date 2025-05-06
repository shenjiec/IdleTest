using MyWebApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebApp1.Services
{
    public class BattleValueService
    {
        dbGamingEntities _db = new dbGamingEntities();
        private bool _battleFinish;

        public BattleValueService() { }

        public BattleValueService(bool battleFinish)
        {
            _battleFinish = battleFinish;
        }

        public BattleValue CalculateBattleValues(string playerId)
        {
            CalculateService calculateService = new CalculateService();

            BattleValue battleValue = new BattleValue();

            int[] affix = calculateService.GetPlayerAffix(playerId);

            var player = _db.Player.Where(m => m.PlayerId == playerId).FirstOrDefault();

            battleValue.PlayerName = player.PlayerName;
            battleValue.PlayerLevel = player.Level;

            if (_battleFinish)//戰鬥結束並獲得精華 且當前階段+1
            {
                player.CurrentStage = player.CurrentStage + 1 > int.MaxValue ? int.MaxValue : player.CurrentStage + 1;
                player.BestStage = player.CurrentStage > player.BestStage ? player.CurrentStage : player.BestStage;

                Random random = new Random();
                int rd = random.Next(15 + player.CurrentStage / 15, 30 + player.CurrentStage / 12);//精華預設掉落數量隨機
                int essenceAmount = (int)Math.Floor(rd * (1 + affix[10] * 0.01));//水晶額外效益:「精華」掉落數量提升n%

                var db_playerMaterial = _db.Player_Material.Where(m => m.PlayerId == playerId && m.MaterialId == 0).FirstOrDefault();
                if (db_playerMaterial == null)
                {
                    var playerMaterial = new Player_Material();
                    playerMaterial.PlayerId = playerId;
                    playerMaterial.MaterialId = 0;
                    playerMaterial.Amount = essenceAmount;
                    _db.Player_Material.Add(playerMaterial);
                }
                else
                {
                    db_playerMaterial.Amount = db_playerMaterial.Amount + essenceAmount > int.MaxValue ? int.MaxValue : db_playerMaterial.Amount + essenceAmount;
                }
                battleValue.ShowMessage = $"+{essenceAmount} ({rd}+{essenceAmount - rd})\n";
                _db.SaveChanges();
            }

            battleValue.MonsterName = getMonsterName();
            battleValue.CurrentStage = player.CurrentStage + 1;
            //battleValue.StageHP = battleValue.CurrentStage * 10;
            battleValue.StageHP = Math.Floor(10 * battleValue.CurrentStage * (1 + battleValue.CurrentStage / 10 * 0.019d)
                                                                           * (1 + battleValue.CurrentStage / 50 * 0.029d)
                                                                           * (1 + battleValue.CurrentStage / 100 * 0.039d)
                                                                           * (1 + battleValue.CurrentStage / 500 * 0.39d)
                                                                           * (1 + battleValue.CurrentStage / 1000 * 0.99d));

            battleValue.StageDefense1 = battleValue.CurrentStage > 100 ? (battleValue.CurrentStage - 100) / 4 : 0;
            battleValue.StageDefense2 = battleValue.CurrentStage > 100 ? (battleValue.CurrentStage - 100) / 8 : 0;

            battleValue.StageEssence =  $"{15 + battleValue.CurrentStage / 15} ~ {30 + battleValue.CurrentStage / 12 - 1}";//精華掉落數量

            //預計算百分比增益
            double damageMultiplier = (1 + affix[6] * 0.01) * (1 + affix[9] * 0.01);  //計算傷害增益的倍數
            double critMultiplier = 1.5 + affix[8] * 0.01;               //計算暴擊傷害的倍數
            double hitMultiplier = 1 + affix[7] * 0.01;                  //計算命中增益的倍數

            //計算傷害
            battleValue.PlayerDamage = 1 + (long)Math.Floor((affix[0] + affix[3]) * damageMultiplier);

            //計算暴擊傷害
            battleValue.PlayerDamageCrit = (long)Math.Floor(battleValue.PlayerDamage * critMultiplier);

            //計算命中值
            battleValue.PlayerHit = 100 + (long)Math.Floor((affix[1] + affix[4]) * hitMultiplier);

            //計算暴擊值
            battleValue.PlayerCrit = affix[2] + affix[5];

            int[] material = calculateService.GetPlayerMaterial(playerId);

            battleValue.PlayerEssence = material[0];
            battleValue.PlayerCrystal = material[1];

            return battleValue;
        }

        private string getMonsterName()
        {
            string[] firstNames = { "黑", "白", "水", "血", "天", "雷", "火", "炎", "毒", "鋼", "冰", "妖" };

            string[] secondNames = { "牙獸", "龍", "狼", "祭虎", "蛇", "雀", "鳥" };

            Random rand = new Random();

            return firstNames[rand.Next(firstNames.Length)] + secondNames[rand.Next(secondNames.Length)];
        }
    }
}