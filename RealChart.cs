using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System.Management;


namespace DemoSharp
{
    public partial class RealChart : Form
    {
        private bool Listening = false;//是否没有执行完invoke相关操作
        private bool serialPoartClosing = false;//是否正在关闭串口，执行Application.DoEvents，并阻止再次invoke


        private Queue<double> ch1Queue = new Queue<double>();
        private Queue<double> ch2Queue = new Queue<double>();
        private Queue<double> ch3Queue = new Queue<double>();

        private List<byte> buffer = new List<byte>(4096);

        private string FileName;
        const int ADCMAX = 8388608;// // 800000

        private double[] arr = new double[200];

        const byte ADS1293START = 0x01;
        const byte ADS1293STOP = 0x02;

        public RealChart()
        {
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint |
                           ControlStyles.AllPaintingInWmPaint,
                           true);//开启双缓冲
            this.UpdateStyles();
            InitializeComponent();
            serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceived);  //手动添加事件处理程序，相当于函数声明
            //serialPort1.ReadBufferSize = 900;
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }

        public enum HardwareEnum
        {
            // 硬件
            Win32_Processor, // CPU 处理器
            Win32_PhysicalMemory, // 物理内存条
            Win32_Keyboard, // 键盘
            Win32_PointingDevice, // 点输入设备，包括鼠标。
            Win32_FloppyDrive, // 软盘驱动器
            Win32_DiskDrive, // 硬盘驱动器
            Win32_CDROMDrive, // 光盘驱动器
            Win32_BaseBoard, // 主板
            Win32_BIOS, // BIOS 芯片
            Win32_ParallelPort, // 并口
            Win32_SerialPort, // 串口
            Win32_SerialPortConfiguration, // 串口配置
            Win32_SoundDevice, // 多媒体设置，一般指声卡。
            Win32_SystemSlot, // 主板插槽 (ISA & PCI & AGP)
            Win32_USBController, // USB 控制器
            Win32_NetworkAdapter, // 网络适配器
            Win32_NetworkAdapterConfiguration, // 网络适配器设置
            Win32_Printer, // 打印机
            Win32_PrinterConfiguration, // 打印机设置
            Win32_PrintJob, // 打印机任务
            Win32_TCPIPPrinterPort, // 打印机端口
            Win32_POTSModem, // MODEM
            Win32_POTSModemToSerialPort, // MODEM 端口
            Win32_DesktopMonitor, // 显示器
            Win32_DisplayConfiguration, // 显卡
            Win32_DisplayControllerConfiguration, // 显卡设置
            Win32_VideoController, // 显卡细节。
            Win32_VideoSettings, // 显卡支持的显示模式。

            // 操作系统
            Win32_TimeZone, // 时区
            Win32_SystemDriver, // 驱动程序
            Win32_DiskPartition, // 磁盘分区
            Win32_LogicalDisk, // 逻辑磁盘
            Win32_LogicalDiskToPartition, // 逻辑磁盘所在分区及始末位置。
            Win32_LogicalMemoryConfiguration, // 逻辑内存配置
            Win32_PageFile, // 系统页文件信息
            Win32_PageFileSetting, // 页文件设置
            Win32_BootConfiguration, // 系统启动配置
            Win32_ComputerSystem, // 计算机信息简要
            Win32_OperatingSystem, // 操作系统信息
            Win32_StartupCommand, // 系统自动启动程序
            Win32_Service, // 系统安装的服务
            Win32_Group, // 系统管理组
            Win32_GroupUser, // 系统组帐号
            Win32_UserAccount, // 用户帐号
            Win32_Process, // 系统进程
            Win32_Thread, // 系统线程
            Win32_Share, // 共享
            Win32_NetworkClient, // 已安装的网络客户端
            Win32_NetworkProtocol, // 已安装的网络协议
            Win32_PnPEntity,//all device
        }

        public static string[] MulGetHardwareInfo(HardwareEnum hardType, string propKey)
        {

            List<string> strs = new List<string>();
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + hardType))
                {
                    var hardInfos = searcher.Get();
                    foreach (var hardInfo in hardInfos)
                    {
                        if (hardInfo.Properties[propKey].Value != null)
                        {
                            if (hardInfo.Properties[propKey].Value.ToString().Contains("COM"))
                            {
                                strs.Add(hardInfo.Properties[propKey].Value.ToString());
                            }
                        }
                    }
                    searcher.Dispose();
                }
                return strs.ToArray();
            }
            catch
            {
                return null;
            }
            finally
            { 
                strs = null;
            }
        }

        private void RealChart_Load(object sender, EventArgs e)
        {
            InitChart();
            this.chart1.Series[0].Points.AddXY(0, 0);
            this.chart1.Series[1].Points.AddXY(0, 0);
            this.chart1.Series[2].Points.AddXY(0, 0);
            timer1.Interval = 80;                     //定时器执行间隔时间,单位为毫秒;
                                                      //通过WMI获取COM端口
            string[] SerialPortName = MulGetHardwareInfo(HardwareEnum.Win32_PnPEntity, "Name");
            foreach (string PortName in SerialPortName)
            {
                comboBox1.Items.Add(PortName);
            }
            if (SerialPortName.Length != 0)
                comboBox1.Text = SerialPortName[0];
            comboBox2.Text = "115200";
            
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }
        private void WriteByteToSerialPort(byte data)
        {
            byte[] Buffer = new byte[1] { data };
            if (serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Write(Buffer, 0, 1);
                }
                catch
                {
                    MessageBox.Show("命令发送失败", "提示");
                }
            }
            else
                MessageBox.Show("串口未打开", "提示");
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            this.timer1.Start();
            try
            {
                //string portName = (comboBox1.Text).Substring(comboBox1.Text.Length - 5, 4);
                string portName = comboBox1.Text;
                string[] sArray = portName.Split(new char[2] { '(', ')' });
                string selectCOM = sArray[sArray.Length - 2];

                serialPort1.PortName = selectCOM;
                serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text, 10); //波特率设置，转换为10进制，可不写，16进制必须要写
                serialPort1.Open();                                        //打开串口
                btnStart.Enabled = false;                                   //将“打开串口”按钮除能
                btnStop.Enabled = true;                                    //将“关闭串口”按钮使能
                WriteByteToSerialPort(ADS1293START);
            }
            catch
            {
                MessageBox.Show("端口错误", "错误");
            }
            if (Datasave_Box.Checked)
            {
                string fileName = Application.StartupPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + ".txt";
                FileName = fileName;
            }
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            this.timer1.Stop();
            try
            {
                WriteByteToSerialPort(ADS1293STOP);
                serialPoartClosing = true;
                while (Listening)
                    Application.DoEvents();
                serialPoartClosing = false;
                serialPort1.Close();
                btnStop.Enabled = false;
                btnStart.Enabled = true;
            }
            catch
            {

            }
            if (Datasave_Box.Checked)
            {
                FileStream file = new FileStream(@FileName, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(file);
                sr.Close();
                file.Close();
            }
        }
        private void searchAndAddSerialPortToCombBox(SerialPort MyPort, ComboBox MyBox)
        {
            MyBox.Items.Clear();               //清空下拉列表
            string[] SerialPortName = MulGetHardwareInfo(HardwareEnum.Win32_PnPEntity, "Name");
            foreach (string PortName in SerialPortName)
            {
                MyBox.Items.Add(PortName);
            }
            if (SerialPortName.Length != 0)
                MyBox.Text = SerialPortName[0];
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            searchAndAddSerialPortToCombBox(serialPort1, comboBox1);
        }

        private void InitChart() 
        {
            //定义图表区域
            this.chart1.ChartAreas.Clear();
            ChartArea chartArea1 = new ChartArea("C1");
            ChartArea chartArea2 = new ChartArea("C2");
            ChartArea chartArea3 = new ChartArea("C3");

            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.ChartAreas.Add(chartArea2);
            this.chart1.ChartAreas.Add(chartArea3);


            //定义存储和显示点的容器
            this.chart1.Series.Clear();

            Series series1 = new Series("S1");
            series1.ChartArea = "C1";
            this.chart1.Series.Add(series1);

            Series series2 = new Series("S2");
            series2.ChartArea = "C2";
            this.chart1.Series.Add(series2);

            Series series3 = new Series("S3");
            series3.ChartArea = "C3";
            this.chart1.Series.Add(series3);

            //设置图表显示样式
            this.chart1.ChartAreas[0].AxisY.Minimum = 10;//6.48
            this.chart1.ChartAreas[0].AxisY.Maximum =10.6;//6.52
            this.chart1.ChartAreas[0].AxisX.Minimum = 0;
            this.chart1.ChartAreas[0].AxisX.Maximum = 900;
            this.chart1.ChartAreas[0].AxisX.Interval = 100;
            this.chart1.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;

            this.chart1.ChartAreas[1].AxisY.Minimum = 10;//6.48
            this.chart1.ChartAreas[1].AxisY.Maximum = 10.6;//6.52
            this.chart1.ChartAreas[1].AxisX.Minimum = 0;
            this.chart1.ChartAreas[1].AxisX.Maximum = 900;
            this.chart1.ChartAreas[1].AxisX.Interval = 100;
            this.chart1.ChartAreas[1].AxisX.MajorTickMark.Enabled = false;

            this.chart1.ChartAreas[2].AxisY.Minimum = 10;//6.48
            this.chart1.ChartAreas[2].AxisY.Maximum = 10.6;//6.52
            this.chart1.ChartAreas[2].AxisX.Minimum = 0;
            this.chart1.ChartAreas[2].AxisX.Maximum = 900;
            this.chart1.ChartAreas[2].AxisX.Interval = 100;
            this.chart1.ChartAreas[2].AxisX.MajorTickMark.Enabled = false;

            this.chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.chart1.ChartAreas[1].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.chart1.ChartAreas[1].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.chart1.ChartAreas[2].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.chart1.ChartAreas[2].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            //设置图表显示样式
            this.chart1.Series[0].Color = Color.Green;
            this.chart1.Series[0].ChartType = SeriesChartType.Spline;
            this.chart1.Series[1].Color = Color.Green;
            this.chart1.Series[1].ChartType = SeriesChartType.Spline;
            this.chart1.Series[2].Color = Color.Green;
            this.chart1.Series[2].ChartType = SeriesChartType.Spline;

            //设置X轴滚动条
            this.chart1.Series[0].Points.Clear();
            this.chart1.Series[1].Points.Clear();
            this.chart1.Series[2].Points.Clear();
        }
        private void updateQueueValue(byte[] data)
        {
            double s;
            // 已经限定了data的长度大于零才读取
            int frames = (data.Length - data.Length % 12) / 12; // 数据中的帧数
            int len = data.Length - data.Length % 12;           // 数据减去多余数据后的长度，此变量一定是6的倍数
            int points = (len - frames * 3) / 9;                // 当前数据段中每个通道存在的ECG点数

            // 心电数据队列大于等于600时，数据缓冲队列中的数量大于600*6 = 3600，不可过多，内存一页为4096
            if(ch1Queue.Count >= 800)
            {
                // 当心电数据队列中的数据量大于900时，比较data中的数据量和心电数据队列中的数据量，判断出队列的数量
                if (data.Length > ch1Queue.Count)
                {
                    for (int i = 0; i < ch1Queue.Count; i++)
                    {
                        ch1Queue.Dequeue();
                        ch2Queue.Dequeue();
                        ch3Queue.Dequeue();
                    }
                    this.chart1.ChartAreas[0].AxisX.Minimum += ch1Queue.Count;
                    this.chart1.ChartAreas[0].AxisX.Maximum += ch1Queue.Count;
                    this.chart1.ChartAreas[1].AxisX.Minimum += ch2Queue.Count;
                    this.chart1.ChartAreas[1].AxisX.Maximum += ch2Queue.Count;
                    this.chart1.ChartAreas[2].AxisX.Minimum += ch3Queue.Count;
                    this.chart1.ChartAreas[2].AxisX.Maximum += ch3Queue.Count;
                }
                else
                {
                    for(int i = 0; i < frames; i++)
                    {
                        ch1Queue.Dequeue();
                        ch2Queue.Dequeue();
                        ch3Queue.Dequeue();
                    }
                    this.chart1.ChartAreas[0].AxisX.Minimum += frames;
                    this.chart1.ChartAreas[0].AxisX.Maximum += frames;
                    this.chart1.ChartAreas[1].AxisX.Minimum += frames;
                    this.chart1.ChartAreas[1].AxisX.Maximum += frames;
                    this.chart1.ChartAreas[2].AxisX.Minimum += frames;
                    this.chart1.ChartAreas[2].AxisX.Maximum += frames;
                }
            }
            // 将所有数据入队列
            buffer.AddRange(data);
            while (buffer.Count >= 12)
            {
                // 判断数据是否有帧头，如果没有帧头就移除帧头前面的数据
                for (int i = 0; i < buffer.Count; i++)
                {
                    if (buffer[i] == 0xAA && buffer[i + 1] == 0xBB && buffer[i + 2] == 0xCC)
                        break;
                    else
                        buffer.RemoveAt(0);
                }
                // 找到帧头之后，判断当前缓冲区中的数据量是否多于一帧，如果少于一帧则不处理

                if (buffer[0] == 0xAA && buffer[1] == 0xBB && buffer[2] == 0xCC)
                {
                    // 取出帧头后前三个放入ch1Queue
                    s = ((buffer[3] * Math.Pow(2, 16) + buffer[4] * Math.Pow(2, 8) + buffer[5]) / ADCMAX - 0.5) * 2.4 * 2 / 3.5;
                    ch1Queue.Enqueue(s);

                    // 取出帧头后中间三个放入ch2Queue中
                    s = ((buffer[6] * Math.Pow(2, 16) + buffer[7] * Math.Pow(2, 8) + buffer[8]) / ADCMAX - 0.5) * 2.4 * 2 / 3.5;
                    ch2Queue.Enqueue(s);

                    // 取出帧头后最后三个放入ch3Queue中
                    s = ((buffer[9] * Math.Pow(2, 16) + buffer[10] * Math.Pow(2, 8) + buffer[11]) / ADCMAX - 0.5) * 2.4 * 2 / 3.5;
                    ch3Queue.Enqueue(s);
                }
                buffer.RemoveRange(0, 12);
            }
            AI0_textBox.Text = (data.Length).ToString();
        }


        private void updateQueueValue( byte[] data,string fileName)
        {
            double s;
            // 已经限定了data的长度大于零才读取
            int frames = (data.Length - data.Length % 12) / 12; // 数据中的帧数
            int len = data.Length - data.Length % 12;           // 数据减去多余数据后的长度，此变量一定是6的倍数
            int points = (len - frames * 3) / 9;                // 当前数据段中每个通道存在的ECG点数

            // 心电数据队列大于等于600时，数据缓冲队列中的数量大于600*6 = 3600，不可过多，内存一页为4096
            if (ch1Queue.Count >= 800)
            {
                // 当心电数据队列中的数据量大于900时，比较data中的数据量和心电数据队列中的数据量，判断出队列的数量
                if (data.Length > ch1Queue.Count)
                {
                    for (int i = 0; i < ch1Queue.Count; i++)
                    {
                        ch1Queue.Dequeue();
                        ch2Queue.Dequeue();
                        ch3Queue.Dequeue();
                    }
                    this.chart1.ChartAreas[0].AxisX.Minimum += ch1Queue.Count;
                    this.chart1.ChartAreas[0].AxisX.Maximum += ch1Queue.Count;
                    this.chart1.ChartAreas[1].AxisX.Minimum += ch2Queue.Count;
                    this.chart1.ChartAreas[1].AxisX.Maximum += ch2Queue.Count;
                    this.chart1.ChartAreas[2].AxisX.Minimum += ch3Queue.Count;
                    this.chart1.ChartAreas[2].AxisX.Maximum += ch3Queue.Count;
                }
                else
                {
                    for (int i = 0; i < frames; i++)
                    {
                        ch1Queue.Dequeue();
                        ch2Queue.Dequeue();
                        ch3Queue.Dequeue();
                    }
                    this.chart1.ChartAreas[0].AxisX.Minimum += frames;
                    this.chart1.ChartAreas[0].AxisX.Maximum += frames;
                    this.chart1.ChartAreas[1].AxisX.Minimum += frames;
                    this.chart1.ChartAreas[1].AxisX.Maximum += frames;
                    this.chart1.ChartAreas[2].AxisX.Minimum += frames;
                    this.chart1.ChartAreas[2].AxisX.Maximum += frames;
                }
            }
            // 将所有数据入队列
            buffer.AddRange(data);
            while (buffer.Count >= 12)
            {
                // 判断数据是否有帧头，如果没有帧头就移除帧头前面的数据
                for (int i = 0; i < buffer.Count; i++)
                {
                    if (buffer[i] == 0xAA && buffer[i + 1] == 0xBB && buffer[i + 2] == 0xCC)
                        break;
                    else
                        buffer.RemoveAt(0);
                }
                // 找到帧头之后，判断当前缓冲区中的数据量是否多于一帧，如果少于一帧则不处理

                if (buffer[0] == 0xAA && buffer[1] == 0xBB && buffer[2] == 0xCC)
                {
                    // 取出帧头后前三个放入ch1Queue
                    s = ((buffer[3] * Math.Pow(2, 16) + buffer[4] * Math.Pow(2, 8) + buffer[5]) / ADCMAX - 0.5) * 2.4 * 2 / 3.5;
                    ch1Queue.Enqueue(s);

                    // 取出帧头后中间三个放入ch2Queue中
                    s = ((buffer[6] * Math.Pow(2, 16) + buffer[7] * Math.Pow(2, 8) + buffer[8]) / ADCMAX - 0.5) * 2.4 * 2 / 3.5;
                    ch2Queue.Enqueue(s);

                    // 取出帧头后最后三个放入ch3Queue中
                    s = ((buffer[9] * Math.Pow(2, 16) + buffer[10] * Math.Pow(2, 8) + buffer[11]) / ADCMAX - 0.5) * 2.4 * 2 / 3.5;
                    ch3Queue.Enqueue(s);
                }
                buffer.RemoveRange(0, 12);
            }

            AI0_textBox.Text = (data.Length).ToString();

            FileStream file = new FileStream(@fileName, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(file);
            long fl = file.Length;
            file.Seek(fl, SeekOrigin.Begin);
            for(int i = 0; i < data.Length; i++)
            {
                string str = data[i].ToString();
                sw.Write(str + " ");
            }
            sw.Flush();
            sw.Close();
            file.Close();
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)       //串口数据接收事件
        {
            if (serialPoartClosing)
                return;
            try
            {
                Listening = true;//设置标记，说明我已经开始处理数据，一会儿要使用系统UI的。
                byte[] data = new byte[serialPort1.BytesToRead];
                serialPort1.Read(data, 0, data.Length);//上一步获取了缓冲区中的字节数，这一句就读取缓冲区中的数据到data数组中
                int num1 = data.Length;
                 // 限定从串口中读取到的数据量
                if (num1 > 0 && num1 <= 4096)
                {
                    this.Invoke(new System.Threading.ThreadStart(delegate ()
                    {
                        if (Datasave_Box.Checked)
                        {
                            updateQueueValue(data, FileName);
                        }
                        else
                        {
                            updateQueueValue(data);
                        }
                    
                    }));
                }
            }
            finally
            {
                Listening = false;//我用完了，ui可以关闭串口了。
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.chart1.Series[0].Points.Clear();
            this.chart1.Series[1].Points.Clear();
            this.chart1.Series[2].Points.Clear();

            int j1 = (int)this.chart1.ChartAreas[0].AxisX.Minimum + 1;
            int j2 = (int)this.chart1.ChartAreas[1].AxisX.Minimum + 1;
            int j3 = (int)this.chart1.ChartAreas[2].AxisX.Minimum + 1;


            if (Autoscale_Box.Checked)
            {
                if (ch1Queue.Count > 300)
                {
                    for (int i = 0; i < 200; i++)
                    {
                        arr[i] = ch1Queue.ElementAt(ch1Queue.Count - i - 1);
                    }
                    double ylim_d1 = Math.Round(arr.Min(), 3); // 获取当前队列下界
                    double ylim_u1 = Math.Round(arr.Max(), 3); // 获取当前队列上界
                    double ave1 = Math.Round(arr.Average(), 3); // 获取当前队列平均值
                    this.chart1.ChartAreas[0].AxisY.Maximum = ave1 + (ylim_u1 - ave1) * 1.3;
                    this.chart1.ChartAreas[0].AxisY.Minimum = ave1 - (ave1 - ylim_d1) * 1.3;
                }
                if (ch2Queue.Count > 300)
                {
                    for (int i = 0; i < 200; i++)
                    {
                        arr[i] = ch2Queue.ElementAt(ch2Queue.Count - i - 1);
                    }
                    double ylim_d2 = Math.Round(arr.Min(), 3); // 获取当前队列下界
                    double ylim_u2 = Math.Round(arr.Max(), 3); // 获取当前队列上界
                    double ave2 = Math.Round(arr.Average(), 3); // 获取当前队列平均值
                    this.chart1.ChartAreas[1].AxisY.Maximum = ave2 + (ylim_u2 - ave2) * 1.3;
                    this.chart1.ChartAreas[1].AxisY.Minimum = ave2 - (ave2 - ylim_d2) * 1.3;
                }
                if (ch3Queue.Count > 300)
                {
                    for (int i = 0; i < 200; i++)
                    {
                        arr[i] = ch3Queue.ElementAt(ch3Queue.Count - i - 1);
                    }
                    double ylim_d3 = Math.Round(arr.Min(), 3); // 获取当前队列下界
                    double ylim_u3 = Math.Round(arr.Max(), 3); // 获取当前队列上界
                    double ave3 = Math.Round(arr.Average(), 3); // 获取当前队列平均值
                    this.chart1.ChartAreas[2].AxisY.Maximum = ave3 + (ylim_u3 - ave3) * 1.3;
                    this.chart1.ChartAreas[2].AxisY.Minimum = ave3 - (ave3 - ylim_d3) * 1.3;
                }
            }
            else
            {
                if(Ymax_txtBox1.Text == String.Empty || Ymin_txtBox1.Text == String.Empty)
                {
                    this.chart1.ChartAreas[0].AxisY.Minimum = 10;//6.48
                    this.chart1.ChartAreas[0].AxisY.Maximum = 10.6;//6.52
                }
                else
                {
                    double y_u = double.Parse(Ymax_txtBox1.Text);
                    double y_d = double.Parse(Ymin_txtBox1.Text);
                    this.chart1.ChartAreas[0].AxisY.Maximum = y_u;
                    this.chart1.ChartAreas[0].AxisY.Minimum = y_d;
                }
                if (Ymax_txtBox2.Text == String.Empty || Ymin_txtBox2.Text == String.Empty)
                {
                    this.chart1.ChartAreas[1].AxisY.Minimum = 10;//6.48
                    this.chart1.ChartAreas[1].AxisY.Maximum = 10.6;//6.52
                }
                else
                {
                    double y_u = double.Parse(Ymax_txtBox2.Text);
                    double y_d = double.Parse(Ymin_txtBox2.Text);
                    this.chart1.ChartAreas[1].AxisY.Maximum = y_u;
                    this.chart1.ChartAreas[1].AxisY.Minimum = y_d;
                }
                if (Ymax_txtBox3.Text == String.Empty || Ymin_txtBox3.Text == String.Empty)
                {
                    this.chart1.ChartAreas[2].AxisY.Minimum = 10;//6.48
                    this.chart1.ChartAreas[2].AxisY.Maximum = 10.6;//6.52
                }
                else
                {
                    double y_u = double.Parse(Ymax_txtBox3.Text);
                    double y_d = double.Parse(Ymin_txtBox3.Text);
                    this.chart1.ChartAreas[2].AxisY.Maximum = y_u;
                    this.chart1.ChartAreas[2].AxisY.Minimum = y_d;
                }
            }
            try
            {
                for (int i = 0; i < ch1Queue.Count; i++)
                {
                    this.chart1.Series[0].Points.AddXY(i + j1, ch1Queue.ElementAt(i));
                }
                for (int i = 0; i < ch2Queue.Count; i++)
                {
                    this.chart1.Series[1].Points.AddXY(i + j2, ch2Queue.ElementAt(i));
                }
                for (int i = 0; i < ch3Queue.Count; i++)
                {
                    this.chart1.Series[2].Points.AddXY(i + j3, ch3Queue.ElementAt(i));
                }
                Invalidate();
            }
            catch(Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }
    }
}
