namespace AlphaReturns.System.Unit {
    // Enum untuk jenis modifikasi statistik
    public enum StatModType {
        Flat = 100, //Modifikasi nilai flat
        PercentAdd = 200, // Penambahan persentase
        PercentMult = 300, // Perkalian persentase
    }

    //Class untuk merepresentasikan sebuah modifikasi statistik
    public class StatModifier {
        public readonly float Value;
        public readonly StatModType Type;
        public readonly int Order;
        public readonly object Source;

        // Konstruktor untuk inisialisasi modifikasi statistik
        public StatModifier(float value, StatModType type, int order, object source) {
            Value = value;
            Type = type;
            Order = order;
            Source = source;
        }

        // Konstruktor overload untuk modifikasi nilai flat
        public StatModifier(float value, StatModType type) : this(value, type, (int)type, null) { }

        // Konstruktor overload untuk modifikasi dengan urutan tertentu
        public StatModifier(float value, StatModType type, int order) : this(value, type, order, null) { }

        // Konstruktor overload untuk modifikasi dengan sumber tertentu
        public StatModifier(float value, StatModType type, object source) : this(value, type, (int)type, source) { }
    }
}
