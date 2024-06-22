using Sender.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Sender
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private RabbitMqService _rabbitMqService;

        public MainWindow()
        {
            InitializeComponent();
            InitializeRabbitMq();
        }

        //创建RabbitMqService实例
        private void InitializeRabbitMq()
        {
            _rabbitMqService = new RabbitMqService("localhost", "myQueue");
        }

        /*点击发送状态*/
        private void sendStatus_click(object sender, RoutedEventArgs e)
        {
            bool isChecked = false;
            string selectedTag=null;
            foreach (UIElement element in radioGroup.Children)
            {
                if (element is RadioButton radioButton && radioButton.IsChecked==true)
                {
                    isChecked = true;
                    selectedTag = radioButton.Tag.ToString();
                    break;
                }
            }
            if (isChecked)
            {
                //发送消息到 RabbitMQ
                _rabbitMqService.SendMessage(new KeyValuePair<string, object>("status", selectedTag));
            }
            else
            {
                MessageBox.Show("请选择状态！");
            }
        }

        /*点击发送图片*/
        private void sendPicture_click(object sender, RoutedEventArgs e)
        {
            string fileNameStr = fileName.Text.Trim();
            if (fileNameStr != "")
            {
                //发送消息到 RabbitMQ
                _rabbitMqService.SendMessage(new KeyValuePair<string, object>("fileName", fileNameStr));
            }
            else
            {
                MessageBox.Show("请输入图片文件名！");
            }
        }


        //在窗口关闭时释放RabbitMQ连接资源。
        protected override void OnClosed(EventArgs e)
        {
            _rabbitMqService.Dispose();
            base.OnClosed(e);
        }
    }
}
