using MyWebApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWebApp1.Services
{
    public class CalculateService
    {
        dbGamingEntities _db = new dbGamingEntities();

        public CalculateService() { }

        public int[] GetPlayerAffix(string playerId)
        {
            //將玩家的Affix等級資訊存入陣列並回傳
            int[] playerAffix = new int[_db.data_Affix.Count()];
            var playerAffixList = _db.Player_Affix.Where(m => m.PlayerId == playerId).ToList();

            foreach (var item in playerAffixList)
            {
                playerAffix[item.AffixId] = item.AffixLv;
            }

            return playerAffix;
        }

        public int[] GetPlayerMaterial(string playerId)
        {
            //將玩家的Material素材數量存入陣列並回傳
            int[] playerMaterial = new int[_db.data_Material.Count()];
            var playerMaterialList = _db.Player_Material.Where(m => m.PlayerId == playerId).ToList();
            foreach (var item in playerMaterialList)
            {
                playerMaterial[item.MaterialId] = item.Amount;
            }
            return playerMaterial;
        }

        public string[] GetMaterialName(int MaterialId)
        {
            var dataMaterial = _db.data_Material.Where(m => m.MaterialId == MaterialId).FirstOrDefault();
            return new string[] { dataMaterial.MaterialName, dataMaterial.MaterialDesc };
        }

        public int CalculateSum(int start, int cost, int terms)
        {
            //等差數列計算總和 start起始 cost等差 tems項數
            double lastTerm = start + (terms - 1) * cost;//計算末項
            double totalSum = (terms / 2.0) * (start + lastTerm);//計算總和

            return totalSum > int.MaxValue ? 0 : Convert.ToInt32(Math.Round(totalSum, 0, MidpointRounding.AwayFromZero));
        }

        public List<Affix> CalculateAffixValue(string PlayerId, int MaterialId)
        {
            //前端呈現Affix的資料

            var affixList = new List<Affix>();
            var dataAffixList = _db.data_Affix.Where(m => m.MaterialId == MaterialId).ToList();

            foreach (var item in dataAffixList)
            {
                Affix affix = new Affix();

                affix.AffixId = item.AffixId;
                affix.AffixName = item.AffixName;
                affix.AffixCost = item.AffixCost;

                var playerAffixList = _db.Player_Affix.Where(m => m.PlayerId == PlayerId && m.AffixId == item.AffixId).FirstOrDefault();
                if (playerAffixList != null)
                {
                    affix.AffixLv = playerAffixList.AffixLv;
                }
                affix.AffixDesc = $"{item.AffixDesc}+{affix.AffixLv.ToString("#,0")}{item.AffixUnit}";

                affix.AffixLvAdd1 = CalculateSum((affix.AffixLv + 1) * affix.AffixCost, affix.AffixCost, 1);
                affix.AffixLvAdd10 = CalculateSum((affix.AffixLv + 1) * affix.AffixCost, affix.AffixCost, 10);
                affix.AffixLvAdd100 = CalculateSum((affix.AffixLv + 1) * affix.AffixCost, affix.AffixCost, 100);

                affix.MaterialId = MaterialId;

                affixList.Add(affix);
            }

            return affixList;
        }

        public string PlayerAffixUpdate(string PlayerId, int AffixId, int AffixLv, int terms)
        {
            //升級Affix

            //查詢data_Affix
            var dataAffix = _db.data_Affix.Where(m => m.AffixId == AffixId).FirstOrDefault();
            //查詢Player_Material 玩家擁有該材料的數量
            var currentPlayerMaterial = _db.Player_Material.Where(m => m.PlayerId == PlayerId && m.MaterialId == dataAffix.MaterialId).FirstOrDefault();
            int material = currentPlayerMaterial != null ? currentPlayerMaterial.Amount : 0;
            //計算升級數量是否足夠
            int totalSum = CalculateSum((AffixLv + 1) * dataAffix.AffixCost, dataAffix.AffixCost, terms);
            if (material < totalSum)
            {
                return "素材不足";
            }
            if (totalSum == 0)
            {
                return "無法升級";
            }

            //素材足夠 扣除素材
            currentPlayerMaterial.Amount -= totalSum;
            //升級Affix
            var currentPlayerAffix = _db.Player_Affix.Where(m => m.PlayerId == PlayerId && m.AffixId == AffixId).FirstOrDefault();

            if (currentPlayerAffix == null)
            {
                //首次升級
                var playerAffix = new Player_Affix();
                playerAffix.PlayerId = PlayerId;
                playerAffix.AffixId = AffixId;
                playerAffix.AffixLv = terms;
                _db.Player_Affix.Add(playerAffix);
            }
            else
            {
                //累加等級
                currentPlayerAffix.AffixLv += terms;
            }
            //存檔
            _db.SaveChanges();

            return "升級成功";
        }

        private int GetTotalCrystal(int CurrentStage, int multiplier)
        {
            //計算轉生可取得的水晶總數
            double result = Math.Floor(CurrentStage * 10 * (CurrentStage / 100) * (1 + multiplier * 0.01));
            return result > int.MaxValue ? int.MaxValue : (int)result;
        }

        public LevelUpModel CalculateLevelUpValue(string PlayerId)
        {
            //前端呈現轉生的資料

            LevelUpModel levelUp = new LevelUpModel();

            var player = _db.Player.Where(m => m.PlayerId == PlayerId).FirstOrDefault();

            levelUp.PlayerName = player.PlayerName;
            levelUp.PlayerLevel = player.Level;
            levelUp.CurrentStage = player.CurrentStage;

            int[] playerAffix = GetPlayerAffix(PlayerId);
            levelUp.StageInitialValue = playerAffix[12];

            int[] playerMaterial = GetPlayerMaterial(PlayerId);
            int totalCrystal = GetTotalCrystal(levelUp.CurrentStage, playerAffix[11]);
            levelUp.PlayerCrystal = playerMaterial[1];
            levelUp.CrystalVariate = playerMaterial[1] + totalCrystal > int.MaxValue ? int.MaxValue : playerMaterial[1] + totalCrystal;

            double temp = levelUp.CurrentStage * 10 * (levelUp.CurrentStage / 100);
            levelUp.CrystalVariableParam1 = temp > int.MaxValue ? int.MaxValue : (int)temp;
            levelUp.CrystalVariableParam2 = totalCrystal - (int)temp;

            levelUp.EssenceInitialValue1 = playerAffix[12] * 10 > int.MaxValue ? int.MaxValue : playerAffix[12] * 10;
            levelUp.EssenceInitialValue2 = playerAffix[13];

            return levelUp;
        }

        public string PlayerLevelUpdate(string PlayerId)
        {
            //轉生

            int[] playerAffix = GetPlayerAffix(PlayerId);

            var player = _db.Player.Where(m => m.PlayerId == PlayerId).FirstOrDefault();

            if (player.CurrentStage < playerAffix[12] + 100)
            {
                return "階段不足，無法轉生";
            }

            //水晶
            int getTotalCrystal = GetTotalCrystal(player.CurrentStage, playerAffix[11]);

            var currentPlayerCrystal = _db.Player_Material.Where(m => m.PlayerId == PlayerId && m.MaterialId == 1).FirstOrDefault();
            if (currentPlayerCrystal == null)
            {
                //首次獲得水晶
                var playerMaterial = new Player_Material();
                playerMaterial.PlayerId = PlayerId;
                playerMaterial.MaterialId = 1;
                playerMaterial.Amount = getTotalCrystal;
                _db.Player_Material.Add(playerMaterial);
            }
            else
            {
                //累加水晶
                currentPlayerCrystal.Amount += getTotalCrystal;
            }

            //階段初始化
            player.CurrentStage = playerAffix[12];
            player.Level = player.Level + 1 > int.MaxValue ? int.MaxValue : player.Level + 1;

            //精華初始化
            var currentPlayerEssence = _db.Player_Material.Where(m => m.PlayerId == PlayerId && m.MaterialId == 0).FirstOrDefault();
            currentPlayerEssence.Amount = playerAffix[12] * 10 * (100 + playerAffix[13]) / 100;

            //精華詞綴初始化
            var currentPlayerAffix = _db.Player_Affix.Where(m => m.PlayerId == PlayerId).ToList();
            foreach (var item in currentPlayerAffix)
            {
                var dataAffix = _db.data_Affix.Where(m => m.AffixId == item.AffixId && m.MaterialId == 0).FirstOrDefault();
                if (dataAffix != null)
                {
                    item.AffixLv = 0;
                }
            }

            //存檔
            _db.SaveChanges();

            return "轉生成功";
        }

        public PlayerInfo DisplayPlayerInfo(string PlayerId)
        {
            //前端呈現玩家資料

            PlayerInfo playerInfo = new PlayerInfo();

            var player = _db.Player.Where(m => m.PlayerId == PlayerId).FirstOrDefault();

            playerInfo.PlayerId = PlayerId;
            playerInfo.PlayerName = player.PlayerName;
            playerInfo.CreateDate = player.CreateDate;
            playerInfo.BestStage = player.BestStage;
            playerInfo.CurrentStage = player.CurrentStage;
            playerInfo.PlayerLevel = player.Level;

            int[] playerMaterial = GetPlayerMaterial(PlayerId);

            playerInfo.PlayerEssence = playerMaterial[0];
            playerInfo.PlayerCrystal = playerMaterial[1];

            int[] playerAffix = GetPlayerAffix(PlayerId);

            playerInfo.PlayerAtk1 = playerAffix[0];
            playerInfo.PlayerAtk2 = playerAffix[3];
            playerInfo.PlayerAtk3 = playerAffix[6];
            playerInfo.PlayerHit1 = playerAffix[1];
            playerInfo.PlayerHit2 = playerAffix[4];
            playerInfo.PlayerHit3 = playerAffix[7];
            playerInfo.PlayerCrit1 = playerAffix[2];
            playerInfo.PlayerCrit2 = playerAffix[5];
            playerInfo.PlayerCritDamage = playerAffix[8];
            playerInfo.PlayerFinalDamage = playerAffix[9];
            playerInfo.PlayerEssenceAmt = playerAffix[10];
            playerInfo.PlayerCrystalAmt = playerAffix[11];
            playerInfo.StageInitialValue = playerAffix[12];
            playerInfo.EssenceInitialValue1 = playerAffix[12] * 10 > int.MaxValue ? int.MaxValue : playerAffix[12] * 10;
            playerInfo.EssenceInitialValue2 = playerAffix[13];

            return playerInfo;
        }

    }
}