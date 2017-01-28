using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace WurmMesi
{
    /// <summary>
    /// 食べ物の効能一覧
    /// </summary>
    public class FoodEffects : ObservableCollection<FoodEffect>
    {
        //[21:50:24] The potato pasty tastes good. It also seems quite nutritional. You think it might have some salt in it.
        Regex food1 = new Regex("The (?<food>[a-zA-Z ]+?) tastes (?<nut>[a-zA-Z \\.\\!]+?)$");
        Regex food2 = new Regex("You think the (?<food>[a-zA-Z ]+?) might give you more of an insight about (?<eff>[a-zA-Z ]+?)!");
        public bool Parse(string text)
        {
            var m1 = food1.Match(text);
            if (m1.Success)
            {
                var food = m1.Groups["food"].Value;
                var nut = m1.Groups["nut"].Value;
                var model = this.FirstOrDefault(_ => _.FoodName == food);
                if (null == model)
                {
                    model = new FoodEffect();
                    Add(model);
                    model.FoodName = food;
                }
                model.Nutritional = nut;
                model.UpdateTime = DateTime.Now;
                return true;
            }

            var m2 = food2.Match(text);
            if (m2.Success)
            {
                var food = m2.Groups["food"].Value;
                var eff = m2.Groups["eff"].Value;
                var model = this.FirstOrDefault(_ => _.FoodName == food);
                if (null == model)
                {
                    model = new FoodEffect();
                    Add(model);
                    model.FoodName = food;
                }
                model.Effective = eff;
                model.UpdateTime = DateTime.Now;
                return true;
            }

            return false;
        }
    }
}
