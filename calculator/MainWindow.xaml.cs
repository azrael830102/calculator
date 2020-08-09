using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace calculator
{
    public partial class MainWindow : Window
    {
        private static RecordObj currentRecord;
        private static string ErrorMsg = "Error!";

        public MainWindow()
        {
            InitializeComponent();
            Query.Visibility = Visibility.Hidden;
        }
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            //operator & operand click event
            if (TxtExpre.Text.Equals(ErrorMsg))
            {
                ClearText(true);
            }
            Button b = (Button)sender;
            TxtExpre.Text += b.Content.ToString();
        }
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            BackSpace();
        }
        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearText(true);
        }

        private void ResultBtn_click(object sender, RoutedEventArgs e)
        {
            ShowResult();
        }
        private void InsertBtn_click(object sender, RoutedEventArgs e)
        {
            ShowResult();
            if (RecordDao.CheckTheRecord(currentRecord))
            {
                System.Windows.MessageBox.Show("This expression already exisits.");
            }
            else
            {
                RecordDao.InsertTheRecord(currentRecord);
            }
        }

        private void BackSpace()
        {
            if (TxtExpre.Text.Length > 0)
            {
                TxtExpre.Text = TxtExpre.Text.Substring(0, TxtExpre.Text.Length - 1);
            }
            if (TxtPos.Text.Count() != 0)
            {
                ClearText(false);
            }
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            btnIns.Focusable = false;
            switch (e.Key)
            {
                case Key.Back:
                    BackSpace();
                    break;
                case Key.Return:
                    ShowResult();
                    break;
                case Key.Escape:
                case Key.Decimal:
                    ClearText(true);
                    break;
                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                    TxtExpre.Text += e.Key.ToString().Replace("NumPad", "");
                    break;
                case Key.Add:
                    TxtExpre.Text += '+';
                    break;
                case Key.Subtract:
                    TxtExpre.Text += '-';
                    break;
                case Key.Multiply:
                    TxtExpre.Text += '*';
                    break;
                case Key.Divide:
                    TxtExpre.Text += '/';
                    break;
            }

        }

        private void QueryBtn_click(object sender, RoutedEventArgs e)
        {
            RefreshTheList();
            Main.Visibility = Visibility.Hidden;
            Query.Visibility = Visibility.Visible;
        }
        private void PreBtn_click(object sender, RoutedEventArgs e)
        {
            Main.Visibility = Visibility.Visible;
            Query.Visibility = Visibility.Hidden;
        }
        private void DeleBtn_click(object sender, RoutedEventArgs e)
        {
            RecordObj selectedRecord = (RecordObj)RecordsView.SelectedItem;
            if (selectedRecord == null)
            {
                System.Windows.MessageBox.Show("Please select a record");
            }
            else
            {
                RecordDao.DeleteTheRecord(selectedRecord);
                RefreshTheList();
            }
        }


        private void RefreshTheList()
        {
            List<RecordObj> records = RecordDao.QueryAllRecord();
            RecordsView.ItemsSource = records;
        }
        private void ShowResult()
        {
            string infixStr = TxtExpre.Text;
            if (ConversionAndCalculation.TestExpression(infixStr))
            {
                string prefixStr = ConversionAndCalculation.DoTransPre(infixStr);
                string[] postfixArr = ConversionAndCalculation.DoTransPos(infixStr);
                string postfixStr = String.Concat(postfixArr);
                int decResult = ConversionAndCalculation.Calculation(postfixArr);
                string binResult = Convert.ToString(decResult, 2);

                currentRecord = new RecordObj(infixStr, prefixStr, postfixStr, decResult.ToString(), binResult);
                TxtPre.Text = currentRecord.Prefix;
                TxtPos.Text = currentRecord.Postfix;
                TxtDec.Text = currentRecord.Deci;
                TxtBin.Text = currentRecord.Bin;
            }
            else
            {
                TxtExpre.Text = ErrorMsg;
            }
        }

        private void ClearText(bool includExpression)
        {
            if (includExpression)
            {
                TxtExpre.Text = "";
            }
            TxtDec.Text = "";
            TxtBin.Text = "";
            TxtPre.Text = "";
            TxtPos.Text = "";
        }


    }
}
