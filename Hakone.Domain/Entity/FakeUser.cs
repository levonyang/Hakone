using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hakone.Domain
{
    public class FakeUser
    {
        public string UserName { get; set; }
        public int UserId { get; set; }

        public static List<FakeUser> GetList()
        {
            var result = new List<FakeUser>();

            string[] arrStr =
            {
                "苏木", "沐风晚秋", "夏木南生", "橙子姑娘", "云朵上de歌", "fanxin", "小红豆", "Wendy Wong", "印朵朵", "cindy", "DrDrug",
                "selience", "盏盏心灯", "小鱼干", "小虾米", "阿拉丁", "Dido", "青春尾巴", 
                "陳小V","番茄淡汤","猫小咪","西瓜妹妹","旺仔小丸子","peggy"
            };
            var i = 1;
            foreach (var str in arrStr)
            {
                result.Add(new FakeUser { UserId = i, UserName = str });
                i++;
            }

            return result;
        }
    }
}
