namespace InterviewXFlowAndrewRomanenko;

class Example2
{
    public class ObservableProperty<T>
    {
        public event Action<T, T> Changed; 
        
        private T _value;
        private readonly bool _isValueType = typeof(T).IsValueType;

        public ObservableProperty()
        {
        }

        public ObservableProperty(T defaultValue)
        {
            _value = defaultValue;
        }

        public static implicit operator T(ObservableProperty<T> value)
        {
            return value.Value;
        }

        public T Value
        {
            get => _value;
            set
            {
                if (_isValueType)
                {
                    if (value.Equals(_value))
                        return;
                }
                else
                {
                    if (value == null && _value == null)
                        return;

                    if (value != null && _value != null )
                    {
                        if (value.Equals(_value))
                            return;
                    }
                }
                
                Changed.Invoke(_value, value);
                _value = value;
            }
        }
    }
    
    public class Player
    {
        public readonly ObservableProperty<float> Health;

        // лучше использовать отдельную логику инициализации параметров игрока из конфига
        public Player(float initialHealth)
        {
            Health = new ObservableProperty<float>(initialHealth);
        }
    }
    
    // Где возможно, лучше всегда использовать композицию, вместо наследования, для простоты оставлю как в примере
    public class ExtProgram : Program
    {
        private const float HealthChangeThreshold = 10;

        // Виджет, отображающий игроку здоровье.
        private static TextView _healthView = new TextView();
        
        public static void ExtMain(string[] args)
        {
            // Вызов кода по созданию игрока.
            Main(args);
            
            player.Health.Changed += OnPlayerHealthChanged;

            // Ударяем игрока.
            HitPlayer();
        }
        
        private static void OnPlayerHealthChanged(float previousHealth, float currentHealth)
        {
            _healthView.Text = player.Health.ToString();
            
            if (previousHealth - currentHealth > HealthChangeThreshold)
                _healthView.Color = Color.Red;
            else
                _healthView.Color = Color.White;
        }
    }
}