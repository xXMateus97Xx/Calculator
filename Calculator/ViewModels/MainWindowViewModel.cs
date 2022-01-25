using Avalonia.Controls;
using Avalonia.Input;
using Calculator.Conversors;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;

namespace Calculator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            _sourceBases = new ObservableCollection<Bases> { Bases.Binary, Bases.Octal, Bases.Decimal, Bases.Hexadecimal };
            _destBases = new ObservableCollection<Bases> { Bases.Binary, Bases.Octal, Bases.Decimal, Bases.Hexadecimal };
            _octalKeysEnabled = true;
            _decimalKeysEnabled = true;
            _hexKeysEnabled = false;
            _source = Bases.Decimal;
            _destiny = Bases.Binary;
        }

        ObservableCollection<Bases> _sourceBases;
        public ObservableCollection<Bases> SourceBases => _sourceBases;

        ObservableCollection<Bases> _destBases;
        public ObservableCollection<Bases> DestBases => _destBases;

        private bool _hexKeysEnabled;
        public bool HexKeysEnabled
        {
            get => _hexKeysEnabled;
            set => this.RaiseAndSetIfChanged(ref _hexKeysEnabled, value);
        }

        private bool _octalKeysEnabled;
        public bool OctalKeysEnabled
        {
            get => _octalKeysEnabled;
            set => this.RaiseAndSetIfChanged(ref _octalKeysEnabled, value);
        }

        private bool _decimalKeysEnabled;
        public bool DecimalKeysEnabled
        {
            get => _decimalKeysEnabled;
            set => this.RaiseAndSetIfChanged(ref _decimalKeysEnabled, value);
        }

        Bases _source;
        public Bases Source
        {
            get => _source;
            set
            {
                if (_source != value)
                    Input = string.Empty;

                this.RaiseAndSetIfChanged(ref _source, value);
                AdjustKeyVisibility();
            }
        }

        Bases _destiny;
        public Bases Destiny
        {
            get => _destiny;
            set
            {
                this.RaiseAndSetIfChanged(ref _destiny, value);
                Calculate();
            }
        }

        string _input;
        public string Input
        {
            get => _input;
            set
            {
                var newValue = value;
                if (newValue == null)
                    newValue = string.Empty;

                newValue = newValue.ToUpper();

                if (!CheckValidInput(newValue))
                    return;

                this.RaiseAndSetIfChanged(ref _input, newValue);
                Calculate();
            }
        }

        string _output;
        public string Output
        {
            get => _output;
            set => this.RaiseAndSetIfChanged(ref _output, value);
        }

        public void OnKeyPressed(object sender, KeyEventArgs args)
        {
            if (!CheckValidInput(args.Key))
            {
                var txt = (TextBox)sender;
                txt.Text = Input;
                Input = Input;
            }
        }

        public void Backspace()
        {
            var input = Input;
            if (string.IsNullOrEmpty(input))
                return;

            Input = input.Substring(0, input.Length - 1);
        }

        public void AddNumber(string number)
        {
            Input += number;
        }

        public void Reset()
        {
            Input = string.Empty;
        }

        bool CheckValidInput(Key key)
        {
            return Source switch
            {
                Bases.Binary => key == Key.NumPad0 || key == Key.NumPad1,
                Bases.Octal => key >= Key.NumPad0 && key <= Key.NumPad7,
                Bases.Decimal => key >= Key.NumPad0 && key <= Key.NumPad9,
                Bases.Hexadecimal => key >= Key.NumPad0 && key <= Key.NumPad9 || key >= Key.A && key <= Key.F,
                _ => false,
            };
        }

        bool CheckValidInput(string newInput)
        {
            if (string.IsNullOrEmpty(newInput))
                return true;

            if (newInput.Length > 37)
                return false;

            return Source switch
            {
                Bases.Binary => newInput.All(x => x == '0' || x == '1'),
                Bases.Octal => newInput.All(x => x >= '0' && x <= '7'),
                Bases.Decimal => newInput.All(x => char.IsDigit(x)),
                Bases.Hexadecimal => newInput.All(x => char.IsDigit(x) || (x >= 'A' && x <= 'F')),
                _ => false,
            };
        }

        void AdjustKeyVisibility()
        {
            switch (Source)
            {
                case Bases.Binary:
                    OctalKeysEnabled = false;
                    DecimalKeysEnabled = false;
                    HexKeysEnabled = false;
                    break;
                case Bases.Octal:
                    OctalKeysEnabled = true;
                    DecimalKeysEnabled = false;
                    HexKeysEnabled = false;
                    break;
                case Bases.Decimal:
                    OctalKeysEnabled = true;
                    DecimalKeysEnabled = true;
                    HexKeysEnabled = false;
                    break;
                case Bases.Hexadecimal:
                    OctalKeysEnabled = true;
                    DecimalKeysEnabled = true;
                    HexKeysEnabled = true;
                    break;
            }
        }

        void Calculate()
        {
            var input = Input;
            if (string.IsNullOrEmpty(input))
            {
                Output = string.Empty;
                return;
            }

            var sourceConversor = Conversor.Create(Source);
            var destinyConversor = Conversor.Create(Destiny);

            var parsedInput = sourceConversor.FromString(input);
            var result = destinyConversor.ToString(parsedInput);

            if (string.IsNullOrEmpty(result))
                result = "0";

            Output = result;
        }
    }
}
