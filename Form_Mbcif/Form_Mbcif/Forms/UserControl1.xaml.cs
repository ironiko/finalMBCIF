using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Form_Mbcif.Forms
{
    public partial class UserControl1 : UserControl
    {
        private generagrafo gg;

        public UserControl1(generagrafo gg)
        {
            this.gg = gg;
            this.DataContext = gg;
            InitializeComponent();
            
        }
    }
}
