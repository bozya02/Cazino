using Cazino.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cazino.Pages
{
    /// <summary>
    /// Interaction logic for CazinoPage.xaml
    /// </summary>
    public partial class CazinoPage : Page
    {
        public User User { get; set; }
        public const int Bet = 10;
        public bool IsSpinning = false;

        public int Slot1;
        public int Slot2;
        public int Slot3;

        public CazinoPage(User user)
        {
            InitializeComponent();

            User = user;
            this.DataContext = this;
        }

        private async void btnSpin_Click(object sender, RoutedEventArgs e)
        {
            if (User.IsBankrupt)
            {
                MessageBox.Show("Вы полный бакнрот, больше здесь вы не играете", "Ты больше не армянин");
                return;
            }
            
            if (User.IsCreditTaking && User.InCreditAttempt > 0)
            {
                if (!IsSpinning)
                    User.InCreditAttempt--;
            }
            else if (User.IsCreditTaking && User.InCreditAttempt == 0)
            {
                MessageBox.Show("Время отдавать долг", "Хи-хи-ха");
                User.Points -= 100;
                tbBalance.Text = User.Points.ToString();

                if (User.Points < 0)
                {
                    User.IsBankrupt = true;
                    MessageBox.Show("Вы полный бакнрот, больше здесь вы не играете", "ЛОХ");
                    return;
                }
                else
                {
                    User.IsCreditTaking = false;
                    User.InCreditAttempt = 10;
                }   
            }
            

            if (User.Points < Bet && !IsSpinning)
            {
                MessageBox.Show("У вас нехватка баланса", "Печаль");
                ChangeCreditVisibility(Visibility.Visible);
                return;
            }

            if (!IsSpinning)
            {
                btnSpin.Content = "Остановить";
                IsSpinning = !IsSpinning;
                User.Points -= Bet;
                tbBalance.Text = User.Points.ToString();
            }
            else
            {
                IsSpinning = false;
                btnSpin.Content = "Крутить";
                return;
            }

            var rnd = new Random();

            await Task.Run(() =>
            {
                for (int i = 0; i < 20 && IsSpinning; i++)
                {
                    Dispatcher.Invoke(() =>
                    {
                        Slot1 = rnd.Next(1, 6);
                        Slot2 = rnd.Next(1, 6);
                        Slot3 = rnd.Next(1, 6);

                        SetImage(ref imgSlot1, Slot1);
                        SetImage(ref imgSlot2, Slot2);
                        SetImage(ref imgSlot3, Slot3);
                    });
                    Thread.Sleep(300);
                }
            });

            IsSpinning = false;
            btnSpin.Content = "Крутить";

            if (Slot1 == Slot2 && Slot2 == Slot3)
            {
                User.Points += Bet * 100;
                MessageBox.Show($"Выигрыш\n+{Bet * 100}", "Ультра победа");
            }
            else if (Slot1 == Slot2 || Slot1 == Slot3 || Slot2 == Slot3)
            {
                User.Points += Bet * 10;
                MessageBox.Show($"Выигрыш\n+{Bet * 10}", "Победа");
            }
            else
            {
                MessageBox.Show("Вы проиграли", "Неудача");
            }

            tbBalance.Text = User.Points.ToString();

            CazinoEntities.GetContext().SaveChanges();
        }

        public void SetImage(ref Image image, int imageNum)
        {
            image.Source = new BitmapImage(new Uri($"/Cazino;component/Resources/image{imageNum}.png", UriKind.Relative));
        }

        private void btnTakeCredit_Click(object sender, RoutedEventArgs e)
        {
            User.Points += 100;
            tbBalance.Text = User.Points.ToString();
            ChangeCreditVisibility(Visibility.Hidden);

            CazinoEntities.GetContext().SaveChanges();
        }

        public void ChangeCreditVisibility(Visibility visibility)
        {
            btnTakeCredit.Visibility = visibility;
        }
    }
}
