using System;
using System.ComponentModel;

namespace WurmMesi
{
    /// <summary>
    /// 食べ物の効能
    /// </summary>
    public class FoodEffect : INotifyPropertyChanged
    {
        string foodName;
        string nutritional;
        string effective;
        DateTime updateTime;
        string memo;

        public string FoodName
        {
            get { return foodName; }
            set
            {
                if (foodName == value) return;
                foodName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FoodName)));
            }
        }
        public string Nutritional
        {
            get { return nutritional; }
            set
            {
                if (nutritional == value) return;
                nutritional = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nutritional)));
            }
        }

        public string Effective
        {
            get { return effective; }
            set
            {
                if (effective == value) return;
                effective = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Effective)));
            }
        }

        public DateTime UpdateTime
        {
            get { return updateTime; }
            set
            {
                if (updateTime == value) return;
                updateTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UpdateTime)));
            }
        }

        public string Memo
        {
            get { return memo; }
            set
            {
                if (memo == value) return;
                memo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Memo)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
