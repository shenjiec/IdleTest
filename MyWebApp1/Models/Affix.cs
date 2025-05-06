using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyWebApp1.Models
{
    public class Affix
    {
        [DisplayName("詞綴編號")]
        public int AffixId { get; set; }

        [DisplayName("名稱")]
        public string AffixName { get; set; }

        [DisplayName("描述")]
        public string AffixDesc { get; set; }

        [DisplayName("花費")]
        public int AffixCost { get; set; }

        [DisplayName("等級")]
        public int AffixLv { get; set; }

        [DisplayName("等級加1")]
        public int AffixLvAdd1 { get; set; }

        [DisplayName("等級加10")]
        public int AffixLvAdd10 { get; set; }

        [DisplayName("等級加100")]
        public int AffixLvAdd100 { get; set; }

        [DisplayName("材料編號")]
        public int MaterialId { get; set; }
    }
}