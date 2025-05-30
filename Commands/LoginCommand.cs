using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace gp_unisis.Commands
{
    class LoginCommand : RelayCommand
    {
        public LoginCommand() : base(execute)
        {
           
        }

        private static void execute(object parameter)
        {
            if (parameter is object[] parameters && parameters.Length == 2)
            {
                string username = parameters[0] as string;
                string password = parameters[1] as string;

                MessageBox.Show($"Kullanıcı Adı: {username}\nŞifre: {password}");
            }

            MessageBox.Show("rfefe");
        }

    }
}
