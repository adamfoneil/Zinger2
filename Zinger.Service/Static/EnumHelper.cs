using System.Collections.ObjectModel;

namespace Zinger.Service.Static
{
    public static class EnumHelper
    {
        public static Dictionary<TEnum, string> ToDictionary<TEnum>() where TEnum : struct, Enum
        {
            var names = Enum.GetNames<TEnum>();
            var values = Enum.GetValues<TEnum>();
            return values.Select((val, index) => new
            {
                Name = names[index],
                Value = val
            }).ToDictionary(item => item.Value, item => item.Name);
        }

        public static ObservableCollection<KeyValuePair<TEnum, string>> ToObservableCollection<TEnum>() where TEnum : struct, Enum
        {
            var dictionary = ToDictionary<TEnum>();
            return new ObservableCollection<KeyValuePair<TEnum, string>>(dictionary);
        }
    }
}
